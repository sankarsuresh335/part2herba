using Newtonsoft.Json;
using NUnit.Framework;
using OpenQA.Selenium;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Automation.Framework
{

    public class LoginApi
    {
        public static IRestResponse GetAuthToken(string ds, string password, string locale, Uri baseUrl)
        {
            RestClient restClient = new RestClient(baseUrl);
            restClient.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            var restRequest = new RestRequest("api/auth", Method.POST);

            LoginRequestModel logReq = new LoginRequestModel()
            {
                CredentialsObj = new LoginRequestModel.Credentials()
                {
                    Username = ds,
                    Password = password,
                    Locale = locale
                }
            };

            restRequest.AddHeader("content-type", "application/json");
            restRequest.AddHeader("X-HLLOCALE", locale);
            restRequest.AddHeader("X-HLCLIENTAPP", "shop");
            restRequest.AddHeader("X-HLCLIENTDEVICE", "Samsung");
            restRequest.AddHeader("X-HLCLIENTOS", "iOS 8.1");
            restRequest.AddHeader("X-HLBUILD", "1.1.1");
            restRequest.AddHeader("X-HLAPPID", "1");
            restRequest.RequestFormat = DataFormat.Json;
            restRequest.AddJsonBody(JsonConvert.SerializeObject(logReq));
            IRestResponse restResponse = restClient.Execute(restRequest);
            //Assert.AreEqual(HttpStatusCode.OK, restResponse.StatusCode, "Login failed response status in not 200");
            //dynamic obj = JsonConvert.DeserializeObject(restResponse.Content);
            //Assert.IsTrue(obj.data.token != null, "Login token in null");
            //Assert.IsTrue(obj.data.ASPXAUTH != null, "Login token in null");
            //test.Aspauth = obj.data.ASPXAUTH;
            //return obj.data.token;

            return restResponse;
        }
    }
}
