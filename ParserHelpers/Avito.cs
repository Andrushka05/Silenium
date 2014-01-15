using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using com.gargoylesoftware.htmlunit;
using com.sun.org.apache.xerces.@internal.impl;
using ikvm.extensions;
using OpenQA.Selenium;
using java.util.concurrent;

using OpenQA.Selenium.Support.UI;
using org.openqa.selenium;
using org.openqa.selenium.android;
using org.openqa.selenium.firefox;
using org.openqa.selenium.htmlunit;
using org.openqa.selenium.ie;
using org.openqa.selenium.@internal.seleniumemulation;
using org.openqa.selenium.phantomjs;
using org.openqa.selenium.remote;
using By = org.openqa.selenium.By;

//using OpenQA.Selenium;
//using OpenQA.Selenium.Chrome;
//using OpenQA.Selenium.Firefox;
//using OpenQA.Selenium.Remote;


namespace ParserHelpers
{
	public class Avito:Ad<Av>
	{
        public Avito()
        {
            //_driver = new FirefoxDriver();
            //_driver = new RemoteWebDriver(DesiredCapabilities.HtmlUnitWithJavaScript());
            //_driver.Navigate().GoToUrl("http://www.avito.ru/");
            _driver = InitWebDriver();
            //_driver.setJavascriptEnabled(true);
            _driver.get("http://m.avito.ru");
            _driver.manage().timeouts().implicitlyWait(30, TimeUnit.SECONDS);
        }
        public override List<Link> CityList()
		{
            var host = "http://m.avito.ru";
            var driver =InitWebDriver();
            driver.get(host);
            var html = new HtmlAgilityPack.HtmlDocument();
            html.LoadHtml(driver.getPageSource());
            var aList = html.DocumentNode.SelectNodes("//a[contains(concat(' ', @href, ' '), 'location')]");
            var res = new List<Link>();
            if (aList != null)
            {
                foreach (var a in aList)
                {
                    var temp = a.Attributes["href"].Value.Trim();
                    var name = a.InnerText.Trim();
                    res.Add(!temp.Contains("avito.ru")
                        ? new Link() { Url = host + temp, Name = name }
                        : new Link() { Url = temp, Name = name });
                }
            }
            return res;
		}
        public override List<Link> CategoryList(string link)
		{
		    var l = link.Insert(link.IndexOf(".ru/") + 4, "catalog/").Replace("location/","");
		    var host = l.Substring(0, l.IndexOf(".ru/") + 3);
		    var driver = InitWebDriver();
            driver.get(l);
		    var html = new HtmlAgilityPack.HtmlDocument();
            html.LoadHtml(driver.getPageSource());
            var aList = html.DocumentNode.SelectNodes("//a[contains(concat(' ', @href, ' '), 'catalog')]");
		    var res = new List<Link>();
		    if (aList != null)
		    {
		        foreach (var a in aList)
		        {
                    var temp = a.Attributes["href"].Value.Replace("catalog/","").Trim();
		            var name = a.InnerText.Trim();
		            res.Add(!temp.Contains("avito.ru")
		                ? new Link() {Url = host + temp, Name = name}
		                : new Link() {Url = temp, Name = name});
		        }
		    }
			return res;
		}
        public override void Login(string email, string pass)
		{
            var loginIn = _driver.findElement(By.linkText("Войти"));
			//https://www.avito.ru/profile/login?next=%2Fprofile
			_driver.get(loginIn.getAttribute("href"));
			var login = new Auths(_driver, By.name("login"), By.name("password"), By.partialLinkText("Войти"));
			login.LogIn(email, pass);
		}
        public override void LogOut()
		{
			//Auths.LogOut(_driver,By.);
		}
        public static int CountAds(string url)
		{
		    var driver = InitWebDriver();
			driver.get(url.replace("m.","www."));
			var countSpan = driver.findElement(By.xpath("//span[contains(concat(' ', @class, ' '), 'catalog_breadcrumbs-count')]"));
			var res = Regex.Replace(countSpan.getText(), @"[^\d]", "");
			var r = 0;
			Int32.TryParse(res, out r);
			return r;
		}
		public override List<Av> GetAdList(string url, ProgressBar progress, ref string error)
		{
			var adList = new List<Av>();
            //progress.Maximum = CountAds(url);
            
            var profile = new OpenQA.Selenium.Firefox.FirefoxProfile();
            //String PROXY = "140.119.24.11:80";
            //var proxy = new OpenQA.Selenium.Proxy
            //{
            //    HttpProxy = PROXY,
            //    FtpProxy = PROXY,
            //    SslProxy = PROXY
            //};
            //profile.SetProxyPreferences(proxy);
            IWebDriver dr = new OpenQA.Selenium.Firefox.FirefoxDriver(profile);
            dr.Navigate().GoToUrl(url);
		    dr.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(120));
			//_driver.get(url);
			int countError = 0;
			for (int i = 1; ; i++)
			{
                try
                {
                    //if (i != 1)
                    //    _driver.get(url + "?p=" + i);
                    //var r = new Random();
                    //var timeout = r.Next(2000, 7000);
                    //Thread.Sleep(timeout);
					//Получаем ссылки на объявления
                    var adLinks = dr.FindElements(OpenQA.Selenium.By.XPath("//article[contains(concat(' ', @class, ' '), 'b-item')]/a"));//_driver.findElements(By.xpath("//article[contains(concat(' ', @class, ' '), 'b-item')]/a"));//(By.xpath("//h3[@class='title']/a"));
					for (int j = 0; j < adLinks.Count; j++)
					{
					    var links =
					        dr.FindElements(OpenQA.Selenium.By.XPath("//article[contains(concat(' ', @class, ' '), 'b-item')]/a")); //_driver.findElements(By.xpath("//article[contains(concat(' ', @class, ' '), 'b-item')]/a"));
					    if (links.Count == 0||links.Count<j)
					    {
					        Thread.Sleep(3000);
                            links =dr.FindElements(OpenQA.Selenium.By.XPath("//article[contains(concat(' ', @class, ' '), 'b-item')]/a"));
					    }
                        var link = links[j];
						//Thread.Sleep(1000);
						//_driver.Navigate().GoToUrl(link.Replace("www", "m"));
                        var l = link.GetAttribute("href");
                        //var begin = l.indexOf("href");
                        //var href =l.Substring(begin + 5, l.IndexOf("\"", begin + 8) - begin - 5).Replace("\"", "").Trim();
                        //if (href.Contains("www.avito.ru"))
                        //    href = href.Replace("www", "m");
                        //else
                        //    href = "https://m.avito.ru" + href;
						link.Click();
                        Thread.Sleep(1000);
						//_driver.get(href);
						var avito = new Av();
						avito.Url = l.Replace("/m.", "/www.");
					    
					    var element =
					        dr.FindElement(OpenQA.Selenium.By.XPath("//div[contains(concat(' ', @class, ' '), 'info-price')]"));//_driver.findElement(By.xpath("//div[contains(concat(' ', @class, ' '), 'info-price')]"));
                        element.Click();
                        
                        //var javascript = new JavascriptLibrary();
                        //javascript.callEmbeddedSelenium(_driver, "triggerEvent", element, "focus");
                        Thread.Sleep(300);
                        //((JavascriptExecutor)_driver).executeScript("return document.getElementsByName('a')[0].focus()");
 
						try
						{
						    try
						    {
						        var p =
						            dr.FindElement(
						                OpenQA.Selenium.By.XPath("//li[contains(concat(' ', @class, ' '), 'action-show-number')]/a"));
						            //_driver.findElement(By.xpath("//li[contains(concat(' ', @class, ' '), 'action-show-number')]/a"));
						        p.Click();
						        var p2 = dr.FindElement(
						            OpenQA.Selenium.By.XPath("//li[contains(concat(' ', @class, ' '), 'action-show-number')]/a"));
						        if (p2.Text.Contains("По") || p2.Text.Contains("Попробуйте"))
						        {
                                    Thread.Sleep(1500);
                                    p =dr.FindElement(
                                        OpenQA.Selenium.By.XPath("//li[contains(concat(' ', @class, ' '), 'action-show-number')]/a"));
                                    p.Click();
						        }
						    }
						    catch (Exception ex)
						    {
                                error += "Не взял номер на странице: " + l + "\r\n";
						    }
                            Thread.Sleep(1000);
						    var html = new HtmlAgilityPack.HtmlDocument();
                            html.LoadHtml(dr.PageSource);
							var price = HtmlAgility.GetItemInnerText(html,"//div[contains(concat(' ', @class, ' '), 'info-price')]");
							var title = HtmlAgility.GetItemsInnerText(html,"//header[contains(concat(' ', @class, ' '), 'single-item-header')]/span", "", null, " ");
							var desc = HtmlAgility.GetItemInnerText(html,"//div[contains(concat(' ', @class, ' '), 'description-wrapper')]");
							var city = HtmlAgility.GetItemsInnerText(html,"//div[contains(concat(' ', @class, ' '), 'address-person-params')]/span","",null,"");
							var id = HtmlAgility.GetItemInnerText(html,
									"//span[contains(concat(' ', @class, ' '), 'item-id')]");
							var countView = HtmlAgility.GetItemInnerText(html,
									"//span[contains(concat(' ', @class, ' '), 'item-view-count')]");
							var date = HtmlAgility.GetItemInnerText(html,	"//span[contains(concat(' ', @class, ' '), 'item-add-date')]");
							var image = HtmlAgility.GetPhoto(html, "","//li[contains(concat(' ', @class, ' '), 'photo-container')]/img");
							var contactName = HtmlAgility.GetItemInnerText(html,
									"//div[contains(concat(' ', @class, ' '), 'person-contact-name')]");
							var shopName = HtmlAgility.GetItemInnerText(html,
									"//div[contains(concat(' ', @class, ' '), 'person-name')]");
							var catalogPath = HtmlAgility.GetItemsInnerText(html,
									"//span[contains(concat(' ', @class, ' '), 'param')]", "", null, "/");
							var name = HtmlAgility.GetItemInnerText(html,
									"//div[contains(concat(' ', @class, ' '), 'person-name')]");
							var phone = HtmlAgility.GetItemInnerText(html,
									"//li[contains(concat(' ', @class, ' '), 'action-show-number')]/a");
							if (date.ToLower().Contains("вчера"))
								avito.Date =
										(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 1))
												.ToLongDateString() + " " +
										date.Substring(date.ToLower().LastIndexOf("в") + 1).Trim();
							else if (date.ToLower().Contains("вчера"))
								avito.Date = (new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day))
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
							avito.Author = HtmlAgility.ReplaceWhiteSpace(name.Replace("\r", "").Replace("\n", "").Trim());
							//.getText().Trim();
							avito.Price = price.Trim();
                            
							avito.Phone = phone.Replace(" ", "").Replace("-", "");
							countError = 0;
							
						}
						catch (Exception ex)
						{
							error +="Ошибка на странице: "+l+ "\r\nlog:"+ex+"\r\n";
							
						}
						adList.Add(avito);
						progress.Value++;
						progress.Refresh();
                        dr.Navigate().Back();
                        Thread.Sleep(800);
                        SaveToFile.SaveExcel2007(adList, Environment.CurrentDirectory + @"\avito1.xlsx", "Avito");
						//var temps1 = dr.PageSource; //_driver.getPageSource();
                    }
                    try
                    {
                        var aNext =
                            dr.FindElement(
                                OpenQA.Selenium.By.XPath("//li[contains(concat(' ', @class, ' '), 'page-next')]/a"));//_driver.findElement(By.xpath("//li[contains(concat(' ', @class, ' '), 'page-next')]/a"));
                        aNext.Click();
                        var temps2 = dr.PageSource; //_driver.getPageSource();
                    }
                    catch (Exception ex)
                    {
                        error += "Эта страница последняя: " + url + "?p=" + i + "\r\nlog:" + ex + "\r\n";
                        break;
                    }
                    SaveToFile.SaveExcel2007(adList, Environment.CurrentDirectory + @"\avito1.xlsx", "Avito");
                    //SaveToFile.SaveCSV(adList, Environment.CurrentDirectory + @"\avito.csv");
                    adList=new List<Av>();

                }
                catch (Exception ex)
                {
                    countError++;
                    error += "Не удалось загрузить страницу: " + url + "?p=" + i + "\r\nlog:" + ex + "\r\n";
                    if (countError == 7)
                        break;
                    else if (countError > 2)
                        dr.Navigate().GoToUrl(url + "?p=" + (i + 1));

                }
			}

			_driver.quit();
            _driver.close();
            
			return adList;
		}
        public override bool PlaceAd(List<Av> adList)
		{
			//var placeAdButt = _driver.FindElement(By.Name("Подать объявление"));
			//http://www.avito.ru/additem
		    return false;
		}
    }

}
