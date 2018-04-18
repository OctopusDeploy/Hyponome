using Newtonsoft.Json;

public class GithubAccessTokenResponse
{
	[JsonPropertyAttribute("access_token")]
	public string AccessToken {get;set;}
	[JsonPropertyAttribute("scope")]
	public string Scope {get;set;}
	[JsonPropertyAttribute("token_type")]
	public string TokenType {get;set;}
}
