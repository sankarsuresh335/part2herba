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

namespace Shop.Automation.Pages.Catalog
{

    public class ProductListPage : Page
    {
        #region Constructors
        public ProductListPage() { }
        public ProductListPage(IWebDriver driver, TestRunSettings runSettings)
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
        #region Sort Region
        [FindsBy(How = How.Id, Using = "Adobe-TC-ProductGroupTitleAbove")]
        public IWebElement SubcategoryTitle { get; set; }
        [FindsBy(How = How.Id, Using = "select-catalog-price-filter")]
        public IWebElement SortByDropdown { get; set; }
        [FindsBy(How = How.XPath, Using = "//div[@id='subcategory']")]
        public IWebElement ProductListViewModelFrame { get; set; }
        #endregion

        #region Product List Items
        [FindsBy(How = How.CssSelector, Using = "section.product-list>div>div.item")]
        public IList<IWebElement> ProductElementContainerList { get; set; }
        [FindsBy(How = How.XPath, Using = "//section[@class='product-grid']")]
        public IWebElement SectionProductGrid { get; set; }
        [FindsBy(How = How.Id, Using = "toggle-grid")]
        public IWebElement ToggleGrid { get; set; }
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
        [FindsBy(How = How.XPath, Using = "//div[@class='num-of-items']")]
        public IWebElement ShowResultTagBottom { get; set; }
        [FindsBy(How = How.XPath, Using = "//a[@class='favorite-toggle']/i[@class='icon-heart-fl-2']")]
        public IList<IWebElement> FavoriteGrayList { get; set; }
        [FindsBy(How = How.XPath, Using = "//a[@class='favorite-toggle active']/i[@class='icon-heart-fl-2']")]
        public IList<IWebElement> FavoriteSelectedList { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='sku']")]
        public IList<IWebElement> SkuNumberList { get; set; }

        [FindsBy(How = How.XPath, Using = "//a[contains(@class, 'hearth-pulsing')]")]
        public IList<IWebElement> FavoriteTransition { get; set; }

        [FindsBy(How = How.XPath, Using = "//strike[@data-bind='text: Price']")]
        public IList<IWebElement> RetailPriceList { get; set; }
        [FindsBy(How = How.XPath, Using = "//div[@data-bind='text: YourPrice']")]
        public IList<IWebElement> YourPriceList { get; set; }
        [FindsBy(How = How.XPath, Using = "//span[@class='volume label']//following-sibling::span[@class='volume']")]
        public IList<IWebElement> VolumenPointsList { get; set; }
        [FindsBy(How = How.XPath, Using = "//span[@class='earnbase label']//following-sibling::span[@class='earnbase']")]
        public IList<IWebElement> EarnBaseList { get; set; }
        [FindsBy(How = How.XPath, Using = "//a[@class='minus btn-increment']")]
        public IList<IWebElement> MinusBtnList { get; set; }
        [FindsBy(How = How.XPath, Using = "//a[@class='plus btn-increment']")]
        public IList<IWebElement> PlusBtnList { get; set; }
        [FindsBy(How = How.XPath, Using = "//input[@class='increment']")]
        public IList<IWebElement> IncrementInputList { get; set; }

        [FindsBy(How = How.XPath, Using = "//a[@class='btn-add-cart']")]
        public IList<IWebElement> AddtoCartBtnList { get; set; }

        [FindsBy(How = How.XPath, Using = "//a[contains(@class,'btn-add-cart') and @data-bind]")]
        public IList<IWebElement> AddToCartValidElementsList { get; set; }

        [FindsBy(How = How.XPath, Using = "//a[@class='btn-add-cart added']")]
        public IList<IWebElement> AddedtoCartBtnList { get; set; }

        [FindsBy(How = How.XPath, Using = "//span[@class='badge-counter']//parent::a")]
        public IWebElement Cartbtn { get; set; }
        #endregion

        #region [this section get element if increment input is displayed.]
        [FindsBy(How = How.XPath, Using = "//input[@class='increment']//ancestor::div[@class='add-to-cart']//preceding-sibling::a[@class='product-info']//div[@data-bind='text: YourPrice']")]
        public IList<IWebElement> PriceList { get; set; }

        [FindsBy(How = How.XPath, Using = "//input[@class='increment']//ancestor::div[@class='add-to-cart']//preceding-sibling::a[@class='product-info']//span[@data-bind='text: VolumePoints']")]
        public IList<IWebElement> VolumeList { get; set; }

        [FindsBy(How = How.XPath, Using = "//input[@class='increment']//ancestor::div[@class='add-to-cart']//preceding-sibling::a[@class='product-info']//div[@class='sku']")]
        public IList<IWebElement> AddToCartElementSKUList { get; set; }

        #endregion [this section get element if increment input is displayed.]																   


        [FindsBy(How = How.XPath, Using = "//h4[@data-bind='visible: NoProducts']")]
        public IWebElement NoProducts { get; set; }
        [FindsBy(How = How.XPath, Using = "//span[contains(@data-bind,'totalProducts')]")]
        public IList<IWebElement> TotalProducts { get; set; }

        [FindsBy(How = How.XPath, Using = "//span[@data-bind='text: displayedProducts']")]
        public IWebElement DisplayedProducts { get; set; }
        [FindsBy(How = How.XPath, Using = "//div[@class='k-widget k-window modal-order-purpose k-state-focused']")]
        public IWebElement SelectAnOrderPurpose { get; set; }
        public warehouse CurrentWarehouse { get; set; }

        #region Left Content - Filter Section
        [FindsBy(How = How.XPath, Using = "//div[@class='left-content']//li[@class='flavor']//label[@class='title']//following-sibling::label")]
        public IList<IWebElement> FlavorLabel { get; set; }
        [FindsBy(How = How.XPath, Using = "//div[@class='left-content']//input[@name='flavor']")]
        public IList<IWebElement> FlavorInput { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='left-content']//input[@name='flavor']")]
        public IWebElement FlavorInputTest { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='left-content']//li[@class='size']//label[@class='title']//following-sibling::label")]
        public IList<IWebElement> SizeLabel { get; set; }
        [FindsBy(How = How.XPath, Using = "//div[@class='left-content']//input[@name='size']")]
        public IList<IWebElement> SizeInput { get; set; }
        [FindsBy(How = How.XPath, Using = "//div[@class='left-content']//li[@class='ingredients']//label[@class='title']//following-sibling::label")]
        public IList<IWebElement> IngredientLabel { get; set; }
        [FindsBy(How = How.XPath, Using = "//div[@class='left-content']//input[@name='ingredients']")]
        public IList<IWebElement> IngredientInput { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='left-content']//li[@class='color']//label[@class='title']//following-sibling::label")]
        public IList<IWebElement> ColorLabel { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='left-content']//input[@name='color']")]
        public IList<IWebElement> ColorInput { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='left-content']//input[@name='color']")]
        public IWebElement ColorInputTest { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='left-content']//input[@name='Gender']")]
        public IList<IWebElement> GenderInput { get; set; }
        [FindsBy(How = How.Id, Using = "resetFilter-btn")]
        public IWebElement ResetFilterBtn { get; set; }
        [FindsBy(How = How.XPath, Using = "//label[contains(text(),Price)]//following-sibling::div[@class='k-widget k-slider k-slider-horizontal price-range k-state-default']//div[@class='k-slider-track']")]
        public IWebElement PriceSliderTrack { get; set; }
        [FindsBy(How = How.XPath, Using = "//label[contains(text(),Price)]//following-sibling::div[@class='k-widget k-slider k-slider-horizontal price-range k-state-default']//div[@class='k-slider-track']//a[1]")]
        public IList<IWebElement> PriceSliderHandle { get; set; }
        #endregion

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
        //public override string Path => $"/{RunSettings.Locale}{RunSettings.Catalog_Product_List}{RunSettings.ValidUser.Schema}/{RunSettings.CatalogCategory.Category_ProductList}";
        public override string Path => $"/{RunSettings.Locale}{RunSettings.Catalog_Product_List}{RunSettings.ValidUser.Schema}/{RunSettings.CatalogCategory.Category_ProductList}";
        public string AddressPersonalPath => RunSettings.Url.ToString() + RunSettings.Locale + RunSettings.API_Address_Delivery + "personal/" + RunSettings.ValidUser.Schema;
        public string PathFavorite => $"/{RunSettings.Locale}{RunSettings.Catalog_FavoritePage}";
        private string PathCart => $"{RunSettings.Locale}{RunSettings.Cart_URL}{RunSettings.ValidUser.Schema}{RunSettings.CartOrderCategory_RSO}";
        #endregion

        #region Functionality of Test Cases.

        public IWebElement TotalProductsGrouped()
        {
            foreach (IWebElement total in TotalProducts)
            {
                if (total.Displayed)
                {
                    return total;
                }
            }

            return TotalProducts[0];
        }

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

        public IList<double> GetProductListValuesVi(IList<IWebElement> listItems)
        {
            Debug.WriteLine("Creating List of webElements");
            var ret = new List<double>();
            foreach (IWebElement item in listItems)
            {
                string item2 = item.Text;
                item2 = item2.Replace(".", "-");
                item2 = item2.Replace(",", ".");
                item2 = item2.Replace("-", ",");
                Debug.WriteLine(item2);
                double value = Convert.ToDouble(item2);
                ret.Add(value);
            }
            return ret;
        }

        /// <summary>
        /// Scrool until all the elements has been shown in the list and validate that the label of total number of products is equals to the total number of elements displayed
        /// </summary>
        /// <returns></returns>
        public bool ScrollingDownByResults()
        {
            ValidateProductsAreDisplayed();

            int Showing = int.Parse(this.WaitForElementTextToBeDisplayed(DisplayedProducts));
            int TotalProducts = int.Parse(this.WaitForElementTextToBeDisplayed(TotalProductsGrouped()));

            do
            {
                //Get the number of elements currently being display scroll the last element to the top of the window in order to refresh the number of elements
                ScrollIntoView(ProductElementContainerList[Showing - 1]);

                //Wait till the product list is refreshed
                PageExtension.WaitForElementGone(this, CommonFindBy.BusyIndicator);
                this.Driver.WaitForDocumentReady();

                Showing = int.Parse(this.WaitForElementTextToBeDisplayed(DisplayedProducts));
            } while (Showing < TotalProducts);

            //Validate that the number of products matches the total products label
            if (ProductElementContainerList.Count == TotalProducts)
                return true;
            else
                return false;
        }

        public bool ValidationFilter(IList<IWebElement> input, IList<IWebElement> label)
        {

            if (input.Count > 0)
            {
                for (int i = 0; i < input.Count; i++)
                {
                    Debug.WriteLine("Reseting Filters...");
                    ResetFilters();
                    string num = label.ElementAt(i).Text;
                    PageExtension.WaitForElementGone(this, CommonFindBy.BusyIndicator);
                    var number = Regex.Match(num, @"^.*\((.*?)\)[^\(]*$").Groups[1].Value;
                    Debug.WriteLine($"Clicking on Filter {label.ElementAt(i).Text}");
                    this.DoClick(input, i);
                    PageExtension.WaitForElementGone(this, CommonFindBy.BusyIndicator);
                    Debug.WriteLine($"validating Filter");
                    if (ProductNameByList.Count != int.Parse(number) && TotalProductsGrouped().Equals(int.Parse(number)))
                    {
                        return false;
                    }
                    PopulateElements();
                }
            }
            return true;
        }

        public bool ValidationFilterMoreThanOneProduct(IList<IWebElement> input, IList<IWebElement> label)
        {
            //Check that we have more at least 2 options in the filter to perform the test.
            if (input.Count < 2)
            {
                throw new Exception("In order to perform this test the filter should have more than 1 option.");
            }

            //Reset the filters
            Debug.WriteLine("Reseting Filters...");
            this.DoClick(ResetFilterBtn);
            PageExtension.WaitForElementGone(this, CommonFindBy.BusyIndicator);

            //Get 2 random numbers, this represent the 2 indexes of the filter to select
            var random = GetTwoValuesDifferentRandom(1, input.Count);

            //From the label, get the number of products and sum both to get the expected result
            int expectedNumberOfElements = int.Parse(Regex.Match(label.ElementAt(random.Item1).Text, @"^.*\((.*?)\)[^\(]*$").Groups[1].Value);
            expectedNumberOfElements = expectedNumberOfElements + int.Parse(Regex.Match(label.ElementAt(random.Item2).Text, @"^.*\((.*?)\)[^\(]*$").Groups[1].Value);

            //Click the first filter option
            Debug.WriteLine($"Clicking on Filter {label.ElementAt(random.Item1).Text}");
            //input[random.Item1].Click();
            this.DoClick(input[random.Item1]);
            PageExtension.WaitForElementGone(this, CommonFindBy.BusyIndicator);

            //Click the first filter option
            Debug.WriteLine($"Clicking on Filter {label.ElementAt(random.Item2).Text}");
            this.DoClick(input[random.Item2]);
            PageExtension.WaitForElementGone(this, CommonFindBy.BusyIndicator);

            PopulateElements();

            //Validate
            if (ProductNameByList.Count != expectedNumberOfElements || TotalProductsGrouped().Equals(expectedNumberOfElements))
            {
                return false;
            }

            return true;
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

        public void ResetFilters()
        {
            Debug.WriteLine("Reseting Filters....");
            this.DoClick(ResetFilterBtn);
            PageExtension.WaitForElementGone(this, CommonFindBy.BusyIndicator);
        }

        /// <summary>
        /// Call ValidateFilterInputsAreDeselected() for all the filters, which will validate that all the inputs are deselected.
        /// </summary>
        /// <returns></returns>
        public bool ValidateAllTheFiltersWereReseted()
        {
            ValidateFilterInputsAreDeselected(FlavorInput);
            ValidateFilterInputsAreDeselected(SizeInput);
            ValidateFilterInputsAreDeselected(IngredientInput);
            ValidateFilterInputsAreDeselected(GenderInput);
            ValidateFilterInputsAreDeselected(ColorInput);

            return true;
        }

        /// <summary>
        /// Check that all the inputs in a specific List of elements is deselected. Throws an error if on element is selected.
        /// </summary>
        /// <param name="filter"></param>
        private void ValidateFilterInputsAreDeselected(IList<IWebElement> filter)
        {
            if (filter.Count > 0)
            {
                foreach (IWebElement element in filter)
                {
                    if (element.Selected)
                    {
                        throw new Exception("One Input was not deselected.");
                    }
                }
            }
        }


        /// <summary>
        /// Select all the checkboxes from all the filters
        /// </summary>
        public void SelectAllInputsInFilters()
        {
            //Validate products are displayed
            ValidateProductsAreDisplayed();

            //Unselect of the checkboxes from all the filters
            SelectAllCheckBoxesInFilter(FlavorInput);
            SelectAllCheckBoxesInFilter(SizeInput);
            SelectAllCheckBoxesInFilter(IngredientInput);
            SelectAllCheckBoxesInFilter(GenderInput);
            SelectAllCheckBoxesInFilter(ColorInput);
        }

        /// <summary>
        /// Click all the inputs in the IList
        /// </summary>
        /// <param name="filter"></param>
        private void SelectAllCheckBoxesInFilter(IList<IWebElement> filter)
        {
            if (filter.Count > 0)
            {
                Debug.WriteLine("Selecting inputs Flavor");
                foreach (IWebElement element in filter)
                {
                    if (!element.Selected)
                    {
                        this.DoClick(element);
                        PageExtension.WaitForElementGone(this, CommonFindBy.BusyIndicator);
                    }
                }
            }
            else
            {
                Debug.WriteLine($"No inputs to click in filter{filter.ToString()}");
            }
        }


        /// <summary>
        /// Click Fav icon of all the products ther are not part of Favorites
        /// </summary>
        public int SentAllItemsToFav()
        {
            //Before performing the test, validate that at least one product is displayed
            ValidateProductsAreDisplayed();

            //Load all the products
            LoadAllTheProducts();

            //Save the number of product are are not part of Favs
            int numberOfItemsAddedToFav = this.FavoriteGrayList.Count;

            //Select every product to favs
            while (FavoriteGrayList.Count != 0)
            {
                this.DoClick(FavoriteGrayList[0]);
                WaitForProductToBeInFav();
                this.Driver.WaitForDocumentReady();
            }

            return numberOfItemsAddedToFav;
        }

        /// <summary>
        /// Click Fav icon of all the products ther are not part of Favorites
        /// </summary>
        public int SentItemsToFav(int numberOfItemsToAddToFav)
        {
            //Before performing the test, validate that at least one product is displayed
            ValidateProductsAreDisplayed();

            //Load all the products
            LoadAllTheProducts();

            //Check if productcs can be sent to favs
            if (FavoriteGrayList.Count < numberOfItemsToAddToFav)
            {
                throw new Exception($"There are not enough products to sent {numberOfItemsToAddToFav} to Favs. Available Products {FavoriteGrayList.Count}");
            }

            for (int i = 0; i < numberOfItemsToAddToFav; i++)
            {
                this.DoClick(FavoriteGrayList[0]);
                WaitForProductToBeInFav();
                Driver.WaitForDocumentReady();
            }

            return numberOfItemsToAddToFav;
        }

        /// <summary>
        /// Clicks increment button a certain number of times indicated in the first parameter. Second parameter is optional and indicates the index of the element of the list.
        /// </summary>
        /// <param name="numberOfClicks"></param>
        /// <param name="elementInList"></param>
        public void ClickIncrementButton(int numberOfClicks, int elementInList = 0)
        {
            for (int i = 0; i < numberOfClicks; i++)
            {
                Debug.WriteLine("Clicking on Increment Button on Quantity Control");
                this.PlusBtnList.ElementAt(elementInList).Click();
            }
        }

        /// <summary>
        /// Clicks decrement button a certain number of times indicated in the first parameter. Second parameter is optional and indicates the index of the element of the list.
        /// </summary>
        /// <param name="numberOfClicks"></param>
        /// <param name="elementInList"></param>
        public void ClickDecrementButton(int numberOfClicks, int elementInList = 0)
        {
            for (int i = 0; i < numberOfClicks; i++)
            {
                Debug.WriteLine("Clicking on Increment Button on Quantity Control");
                this.MinusBtnList.ElementAt(elementInList).Click();
            }
        }

        public void WaitPageToLoad()
        {
            try
            {
                Driver.WaitForDocumentReady();
                PageExtension.WaitForElementPresentAndVisible(this, SectionProductList);
                PageExtension.WaitUntil(this, b => AssertHelpers.WebElementExists(NoProducts) == true);
                PageExtension.WaitUntil(this, b => AssertHelpers.WebElementExists(ResetFilterBtn) == true);
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
        public ShoppingCartPage GoToCart()
        {
            Debug.WriteLine("Landing to Cart Page");
            GoToUrl(PathCart);
            PageExtension.WaitForElementGone(this, CommonFindBy.BusyIndicator);
            return new ShoppingCartPage(Driver, RunSettings);
        }

        /// <summary>
        /// Navigate to an product category. Parameter should be an integer that is concatenated to the URL: /Shop/Catalog/Items/Search/Ds/546
        /// </summary>
        /// <param name="catalogCategory"></param>
        /// <returns></returns>
        public Pricelist GoToSpecificPriceListCategory(int catalogCategory)
        {
            string partialURL = $"/{RunSettings.Locale}{RunSettings.Catalog_Product_List}{RunSettings.ValidUser.Schema}/{catalogCategory}";
            GoToUrl(partialURL);
            this.Driver.WaitForDocumentReady();
            PageExtension.WaitForElementGone(this, CommonFindBy.BusyIndicator);
            return new Pricelist(Driver, RunSettings);
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
        
        /// <summary>
        /// Validate that one or more products are displayed to the user checking if No Product Label is shown and wait till the table contains more than one element in the table visible. If no products available it will return an exception
        /// </summary>
        public void ValidateProductsAreDisplayed()
        {
            //Check if the No Products labels is displayed
            if (CheckNoProductLabelDisplayed())
            {
                throw new Exception($"No Products displayed in {this.Driver.Url}. Check and update the URL and try again.");
            }

            int timeOut = 5;
            //Wait 5 seconds to have the table displayed
            if (!this.WaitUntil(p => (ProductElementContainerList.Count > 0 && ProductElementContainerList[0].Displayed), timeOut))
            {
                throw new Exception($"The table was not loaded in the indicated time of {timeOut}");
            }
            
        }

        /// <summary>
        /// Do this scrolling to the footer till the displayed product label is the same as total product label
        /// </summary>
        public void LoadAllTheProducts()
        {
            if (!CheckNoProductLabelDisplayed())
            {
                while (Int16.Parse(this.WaitForElementTextToBeDisplayed(DisplayedProducts)) < Int16.Parse(this.WaitForElementTextToBeDisplayed(TotalProductsGrouped())))
                {
                    this.ScrollIntoView(this.Single(CommonFindBy.PageFooter));
                    this.Driver.WaitForDocumentReady();
                    PageExtension.WaitForElementGone(this, CommonFindBy.BusyIndicator);
                }
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
                    string skuToRemove = this.AddToCartElementSKUList.ElementAt(i).Text;

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
        /// After clicking Fav icon, there is change of @class in the fav icon causing some sync issues. The method wait 1 second, per defautl, till all there are no fav with class as hearth-pulsing
        /// </summary>
        /// <param name="timeout"></param>
        public void WaitForProductToBeInFav(int timeout = 1)
        {
            this.WaitUntil(p => this.FavoriteTransition.Count == 0);
            this.WaitForElementGone(CommonFindBy.BusyIndicator);
            Driver.WaitForDocumentReady();
        }

        /// <summary>
        /// Wait ofr Volume and Subtotal banner is visible. If visible move the element to be visible.
        /// </summary>
        public void WaitForVolumeBanner()
        {
            if (this.WaitForElementPresentAndVisible(VolumenCalculator))
            {
                this.ScrollElementBelowBanner(VolumenCalculator);
            }

        }
    }
}
