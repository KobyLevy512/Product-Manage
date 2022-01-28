using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProductManage
{
    public partial class Form1 : Form
    {
        int id = 1;
        public Form1()
        {
            InitializeComponent();
            saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
        }

        private void ClearBtn_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
        }

        private void AddBtn_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "")
                dataGridView1.Rows.Add(id++, textBox1.Text, textBox2.Text, textBox3.Text, int.Parse(textBox2.Text) * int.Parse(textBox3.Text));
            else MessageBox.Show("Empty field not alowed");
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                textBox1.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                textBox2.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
                textBox3.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            }
        }

        private void RemoveBtn_Click(object sender, EventArgs e)
        {
            var j = dataGridView1.SelectedRows;
            for (int i = 0; i< j.Count; i++)
            {
                dataGridView1.Rows.Remove(j[i]);
            }
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                dataGridView1.SelectedRows[0].Cells[1].Value = textBox1.Text;
                dataGridView1.SelectedRows[0].Cells[2].Value = textBox2.Text;
                dataGridView1.SelectedRows[0].Cells[3].Value = textBox3.Text;
                dataGridView1.SelectedRows[0].Cells[4].Value = int.Parse(textBox2.Text) * int.Parse(textBox3.Text);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Total:" + Total(0, 0).ToString());
        }
        private int Total(int i, int v)
        {
            if(i < dataGridView1.Rows.Count)
            {
                return Total(i + 1, v + int.Parse(dataGridView1.Rows[i].Cells[4].Value.ToString()));
            }
            return v;
        }
        private void Lowest(int i, DataGridViewCellCollection v)
        {
           
            if (i < dataGridView1.Rows.Count)
            {
                if(int.Parse(dataGridView1.Rows[i].Cells[4].Value.ToString())< int.Parse(v[4].Value.ToString()))
                {
                    v = dataGridView1.Rows[i].Cells;
                }
                Lowest(i + 1, v);
            }
            else
            MessageBox.Show("Lowest:" + v[1].Value.ToString() + ".Total:" + v[4].Value.ToString());
        }
        private void Highest(int i, DataGridViewCellCollection v)
        {

            if (i < dataGridView1.Rows.Count)
            {
                if (int.Parse(dataGridView1.Rows[i].Cells[4].Value.ToString()) > int.Parse(v[4].Value.ToString()))
                {
                    v = dataGridView1.Rows[i].Cells;
                }
                Highest(i + 1, v);
            }
            else
                MessageBox.Show("Highest:" + v[1].Value.ToString() + ".Total:" + v[4].Value.ToString());
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if(dataGridView1.Rows.Count>0)
                Lowest(1, dataGridView1.Rows[0].Cells);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
                Highest(1, dataGridView1.Rows[0].Cells);
        }

        private string PrintData(int y,string ret)
        {
            if (y < dataGridView1.Rows.Count)
            {
                ret += "index:" + dataGridView1.Rows[y].Cells[0].Value + "\n";
                ret += "name:" + dataGridView1.Rows[y].Cells[1].Value + "\n";
                ret += "amount:" + dataGridView1.Rows[y].Cells[2].Value + "\n";
                ret += "price:" + dataGridView1.Rows[y].Cells[3].Value + "\n";
                ret += "total:" + dataGridView1.Rows[y].Cells[4].Value + "\n\n";
                return PrintData(y + 1, ret);
            }
            return ret;
        }

        private void LoadFile(string path)
        {
            int index = 0;
            string name = "", amount = "", price ="";
            dataGridView1.Rows.Clear();
            foreach (string line in File.ReadAllLines(path))
            {
                if(line.IndexOf("index:")>-1)
                {
                    index = int.Parse(line.Substring(line.IndexOf("index:") + 6));
                }
                else if (line.IndexOf("name:") > -1)
                {
                    name = line.Substring(line.IndexOf("name:") + 5);
                }
                else if (line.IndexOf("amount:") > -1)
                {
                    amount = line.Substring(line.IndexOf("amount:") + 7);
                }
                else if (line.IndexOf("price:") > -1)
                {
                    price = line.Substring(line.IndexOf("price:") + 6);
                }
                else if (line.IndexOf("total:") > -1)
                {
                    dataGridView1.Rows.Add(index, name, amount, price, line.Substring(line.IndexOf("total:") + 6));
                }
            }
        }

        private void changeColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                BackColor = colorDialog1.Color;
                AddBtn.ForeColor = colorDialog1.Color;
                RemoveBtn.ForeColor = colorDialog1.Color;
                SaveBtn.ForeColor = colorDialog1.Color;
                ClearBtn.ForeColor = colorDialog1.Color;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string data = PrintData(0, "");
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(saveFileDialog1.FileName, data);
            }
            Process.Start(saveFileDialog1.FileName);
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                LoadFile(openFileDialog1.FileName);
            }
        }
    }
}
