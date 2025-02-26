using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SaucedemoAutomation
{
    public class ProductCollection : IEnumerable<string>
    {
        private List<string> _products = new List<string>();

        public void Add(string product)
        {
            _products.Add(product);
        }

        public IEnumerator<string> GetEnumerator()
        {
            return _products.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    [TestFixture]
    public class IEnumerableCollection
    {
        private IWebDriver driver;

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://www.saucedemo.com/");
        }

        [Test]
        public void AddMultipleItemsToCartTest()
        {
            driver.FindElement(By.Id("user-name")).SendKeys("standard_user");
            driver.FindElement(By.Id("password")).SendKeys("secret_sauce");
            driver.FindElement(By.Id("login-button")).Click();

            ProductCollection productsToAdd = new ProductCollection();
            productsToAdd.Add("sauce-labs-backpack");
            productsToAdd.Add("sauce-labs-bike-light");

            ArrayList addedProducts = new ArrayList();

            foreach (var product in productsToAdd)
            {
                IWebElement addToCartButton = driver.FindElement(By.CssSelector($"button[name='add-to-cart-{product}']"));
                addToCartButton.Click();
                addedProducts.Add(product);
            }

            Dictionary<string, string> productDisplayNames = new Dictionary<string, string>
            {
                { "sauce-labs-backpack", "Sauce Labs Backpack" },
                { "sauce-labs-bike-light", "Sauce Labs Bike Light" }
            };

            driver.FindElement(By.ClassName("shopping_cart_link")).Click();

            Assert.That(driver.Url, Does.Contain("cart.html"), "Failed to navigate to cart page.");

            foreach (var product in addedProducts)
            {
                Assert.That(driver.PageSource, Does.Contain(productDisplayNames[product.ToString()]), $"{productDisplayNames[product.ToString()]} not found in cart.");
            }
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }
    }
}