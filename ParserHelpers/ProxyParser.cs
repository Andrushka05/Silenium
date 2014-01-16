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
using HtmlAgilityPack;
using java.util;

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

        public List<string> GetProxyOnHtml(string url)
        {
            var result = new List<string>();
            var html = HtmlAgility.GetHtmlDocument(url, "http://google.com/", null);
            if (html != null)
            {
                
                var link = url.Replace("http://", "").Replace("https://", "");
                var host = link.Substring(0, link.IndexOf("/"));
                link = link.Substring(link.IndexOf("/") + 1, link.Length - 8 - link.IndexOf("/"));
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
                    if (res2.Count == 0 || res2.Count < res.Count)
                    {


                        foreach (var re in res)
                        {
                            var table =
                                html.DocumentNode.SelectNodes("//*[text()[contains(.,'" + re +
                                                              "')]]/following-sibling::td[1]");
                            var port = Regex.Replace(
                                table[0].InnerHtml.Remove(0, table[0].InnerHtml.IndexOf(res[0]) + res[0].Length),
                                @"[^\d]", "");
                            result.Add(re + ":" + port);
                        }
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
                        if (res2.Count == 0 || res2.Count < res.Count)
                        {


                            foreach (var re in res)
                            {
                                var table =
                                    html.DocumentNode.SelectNodes("//*[text()[contains(.,'" + re +
                                                                  "')]]/following-sibling::td[1]");
                                var port = Regex.Replace(
                                    table[0].InnerHtml.Remove(0, table[0].InnerHtml.IndexOf(res[0]) + res[0].Length),
                                    @"[^\d]", "");
                                result.Add(re + ":" + port);
                            }
                        }
                    }
                }
            }
            return result;
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

        public List<string> CheckAvito(string path="")
        {
            var list = path.Length == 0 ? File.ReadAllLines(PathFile).ToList() : File.ReadAllLines(path).ToList();
            var res = new List<string>();
            foreach (var l in list)
            {
                WebClient web=new WebClient {Proxy = new WebProxy(l)};
                var tres = web.DownloadString("http://www.avito.ru/");

            }
            
            return null;
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
