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

        public static string PerformRestGetRequestWithApiKey(string url, string apiPath, string apiKey)
        {
            var client = new RestClient(url);               
            var request = new RestRequest(apiPath, Method.GET);
            request.AddHeader(@"Authorization", @"Token token=" + apiKey);
            var response = client.Execute(request);
            if (response.IsSuccessful) { return response.Content; }
            throw new Exception(@"Unsuccessful REST Request");            
        }

        /// <summary>
        /// Perform REST POST request with API key
        /// </summary>
        /// <param name="url">Base URL</param>
        /// <param name="apiPath">API path to be appended to end of URL</param>
        /// <param name="apiKey">API Key</param>
        /// <param name="json">JSON content to send</param>
        /// <returns>Tuple&lt;bool, string&gt; =&gt; Tuple&lt;Request success, JSON response&gt;</returns>
        public static Tuple<bool, string> PerformRestPostRequestWithApiKey(string url, string apiPath, string apiKey, string json)
        {
            var client = new RestClient(url + apiPath);
            var request = new RestRequest(Method.POST);
            request.AddHeader(@"Content-Type", @"application/json");
            request.AddHeader(@"Authorization", @"Token token=" + apiKey);
            request.AddParameter("application/json", json, ParameterType.RequestBody);
            var response = client.Execute(request);
            if (response.IsSuccessful) { return new Tuple<bool, string>(true, response.Content); }
            throw new Exception(@"Unsuccessful REST Request");            
        }
        
        //public static bool PerformRestPutRequestWithApiKeyAndId(string url, string apiPath, string apiKey, string json, string id)
        //{
        //    var client = new RestClient(url + apiPath + @"/" + id);
        //    var request = new RestRequest(Method.PUT);
        //    request.AddHeader(@"Content-Type", @"application/json");
        //    request.AddHeader(@"Authorization", @"Token token=" + apiKey);
        //    request.AddParameter("application/json", json, ParameterType.RequestBody);
        //    var response = client.Execute(request);
        //    if (response.IsSuccessful) { return true; }
        //    throw new Exception(@"Unsuccessful REST Request");
        //}
    }
}
