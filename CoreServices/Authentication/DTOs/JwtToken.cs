using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreServices.Authentication.DTOs
{
    public class JwtToken
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        [JsonProperty("expires_at")]
        public string ExpiresAt { get; set; }
    }
}
