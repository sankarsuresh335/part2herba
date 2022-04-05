using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Shop.Automation.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using Shop.Automation.Pages.Cart;
using Shop.Automation.Pages.Address;
using Shop.Automation.Framework.Helpers;
using NUnit.Framework;

namespace Shop.Automation.Pages.Catalog
{
    public class EventTicketsPage : Page
    {
        #region Constructors
        public EventTicketsPage() { }
        public EventTicketsPage(IWebDriver driver, TestRunSettings runSettings)
        {
            this.Driver = driver;
            this.RunSettings = runSettings;
            PopulateElements();
        }
        #endregion
        #region Mapping
        [FindsBy(How = How.XPath, Using = "//ul[@class='breadcrumbs']//a")]
        public IList<IWebElement> Breadcrumbs { get; set; }
        [FindsBy(How = How.XPath, Using = "//section[@class='product-content']")]
        public IWebElement ProductContent { get; set; }
        [FindsBy(How = How.XPath, Using = "//h2[@class='title']")]
        public IWebElement Title { get; set; }
        [FindsBy(How = How.XPath, Using = "//div[@class='sku']/span[1]")]
        public IWebElement SkuNumber { get; set; }
        [FindsBy(How = How.XPath, Using = "//a[@href='#product-information']")]
        public IWebElement MoreDetailsLink { get; set; }
        [FindsBy(How = How.XPath, Using = "//div[@class='zoomContainer']")]
        public IWebElement Zoomcontainer { get; set; }
        [FindsBy(How = How.XPath, Using = "//span[contains(@data-bind, 'text: Price')]")]
        public IWebElement RetailPrice { get; set; }
        [FindsBy(How = How.XPath, Using = "//a[@class='minus btn-increment']")]
        public IWebElement MinusBtn { get; set; }
        [FindsBy(How = How.XPath, Using = "//a[@class='plus btn-increment']")]
        public IWebElement PlusBtn { get; set; }
        [FindsBy(How = How.XPath, Using = "//input[@class='increment']")]
        public IWebElement IncrementInput { get; set; }
        [FindsBy(How = How.XPath, Using = "//h4[@class='sold-out']")]
        public IWebElement SoldOut { get; set; }
        [FindsBy(How = How.XPath, Using = "//a[@class='btn-add-cart-large']")]
        public IWebElement AddtoCartBtn { get; set; }
        [FindsBy(How = How.XPath, Using = "//ul[@id='myVariations']")]
        public IList<IWebElement> Myvariations { get; set; }
        [FindsBy(How = How.XPath, Using = "//ul[@class='thumbs']/li/img")]
        public IList<IWebElement> Thumbs { get; set; }
        [FindsBy(How = How.XPath, Using = "//span[@data-bind='click: ChangeImgSrc']")]
        public IWebElement ChangeImgSrc { get; set; }
        [FindsBy(How = How.XPath, Using = "//div[@class='zoomWindow']")]
        public IWebElement ZoomWindow { get; set; }


        #region VolumenCalculator section
        [FindsBy(How = How.XPath, Using = "//div[@class='add-confirm']")]
        public IWebElement VolumenCalculator { get; set; }
        [FindsBy(How = How.XPath, Using = "//span[@data-bind='text: DiscountedSubtotal']")]
        public IWebElement Subtotalcalculator { get; set; }
        [FindsBy(How = How.XPath, Using = "//span[@data-bind='text: totalVPStore']")]
        public IWebElement VolumeOnCalculator { get; set; }
        [FindsBy(How = How.Id, Using = "btn-catalog-view-cart")]
        public IWebElement ViewCartButton { get; set; }
        [FindsBy(How = How.Id, Using = "btn-catalog-go-checkout")]
        public IWebElement ProceedToCheckoutButton { get; set; }
        #endregion

        #region Product Overview
        [FindsBy(How = How.Id, Using = "info-toggle")]
        public IWebElement InfoToggle { get; set; }
        [FindsBy(How = How.XPath, Using = "//section[@class='product-content']")]
        public IList<IWebElement> ProductContentList { get; set; }
        #endregion
        #endregion

        #region Path Settings
        public override string Path => $"/{RunSettings.Locale}{RunSettings.Catalog_EventTickets_Details}{RunSettings.ValidUser.Schema}/{GetValidEventSKU()}";
        public string AddressPersonalPath => RunSettings.Url.ToString() + RunSettings.Locale + RunSettings.API_Address_Delivery + "personal/" + RunSettings.ValidUser.Schema;
        public string ShoppingCartPath => $"{RunSettings.Url}{RunSettings.Locale}{RunSettings.Cart_URL}{RunSettings.ValidUser.Schema}?{RunSettings.CartOrderCategory_RSO}";
        protected string ETOCartPath => $"{RunSettings.Locale}{RunSettings.Cart_URL}Ds/{RunSettings.CartOrderCategory_ETO}";
        #endregion

        public string GetValidEventSKU()
        {
            CatalogAPIConsumer apiConsumer = new CatalogAPIConsumer(TestContext.Parameters["targetEnvironment"], RunSettings.Locale);

            return apiConsumer.GetEventSKU();
        }

        #region Functionality of Test Cases.
        public List<IWebElement> FillPageElementsList()
        {
            List<IWebElement> EventsTicketsElementsList = new List<IWebElement>();
            //Page elements
            EventsTicketsElementsList.Add(Title);
            EventsTicketsElementsList.Add(SkuNumber);
            EventsTicketsElementsList.Add(RetailPrice);
            EventsTicketsElementsList.Add(ProductContent);
            return EventsTicketsElementsList;
        }
        public bool CheckingThumbs()
        {
            int starIndex, lastIndex, length = 0;
            String zoomImageURL, substring, src = null;

            foreach (var element in Thumbs)
            {
                src = element.GetAttribute("src");
                element.Click();
                WaitPageToLoad();
                zoomImageURL = ZoomWindow.GetCssValue("background-image");
                starIndex = zoomImageURL.IndexOf('"') + 1;
                lastIndex = zoomImageURL.LastIndexOf('"') - 1;
                length = lastIndex - starIndex + 1;
                substring = zoomImageURL.Substring(starIndex, length);
                if (!src.Equals(substring))
                {
                    return false;
                }
            }
            return true;
        }

        public float GetIncrementValuePlusOne()
        {
            this.DoClick(PlusBtn);
            return float.Parse(IncrementInput.GetAttribute("value"));
        }

        public float GetIncrementValueMinusOne()
        {
            this.DoClick(MinusBtn);
            Thread.Sleep(1000);
            return float.Parse(IncrementInput.GetAttribute("value"));
        }

        public void ClicOnPlusbutton()
        {
            this.DoClick(PlusBtn);
            WaitPageToLoad();
        }

        public void ClicAddToCart()
        {
            Debug.WriteLine("Clicking on AddtocartBtn");
            this.DoClick(AddtoCartBtn);
            WaitPageToLoad();
        }
        public void WaitPageToLoad()
        {
            try
            {
                Debug.WriteLine("Waiting page load webelements");
                Driver.WaitForDocumentReady();
                PageExtension.WaitForElementPresentAndVisible(this, Zoomcontainer);
                PageExtension.WaitUntil(this, b => AssertHelpers.WebElementExists(Title) == true);
                PageExtension.WaitForElementGone(this, CommonFindBy.BusyIndicator);
            }
            catch (Exception e)
            {
            }
        }

        public Pricelist GoPricelist(string path)
        {
            LoadPage(path);
            PageExtension.WaitForElementGone(this, CommonFindBy.BusyIndicator);
            return new Pricelist(Driver, RunSettings);
        }

        public void WaitToLoadProductDetails()
        {
            this.WaitForElementPresentAndVisible(SkuNumber);
            this.WaitForElementPresentAndVisible(ProductContentList.LastOrDefault());
        }

        public ShoppingCartPage ShoppingCartPage()
        {
            LoadPage(ShoppingCartPath);
            Driver.WaitForDocumentReady();
            this.WaitForElementGone(CommonFindBy.BusyIndicator);
            return new ShoppingCartPage(Driver, RunSettings);
        }

        public DeliveryAddressPersonalPage_US SetDeliveryAddressPersonal()
        {
            LoadPage(AddressPersonalPath);
            return new DeliveryAddressPersonalPage_US();
        }

        /// <summary>
        /// Navigate to Shop cart page.
        /// </summary>
        /// <returns></returns>
        public ShoppingCartPage GoToETOCart()
        {
            Debug.WriteLine("Landing to ETO Cart Page");
            GoToUrl(ETOCartPath);
            PageExtension.WaitForElementGone(this, CommonFindBy.BusyIndicator);
            return new ShoppingCartPage(Driver, RunSettings);
        }
        #endregion
    }
}
