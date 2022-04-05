using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Shop.Automation.Framework;
using Shop.Automation.Pages.Cart;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Shop.Automation.Pages.Catalog
{
    public class FavoriteProducts : Page
    {
        #region Constructors
        public FavoriteProducts()
        { }
        public FavoriteProducts(IWebDriver driver, TestRunSettings runSettings)
        {
            this.Driver = driver;
            this.RunSettings = runSettings;
            PopulateElements();
        }
        #endregion
        #region Mapping
        public By k_loading_mask = By.XPath("//div[contains(@class, 'k-loading-image')]");

        [FindsBy(How = How.Id, Using = "video-tutorial-section")]
        public IWebElement VideoTutorialContainer { get; set; }

        [FindsBy(How = How.XPath, Using = "section[@class='category-content']")]
        public IWebElement categoryContent { get; set; }
        [FindsBy(How = How.XPath, Using = "//ul[@class='breadcrumbs']//a")]
        public IList<IWebElement> Breadcrumbs { get; set; }
        [FindsBy(How = How.XPath, Using = "//a[@wire-model='VideoTutorialViewModel']")]
        public IWebElement VideoTutorialLink { get; set; }
        [FindsBy(How = How.Id, Using = "video-tutorial")]
        public IWebElement VideoTutorial { get; set; }
        [FindsBy(How = How.XPath, Using = "//span[@class='volume label']//following-sibling::span[@class='volume']")]
        public IList<IWebElement> VolumenPointsList { get; set; }
        [FindsBy(How = How.XPath, Using = "//strike[@data-bind='text: Price']")]
        public IList<IWebElement> RetailPriceList { get; set; }
        [FindsBy(How = How.Id, Using = "select-catalog-price-filter")]
        public IWebElement SortByDropdown { get; set; }
        [FindsBy(How = How.Id, Using = "toggle-grid")]
        public IWebElement ToggleGrid { get; set; }
        [FindsBy(How = How.XPath, Using = "//section[@class='product-grid']")]
        public IWebElement SectionProductGrid { get; set; }
        [FindsBy(How = How.XPath, Using = "//section[@class='product-list']")]
        public IWebElement SectionProductList { get; set; }
        [FindsBy(How = How.Id, Using = "toggle-list")]
        public IWebElement ToggleList { get; set; }
        [FindsBy(How = How.XPath, Using = "//a[@class='plus btn-increment']")]
        public IList<IWebElement> PlusBtnList { get; set; }
        [FindsBy(How = How.XPath, Using = "//input[@class='increment']")]
        public IList<IWebElement> IncrementInputList { get; set; }
        [FindsBy(How = How.XPath, Using = "//a[@class='minus btn-increment']")]
        public IList<IWebElement> MinusBtnList { get; set; }
        [FindsBy(How = How.XPath, Using = "//a[@class='product-info']")]
        public IList<IWebElement> ProductNameLinksList { get; set; }
        [FindsBy(How = How.XPath, Using = "//a[@class='btn-add-cart']")]
        public IList<IWebElement> AddtoCartBtnList { get; set; }
        [FindsBy(How = How.XPath, Using = "//div[@class='add-confirm']")]
        public IWebElement VolumenCalculator { get; set; }
        [FindsBy(How = How.XPath, Using = "//span[@data-bind='text: DiscountedSubtotal']")]
        public IWebElement Subtotalcalculator { get; set; }
        [FindsBy(How = How.XPath, Using = "//input[@class='increment']//ancestor::div[@class='add-to-cart']//preceding-sibling::a[@class='product-info']//div[@data-bind='text: YourPrice']")]
        public IList<IWebElement> PriceList { get; set; }
        [FindsBy(How = How.XPath, Using = "//span[@data-bind='text: totalVPStore']")]
        public IWebElement VolumeOnCalculator { get; set; }
        [FindsBy(How = How.XPath, Using = "//input[@class='increment']//ancestor::div[@class='add-to-cart']//preceding-sibling::a[@class='product-info']//span[@data-bind='text: VolumePoints']")]
        public IList<IWebElement> VolumeList { get; set; }
        [FindsBy(How = How.Id, Using = "btn-catalog-view-cart")]
        public IWebElement ViewCartButton { get; set; }
        [FindsBy(How = How.Id, Using = "btn-catalog-go-checkout")]
        public IWebElement ProceedToCheckoutButton { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='product-list-items k-widget k-listview']/div[@class='item']")]
        public IList<IWebElement> FavProductList{ get; set; }

        [FindsBy(How = How.XPath, Using = "//a[contains(@class, 'hearth-pulsing')]")]
        public IList<IWebElement> FavoriteTransition { get; set; }

        [FindsBy(How = How.XPath, Using = "//a[@class='btn-add-cart added']")]
        public IList<IWebElement> AddedtoCartBtnList { get; set; }

        [FindsBy(How = How.XPath, Using = "//input[@class='increment']//ancestor::div[@class='add-to-cart']//preceding-sibling::a[@class='product-info']//div[@class='sku']")]
        public IList<IWebElement> AddToCartElementSKUList { get; set; }

        [FindsBy(How = How.Id, Using = "toggle-grid")]
        public IWebElement btnGridView { get; set; }
        [FindsBy(How = How.Id, Using = "toggle-list")]
        public IWebElement btnListView { get; set; }
        [FindsBy(How = How.XPath, Using = "//h4[@data-bind='visible: NoProducts']")]
        public IWebElement NoProducts { get; set; }
        [FindsBy(How = How.XPath, Using = "//span[@data-bind='text: displayedProducts']")]
        public IWebElement DisplayedProducts { get; set; }
        [FindsBy(How = How.XPath, Using = "//span[@data-bind='text: totalProducts']")]
        public IWebElement TotalProducts { get; set; }
        [FindsBy(How = How.XPath, Using = "//a[@class='favorite-toggle active']/i[@class='icon-heart-fl-2']")]
        public IList<IWebElement> FavoriteSelectedList { get; set; }
        // Product list Section
        //input[@class='increment']//ancestor::div[@class='hl-form']//preceding-sibling::div[@class='sku-info' and @data-bind='text: Sku']
        [FindsBy(How = How.XPath, Using = "//*[@id='subcategory']//a[@class='favorite-toggle active']")]
        public IWebElement lnkHeart { get; set; }
        [FindsBy(How = How.Id, Using = "select-catalog-price-filter")]
        public IWebElement cboSortBy { get; set; }
        [FindsBy(How = How.XPath, Using = "//div[@class='num-of-items']")]
        public IWebElement ShowResultTagBottom { get; set; }
        [FindsBy(How = How.XPath, Using = "//div[@class='sku']")]
        public IList<IWebElement> ProductSKUList { get; set; }

        [FindsBy(How = How.CssSelector, Using = "section.product-list>div>div.item")]
        public IList<IWebElement> ProductElementContainerList { get; set; }

        [FindsBy(How = How.XPath, Using = "//a[contains(@class,'btn-add-cart') and @data-bind]")]
        public IList<IWebElement> AddToCartValidElementsList { get; set; }
        //select-catalog-price-filter

        #region CategorySideBar
        [FindsBy(How = How.XPath, Using = "//section[@class='category-content']/div/div/div/div/ul[@id='category-selecction-ul']")]
        public IWebElement CategoryULContainer { get; set; }

        [FindsBy(How = How.XPath, Using = "//section[@class='category-content']/div/div/div/div/ul[@id='category-selecction-ul']/li[@class='main-category']")]
        public IList<IWebElement> CategoryMainCategoryLinks { get; set; }



        [FindsBy(How = How.XPath, Using = "//section[@class='category-content']/div/div/div/div/ul[@id='category-selecction-ul']/li[@class='main-category']/ul[contains(@class,'subcategory-container')]")]
        public IList<IWebElement> SubCategoryULContainerList{ get; set; }

        [FindsBy(How = How.XPath, Using = "//section[@class='category-content']/div/div/div/div/ul[@id='category-selecction-ul']//a[@class='parent-category']")]
        public IList<IWebElement> CategoryParentLinkList { get; set; }

        [FindsBy(How = How.XPath, Using = "//section[@class='category-content']/div/div/div/div/ul[@id='category-selecction-ul']//a[@href]")]
        public IList<IWebElement> CategoryHRefLinkList { get; set; }

        public string categoryChildULXpathLocator = "./../ul[contains(@class,'subcategory-container')]";
        public string categoryNavigateBackLink = "./../ul//a[@class= 'parent-category navLeft']";

        #endregion

        #endregion Mapping

        #region Path settings
        public override string Path => $"/{RunSettings.Locale}{RunSettings.Catalog_FavoritePage}{RunSettings.ValidUser.Schema}";
        public string pricelist => $"/{RunSettings.Locale}{RunSettings.Catalog_Pricelist}{RunSettings.ValidUser.Schema}";
        public string PathEnergyAndFitness_Hydratation => $"/{RunSettings.Locale}{RunSettings.Catalog_Products_Category}{RunSettings.ValidUser.Schema}/547";
        #endregion

        #region Functionality of Test Cases.
        public List<IWebElement> FillPageElementsList()
        {
            List<IWebElement> PricelistElementsList = new List<IWebElement>();
            PricelistElementsList.Add(btnGridView);
            PricelistElementsList.Add(btnListView);
            PricelistElementsList.Add(cboSortBy);
            return PricelistElementsList;
        }

        /// <summary>
        /// Remove every product in Favorites
        /// </summary>
        public void ResetFavorite()
        {
            bool doWeHaveProducts = ThereAreProductsInTable();
            
            Debug.WriteLine("Reseting Favorites Products");

            if (doWeHaveProducts)
            {
                //Scroll to the footer to refresh the list of products, do it till all the products are displayed
                LoadAllTheProducts();

                //There are some really rare occations where the icon does not changes and we are kept in a loop for that reason use a for and save the number of elements in fav
                int numberOfElementsInFav = FavoriteSelectedList.Count;

                for (int i = 0; i < numberOfElementsInFav; i++)
                {
                    this.DoClick(FavoriteSelectedList[0]);
                    WaitForProductToBeInFav();
                }
            }
            else
            {
                Trace.WriteLine("No elements in fav. ResetFavorite() could not be perfomed.");
            }
            
        }

        /// <summary>
        /// Wait till the Favorite table is loaded and return false it no products where loaded.
        /// </summary>
        /// <returns></returns>
        public bool ThereAreProductsInTable()
        {
            //Favs Products may not have products, which is ok.... so catch the exception and return false
            try
            {
                ValidateProductsAreDisplayed();
            }
            catch (Exception)
            {
                Trace.WriteLine("Exception catched, return false. No products in favs");

                return false;
            }

            return true;
        }

        public int GetTotalFavorite()
        {
            Debug.WriteLine("Getting amount of favorite selected.");
            int totalProducts = int.Parse(this.WaitForElementTextToBeDisplayed(TotalProducts));
            return totalProducts;
        }

        public ProductListPage GoEnergyAndFitness_Hydration()
        {
            Debug.WriteLine("Landing to Energy And Fitness Hydratation");
            GoToUrl(PathEnergyAndFitness_Hydratation);
            PageExtension.WaitForElementGone(this, CommonFindBy.BusyIndicator);
            return new ProductListPage(Driver, RunSettings);
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

        public void SelectSortByOption(string option)
        {
            Debug.WriteLine($"Selecting dropDrown Option -> {option}");
            SortByDropdown.SelectDropDownOption(option);
            PageExtension.WaitForElementGone(this, CommonFindBy.BusyIndicator);
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
            while (Int16.Parse(DisplayedProducts.Text) < Int16.Parse(TotalProducts.Text))
            {
                if (WebElementExtension.IsWebElementDisplayed(element))
                {
                    return true;
                }
                else
                {
                    PopulateElements();
                    ScrollIntoView(ShowResultTagBottom);
                    Driver.WaitForDocumentReady();
                    PageExtension.WaitForElementGone(this, CommonFindBy.BusyIndicator);
                }
            }
            return false;
        }

        public bool CheckingElementInList(String SkuNumber)
        {
            bool found = false;

            found = ProductSKUList.Count.Equals(null) ? true : false; 

            foreach (IWebElement item in ProductSKUList)
            {
                if (item.Text.Contains(SkuNumber))
                {
                    found = true;
                }
            }
            return found;
        }


        //TODO:Create a set of methods that can be used (inherited?) for all the methods that send to a page in catalog  using the Path variable in the page class
        public Pricelist GoPricelistPage()
        {
            Pricelist priceListPage = new Pricelist(Driver, RunSettings);
            priceListPage.LoadPage();
            priceListPage.PopulateElements();

            return priceListPage;
        }

        public ProductListPage GoToProductlistPage()
        {
            ProductListPage productListPage = new ProductListPage(Driver, RunSettings);
            productListPage.LoadPage();
            productListPage.PopulateElements();

            return productListPage;
        }

        /// <summary>
        /// Navigate to Shop cart page.
        /// </summary>
        /// <returns></returns>
        public ShoppingCartPage GoToCart()
        {
            ShoppingCartPage shopCartPage = new ShoppingCartPage(Driver, RunSettings);
            shopCartPage.LoadPage();
            shopCartPage.PopulateElements();

            return shopCartPage;
        }
        #endregion

        /// <summary>
        /// Waits 1 second to verify that No Product message is shown
        /// </summary>
        private bool IsNoProductLabelDisplayed(int waitTimeForNoProductMsg = 2)
        {
            if (this.WaitForElementPresentAndDisplayed(NoProducts, waitTimeForNoProductMsg))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Validate that one or more products are displayed to the user. If no products available it will return an exception
        /// </summary>
        public void ValidateProductsAreDisplayed()
        {
            //Check if the No Products labels is displayed
            if (IsNoProductLabelDisplayed())
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
            if (!IsNoProductLabelDisplayed())
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
        /// Check if we have products products in favs. Return true if the number of products is greater or equal to the parameter. It also waits 5 second, per defautl, to have a table with more than one element
        /// </summary>
        /// <returns></returns>
        public bool ThereAreFavoriteProducts(int expectedNumbeOfProducts = 1)
        {
            bool expectedNumberInPage = false;

            if (ThereAreProductsInTable())
            {
                LoadAllTheProducts();
                expectedNumberInPage = ValidateNumberOfElements(FavProductList, expectedNumbeOfProducts);
            }

            return expectedNumberInPage;
        }

        /// <summary>
        /// Return true or false if the SKU in the paramater is found in Fav list.
        /// </summary>
        /// <param name="skuId"></param>
        /// <returns></returns>
        public bool IsSKUInFavTable(string skuId)
        {
            LoadAllTheProducts();

            if (ProductSKUList.Where(p => p.Text.Trim().Equals(skuId)).Count() == 0)
            {
                return false;
            }
            else
            {
                return true;
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
        /// Wait ofr Volume and Subtotal banner is visible. If visible move the element to be visible.
        /// </summary>
        public void WaitForVolumeBanner()
        {
            if (this.WaitForElementPresentAndVisible(VolumenCalculator))
            {
                this.ScrollElementBelowBanner(VolumenCalculator);
            }
           
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
        /// Return true if all the links in CategoryMainCategoryLinks are visibles
        /// </summary>
        /// <returns></returns>
        public bool CheckMainCategoryLinksAreVisible()
        {
            foreach (IWebElement mainLink in CategoryMainCategoryLinks)
            {
                if(!CommonMethods.IsElementDisplayed(mainLink) && !CommonMethods.IsElementEnabled(mainLink))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Check if the UL list with all the subcategory links are visible to the user. The method needs the Main Category Link to get the UL related to it.
        /// </summary>
        /// <param name="MainCategoryLink"></param>
        /// <returns></returns>
        public bool CheckSubCategoryULIsVisible(IWebElement MainCategoryLink)
        {
            IWebElement ChildreULList = MainCategoryLink.FindElement(By.XPath(categoryChildULXpathLocator));

            this.ScrollElementBelowBanner(ChildreULList);

            return CommonMethods.IsElementDisplayed(ChildreULList) && CommonMethods.IsElementEnabled(ChildreULList);
        }
    }

}
