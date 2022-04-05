namespace Shop.Automation.Framework
{
    public interface ITestPageFactory
    {
        T Create<T>() where T : Page, new();
    }
}
