using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Shop.Automation.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using Shop.Automation.Pages.Address;

using System.Collections;
using NUnit.Framework;

namespace Shop.Automation.Pages.Catalog
{
    public class Pricelist : Page
    {
        #region Constructors
        public Pricelist() { }
        public Pricelist(IWebDriver driver, TestRunSettings runSettings)
        {
            this.Driver = driver;
            this.RunSettings = runSettings;
            PopulateElements();
        }
        #endregion
        #region Mapping
        public const string ProductNameBy = "//span[@data-bind='html: Name']";
        public const string RetailPriceBy = "//span[@data-bind='text: Price']";
        public const string VolumePointsBy = "//span[@data-bind='text: VolumePoints']";

        [FindsBy(How = How.XPath, Using = "//div[@wire-model='OrderIntention']")]
        public IWebElement OrderPurposeSection { get; set; }
        [FindsBy(How = How.XPath, Using = "//a[@wire-model='VideoTutorialViewModel']")]
        public IWebElement VideoTutorialLink { get; set; }
        [FindsBy(How = How.Id, Using = "video-tutorial")]
        public IWebElement VideoTutorial { get; set; }
        [FindsBy(How = How.XPath, Using = "//div[./span[@id='modal-video-tutorial_wnd_title']]//a[@class='k-window-action k-link']")]
        public IWebElement CloseVideoTutorial { get; set; }

        #region [PAGE FILTERS]

        [FindsBy(How = How.Id, Using = "search")]
        public IWebElement SearchProductsBar { get; set; }
        [FindsBy(How = How.Id, Using = "Filter")]
        public IWebElement FilteryByDropdown { get; set; }
        [FindsBy(How = How.Id, Using = "select-catalog-price-filter")]
        public IWebElement SortByDropdown { get; set; }
        #endregion [page filters]
        [FindsBy(How = How.XPath, Using = "//ul[@class='breadcrumbs']//a")]
        public IList<IWebElement> Breadcrumbs { get; set; }
        #region [PRODUCTS SECTION]
        [FindsBy(How = How.XPath, Using = "//span[@data-bind='html: Name']")]
        public IList<IWebElement> ProductNameByList { get; set; }
        [FindsBy(How = How.XPath, Using = "//div[@class='p-info']/a")]
        public IList<IWebElement> ProductNameLinksList { get; set; }
        [FindsBy(How = How.XPath, Using = "//span[@class='num-items']//span[@data-bind='text: displayedProducts']")]
        public IWebElement DisplayedProducts { get; set; }
        [FindsBy(How = How.XPath, Using = "//span[@class='num-items']//span[@data-bind='text: totalProducts']")]
        public IWebElement TotalProducts { get; set; }
        [FindsBy(How = How.XPath, Using = "//div[@class='num-of-items']")]
        public IWebElement ShowResultTagBottom { get; set; }
        [FindsBy(How = How.ClassName, Using = "icon-heart-fl-2")]
        public IWebElement FavoritesIcon { get; set; }
        [FindsBy(How = How.XPath, Using = "//div[@class='favorite-info']/a")]
        public IWebElement ObtainFavorite { get; set; }

        [FindsBy(How = How.XPath, Using = "//a[@class='favorite-toggle']//parent::div")]
        public IList<IWebElement> FavoriteGray { get; set; }

        [FindsBy(How = How.XPath, Using = "//a[contains(@class, 'hearth-pulsing')]")]
        public IList<IWebElement> FavoriteTransition { get; set; }

        [FindsBy(How = How.XPath, Using = "//a[@class='favorite-toggle active']//parent::div")]
        public IList<IWebElement> FavoriteSelected { get; set; }
        [FindsBy(How = How.XPath, Using = "//div[@data-bind='visible: NoProducts']")]
        public IWebElement NoProducts { get; set; }
        [FindsBy(How = How.XPath, Using = "//div[@data-bind='text: Sku']")]
        public IWebElement SkuNumber { get; set; }
        [FindsBy(How = How.XPath, Using = ProductNameBy)]
        public IWebElement ProductName { get; set; }
        [FindsBy(How = How.XPath, Using = RetailPriceBy)]
        public IWebElement RetailPrice { get; set; }
        [FindsBy(How = How.XPath, Using = VolumePointsBy)]
        public IWebElement VolumePoints { get; set; }
        [FindsBy(How = How.XPath, Using = "//span[@data-bind='text: EarnBase']")]
        public IWebElement EarnBase { get; set; }
        [FindsBy(How = How.XPath, Using = "//*[@class='minus btn-increment']")]
        public IWebElement MinusButton { get; set; }
        [FindsBy(How = How.XPath, Using = "//*[@class='minus btn-increment']")]
        public IList<IWebElement> MinusButtonArray { get; set; }
        [FindsBy(How = How.ClassName, Using = "increment")]
        public IWebElement IncrementInput { get; set; }
        #endregion [Products section = PS]
        #region [THIS SECTION GET ELEMENT IF INCREMENT INPUT IS DISPLAYED.]
        [FindsBy(How = How.XPath, Using = "//input[@class='increment']//ancestor::div[@class='hl-form']//preceding-sibling::div[@class='sku-info' and @data-bind='text: Sku']")]
        public IList<IWebElement> SkuInfo_b { get; set; }
        //get all increment input 
        [FindsBy(How = How.XPath, Using = "//input[@class='increment']")]
        public IList<IWebElement> SelectedProductItemsInputList { get; set; }
        [FindsBy(How = How.XPath, Using = "//input[@class='increment']//ancestor::div[@class='hl-form']//preceding-sibling::div[@class='p-info']//*[@data-bind='text: Price']")]
        public IList<IWebElement> PriceList { get; set; }
        [FindsBy(How = How.XPath, Using = "//input[@class='increment']//ancestor::div[@class='hl-form']//preceding-sibling::div[@class='p-info']//*[@data-bind='text: VolumePoints']")]
        public IList<IWebElement> VolumenPointList { get; set; }
        [FindsBy(How = How.XPath, Using = "//*[@class='minus btn-increment']")]
        public IList<IWebElement> MinusButtonList { get; set; }
        [FindsBy(How = How.XPath, Using = "//*[@class='plus btn-increment']")]
        public IList<IWebElement> PlusButtonList { get; set; }
        #endregion [this section get element if increment input is displayed.]
        #region [PRODUCT LIST ITEMS]
        [FindsBy(How = How.XPath, Using = "//*[@class='plus btn-increment']")]
        public IWebElement PlusButton { get; set; }
        [FindsBy(How = How.XPath, Using = "//div[@data-bind='visible: NoProducts']")]
        public IWebElement NoProductsMessage { get; set; }
        [FindsBy(How = How.XPath, Using = "//a[@data-bind='click: ShowAvailability']")]
        public IWebElement ShowAvailability { get; set; } //Show availability option
        [FindsBy(How = How.XPath, Using = "//a[@data-bind='click: HideAvailability']")]
        public IWebElement HideAvailability { get; set; } //Hide availability option
        //                *** Product Section -> Product Available
        [FindsBy(How = How.XPath, Using = "//div[@class='availability' and @style='display: block;']")]
        public IWebElement AvailabilityDiv { get; set; } //Availability div after clic on showAvailability option.
        [FindsBy(How = How.XPath, Using = "//div[@class='availability' and @style='display: block;']//a[@data-bind='click: notifyMe']")]
        public IWebElement NotifyMe { get; set; } //Notify Me option.

        [FindsBy(How = How.XPath, Using = "//a[@class='notify-me']")]
        public IWebElement NotifyMeFor { get; set; } //Notify Me option.
        
        [FindsBy(How = How.XPath, Using = "//a[@class='notify-me']")]
        public IList<IWebElement> NotifyMeForList { get; set; }

        [FindsBy(How = How.XPath, Using = "//a[@class='notify-me active']")]
        public IWebElement NotifyMeActive { get; set; } //Notify Me option.
        [FindsBy(How = How.XPath, Using = "//div[@class='availability' and @style='display: block;']//p[@class='all-products']//a")]
        public IWebElement ViewAvailabilityForAllProducts { get; set; } //Notify Me option.
        [FindsBy(How = How.XPath, Using = "//h4[@class='sold-out']")]
        public IWebElement SoldOut { get; set; } //Notify Me option.

        private string AvailabilityContent => ".//ancestor::div[@class='hl-form']/following-sibling::div[@class='availability']";

        #endregion
        #region [PRICING SECTION]
        [FindsBy(How = How.XPath, Using = "//span[@data-bind='text: Discount']")]
        public IWebElement DiscountRate { get; set; }
        [FindsBy(How = How.XPath, Using = "//span[@data-bind='text: RemainingVP']")]
        public IWebElement RemainingVp { get; set; }
        [FindsBy(How = How.XPath, Using = "//span[@data-bind='text: totalItemStore']")]
        public IWebElement ItemsAddedToCart { get; set; }
        [FindsBy(How = How.XPath, Using = "//div[@data-bind='text: totalPriceStore']")]
        public IWebElement TotalPriceAddedToCart { get; set; }
        [FindsBy(How = How.XPath, Using = "//div[@class='price']//span[@data-bind='text: totalVPStore']")]
        public IWebElement TotalVpAddedToCart { get; set; }
        [FindsBy(How = How.XPath, Using = "//span[@data-bind='text: totalItemSelected']")]
        public IWebElement ItemsSelected { get; set; }
        [FindsBy(How = How.XPath, Using = "//div[@data-bind='text: totalPriceSelected']")]
        public IWebElement TotalPriceSelected { get; set; }
        [FindsBy(How = How.XPath, Using = "//a[@data-bind='click: clearCart']")]
        public IWebElement ClearCart { get; set; }
        [FindsBy(How = How.XPath, Using = "//span[@data-bind='text: totalVPSelected']")]
        public IWebElement TotalVpSelected { get; set; }
        [FindsBy(How = How.XPath, Using = "//span[@data-bind='text: totalCartItems']")]
        public IWebElement TotalCartItems { get; set; }
        [FindsBy(How = How.XPath, Using = "//div[@data-bind='text: totalCartPrice']")]
        public IWebElement TotalCartPrice { get; set; }
        [FindsBy(How = How.XPath, Using = "//span[@data-bind='text: totalCartVP']")]
        public IWebElement TotalCartVps { get; set; }
        [FindsBy(How = How.XPath, Using = "//a[@class='btn-add-cart']")]
        public IWebElement ButtonBuyNow { get; set; }
        [FindsBy(How = How.XPath, Using = "//a[@wire-model='SaveCartViewModel']")]
        public IWebElement SaveForLater { get; set; }

        [FindsBy(How = How.CssSelector, Using = "div#product-lines>div")]
        public IList<IWebElement> ProductElementContainerList { get; set; }
        #endregion
        #region [SAVECART SCREEN]       
        [FindsBy(How = How.XPath, Using = "//div[@class='cart-nickname']")]
        public IWebElement SaveCart_SaveCartTag { get; set; }
        [FindsBy(How = How.XPath, Using = "//div[@class='k-overlay']")]
        public IWebElement SaveCart_Overlay { get; set; }
        [FindsBy(How = How.XPath, Using = "(//span[@class='k-icon k-i-close icon-delete-fl-5'])[2]")]
        public IWebElement SaveCart_Close { get; set; }
        [FindsBy(How = How.XPath, Using = "//a[@class='btn-cancel opp-side' and @data-bind='click: CloseSavedCartModal']")]
        public IWebElement SaveCart_Cancel { get; set; }
        [FindsBy(How = How.XPath, Using = "//input[@data-bind='value: CartName, events: {keyup: checkPressKey} ']")]
        public IWebElement SaveCart_CartNameInput { get; set; }
        [FindsBy(How = How.XPath, Using = "//a[@class='btn-save' and @data-bind='click: SaveCartNickname']")]
        public IWebElement SaveCart_SaveButton { get; set; }
        [FindsBy(How = How.XPath, Using = "//*[@data-bind='text: Error']")]
        public IWebElement SaveCart_Warning { get; set; }
        [FindsBy(How = How.XPath, Using = "//div[@class='confirm-nickname']")]
        public IWebElement SaveCart_YourCartHasBeenSavedTag { get; set; }
        #endregion
        #region [Get Element if Icon Gray is present.]
        [FindsBy(How = How.XPath, Using = "//a[@class='favorite-toggle']//ancestor::div[@class='favorite-info']//following-sibling::div[@data-bind='text: Sku']")]
        public IList<IWebElement> GetSkuFavoritedGrayPresent { get; set; }
        #endregion
        public List<IWebElement> PricelistElementsList { get; set; }
        #endregion
        #region Path Settings
        public override string Path => $"/{RunSettings.Locale}{RunSettings.Catalog_Pricelist}{RunSettings.ValidUser.Schema}";
        public string AddressPersonalPath => RunSettings.Url.ToString() + RunSettings.Locale + RunSettings.API_Address_Delivery + "personal/" + RunSettings.ValidUser.Schema;
        public string FavoritePage => $"/{RunSettings.Locale}{RunSettings.Catalog_FavoritePage}{RunSettings.ValidUser.Schema}";
        #endregion
        #region Functionality of Test Cases.
        public List<IWebElement> FillPageElementsList()
        {
            List<IWebElement> PricelistElementsList = new List<IWebElement>();
            //Page elements
            PricelistElementsList.Add(OrderPurposeSection);
            //Filter elements
            PricelistElementsList.Add(SearchProductsBar);
            PricelistElementsList.Add(FilteryByDropdown);
            PricelistElementsList.Add(SortByDropdown);
            //Products listed elements
            PricelistElementsList.Add(FavoritesIcon);
            PricelistElementsList.Add(SkuNumber);
            PricelistElementsList.Add(ProductName);
            PricelistElementsList.Add(RetailPrice);
            PricelistElementsList.Add(VolumePoints);
            PricelistElementsList.Add(EarnBase);
            PricelistElementsList.Add(MinusButton);
            PricelistElementsList.Add(IncrementInput);
            PricelistElementsList.Add(PlusButton);
            //Pricing elements
            PricelistElementsList.Add(DiscountRate);
            PricelistElementsList.Add(ItemsAddedToCart);
            PricelistElementsList.Add(TotalPriceAddedToCart);
            PricelistElementsList.Add(TotalVpAddedToCart);
            PricelistElementsList.Add(ItemsSelected);
            PricelistElementsList.Add(TotalPriceSelected);
            PricelistElementsList.Add(TotalVpSelected);
            PricelistElementsList.Add(TotalCartItems);
            PricelistElementsList.Add(TotalCartPrice);
            PricelistElementsList.Add(TotalCartVps);
            PricelistElementsList.Add(ButtonBuyNow);
            return PricelistElementsList;
        }

        public void SelectSortByOption(string option)
        {
            SortByDropdown.SelectDropDownOption(option);
            WaitPageToLoad();
            PopulateElements();
        }



        public IReadOnlyList<string> GetProductListNames()
        {
            var ret = new List<string>();
            IReadOnlyList<IWebElement> listItems = listItems = Some(By.XPath(ProductNameBy));
            var listItemsStrings = listItems.Select(item => item.Text.Replace(System.Environment.NewLine, ""));
            ret.AddRange(listItemsStrings);
            return ret;
        }

        public IReadOnlyList<double> GetProductListValues(SortByOption option)
        {
            IReadOnlyList<IWebElement> listItems = null;
            var ret = new List<double>();
            switch (option)
            {
                case SortByOption.Price:
                    listItems = Some(By.XPath(RetailPriceBy));
                    break;
                case SortByOption.Volume:
                    listItems = Some(By.XPath(VolumePointsBy));
                    break;
            }
            foreach (IWebElement item in listItems)
            {
                double value = Convert.ToDouble(item.Text);
                ret.Add(value);
            }
            return ret;
        }

        public enum SortByOption
        {
            Name,
            Price,
            Volume
        }

        public int GetTotalFavorite()
        {
            FilteryByDropdown.SelectDropDownOption("favorites");
            PageExtension.WaitForElementGone(this, CommonFindBy.BusyIndicator);
            PopulateElements();
            PageExtension.WaitUntil(this, b => FavoriteSelected.Count > 0);
            PageExtension.WaitForElementPresentAndVisible(this, TotalProducts);
            PageExtension.ScrollElementIntoMiddle(this, TotalProducts);
            int totalProducts = int.Parse(TotalProducts.Text);
            return totalProducts;
        }

        public void ResetFavorites()
        {
            FilteryByDropdown.SelectDropDownOption("favorites");
            PageExtension.WaitForElementGone(this, CommonFindBy.BusyIndicator);

            //If "No Products to Display" message is shown, do not try to clear fav.
            if (!CheckNoProductLabelDisplayed())
            {
                //Scroll till all the fav has been displayed and the remove them from favs
                PageExtension.WaitForElementGone(this, CommonFindBy.BusyIndicator);
                PopulateElements();
                PageExtension.WaitUntil(this, b => FavoriteSelected.Count > 0);
                if (ScrollingDownByResults() == true)
                {
                    PopulateElements();
                    PageExtension.WaitUntil(this, b => FavoriteSelected.Count > 0);
                    foreach (IWebElement element in FavoriteSelected)
                    {
                        this.DoClick(element);
                        WaitForProductToBeInFav();
                    }
                }
            }

            //Select All Products
            FilteryByDropdown.SelectDropDownOption("");
            PageExtension.WaitForElementGone(this, CommonFindBy.BusyIndicator);
            PopulateElements();
            PageExtension.WaitUntil(this, b => FavoriteGray.Count > 0);
        }

        public bool ScrollingDownByResults()
        {         
            ValidateProductsAreDisplayed();

            int Showing = int.Parse(this.WaitForElementTextToBeDisplayed(DisplayedProducts));
            int TotalProductsLabel = int.Parse(this.WaitForElementTextToBeDisplayed(TotalProducts));

            do
            {
                //Get the number of elements currently being display scroll the last element to the top of the window in order to refresh the number of elements
                ScrollIntoView(ProductElementContainerList[Showing - 1]);

                //Wait till the product list is refreshed
                PageExtension.WaitForElementGone(this, CommonFindBy.BusyIndicator);
                this.Driver.WaitForDocumentReady();

                Showing = int.Parse(this.WaitForElementTextToBeDisplayed(DisplayedProducts));
            } while (Showing < TotalProductsLabel);

            //Validate that the number of products matches the total products label
            if (ProductElementContainerList.Count == TotalProductsLabel)
                return true;
            else
                return false;
        }

        public bool ScrollingForElement(IWebElement element)
        {
            if (WebElementExtension.IsWebElementDisplayed(NoProducts))
            {
                return false;
            }

            do
            {
                ScrollToResults();

                if (WebElementExtension.IsWebElementDisplayed(element))
                {
                    ScrollToResults();
                    return true;
                }
            } while (Int16.Parse(DisplayedProducts.Text) < Int16.Parse(TotalProducts.Text));

            return false;
        }

        private void ScrollToResults()
        {
            PopulateElements();
            ScrollIntoView(ShowResultTagBottom);
            Driver.WaitForDocumentReady();
            PageExtension.WaitForElementGone(this, CommonFindBy.BusyIndicator);
        }

        /// <summary>
        /// Some xpaths return a list of elements even though the elements are not loaded or visibles (As Notify link). This method scroll till find a visible element in the list its index.
        /// Return -1 if element not visible.
        /// </summary>
        /// <param name="elementList"></param>
        /// <returns></returns>
        public int ScrollingTillFindElement(IList<IWebElement> elementList)
        {
            //Check that products are displayed
            ValidateProductsAreDisplayed();

            int elementIndex = -1;
            bool webElementFound = false;

            while (Int16.Parse(DisplayedProducts.Text) < Int16.Parse(TotalProducts.Text))
            {
                elementIndex = 0;

                foreach (IWebElement element in elementList)
                {
                    if (WebElementExtension.IsWebElementDisplayed(element))
                    {
                        webElementFound = true;
                        break;
                    }

                    elementIndex++;
                }

                if (webElementFound)
                {
                    break;
                }
                else
                {
                    PopulateElements();
                    ScrollIntoView(ShowResultTagBottom);
                    Driver.WaitForDocumentReady();
                    PageExtension.WaitForElementGone(this, CommonFindBy.BusyIndicator);
                }
            }

            if (!webElementFound)
            {
                return -1;
            }
            else
            {
                return elementIndex;
            }

        }

        public bool SaveCartForLater()
        {
            int i = 0;
            PageExtension.WaitForElementClicklable(this, SaveForLater);
            PageExtension.ScrollElementIntoMiddle(this, SaveForLater);
            PageExtension.WaitForElementGone(this,CommonFindBy.BusyIndicator);
            this.DoClick(SaveForLater);
            PageExtension.WaitForElementPresentAndVisible(this,SaveCart_Overlay);
            do
            {
                SaveCart_CartNameInput.Clear();
                SaveCart_CartNameInput.SendKeys(String.Concat("Test_" + DateTime.Now.ToString("hh.mm.ss.ffffff"), i));
                SaveCart_SaveButton.Click();
                PageExtension.WaitForElementGone(this, CommonFindBy.BusyIndicator);
                i++;
            } while (SaveCart_Warning.Text != "");

            if (SaveCart_YourCartHasBeenSavedTag.GetCssValue("display").Equals("block"))
            {
                return true;
            }
            else { return false; }
        }

        public bool SearchByNameOrSku(String nameorSku)
        {
            PageExtension.WaitForElementPresentAndVisible(this, SearchProductsBar);
            SearchProductsBar.SendKeys(nameorSku);
            WaitPageToLoad();
            if (WebElementExtension.IsWebElementDisplayed(NoProducts))
                {
                Trace.WriteLine("Products not exists. no product to display");
                return false;
                }
            return true;
        }

        public DeliveryAddressPersonalPage_US SetDeliveryAddressPersonal()
        {
            LoadPage(AddressPersonalPath);
            return new DeliveryAddressPersonalPage_US();
        }

        public void WaitPageToLoad()
        {
                Driver.WaitForDocumentReady();
                PageExtension.WaitForElement(this, CommonFindBy.FavoriteIcon);
                PageExtension.WaitForElement(this, CommonFindBy.CartIcon);
                PageExtension.WaitForElementGone(this, CommonFindBy.BusyIndicator);
        }

        public float GetIncrementValuePlusOne()
        {
            this.DoClick(PlusButtonList, 0);
            PageExtension.FluentWait(Driver, 5000, 5000);
            return float.Parse(SelectedProductItemsInputList.ElementAt(0).GetAttribute("value"));
        }

        public float GetIncrementValueMinusOne()
        {
            ClickOnMinusbutton(0);
            return float.Parse(SelectedProductItemsInputList.ElementAt(0).GetAttribute("value"));
        }

        public void ClickOnMinusbutton(int num)
        {
            this.DoClick(MinusButtonList, num);
            Thread.Sleep(400);
        }

        public void ClickOnFavoriteSelected(int num)
        {
            this.DoClick(FavoriteSelected, num);
            WaitForProductToBeInFav();
        }

        public string GetIdClickedOnGrayIcon()
        {
            return GetSkuFavoritedGrayPresent.ElementAt(0).Text;
        }

        public void ClickOnBuyNowButton()
        {
            //ButtonBuyNow.Click();
            this.DoClick(ButtonBuyNow);
            PageExtension.WaitForElementGone(this,CommonFindBy.BusyIndicator);
            Thread.Sleep(400);
        }

        public void ClickOnSaveCart_CancelButton()
        {
            this.DoClick(SaveCart_Cancel);
            PageExtension.WaitForElementGone(this, SaveCart_Overlay);
            Thread.Sleep(400);
        }

        public void ClickOnCancelForLater()
        {
            this.DoClick(SaveCart_Cancel);
            PageExtension.WaitForElementGone(this,SaveCart_Overlay); ;
        }

        public FavoriteProducts GetFavoritePage()
        {
            GoToUrl(FavoritePage);
            WaitPageToLoad();
            return new FavoriteProducts(Driver, RunSettings);
        }
        #endregion

        /// <summary>
        /// Send products to favs. This method only sent the first available element, the first element in FavoriteGray.
        /// </summary>
        /// <param name="numberOfElementsToFav"></param>
        public void SendProductsToFav(int numberOfElementsToFav = 1)
        {
            for (int i = 0; i < numberOfElementsToFav; i++)
            {
                this.DoClick(this.FavoriteGray, 0);
                WaitForProductToBeInFav();
            }
        }


        /// <summary>
        /// After clicking Fav icon, there is change of @class in the fav icon causing some sync issues. The method wait 1 second, per defautl, till all there are no fav with class as hearth-pulsing
        /// </summary>
        /// <param name="timeout"></param>
        public void WaitForProductToBeInFav(int timeout = 1) {
            this.WaitUntil(p => this.FavoriteTransition.Count == 0);
            this.WaitForElementGone(CommonFindBy.BusyIndicator);
            Driver.WaitForDocumentReady();
        }


        /// <summary>
        /// Waits 1 second (as default) to verify that No Product message is shown,  returns true if that is the case.
        /// </summary>
        private bool CheckNoProductLabelDisplayed(int waitTimeForNoProductMsg = 1)
        {
            if (this.WaitForElementPresentAndDisplayed(NoProducts, waitTimeForNoProductMsg))
                return true;
            else
                return false;
        }

        //TODO: ValidateProductsAreDisplayed should be part of a assert class
        /// <summary>
        /// Validate that one or more products are displayed to the user. If no products available it will return an exception
        /// </summary>
        public void ValidateProductsAreDisplayed()
        {
            //TODO: Add one one condition to validate if the table is empty
            if (CheckNoProductLabelDisplayed())
            {
                Assert.Fail($"No Products displayed in {this.Driver.Url}. Check and update the URL and try again.");
            }
        }

        /// <summary>
        /// Do this scrolling to the footer till the displayed product label is the same as total product label
        /// </summary>
        public void LoadAllTheProducts()
        {
            if (!CheckNoProductLabelDisplayed())
            {
                while (Int16.Parse(this.WaitForElementTextToBeDisplayed(DisplayedProducts)) < Int16.Parse(this.WaitForElementTextToBeDisplayed(TotalProducts)))
                {
                    this.ScrollIntoView(this.Single(CommonFindBy.PageFooter));
                    this.Driver.WaitForDocumentReady();
                    PageExtension.WaitForElementGone(this, CommonFindBy.BusyIndicator);
                }
            }

        }

        /// <summary>
        /// Method clicks increment button of one product. Important: method checks only the products that have quantity element, eg: If the first product is not available, it is excluded.
        /// </summary>
        /// <param name="productIndex"></param>
        /// <param name="numberOfProductsToSelect"></param>
        public void SelectItemsOfProductWithIndex(int productIndex, int numberOfProductsToSelect)
        {
            for (int i = 0; i < numberOfProductsToSelect; i++)
            {
                this.DoClick(PlusButtonList, productIndex);
            }
        }

        /// <summary>
        /// Method clicks decrement button of one product. Important: method checks only the products that have quantity element, eg: If the first product is not available, it is excluded.
        /// </summary>
        /// <param name="productIndex"></param>
        /// <param name="numberOfProductsToSelect"></param>
        public void DeselectItemsOfProductWithIndex(int productIndex, int numberOfProductsToSelect)
        {
            for (int i = 0; i < numberOfProductsToSelect; i++)
            {
                this.DoClick(MinusButtonList, productIndex);
            }
        }

        /// <summary>
        /// Navigate to original landing page that is set up in Path variable and for PriceList also clear cart
        /// </summary>
        public override void LoadPage()
        {
            this.GoToUrl(this.Path);
            this.PopulateElements();
            this.Driver.WaitForDocumentReady();
            this.WaitForElementGone(CommonFindBy.BusyIndicator);

            this.DoClick(ClearCart);
        }

        /// <summary>
        /// Click on Show Availability button, the first one available and then wait will the content is displayed.
        /// </summary>
        public void ShowProductAvailability()
        {
            //Click link
            this.DoClick(ShowAvailability);

            //From this element get the content element of Product Availability
            IWebElement productAvailabilityLocator = ShowAvailability.FindElement(By.XPath(AvailabilityContent));

            //Wait till the content is diplayed
            this.WaitUntil(p => (productAvailabilityLocator.GetAttribute("style").Equals("display: block;") || productAvailabilityLocator.GetAttribute("style").Equals("")));
            //display: block;
        }

        /// <summary>
        /// Click on Hide Availability button, the first one available, and then wait will the content is hidden.
        /// </summary>
        public void HideProductAvailability()
        {
            //Click link
            this.DoClick(HideAvailability);
            //From this element get the content element of Product Availability
            IWebElement productAvailabilityLocator = HideAvailability.FindElement(By.XPath(AvailabilityContent));
            //Wait till the content is diplayed
            this.WaitUntil(p => productAvailabilityLocator.GetAttribute("style").Equals("display: none;"));
        }
    }
}