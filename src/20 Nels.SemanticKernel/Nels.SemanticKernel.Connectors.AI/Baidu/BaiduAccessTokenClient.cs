using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.SemanticKernel;
using Nels.SemanticKernel.Baidu.Core;
using Nels.SemanticKernel.Baidu.Core.Models;

namespace Nels.SemanticKernel.Baidu;

internal class BaiduAccessTokenClient
{
    public async static Task<string> GetAccessToken(BaiduClient baiduClient)
    {
        ////https://cloud.baidu.com/doc/Reference/s/wjwvz1xt2
        string url = $"{baiduClient.Endpoint}{baiduClient.Separator}/oauth/2.0/token";
        StringContent postData =
            new StringContent(
                $"grant_type=client_credentials&client_id={baiduClient.ClientId}&client_secret={baiduClient.ApiKey}",
                Encoding.UTF8, "application/x-www-form-urlencoded");

        HttpClient httpClient = new HttpClient();

        HttpResponseMessage httpResponseMessage = await httpClient.PostAsync(url, postData);
        string result = await httpResponseMessage.Content.ReadAsStringAsync();
        if (!httpResponseMessage.StatusCode.Equals(HttpStatusCode.OK) || string.IsNullOrEmpty(result))
        {
            throw new KernelException($"BadiDu 大模型认证失败：'{result}'");
        }
        AccessTokenResponse accessTokenResponse = JsonSerializer.Deserialize<AccessTokenResponse>(result);
        return accessTokenResponse == null ? "" : accessTokenResponse.access_token;
    }
}