using System;
using RestSharp;
using RestSharp.Authenticators;

namespace MDPMS.Helper.Rest
{
    public static class RestHelper
    {
        public static string PerformRestGetRequestWithHttpBasicAuth(string url, string apiPath, string username, string password)
        {
            var client = new RestClient(url) { Authenticator = new HttpBasicAuthenticator(username, password) };
            var request = new RestRequest(apiPath, Method.GET);
            var response = client.Execute(request);            
            if (response.IsSuccessful) { return response.Content; }
            throw new Exception(@"Unsuccessful REST Request");            
        }
    }
}
