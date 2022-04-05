using AventStack.ExtentReports;
using HL.Blocks.Caching.RedisCache;
using Microsoft.TeamFoundation.TestManagement.Client;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using Shop.Automation.Framework.Helpers.Automated_Reports;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Unity;
using Unity.Injection;

namespace Shop.Automation.Framework
{
    [TestFixture]
    [Parallelizable]
    [IntegrationTestCategory]
    public class BasePageTest<T> where T : Page, new()
    {
        Dictionary<int, TestStatus> TestStatusContainer = new Dictionary<int, TestStatus>();
        //Redis
        private UnityContainer _container = new UnityContainer();
        public CacheAdapter setUpCache;
        //Redis end
        ExtentTest extentTest;
        public const string WebDriverKey = "__WebDriver__";
        public const string CurrentPageKey = "__CurrentPage__";
        public const string TargetEnvironmentKey = "targetEnvironment";
        private const string IsAuthenticatedKey = "__IsAuthenticated__";
        public const string browserKey = "Browser";
        public const string FirefoxPathKey = "FirefoxPath";
        public const string ChromePathKey = "ChromePath";
        private string Locale = "en-US";

        //public TestExecutionReport testExecutionReport;

        #region Report
        private static ReaderWriterLockSlim _readWriteLock = new ReaderWriterLockSlim();
        private Stopwatch tcStopWatch;
        protected readonly string reportPathPrefix = $"{Environment.CurrentDirectory}\\Report\\";
        protected readonly string reportPathSubfix = $"\\{DateTime.Now.Month.ToString()}.{DateTime.Now.Day.ToString()}.{DateTime.Now.Year.ToString()}\\";
        public const string GeneralReportName = "generalReportName";
        public const string FinalReportName = "finalReportName";
        public const string CreateReportProperty = "createReport";
        #endregion

        // ReSharper disable once StaticMemberInGenericType
        // Intentional: ClassState static field will have a different instance for each closed generic type of <T>
        public ConcurrentDictionary<string, object> ClassState = new ConcurrentDictionary<string, object>();

        public T CurrentPage;

        public IWebDriver Driver => (IWebDriver)ClassState[WebDriverKey];
        public bool IsAuthenticated
        {
            get
            {
                object value;
                return ClassState.TryGetValue(IsAuthenticatedKey, out value) && (bool)value;
            }
            set { ClassState[IsAuthenticatedKey] = value; }
        }

        public BasePageBehavior TestBehavior { get; set; }
        // ReSharper disable once ConvertToAutoProperty
        public TestContext TestContext { get; set; }
        public BasePageTest() : this(new BasePageBehavior()) { }
        public BasePageTest(BasePageBehavior basePageBehavior)
        {
            //testExecutionReport = new TestExecutionReport();
            TestBehavior = basePageBehavior;
            var localeBasedBehavior = basePageBehavior as LocaleBasedPageBehavior;
            if (localeBasedBehavior != null && !string.IsNullOrEmpty(localeBasedBehavior.Locale))
            {
                Locale = localeBasedBehavior.Locale;
            }
        }

        [OneTimeSetUp]
        public virtual void OneTimeSetUp()
        {
            ReportFileCreation();
            if (!ExtentReport.ReporterInstance._extent.ContainsKey(TestContext.CurrentContext.Test.ClassName))
                ExtentReport.ReporterInstance.CreateReporter(TestContext.CurrentContext.Test.ClassName);
        }

        [SetUp]
        public virtual void TestInitialize()
        {
            //Start the stopwatch.
            StartExcetutionTimeCounting();
            extentTest = ExtentReport.ReporterInstance._extent[TestContext.CurrentContext.Test.ClassName].CreateTest(TestContext.CurrentContext.Test.Name);
            var testDriver = SetDriver();
            var targetEnvironmentName = GetTargetEnvironmentName();
            var runSettingsFactory = new TestRunSettingsFactory(targetEnvironmentName, Locale);
            try
            {
                CurrentPage = new TestPageFactory(testDriver, runSettingsFactory).Create<T>();
                //SetRedis();
            }
            catch (Exception ex)
            {
                testDriver.Quit();
                throw;
            }
        }

        public void InitializeFromMethod(string locale)
        {
            var testDriver = SetDriver();
            var targetEnvironmentName = GetTargetEnvironmentName();
            var runSettingsFactory = new TestRunSettingsFactory(targetEnvironmentName, locale);

            try
            {
                CurrentPage = new TestPageFactory(testDriver, runSettingsFactory).Create<T>();
            }
            catch
            {
                testDriver.Quit();
                throw;
            }
        }

        private void TfsOperations()
        {
            string name = GetType().Name;
            //var testId = ((int)TestContext.CurrentContext.Test.Properties["TestCaseId"].First());
            var webDriver = Driver;
            var testId = string.Empty;
            foreach (var item in TestStatusContainer)
            {
                testId = testId + item.Key + ",";
            }
            var testIdList = testId.Remove(testId.Length - 1);
            // Create a new test run 
            ITestPointCollection testPoints = null;
            // Add the certain a test to  the run 
            try
            {
                testPoints = TfsInstance.Instance.plan.QueryTestPoints($"SELECT * from TestPoint where testPoint.TestCaseId in ({testIdList}) and testPoint.SuiteId in ({Convert.ToInt32(TestContext.Parameters.Get("TfsSubSuiteId"))})"); //ITestPointCollection testPoints
            }
            catch (Exception ex)
            {

            }
            if (!TfsInstance.Instance.runs.ContainsKey(name))
            {
                TfsInstance.Instance.runs.Add(name, TfsInstance.Instance.plan.CreateTestRun(true));
                TfsInstance.Instance.runs[name].Title = name;
            }
            foreach (ITestPoint testPoint in testPoints)
            {
                TfsInstance.Instance.runs[name].AddTestPoint(testPoint, null);
            }
            TfsInstance.Instance.runs[name].Save();

            //Update the outcome of the test       
            ITestCaseResultCollection results = TfsInstance.Instance.runs[name].QueryResults();
            foreach (ITestCaseResult result in results)
            {
                var requiredKeyValuePair = TestStatusContainer.Where(q => q.Key == result.TestCaseId).FirstOrDefault();
                if (requiredKeyValuePair.Key == result.TestCaseId)
                {
                    result.Outcome = (requiredKeyValuePair.Value == NUnit.Framework.Interfaces.TestStatus.Passed) ?
                                        TestOutcome.Passed : TestOutcome.Failed;
                    result.State = TestResultState.Completed;
                    result.Save();
                }
            }

            TfsInstance.Instance.runs[name].Save();
            TfsInstance.Instance.runs[name].Refresh();
        }


        /// <summary>
        /// Starts the Driver and opens the web browser
        /// </summary>
        /// <param name="timeout">Timeout for finding objects and loading pages, default 30 seconds</param>
        /// <param name="sURL">First url to be loaded</param>
        /// Params are obtained from environment.runsettings(example qa4.runsettings). 
        /// **IMPORTANT INFO: to get more information please go shop.automation.acceptance/RunSettings/environment.runsettings
        //internal IWebDriver SetDriver()
        //{
        //    object existing;
        //    if (ClassState.TryGetValue(WebDriverKey, out existing) && existing != null)
        //    {
        //        return existing as IWebDriver;
        //    }

        //    IWebDriver result = null;

        //    var tempContext = TestContext.CurrentContext;

        //    switch (TestContext.Parameters[browserKey].ToString().ToUpper())
        //    {
        //        case "FIREFOX":
        //            try
        //            {
        //                FirefoxDriverService service = FirefoxDriverService.CreateDefaultService(TestContext.CurrentContext.WorkDirectory,"geckodriver.exe");
        //                FirefoxOptions firefoxOptions = HeadlessMode.GetInstance.GetFirefoxOptions;
        //                //options.BrowserExecutableLocation = TestContext.Parameters[FirefoxPathKey].ToString() + "firefox.exe";
        //                result = (firefoxOptions != null) ? new FirefoxDriver(service, firefoxOptions, TimeSpan.FromMinutes(1)) : new FirefoxDriver();
        //            }   
        //            catch (Exception e)
        //            {
        //                throw new AssertionException("Something wrong´s with firefox path or firefox driver. Firefox Path and firefox driver must not be blank");
        //            }
        //            break;
        //        case "CHROME":
        //            try
        //            {
        //                ChromeOptions chromeOptions = HeadlessMode.GetInstance.GetChromeOptions;
        //                result = (chromeOptions != null) ? new ChromeDriver(TestContext.CurrentContext.WorkDirectory, chromeOptions) : new ChromeDriver();
        //            }
        //            catch
        //            {
        //                throw new AssertionException("Something wrong is with chrome driver path. chromePath must not be blank");
        //            }
        //            break;
        //        default:
        //            result = new FirefoxDriver();
        //            break;
        //    }

        //    if (result.Manage().Window.Size.Width < 1041)
        //    {
        //        result.Manage().Window.Maximize();
        //        if (result.Manage().Window.Size.Width < 1041)
        //        {
        //            result.Manage().Window.Size = new System.Drawing.Size(1260, 960);
        //        }
        //    }
        //    ClassState[WebDriverKey] = result;
        //    ClassState[IsAuthenticatedKey] = false;
        //    result.Manage().Cookies.DeleteAllCookies();
        //    return result;
        //}

        internal IWebDriver SetDriver()
        {
            string getExtensionFilePath = TestContext.Parameters["ExtensionFilePath"];
            string extensionFilePath = getProjectPath().ToString() + getExtensionFilePath;
            IWebDriver result = null;
            object existing;
            if (ClassState.TryGetValue(WebDriverKey, out existing) && existing != null)
            {
                return existing as IWebDriver;
            }
            string getBrowser = TestContext.Parameters["Browser"];

            if (TestContext.Parameters["UseBrowserStack"].ToString().ToLower() == "true")
            {
                Dictionary<string, object> profile = new Dictionary<string, object>();
                // 0 - Default, 1 - Allow, 2 - Block - To enable geo loation
                profile.Add("profile.default_content_setting_values.geolocation", 1);
                var browserOptions = new ChromeOptions();
                browserOptions.PlatformName = "Windows 10";
                browserOptions.BrowserVersion = "latest";
                if (TestContext.Parameters["IsCookieEnable"].ToString().ToLower() == "true")
                    browserOptions.AddExtensions(new Uri(extensionFilePath).LocalPath);
                var sauceOptions = new Dictionary<string, object>();
                /*sauceOptions.Add("parentTunnel", "HLSauceSVC");
                sauceOptions.Add("tunnelIdentifier", "HLSauceSVC_tunnel_id_vm01");*/
                sauceOptions.Add("screenResolution", "1920x1080");
                // SET CHROME OPTIONS
                sauceOptions.Add("prefs", profile);
                browserOptions.AddAdditionalCapability("sauce:options", sauceOptions, true);
                sauceOptions.Add("build", TestContext.Parameters["BuildName"].ToString() + DateTime.Now.ToString("MM/dd/yyyy"));
                sauceOptions.Add("name", GetType().Name);
                sauceOptions.Add("maxDuration", 10800);
                result = new RemoteWebDriver(new Uri("https://sso-sso-herbalife-sheiksa:ef32abfe-40f5-4251-94af-a8e21d771c5e@ondemand.us-west-1.saucelabs.com:443/wd/hub"), browserOptions);
                if (TestContext.Parameters["IsCookieEnable"].ToString().ToLower() == "true")
                    result = SetCookieCompliance(result);
            }
            else
            {
                string getChromeDriverPath = TestContext.Parameters["YourChromeDriverPath"];
                var filepath = System.IO.Path.Combine(
                        System.Reflection.Assembly.GetExecutingAssembly().Location.ToString().Replace("Shop.Automation.Framework.dll", string.Empty),
                        getChromeDriverPath);
                if (getBrowser == "chrome")
                {
                    ChromeOptions chromeOptions = new ChromeOptions();

                    chromeOptions.BinaryLocation = getChromeDriverPath;
                    chromeOptions.AddArgument("--window-size=1920,1080");
                    //result = new ChromeDriver(@filepath, chromeOptions);
                    if (TestContext.Parameters["IsCookieEnable"].ToString().ToLower() == "true")
                        chromeOptions.AddExtensions(new Uri(extensionFilePath).LocalPath);
                    result = new ChromeDriver(chromeOptions);
                    if (TestContext.Parameters["IsCookieEnable"].ToString().ToLower() == "true")
                        result = SetCookieCompliance(result);
                }
                else
                {
                    result = new FirefoxDriver();
                }

            }
            result.Manage().Window.Maximize();
            ClassState[WebDriverKey] = result;
            ClassState[IsAuthenticatedKey] = false;
            return result;
        }


        public string GetTargetEnvironmentName()
        {
            var targetEnvironmentName = "";
            string readFromConfig = TestContext.Parameters["TestRunEnvironment"];
            targetEnvironmentName = (TestContext.Parameters[TargetEnvironmentKey] ?? readFromConfig).ToString();
            return targetEnvironmentName;
        }

        //[TearDown]
        //public virtual void TestCleanup()
        //{
        //    //Stop running time
        //    StopExecutionTimeCounting();

        //    var webDriver = Driver;
        //    if (webDriver == null) { return; }
        //    try
        //    {
        //        if (TestContext?.Result.Outcome.Status == TestStatus.Failed)
        //        {
        //            webDriver.SaveScreenshot(TestContext);
        //            DestroyDriver();
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        TestContext.WriteLine("Failed saving screenshot. {0}", e);
        //        DestroyDriver();
        //    }
        //    finally
        //    {
        //        if (TestBehavior.DriverInstantiation == DriverInstantiation.EveryTest)
        //        {
        //            DestroyDriver();
        //        }
        //    }

        //    //Add the TC to the report
        //    AddTCToReport(); 
        //}
        [TearDown]
        public virtual void TestCleanup()
        {
            if (TestContext.Parameters["UseTfs"].ToString().ToLower() == "true")
            {
                var testId = ((int)TestContext.CurrentContext.Test.Properties["TestCaseId"].First());
                TestStatusContainer.Add(testId, TestContext.CurrentContext.Result.Outcome.Status);

            }
            var webDriver = Driver;
            if (webDriver == null) { return; }
            try
            {
                if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
                {
                    webDriver.SaveScreenshot();
                    //testExecutionReport.UpdateExcel(TestContext.CurrentContext.Test.Name, "FAIL", "Testcase Execution failed");
                }
                else if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Passed)
                {
                    //testExecutionReport.UpdateExcel(TestContext.CurrentContext.Test.Name, "PASS", "Testcase executed successfully");
                }
                if (TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed)
                {
                    string screenShotPath = Capture(Driver, TestContext.CurrentContext.Test.MethodName);
                    extentTest.Log(Status.Fail, "Snapshot below: " + extentTest.AddScreenCaptureFromPath(screenShotPath.Replace("\\Reports", "")));
                    extentTest.Log(Status.Fail, TestContext.CurrentContext.Result.Message.Replace("\r\n", "<br />") + "<br />" + "StackTrace: " + "<br />" + TestContext.CurrentContext.Result.StackTrace);
                }
                else
                    extentTest.Log(Status.Pass, TestContext.CurrentContext.Result.StackTrace);

            }
            catch (Exception e)
            {
                TestContext.WriteLine("Failed saving screenshot. {0}", e);
            }
            finally
            {
                if (TestBehavior.DriverInstantiation == DriverInstantiation.EveryTest)
                {
                    DestroyDriver();
                }
            }

        }

        [OneTimeTearDown]
        public virtual void Cleanup()
        {
            if (TestContext.Parameters["UseTfs"].ToString().ToLower() == "true")
            {
                TfsOperations();
            }
            DestroyDriver();
            KillDriverProccess();
            ExtentReport.ReporterInstance._extent[TestContext.CurrentContext.Test.ClassName].Flush();
         }



        public void BaseClassInitialize(TestContext testContext)
        {
            ClassState[IsAuthenticatedKey] = false;
            ClassState[WebDriverKey] = null;
        }

        protected void DestroyDriver()
        {
            var driver = ClassState[WebDriverKey] as IWebDriver;
            try
            {
                if (driver != null)
                {
                    driver.Quit();
                }
            }
            catch
            {
                // driver quit failure. No recovery option.
                driver.Dispose();
            }
            finally
            {
                ClassState[WebDriverKey] = null;
                ClassState[IsAuthenticatedKey] = false;
            }
        }

        private void KillDriverProccess()
        {
            try
            {
                Process[] AllProcesses = Process.GetProcesses();
                foreach (var process in AllProcesses)
                {
                    if (process.MainWindowTitle != "")
                    {
                        string s = process.ProcessName.ToLower();
                        if (s == "phantomjs" || s == "firefox" || s == "geckodriver" || s == "chromedriver")
                        {
                            process.Kill();
                        }
                    }
                }
            }
            catch
            { }
        }

        //private void SetRedis()
        //{
        //    Trace.WriteLine(ConfigurationManager.AppSettings.Get("RedisCacheSuffix"));
        //    Trace.WriteLine(ConfigurationManager.AppSettings.Get("RedisConnectionString"));
        //    _container.RegisterType<ITierCache, RedisCache>(new InjectionConstructor());
        //    setUpCache = new CacheAdapter(_container.Resolve<ITierCache>());
        //}


        protected virtual IEnumerable<Func<Task>> Preconditions()
        {
            return Enumerable.Empty<Func<Task>>();
        }
        public string getProjectPath()
        {
            string getExtensionFilePath = TestContext.Parameters["ExtensionFilePath"];
            var path = System.Reflection.Assembly.GetCallingAssembly().CodeBase;
            string actualPath;
            if (path.Contains("/bin"))
                actualPath = path.Substring(0, path.LastIndexOf("bin"));
            else
                actualPath = path.Replace("Shop.Automation.Framework.DLL", "");
            var projectPath = new Uri(actualPath).LocalPath;
            return projectPath;
        }
        #region LogIn
        /// <summary>
        /// Log to Home Page using Admin or directly on test page. If UseAdminToLogIn in app.config contains "y", the method will try using Admin
        /// </summary>
        protected virtual void LogIn()
        {
            if (ConfigurationManager.AppSettings.Get("UseAdminToLogIn").ToLower().Trim().Contains("y") && !IsAuthenticated)
            {
                EnterCredentialsInAdminPage();

                CurrentPage.LoadPage();

                //Only after logging in, it is expected to have the accept cookies popup
                CurrentPage.AcceptCookies();
            }
            else if (IsAuthenticated)
            {
                CurrentPage.LoadPage();
            }
            else
            {
                string url = CurrentPage.Land();

                //For env the cookies pop up is not always
                bool cookiesAlreadyAccepted = CurrentPage.AcceptCookies();

                CurrentPage.GoToUrlAuthenticated(url, CurrentPage.RunSettings.ValidUser);
                CurrentPage.Driver.WaitForDocumentReady();
                CurrentPage.WaitForElementGone(CommonFindBy.BusyIndicator);
                IsAuthenticated = true;

                //Only after logging in, it is expected to have the accept cookies popup
                if (!cookiesAlreadyAccepted)
                    CurrentPage.AcceptCookies();
            }
        }


        /// <summary>
        /// Log in to land at Home Page always using Admin.
        /// </summary>
        protected virtual void LogInUsingOnlyAdmin()
        {
            if (IsAuthenticated)
            {
                CurrentPage.LoadPage();
            }
            else
            {
                EnterCredentialsInAdminPage();

                CurrentPage.LoadPage();

                //Only after logging in, it is expected to have the accept cookies popup
                CurrentPage.AcceptCookies();
            }
        }


        /// <summary>
        /// Try to log in using Admin Page. If it fails, the method will retry the number of times set up in NoOfRetriesInAdmin in app.config.
        /// </summary>
        protected void EnterCredentialsInAdminPage()
        {
            int numberOfTries = 0;
            int maxNumberOfTries = Convert.ToInt16(ConfigurationManager.AppSettings.Get("NoOfRetriesInAdmin"));

            do
            {
                try
                {
                    numberOfTries++;
                    IsAuthenticated = CurrentPage.LoginToAdminUrl(CurrentPage.RunSettings.Catalog_Authentication);
                }
                catch (Exception)
                {
                    IsAuthenticated = false;
                    DestroyDriver();
                    TestInitialize();
                }

            } while (!IsAuthenticated && numberOfTries < maxNumberOfTries);

            if (!IsAuthenticated)
                throw new Exception("I was not able to log in :(. Try with another user or later....");
        }
        public string Capture(IWebDriver driver, string screenShotName)
        {
            ITakesScreenshot ts = (ITakesScreenshot)driver;
            Screenshot screenshot = ts.GetScreenshot();
            var path = System.Reflection.Assembly.GetCallingAssembly().CodeBase;
            string actualPath;
            if (path.Contains("/bin"))
                actualPath = path.Substring(0, path.LastIndexOf("bin"));
            else
                actualPath = path.Replace("Shop.Automation.Framework.DLL", "");
            var projectPath = new Uri(actualPath).LocalPath;
            var folderName = "Reports\\ErrorScreenshots" + DateTime.Now.ToString("ddMMyyyy");
            Directory.CreateDirectory(projectPath.ToString() + folderName);
            Directory.SetCurrentDirectory(projectPath.ToString());
            string finalpth = projectPath.ToString() + folderName + "\\" + screenShotName + ".png";
            string localpath = ".\\" + folderName + "\\" + screenShotName + ".png";
            screenshot.SaveAsFile(localpath, ScreenshotImageFormat.Png);
            return localpath;
        }
        #endregion

        #region Reports
        private void ReportFileCreation()
        {
            if (CreateReport())
            {
                //TODO: Find a way to lock the resource or use TearSetUp to run this code block only one time.
                string reportPath = reportPathPrefix + GetTargetEnvironmentName() + reportPathSubfix;
                string reportName = GetGeneralReportName();
                string fileName = reportPath + reportName;

                _readWriteLock.EnterWriteLock();
                //File Management
                if (!Directory.Exists(reportPath))
                {
                    Debug.WriteLine("Creating Directory");
                    Directory.CreateDirectory(reportPath);
                }

                //Excel writing
                GeneralReportManager generalReport = null;
                try
                {
                    //If the file does not exist create a new excel, if exist set up the Application to use the first sheet
                    if (!File.Exists(fileName + ".xlsx"))
                    {
                        Debug.WriteLine("Calling GeneralReportManager to create excel file");
                        generalReport = new GeneralReportManager(fileName);
                    }
                }
                catch (Exception Ex)
                {
                    Debug.WriteLine(Ex.ToString());
                }
                finally
                {
                    _readWriteLock.ExitWriteLock();
                    if (generalReport != null)
                        generalReport.CloseExcelFile();
                }
            }
        }

        private void AddTCToReport()
        {
            if (CreateReport() && !TestContext.CurrentContext.Result.Outcome.Status.ToString().ToLower().Equals("skipped"))
            {
                //Path is created as {projectPath}/Env/mont.day.year/report/excel
                string reportPath = reportPathPrefix + GetTargetEnvironmentName() + reportPathSubfix;
                string reportName = GetGeneralReportName();
                string fileName = reportPath + reportName;

                //Row Info
                List<string> elementRow = new List<string>
            {
                GetTCClassName(),
                CurrentPage.RunSettings.Locale ?? "NULL",
                GetTCName(),
                NUnit.Framework.TestContext.CurrentContext.Result.Outcome.Status.ToString() ?? "NULL",
                "'" + GetExecutionElapseTime(),
                TestContext.CurrentContext.Result.Message ?? ""
            };

                //Excel writing
                _readWriteLock.EnterWriteLock();
                GeneralReportManager generalReport = null;
                try
                {
                    //create GeneralReportManager instance and set the path location
                    generalReport = new GeneralReportManager { Path = fileName };

                    generalReport.AddAReportRow(elementRow);

                    generalReport.SaveExcelFile();
                }
                catch (Exception)
                {
                    //Console.WriteLine(Ex.ToString());
                    throw;
                }
                finally
                {
                    _readWriteLock.ExitWriteLock();
                    generalReport.CloseExcelFile();
                }
            }
        }

        private string GetTCName()
        {
            string tcName = "";
            string parameters = "";

            if (TestContext.CurrentContext.Test.Arguments.Count() > 0)
            {
                parameters = "(";

                foreach (object parameter in TestContext.CurrentContext.Test.Arguments)
                {
                    parameters += parameter + ",";
                }

                parameters = parameters.Substring(0, parameters.Length - 1);
                parameters += ")";


            }

            tcName = TestContext.CurrentContext.Test.MethodName + parameters;

            return tcName;
        }

        private string GetTCClassName()
        {
            string className = "";
            //if the TC is part of cart the class is the name of the class and not the Page
            if (TestContext.CurrentContext.Test.ClassName.Contains("Shop.Automation.Acceptance.Cart"))
            {
                //remove the locale, it is asummed that the locale is the last 4 laters of the class name
                string locale = CurrentPage.RunSettings.Locale.Replace("-", "");
                string cartClassName = TestContext.CurrentContext.Test.ClassName.Trim().Replace(locale, "").Trim();

                //Remove the last character if it is a "_"
                cartClassName = cartClassName.Substring(cartClassName.Length - 1).Equals("_") ? cartClassName.Substring(0, cartClassName.Length - 1) : cartClassName;

                //get the name
                string[] classFullName = cartClassName.ToString().Split('.');
                className = classFullName[classFullName.Length - 1] ?? "NULL";
            }
            else
            {
                string[] classFullName = CurrentPage.ToString().Split('.');
                className = classFullName[classFullName.Length - 1] ?? "NULL";
            }

            return className.Trim();
        }

        public string GetGeneralReportName()
        {
            var generalReportFileName = (ConfigurationManager.AppSettings.Get(GeneralReportName) ?? "ShopAutomation_Report1").ToString();
            return generalReportFileName;
        }

        public string GetFinalReportName()
        {
            var finalReportFileName = (ConfigurationManager.AppSettings.Get(FinalReportName) ?? "ShopAutomation_FinalReport").ToString();
            return finalReportFileName;
        }

        public bool CreateReport()
        {
            var finalReportFileName = (ConfigurationManager.AppSettings.Get(CreateReportProperty) ?? "N").ToString();
            bool createReport = true;

            if (!finalReportFileName.Trim().ToUpper().StartsWith("Y"))
            {
                createReport = false;
            }
            return createReport;
        }

        #region Stopwath
        private void StartExcetutionTimeCounting()
        {
            if (CreateReport())
            {
                tcStopWatch = Stopwatch.StartNew();
            }
        }


        /// <summary>
        /// Stop the timer.
        /// </summary>
        private void StopExecutionTimeCounting()
        {
            if (CreateReport())
            {
                try
                {
                    tcStopWatch.Stop();
                }
                catch (Exception)
                {
                    Debug.WriteLine("Could not stop StopWathc.... check that it was initialized.");
                }
            }
        }

        /// <summary>
        /// Return the exetion time with hh:mm:ss format
        /// </summary>
        /// <returns></returns>
        private string GetExecutionElapseTime()
        {
            try
            {
                return String.Format("{0:hh\\:mm\\:ss}", tcStopWatch.Elapsed.ToString());
            }
            catch (Exception)
            {

                return "00:00:00";
            }
        }
        #endregion
        #endregion

        #region cookie compliance
        protected IWebDriver SetCookieCompliance(IWebDriver dr)
        {
            dr.WaitForDocumentReady();
            dr.SwitchTo().Window(dr.WindowHandles[0]);
            dr.FindElement(By.XPath("//a[@href='/rules/editor/create/Redirect']")).Click();
            Thread.Sleep(1000);
            dr.FindElement(By.XPath("//input[@name='name']")).SendKeys("Adobe Tag Manager");
            dr.FindElement(By.XPath("//input[@placeholder='e.g. google']")).SendKeys("satelliteLib-12930be22558042bc632cff190e4776deb189a2a.js");
            dr.FindElement(By.XPath("(//input[@class='ant-input'])[3]")).SendKeys("https://assets.adobedtm.com/78ef23cd3941/4d66435cf9ad/launch-6c390220da59-development.min.js");
            dr.FindElement(By.XPath("//button[@class='ant-btn ant-btn-primary btn-icon btn-3']")).Click();
            Thread.Sleep(1000);
            dr.FindElement(By.XPath("//a[@href='/rules/my-rules']")).Click();
            Thread.Sleep(2000);
            //dr.FindElement(By.XPath("//a[@href='/rules/editor/create/Cancel']")).Click();
            //Thread.Sleep(2000);
            //dr.FindElement(By.XPath("//input[@name='name']")).SendKeys("remove the old legacy cookie");
            //dr.FindElement(By.XPath("//input[@placeholder='e.g. google']")).SendKeys("optanon.blob.core.windows.net");
            //dr.FindElement(By.XPath("//button[@class='ant-btn ant-btn-primary btn-icon btn-3']")).Click();
            //Thread.Sleep(2000);
            dr.Close();
            dr.SwitchTo().Window(dr.WindowHandles[0]);
            return dr;
        }
        #endregion
    }

}
