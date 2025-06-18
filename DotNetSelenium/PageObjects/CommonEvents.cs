using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;
using System;
using System.IO;

public class CommonEvents
{
    private readonly IWebDriver _driver;
    private readonly WebDriverWait _wait;
    private readonly IJavaScriptExecutor _jsExecutor;

    public CommonEvents(IWebDriver driver, int timeoutInSeconds = 10)
    {
        _driver = driver; // ✅ Now we're assigning the constructor parameter to the private field
        _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutInSeconds));
        _jsExecutor = (IJavaScriptExecutor)_driver;
    }
// Write the most common functions here to reuse

}
