using System;
using NUnit.Framework;
using DotNetSelenium.PageObjects;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using CoreUtilities;
using DotNetSelenium.Listeners;
using DotNetSelenium.Utilities;
using Newtonsoft.Json.Linq;

namespace DotNetSelenium.TestCases
{
    [TestFixture]
    public class Tests : TestListener
    {

        private IWebDriver? driver;
        //private TestBase testBase;
        private LoginPage? loginPage;
        private SubstorePage? substorePage;
        
    [OneTimeSetUp]
    public void OneTimeSetup()
        {
            driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://healthapp.yaksha.com/");
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);

            // Initialize the pages
            loginPage = new LoginPage(driver);
            substorePage = new SubstorePage(driver);
            
            // Perform login
            loginPage.PerformLogin();
        }
        
        [Test,Order(1)]
        public void Test_SubstoreTabNavigationAndUrlVerification()
        {
            // Act
            string currentUrl = substorePage.ScrollToSubstoreTabAndVerifyUrl();

            // Assert
            Assert.IsTrue(currentUrl.Contains("WardSupply"), 
                $"Expected URL to contain 'WardSupply', but got: {currentUrl}");
        }

         [Test,Order(2)]
        public void Test_ClickFourthCounterIfAvailable()
        {
            try
            {
                bool result = substorePage.ClickFourthCounterIfAvailable();
                Assert.IsTrue(result, "Expected to click the fourth counter successfully.");
            }
            catch (Exception ex)
            {
                Assert.Fail("Test failed due to exception: " + ex.Message);
            }
        }

    [Test,Order(3)]
        public void Test_VerifyModuleSignoutHoverText()
        {
            var expectedData = new Dictionary<string, string>
            {
                { "moduleSignOutHoverText", "To change, you can always click here." }
            };

            try
            {
                bool result = substorePage.VerifyModuleSignoutHoverText(expectedData);
                Assert.IsTrue(result, "Expected hover text to match the expected value.");
            }
            catch (Exception ex)
            {
                Assert.Fail("Test failed due to exception: " + ex.Message);
            }
        }

        [Test,Order(4)]
        public void TC_VerifySubstoreSubModules_AreAccessible()
        {
            // Arrange: Create test data for expected values
            var substoreExpectedData = new Dictionary<string, string>
            {
                { "URL", "https://your-app-url/#/WardSupply" } // Update this to the actual expected URL if necessary
            };

            try
            {
                // Act
                bool result = substorePage.VerifySubstoreSubModule(substoreExpectedData);

                // Assert
                Assert.IsTrue(result, "The Inventory and Pharmacy sub-modules should be accessible and clickable.");
            }
            catch (Exception ex)
            {
                Assert.Fail("Test failed with exception: " + ex.Message);
            }
        }

        [Test,Order(5)]
        public void TC_VerifySubModulesUnderInventory_AreDisplayed()
        {
            try
            {
                bool result = substorePage.SubModulePresentInventory();
                Assert.IsTrue(result, "Sub-modules under Inventory should be displayed.");
                Console.WriteLine("Test Passed: Sub-modules under Inventory are displayed.");
            }
            catch (Exception ex)
            {
                Assert.Fail("Test Failed due to exception: " + ex.Message);
            }
        }

        [Test,Order(6)]
        public void VerifyNavigationBetweenSubmodules_ShouldNavigateAllSuccessfully()
        {
            // Act
            bool result = substorePage.VerifyNavigationBetweenSubmodules();

            // Assert
            Assert.IsTrue(result, "Navigation between submodules did not complete successfully.");
        }

        [Test,Order(7)]
            public void Test_TakingScreenshotOfTheCurrentPage()
            {
                // Act
                bool result = substorePage.TakingScreenshotOfTheCurrentPage();

                // Assert
                Assert.IsTrue(result, "Screenshot capture failed.");
            }

        [Test,Order(8)]
        public void Test_UIComponentsVisibility_OnInventoryRequisitionPage()
        {
            try
            {
                bool result = substorePage.VerifyInventoryRequisitionUIElements();
                Assert.IsTrue(result, "Not all required UI components were visible on the Inventory Requisition page.");
            }
            catch (Exception ex)
            {
                Assert.Fail("Exception occurred while verifying UI components: " + ex.Message);
            }
        }

        [Test,Order(9)]
        public void Test_CreateRequisition_SuccessMessageDisplayed()
        {
            try
            {
                string expectedMessage = "Requisition is Generated and Saved";
                string actualMessage = substorePage.VerifyCreateRequisitionButton();

                Assert.IsTrue(actualMessage.Contains(expectedMessage),
                    $"Expected message to contain: '{expectedMessage}', but got: '{actualMessage}'");
            }
            catch (Exception ex)
            {
                Assert.Fail("Failed while verifying requisition creation: " + ex.Message);
            }
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            if (driver != null)
            {
                driver.Quit();
                driver.Dispose();
                driver = null;
            }
        }


        /**
 * ------------------------------------------------------Helper Methods----------------------------------------------------
 */

        public void VerifyUserIsLoggedIn(IWebDriver driver)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//li[@class='dropdown dropdown-user']")));
            Assert.IsTrue(driver.FindElement(By.XPath("//li[@class='dropdown dropdown-user']")).Displayed);
        }

        public void VerifyUserIsLoggedOut(IWebDriver driver)
        {
            Assert.IsTrue(driver.FindElement(By.Id("login")).Displayed);
        }

        public void VerifyVisitType(IWebDriver driver)
        {
            var tableElements = driver.FindElements(By.CssSelector("div[col-id='AppointmentType']"));
            Assert.Greater(tableElements.Count, 1);
        }

        public void VerifyUserIsOnCorrectURL(IWebDriver driver, string expectedURL)
        {
            string actualURL = driver.Url;
            Assert.IsTrue(actualURL.Contains(expectedURL));
        }

        public void VerifyImageIsUploaded(IWebDriver driver)
        {
            Assert.IsTrue(driver.FindElement(By.CssSelector("div.wrapper img")).Displayed);
        }

        public void IsTooltipDisplayed(IWebDriver driver)
        {
            Assert.IsTrue(driver.FindElement(By.CssSelector("div.modal-content")).Displayed);
        }

        public void VerifyErrorMessage(IWebDriver driver)
        {
            Assert.IsTrue(driver.FindElement(By.XPath("//span[text()='Select doctor from the list.']")).Displayed);
        }

        public void OTBookingModalIsDisplayed(IWebDriver driver)
        {
            Assert.IsTrue(driver.FindElement(By.CssSelector("div.modelbox-div")).Displayed);
        }

        public void VerifyIfRecordsArePresent(IWebDriver driver)
        {
            var records = driver.FindElements(By.CssSelector("div[col-id='PatientName']"));
            Assert.Greater(records.Count, 1);
        }

        public void VerifyTdsTest(IWebDriver driver)
        {
            var patientNames = driver.FindElements(By.CssSelector("div[col-id='FullName']"));
            Assert.IsTrue(patientNames[1].Text.Contains("Rakesh"));
        }


    }
}