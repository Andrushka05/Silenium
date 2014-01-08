using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using Selenium;
using ParserHelpers;

namespace Silenium
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var av=new Avito();
            Stopwatch st=new Stopwatch();
            st.Start();
            var temp=av.GetAdList("http://www.avito.ru/pskov");
            st.Stop();
            var stds = st.Elapsed.ToString();
            //save
            SaveToFile.SaveExcel2007(temp,Environment.CurrentDirectory+@"\avito1.xlsx","Avito");
            SaveToFile.SaveCSV(temp, Environment.CurrentDirectory + @"\avito.csv");
        }
    }
}
