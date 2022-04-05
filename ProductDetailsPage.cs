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
using OpenQA.Selenium.Interactions;

namespace Shop.Automation.Pages.Catalog
{
    public class ProductDetailsPage : Page
    {
        #region Constructors
        public ProductDetailsPage()
        { }
        public ProductDetailsPage(IWebDriver driver, TestRunSettings runSettings)
        {
            this.Driver = driver;
            this.RunSettings = runSettings;
            PopulateElements();
        }
        #endregion
        #region Mapping
        [FindsBy(How = How.XPath, Using = "//ul[@class='breadcrumbs']//a")]
        public IList<IWebElement> Breadcrumbs { get; set; }
        [FindsBy(How = How.XPath, Using = "//h2[@class='title']")]
        public IWebElement Title { get; set; }
        [FindsBy(How = How.ClassName, Using = "icon-heart-fl-2")]
        public IWebElement FavoritesIcon { get; set; }
        [FindsBy(How = How.XPath, Using = "//i[@class='icon-heart-fl-2']//parent::a")]
        public IWebElement ObtainFavorite { get; set; }
        [FindsBy(How = How.XPath, Using = "//a[@class='favorite-toggle']/i[@class='icon-heart-fl-2']")]
        public IWebElement FavoriteGray { get; set; }
        [FindsBy(How = How.XPath, Using = "//a[@class='favorite-toggle active']/i[@class='icon-heart-fl-2']")]
        public IWebElement FavoriteSelected { get; set; }
        [FindsBy(How = How.XPath, Using = "//div[@class='sku']/span[1]")]
        public IWebElement SkuNumber { get; set; }
        [FindsBy(How = How.XPath, Using = "//a[@href='#product-information']")]
        public IWebElement MoreDetailsLink { get; set; }
        [FindsBy(How = How.XPath, Using = "//a[@data-bind='visible: LabelPDF, attr: {href: LabelPDF}']")]
        public IWebElement IngredientsLink { get; set; }
        [FindsBy(How = How.XPath, Using = "//div[@class='zoomContainer']")]
        public IWebElement Zoomcontainer { get; set; }
        [FindsBy(How = How.XPath, Using = "//strike[@data-bind='text: Price, visible: IsShowingStrikedPrice']")]
        public IWebElement RetailPrice { get; set; }
        [FindsBy(How = How.XPath, Using = "//span[@data-bind='text: YourPrice']")]
        public IWebElement YourPrice { get; set; }
        [FindsBy(How = How.XPath, Using = "//span[@data-bind='text: VolumePoints']")]
        public IWebElement VolumePoints { get; set; }
        [FindsBy(How = How.XPath, Using = "//span[@data-bind='text: EarnBase']")]
        public IWebElement EarnBase { get; set; }
        [FindsBy(How = How.XPath, Using = "//a[@class='minus btn-increment']")]
        public IWebElement MinusBtn { get; set; }
        [FindsBy(How = How.XPath, Using = "//a[@class='plus btn-increment']")]
        public IWebElement PlusBtn { get; set; }
        [FindsBy(How = How.XPath, Using = "//input[@class='increment']")]
        public IWebElement IncrementInput { get; set; }
        [FindsBy(How = How.XPath, Using = "//h4[@class='sold-out']")]
        public IWebElement SoldOut { get; set; }
        [FindsBy(How = How.XPath, Using = "//a[@id='availability']")]
        public IWebElement AvailabilityInSelectWarehouses { get; set; }
        [FindsBy(How = How.XPath, Using = "//a[@class='btn-add-cart-large']")]
        public IWebElement AddtoCartBtn { get; set; }
        [FindsBy(How = How.XPath, Using = "//ul[@id='myVariations']")]
        public IList<IWebElement> Myvariations { get; set; }
        [FindsBy(How = How.XPath, Using = "//ul[@class='thumbs']/li/img[not(@class='video')]")]
        public IList<IWebElement> Thumbs { get; set; }
        [FindsBy(How = How.XPath, Using = "//span[@data-bind='click: ChangeImgSrc']")]
        public IWebElement ChangeImgSrc { get; set; }
        [FindsBy(How = How.XPath, Using = "//div[@class='zoomWindow']")]
        public IWebElement ZoomWindow { get; set; }
        [FindsBy(How = How.XPath, Using = "//div[@class='k-widget k-window modal-order-purpose k-state-focused']")]
        public IWebElement SelectAnOrderPurpose { get; set; }

        [FindsBy(How = How.XPath, Using = "//a[contains(@class, 'hearth-pulsing')]")]
        public IList<IWebElement> FavoriteTransition { get; set; }


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

        [FindsBy(How = How.XPath, Using = "//ul[@class='Flavor']//li")]
        public IList<IWebElement> FlavorThumbs { get; set; }
        [FindsBy(How = How.XPath, Using = "//ul[@class='Size']//li")]
        public IList<IWebElement> SizeThumbs { get; set; }
        By flavorThumbs2 = By.XPath("//ul[@class='Flavor']//li");



        #region Product Overview
        [FindsBy(How = How.Id, Using = "info-toggle")]
        public IWebElement InfoToggle { get; set; }
        [FindsBy(How = How.XPath, Using = "//section[@class='product-content']")]
        public IList<IWebElement> ProductContent { get; set; }
        #endregion

        #endregion

        #region VideoThumbs
        [FindsBy(How = How.Id, Using = "videoContainer")]
        public IWebElement ThumbVideoContainer { get; set; }

        [FindsBy(How = How.XPath, Using = "//ul[@class='thumbs']/li/img[@class='video']")]
        public IList<IWebElement> VideoThumbsList { get; set; }

        [FindsBy(How = How.Id, Using = "myPlayerID")]
        public IWebElement VideoPlayer { get; set; }
        #endregion

        #region Path Settings
        public override string Path => $"/{RunSettings.Locale}{RunSettings.Catalog_ProductDetail}{RunSettings.ValidUser.Schema}/{RunSettings.CatalogCategory.ProductDetails_SKU}";
        public string AddressPersonalPath => RunSettings.Url.ToString() + RunSettings.Locale + RunSettings.API_Address_Delivery + "personal/" + RunSettings.ValidUser.Schema;
        public string PathFavorite => $"/{RunSettings.Locale}/Shop/Catalog/Items/Favorites/DS";
        private string PathCart => $"{RunSettings.Locale}{RunSettings.Cart_URL}Ds?{RunSettings.CartOrderCategory_RSO}";
        #endregion

        #region Functionality of Test Cases.
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

        /// <summary>
        /// Check that all the videos in the thumbnail are displayed.
        /// </summary>
        /// <returns></returns>
        public Tuple<bool, string> CheckingVideoThumbs()
        {
            string videoId = "";

            foreach (var videoThumb in VideoThumbsList)
            {
                //save the id the video to do a validation
                videoId = videoThumb.GetAttribute("data-videoid");

                //Use action class to perform a click.
                this.MoveToElementAndClick(videoThumb);

                //check that the video is displayed
                if (this.WaitForElementPresentAndDisplayed(ThumbVideoContainer, 3))
                {
                    if (!videoId.Equals(VideoPlayer.GetAttribute("data-video-id")))
                    {
                        return Tuple.Create(false, "Video displayed does not matched with the one in the thumbs.");
                    }
                }
                else
                {
                    return Tuple.Create(false, "Video was not displayed.");
                }

            }

            return Tuple.Create(true, "All was ok.");
        }

        public void ClickOnFavoriteIcon()
        {
            FavoritesIcon.Click();
            WaitForProductToBeInFav();
        }

        public float GetIncrementValuePlusOne()
        {
            PlusBtn.Click();
            return float.Parse(IncrementInput.GetAttribute("value"));
        }

        public float GetIncrementValueMinusOne()
        {
            MinusBtn.Click();
            Thread.Sleep(1000);
            return float.Parse(IncrementInput.GetAttribute("value"));
        }

        public void ClicOnPlusbutton()
        {
            PlusBtn.Click();
            WaitPageToLoad();
        }

        public void ClicAddToCart()
        {
            Debug.WriteLine("Clicking on AddtocartBtn");
            AddtoCartBtn.Click();
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
                PageExtension.WaitUntil(this, b => AssertHelpers.WebElementExists(ObtainFavorite) == true);
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

        public FavoriteProducts GoToFavoriteProducts()
        {
            Debug.WriteLine("Landing to Energy And Fitness Hydratation");
            GoToUrl(PathFavorite);
            PageExtension.WaitForElementGone(this, CommonFindBy.BusyIndicator);
            return new FavoriteProducts(Driver, RunSettings);
        }

        public void WaitToLoadProductDetails()
        {
            this.WaitForElementPresentAndVisible(SkuNumber);
            this.WaitForElementPresentAndVisible(ProductContent.LastOrDefault());
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
        public ShoppingCartPage GoToCart()
        {
            Debug.WriteLine("Landing to Cart Page");
            GoToUrl(PathCart);
            PageExtension.WaitForElementGone(this, CommonFindBy.BusyIndicator);
            return new ShoppingCartPage(Driver, RunSettings);
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
        /// Navigate to Shop Cart page and click clear cart btn
        /// </summary>
        public void ClearCartManually()
        {
            ShoppingCartPage sc = GoToCart();

            string elementDisable = sc.SC_ClearCartButton.GetAttribute("disabled") ?? "false";

            if (!elementDisable.Equals("true"))
            {
                sc.MoveToElementAndClick(sc.SC_ClearCartButton);
                sc.WaitCartPageToLoad();
                sc.PopulateElements();
            }
        }
        #endregion
    }
}
