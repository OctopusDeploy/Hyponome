using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Authentication.Cookies;
using Microsoft.AspNet.Mvc;
using System.Net.Http;
using Newtonsoft.Json;
using Hyponome.Core;

[RouteAttribute("[controller]")]
public class AccountController : Controller
{
	readonly IGithubClientService githubClientService;
	string authenticationScheme = "Github";
	string claimsIssuer = "OAuth2-Github";
	
	public AccountController(IGithubClientService githubClientService)
	{
		this.githubClientService = githubClientService;
	}
	
	[RouteAttribute("[action]")]
	public ActionResult Login()
	{
		Console.WriteLine("[Login] Redirecting to Authorize");
		return RedirectToAction("Authorize", "Account");
	}

	[RouteAttribute("[action]")]
	public ActionResult Authorize()
	{
		githubClientService.GenerateState();
		Console.WriteLine("[Authorize] State generated {0}", githubClientService.CurrentState);
		return Redirect(string.Format(githubClientService.Options.GithubLoginUri + "authorize?client_id={0}&redirect_uri={1}&scope={2}&state={3}",
			githubClientService.Options.GithubOAuthClientId,
			githubClientService.Options.GithubOAuthRedirectUri,
			githubClientService.Options.GithubOAuthScope,
			githubClientService.CurrentState
		));
	}
	
	[RouteAttribute("[action]")]
	public async Task<ActionResult> Authorized(string code, string state)
	{
		Console.WriteLine("[Authorized] Received state {0}", state);
		if (githubClientService.CurrentState == state)
		{
			var content = new FormUrlEncodedContent(
				new List<KeyValuePair<string, string>> {
					new KeyValuePair<string, string>("client_id", githubClientService.Options.GithubOAuthClientId),
					new KeyValuePair<string, string>("client_secret", githubClientService.Options.GithubOAuthClientSecret),
					new KeyValuePair<string, string>("code", code),
					new KeyValuePair<string, string>("redirect_uri",githubClientService.Options.GithubOAuthRedirectUri)
				}
			);
			
			var http = new HttpClient();
			http.DefaultRequestHeaders.Accept.Clear();
			http.DefaultRequestHeaders.Accept.ParseAdd("application/json");
			
			var authResponse = await http.PostAsync(githubClientService.Options.GithubLoginUri + "access_token", content);
			var result = await authResponse.Content.ReadAsStringAsync();
			var responseObject = JsonConvert.DeserializeObject<GithubAccessTokenResponse>(result);
			
			var user = await githubClientService.SetCredentials(responseObject.AccessToken);
			var identity = new ClaimsIdentity(
				"Github", 
				ClaimsIdentity.DefaultNameClaimType, 
				ClaimsIdentity.DefaultRoleClaimType);
				
			identity.AddClaims(new [] {
				new Claim(ClaimTypes.NameIdentifier, user.Id.ToString(), ClaimValueTypes.String, claimsIssuer),
				new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login, ClaimValueTypes.String, claimsIssuer),
				new Claim("urn:github:name", user.Name, ClaimValueTypes.String, claimsIssuer),
				new Claim("urn:github:url", user.HtmlUrl, ClaimValueTypes.String, claimsIssuer),
				new Claim("urn:github:avatar", user.AvatarUrl, ClaimValueTypes.String, claimsIssuer),
				new Claim("urn:github:accessToken", responseObject.AccessToken, ClaimValueTypes.String, claimsIssuer)
			});
			Console.WriteLine("[Authorized] Successfully logged in.");
			Context.Response.SignIn(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
		}
		else
		{
			Console.WriteLine("[Authorized] Failed to log in, states did not match.", githubClientService.CurrentState, state);
			return new HttpUnauthorizedResult();
		}
		return RedirectToAction("Index", "Home");
	}
	
	[RouteAttribute("[action]")]
	public ActionResult Logout()
	{
		if (!string.IsNullOrEmpty(Context.User.Identity.Name))
		{
			Context.Response.SignOut(CookieAuthenticationDefaults.AuthenticationScheme);
			Console.WriteLine("[Logout] {0} logged out.", Context.User.Identity.Name);
		}
		return RedirectToAction("Index", "Home");
	}
}