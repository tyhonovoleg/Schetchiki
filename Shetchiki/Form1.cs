using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Shetchiki
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            dataGridView1.ColumnCount = 11;
            dataGridView1.Columns[0].Name = "Серийный номер радиомодуля №";
            dataGridView1.Columns[1].Name = "Артикул прибора №";
            dataGridView1.Columns[2].Name = "Номер радио сети";
            dataGridView1.Columns[3].Name = "Последний день текущего месяца";
            dataGridView1.Columns[4].Name = "Подъезд";
            dataGridView1.Columns[5].Name = "Место установки";
            dataGridView1.Columns[6].Name = "Квартира №";
            dataGridView1.Columns[7].Name = "Начальные показатели прибора";
            dataGridView1.Columns[8].Name = "Тип воды горячая или холодная";
            dataGridView1.Columns[9].Name = "?";
            dataGridView1.Columns[10].Name = "Серийный номер счетчика воды №";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            string[] filenames = openFileDialog1.FileNames;
            dataGridView1.RowCount = filenames.Length;
            for (int j = 0; j < filenames.Length; j++)
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(filenames[j]);
                XmlElement xRoot = xDoc.DocumentElement;
                foreach (XmlNode xnode in xRoot)
                {
                    if (xnode.ChildNodes.Count > 0)
                    {
                        string[] row = new string[11];
                        for (int i = 0; i < 11; i++)
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

                        }

                        dataGridView1.Rows[j].SetValues(row);

                    }

                }
            }

        }
    }
}
