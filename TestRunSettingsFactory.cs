using Newtonsoft.Json;
using System;
using System.IO;
using NUnit.Framework;

namespace Shop.Automation.Framework
{
    public class TestRunSettingsFactory : ITestRunSettingsFactory
    {
        private readonly string _targetEnvironment;
	    private readonly string _targetLocale;

	    /// <param name="targetEnvironment">The environment name, such as "Local", "QA", "Staging" etc.</param>
	    /// <param name="targetLocale"></param>
	    public TestRunSettingsFactory(string targetEnvironment, string targetLocale)
        {
            if (string.IsNullOrEmpty(targetEnvironment) || string.IsNullOrEmpty(targetLocale))
            {
                throw new ArgumentNullException(nameof(targetEnvironment));

            }
            _targetEnvironment = targetEnvironment;
	        _targetLocale = targetLocale;
		}

		public string SettingsFilePath => $"./TestRunSettings/{_targetEnvironment}/TestRunSettings.{_targetLocale}.json";

		/// <summary>
		/// 
		/// </summary>        
		/// <returns>An instance of <see cref="TestRunSettings"/> 
		/// constructed from a file named TestRunSettings.[targetEnvironment].json</typeparam></returns>
		public TestRunSettings Create()
        {
            var settingsFileFullPath = TestContext.CurrentContext.WorkDirectory + SettingsFilePath;
            if (!File.Exists(settingsFileFullPath))
            {
                throw new FileNotFoundException("Test Run Settins file not found", settingsFileFullPath);
            }
            var result = JsonConvert.DeserializeObject<TestRunSettings>(File.ReadAllText(settingsFileFullPath));
            return result;
        }
    }
}
