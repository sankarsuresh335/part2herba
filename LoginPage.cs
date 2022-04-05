using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Shop.Automation.Framework;

namespace Shop.Automation.Pages.Login
{
    public class LoginPage : Page
    {
        [FindsBy(How = How.Id, Using = "Username")]
        public IWebElement UserName { get; set; }

        [FindsBy(How = How.Id, Using = "Password")]
        public IWebElement Password { get; set; }

        [FindsBy(How = How.Id, Using = "submit")]
        public IWebElement LoginButton { get; set; }

        [FindsBy(How = How.Id, Using = "Username-error")]
        public IWebElement BadUserNameError { get; set; }

        [FindsBy(How = How.Id, Using = "Password-error")]
        public IWebElement BadPasswordError { get; set; }

        [FindsBy(How = How.ClassName, Using = "field-validation-error")]
        public IWebElement BadCredentialsError { get; set; }

        public override string Path => RunSettings.Url.ToString().Replace("www","accounts") + "/?appId=1&locale=" + 
            RunSettings.Locale + "&redirect=" + RunSettings.Url; 

        public new string RelativePath;

        public LoginPage Login(Credentials credentials)
        {
            this.Authenticate(credentials);
            return this;
        }
    }
}