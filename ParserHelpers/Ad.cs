using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using org.openqa.selenium;
using org.openqa.selenium.htmlunit;
using org.openqa.selenium.remote;

namespace ParserHelpers
{
    public abstract class Ad<T>:IAd<T>
    {
        protected WebDriver _driver;
        public abstract void Login(string email, string pass);
        public abstract void LogOut();
        public abstract List<T> GetAdList(string url, ProgressBar progress, ref string error);
        public abstract bool PlaceAd(List<T> adList);
        public abstract List<Link> CategoryList(string link);
        public abstract List<Link> CityList();
        public static WebDriver InitWebDriver()
        {
            DesiredCapabilities capabilities = DesiredCapabilities.firefox();
            capabilities.setBrowserName("firefox"); 
            capabilities.setPlatform(Platform.LINUX); 
            capabilities.setVersion("3.6");
            //capabilities.setBrowserName("Mozilla/5.0 (X11; Linux x86_64; rv:24.0) Gecko/20100101 Firefox/24.0");
            //capabilities.setVersion("24.0");
            //capabilities.setJavascriptEnabled(true);
            WebDriver driver = new HtmlUnitDriver(capabilities);
            return driver;
        }

    }
}
