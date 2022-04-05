using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using Shop.Automation.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Automation.Pages.Catalog
{
    public class ShopFAQPage : Page
    {
        #region Constructors
        public ShopFAQPage() { }
        public ShopFAQPage(IWebDriver driver, TestRunSettings runSettings)
        {
            this.Driver = driver;
            this.RunSettings = runSettings;
            PopulateElements();
        }
        #endregion

        #region Mapping
        [FindsBy(How = How.XPath, Using = "//div[@class='faq-text']//child::p")]
        public IList<IWebElement> QuestionsList { get; set; }

        [FindsBy(How = How.XPath, Using = "//li[@class='faq-item']//child::a")]
        public IList<IWebElement> QuestionsListVerify { get; set; }
        #endregion Mapping

        #region Path Settings
        public override string Path => $"/{RunSettings.Locale}{RunSettings.Catalog_ShopFAQ_Page}{RunSettings.ValidUser.Schema}";
        #endregion

        #region Functionality of Test Cases.
        /// <summary>
        /// click on each question section to display it wait and make another click to hide it.
        /// </summary>
        public void ShowAndHideAllQuestions()
        {
            foreach (IWebElement question in QuestionsListVerify)
            {
                this.DoClick(question);
                this.WaitUntil(p => question.FindElement(By.XPath("./../div")).GetAttribute("style").Equals("display: block;"), 2);
                this.DoClick(question);
                this.WaitUntil(p => question.FindElement(By.XPath("./../div")).GetAttribute("style").Equals("display: none;"), 2);
            }
        }
        #endregion
    }
}
