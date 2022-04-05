using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Shop.Automation.Framework;
using Shop.Automation.Framework.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Shop.Automation.Pages.Catalog
{
    public class ApparelListPage : ProductListPage
    {
        public string validApparelCategory = "";

        #region Mapping
        [FindsBy(How = How.XPath, Using = "//div[@class='filters']//input[@name='size' and @type='checkbox']")]
        public IList<IWebElement> SizeFiltersCheckBoxes { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='filters']//li[@class='size']")]
        public IWebElement SizeFilter { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='filters']//input[@name='color' and @type='checkbox']")]
        public IList<IWebElement> ColorFiltersCheckBoxes { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='filters']//li[@class='color']")]
        public IWebElement ColorFilter { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='filters']//li[@class='color']")]
        public IWebElement QuantitySection { get; set; }

        public string ProductQuantityLocator => ".//li[@class='quantity']";

        public string ViewDetailsLocator => ".//*[@class='add-to-cart']";
        #endregion

        #region Path Settings 
        public override string Path => $"/{RunSettings.Locale}{RunSettings.Catalog_Product_List}{RunSettings.ValidUser.Schema}/{SetApparelCategory()}";

        public string SetApparelCategory(int elementIndex = 0)
        {
            if (validApparelCategory.Equals(""))
            {
                validApparelCategory = GetApparelCategoryIDFromAPI(elementIndex);
            }

            return validApparelCategory;
        }

        public string GetApparelCategoryIDFromAPI(int elementIndex = 0)
        {
            CatalogAPIConsumer apiConsumer = new CatalogAPIConsumer(TestContext.Parameters["targetEnvironment"], RunSettings.Locale);

            return apiConsumer.GetApparelCategoryID(elementIndex);
        }
        #endregion


        /// <summary>
        /// Select a checkbox item and return the text and the number of items to be shown after the filter is applied. If the element is already check an expection is thrown
        /// </summary>
        /// <param name="checkboxFilter"></param>
        /// <returns></returns>
        public Tuple<string, int> CheckFilter(IWebElement checkboxFilter)
        {
            string filterValue = "";
            string filterText = "";
            int numberOfTIems = 0;

            //if the element is already checked, send an exception
            if (checkboxFilter.Selected)
            {
                throw new Exception("The element is already checked");
            }

            //save the text value of the filter, navigate to the father and get the value of the label
            filterValue = checkboxFilter.FindElement(By.XPath("./..")).Text;

            //Get the filter value
            filterText = filterValue.Trim().Split(' ')[0];

            //Get the number of items to be displayed
            numberOfTIems = Convert.ToInt16(Regex.Match(filterValue, @"^.*\((.*?)\)[^\(]*$").Groups[1].Value);

            //Select the filter
            this.DoClick(checkboxFilter);
            PageExtension.WaitForElementGone(this, CommonFindBy.BusyIndicator);

            //return the filter value
            return new Tuple<string, int> (filterText, numberOfTIems);
        }

        /// <summary>
        /// Check that every title matches the size in the parameter. If one of the products does not match return false
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public bool AllTheProductsMatchSize(string size)
        {
            bool everythingOK = true;

            foreach (IWebElement productTitle in ProductNameByList)
            {
                if (!GetSizeFromProduct(productTitle.Text.Trim()).Equals(size))
                {
                    Debug.WriteLine($"Product {productTitle.Text} does not match the expected size {size}");
                    everythingOK = false;
                    break;
                }    
            }

            return everythingOK;
        }


        /// <summary>
        /// The name of the apparels is expected to have the size at the end of the name. This method return the is size of a product name in Apparels
        /// </summary>
        /// <param name="productName"></param>
        /// <returns></returns>
        public string GetSizeFromProduct(string productName)
        {
            string[] splittedTitle = productName.Split(' ');

            return splittedTitle[splittedTitle.Length - 1].Trim();
        }
    }
}
