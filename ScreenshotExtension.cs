using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;
using System;
using System.IO;

namespace Shop.Automation.Framework
{
    public static class ScreenshotExtension

    {
        public static string CreateScreenshotFileName()
        {
            var fileName = $"{TestContext.CurrentContext.Test.ClassName}_{TestContext.CurrentContext.Test.Name}_{DateTime.Now.ToString("yyyy-MM-dd HH_mm_ss")}.jpg";
            var result = Path.Combine(TestContext.CurrentContext.TestDirectory, fileName);
            return result;
        }

        public static void SaveScreenshot(this IWebDriver webDriver, ScreenshotImageFormat imageFormat = ScreenshotImageFormat.Jpeg)
        {
            var screenShot = webDriver.TakeScreenshot();
            var fileName = CreateScreenshotFileName();
            Directory.CreateDirectory(Path.GetDirectoryName(fileName));
            screenShot.SaveAsFile(fileName, imageFormat);
            TestContext.AddTestAttachment(fileName);
            TestContext.WriteLine("Snapshot from URL {0}", webDriver.Url);
        }
    }
}
