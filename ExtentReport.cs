using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Automation.Framework
{
    public class ExtentReport
    {
        public Dictionary<string, ExtentReports> _extent;

        private static readonly Lazy<ExtentReport> lazyReporter = new Lazy<ExtentReport>(() => new ExtentReport());
        public ExtentReport()
        {
            _extent = new Dictionary<string, ExtentReports>();
        }

        public static ExtentReport ReporterInstance { get { return lazyReporter.Value; } }

        public void CreateReporter(string locale)
        {
            var path = System.Reflection.Assembly.GetCallingAssembly().CodeBase;
            string actualPath;
            if (path.Contains("/bin"))
                actualPath = path.Substring(0, path.LastIndexOf("bin"));
            else
                actualPath = path.Replace("Shop.Automation.Framework.DLL", "");
            var projectPath = new Uri(actualPath).LocalPath;
            var folderName = "Reports";
            Directory.CreateDirectory(projectPath.ToString() + folderName);
            var reportPath = projectPath + folderName + "\\Results" + DateTime.Now.ToString("ddMMyyyyHHmmss") + "_" + locale + ".html";
            var htmlReporter = new ExtentV3HtmlReporter(reportPath);
            _extent.Add(locale, new ExtentReports());
            _extent[locale].AttachReporter(htmlReporter);
        }
    }
}
