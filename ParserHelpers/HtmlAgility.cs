using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using HtmlAgilityPack;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;

namespace ParserHelpers
{
    public static class HtmlAgility
    {
        //"$(SolutionDir)\ILMerge\merge_all.bat" "$(SolutionDir)" "$(TargetPath)" $(ConfigurationName)
        //public static List<Category> GetListCategory(HtmlDocument doc, string xPath, string shopUrl)
        //{
        //    var cats = doc.DocumentNode.SelectNodes(xPath);
        //    var catList = new List<Category>();
        //    var arr = new string[]
        //            {
        //                "categ", "catal", "woman", "man", "katalog", "kategorii", "platja", "aksessuary", "roomdecor",
        //                "folder", "collect","kategoriya","cat="
        //            };
        //    foreach (var cat in cats)
        //    {
        //        if (cat.Attributes.Count > 0)
        //        {
        //            var link = cat.Attributes["href"].Value;
        //            bool good = arr.Any(ar => link.Contains(ar));
        //            if (link.Contains("s-trikbel") || shopUrl.Contains("artvision-opt") ||
        //                shopUrl.Contains("opt-ekonom") || shopUrl.Contains("witerra") ||
        //                shopUrl.Contains("ru.gipnozstyle") || shopUrl.Contains("trikotage") ||
        //                shopUrl.Contains("npopt") || shopUrl.Contains("japan-cosmetic") ||
        //                shopUrl.Contains("liora-shop") || shopUrl.Contains("opttextil") || shopUrl.Contains("donnasara") || shopUrl.Contains("besthat"))
        //                good = true;
        //            if (link.Contains("roomdecor") && (link.Contains("6195") || link.Contains("6159")))
        //                good = false;

        //            if (shopUrl.Contains("xn----0tbbbddeld.xn--p1ai") || shopUrl.Contains("td-adel"))
        //            {
        //                var t1 = cat.ParentNode.InnerHtml;
        //                if (t1.Contains("<ul") || link.Contains("new"))
        //                    good = false;
        //            }

        //            if (good)
        //            {
        //                if (shopUrl.Contains("www.trimedwedya.ru"))
        //                    link = "http://www.trimedwedya.ru" + link;
        //                else if (shopUrl.Contains("td-adel"))
        //                    link = "http://td-adel.ru" + link;
        //                else if (shopUrl.Contains("xn----0tbbbddeld.xn--p1ai"))
        //                    link = "http://xn----0tbbbddeld.xn--p1ai/" + link;
        //                else if (shopUrl.Contains("lemming.su"))
        //                    link = "http://lemming.su" + link;
        //                else if (shopUrl.Contains("vsspb"))
        //                    link = "http://vsspb.com" + link;
        //                else if (!link.Contains(shopUrl))
        //                    link = shopUrl + link;
        //                catList.Add(new Category() { Name = cat.InnerText, Url = WebUtility.HtmlDecode(link) });
        //            }
        //        }
        //    }
        //    return catList;
        //}
        public static string GetShopCatLink(string shopUrl)
        {
            var query = "//ul/li/a";
            if (shopUrl.Contains("trimedwedya") || shopUrl.Contains("artvision-opt"))
                query = "//ul/li/ul/li/a";
            else if (shopUrl.Contains("butterfly-dress"))
                query = "//ul/li/ul/li/div/a";
            else if (shopUrl.Contains("s-trikbel"))
                query = "//li[contains(concat(' ', @class, ' '), ' name ')]/a";
            else if (shopUrl.Contains("roomdecor"))
                query = "//li/ul/li/a";
            else if (shopUrl.Contains("nashipupsi"))
                query = "//a[contains(concat(' ', @href, ' '), 'folder')]";
            else if (shopUrl.Contains("opt-ekonom"))
                query = "//span[contains(concat(' ', @class, ' '), ' inner ')]/a";
            else if (shopUrl.Contains("lemming"))
                query = "//span/a";
            else if (shopUrl.Contains("piniolo"))
                query = "//li[contains(concat(' ', @class, ' '), ' item ')]/a";
            else if (shopUrl.Contains("witerra"))
                query = "//td[contains(concat(' ', @class, ' '), ' boxText ')]/a";
            else if (shopUrl.Contains("ru.gipnozstyle"))
                query = "//div[contains(concat(' ', @class, ' '), ' twocol ')]/a";
            else if (shopUrl.Contains("shop-nogti"))
                query = "//div/div/div/a";
            else if (shopUrl.Contains("iv-trikotage"))
                query = "//div[contains(concat(' ', @class, ' '), ' menu_spec ')]/ul/li/a";
            else if (shopUrl.Contains("optovik-centr"))
                query = "//a[contains(concat(' ', @class, ' '), ' mainlevel_frontpage_categories ')]";
            else if (shopUrl.Contains("japan-cosmetic"))
                query = "//div[contains(concat(' ', @class, ' '), 'moduletableproizv')]/div/a";
            else if (shopUrl.Contains("ekb-opt"))
                query = "//p[contains(concat(' ', @class, ' '), ' catalog level1')]/a";
            else if (shopUrl.Contains("aimico-kids"))
                query = "//ul[contains(concat(' ', @class, ' '), ' aim-vmenu')]/li/a";
            else if (shopUrl.Contains("texxit"))
                query = "//ul[contains(concat(' ', @class, ' '), ' menu')]/li/a";
            else if (shopUrl.Contains("liora-shop"))
                query = "//ul[contains(concat(' ', @class, ' '), 'level1')]/li/a";
            else if (shopUrl.Contains("vsspb"))
                query = "//ul[contains(concat(' ', @class, ' '), 'menu_ver')]/li/a | //ul[contains(concat(' ', @class, ' '), 'menu_ver')]/li/ul/li/a";
            else if (shopUrl.Contains("stilgi"))
                query = "//td/script[2]";
            else if (shopUrl.Contains("stefanika"))
                query = "//ul/li/div/a";
            else if (shopUrl.Contains("opttextil"))
                query = "//div[contains(concat(' ', @class, ' '), ' elem ')]/a | //div[contains(concat(' ', @class, ' '), ' popsmenu ')]/div/span/a";
            else if (shopUrl.Contains("bus-i-nka"))
                query = "//ul[contains(concat(' ', @class, ' '), ' categories ')]/li/a";
            else if (shopUrl.Contains("donnasara"))
                query = "//ul[contains(concat(' ', @class, ' '), ' b-sidebar-menu__submenu ')]/li/a";
            else if (shopUrl.Contains("lefik"))
                query = "//ul[contains(concat(' ', @class, ' '), ' collections-list ')]/li/a | //ul[contains(concat(' ', @class, ' '), ' subcol ')]/li/a";
            else if (shopUrl.Contains("topopt"))
                query = "//div[contains(concat(' ', @class, ' '), 'newMenu')]/div/a";
            else if (shopUrl.Contains("besthat"))
                query = "//ul[contains(concat(' ', @class, ' '), 'menu-subcategories')]/li/a";
            else if (shopUrl.Contains("colgotki"))
                query = "//div[contains(concat(' ', @class, ' '), 'art-BlockContent-body')]/a";
            else if (shopUrl.Contains("voolya"))
                query = "//div[contains(concat(' ', @class, ' '), 'menu')]/a";
            return query;
        }
        //public static HashSet<string> GetManyLevelCatalog(string catalogUrl,string xPath1, string xPath2,string cook,string site,)
        //{
        //    var res = new HashSet<string>();

        //    var prod = Helpers.GetProductLinks(catalog.Url + "&limit=400", cook, "http://www.colgotki.com",
        //            "//div[contains(concat(' ', @class, ' '), ' browseProductContainer ')]/a", null);
        //    if (prod.Count == 0)
        //    {
        //        var cat = Helpers.GetProductLinks(catalog.Url, cook, "http://optovik-centr.ru", "//td[contains(concat(' ', @align, ' '), ' center ')]/a", null);
        //        if (cat.Any())
        //        {
        //            var temp = new List<string>();
        //            foreach (var c in cat)
        //            {
        //                var tr = Helpers.GetProductLinks(catalog.Url + "&limit=400", cook, "http://www.colgotki.com",
        //                            "//div[contains(concat(' ', @class, ' '), ' browseProductContainer ')]/a", null);
        //                temp.AddRange(tr.ToList());
        //            }
        //            prod = new HashSet<string>(temp);
        //        }
        //    }

        //    var tr = Helpers.GetProductLinks(c1, cook, "http://shop-nogti.ru", "//div[contains(concat(' ', @class, ' '), ' browseProductContainer ')]/h2/a", null);
        //    if (tr.Any())
        //        temp.AddRange(tr.ToList());
        //    else
        //    {
        //        var doc2 = Helpers.GetHtmlDocument(c1, catalog.Url, null, cook);
        //        var cat2 = Helpers.GetPhoto(doc2, "//tr/td/a", "", "http://shop-nogti.ru", "", "categ");
        //        if (cat2.Any())
        //        {
        //            foreach (var c2 in cat2)
        //            {
        //                var tr2 = Helpers.GetProductLinks(c2, cook, "http://shop-nogti.ru", "//div[contains(concat(' ', @class, ' '), ' browseProductContainer ')]/h2/a", null);
        //                if (tr2.Any())
        //                    temp.AddRange(tr2.ToList());
        //                else
        //                {
        //                    var doc3 = Helpers.GetHtmlDocument(c2, c1, null, cook);
        //                    var cat3 = Helpers.GetPhoto(doc3, "//tr/td/a", "", "http://shop-nogti.ru", "", "categ");
        //                    if (cat3.Any())
        //                    {
        //                        foreach (var c3 in cat3)
        //                        {
        //                            var tr3 = Helpers.GetProductLinks(c3, cook, "http://shop-nogti.ru", "//div[contains(concat(' ', @class, ' '), ' browseProductContainer ')]/h2/a", null);
        //                            if (tr3.Any())
        //                                temp.AddRange(tr3.ToList());
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return res;
        //} 

        /// <summary>
        /// Вытаскивает текст без тегов, теги заменяются \r\n
        /// </summary>
        /// <param name="html">html код страницы</param>
        /// <param name="notWord">Список слов которые не должны содержать в тексте</param>
        /// <param name="word">Слово, которое должно содержаться в строке</param>
        /// <param name="split">Разделитель строк</param>
        /// <returns></returns>
        public static string GetTextReplaceTags(HtmlDocument doc, string xPath, List<string> notWord, string word = "",
            string split = "\r\n")
        {
            var node = doc.DocumentNode.SelectNodes(xPath);
            if (node != null)
            {
                var html = "";
                foreach (var n in node)
                {
                    if (!string.IsNullOrEmpty(n.InnerHtml))
                        html += n.InnerHtml + " ";
                }
                var arrList = new List<string>();
                if (!string.IsNullOrEmpty(html))
                {
                    var ar = Regex.Split(html, "<");
                    var res = "";
                    foreach (var s in ar)
                    {
                        var str = s.Substring(s.IndexOf(">") + 1).Trim();
                        if (!string.IsNullOrEmpty(str) && !arrList.Any(x => x.Equals(str)))
                        {
                            if (notWord != null && notWord.Any())
                            {
                                var i = notWord.Count(nw => str.Contains(nw));
                                if (i == 0)
                                {
                                    if (string.IsNullOrEmpty(word))
                                    {
                                        res += str + split;
                                        arrList.Add(str);
                                    }
                                    else
                                    {
                                        if (str.Contains(word))
                                        {
                                            res += str + split;
                                            arrList.Add(str);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(word))
                                {
                                    res += str + split;
                                    arrList.Add(str);
                                }
                                else
                                {
                                    if (str.Contains(word))
                                    {
                                        res += str + split;
                                        arrList.Add(str);
                                    }
                                }
                            }
                        }

                    }
                    if (res.Length > 0)
                        res = res.Remove(res.Length - split.Count());
                    return res;
                }
            }

            return string.Empty;
        }

        public static string ReplaceWhiteSpace(string text)
        {
            var res = string.Join("_", text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)).Replace("_", " ");
            return res;
        }
        //public static void GetCatalog(ref List<ShopBig> shopBigs)
        //{
        //    foreach (var shopBig in shopBigs)
        //    {
        //        var cook = GetCookiePost(shopBig.Url, new NameValueCollection());
        //        var html = GetHtmlDocument(shopBig.Url, "https://google.com", null, cook);
        //        if (html == null)
        //            continue;
        //        var cats = html.DocumentNode.SelectNodes(shopBig.XPath);
        //        if (cats != null)
        //        {
        //            var temp = new List<Category>();
        //            foreach (var cat in cats)
        //            {
        //                if (!string.IsNullOrEmpty(cat.InnerText))
        //                {
        //                    var url = cat.Attributes["href"].Value;
        //                    if (shopBig.Host == null || string.IsNullOrEmpty(shopBig.Host))
        //                        shopBig.Host = shopBig.Url;
        //                    if (url.Contains(shopBig.Host))
        //                        temp.Add(new Category() { Name = cat.InnerText, Url = url });
        //                    else
        //                        temp.Add(new Category() { Name = cat.InnerText, Url = shopBig.Host + url });
        //                }
        //            }
        //            shopBig.CatalogList = temp;
        //        }
        //    }

        //}
        public static HtmlAgilityPack.HtmlDocument GetHtmlDocument(string url, string refererLink, Encoding encode, string cook = "")
        {
            var s = "";
            try
            {

                var client = new System.Net.WebClient();
                client.Headers.Add(HttpRequestHeader.Cookie, cook);
                client.Headers.Add(HttpRequestHeader.Accept, "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                client.Headers.Add(HttpRequestHeader.Referer, HttpUtility.UrlEncode(refererLink));
                client.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.63 Safari/537.36");
                client.Headers.Add(HttpRequestHeader.AcceptLanguage, "ru-RU,ru;q=0.8,en-US;q=0.6,en;q=0.4");
                var data = client.OpenRead(url);
                if (encode == null)
                    encode = Encoding.UTF8;
                var reader = new StreamReader(data, encode);
                s = reader.ReadToEnd();
                data.Close();
                reader.Close();
                var doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(HttpUtility.HtmlDecode(s));
                return doc;
            }
            catch (Exception ex) { }
            return null;
        }
        public static List<string> GetPhoto(HtmlAgilityPack.HtmlDocument doc, string xPath1, string xPath2 = "", string host1 = "", string host2 = "", string word = "", string att1 = "href", string att2 = "src")
        {
            var phs = new List<string>();
            if (!string.IsNullOrEmpty(xPath1))
            {
                var photos = doc.DocumentNode.SelectNodes(xPath1);
                if (photos != null)
                {
                    if (!string.IsNullOrEmpty(word))
                    {
                        foreach (var p in photos)
                        {
                            if (p.Attributes[att1].Value.Contains(word))
                                phs.Add(host1 + p.Attributes[att1].Value);
                        }
                    }
                    else
                        phs.AddRange(photos.Select(p => host1 + p.Attributes[att1].Value));
                }
            }
            if (!string.IsNullOrEmpty(xPath2))
            {
                var photos = doc.DocumentNode.SelectNodes(xPath2);
                if (photos != null)
                {
                    if (!string.IsNullOrEmpty(word))
                    {
                        foreach (var p in photos)
                        {
                            if (p.Attributes[att2].Value.Contains(word))
                                phs.Add(host2 + p.Attributes[att2].Value);
                        }
                    }
                    else
                        phs.AddRange(photos.Select(p => host2 + p.Attributes[att2].Value));
                }
            }
            return new HashSet<string>(phs).ToList();
        }
        public static string GetItemsAttributt(HtmlAgilityPack.HtmlDocument doc, string xPath, string word, string attribut, List<string> notWord, string split = "\r\n")
        {
            var temp = "";
            var obj = doc.DocumentNode.SelectNodes(xPath);
            if (obj != null)
            {
                foreach (var fd in obj)
                {
                    if (notWord != null && notWord.Count > 0)
                    {
                        var bo = notWord.Where(x => fd.Attributes[attribut].Value.ToLower().Contains(x.ToLower())).ToList();
                        if (!bo.Any() && !string.IsNullOrEmpty(fd.Attributes[attribut].Value.Trim()))
                        {
                            if (string.IsNullOrEmpty(word))
                                temp += fd.Attributes[attribut].Value.Trim() + split;
                            else
                            {
                                if (fd.Attributes[attribut].Value.Contains(word))
                                    temp += fd.Attributes[attribut].Value.Trim().Replace(word, "") + split;
                            }
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(fd.Attributes[attribut].Value.Trim()))
                        {
                            if (string.IsNullOrEmpty(word))
                                temp += fd.Attributes[attribut].Value.Trim() + split;
                            else
                            {
                                if (fd.Attributes[attribut].Value.Contains(word))
                                    temp += fd.Attributes[attribut].Value.Trim().Replace(word, "") + split;
                            }
                        }
                    }
                }
                if (!string.IsNullOrEmpty(temp))
                    temp = temp.Substring(0, temp.Length - split.Length);
            }
            return temp;
        }
        public static List<string> GetItemsAttributtList(HtmlAgilityPack.HtmlDocument doc, string xPath, string word, string attribut, List<string> notWord, List<string> replace)
        {
            var temp = new List<string>();
            var obj = doc.DocumentNode.SelectNodes(xPath);
            if (obj != null)
            {
                foreach (var fd in obj)
                {
                    if (notWord != null && notWord.Count > 0)
                    {
                        var bo = notWord.Where(x => fd.Attributes[attribut].Value.ToLower().Contains(x.ToLower())).ToList();
                        if (!bo.Any() && !string.IsNullOrEmpty(fd.Attributes[attribut].Value.Trim()))
                        {
                            if (string.IsNullOrEmpty(word))
                            {
                                if (replace == null || !replace.Any())
                                    temp.Add(fd.Attributes[attribut].Value.Trim());
                                else
                                {
                                    var t = fd.Attributes[attribut].Value.Trim();
                                    foreach (var rep in replace)
                                    {
                                        t.Replace(rep, "");
                                    }
                                    if (!string.IsNullOrEmpty(t.Trim()))
                                        temp.Add(t.Trim());
                                }
                            }
                            else
                            {
                                if (fd.Attributes[attribut].Value.Contains(word))
                                {
                                    if (replace == null || !replace.Any())
                                        temp.Add(fd.Attributes[attribut].Value.Trim());
                                    else
                                    {
                                        var t = fd.Attributes[attribut].Value.Trim();
                                        foreach (var rep in replace)
                                        {
                                            t.Replace(rep, "");
                                        }
                                        if (!string.IsNullOrEmpty(t.Trim()))
                                            temp.Add(t.Trim());
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(fd.Attributes[attribut].Value.Trim()))
                        {
                            if (string.IsNullOrEmpty(word))
                            {
                                if (replace == null || !replace.Any())
                                    temp.Add(fd.Attributes[attribut].Value.Trim());
                                else
                                {
                                    var t = fd.Attributes[attribut].Value.Trim();
                                    foreach (var rep in replace)
                                    {
                                        t.Replace(rep, "");
                                    }
                                    if (!string.IsNullOrEmpty(t.Trim()))
                                        temp.Add(t.Trim());
                                }
                            }
                            else
                            {
                                if (fd.Attributes[attribut].Value.Contains(word))
                                {
                                    if (replace == null || !replace.Any())
                                        temp.Add(fd.Attributes[attribut].Value.Trim());
                                    else
                                    {
                                        var t = fd.Attributes[attribut].Value.Trim();
                                        foreach (var rep in replace)
                                        {
                                            t.Replace(rep, "");
                                        }
                                        if (!string.IsNullOrEmpty(t.Trim()))
                                            temp.Add(t.Trim());
                                    }
                                }
                            }
                        }
                    }

                }
            }
            var res = new HashSet<string>(temp);
            return res.ToList();
        }
        /// <summary>
        /// Получает строку с данными из массива inner html element
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="xPath"></param>
        /// <param name="word">Словов которое должно обязательно присутсвовать в строке, при сохранение будет удалено из самой строки</param>
        /// <param name="notWord">С указанными словами строки сохраняться не будут</param>
        /// <param name="split">Разделитель в строке между элементами</param>
        /// <returns></returns>
        public static string GetItemsInnerText(HtmlAgilityPack.HtmlDocument doc, string xPath, string word, List<string> notWord, string split = "\r\n")
        {
            var temp = "";
            var obj = doc.DocumentNode.SelectNodes(xPath);
            if (obj != null)
            {
                foreach (var fd in obj)
                {
                    if (notWord != null && notWord.Count > 0)
                    {
                        var bo = notWord.Where(x => fd.InnerText.ToLower().Contains(x.ToLower())).ToList();
                        if (!bo.Any() && !string.IsNullOrEmpty(fd.InnerText.Trim()))
                        {
                            if (string.IsNullOrEmpty(word))
                                temp += fd.InnerText.Trim() + split;
                            else
                            {
                                if (fd.InnerText.Contains(word))
                                    temp += fd.InnerText.Trim().Replace(word, "") + split;
                            }
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(fd.InnerText.Trim()))
                        {
                            if (string.IsNullOrEmpty(word))
                                temp += fd.InnerText.Trim() + split;
                            else
                            {
                                if (fd.InnerText.Contains(word))
                                    temp += fd.InnerText.Trim().Replace(word, "") + split;
                            }
                        }
                    }
                }
                if (!string.IsNullOrEmpty(temp))
                    temp = temp.Substring(0, temp.Length - split.Length);
            }
            return temp;
        }
        public static List<string> GetItemsInnerTextList(HtmlAgilityPack.HtmlDocument doc, string xPath, string word, List<string> notWord, List<string> replace)
        {
            var temp = new List<string>();
            var obj = doc.DocumentNode.SelectNodes(xPath);
            if (obj != null)
            {
                foreach (var fd in obj)
                {
                    if (notWord != null && notWord.Count > 0)
                    {
                        var bo = notWord.Where(x => fd.InnerText.ToLower().Contains(x.ToLower())).ToList();
                        if (!bo.Any() && !string.IsNullOrEmpty(fd.InnerText.Trim()))
                        {
                            if (string.IsNullOrEmpty(word))
                            {
                                if (replace == null || !replace.Any())
                                    temp.Add(fd.InnerText.Trim());
                                else
                                {
                                    var t = fd.InnerText.Trim();
                                    foreach (var rep in replace)
                                    {
                                        t.Replace(rep, "");
                                    }
                                    if (!string.IsNullOrEmpty(t.Trim()))
                                        temp.Add(t.Trim());
                                }
                            }
                            else
                            {
                                if (fd.InnerText.Contains(word))
                                {
                                    if (replace == null || !replace.Any())
                                        temp.Add(fd.InnerText.Replace(word, "").Trim());
                                    else
                                    {
                                        var t = fd.InnerText.Replace(word, "").Trim();
                                        foreach (var rep in replace)
                                        {
                                            t.Replace(rep, "");
                                        }
                                        if (!string.IsNullOrEmpty(t.Trim()))
                                            temp.Add(t.Trim());
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(fd.InnerText.Trim()))
                        {
                            if (string.IsNullOrEmpty(word))
                            {
                                if (replace == null || !replace.Any())
                                    temp.Add(fd.InnerText.Trim());
                                else
                                {
                                    var t = fd.InnerText.Trim();
                                    foreach (var rep in replace)
                                    {
                                        t.Replace(rep, "");
                                    }
                                    if (!string.IsNullOrEmpty(t.Trim()))
                                        temp.Add(t.Trim());
                                }
                            }
                            else
                            {
                                if (fd.InnerText.Contains(word))
                                {
                                    if (replace == null || !replace.Any())
                                        temp.Add(fd.InnerText.Replace(word, "").Trim());
                                    else
                                    {
                                        var t = fd.InnerText.Replace(word, "").Trim();
                                        foreach (var rep in replace)
                                        {
                                            t.Replace(rep, "");
                                        }
                                        if (!string.IsNullOrEmpty(t.Trim()))
                                            temp.Add(t.Trim());
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return temp;
        }
        /// <summary>
        /// Получить содержимое тега data[numberObject].InnerText.Trim()
        /// </summary>
        /// <param name="doc">html документ</param>
        /// <param name="xPath">Путь к нужному тегу</param>
        /// <param name="numberObject"></param>
        /// <returns>Внутренний текст тега</returns>
        public static string GetItemInnerText(HtmlAgilityPack.HtmlDocument doc, string xPath, int numberObject = 0)
        {
            var data = doc.DocumentNode.SelectNodes(xPath);
            var d = "";
            if (data != null)
            {
                if (data.Count > numberObject)
                    d = data[numberObject].InnerText.Trim();
            }
            return d;
        }
        public static string GetItemsInnerHtml(HtmlAgilityPack.HtmlDocument doc, string xPath, string word, List<string> notWord, string split = "\r\n")
        {
            var temp = "";
            var obj = doc.DocumentNode.SelectNodes(xPath);
            if (obj != null)
            {
                foreach (var fd in obj)
                {
                    if (notWord != null && notWord.Count > 0)
                    {
                        var bo = notWord.Where(x => fd.InnerHtml.ToLower().Contains(x.ToLower())).ToList();
                        if (!bo.Any() && !string.IsNullOrEmpty(fd.InnerHtml.Trim()))
                        {
                            if (string.IsNullOrEmpty(word))
                                temp += fd.InnerHtml.Trim() + split;
                            else
                            {
                                if (fd.InnerText.Contains(word))
                                    temp += fd.InnerHtml.Trim().Replace(word, "") + split;
                            }
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(fd.InnerHtml.Trim()))
                        {
                            if (string.IsNullOrEmpty(word))
                                temp += fd.InnerHtml.Trim() + split;
                            else
                            {
                                if (fd.InnerText.Contains(word))
                                    temp += fd.InnerHtml.Trim().Replace(word, "") + split;
                            }
                        }
                    }
                }
                if (!string.IsNullOrEmpty(temp))
                    temp = temp.Substring(0, temp.Length - split.Length);
            }
            return temp;
        }
        public static string GetItemInnerHtml(HtmlAgilityPack.HtmlDocument doc, string xPath, int numberObject = 0)
        {
            var data = doc.DocumentNode.SelectNodes(xPath);
            var d = "";
            if (data != null)
            {
                if (data.Count > numberObject)
                    d = data[numberObject].InnerHtml.Trim();
            }
            return d;
        }
        public static string GetEncodingCategory(string str)
        {
            var win = Encoding.GetEncoding("windows-1251");
            byte[] winBytes = win.GetBytes(str);
            var cat = Encoding.UTF8.GetString(winBytes, 0, winBytes.Length);
            return HttpUtility.HtmlDecode(cat);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="catalogLink"></param>
        /// <param name="cook"></param>
        /// <param name="site">host</param>
        /// <param name="xA">Путь к ссылкам на товар</param>
        /// <param name="xApage">Путь к ссылкам на страницы</param>
        /// <param name="type">По умолчанию Encoding.UTF8</param>
        /// <returns></returns>
        public static HashSet<string> GetProductLinks(string catalogLink, string cook, string site, string xA, string xApage, Encoding type, string linkPage = "", string host = "")
        {
            var prLink = new List<string>();
            try
            {
                var client = new System.Net.WebClient();
                if (!string.IsNullOrEmpty(cook))
                    client.Headers.Add(HttpRequestHeader.Cookie, cook);
                client.Headers.Add(HttpRequestHeader.Accept,
                    "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                client.Headers.Add(HttpRequestHeader.Referer, site + "/");
                client.Headers.Add(HttpRequestHeader.UserAgent,
                    "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.63 Safari/537.36");
                client.Headers.Add(HttpRequestHeader.AcceptLanguage, "ru-RU,ru;q=0.8,en-US;q=0.6,en;q=0.4");
                var data = client.OpenRead(catalogLink);
                if (type == null)
                    type = Encoding.UTF8;
                var reader = new StreamReader(data, type);
                string s = reader.ReadToEnd();
                data.Close();
                reader.Close();
                if (s.Contains("\"refresh\""))
                {

                    var newLink = GetLinkOfString(s);
                    client = new System.Net.WebClient();
                    client.Headers.Add(HttpRequestHeader.Cookie, cook);
                    client.Headers.Add(HttpRequestHeader.Accept, "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                    client.Headers.Add(HttpRequestHeader.Referer, catalogLink);
                    client.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.63 Safari/537.36");
                    client.Headers.Add(HttpRequestHeader.AcceptLanguage, "ru-RU,ru;q=0.8,en-US;q=0.6,en;q=0.4");
                    data = client.OpenRead(newLink);
                    reader = new StreamReader(data, type);
                    s = reader.ReadToEnd();

                }
                data.Close();
                reader.Close();
                var doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(s);
                if (string.IsNullOrEmpty(host))
                    host = site;
                var a = doc.DocumentNode.SelectNodes(xA);
                if (a != null)
                {
                    foreach (var p in a)
                    {
                        prLink.Add(host + WebUtility.HtmlDecode(p.Attributes["href"].Value));
                    }
                    var pages = doc.DocumentNode.SelectNodes(xApage);
                    if (pages != null && pages.Count > 0)
                    {
                        var preLink = new List<string>();
                        foreach (var pag in pages)
                        {
                            var link2 = WebUtility.HtmlDecode(pag.Attributes["href"].Value);
                            if (!preLink.Contains(link2))
                            {
                                var web2 = new HtmlWeb();
                                if (linkPage.Length == 0)
                                    linkPage = site;
                                HtmlAgilityPack.HtmlDocument doc2 = web2.Load(linkPage + link2);
                                var a2 = doc2.DocumentNode.SelectNodes(xA);
                                if (a2 != null)
                                {
                                    foreach (var p in a2)
                                    {
                                        prLink.Add(host + WebUtility.HtmlDecode(p.Attributes["href"].Value));
                                    }
                                }
                                preLink.Add(link2);
                            }
                        }
                    }

                }
            }
            catch (Exception ex) { }
            return new HashSet<string>(prLink);
        }
        public static HashSet<string> GetProductLinks(string catalogLink, string cook, string site, string xA, string xApage, string strPage, Encoding type, int numberMaxLink = 2, string host = "")
        {
            var prLink = new List<string>();
            try
            {
                var client = new System.Net.WebClient();
                client.Headers.Add(HttpRequestHeader.Cookie, cook);
                client.Headers.Add(HttpRequestHeader.Accept, "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                client.Headers.Add(HttpRequestHeader.Referer, site + "/");
                client.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.63 Safari/537.36");
                client.Headers.Add(HttpRequestHeader.AcceptLanguage, "ru-RU,ru;q=0.8,en-US;q=0.6,en;q=0.4");
                var data = client.OpenRead(catalogLink);
                if (type == null)
                    type = Encoding.UTF8;
                var reader = new StreamReader(data, type);
                string s = reader.ReadToEnd();
                data.Close();
                reader.Close();
                var doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(s);
                if (string.IsNullOrEmpty(host))
                    host = site;
                var a = doc.DocumentNode.SelectNodes(xA);
                if (a != null)
                {
                    foreach (var p in a)
                    {
                        prLink.Add(host + WebUtility.HtmlDecode(p.Attributes["href"].Value));
                    }
                    var pages = doc.DocumentNode.SelectNodes(xApage);
                    if (pages != null && pages.Count > 1)
                    {
                        var max = 0;
                        if (pages.Count > 2)
                        {
                            var tr = Int32.TryParse(pages[numberMaxLink].InnerText.Trim(), out max);
                            if (!tr)
                            {
                                var link = pages[numberMaxLink].Attributes["href"].Value;
                                var num = link.Substring(link.IndexOf(strPage) + strPage.Length, link.Length - (link.IndexOf(strPage) + strPage.Length));
                                tr = Int32.TryParse(num, out max);
                            }
                        }
                        else
                            max = Convert.ToInt32(pages[1].InnerText.Trim());
                        for (var i = 2; i <= max; i++)
                        {
                            var web2 = new HtmlWeb();
                            HtmlAgilityPack.HtmlDocument doc2 = web2.Load(catalogLink + strPage + i);
                            var a2 = doc2.DocumentNode.SelectNodes(xA);
                            if (a2 != null)
                            {
                                foreach (var p in a2)
                                {
                                    prLink.Add(host + WebUtility.HtmlDecode(p.Attributes["href"].Value));
                                }
                            }
                            Thread.Sleep(7000);
                        }
                    }

                }
            }
            catch (Exception ex) { }
            return new HashSet<string>(prLink);
        }
        public static HashSet<string> GetProductLinks2(string catalogLink, string cook, string site, string xA, string xApage, string strPage, Encoding type, string host = "")
        {
            var prLink = new List<string>();
            try
            {
                var client = new System.Net.WebClient();
                client.Headers.Add(HttpRequestHeader.Cookie, cook);
                client.Headers.Add(HttpRequestHeader.Accept, "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                client.Headers.Add(HttpRequestHeader.Referer, site + "/");
                client.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.63 Safari/537.36");
                client.Headers.Add(HttpRequestHeader.AcceptLanguage, "ru-RU,ru;q=0.8,en-US;q=0.6,en;q=0.4");
                var data = client.OpenRead(catalogLink);
                if (type == null)
                    type = Encoding.UTF8;
                var reader = new StreamReader(data, type);
                string s = reader.ReadToEnd();
                data.Close();
                reader.Close();
                var doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(s);
                if (string.IsNullOrEmpty(host))
                    host = site;
                var a = doc.DocumentNode.SelectNodes(xA);
                if (a != null)
                {
                    foreach (var p in a)
                    {
                        var res = p.Attributes["href"].Value;
                        if (!res.Contains(host))
                            prLink.Add(host + WebUtility.HtmlDecode(res));
                        else
                            prLink.Add(WebUtility.HtmlDecode(res));
                    }
                    var pages = doc.DocumentNode.SelectNodes(xApage);
                    if (pages != null)
                    {
                        var max = 0;
                        var l = HttpUtility.HtmlDecode(pages[0].Attributes["href"].Value);
                        var num = Regex.Replace(l.Substring(l.LastIndexOf(strPage), l.Length - l.LastIndexOf(strPage)), @"[^\d]", "");
                        var tr = Int32.TryParse(num, out max);

                        for (var i = 2; i <= max; i++)
                        {
                            var web2 = new HtmlWeb();
                            HtmlAgilityPack.HtmlDocument doc2 = web2.Load(catalogLink + strPage + i);
                            var a2 = doc2.DocumentNode.SelectNodes(xA);
                            if (a2 != null)
                            {
                                foreach (var p in a2)
                                {
                                    var res = p.Attributes["href"].Value;
                                    if (!res.Contains(host))
                                        prLink.Add(host + WebUtility.HtmlDecode(res));
                                    else
                                        prLink.Add(WebUtility.HtmlDecode(res));
                                }
                            }
                            if (i == max)
                            {
                                var pages2 = doc2.DocumentNode.SelectNodes(xApage);

                                var max2 = 0;
                                var l2 = HttpUtility.HtmlDecode(pages2[0].Attributes["href"].Value);
                                var num2 = Regex.Replace(l2.Substring(l2.LastIndexOf(strPage), l2.Length - l2.LastIndexOf(strPage)), @"[^\d]", "");
                                var tr2 = Int32.TryParse(num2, out max2);
                                if (max2 > max)
                                {
                                    for (var j = max + 1; j <= max2; j++)
                                    {
                                        var web3 = new HtmlWeb();
                                        HtmlAgilityPack.HtmlDocument doc3 = web3.Load(catalogLink + strPage + j);
                                        var a3 = doc3.DocumentNode.SelectNodes(xA);
                                        if (a3 != null)
                                        {
                                            foreach (var p in a3)
                                            {
                                                var res = p.Attributes["href"].Value;
                                                if (!res.Contains(host))
                                                    prLink.Add(host + WebUtility.HtmlDecode(res));
                                                else
                                                    prLink.Add(WebUtility.HtmlDecode(res));
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                }
            }
            catch (Exception ex) { }
            return new HashSet<string>(prLink);
        }
        public static string GetLinkOfString(string text)
        {
            var beg = text.IndexOf("http:");
            var link = text.Substring(beg, text.IndexOf("\"", beg + 3) - beg);
            return link;
        }
        public static HashSet<string> GetProductLinks(string catalogLink, string cook, string site, string xA, Encoding type, string host = "", string linkEnd = "")
        {
            var prLink = new List<string>();
            try
            {
                var client = new System.Net.WebClient();
                client.Headers.Add(HttpRequestHeader.Cookie, cook);
                client.Headers.Add(HttpRequestHeader.Accept, "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                client.Headers.Add(HttpRequestHeader.Referer, site + "/");
                client.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.63 Safari/537.36");
                client.Headers.Add(HttpRequestHeader.AcceptLanguage, "ru-RU,ru;q=0.8,en-US;q=0.6,en;q=0.4");
                var data = client.OpenRead(catalogLink + linkEnd);
                if (type == null)
                    type = Encoding.UTF8;
                var reader = new StreamReader(data, type);
                string s = reader.ReadToEnd();
                data.Close();
                reader.Close();
                if (s.Contains("\"refresh\""))
                {

                    var newLink = GetLinkOfString(s) + linkEnd;
                    client = new System.Net.WebClient();
                    client.Headers.Add(HttpRequestHeader.Cookie, cook);
                    client.Headers.Add(HttpRequestHeader.Accept, "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                    client.Headers.Add(HttpRequestHeader.Referer, catalogLink);
                    client.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.63 Safari/537.36");
                    client.Headers.Add(HttpRequestHeader.AcceptLanguage, "ru-RU,ru;q=0.8,en-US;q=0.6,en;q=0.4");
                    data = client.OpenRead(newLink);
                    reader = new StreamReader(data, type);
                    s = reader.ReadToEnd();

                }
                data.Close();
                reader.Close();
                var doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(s);
                if (string.IsNullOrEmpty(host))
                    host = site;
                var a = doc.DocumentNode.SelectNodes(xA);
                if (a != null)
                {
                    foreach (var p in a)
                    {
                        var temp = WebUtility.HtmlDecode(p.Attributes["href"].Value);
                        if (!temp.Contains(host))
                            temp = host + temp;
                        prLink.Add(temp);
                    }
                }
            }
            catch (Exception ex) { }
            return new HashSet<string>(prLink);
        }
        public static string GetCookiePost(string link, NameValueCollection col)
        {
            var request = (HttpWebRequest)HttpWebRequest.Create(link);
            if (link.Contains("wildberries"))
            {
                request.Method = "Post";
                request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                request.ContentType = "application/x-www-form-urlencoded";
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.63 Safari/537.36";
                WebHeaderCollection myWebHeaderCollection = request.Headers;
                myWebHeaderCollection.Add("Accept-Language:ru");
                request.ContentLength = 0;
            }
            var resp = (HttpWebResponse)request.GetResponse();
            var cooks = resp.Headers.GetValues("Set-Cookie");
            resp.Close();
            var cook = "";
            if (cooks != null)
            {
                if (cooks[0].ToLower().Contains("path"))
                {
                    foreach (var s in cooks)
                    {
                        if (s.IndexOf(";") > s.IndexOf("="))
                        {
                            var temp = s.Substring(0, s.IndexOf(";") + 1);
                            if (!cook.Contains(temp))
                                cook += temp + " ";
                        }
                        else if (!s.Contains("path"))
                        {
                            cook += s + " ";
                        }
                    }

                }
                else
                {
                    foreach (var c in cooks)
                    {
                        if (!c.Contains("path"))
                            cook += c.Substring(0, c.IndexOf(";") + 1) + " ";
                    }
                }
            }
            //if (col.Count > 0)
            //{
            var client2 = new System.Net.WebClient();
            client2.Headers.Add(HttpRequestHeader.Cookie, cook);
            client2.Headers.Add(HttpRequestHeader.Accept, "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
            client2.Headers.Add(HttpRequestHeader.Referer, "https://google.com");
            client2.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.63 Safari/537.36");
            client2.Headers.Add(HttpRequestHeader.AcceptLanguage, "ru-RU,ru;q=0.8,en-US;q=0.6,en;q=0.4");
            try
            {
                var byt = client2.UploadValues(link, "POST", col);

                var reader2 = new StreamReader(new MemoryStream(byt));
                string s21 = reader2.ReadToEnd();
            }
            catch (Exception ex) { }
            //}
            return cook;
        }
        //public static void SaveToFile<T>(List<T> pr, string path, bool photo = false, bool sort = true, bool checkAttr = true) where T : Product
        //{
        //    var hash = new HashSet<string>(pr.Select(x => x.Url));
        //    var cL = new List<T>();

        //    if (hash.Count != pr.Count && sort)
        //    {
        //        foreach (var t in hash)
        //        {
        //            foreach (var g in pr)
        //            {
        //                if (t.Contains(g.Url))
        //                {
        //                    cL.Add(g);
        //                    break;
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        cL = pr;
        //    }

        //    if (checkAttr)
        //    {
        //        var articl = cL.Select(x => x.Article).ToList();
        //        var articDesc = new HashSet<string>(articl);
        //        if (articl.Count() != articDesc.Count())
        //        {
        //            var group = articl.GroupBy(x => x).Select(x => new { key = x.Key, count = x.Count() });
        //            foreach (var gr in group)
        //            {
        //                if (gr.count > 1)
        //                {
        //                    var allGroupArtics = new List<T>();
        //                    allGroupArtics.AddRange(cL.Where(x => x.Article == gr.key));

        //                    var newArts = new List<T>();
        //                    int i = GetUniqArtic(allGroupArtics[0].Url);

        //                    foreach (var allGroupArtic in allGroupArtics)
        //                    {
        //                        var temp = allGroupArtic;
        //                        if (i != 0)
        //                            temp.Article = temp.Article + "_" + i;
        //                        newArts.Add(temp);
        //                        i++;
        //                    }
        //                    //Delete
        //                    foreach (var allGroupArtic in allGroupArtics)
        //                    {
        //                        cL.Remove(allGroupArtic);
        //                    }
        //                    //add
        //                    foreach (var newArt in newArts)
        //                    {
        //                        cL.Add(newArt);
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    SaveExcel2007<T>(cL, path, "Каталог", cL.Max(x => x.Photos.Count), photo);
        //}
        public static int GetUniqArtic(string link)
        {
            var num = Regex.Replace(link, @"[^\d]", "");
            if (num.Length == 0)
                num = link.Length.ToString();
            var res = "";
            res = num.Length > 2 ? num.Substring(num.Length - 2, 2) : num;
            return Int32.Parse(res);
        }
        public static void SaveExcel2007<T>(IEnumerable<T> list, string path, string nameBook, int countPhoto, bool photo = false)
        {
            if (list == null || !list.Any()) return;
            Type itemType = typeof(T);
            var props = itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance).OrderBy(p => p.Name);

            var dt = new DataTable(itemType.Name) { Locale = System.Threading.Thread.CurrentThread.CurrentCulture };
            if (dt.Rows.Count < 1 && dt.Columns.Count < 1)
            {
                //Создаём столбцы

                var temps = props.ToList();
                var ph = props.FirstOrDefault(x => x.Name == "Photo");
                if (ph != null && photo)
                {
                    dt.Columns.Add("Photo");
                    temps.Remove(ph);
                }
                dt.Columns.Add("main_categories");
                temps.Remove(props.FirstOrDefault(x => x.Name == "main_categories"));
                dt.Columns.Add("CategoryPath");
                temps.Remove(props.FirstOrDefault(x => x.Name == "CategoryPath"));
                dt.Columns.Add("Name");
                temps.Remove(props.FirstOrDefault(x => x.Name == "Name"));
                dt.Columns.Add("article");
                temps.Remove(props.FirstOrDefault(x => x.Name == "article"));
                dt.Columns.Add("Article");
                temps.Remove(props.FirstOrDefault(x => x.Name == "Article"));
                var prices = props.Where(x => x.Name.Contains("Prices"));
                temps.Remove(props.FirstOrDefault(x => x.Name == "Prices"));
                var price = props.Where(x => x.Name.Contains("Price"));
                foreach (var p in price)
                {
                    if (!p.Name.Contains("Prices"))
                    {
                        dt.Columns.Add(p.Name);
                        temps.Remove(p);
                    }
                }

                if (prices.Any())
                {
                    foreach (var p in prices)
                    {
                        for (int i = 0; i <= 3; i++)
                            dt.Columns.Add(p.Name + i);
                    }
                }
                dt.Columns.Add("client_price");
                temps.Remove(props.FirstOrDefault(x => x.Name == "client_price"));
                temps.Remove(props.FirstOrDefault(x => x.Name == "state"));
                temps.Remove(props.FirstOrDefault(x => x.Name == "Description"));
                temps.Remove(props.FirstOrDefault(x => x.Name == "Url"));
                var photos = props.Where(x => x.Name.Contains("Photos"));
                temps.Remove(props.FirstOrDefault(x => x.Name == "Photos"));
                foreach (var prop in temps)
                {
                    if (!prop.Name.Equals("Photo"))
                        dt.Columns.Add(prop.Name);
                }

                dt.Columns.Add("state");
                dt.Columns.Add("Description");
                dt.Columns.Add("Url");
                foreach (var p in photos)
                {
                    for (int i = 0; i <= countPhoto; i++)
                        dt.Columns.Add(p.Name + i);
                }
            }

            foreach (var x in list)
            {
                DataRow newRow = dt.NewRow();
                //newRow["CompanyID"] = "NewCompanyID";
                foreach (var prop in props)
                {
                    if (!photo)
                    {
                        if (prop.Name.Equals("Photo"))
                            continue;
                    }
                    var val = prop.GetValue(x);
                    if (val == null)
                        newRow[prop.Name] = "";
                    else if (val is IList)
                    {
                        //var temp = "";
                        //var type=prop.GetType().GetGenericTypeDefinition();
                        //var pr2=type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                        int i = 0;
                        foreach (var v in (List<string>)val)
                        {
                            var name = prop.Name + i;
                            newRow[name] = v;
                            i++;
                            //temp += (temp.Length > 0 ? " , " : "") + v;
                        }
                        //newRow[prop.Name] = temp;
                    }
                    else if (prop.Name.Contains("Price"))
                    {
                        double price = 0;
                        var converts = val.ToString().Replace(",", ".").Replace(" ", "");
                        var conv = Double.TryParse(val.ToString(), NumberStyles.AllowDecimalPoint, CultureInfo.CreateSpecificCulture("en-US"), out price);
                        if (!conv)
                            conv = Double.TryParse(val.ToString(), out price);
                        newRow[prop.Name] = price;
                    }
                    //else if (prop.Name.Equals("Photo"))
                    //{
                    //    if (photo)
                    //    {
                    //        newRow[prop.Name] = val.ToString();
                    //    }
                    //var img = val as System.Drawing.Image;
                    //MemoryStream ms = new MemoryStream();
                    //img.Save(ms, img.RawFormat);
                    //newRow[prop.Name] = ms.ToArray();
                    //}
                    else
                    {
                        newRow[prop.Name] = val.ToString();
                    }
                }
                dt.Rows.Add(newRow);
            }

            using (var p = new ExcelPackage(File.Exists(path) ? new FileInfo(path) : null))
            {
                //Here setting some document properties
                //p.Workbook.Properties.Author = "Zeeshan Umar";
                p.Workbook.Properties.Title = nameBook;

                //Create a sheet
                //p.Workbook.Worksheets.Add("Sample WorkSheet");
                ExcelWorksheet ws = null;
                int colIndex = 1;
                int rowIndex = 1;
                if (p.Workbook.Worksheets.Count == 0)
                {
                    ws = p.Workbook.Worksheets.Add("Sample WorkSheet");
                    ws.Name = itemType.Name; //Setting Sheet's name
                    ws.Cells.Style.Font.Size = 11; //Default font size for whole sheet
                    ws.Cells.Style.Font.Name = "Calibri"; //Default Font name for whole sheet

                    //Merging cells and create a center heading for out table
                    //ws.Cells[1, 1].Value = "Sample DataTable Export";
                    //ws.Cells[1, 1, 1, dt.Columns.Count].Merge = true;
                    //ws.Cells[1, 1, 1, dt.Columns.Count].Style.Font.Bold = true;
                    //ws.Cells[1, 1, 1, dt.Columns.Count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    colIndex = 1;

                    foreach (DataColumn dc in dt.Columns) //Creating Headings
                    {
                        var cell = ws.Cells[rowIndex, colIndex];
                        var cel2 = ws.Cells[rowIndex + 1, colIndex];
                        //Setting the background color of header cells to Gray
                        var fill = cel2.Style.Fill;
                        //fill.PatternType = ExcelFillStyle.Solid;
                        //fill.BackgroundColor.SetColor(Color.Gray);
                        cel2.Style.Font.Bold = true;

                        //Setting Top/left,right/bottom borders.
                        var border = cel2.Style.Border;
                        border.Bottom.Style =
                            border.Top.Style =
                            border.Left.Style =
                            border.Right.Style = ExcelBorderStyle.Thin;

                        //Setting Value in cell

                        cell.Value = dc.ColumnName;
                        if (dc.ColumnName == "Photo")
                            cel2.Value = "Фото";
                        else if (dc.ColumnName == "CategoryPath")
                            cel2.Value = "Рубрика в каталоге закупки";
                        else if (dc.ColumnName == "Name")
                            cel2.Value = "Название";
                        else if (dc.ColumnName == "Article")
                            cel2.Value = "Артикул поставщика (необязательно)";
                        else if (dc.ColumnName == "Size")
                            cel2.Value = "Размер";
                        else if (dc.ColumnName == "Color")
                            cel2.Value = "Цвет";
                        else if (dc.ColumnName.Contains("Prices"))
                        {
                            var reg = Regex.Replace(dc.ColumnName, @"[^\d]", "");
                            cel2.Value = "Оптовая цена " + reg;
                        }
                        else if (dc.ColumnName.Contains("Price"))
                        {
                            var reg = Regex.Replace(dc.ColumnName, @"[^\d]", "");
                            if (reg.Length == 0)
                                cel2.Value = "Оптовая цена";
                            else
                                cel2.Value = "Оптовая цена " + reg;
                        }
                        else if (dc.ColumnName == "Description")
                            cel2.Value = "Описание";
                        else if (dc.ColumnName == "Url")
                            cel2.Value = "Ссылка на сайте поставщика";
                        else if (dc.ColumnName.Contains("Photos"))
                        {
                            var reg = Regex.Replace(dc.ColumnName, @"[^\d]", "");
                            if (reg.Length == 0)
                                cel2.Value = "Фото 1";
                            else
                                cel2.Value = "Фото " + reg;
                        }
                        else if (dc.ColumnName == "state")
                            cel2.Value = "Статус";
                        else if (dc.ColumnName == "client_price")
                            cel2.Value = "Цена c орг. сбором";
                        else if (dc.ColumnName == "main_categories")
                            cel2.Value = "Рубрика в общем каталоге";
                        else if (dc.ColumnName == "article")
                            cel2.Value = "Артикул";
                        else
                            cel2.Value = dc.ColumnName;
                        colIndex++;
                    }
                }
                else
                    ws = p.Workbook.Worksheets.First();

                rowIndex = ws.Dimension.End.Row;
                foreach (DataRow dr in dt.Rows) // Adding Data into rows
                {
                    colIndex = 1;
                    rowIndex++;
                    foreach (DataColumn dc in dt.Columns)
                    {
                        var cell = ws.Cells[rowIndex, colIndex];

                        if (dc.ColumnName.Contains("Price"))
                        {
                            cell.Style.Numberformat.Format = "#,##0.00";
                            double res = 0;
                            var rrr = double.TryParse(dr[dc.ColumnName].ToString(), out res);
                            cell.Value = res;
                        }
                        else if (dc.ColumnName.Equals("Photo"))
                        {
                            ws.Row(rowIndex).Height = 71;
                            ws.Column(colIndex).Width = 18;
                            //add picture to cell
                            //BinaryFormatter bf = new BinaryFormatter();
                            //Stream ms = new MemoryStream();
                            //bf.Serialize(ms, dr[dc.ColumnName]);
                            var pa = dr[dc.ColumnName].ToString();
                            var name = Guid.NewGuid().ToString();
                            try
                            {
                                using (var image = Image.FromFile(pa))
                                {
                                    ExcelPicture pic = ws.Drawings.AddPicture(name, image);
                                    //position picture on desired column
                                    pic.From.Column = colIndex - 1; //pictureCol - 1;
                                    pic.From.Row = rowIndex - 1; //currentRow - 1;
                                    pic.From.ColumnOff = 9525; //ExcelHelper.Pixel2MTU(1);
                                    pic.From.RowOff = 9525; // ExcelHelper.Pixel2MTU(1);
                                    //set picture size to fit inside the cell
                                    pic.SetSize(70, 70);
                                }
                            }
                            catch (Exception ex)
                            {
                                ws.Row(rowIndex).Height = 12;
                            }
                        }
                        else
                        {
                            cell.Value = dr[dc.ColumnName];
                        }
                        //Setting Value in cell

                        var border = cell.Style.Border;
                        border.Left.Style =
                            border.Right.Style = ExcelBorderStyle.Thin;

                        //Setting borders of cell

                        colIndex++;
                    }
                }

                //colIndex = 0;
                //foreach (DataColumn dc in dt.Columns) //Creating Headings
                //{
                //    colIndex++;
                //    var cell = ws.Cells[rowIndex, colIndex];

                //    //Setting Sum Formula
                //    cell.Formula = "Sum(" +
                //                    ws.Cells[3, colIndex].Address +
                //                    ":" +
                //                    ws.Cells[rowIndex - 1, colIndex].Address +
                //                    ")";

                //    //Setting Background fill color to Gray
                //    cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                //    cell.Style.Fill.BackgroundColor.SetColor(Color.Gray);
                //}

                //Generate A File with Random name
                Byte[] bin = p.GetAsByteArray();
                File.WriteAllBytes(path, bin);
            }
        }

        public static string SavePhoto(List<string> photos, string path)
        {
            try
            {
                if (photos != null && photos.Any())
                {
                    var web = new WebClient();
                    var img = web.DownloadData(new Uri(photos[0]));
                    var ms = new MemoryStream(img);
                    Image image = Image.FromStream(ms);
                    var name = Guid.NewGuid().ToString();
                    var ttt = new ImageFormatConverter().ConvertToString(image.RawFormat).ToLower();
                    path = path + @"\" + name + "." + ttt;
                    image = ResizeOrigImg(image, 70, 70);
                    image.Save(path);
                    return path;
                }
            }
            catch (Exception ex) { }
            return string.Empty;
        }

        public static Image ResizeOrigImg(Image image, int nWidth, int nHeight)
        {
            int newWidth, newHeight;
            var coefH = (double)nHeight / (double)image.Height;
            var coefW = (double)nWidth / (double)image.Width;
            if (coefW >= coefH)
            {
                newHeight = (int)(image.Height * coefH);
                newWidth = (int)(image.Width * coefH);
            }
            else
            {
                newHeight = (int)(image.Height * coefW);
                newWidth = (int)(image.Width * coefW);
            }

            Image result = new Bitmap(newWidth, newHeight);
            using (var g = Graphics.FromImage(result))
            {
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                g.DrawImage(image, 0, 0, newWidth, newHeight);
                g.Dispose();
            }
            return result;
        }
    }
}
