public class GithubOptions
{
	public string GithubComUri {get;set;} = "https://github.com/";
	public string GithubApiUri {get;set;} = "https://api.github.com/";
	public string GithubLoginUri {get;set;} = "https://github.com/login/oauth/";
	public string GithubOAuthApp {get;set;}
	public string GithubOrganization {get;set;}
	public string GithubRepository {get;set;}
	public string GithubOAuthClientId {get;set;}
	public string GithubOAuthClientSecret {get;set;}
	public string GithubOAuthRedirectUri {get;set;}
	public string GithubOAuthScope {get;set;}
}