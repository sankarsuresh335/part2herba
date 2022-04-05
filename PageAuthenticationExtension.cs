using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using OpenQA.Selenium;
using Shop.Automation.Framework.Mapping;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;

namespace Shop.Automation.Framework
{
    public static class PageAuthenticationExtension
    {
        public static void Authenticate(this Page page, Credentials credentials, string returnUrl = null)
        {
            page.WaitForElement(CommonFindBy.LoginUserName);
            page.Single(CommonFindBy.LoginUserName).SendKeys(credentials.Username);
            page.Single(CommonFindBy.LoginPassword).SendKeys(credentials.Password);
            page.ClickElementJs(CommonFindBy.LoginSubmit);
        }

        public static void EnsureAuthenticated(this Page page, Credentials credentials, string returnUrl = null)
        {
            if (page.RunSettings.Url.ToString() != "https://zus2prs-accounts.myherbalife.com/?appId=1&locale=en-US&redirect=https://zus2prs.myherbalife.com/")
            {
                page.WaitForElement(CommonFindBy.LandPageForward);
            }
            //page.WaitForElement(CommonFindBy.LandPageForward);
            try
            {
                if (WebElementExtension.IsWebElementDisplayed(page.Driver.FindElement(CommonFindBy.CookiesAccepterButon)))
                {
                    page.WaitForElementClicklable(page.Single(CommonFindBy.CookiesAccepterButon), 5).Click();
                    page.WaitForElementGone(page.Single(CommonFindBy.CookiesAccepterButon));
                }
            }
            catch (Exception e) {
                Trace.WriteLine("it looks that the cookies messega is not displayed");
            }
            if (page.RunSettings.Url.ToString() != "https://zus2prs-accounts.myherbalife.com/?appId=1&locale=en-US&redirect=https://zus2prs.myherbalife.com/")
            {
                page.Single(CommonFindBy.LandPageForward).Click();
            }
            //page.Single(CommonFindBy.LandPageForward).Click();
            Authenticate(page, credentials, returnUrl);
			if (page.WaitForElement(CommonFindBy.AccountIcon) || page.WaitForElementPresentAndVisible(CommonFindBy.ViewPos))
			{ }
			else {

                throw new AssertFailedException("Authentication failed");
            }
        }

        /// <summary>
        /// Authenticates user and goes to a destination URL
        /// </summary>
        /// <param name="page">The page under test</param>
        /// <param name="path">When supplied, goes to that url immediately after authentication. If not supplied, <paramref name="page"/>.RelativeUrl will be used.</param>
        /// <param name="credentials">Credentials of user. If not supplied, run settings will be used to determine user.</param>
        public static void GoToUrlAuthenticated(this Page page, string path = null, Credentials credentials = null, IDictionary<string, string> SetCookies = null, string locale = null)
        {
            HandlePopup(page, SetCookies);
            if (NUnit.Framework.TestContext.Parameters["TestRunEnvironment"] != "Prod" && NUnit.Framework.TestContext.Parameters["TestRunEnvironment"] != "Green" 
                && NUnit.Framework.TestContext.Parameters["TestRunEnvironment"] != "Black")
            {
                page.SelectProject(true);
            }
            if (page.RunSettings.Locale !="de-DE-MB")
            {
                page.EnsureAuthenticated(credentials ?? page.RunSettings.ValidUser, path ?? page.RelativePath);
            }
        }
        public static void Login(this Page page, string path = null, Credentials credentials = null, IDictionary<string, string> SetCookies = null, string locale = null,int retry = 0)
        {
            Uri baseUrl = page.RunSettings.FarmUrl; //"https://zus2prs.myherbalife.com/";
            string Aspauth=string.Empty;
            string Token = string.Empty;
            // SSO Login                
            if (retry < 3)
            {
                // Direct login with API
                Guid guidObj = Guid.NewGuid();
                try
                {
                    for (int i = 0; i <= 2; i++)
                    {
                        var restResponse = LoginApi.GetAuthToken(credentials.Username, credentials.Password, locale, baseUrl);
                        if (restResponse.StatusCode != HttpStatusCode.OK)
                        {

                            continue;
                        }
                        else
                        {
                            dynamic obj = JsonConvert.DeserializeObject(restResponse.Content);
                            if (obj.data.token == null || obj.data.ASPXAUTH == null)
                            {
                                continue;
                            }
                            else
                            {
                                Aspauth = obj.data.ASPXAUTH;
                                Token = obj.data.token;
                                break;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Assert.Fail("Response doesn't retruns token", ex.Message);
                }
                string Url_Redirect = "https://zus2prs.myherbalife.com/"+page.RunSettings.Locale+"/Shop/Cart/Home/Index/" + page.RunSettings.ValidUser.Schema + "/RSO";                                
                page.Driver.Manage().Cookies.AddCookie(new OpenQA.Selenium.Cookie("RENDERING_LOCALE", locale.Replace("-", "_")));
                page.Driver.Manage().Cookies.AddCookie(new OpenQA.Selenium.Cookie("locale", locale.Replace("-", "_")));
                page.Driver.Manage().Cookies.AddCookie(new OpenQA.Selenium.Cookie("MyHL_SSO", "AccessToken=" + Token));
                page.Driver.Manage().Cookies.AddCookie(new OpenQA.Selenium.Cookie(".ASPXAUTH", Aspauth));
                page.Driver.Manage().Cookies.AddCookie(new OpenQA.Selenium.Cookie("Hlf_billing", guidObj + "," + Environment.UserName + ",101924"));
                HandlePopup(page, SetCookies);
                page.Driver.Navigate().GoToUrl(Url_Redirect);
                page.WaitForPageLoad();
                page.SelectProject(true);
                page.WaitForPageLoad();
                page.Driver.Navigate().GoToUrl(Url_Redirect);
                page.WaitForPageLoad();
            }
        }
        public static void GoToAdminUrl(this Page page, string path = null, Credentials credentials = null)
        {
            page.LoginToAdminUrl(credentials ?? page.RunSettings.ValidUser, path ?? page.RelativePath);
        }

        public static void GoToAdminUrl(this Page page, CartUser cartUser = null)
        {
            page.GoToUrl(page.RunSettings.AdminUrl.ToString());
            page.LoginToAdminUrl(cartUser ?? page.RunSettings.CartDS, page.RunSettings.AdminUrl.ToString());

        }
        public static void GoToAdminUrlMB(this Page page, CartUser cartUser = null)
        {
            page.GoToUrl(page.RunSettings.AdminUrl.ToString());
            page.LoginToAdminUrl(cartUser ?? page.RunSettings.CartMB, page.RunSettings.AdminUrl.ToString());

        }

        public static void GoToAdminUrl(this Page page, string path = null, Credentials credentials = null, string locale = null, IDictionary<string, string> SetCookies = null)
        {
            page.LoginToAdminUrl(credentials ?? page.RunSettings.ValidUser, path ?? page.Path, SetCookies, locale);
        }

        public static void LoginToAdminUrl(this Page page, Credentials credentials, string returnUrl = null, IDictionary<string, string> SetCookies = null, string locale = null)
        {
            page.WaitForElement(CommonFindBy.AdminLoginButton);
            try
            {
                if (page.IsElementDisplayed(CommonFindBy.Agreecheckbox))
                {
                    page.ClickElementJs(CommonFindBy.Agreecheckbox);
                    page.ClickElementJs(CommonFindBy.AgreeContinue);
                }
            }
            catch
            {

            }
            page.Single(CommonFindBy.AdminDsIdField).SendKeys(credentials.Password);
            page.Single(CommonFindBy.AdminDsIdField).SendKeys(Keys.Tab);
            if (!locale.Contains("en-US"))
            {
                page.SelectDropDownOption(CommonFindBy.SelectCountry, locale);
            }
            //page.SelectDropDownOption(CommonFindBy.SelectCountry, locale);
            page.Single(CommonFindBy.AdminLoginButton).Click();
            //((IJavaScriptExecutor)page.Driver).ExecuteScript("return document.readyState").Equals("complete")
            PageExtension.WaitForTimeSpan(3000);
            page.Driver.WaitForDocumentReady();
            page.GetNewWindow();
            page.Driver.WaitForDocumentReady();
            PageExtension.WaitForTimeSpan(1000);
            HandlePopup(page, SetCookies);
            if (NUnit.Framework.TestContext.Parameters["TestRunEnvironment"] != "Prod" && NUnit.Framework.TestContext.Parameters["TestRunEnvironment"] != "Green"
                && NUnit.Framework.TestContext.Parameters["TestRunEnvironment"] != "Black")
            {
                page.SelectProject(false);
            }            
            PageExtension.WaitForTimeSpan(3000);
            if (page.WaitForElement(CommonFindBy.AccountIcon) || page.WaitForElementPresentAndVisible(CommonFindBy.ViewPos))
            { }
            else
            {
                throw new AssertFailedException("Authentication failed");
            }
        }

        public static void LoginToAdminUrl(this Page page, Credentials credentials, string returnUrl = null)
        {
            try
            {
                if(WebElementExtension.IsWebElementDisplayed(page.Driver.FindElement(CommonFindBy.PopUpPolicyAcceptance)))
                {
                    page.WaitForElementClicklable(page.Driver.FindElement(CommonFindBy.AgreeCheckBox)).Click();
                    page.WaitForElementClicklable(page.Driver.FindElement(CommonFindBy.ContinueButton)).Click();
                }
            }catch(Exception e)
            {
                Trace.WriteLine("Element is not displaying");
            }

            if (!page.Driver.Url.Contains("MyHLUtils"))
            {
                page.WaitForElement(CommonFindBy.OnlineLoginLnk);
                page.Single(CommonFindBy.OnlineLoginLnk).Click();
            }
            
            page.WaitForElement(CommonFindBy.AdminLoginButton);

            if (page.Driver.Url.Contains("qa4"))
            {
                page.Single(CommonFindBy.AdminDsIdField).SendKeys(credentials.DSID);
                page.SelectAdmCountry(page.RunSettings.Locale);
                page.Single(CommonFindBy.AdminLoginButton).Click();
            }
            else
            {
                page.WaitForElement(CommonFindBy.AdminDsIdComboBox);
                WebElementExtension.SelectDropDownOption(page.Single(CommonFindBy.AdminDsIdComboBox),credentials.Username);
                page.Single(CommonFindBy.AdminLoginButton).Click();
            }
            //((IJavaScriptExecutor)page.Driver).ExecuteScript("return document.readyState").Equals("complete")
            //PageExtension.WaitForTimeSpan(5000);
            page.Driver.WaitForDocumentReady();
            page.GetNewWindow();
            if (NUnit.Framework.TestContext.Parameters["TestRunEnvironment"] != "Prod" && NUnit.Framework.TestContext.Parameters["TestRunEnvironment"] != "Green"
                && NUnit.Framework.TestContext.Parameters["TestRunEnvironment"] != "Black")
            {
                page.SelectProject(false);
            }
            //page.Driver.WaitForDocumentReady();
            if (page.WaitForElement(CommonFindBy.AccountIcon) || page.WaitForElementPresentAndVisible(CommonFindBy.ViewPos))
            { }
            else
            {
                throw new AssertFailedException("Authentication failed");
            }
        }

        public static bool LoginToAdminUrl(this Page page, CatalogAdminAuthentication catalogCredentials, int maxTries = 3) {

            //Navigate to admin url. It is set up in TestRunSettings
            page.GoToUrl(page.RunSettings.AdminUrl.ToString());
            
            //Log in using Admin Page
            LogInAdmin(page, catalogCredentials);

            page.Driver.WaitForDocumentReady();

            //Switch the focus to the new open window
            page.GetNewWindow();
            if (NUnit.Framework.TestContext.Parameters["TestRunEnvironment"] != "Prod" && NUnit.Framework.TestContext.Parameters["TestRunEnvironment"] != "Green"
                && NUnit.Framework.TestContext.Parameters["TestRunEnvironment"] != "Black")
            {
                page.SelectProject(false);
            }
            page.Driver.WaitForDocumentReady();

            //If we are on home, just wait of the page to be loaded
            return CheckUserIsLogged(page);
        }

        /// <summary>
        /// Check that the user is in Home page. If View Pos or Account Icon are visibles,the method return true.
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        private static bool CheckUserIsLogged(Page page)
        {
            bool logWasSuccess = false;
            try
            {
                if (page.WaitForElement(CommonFindBy.AccountIcon) || page.WaitForElementPresentAndVisible(CommonFindBy.ViewPos))
                {
                    page.CloseAllWindowsButCurrent();
                    logWasSuccess = true;
                }
                else
                {
                    logWasSuccess = false;
                }
            }
            catch (System.Exception)
            {
                Debug.WriteLine("When checking if CommonFindBy.AccountIcon or CommonFindBy.ViewPos in Home Page, an exception was catched.");
                throw new AssertFailedException("Authentication failed");
            }

            return logWasSuccess;
        }

        private static void LogInAdmin(Page page, CatalogAdminAuthentication catalogCredentials)
        {
            if (!page.Driver.Url.Contains("MyHLUtils"))
            {
                page.WaitForElement(CommonFindBy.OnlineLoginLnk);
                page.Single(CommonFindBy.OnlineLoginLnk).Click();
            }

            page.Driver.WaitForDocumentReady();

            if (page.Driver.Url.Contains("qa4") || page.Driver.Url.Contains("qa5"))
            {
                page.Single(CommonFindBy.AdminDsIdField).SendKeys(catalogCredentials.UserID);
                page.Single(CommonFindBy.AdminLoginButton).Click();
            }
            else
            {
                page.WaitForElement(CommonFindBy.AdminDsIdComboBox);
                WebElementExtension.SelectDropDownOption(page.Single(CommonFindBy.AdminDsIdComboBox), catalogCredentials.UserID);
                page.Single(CommonFindBy.AdminLoginButton).Click();
            }
        }
        private static void HandlePopup(this Page page, IDictionary<string, string> SetCookies = null)
        {
            if (page.WaitForElement(CommonFindBy.OnetrustAcceptbtn, 30))
            {
                PageExtension.WaitForTimeSpan(2000);
                //page.ClickElementJs(CommonFindBy.OnetrustAcceptbtn);
                page.ClickElementJs(CommonFindBy.OnetrustSettingbtn);
                PageExtension.WaitForTimeSpan(2000);
                bool IsAllowAll = false;
                foreach (KeyValuePair<string, string> Cookies in SetCookies)
                {
                    if (Cookies.Key == "AllowAll" && Cookies.Value.ToLower() == "true")
                    {
                        page.ClickElementJs(CommonFindBy.AllowAllbtn);
                        IsAllowAll = true;
                        break;
                    }
                    else
                    {
                        if (Cookies.Key == "AnalyticsCookies" && Cookies.Value.ToLower() == "true")
                            page.ClickElementJs(CommonFindBy.AnalyticsCookiesToggel);
                        if (Cookies.Key == "TargetingandAdvertisingCookies" && Cookies.Value.ToLower() == "true")
                            page.ClickElementJs(CommonFindBy.TargetingandAdvertisingCookiesToggel);
                        if (Cookies.Key == "PreferenceCookies" && Cookies.Value.ToLower() == "true")
                            page.ClickElementJs(CommonFindBy.PreferenceCookiesToggel);
                        if (Cookies.Key == "PerformanceCookies" && Cookies.Value.ToLower() == "true")
                            page.ClickElementJs(CommonFindBy.PerformanceCookiesToggel);  
                    }
                    
                }
                if (IsAllowAll == false)
                {
                    PageExtension.WaitForTimeSpan(1000);
                    page.Single(CommonFindBy.ConfrimMyChoiceBtn).Click();
                }
            }
            PageExtension.WaitForTimeSpan(1000);
        }
    }
}