using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Shetchiki
{
    public partial class Form1 : Form
    {
        List<string[]> table19320 = new List<string[]>();
        List<string[]> table19450 = new List<string[]>();
        List<string[]> table11490 = new List<string[]>();

        public Form1()
        {
            InitializeComponent();

            dataGridView1.ColumnCount = 12;
            dataGridView1.Columns[0].Name = "Серийный номер радиомодуля №";
            dataGridView1.Columns[1].Name = "Артикул прибора №";
            dataGridView1.Columns[2].Name = "Номер радио сети";
            dataGridView1.Columns[3].Name = "Дата архивации показаний";
            dataGridView1.Columns[4].Name = "Подъезд";
            dataGridView1.Columns[5].Name = "Место установки";
            dataGridView1.Columns[6].Name = "Квартира №";
            dataGridView1.Columns[7].Name = "Начальные показатели прибора";
            dataGridView1.Columns[8].Name = "Тип воды горячая или холодная";
            dataGridView1.Columns[9].Name = "Вес импульса";
            dataGridView1.Columns[10].Name = "Серийный номер счетчика воды №";
            dataGridView1.Columns[11].Name = "Папка";

            dataGridView2.ColumnCount = 10;
            dataGridView2.Columns[0].Name = "Серийный номер радиомодуля №";
            dataGridView2.Columns[1].Name = "Артикул радиомодуля №";
            dataGridView2.Columns[2].Name = "Номер радио сети";
            dataGridView2.Columns[3].Name = "Дата архивации показаний";
            dataGridView2.Columns[4].Name = "Подъезд";
            dataGridView2.Columns[5].Name = "Место установки";
            dataGridView2.Columns[6].Name = "Квартира №";
            dataGridView2.Columns[7].Name = "Начальные показатели прибора";
            dataGridView2.Columns[8].Name = "Серийный номер теплосчетчика";
            dataGridView2.Columns[9].Name = "Папка";

            dataGridView3.ColumnCount = 12;
            dataGridView3.Columns[0].Name = "Номер распределителя";
            dataGridView3.Columns[1].Name = "Артикул прибора №";
            dataGridView3.Columns[2].Name = "Номер радио сети";
            dataGridView3.Columns[3].Name = "Дата архивации показаний";
            dataGridView3.Columns[4].Name = "Подъезд";
            dataGridView3.Columns[5].Name = "Место установки";
            dataGridView3.Columns[6].Name = "Квартира №";
            dataGridView3.Columns[7].Name = "Дата монтажа";
            dataGridView3.Columns[8].Name = "Тип прибора";
            dataGridView3.Columns[9].Name = "Температура";
            dataGridView3.Columns[10].Name = "Тип учета";
            dataGridView3.Columns[11].Name = "Папка";
        }
        private List<string> GetFiles(string path)
        {
            List<string> result = new List<string>();
            List<string> files = Directory.GetFiles(path).ToList();
            List<string> folders = Directory.GetDirectories(path).ToList();
            foreach (string file in files)
            {
                result.Add(file);
            }
            foreach (string folder in folders)
            {
                List<string> list = GetFiles(folder);
                foreach (string file in list)
                {
                    result.Add(file);
                }
            }
            return result;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    List<string> files = GetFiles(fbd.SelectedPath);

                    List<string> files19320 = new List<string>();
                    List<string> files19450 = new List<string>();
                    List<string> files11490 = new List<string>();
  
                    foreach (string file in files)
                    {
                        string[] parts = file.Split('\\');
                        string filename = parts[parts.Length - 1];
                        if (filename.EndsWith(".xml"))
                        {
                            if (filename.StartsWith("19320"))
                            {
                                files19320.Add(file);
                            }
                            if (filename.StartsWith("19450"))
                            {
                                files19450.Add(file);
                            }
                            if (filename.StartsWith("11490"))
                            {
                                files11490.Add(file);
                            }
                        }
                    }

                   
                    dataGridView1.RowCount = files19320.Count;
                    for (int j = 0; j < files19320.Count; j++)
                    {
                        string filename = files19320[j];
                        XmlDocument xDoc = new XmlDocument();
                        xDoc.Load(filename);
                        XmlElement xRoot = xDoc.DocumentElement;
                        foreach (XmlNode xnode in xRoot)
                        {
                            if (xnode.ChildNodes.Count > 0)
                            {
                                string[] row = new string[dataGridView1.ColumnCount];
                                for (int i = 0; i < dataGridView1.ColumnCount; i++)
                                {
                                    var cell = xnode.ChildNodes[i];
                                    if (cell.Name == "W8")
                                    {
                                        row[i] = cell.InnerText == "kalt" ? "холодная" : "горячая";
                                    }
                                    else
                                    {
                                        row[i] = cell.InnerText;
                                    }

                                    if (i == 11)
                                    {
                                        if (filename.Contains("inbetriebnahme"))
                                        {
                                            row[i] = "inbetriebnahme";
                                        }
                                        else if (filename.Contains("reparametrierung"))
                                        {
                                            row[i] = "reparametrierung";
                                        }
                                        else if (filename.Contains("austausch"))
                                        {
                                            row[i] = "austausch";
                                        }

                                    }
                                }

                                table19320.Add(row);
                                dataGridView1.Rows[j].SetValues(row);

                            }

                        }
                    }

                    dataGridView2.RowCount = files19450.Count;
                    for (int j = 0; j < files19450.Count; j++)
                    {
                        string filename = files19450[j];
                        XmlDocument xDoc = new XmlDocument();
                        xDoc.Load(filename);
                        XmlElement xRoot = xDoc.DocumentElement;
                        foreach (XmlNode xnode in xRoot)
                        {
                            if (xnode.ChildNodes.Count > 0)
                            {
                                string[] row = new string[dataGridView2.ColumnCount];
                                for (int i = 0; i < dataGridView2.ColumnCount; i++)
                                {
                                    var cell = xnode.ChildNodes[i];
                                    if (cell != null && cell.Name != "Property")
                                    {
                                        row[i] = cell.InnerText;
                                    }
                                    if (i == 9)
                                    {
                                        if (filename.Contains("inbetriebnahme"))
                                        {
                                            row[i] = "inbetriebnahme";
                                        }
                                        else if (filename.Contains("reparametrierung"))
                                        {
                                            row[i] = "reparametrierung";
                                        }
                                        else if (filename.Contains("austausch"))
                                        {
                                            row[i] = "austausch";
                                        }

                                    }

                                }

                                table19450.Add(row);
                                dataGridView2.Rows[j].SetValues(row);

                            }

                        }
                    }

                    dataGridView3.RowCount = files11490.Count;
                    for (int j = 0; j < files11490.Count; j++)
                    {
                        string filename = files11490[j];
                        XmlDocument xDoc = new XmlDocument();
                        xDoc.Load(filename);
                        XmlElement xRoot = xDoc.DocumentElement;
                        foreach (XmlNode xnode in xRoot)
                        {
                            if (xnode.ChildNodes.Count > 0)
                            {
                                string[] row = new string[dataGridView3.ColumnCount];
                                for (int i = 0; i < dataGridView3.ColumnCount; i++)
                                {
                                    var cell = xnode.ChildNodes[i];
                                    row[i] = cell.InnerText;
                                    if (i == 11)
                                    {
                                        if (filename.Contains("inbetriebnahme"))
                                        {
                                            row[i] = "inbetriebnahme";
                                        }
                                        else if (filename.Contains("reparametrierung"))
                                        {
                                            row[i] = "reparametrierung";
                                        }
                                        else if (filename.Contains("austausch"))
                                        {
                                            row[i] = "austausch";
                                        }

                                    }

                                }


                                table11490.Add(row);
                                dataGridView3.Rows[j].SetValues(row);

                            }

                        }
                    }
                }
            }

            

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string search = textBox1.Text.Trim();

            List<string[]> filteredTable19320 = new List<string[]>();
            foreach (string[] row in table19320)
            {
                if (row[0].Contains(search))
                {
                    filteredTable19320.Add(row);
                }
            }
            dataGridView1.Rows.Clear();
            if (filteredTable19320.Count > 0)
            {
                dataGridView1.RowCount = filteredTable19320.Count;
                for (int i = 0; i < filteredTable19320.Count; i++)
                {
                    string[] row = filteredTable19320[i];
                    dataGridView1.Rows[i].SetValues(row);
                }
            }

            List<string[]> filteredTable19450 = new List<string[]>();
            foreach (string[] row in table19450)
            {
                if (row[0].Contains(search))
                {
                    filteredTable19450.Add(row);
                }
            }
            dataGridView2.Rows.Clear();
            if (filteredTable19450.Count > 0)
            {
                dataGridView2.RowCount = filteredTable19450.Count;
                for (int i = 0; i < filteredTable19450.Count; i++)
                {
                    string[] row = filteredTable19450[i];
                    dataGridView2.Rows[i].SetValues(row);
                }
            }

            List<string[]> filteredTable11490 = new List<string[]>();
            foreach (string[] row in table11490)
            {
                if (row[0].Contains(search))
                {
                    filteredTable11490.Add(row);
                }
            }
            dataGridView3.Rows.Clear();
            if (filteredTable11490.Count > 0)
            {
                dataGridView3.RowCount = filteredTable11490.Count;
                for (int i = 0; i < filteredTable11490.Count; i++)
                {
                    string[] row = filteredTable11490[i];
                    dataGridView3.Rows[i].SetValues(row);
                }
            }
        }
    }
}
