using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Shop.Automation.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Shop.Automation.Pages.Catalog
{
    public class CategoryPage : Page
    {
        #region Constructors
        public CategoryPage()
        { }
        public CategoryPage(IWebDriver driver, TestRunSettings runsettings)
        {
            this.Driver = driver;
            this.RunSettings = runsettings;
            PopulateElements();
        }
        #endregion
        #region Mapping
        //Breadcrumbs
        [FindsBy(How = How.XPath, Using = "//ul[@class='breadcrumbs']//a")]
        public IList<IWebElement> BreadCrumbsLinks { get; set; }
        [FindsBy(How = How.XPath, Using = "//div[@class='left-content']/h3")]
        public IList<IWebElement> SubcategoryNameTag { get; set; }
        [FindsBy(How = How.XPath, Using = "//a[@id='btn-catalog-subcategory-items']")]
        public IList<IWebElement> SubCategoriesLinksList { get; set; }
        [FindsBy(How = How.XPath, Using = "//a[@id='btn-catalog-subcategory-items']/img")]
        public IList<IWebElement> SubcategoriesImagesLinks { get; set; }
        [FindsBy(How = How.XPath, Using = "//a[@id='btn-catalog-subcategory-items']/span")]
        public IList<IWebElement> SubcategoriesSpan { get; set; }
        [FindsBy(How = How.Id, Using = "category")]
        public IWebElement CategoryFrame { get; set; }
        #endregion
        #region Path Settings
        public override string Path => $"/{RunSettings.Locale}{RunSettings.Catalog_Products_Category}{RunSettings.ValidUser.Schema}/{RunSettings.CatalogCategory.EnergyFitness}";
        #endregion
        #region Functionality of Test Cases.
        public string CreatingLink(int i)
        {
            string uris = RunSettings.Url + RunSettings.Locale + RunSettings.Catalog_Products_Category + RunSettings.ValidUser.Schema + "/" + i;
            return uris;
        }

           public bool ValidationCategoryPagesAreDisplayed()
        {
            // You must close or flush the trace to empty the output buffer.  
            var isPageDisplayedCorrectly = true;
            var breadcrumbs = true;
            var subcategoriesLinks = true;
            var subcatergoriesImages = true;
            var validationElementNotBlank = true;
            Debug.WriteLine("Starting to validate all categories (Links, BreadCrumbs, SubcategoriesLinks, ValidationElementsNotBlank");
                if (CategoryFrame.Displayed)
                {
                    breadcrumbs = (CommonMethods.ValidateLink(BreadCrumbsLinks, "href") == true) ? true : false;
                    subcategoriesLinks = (CommonMethods.ValidateLink(SubCategoriesLinksList, "href") == true) ? true : false;
                    subcatergoriesImages = (CommonMethods.ValidateLink(SubcategoriesImagesLinks, "src") == true) ? true : false;
                    validationElementNotBlank = (ValidationElementNotBlank() == true) ? true : false;
                    if (breadcrumbs != true & subcategoriesLinks != true & subcatergoriesImages != true & validationElementNotBlank != true)
                    {
                        Debug.WriteLine("Breadcrumbs is NOK");
                        Debug.WriteLine("subcategoriesLinks is NOK");
                        Debug.WriteLine("subcatergoriesImages is NOK");
                        Debug.WriteLine("validationElementNotBlank is NOK");
                        return false;
                    }
                }
                else
                {
                    Debug.WriteLine("Page is not being displayed");
                    return isPageDisplayedCorrectly = false;
                }
            return isPageDisplayedCorrectly;
        }

        public bool ValidationElementNotBlank()
        {
            
            foreach (IWebElement element in SubcategoriesSpan)
            {
                Debug.WriteLine($"Checking Subcategory info {element.Text}");
                if (element.Text.Equals(""))
                {
                    return false;
                }
            }
            return true;
        }

        public void WaitPageToLoad()
        {
            try
            {
                Driver.WaitForDocumentReady();
                PageExtension.WaitForElementPresentAndVisible(this, CategoryFrame);
                PageExtension.WaitForElementPresentAndVisible(this, CommonFindBy.CartIcon);
                //PageExtension.WaitUntil(this, b => AssertHelpers.WebElementExists(SearchProductsBar) == true);
                PageExtension.WaitForElementGone(this, CommonFindBy.BusyIndicator);
            }
            catch (Exception e)
            {
            }
        }

        #endregion
    }
}
