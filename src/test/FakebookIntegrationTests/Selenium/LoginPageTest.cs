using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Xunit;

namespace FakebookIntegrationTests.Selenium;

public class LoginPageTest
{
    [Fact]
    [Trait("DisplayName", "Test_LoginPage_ExternalUser")]
    public void Test_LoginPage_ExternalUser()
    {
        var options = new ChromeOptions();
        options.AddArguments("--no-sandbox");
        options.AddArguments("--disable-dev-shm-usage");
        options.AddArguments("--headless"); // if running headless
        var driver = new ChromeDriver(options);


        try
        {
            driver.Navigate().GoToUrl("http://192.168.50.10:31200/");
            driver.Manage().Window.Maximize();

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            var successMessage = wait.Until(d => d.FindElement(By.XPath("//h2[@class='error-code']")));
            Assert.Equal("401 - Unauthorized Access", successMessage.Text);

            var loginPageRedirectButton = wait.Until(d => d.FindElement(By.XPath("//a[@class='home-button']")));
            loginPageRedirectButton.Click();

            var usernameField = wait.Until(d => d.FindElement(By.XPath("//input[@formcontrolname='username']")));
            usernameField.SendKeys("son");

            var passwordField = wait.Until(d => d.FindElement(By.XPath("//input[@formcontrolname='password']")));
            passwordField.SendKeys("123");

            var loginButton = wait.Until(d => d.FindElement(By.XPath("//button[@type='submit']")));
            loginButton.Click();

            var headerMessage = wait.Until(d => d.FindElement(By.XPath("//div[@class='member-layout']/nav")));
            Assert.Equal("Member Navigation", headerMessage.Text);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Test encountered an error: {ex.Message}");
            throw;
        }
    }

    [Fact]
    [Trait("DisplayName", "Test_LoginPage_InternalUser")]
    public void Test_LoginPage_InternalUser()
    {
        var options = new ChromeOptions();
        options.AddArguments("--no-sandbox");
        options.AddArguments("--disable-dev-shm-usage");
        options.AddArguments("--headless"); // if running headless
        var driver = new ChromeDriver(options);


        try
        {
            driver.Navigate().GoToUrl("http://192.168.50.10:31200/");
            driver.Manage().Window.Maximize();

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            var successMessage = wait.Until(d => d.FindElement(By.XPath("//h2[@class='error-code']")));
            Assert.Equal("401 - Unauthorized Access", successMessage.Text);

            var loginPageRedirectButton = wait.Until(d => d.FindElement(By.XPath("//a[@class='home-button']")));
            loginPageRedirectButton.Click();

            var idpLoginPageRedirectButton = wait.Until(d => d.FindElement(By.XPath("//button[text()=' Login with IdP ']")));
            idpLoginPageRedirectButton.Click();

            var usernameField = wait.Until(d => d.FindElement(By.XPath("//input[@formcontrolname='username']")));
            usernameField.SendKeys("son_idp");

            var passwordField = wait.Until(d => d.FindElement(By.XPath("//input[@formcontrolname='password']")));
            passwordField.SendKeys("123");

            var loginButton = wait.Until(d => d.FindElement(By.XPath("//button[text()='Log in']")));
            loginButton.Click();

            var headerMessage = wait.Until(d => d.FindElement(By.XPath("//div[@class='member-layout']/nav")));
            Assert.Equal("Member Navigation", headerMessage.Text);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Test encountered an error: {ex.Message}");
            throw;
        }
    }
}
