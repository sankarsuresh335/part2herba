using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.PageObjects;
using Shop.Automation.Framework;
using System.Configuration;

namespace Shop.Automation.Pages.Catalog
{
    public class PriceListGenerator : Page
    {
        #region Contructors
        public PriceListGenerator() { }

        public PriceListGenerator(IWebDriver driver, TestRunSettings runSettings)
        {
            this.Driver = driver;
            this.RunSettings = runSettings;
            PopulateElements();
        }
        #endregion

        #region PUT_Web_Element_Mapping
        [FindsBy(How = How.XPath, Using = "//*[@class = 'price-list-content']/h3")]
        public IWebElement PriceListGeneratorTitle { get; set; }

        #region Choose Sales Tax Rate
        [FindsBy(How = How.XPath, Using = "(//div[@class = 'step'])[1]")]
        public IWebElement SaleTaxRateContentDiv { get; set; }

        [FindsBy(How = How.XPath, Using = "(//h4[@class = 'step-open'])[1]")]
        public IWebElement SalesTaxRateTitle { get; set; }
        
        [FindsBy(How = How.XPath, Using = "//input[@id = 'catalog-tax-rate']/preceding-sibling::*")]
        public IWebElement SalesTaxRateInput { get; set; }

        [FindsBy(How = How.ClassName, Using = "tax-rate")]
        public IWebElement SalesTaxRateValueSection { get; set; }

        [FindsBy(How = How.Id, Using = "btn-step1")]
        public IWebElement SalesTaxRateNextButton { get; set; }

        //***********Lookup by Address
        [FindsBy(How = How.Id, Using = "chosen-address")]
        public IWebElement ChoosenAddressContainer { get; set; }

        [FindsBy(How = How.Id, Using = "lookup-rate")]
        public IWebElement LookUpAddressContainer { get; set; }

        [FindsBy(How = How.Id, Using = "toggle-lookup")]
        public IWebElement LookUpAddressLink { get; set; }

        [FindsBy(How = How.Id, Using = "Address")]
        public IWebElement LookUpAddressAddressInput { get; set; }

        [FindsBy(How = How.Id, Using = "City")]
        public IWebElement LookUpAddressCityInput { get; set; }

        [FindsBy(How = How.Id, Using = "State")]
        public IWebElement LookUpAddressStateDD { get; set; }

        [FindsBy(How = How.Id, Using = "ZipCode")]
        public IWebElement LookUpAddressZIPInput{ get; set; }

        [FindsBy(How = How.Id, Using = "btn-save-lookup")]
        public IWebElement LookUpAddressSearchBtn { get; set; }

        [FindsBy(How = How.Id, Using = "btn-cancel-lookup")]
        public IWebElement LookUpAddressCancelBtn { get; set; }

        [FindsBy(How = How.CssSelector, Using = "span.k-widget.k-tooltip.k-tooltip-validation.k-invalid-msg")]
        public IList<IWebElement> LookUpAddressErrorMessageList { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='Address']/following-sibling::span[@class='k-widget k-tooltip k-tooltip-validation k-invalid-msg']")]
        public IWebElement LookUpAddressAddressErrorMessage { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='City']/following-sibling::span[@class='k-widget k-tooltip k-tooltip-validation k-invalid-msg']")]
        public IWebElement LookUpAddressCityErrorMessage { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='State']/following-sibling::span[@class='k-widget k-tooltip k-tooltip-validation k-invalid-msg']")]
        public IWebElement LookUpAddressStateErrorMessage { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='ZipCode']/following-sibling::span[@class='k-widget k-tooltip k-tooltip-validation k-invalid-msg' and @style='']")]
        public IWebElement LookUpAddressZipErrorMessage { get; set; }

        [FindsBy(How = How.ClassName, Using = "notification-error")]
        public IWebElement SearchAddressErrorMessage { get; set; }

        //********Choosen Address
        [FindsBy(How = How.XPath, Using = "//span[@data-bind='text: Address']")]
        public IWebElement ChoosenAddress{ get; set; }

        [FindsBy(How = How.XPath, Using = "//span[@data-bind='text: City']")]
        public IWebElement ChoosenCity { get; set; }

        [FindsBy(How = How.XPath, Using = "//span[@data-bind='text: State']")]
        public IWebElement ChoosenState { get; set; }

        [FindsBy(How = How.XPath, Using = "//span[@data-bind='text: ZipCode']")]
        public IWebElement ChoosenZip { get; set; }
        //***********
        #endregion

        #region Choose Shipping and Handling Rate
        [FindsBy(How = How.XPath, Using = "(//div[@class = 'step'])[2]")]
        public IWebElement ShippingContentDiv { get; set; }

        [FindsBy(How = How.XPath, Using = "(//h4[@class = 'step-open'])[2]")]
        public IWebElement ShippingTitle { get; set; }

        [FindsBy(How = How.XPath, Using = "//input[@id = 'catalog-shipping-handling-rate']/preceding-sibling::*")]
        public IWebElement ShippingInput { get; set; }

        [FindsBy(How = How.ClassName, Using = "ship-rate")]
        public IWebElement ShippingRateValueSection { get; set; }

        [FindsBy(How = How.Id, Using = "btn-step2")]
        public IWebElement ShippingNextButton { get; set; }

        [FindsBy(How = How.Id, Using = "btn-back1")]
        public IWebElement ShippingBackButton { get; set; }

        [FindsBy(How = How.XPath, Using = "(//div[@class='step'])[2]//tbody/tr")]
        public IList<IWebElement> ShippingRatesRows { get; set; }
        
        private readonly string rateXpathColumn = "./td[@class='rate']";
        private readonly string selectLinkXpathColumn = "./td[@class='select']/a";
        #endregion

        #region Choose Customer Discount Rate
        [FindsBy(How = How.XPath, Using = "(//div[@class = 'step'])[3]")]
        public IWebElement CustomerDiscountContentDiv { get; set; }

        [FindsBy(How = How.XPath, Using = "(//h4[@class = 'step-open'])[3]")]
        public IWebElement CustomerDiscountTitle { get; set; }

        [FindsBy(How = How.XPath, Using = "//input[@id = 'catalog-customer-discount-rate']/preceding-sibling::*")]
        public IWebElement CustomerDiscountInput { get; set; }

        [FindsBy(How = How.ClassName, Using = "discount-rate")]
        public IWebElement CustomerDiscountValueSection { get; set; }

        [FindsBy(How = How.Id, Using = "btn-step3")]
        public IWebElement CustomerDiscountNextButton { get; set; }

        [FindsBy(How = How.Id, Using = "btn-back2")]
        public IWebElement CustomerDiscountBackButton { get; set; }
        #endregion

        #region Price List Table

        [FindsBy(How = How.XPath, Using = "//div[@class='fht-thead']/table")]
        public IWebElement PLTTableHeader { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='fht-tbody']/table")]
        public IWebElement PLTTableBody { get; set; }

        [FindsBy(How = How.Id, Using = "btn-pl-generator-excel")]
        public IWebElement PLTDownloadExcel { get; set; }

        [FindsBy(How = How.Id, Using = "btn-pl-generator-pdf")]
        public IWebElement PLTDownloadPDF { get; set; }

        //********Advance Filters
        [FindsBy(How = How.Id, Using = "btn-pl-generator-advance-filters")]
        public IWebElement AdvancedFiltersShowFiltersLink { get; set; }

        [FindsBy(How = How.Id, Using = "btn-pl-generator-advance-hide-filters")]
        public IWebElement AdvancedFiltersHideFiltersLink { get; set; }

        [FindsBy(How = How.Id, Using = "column-filters")]
        public IWebElement FilterCheckBoxesContainer { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@id='column-filters']//input[@type='checkbox']")]
        public IList<IWebElement> FilterCheckBoxesList { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@id= 'column-filters']//input[@type='checkbox' and @class='VolumePoints']")]
        public IWebElement VolumenPointsFilterCheck { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@id= 'column-filters']//input[@type='checkbox' and @class='EarnBase']")]
        public IWebElement EarnBaseFilterCheck { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@id= 'column-filters']//input[@type='checkbox' and @class='RetailPrice']")]
        public IWebElement RetailPriceFilterCheck { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@id= 'column-filters']//input[@type='checkbox' and @class='DiscountedRetail']")]
        public IWebElement DiscountedRetailFilterCheck { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@id= 'column-filters']//input[@type='checkbox' and @class='YourPrice']")]
        public IWebElement YourPriceFilterCheck { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@id= 'column-filters']//input[@type='checkbox' and @class='CustomerPrice']")]
        public IWebElement CustomerPriceFilterCheck { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@id= 'column-filters']//input[@type='checkbox' and @class='Discount25']")]
        public IWebElement Discount25FilterCheck { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@id= 'column-filters']//input[@type='checkbox' and @class='Discount35']")]
        public IWebElement Discount35FilterCheck { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@id= 'column-filters']//input[@type='checkbox' and @class='Discount42']")]
        public IWebElement Discount42FilterCheck { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@id= 'column-filters']//input[@type='checkbox' and @class='Discount50']")]
        public IWebElement Discount50FilterCheck { get; set; }
        //********
        #endregion


        #endregion

        #region Path Settings
        public override string Path => $"/{RunSettings.Locale}{RunSettings.Catalog_PLG_Page}{RunSettings.ValidUser.Schema}";

        public readonly string dowloadedDirectory = Environment.CurrentDirectory + @"" + ConfigurationManager.AppSettings.Get("DownloadFilesPath");
        public readonly string PLTFileName = ConfigurationManager.AppSettings.Get("PTLExcelFileName");
        #endregion

        #region Functionality
        public void CalculatePricelistTableWithSalesTaxRate(double salesTaxRate, double shippingRate, double customerDiscount)
        {
            ChooseSalesTaxRateAndContinue(salesTaxRate);
            ChooseShippingAndContinue(shippingRate);
            ChooseCustomerDiscountAndContinue(customerDiscount);
        }

        public void ShowChooseSaleTaxRateContent()
        {
            ShowContent(SaleTaxRateContentDiv, SalesTaxRateTitle);
        }

        public void HideChooseSaleTaxRateContent()
        {
            HideContent(SaleTaxRateContentDiv, SalesTaxRateTitle);
        }

        public void ShowShipppingAndHandlingRateContent()
        {
            ShowContent(ShippingContentDiv, ShippingTitle);
        }

        public void HideShipppingAndHandlingRateContent()
        {
            HideContent(ShippingContentDiv, ShippingTitle);
        }


        public void ShowCustomerDiscountContent()
        {
            ShowContent(CustomerDiscountContentDiv, CustomerDiscountTitle);
        }

        public void HideCustomerDiscountContent()
        {
            HideContent(CustomerDiscountContentDiv, CustomerDiscountTitle);
        }

        public string GetURLForExcelFile()
        {
            return PLTDownloadExcel.GetAttribute("href");
        }

        public string GetURLForDownloadPDF()
        {
            return PLTDownloadPDF.GetAttribute("href");
        }

        public void PrepareForDownload()
        {
            if (!CommonMethods.CreateFolder(dowloadedDirectory))
            {
                CommonMethods.DeleteFilesWithPrefix(PLTFileName, dowloadedDirectory);
            }
        }

        public void ShowFilters()
        {
            //If link visible click on it
            if (AdvancedFiltersShowFiltersLink.Displayed)
            {
                this.DoClick(AdvancedFiltersShowFiltersLink);
            }

            //Wait for the animation
            WaitTillSectionIsShown(FilterCheckBoxesContainer);
        }

        public void HideFilter()
        {
            //If link visible click on it
            if (AdvancedFiltersHideFiltersLink.Displayed)
            {
                this.DoClick(AdvancedFiltersHideFiltersLink);
            }

            //Wait for the animation
            WaitTillSectionIsHidden(FilterCheckBoxesContainer);
        }

        public void DeselectFilter(IWebElement checkBox)
        {
            checkBox.DeselectCheck();

            string filterClass = checkBox.GetAttribute("class") ?? "";

            IWebElement column = PLTTableBody.FindElement(By.XPath($".//td[@class='{filterClass}']"));

            WaitTillSectionIsHidden(column);
        }

        /// <summary>
        /// Check if the Column is visible. This method checks the status of the row in table body
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public bool ColumnBodyIsVisible(string filter)
        {
            try
            {
                IWebElement column = PLTTableBody.FindElement(By.XPath($".//td[@class='{filter}']"));

                PageExtension.ScrollIntoView(this, column, 0);

                return column.Displayed;
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
        public bool ColumnHeaderIsVisible(string filter)
        {
            try
            {
                IWebElement column = PLTTableHeader.FindElement(By.XPath($".//th[contains(@data-bind,'{filter}')]"));
                
                PageExtension.ScrollIntoView(this, column, 0);

                return column.Displayed;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Something when wrong when validating the header {e.Message}");
                return false;
            }
        }

        public void CancelChooseShippingRate()
        {
            //Make the content visible
            MakeShippingRateContentVisible();
            //Click on back btn
            this.DoClick(ShippingBackButton);
            //Wait till the content is hidden
            WaitTillSectionIsHidden(ShippingContentDiv);
        }

        public void CancelChooseCustomerDiscountRate()
        {
            //Make the content visible
            MakeCustomerDiscountContentVisible();
            //Click on back btn
            this.DoClick(CustomerDiscountBackButton);
            //Wait till the content is hidden
            WaitTillSectionIsHidden(CustomerDiscountContentDiv);
        }

        /// <summary>
        /// Find a row from the courier table that has a rate with a porcentage value. If found it select it and return the rate value. If not row is found return -1
        /// </summary>
        /// <returns></returns>
        public double SelectPercentageValueFromCourierTable()
        {
            //return 0;
            double shippingRate;
            //Make the content visible
            MakeShippingRateContentVisible();

            //Get a row from the table an select it.
            String shippingRateString = SelectFromCourierTable("%");
            shippingRate = shippingRateString.Equals("") ? -1 : GetSectionProcentageValueAsNumber(shippingRateString);

            if (shippingRate != -1)
                WaitTillSectionIsHidden(ShippingContentDiv);

            return shippingRate;
        }

        /// <summary>
        /// Find a row from the courier table that has a rate with a porcentage value. If found it select it and return the rate value. If not row is found return -1
        /// </summary>
        /// <returns></returns>
        public double SelectCurrecyValueFromCourierTable()
        {
            //return 0;
            double shippingRate;
            //Make the content visible
            MakeShippingRateContentVisible();

            //Get a row from the table an select it.
            String shippingRateString = SelectFromCourierTable("$");
            shippingRate = shippingRateString.Equals("") ? -1 : GetShippingCurrencyFromTableAsNumber(shippingRateString);

            if (shippingRate != -1)
                WaitTillSectionIsHidden(ShippingContentDiv);           

            return shippingRate;
        }

        /// <summary>
        /// Return the value of a currency rate in shipping and handling rate as souble. In shipping and handling table is expected to have a string with an specific format, e.g. $5.00 minimum charge or $9.0 cargo mínimo
        /// </summary>
        /// <param name="shippingRateString"></param>
        /// <returns></returns>
        private double GetShippingCurrencyFromTableAsNumber(string shippingRateString)
        {
            string[] data = shippingRateString.Split(' ');
            string valueWitOutCurrencySymbol = data[0].Replace('$', ' ').Trim();

            return Convert.ToDouble(valueWitOutCurrencySymbol);
        }

        private string SelectFromCourierTable(string searchCriteria)
        {
            String value = "";
            try
            {
                //Find a row where the rate contains "%" and select it
                IWebElement tableRow = ShippingRatesRows.Where(p => p.FindElement(By.XPath(rateXpathColumn)).Text.Contains(searchCriteria)).First();

                value = tableRow.FindElement(By.XPath(rateXpathColumn)).Text;

                this.DoClick(tableRow.FindElement(By.XPath(selectLinkXpathColumn)));
            }
            catch (Exception)
            {
                Debug.WriteLine("Could not find row with porcentage value");
                return "";
            }

            return value;
        }

        public string ChooseSalesTaxRateAndContinue(double rateValue)
        {
            //Add the the tax rate
            AddSalesTaxRate(rateValue);

            //Click on Next btn
            this.DoClick(SalesTaxRateNextButton);

            //Wait the section is hidden
            WaitTillSectionIsHidden(SaleTaxRateContentDiv);

            this.ScrollElementBelowBanner(SaleTaxRateContentDiv);

            return SalesTaxRateValueSection.Text;
        }

        /// <summary>
        /// Add values to Address, City, State and Zip code and click Search, 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="city"></param>
        /// <param name="state"></param>
        /// <param name="zipCode"></param>
        /// <returns></returns>
        public bool LookUpforAddress(string address, string city, string state, string zipCode)
        {
            //Make sure that the section is visible
            MakeSalesTaxRateContentVisible();

            //Clikc on Lookup by address
            this.DoClick(LookUpAddressLink);
            WaitTillSectionIsShown(LookUpAddressContainer);

            //Add the values
            LookUpAddressAddressInput.SendTextToElement(address);
            LookUpAddressCityInput.SendTextToElement(city);
            LookUpAddressStateDD.SelectDropDownOption(state);
            LookUpAddressZIPInput.SendTextToElement(zipCode);

            //Click Search btn
            this.DoClick(LookUpAddressSearchBtn);

            //Wait till the spinning icon is remove from the DOM
            this.WaitForElementGone(CommonFindBy.BusyIndicator, 60);

            //If an error message is displayes after searching an address, do not wait for the sections to be displayed.
            this.WaitUntil(p => SearchAddressErrorMessage.Displayed, 2);

            //If there is an error message do not continue with the flow
            if (LookUpAddressErrorMessageList.Where(p => p.Displayed).Count() > 0 || SearchAddressErrorMessage.Displayed)
                return false;

            //If no issues where found, wait till the section are visible
            WaitTillSectionIsHidden(LookUpAddressContainer);
            WaitTillSectionIsShown(ChoosenAddressContainer);

            return true;
        }

        public bool LookUpforAddress(string address, string city, int state, string zipCode)
        {
            //Make sure that the section is visible
            MakeSalesTaxRateContentVisible();

            //Clikc on Lookup by address
            this.DoClick(LookUpAddressLink);
            WaitTillSectionIsShown(LookUpAddressContainer);

            //Add the values
            LookUpAddressAddressInput.SendTextToElement(address);
            LookUpAddressCityInput.SendTextToElement(city);
            LookUpAddressStateDD.SelectDropDownIndex(state);
            LookUpAddressZIPInput.SendTextToElement(zipCode);

            //Click Search btn
            this.DoClick(LookUpAddressSearchBtn);

            //Wait till the spinning icon is remove from the DOM
            this.WaitForElementGone(CommonFindBy.BusyIndicator, 60);

            //If an error message is displayes after searching an address, do not wait for the sections to be displayed.
            this.WaitUntil(p => SearchAddressErrorMessage.Displayed, 2);

            //If there is an error message do not continue with the flow
            if (LookUpAddressErrorMessageList.Where(p => p.Displayed).Count() > 0 || SearchAddressErrorMessage.Displayed)
                return false;

            //If no issues where found, wait till the section are visible
            WaitTillSectionIsHidden(LookUpAddressContainer);
            WaitTillSectionIsShown(ChoosenAddressContainer);

            return true;
        }

        /// <summary>
        /// Search for a valid address and click Next to save the rate. Return the Rate value in the header section.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="city"></param>
        /// <param name="state"></param>
        /// <param name="zipCode"></param>
        /// <returns></returns>
        public string LookUpforAddressAndContinue(string address, string city, string state, string zipCode)
        {
            LookUpforAddress(address, city, state, zipCode);

            //Click on Next btn
            this.DoClick(SalesTaxRateNextButton);

            //Wait the section is hidden
            WaitTillSectionIsHidden(SaleTaxRateContentDiv);

            this.ScrollElementBelowBanner(SaleTaxRateContentDiv);

            return SalesTaxRateValueSection.Text;
        }

        public string ChooseShippingAndContinue(double rateValue)
        {
            //Add the the tax rate
            AddShippingAndHandleRate(rateValue);

            //Click on Next btn
            this.DoClick(ShippingNextButton);

            //Wait the section is hidden
            WaitTillSectionIsHidden(ShippingContentDiv);
            this.ScrollElementBelowBanner(ShippingContentDiv);

            return ShippingRateValueSection.Text;
        }

        public string ChooseCustomerDiscountAndContinue(double rateValue)
        {
            //Add the the tax rate
            AddCustomerDiscountRate(rateValue);

            //Click on Next btn
            this.DoClick(CustomerDiscountNextButton);

            //Wait the section is hidden
            WaitTillSectionIsHidden(CustomerDiscountContentDiv);
            this.ScrollElementBelowBanner(CustomerDiscountContentDiv);

            //Wait till the spinning icon is remove from the DOM
            this.WaitForElementGone(CommonFindBy.BusyIndicator, 60);

            return CustomerDiscountValueSection.Text;
        }

        /// <summary>
        ///  Make the content of the section visible and send the text. Returns the value in the input
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public double AddSalesTaxRate(double value)
        {
            //Make sure that the section is visible
            MakeSalesTaxRateContentVisible();

            //Send the text
            SendTextToRateInputs(SalesTaxRateInput, Convert.ToString(value));

            return Convert.ToDouble(SalesTaxRateInput.GetAttribute("value"));
        }
        
        /// <summary>
        ///  Make the content of the section visible and send the text. Returns the value in the input
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public double AddShippingAndHandleRate(double value)
        {
            //Make sure that the section is visible
            MakeShippingRateContentVisible();

            //Send the text
            SendTextToRateInputs(ShippingInput, Convert.ToString(value));

            return Convert.ToDouble(ShippingInput.GetAttribute("value"));
        }

        /// <summary>
        ///  Make the content of the section visible and send the text. Returns the value in the input
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public double AddCustomerDiscountRate(double value)
        {
            //Make sure that the section is visible
            MakeCustomerDiscountContentVisible();

            //Send the text
            SendTextToRateInputs(CustomerDiscountInput, Convert.ToString(value));

            return Convert.ToDouble(CustomerDiscountInput.GetAttribute("value"));
        }

        /// <summary>
        /// Clear the input and send a keys, after that send a tab
        /// </summary>
        /// <param name="webElement"></param>
        /// <param name="text"></param>
        protected void SendTextToRateInputs(IWebElement webElement, string text)
        {
            int charsInInput =  webElement.GetAttribute("value").Length == 0 ? 1 : webElement.GetAttribute("value").Length;

            //Clear the value
            webElement.Clear();

            //Create an instance of Actions, to move the input delete the 0 and then send the text
            Actions builderActions = new Actions(this.Driver);

            builderActions.MoveToElement(webElement).Build().Perform();

            //Clear the text
            for (int i = 0; i < charsInInput; i++)
            {
                builderActions.SendKeys(Keys.Delete).Build().Perform();
            }

            //Send the text to the input
            builderActions.SendKeys(text).Build().Perform();

            //In order to hava the correct value saved in the input click outside the element.
            this.DoClick(PriceListGeneratorTitle);
        }

        /// <summary>
        /// Hide the content of an specific section in PLG clicking on the section title. If the content is already hidden, first show the content.
        /// </summary>
        /// <param name="PGLSection"></param>
        /// <param name="PGLSectionTitle"></param>
        private void HideContent(IWebElement PGLSection, IWebElement PGLSectionTitle)
        {
            //Precondition: Sales Tax Rate should noy be hidden
            if (!PGLSection.Displayed)
            {
                ShowContent(PGLSection, PGLSectionTitle);
            }

            //Click on Choose Sales Tax Rate to hide its content
            this.DoClick(PGLSectionTitle);

            //Wait till the animation is completed
            WaitTillSectionIsHidden(PGLSection);
        }


        private void ShowContent(IWebElement PGLSection, IWebElement PGLSectionTitle)
        {
            //Precondition: Sales Tax Rate should noy be hidden
            if (PGLSection.Displayed)
            {
                HideContent(PGLSection, PGLSectionTitle);
            }

            //Click on Choose Sales Tax Rate to hide its content
            this.DoClick(PGLSectionTitle);

            //Wait till the animation is completed
            WaitTillSectionIsShown(PGLSection);
        }

        /// <summary>
        /// Waits till the animation for showing the content is completed. The style of the section should be "display: block;"
        /// </summary>
        /// <param name="PGLSection"></param>
        public void WaitTillSectionIsShown(IWebElement PGLSection)
        {
            this.WaitUntil(p => (PGLSection.GetAttribute("style").Equals("display: block;")));
        }

        /// <summary>
        /// Waits till the animation for hidding the content is completed. The style of the section should be "display: none;"
        /// </summary>
        /// <param name="PGLSection"></param>
        public void WaitTillSectionIsHidden(IWebElement PGLSection)
        {
            this.WaitUntil(p => PGLSection.GetAttribute("style").Equals("display: none;"));
        }

        /// <summary>
        /// Section value, the one that appears after clicking Next button has a suffix of $ or %. The method remove such character and returns a double.
        /// </summary>
        /// <param name="valueSection"></param>
        /// <returns></returns>
        public double GetSectionProcentageValueAsNumber(string valueSection)
        {
            valueSection = valueSection.Trim();

            string valueWitoutSuffix = valueSection.Substring(0, valueSection.Length - 1);

            return Convert.ToDouble(valueWitoutSuffix);
        }


        /// <summary>
        /// If the content of Sales Tax Rate is not visible, click on the title to make visible
        /// </summary>
        private void MakeSalesTaxRateContentVisible()
        {
            if (!SaleTaxRateContentDiv.Displayed)
                ShowChooseSaleTaxRateContent();
        }

        /// <summary>
        /// If the content of Choose Shipping and Handling Rate is not visible, click on the title to make visible
        /// </summary>
        private void MakeShippingRateContentVisible()
        {
            if (!ShippingContentDiv.Displayed)
                ShowShipppingAndHandlingRateContent();
        }

        /// <summary>
        /// If the content of Customer Discount Rate is not visible, click on the title to make visible
        /// </summary>
        private void MakeCustomerDiscountContentVisible()
        {
            if (!CustomerDiscountContentDiv.Displayed)
                ShowCustomerDiscountContent();
        }
        #endregion

    }
}