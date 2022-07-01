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

namespace IdentityPass.Pages.HumanResources
{
    [Authorize(Policy = Policies.OnlyAdminHR)]
    public class ManageModel : PageModel
    {
        private readonly IHttpClientFactory httpClientFactory;

        public IList<WeatherForecast> WeatherForecast { get; set; }
        public ManageModel(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        public async Task OnGet()
        {
            var httpClient = httpClientFactory.CreateClient(Constants.WebAPILogicalName);
            //var response = await httpClient.GetAsync("WeatherForecast");
            //var data = await response.Content.ReadAsStringAsync();
            //WeatherForecast = JsonConvert.DeserializeObject<IList<WeatherForecast>>(data);
            try
            {
                WeatherForecast = await httpClient.GetFromJsonAsync<IList<WeatherForecast>>("WeatherForecast");

            }
            catch (Exception ex)
            {
                var testA = ex;
                throw;
            }
            var test = WeatherForecast;
        }
    }
}