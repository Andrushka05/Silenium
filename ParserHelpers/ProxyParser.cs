using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;
using com.sun.org.apache.regexp.@internal;
using HtmlAgilityPack;
using java.util;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using xNet.Net;
using FirefoxDriver = org.openqa.selenium.firefox.FirefoxDriver;

namespace ParserHelpers
{
    public class ProxyParser
    {
        public ProxyParser()
        {
            UrlList=new List<string>(){""};
        }

        public string PathFile="proxy.txt";
        public List<string> UrlList;

				public static List<string> GetProxyOnHtml(string url)
        {
            var result = new List<string>();
            var html = HtmlAgility.GetHtmlDocument(url, "http://google.com/", null);
            if (html != null)
            {
                
                var link = url.Replace("http://", "").Replace("https://", "");
                var host = "http://" + link.Substring(0, link.IndexOf("/") + 1);
                try
                {
                    link = link.Substring(link.IndexOf("/") + 1, link.Length - 8 - link.IndexOf("/"));
                }
                catch (Exception ex)
                {
                    
                }
                var aPage = html.DocumentNode.SelectNodes("//a[contains(concat(' ', @href, ' '), '"+link+"')]");
                if (aPage == null)
                {
                    var res =
                        Regex.Matches(html.DocumentNode.InnerHtml,
                            @"\b(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b")
                            .Cast<Match>()
                            .Select(x => x.Groups[0].Value)
                            .ToList();
                    res = new HashSet<string>(res).ToList();
                    var res2 =
                        Regex.Matches(html.DocumentNode.InnerHtml,
                            @"\b(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b\:[0-9]{0,4}")
                            .Cast<Match>()
                            .Select(x => x.Groups[0].Value)
                            .ToList();
                    res2 = new HashSet<string>(res2).ToList();
                    var res3 = Regex.Matches(WebUtility.UrlDecode(html.DocumentNode.InnerHtml), @"\b(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)").Cast<Match>()
                                .Select(x => x.Groups[0].Value)
                                .ToList();
                    res3 = new HashSet<string>(res3).ToList();
                    if (res3.Count > res.Count)
                        res = res3.GetRange(0, res3.Count);
                    if (res2.Count == 0 || res2.Count < res.Count)
                    {
                        foreach (var re in res.GetRange(0, res.Count))
                        {
                            var temp = res2.Where(x => x.Contains(re));
                            if (temp != null && temp.Any())
                                res.Remove(re);
                        }
                        var doc = new HtmlAgilityPack.HtmlDocument();
                        doc.LoadHtml(WebUtility.UrlDecode(html.DocumentNode.InnerHtml));

                        foreach (var re in res)
                        {
                            var table =
                                doc.DocumentNode.SelectNodes("//*[text()[contains(.,'" + re +
                                                              "')]]/following-sibling::td[1]");
                            var port = Regex.Replace(
                                table[0].InnerHtml.Remove(0, table[0].InnerHtml.IndexOf(res[0]) + res[0].Length),
                                @"[^\d]", "");
                            result.Add(re + ":" + port);
                        }
                    }
                    else
                    {
                        result.AddRange(res2);
                    }
                }
                else
                {

                    foreach (var a in aPage)
                    {
                        var l = a.Attributes["href"].Value;
                        if (!l.Contains(host))
                            l = host + l;
                        var html2 = HtmlAgility.GetHtmlDocument(l, url, null);
                        var res =
                        Regex.Matches(html2.DocumentNode.InnerHtml,
                            @"\b(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)")
                            .Cast<Match>()
                            .Select(x => x.Groups[0].Value)
                            .ToList();
                        res = new HashSet<string>(res).ToList();
                        var res2 =
                            Regex.Matches(html2.DocumentNode.InnerHtml,
                                @"\b(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b\:[0-9]{0,4}")
                                .Cast<Match>()
                                .Select(x => x.Groups[0].Value)
                                .ToList();
                        res2 = new HashSet<string>(res2).ToList();
                        var res3 = Regex.Matches(WebUtility.UrlDecode(html2.DocumentNode.InnerHtml), @"\b(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)").Cast<Match>()
                                .Select(x => x.Groups[0].Value)
                                .ToList(); 
                        res3 = new HashSet<string>(res3).ToList();
                        if (res3.Count > res.Count)
                            res = res3.GetRange(0,res3.Count);
                        
                        if (res2.Count == 0 || res2.Count < res.Count)
                        {
                            foreach (var re in res.GetRange(0,res.Count))
                            {
                                var temp = res2.Where(x => x.Contains(re));
                                if (temp != null &&temp.Any())
                                    res.Remove(re);
                            }
                            var doc = new HtmlAgilityPack.HtmlDocument();
                            doc.LoadHtml(WebUtility.UrlDecode(html2.DocumentNode.InnerHtml));
                            foreach (var re in res)
                            {
                                ////*[contains(text(),'match')]
                                var table =
                                    doc.DocumentNode.SelectNodes("//*[text()[contains(.,'" + re +"')]][not(*)]");
                                                                  //"')]/following-sibling::*[1]");
                                var port = Regex.Replace(table[0].InnerHtml.Remove(0, table[0].InnerHtml.IndexOf(res[0]) + res[0].Length),
                                    @"[^\d]", "");
                                result.Add(re + ":" + port);
                            }
                        }
                        else
                        {
                            result.AddRange(res2);
                        }
                    }
                }
            }
            return new List<string>(new HashSet<string>(result));
        } 

        public void GetProxy(string url)
        {
            var html = HtmlAgility.GetHtmlDocument(url, "", null);
            var res = Regex.Matches(html.DocumentNode.InnerHtml, @"\b(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b\:[0-9]{0,4}").Cast<Match>().Select(x => x.Groups[0].Value).ToList();
            res = new HashSet<string>(res).ToList();
            var result=new List<string>();
            foreach (var re in res)
            {
                var ip = re.Substring(0, re.IndexOf(":"));
                var port = re.Replace(ip, "").Replace(":","");
                
                //if (IsPing(ip))
                //{
                    var sock = ConnectSocket(new IPEndPoint(IPAddress.Parse(ip), Int32.Parse(port)));
                    if(sock!=null&&sock.Connected)
                        result.Add(re);
                //}

            }
            var av=CheckIpToSite(result);
            File.AppendAllLines(PathFile,result);
        }
        public void GetProxy()
        {
            foreach (var url in UrlList)
            {
                var html = HtmlAgility.GetHtmlDocument(url, "", null);
                var res = Regex.Matches(html.DocumentNode.InnerText, @"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}:\d{1,5}$");
                //@"\b(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b\:[0-9]{0,4}"

            }
        }
        public static bool CheckAvito(string socks)
        {
            using (var request = new HttpRequest())
                {
                    try
                    {
                        request.UserAgent = HttpHelper.ChromeUserAgent();
                        request.Proxy = Socks5ProxyClient.Parse(socks); //114.33.31.101:17012
												var tr = request.Get("http://m.avito.ru/pskov/vakansii/").ToString();
                        return true;
                    }
                    catch (HttpException http)
                    {
                        try
                        {
                            if (http.Status == xNet.Net.HttpExceptionStatus.ConnectFailure)
                            {
                                request.Proxy = Socks4ProxyClient.Parse(socks); //114.33.31.101:17012
																var tr = request.Get("http://m.avito.ru/pskov/vakansii/").ToString();
                                return true;
                            }
                        }
                        catch (Exception exc)
                        { }
                    }
                    catch (Exception ex) { }
                }
            

            return false;
        }
        public static List<string> CheckAvito(IEnumerable<string> list)
        {
            //var list = path.Length == 0 ? File.ReadAllLines(PathFile).ToList() : File.ReadAllLines(path).ToList();
            var res = new List<string>();
            Parallel.ForEach(list,l=>
            {
                using (var request = new HttpRequest())
                {
                    try
                    {
                        request.UserAgent = HttpHelper.ChromeUserAgent();
                        request.ConnectTimeout = 700;
                        request.Proxy = Socks5ProxyClient.Parse(l); //114.33.31.101:17012
                        var tr = request.Get("http://www.avito.ru").ToString();
                        res.Add(l);
                    }
                    catch (HttpException http)
                    {
                        try
                        {
                            if (http.Status == xNet.Net.HttpExceptionStatus.ConnectFailure)
                            {
                                request.Proxy = Socks4ProxyClient.Parse(l); //114.33.31.101:17012
                                var tr = request.Get("http://www.avito.ru").ToString();
                                res.Add(l);
                            }
                        }
                        catch (Exception exc)
                        { }
                    }
                    catch (Exception ex) { }
                }
            });
            
            return res;
        }

        public List<string> CheckIpToSite(IEnumerable<string> ips, string site = "http://www.avito.ru/")
        {
            var res = new List<string>();
            foreach (var ip in ips)
            {
                var i = ip.Substring(0, ip.IndexOf(":"));
                var port = ip.Replace(i, "").Replace(":", "");
                WebClient web = new WebClient { Proxy = new WebProxy(ip,Int32.Parse(port)) };
                var temp = web.DownloadString("http://www.avito.ru/");
                if(temp.Length>0)
                    res.Add(ip);
            }

            return res;
        }

        /// <summary>
        /// Разделяем списки на сокс и http и сохраняем в разных файлах
        /// </summary>
        public static bool SaveInOnTypeProxy(string path, string pathSocks = "socksMyFind.txt", string pathProxy = "proxyMyFind.txt")
        {
            if (File.Exists(path))
            {
                var lines = File.ReadAllLines(path);
                List<string> socks = new List<string>();
                List<string> http = new List<string>();
                foreach (var line in lines)
                {
                    if (line.Contains("1080") || line.Contains("1081"))
                    {
                        socks.Add(line);
                    }
                    else
                    {
                        http.Add(line);
                    }
                }

                File.AppendAllLines(pathProxy, http);
                File.AppendAllLines(pathSocks, socks);
                File.WriteAllText(path, "");
                return true;
            }
            return false;
        }
        //public static string GetIp(WebProxy pr)
        //{
        //    string link = "http://ip2country.sourceforge.net/ip2c.php?format=JSON";
        //    var s = DownloadString(link, pr, true);
        //    if (string.IsNullOrEmpty(s) || s.IndexOf("{ip:") < 0)
        //        return string.Empty;
        //    var res = JsonConvert.DeserializeObject<Dictionary<string, string>>(s);
        //    return res != null ? res["ip"] : string.Empty;
        //}

        //public static string GetGeolocation(WebProxy pr)
        //{
        //    string link = "http://api.hostip.info/country.php?ip=" + (pr != null ? pr.Address.DnsSafeHost : string.Empty);
        //    //var res = JsonConvert.DeserializeObject<Dictionary<string, string>>(Downloads.DownloadString(link, null, true));
        //    var res = DownloadString(link, null, true);
        //    return res ?? string.Empty;
        //}

        public static HashSet<string> ReadIpList(string path)
        {
            if (File.Exists(path))
            {
                try
                {
                    var strHS = new HashSet<string>(File.ReadAllLines(path));
                    return strHS;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error read file: " + path);
                    return null;
                }
            }
            return null;
        }

        public static string GetHostOfLink(string link)
        {
            var res = string.Empty;
            var host = Regex.Replace(link, @"(http(s)?://)(www.)|(www.)|(http(s)?://)", String.Empty);
            res = host.IndexOf("/") > 0 ? host.Remove(host.IndexOf("/")) : host;
            return res;
        }
        public bool IsPing(string address)
        {
            Ping ping = new Ping();

            try
            {
                PingReply reply = ping.Send(address, 2000);

                if (reply.Status == IPStatus.TimedOut)
                {
                    reply = ping.Send(address, 2000);
                }

                //m_TimePing = reply.RoundtripTime;
                return (reply.Status == IPStatus.Success);
            }
            catch (PingException e)
            {
                return false;
            }
        }

        public static Socket ConnectSocket(IPEndPoint ipEnd)
        {
            IPHostEntry hostEntry = null;
            Socket tempSocket = null;
            tempSocket = new Socket(ipEnd.AddressFamily, SocketType.Stream, ProtocolType.Tcp) { SendTimeout = 2000, ReceiveTimeout = 2000 };
            try
            {
                tempSocket.Connect(ipEnd);
            }
            catch (SocketException ex)
            {
                Console.WriteLine("Connect error" + ipEnd.Address.ToString() + ":" + ipEnd.Port.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error ConnectSocket");
                tempSocket = null;
            }

            return tempSocket;
        }
    }
}
