﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RestSharp;
using System.ComponentModel;
using RestSharp.Authenticators;

namespace DataSift.Rest
{
    internal class RestAPIRequest : IRestAPIRequest, IIngestAPIRequest
    {
        private string _userAgent;
        private string _baseUrl;
        private string _apiVersion;
        private HttpBasicAuthenticator _auth;

        internal RestAPIRequest(string username, string apikey, string baseUrl, string apiVersion)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            _auth = new HttpBasicAuthenticator(username, apikey);

            var version = Assembly.GetExecutingAssembly().GetName().Version;
            _userAgent = "DataSift/v" + apiVersion + " Dotnet/v" + version.ToString();

            _baseUrl = baseUrl;
            _apiVersion = apiVersion;

        }

        public RestAPIResponse Request(string endpoint, dynamic parameters = null, RestSharp.Method method = Method.GET)
        {
            RestClient client;
            client = new RestClient(_baseUrl + "v" + _apiVersion);
            client.Authenticator = _auth;
            client.UserAgent = _userAgent;

            var request = new RestRequest(endpoint, method);

            RestAPIResponse result = null;

            if(parameters != null)
            {
                var parsedParams = APIHelpers.ParseParameters(endpoint, parameters);

                if (method == Method.POST || method == Method.PUT)
                {
                    request.RequestFormat = DataFormat.Json;
                    request.AddBody(parsedParams);
                }
                else
                {
                    foreach (var prm in (IDictionary<string, object>)parsedParams)
                    {
                        request.AddParameter(prm.Key, prm.Value, ParameterType.GetOrPost);
                    }
                }
            }


            IRestResponse response = client.Execute(request);

            if(endpoint == "pull")
            {
                result = new PullAPIResponse() { RateLimit = APIHelpers.ParseRateLimitHeaders(response.Headers), StatusCode = response.StatusCode, PullDetails = APIHelpers.ParsePullDetailHeaders(response.Headers) };
                result.Data = APIHelpers.DeserializeResponse(response.Content, ((PullAPIResponse)result).PullDetails.Format);
            }
            else
            {
                result = new RestAPIResponse() { RateLimit = APIHelpers.ParseRateLimitHeaders(response.Headers), StatusCode = response.StatusCode };
                result.Data = APIHelpers.DeserializeResponse(response.Content);
            }
            
            switch((int)response.StatusCode)
            {
                // Ok status codes
                case 200:
                case 201:
                case 202:
                case 204:
                    break;
                
                //Error status codes
                case 400:
                case 401:
                case 403:
                case 404:
                case 405:
                case 409:
                case 413:
                case 416:
                case 500:
                case 503:
                    throw new RestAPIException(result, (APIHelpers.HasAttr(result.Data, "error")) ? result.Data.error : "The request failed, please see the Data & StatusCode properties for more details.");

                case 429:
                    throw new TooManyRequestsException(result, (APIHelpers.HasAttr(result.Data, "error")) ? result.Data.error : "The request failed because you've exceeded your API request limit.");
            }
            
            return result;

        }

        public RestAPIResponse Ingest(string endpoint, dynamic data, Method method = Method.POST)
        {
            RestClient client;
            client = new RestClient(_baseUrl);
            client.Authenticator = _auth;
            client.UserAgent = _userAgent;

            var request = new RestRequest(endpoint, method);

            RestAPIResponse result = null;

            if (data != null)
            {
                request.AddParameter("application/json", APIHelpers.SerializeToJsonLD(data), ParameterType.RequestBody);
            }
        
            IRestResponse response = client.Execute(request);

            result = new RestAPIResponse() { RateLimit = APIHelpers.ParseRateLimitHeaders(response.Headers), StatusCode = response.StatusCode };
            result.Data = APIHelpers.DeserializeResponse(response.Content);
            
            switch ((int)response.StatusCode)
            {
                // Ok status codes
                case 200:
                case 201:
                case 202:
                case 204:
                    break;

                //Error status codes
                case 400:
                case 401:
                case 403:
                case 404:
                case 405:
                case 409:
                case 413:
                case 416:
                case 500:
                case 503:
                    throw new RestAPIException(result, (APIHelpers.HasAttr(result.Data, "error")) ? result.Data.error : "The request failed, please see the Data & StatusCode properties for more details.");

                case 429:
                    throw new TooManyRequestsException(result, (APIHelpers.HasAttr(result.Data, "error")) ? result.Data.error : "The request failed because you've exceeded your API request limit.");
            }

            return result;
        }
    }

}
