using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Diagnostics;

namespace Shop.Automation.Framework
{
    public class Page
    {
        public virtual string RelativePath { get; set; }
        public virtual string Locale { get; set; }
        public virtual string Path { get; set; }
        public virtual string AlternativePath { get; set; }
        public IWebDriver Driver { get; set; }
        public TestRunSettings RunSettings { get; set; }

        public bool errorMessageCC = false;

        [FindsBy(How = How.XPath, Using = "//*[@id='projectID']")]
        public IWebElement Project_ID { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='userNamePrj']")]
        public IWebElement Username { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@class='optanon-allow-all accept-cookies-button']")]
        public IWebElement Cookies { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@data-bind='click: onDismissForLater, text: showMeLater.text, visible: showMeLater' and @class='btn right']")]
        public IWebElement ShowMeLaterPopUp { get; set; }

        //[FindsBy(How = How.XPath, Using = "//*[@id='modalNotif']//*[@class='btnForward right']")]
        //public IWebElement CompleteNowPopUp { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@class='button' and contains(text(),'Continue')]")]
        public IWebElement Continue { get; set; }


        public IWebElement Single(By by)
        {
            return Driver.FindElement(by);
        }

        public IReadOnlyList<IWebElement> Some(By by)
        {
            return Driver.FindElements(by);
        }
        public void PopulateElements()
        {
            PageFactory.InitElements(Driver, this);
            //PageFactory.InitElements(this, new RetryingElementLocator(Driver, TimeSpan.FromSeconds(1)));
        }

        public string StartWithUrl(string path)
        {
            if (Driver.Url.StartsWith("http"))
            {
                throw new InvalidOperationException("Already started somewhere else. Use Driver to navigate directly.");
            }
            GoToUrl(path);
            PopulateElements();
            return Driver.Url;
        }

        public string StartWithTargetedUrl(string relativePath)
        {
            if (Driver.Url.StartsWith("http"))
            {
                throw new InvalidOperationException("Already started somewhere else. Use Driver to navigate directly.");
            }
            GoToTargetedUrl(relativePath);
            PopulateElements();
            return Driver.Url;
        }

        public void GoToTargetedUrl(string relativePath)
        {
            var target = CreateTargetedUrl(relativePath);
            Driver.Navigate().GoToUrl(target);
        }

        public void GoToUrl(string relativePath)
        {
            var target = CreateUrl(relativePath);
            Driver.Navigate().GoToUrl(target);
        }

        public void GoToUrl(Uri url, string relativePath)
        {
            var target = CreateUrl(url, relativePath); 
            Driver.Navigate().GoToUrl(target);
        }

        public Uri CreateUrl(string relativePath)
        {
            if (string.IsNullOrEmpty(relativePath) && TestContext.Parameters["IsByAdmin"] == "0")
            {
                relativePath = RunSettings.Url.ToString();
            }
            else
            {
                relativePath = RunSettings.AdminUrl.ToString();
            }
            var target = new Uri(RunSettings.Url, relativePath.TrimStart('/'));
            return target;
        }

        public Uri CreateUrl(Uri url, string relativePath)
        {
            var target = new Uri(url, relativePath.TrimStart('/'));
            return target;
        }

        public Uri CreateTargetedUrl(string relativePath)
        {
            var target = new Uri(RunSettings.WPActiveURL, relativePath.TrimStart('/'));
            return target;
        }

        public string GetUrl()
        {
            try
            {
                return Driver.Url;
            }
            catch (Exception e)
            {
                return "";
            }
        }

        public void ClickElementJs(By by)
        {
            ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", Driver.FindElement(by));
        }

        public bool IsElementDisplayed(By ele)
        {
            IWebElement element = Driver.FindElement(ele);
            if (element.Displayed)
                return true;
            else
                return false;
        }

        public void SelectDropDownOption(By ele, string option)
        {
            IWebElement element = Driver.FindElement(ele);
            try
            {
                new SelectElement(element).SelectByValue(option);
            }
            catch (Exception e)
            {

            }

        }

        public void ScrollIntoView(By by)
        {
            if (Driver.FindElement(by).Displayed)
            {
                ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].scrollIntoView();", Driver.FindElement(by));
            }
        }

        public void ScrollIntoView(IWebElement element)
        {
            try
            {
                if (element.Displayed)
                {
                    ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].scrollIntoView(true);", element);
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void ScrollIntoBotttom()
        {
            ((IJavaScriptExecutor)Driver).ExecuteScript("scroll(0,400)");
        }
        
        public void LoadPage(string itemRelativePath)
        {
            Debug.WriteLine($"Go to {itemRelativePath}");
            GoToUrl(itemRelativePath);
            Driver.WaitForDocumentReady();
            this.WaitForElementGone(CommonFindBy.BusyIndicator);
        }


        public IWebDriver GetNewWindow()
        {
            string parentWindow = Driver.CurrentWindowHandle;
            string popupHandle = string.Empty;
            ReadOnlyCollection<string> windowHandles = Driver.WindowHandles;
            foreach (string handle in windowHandles)
            {
                if (handle != parentWindow)
                {
                    popupHandle = handle;
                    return Driver.SwitchTo().Window(popupHandle);
                }
            }
            return null;
        }

        public void SelectProject(bool isSSO)
        {
            if (isSSO == true)
            {
                IWebElement ID = Driver.FindElement(By.XPath("//*[@id='projectID']"));
                IWebElement Name = Driver.FindElement(By.XPath("//*[@id='userNamePrj']"));
                IWebElement Continue1 = Driver.FindElement(By.XPath("//*[@class='button' and contains(text(),'Continue')]"));
                if (CommonMethods.IsElementDisplayed(Cookies))
                {
                    Cookies.Click();
                }
                if (CommonMethods.IsElementDisplayed(ID))
                {
                    PageExtension.WaitForTimeSpan(2000);
                    string readFromConfig = TestContext.Parameters["ProjectID"];
                    string readFromConfig1 = Environment.UserName;
                    ID.SelectDropDownOption(readFromConfig);
                    PageExtension.WaitForTimeSpan(2000);
                    Name.SendKeys(readFromConfig1);
                    Continue1.Click();
                    PageExtension.WaitForTimeSpan(2000);
                }
                else
                {
                    Assert.IsTrue(true, "Project selction popup is not displayed");
                }
            }
            else
            {
                if (CommonMethods.IsElementDisplayed(Cookies)|| CommonMethods.IsElementDisplayed(ShowMeLaterPopUp) || CommonMethods.IsElementDisplayed(Project_ID))
                {
                    if (CommonMethods.IsElementDisplayed(Cookies))
                    {
                        Cookies.Click();
                    }
                    PageExtension.WaitForTimeSpan(2000);
                    if (CommonMethods.IsElementDisplayed(ShowMeLaterPopUp))
                    {
                        ShowMeLaterPopUp.Click();
                    }
                    PageExtension.WaitForTimeSpan(2000);
                    string readFromConfig = TestContext.Parameters["ProjectID"];
                    string readFromConfig1 = Environment.UserName;
                    Project_ID.SelectDropDownOption(readFromConfig);
                    PageExtension.WaitForTimeSpan(2000);
                    Username.SendKeys(readFromConfig1);
                    Continue.Click();
                    PageExtension.WaitForTimeSpan(2000);
                }
                else
                {
                    Assert.IsTrue(true, "Project selction popup is not displayed");
                }
            }
        }
        public void WaitForPageLoad()
        {
            try
            {
                new WebDriverWait(Driver, TimeSpan.FromSeconds(80)).Until(d => ((IJavaScriptExecutor)Driver).ExecuteScript("return document.readyState").Equals("complete"));
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception : " + e.Message);
                new WebDriverWait(Driver, TimeSpan.FromSeconds(80)).Until(d => ((IJavaScriptExecutor)Driver).ExecuteScript("return document.readyState").Equals("complete"));
            }
        }
        #region Catalog Added methods
        /// <summary>
        /// Navigate to original landing page that is set up in Path variable
        /// </summary>
        public virtual void LoadPage()
        {
            this.GoToUrl(this.Path);
            this.PopulateElements();
            this.Driver.WaitForDocumentReady();
            this.WaitForElementGone(CommonFindBy.BusyIndicator);
        }

        protected bool ValidateNumberOfElements(IList<IWebElement> listOfElements, int expectedNumberOfElements = 1, int tableLoadTimeOut = 5)
        {
            this.WaitUntil(p => listOfElements.Count >= 1, tableLoadTimeOut);

            if (listOfElements.Count < expectedNumberOfElements)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        //An Alert Error message is appering and is not possible to find some locations of the xpaths 
        //this method has been created to close it.
        public virtual void CloseAlertIssueWithCC()
        {
            try
            {
                if (errorMessageCC)
                {
                    if (this.WaitForElementPresentAndVisible(CommonFindBy.CloseAlertIssueWithCC, 2))
                    {
                        this.WaitForElementClicklable(Driver.FindElement(CommonFindBy.CloseAlertIssueWithCC)).Click();
                        this.WaitForElementGone(CommonFindBy.AlertIssueMessage);
                    }
                }
                else
                {
                    Trace.Write("Altert error message was not found");
                }
            }
            catch (Exception e)
            {

            }
        }

        public virtual void SelectAdmCountry(string country)
        {
            try
            {

                if (this.WaitForElementPresentAndVisible(CommonFindBy.SelectAdminLocale) && country != null)
                {
                    SelectElement selectCountry = new SelectElement(Driver.FindElement(CommonFindBy.SelectAdminLocale));
                    selectCountry.SelectByValue(country);
                }

            }
            catch (Exception e)
            {
                Trace.Write("Country DropDown not found");
            }
        }

        /// <summary>
        /// If Popup messsage is shown, click on Accept cookies.
        /// </summary>
        public bool AcceptCookies()
        {
            //check that the pop is part of the page
            IList<IWebElement> PopUpWindow = this.Driver.FindElements(CommonFindBy.AcceptCookiesPopup);

            if (PopUpWindow.Count > 0)
            {
                //wait till the button is visible and click it
                IWebElement acceptBtn = Driver.FindElement(CommonFindBy.AcceptCookiesBtn);
                this.WaitUntil(p => acceptBtn.Displayed, 5);

                acceptBtn.Click();

                this.WaitUntil(p => !PopUpWindow[0].Displayed);

                return true;
            }

            return false;
        }
        #endregion
    }
}
