using OpenQA.Selenium;

namespace Shop.Automation.Framework
{
    public class TestPageFactory : ITestPageFactory
    {
        public ITestRunSettingsFactory RunSettingsFactory { get; }
        public readonly IWebDriver Driver;

        public TestPageFactory(IWebDriver driver, ITestRunSettingsFactory runSettingsFactory)
        {
            RunSettingsFactory = runSettingsFactory;
            Driver = driver;
        }

        public T Create<T>() where T : Page, new()
        {
            var result = new T
            {
                Driver = Driver,
                RunSettings = RunSettingsFactory.Create()
            };

            return result;
        }
    }
}
