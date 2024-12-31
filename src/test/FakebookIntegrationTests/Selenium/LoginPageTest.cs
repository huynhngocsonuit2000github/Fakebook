using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace FakebookIntegrationTests.Selenium;
public class LoginPageTest
{

    [Fact]
    [Trait("DisplayName", "LoginTests")]
    public void Test_LoginPage()
    {
        // Initialize WebDriver
        IWebDriver driver = new ChromeDriver();

        try
        {
            // Navigate to the Login page
            driver.Navigate().GoToUrl("https://intranet.tma.com.vn/");

            // Maximize the browser window
            driver.Manage().Window.Maximize();

            // Locate the username field and enter text
            var usernameField = driver.FindElement(By.Id("username")); // Adjust selector as needed
            usernameField.SendKeys("hnson");

            // Locate the password field and enter text
            var passwordField = driver.FindElement(By.Id("password")); // Adjust selector as needed
            passwordField.SendKeys("SOn01698182219####");

            // Locate and click the login button
            var loginButton = driver.FindElement(By.XPath("//input[@name=\"submit\"]")); // Adjust selector as needed
            loginButton.Click();

            // Assert successful login
            var successMessage = driver.FindElement(By.XPath("//img[@src='https://intranet.tma.com.vn/images/emp_images/small_new/220525.jpg']")); // Adjust selector as needed
            Assert.NotNull(successMessage);

            // Output success
            Console.WriteLine("Login test passed!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Test encountered an error: {ex.Message}");
            throw;
        }
        finally
        {
            // Close the browser
            driver.Quit();
        }
    }
}
