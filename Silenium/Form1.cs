using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenQA.Selenium;
using ParserHelpers;

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

        private void button1_Click(object sender, EventArgs e)
        {
            Stopwatch st = new Stopwatch();
            st.Start();
            var av = new Avito();
            var url = "http://m.avito.ru/pskov";
            var error = "";
            var catalogsList = av.CategoryList(url);
            progressBar1.Maximum = 35000;//Avito.CountAds(url);
            foreach (var link in catalogsList)
            {
                var temp = av.GetAdList(link.Url, progressBar1, ref error);
                if (error.Length > 0)
                    MessageBox.Show(error);
            }

            st.Stop();
            var stds = st.Elapsed.ToString();
            //save
            //SaveToFile.SaveExcel2007(temp,Environment.CurrentDirectory+@"\avito1.xlsx","Avito");
            //SaveToFile.SaveCSV(temp, Environment.CurrentDirectory + @"\avito.csv");

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
                    var temp = av.GetAdList(link.Url, progressBar1, ref error);
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
                var temp = ir.GetAdList(link.Url, progressBar1, ref error);
                if (error.Length > 0)
                    MessageBox.Show(error);
            }

            st.Stop();
            var stds = st.Elapsed.ToString();

            
        }


    }
}
