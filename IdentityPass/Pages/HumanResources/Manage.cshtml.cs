using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityPass.Authorization;
using IdentityPass.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;
using IdentityPass.DTOs;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using CoreServices.FileServices.Descriptions;

namespace IdentityPass.Pages.HumanResources
{
    [Authorize(Policy = Policies.OnlyAdminHR)]
    public class ManageModel : PageModel
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IFileService fileService;

        public IList<WeatherForecast> WeatherForecast { get; set; }
        public ManageModel(IHttpClientFactory httpClientFactory, IFileService fileService)
        {
            this.httpClientFactory = httpClientFactory;
            this.fileService = fileService;
        }
        public async Task OnGet()
        {
            var userName = User.Identity.Name;
            var password = await fileService.GetValueFromJsonFileByKey<string>(Constants.UsersJsonFilePath
                , Constants.UsersJsonUsernameKey
                , userName
                , Constants.UsersJsonPasswordKey);
            var httpClient = httpClientFactory.CreateClient(Constants.WebAPILogicalName);

            var response = await httpClient.PostAsJsonAsync(Constants.WebAPIAuthUri, new Credential()
            {
                Username = userName,
                Password = password
            });
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var jwtToken = JsonConvert.DeserializeObject<JwtToken>(content);

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Constants.WebAPIJwtBearerScheme
                , jwtToken.AccessToken);
            WeatherForecast = await httpClient.GetFromJsonAsync<IList<WeatherForecast>>("WeatherForecast");
            
        }
    }
}