using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Shop.Automation.Framework;
using Shop.Automation.Framework.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Automation.Pages.Catalog
{
    public class ApparelDetails : ProductDetailsPage
    {
        public string validSku = "";

        #region Mapping
        [FindsBy(How = How.XPath, Using = "//p[@class='shortcuts']/a[@data-bind='visible: SizeChart']")]
        public IWebElement SizeChartLink { get; set; }

        [FindsBy(How = How.ClassName, Using = "size-table")]
        public IWebElement SizeChartTable { get; set; }

        [FindsBy(How = How.ClassName, Using = "Color")]
        public IWebElement ColorContainer { get; set; }

        [FindsBy(How = How.ClassName, Using = "Size")]
        public IWebElement SizeContainer { get; set; }

        [FindsBy(How = How.XPath, Using = "//ul[@class='Size']/li")]
        public IList<IWebElement> SizeVariationList { get; set; }
        #endregion

        #region Path Settings 
        public override string Path => $"/{RunSettings.Locale}{RunSettings.Catalog_ProductDetail}{RunSettings.ValidUser.Schema}/{SetSku()}";
        public string SetSku(int elementIndex = 0)
        {
            if (validSku.Equals(""))
            {
                validSku = GetApparelSKUFromAPI(elementIndex);
            }

            return validSku;
        }

        public string GetApparelSKUFromAPI(int elementIndex = 0)
        {
            CatalogAPIConsumer apiConsumer = new CatalogAPIConsumer(TestContext.Parameters["targetEnvironment"], RunSettings.Locale);

            return apiConsumer.GetApparelSKU(elementIndex);
        }
        #endregion

        #region Functionality
        /// <summary>
        /// Click on Size Chart link and wait till the table is move to a visible point in the window. If it works as expected return true, otherwise false.
        /// </summary>
        /// <returns></returns>
        public bool SizeChartVisible()
        {
            //Click on the link
            this.DoClick(SizeChartLink);          
            
            //Get the size of the upper banner. The table should be visible and we use the 3 times the size of the upper banner
            double upperBannerHeightSize = Driver.FindElement(CommonFindBy.PageBanner).Size.Height;
            //Get the location in respect with the view port (visible window)
            double sizeTableYLocation = SizeChartTable.GetYLocation(this);

            return this.WaitUntil(p => SizeChartTable.GetYLocation(this) < (upperBannerHeightSize * 3), 5);
        }

        public List<IWebElement> FillPageElementsList()
        {
            List<IWebElement> PricelistElementsList = new List<IWebElement>();
            PricelistElementsList.Add(SizeChartLink);
            PricelistElementsList.Add(SizeChartTable);
            PricelistElementsList.Add(ColorContainer);
            PricelistElementsList.Add(SizeContainer);
            return PricelistElementsList;
        }

        /// <summary>
        /// Save the size, the price and the sku related to the variation product size. Then compare it with the new loaded page, if everything matches return true.
        /// </summary>
        /// <param name="sizeElement"></param>
        /// <returns></returns>
        public bool ChangeSizeCorrect(IWebElement sizeElement)
        {
            //Save the values
            string sku = sizeElement.GetAttribute("data-sku").Trim();
            string size = sizeElement.Text.Trim();

            //Click variation
            this.DoClick(sizeElement);

            //Check
            string currentSku = SkuNumber.Text;
            string currentProductName = Title.Text;

            if (!currentSku.Equals(sku))
            {
                return false;
            }

            string[] splittedTitle = currentProductName.Split(' ');
            if (!splittedTitle[splittedTitle.Length - 1].Equals(size))
            {
                return false;
            }

            return true;
        }
        #endregion

    }
}
