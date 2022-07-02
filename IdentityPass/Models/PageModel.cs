using CoreServices.Authentication.Descriptions;
using CoreServices.Authentication.DTOs;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace IdentityPass.Models
{
    public class PageModel : Microsoft.AspNetCore.Mvc.RazorPages.PageModel
    {
        private readonly IJWTAuthenticationService authenticationService;
        private readonly IHttpClientFactory httpClientFactory;

        public PageModel(IJWTAuthenticationService authenticationService, IHttpClientFactory httpClientFactory)
        {
            this.authenticationService = authenticationService;
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<HttpClient> Authenticate(string userName
                    , string password)
        {
            var jwtTokenString = HttpContext.Session.GetString(Constants.JwtSessionKey);
            JwtToken jwtToken;
            if (string.IsNullOrEmpty(jwtTokenString))
            {
                jwtToken = await Authenticate(Constants.WebAPILogicalName
                    , Constants.WebAPIAuthUri
                    , userName
                    , password);
            }
            else
            {
                jwtToken = JsonConvert.DeserializeObject<JwtToken>(jwtTokenString);
            }

            if (jwtToken is null
                || string.IsNullOrEmpty(jwtToken.AccessToken)
                || DateTime.Parse(jwtToken.ExpiresAt).ToUniversalTime() < DateTime.UtcNow)
            {
                jwtToken = await Authenticate(Constants.WebAPILogicalName
                    , Constants.WebAPIAuthUri
                    , userName
                    , password);
            }
            var httpClient = httpClientFactory.CreateClient(Constants.WebAPILogicalName);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Constants.WebAPIJwtBearerScheme
                , jwtToken.AccessToken);
            return httpClient;
        }
        private async Task<JwtToken> Authenticate(string webAPILogicalName
                    , string webAPIAuthUri
                    , string userName
                    , string password)
        {
            var jwtTokenString = await authenticationService.Authenticate(webAPILogicalName
                    , webAPIAuthUri
                    , userName
                    , password);
            HttpContext.Session.SetString(Constants.JwtSessionKey, jwtTokenString);
            return JsonConvert.DeserializeObject<JwtToken>(jwtTokenString);
        }
    }
}
