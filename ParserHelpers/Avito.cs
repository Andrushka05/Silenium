using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ikvm.extensions;
using OpenQA.Selenium;
using org.openqa.selenium;
using org.openqa.selenium.firefox;
using org.openqa.selenium.htmlunit;
using org.openqa.selenium.ie;
using By = org.openqa.selenium.By;

//using OpenQA.Selenium;
//using OpenQA.Selenium.Chrome;
//using OpenQA.Selenium.Firefox;
//using OpenQA.Selenium.Remote;


namespace ParserHelpers
{
    public class Avito
    {
        public static List<string> CityList()
        {

            return null;
        }

        public static List<string> CategoryList()
        {

            return null;
        }

        public static List<string> MetroList()
        {

            return null;
        }

        public Avito()
        {
            //_driver = new FirefoxDriver();
            //_driver = new RemoteWebDriver(DesiredCapabilities.HtmlUnitWithJavaScript());
            //_driver.Navigate().GoToUrl("http://www.avito.ru/");
            _driver=new HtmlUnitDriver();
            _driver.get("http://www.avito.ru");
        }
        
        private WebDriver _driver;
        public void Login(string email,string pass)
        {
            //IWebDriver driver = new RemoteWebDriver(DesiredCapabilities.HtmlUnitWithJavaScript());
            //driver.Navigate().GoToUrl("http://www.avito.ru/");
            //var loginIn = _driver.FindElement(By.LinkText("Войти"));
            ////https://www.avito.ru/profile/login?next=%2Fprofile
            //_driver.Navigate().GoToUrl(loginIn.GetAttribute("href"));

            //var login=new Auths(_driver,By.Name("login"),By.Name("password"),By.PartialLinkText("Войти"));
            //login.LogIn(email,pass);

        }



        public List<Av> GetAdList(string url)
        {

            var adList = new List<Av>();
            for (int i = 1; ; i++)
            {
                if (i == 1)
                    _driver.get(url);
                else
                    _driver.get(url + "?p=" + i);

                //Получаем ссылки на объявления
                var adLinks = _driver.findElements(By.xpath("//h3[@class='title']/a"));
                var links = adLinks.toArray();//.Select>(x => x.GetAttribute("href")).ToList();
                foreach (var link in links)
                {
                    //Thread.Sleep(1000);
                    //_driver.Navigate().GoToUrl(link.Replace("www", "m"));
                    var l = ((WebElement)link).ToString();
                    var begin = l.indexOf("href");
                    var href = l.Substring(begin + 5, l.IndexOf("\"", begin + 8) - begin - 5).Replace("\"", "").Trim();
                    if (href.Contains("www.avito.ru"))
                        href=href.Replace("www", "m");
                    else
                        href = "https://m.avito.ru" + href;
                    _driver.get(href);
                    string name = "";
                    var avito = new Av();
                    avito.Url = href.Replace("/m.","/www.");
                    try
                    {
                        WebElement p = _driver.findElement(By.xpath("//li[contains(concat(' ', @class, ' '), 'action-show-number')]/a"));
                        p.click();
                        //Thread.Sleep(500);
                        var html = new HtmlAgilityPack.HtmlDocument();
                        html.LoadHtml(_driver.getPageSource());

                        var price = HtmlAgility.GetItemInnerText(html, "//div[contains(concat(' ', @class, ' '), 'info-price')]");
                        var title = HtmlAgility.GetItemsInnerText(html, "//header[contains(concat(' ', @class, ' '), 'single-item-header')]/span", "", null, " ");
                        var desc = HtmlAgility.GetItemInnerText(html, "//div[contains(concat(' ', @class, ' '), 'description-wrapper')]");
                        var city = HtmlAgility.GetItemInnerText(html, "//div[contains(concat(' ', @class, ' '), 'address-person-params')]/span");
                        var id = HtmlAgility.GetItemInnerText(html, "//span[contains(concat(' ', @class, ' '), 'item-id')]");
                        var countView = HtmlAgility.GetItemInnerText(html, "//span[contains(concat(' ', @class, ' '), 'item-view-count')]");
                        var date = HtmlAgility.GetItemInnerText(html, "//span[contains(concat(' ', @class, ' '), 'item-add-date')]");
                        var image = HtmlAgility.GetPhoto(html, "", "//li[contains(concat(' ', @class, ' '), 'photo-container')]/img");
                        var contactName = HtmlAgility.GetItemInnerText(html, "//div[contains(concat(' ', @class, ' '), 'person-contact-name')]");
                        var shopName = HtmlAgility.GetItemInnerText(html, "//div[contains(concat(' ', @class, ' '), 'person-name')]");
                        var catalogPath = HtmlAgility.GetItemsInnerText(html, "//span[contains(concat(' ', @class, ' '), 'param')]", "", null, "/");
                        name=HtmlAgility.GetItemInnerText(html, "//div[contains(concat(' ', @class, ' '), 'person-name')]");
                        var phone=HtmlAgility.GetItemInnerText(html, "//li[contains(concat(' ', @class, ' '), 'action-show-number')]/a");
                        if (date.ToLower().Contains("вчера"))
                            avito.Date =
                                (new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 1))
                                    .ToLongDateString() + " " +
                                date.Substring(date.ToLower().LastIndexOf("в") + 1).Trim();
                        else if (date.ToLower().Contains("вчера"))
                            avito.Date =
                                (new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day))
                                    .ToLongDateString() + " " +
                                date.Substring(date.ToLower().LastIndexOf("в") + 1).Trim();
                        else
                            avito.Date = date.Substring(date.ToLower().LastIndexOf("в") + 1).Trim();

                        if (shopName.ToLower().Contains("магазин"))
                            avito.ShopName = shopName.Replace("(магазин)", "").Trim();

                        avito.Catalog = catalogPath;
                        avito.ContactName = contactName.Replace("(Контактное лицо)", "").Trim();
                        avito.Photos = image;
                        avito.CountShow = countView.Replace("Просмотров:", "").Trim();
                        avito.Article = id.Replace("Объявление", "").Replace("№", "").Trim();
                        avito.City = city.Trim();
                        avito.Description = desc.Trim();
                        avito.Title = HtmlAgility.ReplaceWhiteSpace(title.Replace("\r", "").Replace("\n", "").Trim());
                        avito.Author = HtmlAgility.ReplaceWhiteSpace(name.Replace("\r","").Replace("\n","").Trim());//.getText().Trim();
                        avito.Price = price.Trim();
                        avito.Phone = phone.Replace(" ", "").Replace("-", "");
                        
                    }
                    catch (Exception)
                    {
                    }
                    adList.Add(avito);
                }
            }
            return adList;
        } 

        public void PlaceAd()
        {
            //var placeAdButt = _driver.FindElement(By.Name("Подать объявление"));
            //http://www.avito.ru/additem

        }

        //public List<Av> 
    }

    public class Av:Item
    {
        public string Phone { get; set; }
        public string Author { get; set; }
        public string City { get; set; }
        public string Metro { get; set; }
        public string ContactName { get; set; }
        public string CountShow { get; set; }
        public string Date { get; set; }
        public string ShopName { get; set; }
   }

}
