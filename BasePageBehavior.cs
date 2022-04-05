namespace Shop.Automation.Framework
{
    public class BasePageBehavior
    {
        public DriverInstantiation DriverInstantiation { get; set; }
    }

    public enum DriverInstantiation
    {
        EveryTest,
        UseExisting
    }

	public class LocaleBasedPageBehavior : BasePageBehavior
	{
		public string Locale { get; set; }
	}
}