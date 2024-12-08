namespace Nels.SemanticKernel.Baidu.Core.Models;

internal sealed class AccessTokenResponse
{
    public string access_token { get; set; }

    public int expires_in { get; set; }

    public string scope { get; set; }

    public string refresh_token { get; set; }

    public string session_secret { get; set; }

    public string session_key { get; set; }
}
