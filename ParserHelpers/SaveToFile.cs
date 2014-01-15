using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ParserHelpers
{
    public static class SaveToFile
    {
        public static void SaveXml<T>(IEnumerable<T> list, string path, string nameRootElement)
        {
            Type itemType = typeof(T);
            var props = itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance).OrderBy(p => p.Name);

            XDocument doc = File.Exists(path) ? XDocument.Load(path) : new XDocument();

            if (doc.Element(nameRootElement) == null)
            {
                doc.Add(new XElement(nameRootElement));
            }
            Parallel.ForEach(list, x =>
            {
                var root = new XElement(itemType.Name);
                //root.Add(new XAttribute("name", "name goes here"));
                foreach (var prop in props)
                {
                    root.Add(new XElement(prop.Name, prop.GetValue(x)));
                }
                doc.Element(nameRootElement).Add(root);
            });
            doc.Save(path);
        }

        public static void SaveCSV<T>(IEnumerable<T> items, string path)
        {
            Type itemType = typeof(T);
            var props = itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .OrderBy(p => p.Name);

            using (var writer = new StreamWriter(path))
            {
                writer.WriteLine(string.Join(", ", props.Select(p => p.Name)));

                foreach (var item in items)
                {
                    writer.WriteLine(string.Join(", ", props.Select(p => p.GetValue(item, null))));
                }
            }
        }

        //public static void SaveExcel2003<T>(IEnumerable<T> list, string path, string title)
        //{
        //    Type itemType = typeof(T);
        //    var props = itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance).OrderBy(p => p.Name);
        //    var ds = new DataSet("Trtrtrt");
        //    var dt = new DataTable(itemType.Name) { Locale = System.Threading.Thread.CurrentThread.CurrentCulture };
        //    if (File.Exists(path))
        //    {
        //        //Open a DB connection (in this example with OleDB)
        //        string dbConnectionString =
        //            string.Format(
        //                "Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=\"Excel 8.0;HDR=No;IMEX=1\";",
        //                path);
        //        var con = new OleDbConnection(dbConnectionString);
        //        try
        //        {
        //            con.Open();

        //            //Create a query and fill the data table with the data from the DB
        //            string sql = "SELECT * FROM " + itemType.Name + ";";
        //            var cmd = new OleDbCommand(sql, con);
        //            var adptr = new OleDbDataAdapter { SelectCommand = cmd };

        //            adptr.Fill(dt);
        //        }
        //        finally
        //        {
        //            con.Close();
        //        }
        //    }

        //    if (dt.Rows.Count < 1 && dt.Columns.Count < 1)
        //    {
        //        //Создаём столбцы
        //        foreach (var prop in props)
        //        {
        //            //dt.Columns.Add("Dosage", typeof(int));
        //            dt.Columns.Add(prop.Name); //, prop.PropertyType);
        //        }
        //    }

        //    Parallel.ForEach(list, x =>
        //    {
        //        DataRow newRow = dt.NewRow();
        //        //newRow["CompanyID"] = "NewCompanyID";
        //        foreach (var prop in props)
        //        {
        //            var val = prop.GetValue(x);
        //            if (val == null)
        //                newRow[prop.Name] = "";
        //            else if (val is IList)
        //            {
        //                var temp = ((List<string>)val).Aggregate("", (current, v) => current + ((current.Length > 0 ? " , " : "") + v));
        //                newRow[prop.Name] = temp;
        //            }
        //            else
        //            {
        //                newRow[prop.Name] = val.ToString();
        //            }
        //        }
        //        dt.Rows.Add(newRow);
        //    });

        //    //Add the table to the data set
        //    ds.Tables.Add(dt);

        //    //Here's the easy part. Create the Excel worksheet from the data set
        //    ExcelLibrary.DataSetHelper.CreateWorkbook(path, ds);


        //}

        public static void SaveExcel2007<T>(IEnumerable<T> list, string path, string nameBook)
        {
            if (list == null || !list.Any()) return;
            Type itemType = typeof(T);
            var props = itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance).OrderBy(p => p.Name);


            var dt = new DataTable(itemType.Name) { Locale = System.Threading.Thread.CurrentThread.CurrentCulture };
            if (dt.Rows.Count < 1 && dt.Columns.Count < 1)
            {
                //Создаём столбцы
                foreach (var prop in props)
                {
                    //dt.Columns.Add("Dosage", typeof(int));
                    dt.Columns.Add(prop.Name); //, prop.PropertyType);
                }
            }

            foreach(var x in list)
            {
                DataRow newRow = dt.NewRow();
                //newRow["CompanyID"] = "NewCompanyID";
                foreach (var prop in props)
                {
                    var val = prop.GetValue(x);
                    if (val == null)
                        newRow[prop.Name] = "";
                    else if (val is IList)
                    {
                        var temp = "";
                        //var type=prop.GetType().GetGenericTypeDefinition();
                        //var pr2=type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                        foreach (var v in (List<string>)val)
                        {
                            temp += (temp.Length > 0 ? " , " : "") + v;
                        }
                        newRow[prop.Name] = temp;
                    }
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
                ExcelWorksheet ws = null;
                //Create a sheet
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
                    rowIndex = 1;

                    foreach (DataColumn dc in dt.Columns) //Creating Headings
                    {
                        var cell = ws.Cells[rowIndex, colIndex];

                        //Setting the background color of header cells to Gray
                        var fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.Gray);


                        //Setting Top/left,right/bottom borders.
                        var border = cell.Style.Border;
                        border.Bottom.Style =
                            border.Top.Style =
                                border.Left.Style =
                                    border.Right.Style = ExcelBorderStyle.Thin;

                        //Setting Value in cell
                        cell.Value = dc.ColumnName;

                        colIndex++;
                    }
                }
                else
                {
                    ws = p.Workbook.Worksheets.FirstOrDefault();
                }
                rowIndex = ws.Dimension.End.Row;
                foreach (DataRow dr in dt.Rows) // Adding Data into rows
                {
                    colIndex = 1;
                    rowIndex++;
                    foreach (DataColumn dc in dt.Columns)
                    {
                        var cell = ws.Cells[rowIndex, colIndex];
                        //Setting Value in cell
                        cell.Value = dr[dc.ColumnName];

                        //Setting borders of cell
                        var border = cell.Style.Border;
                        border.Left.Style =
                            border.Right.Style = ExcelBorderStyle.Thin;
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
    }
}
