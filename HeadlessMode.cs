using System;
using System.Configuration;
using NUnit.Framework;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;

namespace Shop.Automation.Framework
{
    public class HeadlessMode
    {
        private static HeadlessMode _instance;
        private const string _headlessChrome = "headless";

        public static HeadlessMode GetInstance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new HeadlessMode();
                }
                return _instance;
            }
            set { }
        }

        public ChromeOptions GetChromeOptions
        {
            get
            {
                ChromeOptions chromeOptions = null;
                if (!String.IsNullOrEmpty(TestContext.Parameters[_headlessChrome]) && Convert.ToBoolean(TestContext.Parameters[_headlessChrome]) == true)
                {
                    chromeOptions = new ChromeOptions();
                    chromeOptions.AddArguments("--headless");
                    chromeOptions.AddArguments("--disable-gpu");
                    chromeOptions.AddArguments("--window-size=1920,1080");
                    chromeOptions.AddArgument("--no-sandbox");
                    chromeOptions.AddUserProfilePreference("download.default_directory", Environment.CurrentDirectory + @"" + ConfigurationManager.AppSettings.Get("DownloadFilesPath"));
                    chromeOptions.AcceptInsecureCertificates = true;
                } else
                {
                    chromeOptions = new ChromeOptions();
                    chromeOptions.AddArguments("--window-size=1920,1080");
                    chromeOptions.AddArgument("--no-sandbox");
                    chromeOptions.AcceptInsecureCertificates = true;
                    chromeOptions.AddUserProfilePreference("download.default_directory", Environment.CurrentDirectory + @"" + ConfigurationManager.AppSettings.Get("DownloadFilesPath"));
                }
                return chromeOptions;
            }
            set { }
        }

        public FirefoxOptions GetFirefoxOptions
        {
            get
            {
                FirefoxOptions firefoxOptions = null;
                if (!String.IsNullOrEmpty(TestContext.Parameters[_headlessChrome]) && Convert.ToBoolean(TestContext.Parameters[_headlessChrome]) == true)
                {
                    firefoxOptions = new FirefoxOptions();
                    firefoxOptions.AddAdditionalCapability("acceptSslCerts", true, true);
                    firefoxOptions.AddAdditionalCapability("acceptInsecureCerts", true, true);
                    firefoxOptions.AddArgument("-width=1920");
                    firefoxOptions.AddArgument("-height=1080");
                    firefoxOptions.AddArgument("-headless");
                }
                else
                {
                    firefoxOptions.AddAdditionalCapability("acceptSslCerts", true, true);
                    firefoxOptions.AddAdditionalCapability("acceptInsecureCerts", true, true);
                    firefoxOptions.AddArgument("-width=1920");
                }
                return firefoxOptions;
            }
            set { }

        }

    }
}
