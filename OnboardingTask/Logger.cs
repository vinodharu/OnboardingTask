using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using log4net;
using log4net.Config;

namespace SaucedemoAutomation
{
    [TestFixture]
    public class AddToCartTest
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AddToCartTest));
        private IWebDriver driver;

        [SetUp]
        public void Setup()
        {
            XmlConfigurator.Configure();
            log.Info("Setting up the WebDriver and navigating to the site.");
            driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://www.saucedemo.com/");
        }

        [Test]
        public void AddItemToCartTest()
        {
            log.Info("Attempting login.");

            driver.FindElement(By.Id("user-name")).SendKeys("standard_user");
            driver.FindElement(By.Id("password")).SendKeys("secret_sauce");
            driver.FindElement(By.Id("login-button")).Click();

            Assert.That(driver.Url, Does.Contain("inventory.html"), "Login failed.");
            log.Info("Login successful.");

            log.Info("Adding an item to the cart.");
            IWebElement addToCartButton = driver.FindElement(By.CssSelector("button[name='add-to-cart-sauce-labs-backpack']"));
            addToCartButton.Click();

            IWebElement cartBadge = driver.FindElement(By.ClassName("shopping_cart_badge"));
            Assert.That(cartBadge.Text, Is.EqualTo("1"), "Item was not added to the cart.");
            log.Info("Item successfully added to the cart.");

            log.Info("Navigating to the cart.");
            driver.FindElement(By.ClassName("shopping_cart_link")).Click();

            Assert.That(driver.Url, Does.Contain("cart.html"), "Failed to navigate to cart page.");
            log.Info("Cart page loaded successfully.");
        }

        [TearDown]
        public void TearDown()
        {
            log.Info("Closing the WebDriver.");
            driver.Quit();
        }
    }
}
