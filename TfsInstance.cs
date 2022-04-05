using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.TestManagement.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Shop.Automation.Framework
{
    public class TfsInstance
    {
        public TfsTeamProjectCollection teamCollection;
        public ITestManagementTeamProject project;
        public ITestPlan plan;
        public Dictionary<string, ITestRun> runs = new Dictionary<string, ITestRun>();

        private static readonly Lazy<TfsInstance>
        lazy =
        new Lazy<TfsInstance>
            (() => new TfsInstance());

        public static TfsInstance Instance { get { return lazy.Value; } }

        private TfsInstance()
        {
        }

        public void CreateTfsConnection()
        {
            teamCollection = TfsTeamProjectCollectionFactory.GetTeamProjectCollection(new Uri("https://tfs.dev.myhrbl.com/tfs/MTS"));
            project = teamCollection.GetService<ITestManagementService>().GetTeamProject("Shop");
            plan = project.TestPlans.Find(Convert.ToInt32(TestContext.Parameters.Get("TfsSuiteId")));
        }
        public void CreateTfsTestRun(string locale)
        {
            runs.Add(locale, plan.CreateTestRun(true));
            runs[locale].Title = "Automation_" + locale;
        }

        public void Dispose()
        {
            teamCollection.Dispose();
        }
    }
}
