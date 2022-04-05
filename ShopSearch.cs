using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Shop.Automation.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Automation.Pages.Catalog
{
    public class ShopSearch : Page
    {

        #region Constructors
        public ShopSearch() { }
        public ShopSearch
(IWebDriver driver, TestRunSettings runSettings)
        {
            this.Driver = driver;
            this.RunSettings = runSettings;
            PopulateElements();
        }
        #endregion

        #region Mapping
        [FindsBy(How = How.XPath, Using = "//*[@id='product']/section[1]/div[1]/span")]
        public IWebElement SKUProduct { get; set; }
        [FindsBy(How = How.XPath, Using = "//input[@id='globalSearch']")]
        public IWebElement GlobalSearch { get; set; }
        [FindsBy(How = How.XPath, Using = "//div[@id='suggestions']//a[2]")]
        public IWebElement SearchBtn { get; set; }
        [FindsBy(How = How.XPath, Using = "//div[@class='item']")]
        public IWebElement Item { get; set; }
        [FindsBy(How = How.XPath, Using = "//div[@id='filter-mobile']//following::h4[1]")]
        public IWebElement InvalidSearchMessage { get; set; }
        [FindsBy(How = How.XPath, Using = "//div[@class='product-list-items k-widget k-listview']")]
        public IWebElement ContentProduct { get; set; }

        //section[@id='best-seller']//div[@class='slick-track']//child::div[contains(@aria-describedby,'slick-slide1')]
        #endregion Mapping

        #region Path Settings
        public override string Path => $"/{RunSettings.Locale}{RunSettings.Catalog_ShopHome_Page}{RunSettings.ValidUser.Schema}";
        #endregion

        #region Functionality of Test Cases.

        public void ValidateSearchProductBYName(string name)
        {
            //Click on Search Button
            this.DoClick(SearchBtn);
        }

        /// <summary>
        /// gffssdf
        /// </summary>
        /// <param name="nameorSku"></param>
        /// <returns></returns>
        public bool InputNameOrSku(String nameorSku)
        {
            PageExtension.WaitForElementPresentAndVisible(this, GlobalSearch);
            GlobalSearch.SendKeys(nameorSku);
            WaitPageToLoad();
            if (WebElementExtension.IsWebElementDisplayed(Item))
            {
                Trace.WriteLine("Products not exists. no product to display");
                return false;
            }
            return true;
        }

        public void WaitPageToLoad()
        {
            Driver.WaitForDocumentReady();
            PageExtension.WaitForElement(this, CommonFindBy.AccountIcon);
        }

        

        public bool CheckMessageDisplayed(int waitTimeForNoProductMsg = 1)
        {
            if (this.WaitForElementPresentAndDisplayed(InvalidSearchMessage, waitTimeForNoProductMsg))
                return true;
            else
                return false;
        }

        public bool CheckContentDisplayed(int waitTimeForNoProductMsg = 1)
        {
            if (this.WaitForElementPresentAndDisplayed(ContentProduct, waitTimeForNoProductMsg))
                return true;
            else
                return false;
        }
        #endregion
    }
}