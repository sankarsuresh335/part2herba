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
    public class InventoryPage : Page
    {
        #region Constructors
        public InventoryPage() { }
        public InventoryPage(IWebDriver driver, TestRunSettings runSettings)
        {
            this.Driver = driver;
            this.RunSettings = runSettings;
            PopulateElements();
        }
        #endregion

        #region Path Settings
        public override string Path => $"/{RunSettings.Locale}{RunSettings.Catalog_Inventory}{RunSettings.ValidUser.Schema}";
        #endregion

        #region Mapping
        [FindsBy(How = How.XPath, Using = "//div[@class='availability-list']//input[@type='checkbox']")]
        public IList<IWebElement> PickupCheckboxList{ get; set; }
        #endregion

        #region TC Functionality
        public string GetCheckboxStringLabel(IWebElement checkbox)
        {
            string checkboxID = checkbox.GetAttribute("id");
            By xpathLocator = By.XPath($"//label[@for='{checkboxID}']");

            return Driver.FindElement(xpathLocator).Text.Trim();
        }

        public string GetCheckboxPickupLocation(IWebElement checkbox)
        {
            string checkboxLabel = GetCheckboxStringLabel(checkbox);

            return checkboxLabel.Split('-')[0].Trim();
        }

        /// <summary>
        /// The data in the table should match the checkboxes. This method returns the corresponding column header based on the provided checkbox
        /// </summary>
        /// <param name="checkbox"></param>
        /// <returns></returns>
        public IWebElement GetColumnHeaderBasedOnCheckBox(IWebElement checkbox)
        {
            string checkboxID = checkbox.GetAttribute("id");
            By xpathLocator = GetHeaderXpathLocator(checkboxID);

            try
            {
                return Driver.FindElement(xpathLocator);
            }
            catch (Exception)
            {

                return null;
            }
        }

        private By GetHeaderXpathLocator(string checkboxID)
        {
            return By.XPath($"//th[contains(@data-bind,'{checkboxID}')]");
        }

        private By GetCheckBoxRowXpathLocator(string checkboxID)
        {
            return By.XPath($"//td[contains(@data-bind,'{checkboxID}')]");
        }

        /// <summary>
        /// Check if the Column is visible. This method checks the status of the row in table body
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public bool ColumnBodyIsVisible(IWebElement checkbox)
        {
            try
            {
                IWebElement rowColumn = Driver.FindElement(GetCheckBoxRowXpathLocator(checkbox.GetAttribute("id")));

                PageExtension.ScrollIntoView(this, rowColumn, 0);

                return rowColumn.Displayed;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Something when wrong when validating the header {e.Message}");
                return false;
            }
        }

        /// <summary>
        ///  Check if the Column is visible. This method checks the status of the row in table header
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public bool ColumnHeaderIsVisible(IWebElement checkbox)
        {
            try
            {
               IWebElement rowColumn = Driver.FindElement(GetHeaderXpathLocator(checkbox.GetAttribute("id")));
                
                PageExtension.ScrollIntoView(this, rowColumn, 0);

                return rowColumn.Displayed;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Something when wrong when validating the header {e.Message}");
                return false;
            }
        }

        /// <summary>
        /// Deselect all the filters
        /// </summary>
        public void DeselectAllFilters()
        {
            foreach (IWebElement checkbox in PickupCheckboxList)
            {
                DeselectCheckBox(checkbox);
            }
        }

        /// <summary>
        /// If the checkbox is selected then click on it, otherwise leave it as it is.
        /// </summary>
        /// <param name="checkbox"></param>
        public void SelectCheckBox(IWebElement checkbox)
        {
            if (!checkbox.Selected)
            {
                this.DoClick(checkbox);

                IWebElement rowColumn = Driver.FindElement(GetHeaderXpathLocator(checkbox.GetAttribute("id")));

                WaitTillSectionIsShown(rowColumn);
            }
        }

        /// <summary>
        /// If the checkbox is not selected then click on it, otherwise leave it as it is.
        /// </summary>
        /// <param name="checkbox"></param>
        public void DeselectCheckBox(IWebElement checkbox)
        {
            if (checkbox.Selected)
            {
                this.DoClick(checkbox);

                IWebElement rowColumn = Driver.FindElement(GetHeaderXpathLocator(checkbox.GetAttribute("id")));

                WaitTillSectionIsHidden(rowColumn);
            }
        }

        /// <summary>
        /// Waits till the animation for showing the content is completed. The style of the section should be "display: block;"
        /// </summary>
        /// <param name="element"></param>
        public void WaitTillSectionIsShown(IWebElement element)
        {
            this.WaitUntil(p => (element.GetAttribute("style").Equals("")));
        }

        /// <summary>
        /// Waits till the animation for hidding the content is completed. The style of the section should be "display: none;"
        /// </summary>
        /// <param name="element"></param>
        public void WaitTillSectionIsHidden(IWebElement element)
        {
            this.WaitUntil(p => element.GetAttribute("style").Equals("display: none;"));
        }
        #endregion
    }
}
