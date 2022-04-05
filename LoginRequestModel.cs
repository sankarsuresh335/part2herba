using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Automation.Framework
{
    public class LoginRequestModel
    {
        [JsonProperty(PropertyName = "data")]
        public Credentials CredentialsObj { get; set; }



        public class Credentials
        {
            [JsonProperty(PropertyName = "username")]
            public string Username { get; set; }



            [JsonProperty(PropertyName = "password")]
            public string Password { get; set; }



            [JsonProperty(PropertyName = "locale")]
            public string Locale { get; set; }
        }
    }
}
