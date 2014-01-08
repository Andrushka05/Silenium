using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace ParserHelpers
{
    public class Auths
    {
        public Auths(IWebDriver driver, By loginBy, By passBy, By buttonBy)
        {
            _driver = driver;
            _loginButton = _driver.FindElement(buttonBy);
            _login = _driver.FindElement(loginBy);
            _pass = _driver.FindElement(passBy);
        }

        private IWebDriver _driver;
        private IWebElement _loginButton;
        private IWebElement _login;
        private IWebElement _pass;

        public void LogIn(string login,string pass)
        {
            _login.SendKeys(login);
            _pass.SendKeys(pass);
            _loginButton.Click();
        }

        public static void LogOut(IWebDriver driver, By logOutBy)
        {
            IWebElement logout = driver.FindElement(logOutBy);
            logout.Click();
        }
    }
}
