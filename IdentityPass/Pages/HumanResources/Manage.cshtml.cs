using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityPass.Authorization;
using IdentityPass.Models;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http.Json;
using IdentityPass.DTOs;
using CoreServices.FileServices.Descriptions;
using CoreServices.Authentication.Descriptions;
using PageModel = IdentityPass.Models.PageModel;

namespace IdentityPass.Pages.HumanResources
{
    [Authorize(Policy = Policies.OnlyAdminHR)]
    public class ManageModel : PageModel
    {
        private readonly IFileService fileService;

        public IList<WeatherForecast> WeatherForecast { get; set; }
        public ManageModel(IFileService fileService
            , IHttpClientFactory httpClientFactory
            , IJWTAuthenticationService authenticationService)
            : base(authenticationService, httpClientFactory)
        {
            this.fileService = fileService;
        }
        public async Task OnGet()
        {
            var userName = User.Identity.Name;
            var password = await fileService.GetValueFromJsonFileByKey<string>(Constants.UsersJsonFilePath
                , Constants.UsersJsonUsernameKey
                , userName
                , Constants.UsersJsonPasswordKey);

            var httpClient = await Authenticate(userName, password);
            WeatherForecast = await httpClient.GetFromJsonAsync<IList<WeatherForecast>>(Constants.WebAPIWeatherForecastUri);
            
        }
    }
}