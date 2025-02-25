using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SaucedemoAutomation
{
    [TestFixture]
    public class LoginTest
    {
        private IWebDriver driver;

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://www.saucedemo.com/");
        }

        [Test]
        public void ValidLoginTest()
        {
            string[] validUsers = { "standard_user", "locked_out_user", "problem_user", "performance_glitch_user" };

            Array.Sort(validUsers);

            Console.WriteLine("Sorted Usernames:");
            foreach (var user in validUsers)
            {
                Console.WriteLine(user);
            }

            string selectedUser = "standard_user";
            int index = Array.IndexOf(validUsers, selectedUser);

            Assert.That(index, Is.GreaterThanOrEqualTo(0), $"User {selectedUser} not found in the list.");

            IWebElement usernameField = driver.FindElement(By.Id("user-name"));
            IWebElement passwordField = driver.FindElement(By.Id("password"));
            IWebElement loginButton = driver.FindElement(By.Id("login-button"));

            usernameField.SendKeys(selectedUser);
            passwordField.SendKeys("secret_sauce");
            loginButton.Click();

            Assert.That(driver.Url, Does.Contain("inventory.html"), "Login failed.");

            Console.WriteLine($"User {selectedUser} logged in successfully.");
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }
    }
}