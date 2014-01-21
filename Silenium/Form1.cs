using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenQA.Selenium;
using ParserHelpers;
using xNet.Net;

namespace Silenium
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private List<Link> m_cityList;
        private List<Link> m_catalogList;
        private readonly string m_pathCheckSocks = Environment.CurrentDirectory + @"\checkSocksAvito.txt";
        private async void button1_Click(object sender, EventArgs e)
        {
            var tr=new Thread(()=>
            {
                Stopwatch st = new Stopwatch();
                st.Start();
                var av = new Avito();
                var url = "http://m.avito.ru/pskov";
                //var catalogsList = av.CategoryList(url);
                //progressBar1.Maximum = Avito.CountAds(url);
                var proxis = new List<string>();
                if (File.Exists(m_pathCheckSocks))
                    proxis = File.ReadAllLines(m_pathCheckSocks).ToList();
                proxis = ProxyParser.CheckAvito(proxis);
                foreach (var proxi in proxis)
                {
                    var profile = new OpenQA.Selenium.Firefox.FirefoxProfile();
                    var pr = new OpenQA.Selenium.Proxy { SocksProxy = proxi };
                    profile.SetProxyPreferences(pr);
                    IWebDriver dr = new OpenQA.Selenium.Firefox.FirefoxDriver(profile);
                    dr.Navigate().GoToUrl(url);
                    Thread.Sleep(7000);
                    try
                    {
                        dr.Close();
                    }
                    catch (Exception ex)
                    {
                        
                    }
                }
                //if (proxis.Any())
                //{
                //    Parallel.ForEach(catalogsList,
                //        new ParallelOptions() {MaxDegreeOfParallelism = proxis.Count > 5 ? 5 : proxis.Count},
                //        (link, look, i) =>
                //        {
                //            if (i > 5)
                //                Thread.Sleep((int) i%5*30000);
                //            else if (i > 0)
                //                Thread.Sleep((int) i*30000);
                //            var numProxy = Convert.ToInt32(i);
                //            if (i == 0)
                //                av.GetAdList(link.Url);
                //            else
                //                av.GetAdList(link.Url, proxis[numProxy]);

                //        });
                //}
                //else
                //{
                //    foreach (var link in catalogsList)
                //    {
                //        av.GetAdList(link.Url);
                //    }
                //}

                st.Stop();
                var stds = st.Elapsed.ToString();
               
            });
            tr.Start();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var avito = new Avito();
            label1.Text = "Идёт загрузка каталога...";
            button1.Enabled = false;
            var c = comboBox1.SelectedItem;
            var city = m_cityList.FirstOrDefault(x => x.Name.Equals(c));
            m_catalogList = avito.CategoryList(city.Url);
            comboBox2.Items.AddRange(m_catalogList.Select(x => x.Name).ToArray());
            comboBox2.SelectedIndex = 0;
            button1.Enabled = true;
            label1.Text = "";
        }

        private void Form1_Activated(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //label1.Text = "Идёт загрузка городов...";
            //button1.Enabled = false;
            //var avito = new Avito();
            //m_cityList = avito.CityList();
            //if (m_cityList.Any())
            //{
            //    comboBox1.Items.AddRange(m_cityList.Select(x => x.Name).ToArray());
            //    comboBox1.SelectedIndex = 0;
            //}
            //button1.Enabled = true;
            //label1.Text = "";
        }

        private Task<bool> Avit()
        {

            //var profile = new OpenQA.Selenium.Firefox.FirefoxProfile();
            //String PROXY = "185.12.230.242:8080";
            //var proxy = new OpenQA.Selenium.Proxy
            //{
            //    HttpProxy = PROXY,
            //    FtpProxy = PROXY,
            //    SslProxy = PROXY
            //};
            //profile.SetProxyPreferences(proxy);
            //IWebDriver dr = new OpenQA.Selenium.Firefox.FirefoxDriver(profile);
            //dr.Navigate().GoToUrl(url);
            var av = new Avito();

            var url = "http://m.avito.ru/pskov";
            var error = "";
            return Task<bool>.Factory.StartNew(() =>
            {
                var catalogsList = av.CategoryList(url);
                progressBar1.Maximum = Avito.CountAds(url);
                foreach (var link in catalogsList)
                {
                    var temp = av.GetAdList(link.Url);
                    //SaveToFile.SaveExcel2007(temp, Environment.CurrentDirectory + @"\avito1.xlsx", "Avito");
                    //SaveToFile.SaveCSV(temp, Environment.CurrentDirectory + @"\avito.csv");
                    if (error.Length > 0)
                        MessageBox.Show(error);
                }

                return true;
            });
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Stopwatch st = new Stopwatch();
            st.Start();
            var ir = new Irr();
            var url = "http://pskov.irr.ru/catalog/";
            var error = "";
            var catalogsList = ir.CategoryList(url);
            foreach (var link in catalogsList)
            {
                var temp = ir.GetAdList(link.Url);
                if (error.Length > 0)
                    MessageBox.Show(error);
            }

            st.Stop();
            var stds = st.Elapsed.ToString();

            
        }

        private void bProxy_Click(object sender, EventArgs e)
        {
            var pr=new ProxyParser();
            var socks1 = pr.GetProxyOnHtml("http://socksproxy-list.blogspot.ru");
            var socks2 = pr.GetProxyOnHtml("http://us-socks.blogspot.ru");
            var socks3 = pr.GetProxyOnHtml("http://golden-socks.blogspot.ru");
            var socks4 = pr.GetProxyOnHtml("http://www.socks24.org");
            var socks5 = pr.GetProxyOnHtml("http://www.socks5list.com");
            var socks6 = pr.GetProxyOnHtml("http://www.live-socks.net");
            var socks7 = pr.GetProxyOnHtml("http://www.proxyfire.net/forum/showthread.php?s=29192ab4a1f6133e0f7c038d68694a5c&t=67390");
            var socks8 = pr.GetProxyOnHtml("http://www.proxyfire.net/forum/showthread.php?s=29192ab4a1f6133e0f7c038d68694a5c&t=67391");
            var socks9 = pr.GetProxyOnHtml("http://www.vip-socks.net");
            socks1.AddRange(socks2);
            socks1.AddRange(socks3);
            socks1.AddRange(socks4);
            socks1.AddRange(socks5);
            socks1.AddRange(socks6);
            socks1.AddRange(socks7);
            socks1.AddRange(socks8);
            socks1.AddRange(socks9);
            var res =new HashSet<string>(socks1);
            File.WriteAllLines(Environment.CurrentDirectory+@"\parsSocks-"+DateTime.Now.Year+"-"+DateTime.Now.Month+"-"+DateTime.Now.Day+".txt",res);
            var t=ProxyParser.CheckAvito(res);
            File.WriteAllLines(m_pathCheckSocks, t);
        }


    }
}
