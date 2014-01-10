
using org.openqa.selenium;


namespace ParserHelpers
{
    public class Auths
    {
        public Auths(WebDriver driver, By loginBy, By passBy, By buttonBy)
        {
            _driver = driver;
            _loginButton = _driver.findElement(buttonBy);
            _login = _driver.findElement(loginBy);
            _pass = _driver.findElement(passBy);
        }

        private WebDriver _driver;
        private WebElement _loginButton;
        private WebElement _login;
        private WebElement _pass;

        public void LogIn(string login,string pass)
        {
            _login.sendKeys(login);
            _pass.sendKeys(pass);
            _loginButton.click();
        }

        public static void LogOut(WebDriver driver, By logOutBy)
        {
            WebElement logout = driver.findElement(logOutBy);
            logout.click();
        }
    }
}
