using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Shop.Automation.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using Shop.Automation.Pages.Cart;
using System.Text.RegularExpressions;
using Shop.Automation.Pages.Address;
using NUnit.Framework;
using Shop.Catalog.Service.Models;
using System.Net;
using Newtonsoft.Json;
using Shop.Automation.Framework.Helpers;

namespace Shop.Automation.Pages.Catalog
{

    public class EventTicketsListPage : Page
    {
        #region Constructors
        public EventTicketsListPage(){}
        public EventTicketsListPage(IWebDriver driver, TestRunSettings runSettings)
        {
            this.Driver = driver;
            this.RunSettings = runSettings;
            PopulateElements();
        }
        #endregion
        #region Mapping
        [FindsBy(How = How.Id, Using = "category")]
        public IWebElement CategoryFrame { get; set; }

        public By k_loading_mask = By.XPath("//div[contains(@class, 'k-loading-image')]");

        [FindsBy(How = How.XPath, Using = "//*[@id='event-list-items']")]
        public IList<IWebElement> EventContent { get; set; }
        #region Sort Region
        [FindsBy(How = How.Id, Using = "Adobe-TC-ProductGroupTitleAbove")]
        public IWebElement SubcategoryTitle { get; set; }
        [FindsBy(How = How.Id, Using = "select-event-price-order")]
        public IWebElement SortByDropdown { get; set; }
        [FindsBy(How = How.XPath, Using = "//div[@id='subcategory']")]
        public IWebElement ProductListViewModelFrame { get; set; }
        #endregion
        #region Product List Items
        [FindsBy(How = How.XPath, Using = "//section[@class='product-list']")]
        public IWebElement SectionProductList { get; set; }
        [FindsBy(How = How.Id, Using = "toggle-list")]
        public IWebElement ToggleList { get; set; }
        [FindsBy(How = How.XPath, Using = "//a[@class='product-info']//img")]
        public IList<IWebElement> ProductImageList { get; set; }
        [FindsBy(How = How.XPath, Using = "//h4[@data-bind='visible: NoProducts']")]
        public IList<IWebElement> NoProductoDisplayed { get; set; }
        [FindsBy(How = How.XPath, Using = "//div[@class='name']")]
        public IList<IWebElement> ProductNameByList { get; set; }
        [FindsBy(How = How.XPath, Using = "//a[@class='product-info']")]
        public IList<IWebElement> ProductNameLinksList { get; set; }
        //[FindsBy(How = How.XPath, Using = "//div[@class='num-of-items']")]
        //public IWebElement ShowResultTagBottom { get; set; }
        //[FindsBy(How = How.XPath, Using = "//div[@class='sku']")]
        //public IList<IWebElement> SkuNumberList { get; set; }
        [FindsBy(How = How.XPath, Using = "//span[@data-bind='text:Price']")]
        public IList<IWebElement> Price { get; set; }
        [FindsBy(How = How.XPath, Using = "//div[@data-bind='text: YourPrice']")]
        public IList<IWebElement> YourPriceList { get; set; }
        [FindsBy(How = How.XPath, Using = "//a[@class='minus btn-increment']")]
        public IList<IWebElement> MinusBtnList { get; set; }
        [FindsBy(How = How.XPath, Using = "//a[@class='plus btn-increment']")]
        public IList<IWebElement> PlusBtnList { get; set; }
        [FindsBy(How = How.XPath, Using = "//input[@class='increment']")]
        public IList<IWebElement> IncrementInputList { get; set; }
        [FindsBy(How = How.XPath, Using = "//a[@class='btn-add-cart']")]
        public IList<IWebElement> AddtoCartBtnList { get; set; }

        [FindsBy(How = How.XPath, Using = "//a[@class='btn-add-cart added']")]
        public IList<IWebElement> AddedtoCartBtnList { get; set; }

        [FindsBy(How = How.XPath, Using = "//span[@class='badge-counter']//parent::a")]
        public IWebElement Cartbtn { get; set; }

        [FindsBy(How = How.XPath, Using = "//a[contains(@class,'btn-add-cart') and @data-bind]")]
        public IList<IWebElement> AddToCartValidElementsList { get; set; }

        [FindsBy(How = How.XPath, Using = "//a[contains(@class,'btn-add-cart') and @data-bind]/ancestor::div[@class='item']/a[@class='product-info']//span[@data-bind='text:Sku']")]
        public IList<IWebElement> AddToCartSKUList { get; set; }

        [FindsBy(How = How.XPath, Using = "//a[contains(@class,'btn-add-cart') and @data-bind]/ancestor::div[@class='item']/a[@class='product-info']//span[@data-bind='text:Price']")]
        public IList<IWebElement> AddToCartPriceList { get; set; }

        private readonly string ticketPriceLocator = "./ancestor::div[@class='item']//*[@data-bind='text:Price']";
        #endregion

        #region [this section get element if increment input is displayed.]
        [FindsBy(How = How.XPath, Using = "//input[@class='increment']//ancestor::div[@class='add-to-cart']//preceding-sibling::a[@class='product-info']//span[@data-bind='text:Price']")]
        public IList<IWebElement> PriceList { get; set; }
        #endregion [this section get element if increment input is displayed.]	

        [FindsBy(How = How.XPath, Using = "//h4[@data-bind='visible: NoProducts']")]
        public IWebElement NoProducts { get; set; }
        [FindsBy(How = How.XPath, Using = "//span[@data-bind='invisible: IsFilterOn, text: totalProductsGrouped']")]
        public IWebElement TotalProductsGrouped { get; set; }
        [FindsBy(How = How.XPath, Using = "//span[@data-bind='text: displayedProducts']")]
        public IWebElement DisplayedProducts { get; set; }
        [FindsBy(How = How.XPath, Using = "//div[@class='k-widget k-window modal-order-purpose k-state-focused']")]
        public IWebElement SelectAnOrderPurpose { get; set; }
        public warehouse CurrentWarehouse { get; set; }

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

        #endregion
        #region Path Settings
        //public override string Path => $"/{RunSettings.Locale}{RunSettings.Catalog_EventTickets_List}{RunSettings.ValidUser.Schema}/{RunSettings.CatalogCategory.Event_List}";
        public override string Path => $"/{RunSettings.Locale}{RunSettings.Catalog_EventTickets_List}{RunSettings.ValidUser.Schema}/{GetCategoryEventFromAPI()}";
        public string AddressPersonalPath => RunSettings.Url.ToString() + RunSettings.Locale + RunSettings.API_Address_Delivery + "personal/" + RunSettings.ValidUser.Schema;
        public string PathFavorite => $"/{RunSettings.Locale}{RunSettings.Catalog_FavoritePage}";
        protected string ETOCartPath => $"{RunSettings.Locale}{RunSettings.Cart_URL}Ds/{RunSettings.CartOrderCategory_ETO}";
        public string EventCategoryList { get; set; }
        #endregion

        public string GetCategoryEventFromAPI()
        {
            CatalogAPIConsumer apiConsumer = new CatalogAPIConsumer(TestContext.Parameters["targetEnvironment"], RunSettings.Locale);

            return apiConsumer.GetEventCategoryID();
        }
        #region Functionality of Test Cases.

        public void SelectSortByOption(string option)
        {
            Debug.WriteLine($"Selecting dropDrown Option -> {option}");
            SortByDropdown.SelectDropDownOption(option);
            PageExtension.WaitForElementGone(this, CommonFindBy.BusyIndicator);
        }

        public IReadOnlyList<string> GetProductListNames()
        {
            var ret = new List<string>();
            var listItemsStrings = ProductNameByList.Select(item => item.Text.Replace(System.Environment.NewLine, ""));
            ret.AddRange(listItemsStrings);
            Driver.FindElement(By.XPath(""));
            return ret;
        }

        public IList<double> GetProductListValues(IList<IWebElement> listItems)
        {
            Debug.WriteLine("Creating List of webElements");
            var ret = new List<double>();
            foreach (IWebElement item in listItems)
            {
                double value = Convert.ToDouble(item.Text);
                ret.Add(value);
            }
            return ret;
        }

        public static Tuple<int, int> GetTwoValuesDifferentRandom(int min, int max)
        {
            Random rnd = new Random();
            int num = rnd.Next(min, max);
            int n = 0;
            n = rnd.Next(1, max);
            n = n == num ? n + 1 : n;
            n = n >= max ? n - 2 : n;
            return new Tuple<int, int>(num, n);
        }



        public void WaitPageToLoad()
        {
            try
            {
                Driver.WaitForDocumentReady();
                PageExtension.WaitForElementPresentAndVisible(this, SectionProductList);
                PageExtension.WaitUntil(this, b => AssertHelpers.WebElementExists(NoProducts) == true);
                PageExtension.WaitForElementGone(this, CommonFindBy.BusyIndicator);
            }
            catch (Exception ex)
            {
            }
        }

        public Pricelist GoPricelist(string path)
        {
            GoToUrl(path);
            PageExtension.WaitForElementGone(this, CommonFindBy.BusyIndicator);
            return new Pricelist(Driver, RunSettings);
        }

        public FavoriteProducts GoToFavoriteProducts()
        {
            Debug.WriteLine("Landing to Energy And Fitness Hydratation");
            GoToUrl(PathFavorite);
            PageExtension.WaitForElementGone(this, CommonFindBy.BusyIndicator);
            return new FavoriteProducts(Driver, RunSettings);
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

        /// <summary>

        /// Add one event to cart. Method waits till the event is added to cart.
        /// </summary>
        /// <param name="v"></param>
        public void AddToCartElement(int indexOfTicket)
        {
            //Click add button of the specified element and wait for the spinner icon tobe removed
            this.DoClick(AddtoCartBtnList, indexOfTicket);
            this.WaitForElementGone(CommonFindBy.BusyIndicator);

            //Wait till the class of the Add button cart return to the default
            this.WaitUntil(p => AddedtoCartBtnList.Count == 0);
        }


        /// <summary>
        /// Validate we 1 or more tickets available to work with
        /// </summary>
        /// <returns></returns>
        public void ValidateThereAreEnoughEventsToSentToCart(int expectedNumberOfElements)
        {
            if (!ValidateNumberOfElements(AddtoCartBtnList, expectedNumberOfElements))
            {
                throw new Exception($"No enough elements to continue with the test.");
            }
        }

        /// <summary>
        /// Click Add to cart to a element specified as an index and return the sum of all the values that were sent to cart
        /// </summary>
        /// <param name="numberOfElementsToAddToCart"></param>
        /// <param name="listOfValues"></param>
        /// <returns></returns>
        public float GetSubtotalSumOfProducts(int numberOfElementsToAddToCart, IList<IWebElement> listOfValues)
        {
            //Get the sum of n number of products
            float listSubtotalSum = 0;
            bool addValueTotal = true;

            //Do not use AddtoCartBtnList, there are some buttons that redirects to product details and after clicking and for a couple of seconds the class of the button changes.
            for (int i = 0; i < numberOfElementsToAddToCart; i++)
            {
                //Expicit Wait until the table is refreshed (if needed)
                this.WaitUntil(d => this.AddToCartValidElementsList.Count >= numberOfElementsToAddToCart);
                this.WaitForElementGone(CommonFindBy.BusyIndicator);
                this.Driver.WaitForDocumentReady();

                //Click Add to Cart
                this.DoClick(this.AddToCartValidElementsList, i);
                this.WaitUntil(p => AddedtoCartBtnList.Count == 0);

                //Wait until volume banner is displayed
                this.WaitForElementGone(CommonFindBy.BusyIndicator);

                //If the banner is not displayed, check if we land to cart page
                if (!this.WaitForElementPresentAndVisible(this.VolumenCalculator) && this.Driver.Url.Contains("/Shop/Cart/Home/Index/"))
                {
                    string skuToRemove = this.AddToCartSKUList.ElementAt(i).Text;

                    ShoppingCartPage shoppingCartPage = new ShoppingCartPage();
                    shoppingCartPage.ShRemoveProductBySKU(skuToRemove);

                    //Go Back to Product list page
                    //TODO: Create a method to navigate back
                    //TODO: Create a method to encapsulate document ready
                    this.Driver.Navigate().Back();
                    this.WaitForElementGone(CommonFindBy.BusyIndicator);
                    this.Driver.WaitForDocumentReady();

                    //Increment the value to try to add one more product
                    numberOfElementsToAddToCart++;

                    //Do not add the value to the total
                    addValueTotal = false;
                }
                else if (!this.WaitForElementPresentAndVisible(this.VolumenCalculator))
                {
                    Assert.Fail("Could not add the product to cart.");
                }

                //Add the value to the total
                if (addValueTotal)
                {
                    float valueElementValue = float.Parse(listOfValues.ElementAt(i).Text);
                    listSubtotalSum += valueElementValue;
                }

                addValueTotal = true;
            }

            return listSubtotalSum;
        }

        /// <summary>
        /// Get the Ticket price of a element from AddtoCartBtnList list. 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Double GetPriceOfTicketIndex(int index)
        {
            return Double.Parse(AddtoCartBtnList.ElementAt(index).FindElement(By.XPath(ticketPriceLocator)).Text);
        }

        public ShoppingCartPage ShoppingCartPage()
        {
            Cartbtn.Click();
            Driver.WaitForDocumentReady();
            this.WaitForElementGone(CommonFindBy.BusyIndicator);
            return new ShoppingCartPage(Driver, RunSettings);
        }

        public DeliveryAddressPersonalPage_US SetDeliveryAddressPersonal()
        {
            LoadPage(AddressPersonalPath);
            return new DeliveryAddressPersonalPage_US();
        }
        #endregion
    }
}
