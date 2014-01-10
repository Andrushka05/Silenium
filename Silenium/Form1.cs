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
            var url = "http://m.avito.ru/pskov";
						var error = "";
            var temp=av.GetAdList(url,progressBar1,ref error);
            st.Stop();
            var stds = st.Elapsed.ToString();
            //save
            SaveToFile.SaveExcel2007(temp,Environment.CurrentDirectory+@"\avito1.xlsx","Avito");
            SaveToFile.SaveCSV(temp, Environment.CurrentDirectory + @"\avito.csv");
					if(error.Length>0)
						MessageBox.Show(error);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
