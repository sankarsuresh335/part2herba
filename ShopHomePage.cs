using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Shop.Automation.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Automation.Pages.Catalog
{
    public class ShopHomePage : Page
    {
        #region Constructors
        public ShopHomePage() { }
        public ShopHomePage(IWebDriver driver, TestRunSettings runSettings)
        {
            this.Driver = driver;
            this.RunSettings = runSettings;
            PopulateElements();
        }
        #endregion

        #region Mapping
        [FindsBy(How = How.XPath, Using = "//a[@class='btn-hero']")]
        public IWebElement OrderTodayLink { get; set; }
        [FindsBy(How = How.XPath, Using = "//div[@class='sku']//span")]
        public IList<IWebElement> SKUProduct { get; set; }
        [FindsBy(How = How.XPath, Using = "//a[@id='toggle-category']")]
        public IWebElement ShopByCategoryLink { get; set; }
        [FindsBy(How = How.XPath, Using = "//div[@class='goal']//child::a")]
        public IWebElement ViewMYDashboardLink { get; set; }
        [FindsBy(How = How.XPath, Using = "//input[@id='products-autocomplete']")]
        public IWebElement Search { get; set; }
        [FindsBy(How = How.XPath, Using = "//input[@id='globalSearch']")]
        public IWebElement GlobalSearch { get; set; }
        [FindsBy(How = How.XPath, Using = "//input[@id='home-search-form']")]
        public IWebElement SearchBtn { get; set; }
        [FindsBy(How = How.XPath, Using = "//a[@data-bind='click: onSelectSearchOption'][2]")]
        public IWebElement GlobalSearchBtn { get; set; }
        [FindsBy(How = How.XPath, Using = "//div[@class='item']")]
        public IWebElement Item { get; set; }
        [FindsBy(How = How.XPath, Using = "//div[@id='filter-mobile']//following::h4[1]")]
        public IList<IWebElement> SearchMessage { get; set; }
        [FindsBy(How = How.XPath, Using = "//a[@class='product-info']")]
        public IList<IWebElement> ContentProduct { get; set; }
        [FindsBy(How = How.XPath, Using = "//a[@class='close-shopby']")]
        public IWebElement CloseLink { get; set; }
        [FindsBy(How = How.XPath, Using = "//div[@class='category-list']/ul[1]//child::li")]
        public IList<IWebElement> Categories { get; set; }
        [FindsBy(How = How.XPath, Using = "//div[@class='category-list']//ul//self::ul//following-sibling::li[last()]/a")]
        public IList<IWebElement> AllLink { get; set; }
        [FindsBy(How = How.XPath, Using = "//div[@class='featured-product slick-slide slick-current slick-active']")]
        public IList<IWebElement> NewProducts { get; set; }
        [FindsBy(How = How.XPath, Using = "//div[@class='best-seller-product slick-slide slick-active']")]
        public IList<IWebElement> BestSellers { get; set; }
        [FindsBy(How = How.XPath, Using = "//section[@id='new-products']//div[@class='slick-track']//child::div[@aria-hidden='false']")]
        public IList<IWebElement> CountProducts { get; set; }
        [FindsBy(How = How.XPath, Using = "//section[@id='best-seller']//div[@class='slick-track']//child::div[@aria-hidden='false']")]
        public IList<IWebElement> CountBestSellers { get; set; }
        [FindsBy(How = How.XPath, Using = "//section[@id='new-products']//div[@class='slick-track']//child::div[contains(@aria-describedby,'slick-slide')]")]
        public IList<IWebElement> ItemProducts { get; set; }
        [FindsBy(How = How.XPath, Using = "//section[@id='best-seller']//div[@class='slick-track']//child::div[contains(@aria-describedby,'slick-slide')]")]
        public IList<IWebElement> ItemBestSellers { get; set; }
        [FindsBy(How = How.XPath, Using = "//Section[@id='new-products' and @data-rec-id='new' ]//button[@class='slicked-next icon-arrow-right-ln-2 slick-arrow']")]
        public IList<IWebElement> RightArrowProducts { get; set; }
        [FindsBy(How = How.XPath, Using = "//Section[@id='new-products' and @data-rec-id='new' ]//button[@class='slicked-prev icon-arrow-left-ln-2 slick-arrow']")]
        public IList<IWebElement> LeftArrowProducts { get; set; }
        [FindsBy(How = How.XPath, Using = "//Section[@id='best-seller' and @data-rec-id='bestseller' ]//button[@class='slicked-next icon-arrow-right-ln-2 slick-arrow']")]
        public IList<IWebElement> RightArrowBestSellers { get; set; }
        [FindsBy(How = How.XPath, Using = "//Section[@id='best-seller' and @data-rec-id='bestseller' ]//button[@class='slicked-prev icon-arrow-left-ln-2 slick-arrow']")]
        public IList<IWebElement> LeftArrowBestSellers { get; set; }
        #endregion Mapping

        #region Path Settings
        public override string Path => $"/{RunSettings.Locale}{RunSettings.Catalog_ShopHome_Page}{RunSettings.ValidUser.Schema}";
        #endregion

        #region Functionality of Test Cases.
        public bool SearchByNameOrSku(String nameorSku)
        {
            PageExtension.WaitForElementPresentAndVisible(this, Search);
            Search.SendKeys(nameorSku);
            WaitPageToLoad();
            this.DoClick(SearchBtn);
            return true;
        }

        public bool GlobalSearchByNameOrSku(String nameorSku)
        {
            PageExtension.WaitForElementPresentAndVisible(this, GlobalSearch);
            GlobalSearch.SendKeys(nameorSku);
            WaitPageToLoad();
            this.DoClick(GlobalSearchBtn);
            return true;
        }

        public void WaitPageToLoad()
        {
            Driver.WaitForDocumentReady();
            PageExtension.WaitForElement(this, CommonFindBy.AccountIcon);
        }

        public bool CheckMessageDisplayed(int waitTimeForNoProductMsg = 1)
        {
            if (this.WaitForElementPresentAndDisplayed(SearchMessage[0], waitTimeForNoProductMsg))
                return true;
            else
                return false;
        }

        public bool CheckContentDisplayed(int waitTimeForNoProductMsg = 1)
        {
            if (this.WaitForElementPresentAndDisplayed(ContentProduct[0], waitTimeForNoProductMsg))
                return true;
            else
                return false;
        }

        public void WaitAndClickClose()
        {
            this.WaitUntil(p => CloseLink.Displayed, 30);
            this.DoClick(CloseLink);
            this.WaitUntil(p => !CloseLink.Displayed, 30);
        }

        public void ClickShopByCategoryLink()
        {
            //Click on Shop by category Button
            this.WaitUntil(p => ShopByCategoryLink.Displayed, 30);
            this.DoClick(ShopByCategoryLink);
            this.WaitUntil(p => ShopByCategoryLink.Displayed, 100);
        }

        public void CheckProductsCarouselRight()
        {
            Boolean RightArrow = RightArrowProducts.Count() > 0;
            int products = ItemProducts.Count / 4;
            for (int i = 0; i < products; i++)
            {
                if (CountProducts.Count == 4 && RightArrow)
                {
                    this.WaitUntil(p => RightArrowProducts[0].Displayed);
                    this.DoClick(RightArrowProducts[0]);
                }
            }
        }

        public void CheckProductsCarouselLeft()
        {
            Boolean LeftArrow = LeftArrowProducts.Count() > 0;
            int products = ItemProducts.Count / 4;
            for (int i = 0; i < products; i++)
            {
                if (CountProducts.Count == 4 && LeftArrow)
                {
                    this.WaitUntil(p => LeftArrowProducts[0].Displayed);
                    this.DoClick(LeftArrowProducts[0]);
                }
            }
        }

        public void CheckBestSellersCarouselRight()
        {
            Boolean RightArrow = RightArrowBestSellers.Count() > 0;
            int products = ItemBestSellers.Count / 4;
            for (int i = 0; i < products; i++)
            {
                if (CountBestSellers.Count == 4 && RightArrow)
                {
                    this.WaitUntil(p => RightArrowBestSellers[0].Displayed);
                    this.DoClick(RightArrowBestSellers[0]);
                }
            }
        }

        public void CheckBestSellersCarouselLeft()
        {
            Boolean LeftArrow = LeftArrowBestSellers.Count() > 0;
            int products = ItemBestSellers.Count / 4;
            for (int i = 0; i < products; i++)
            {
                if (CountBestSellers.Count == 4 && LeftArrow)
                {
                    this.WaitUntil(p => LeftArrowBestSellers[0].Displayed);
                    this.DoClick(LeftArrowBestSellers[0]);
                }
            }
        }
        #endregion
    }
}
