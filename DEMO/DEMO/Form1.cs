using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace DEMO
{
    public partial class Form1 : Form
    {
        Neuro_net work;
        int type = 1;
  
        public Form1()
        {
            InitializeComponent();
            work = new Neuro_net(new List<int> { 6, 20, 1 });
            type = 1;
        }

        private void сохранитьToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            work.SaveObj("Obj.dat");
        }
        private void загрузитьToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            work += work.LoadObj("Obj.dat");
        }
        private void обучитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool error = false;

            double[,] images = new double[dataGridView3.Rows.Count - 1, 7];
            for (int i = 0; i < dataGridView3.Rows.Count - 1; i++)
            {
                int tik = 0;
                while (tik < 7)
                {
                    double elem = parse(dataGridView3[tik, i].Value.ToString());
                    images[i, tik] = elem;
                    tik++;

                    if (elem == -1)
                        error = true;
                }
            }

            if (!error)
            {
                Training wnd = new Training(work, images);
                if (wnd.ShowDialog() == DialogResult.OK)
                    work += wnd.GetNet();
            }
            else
                MessageBox.Show("Неверный формат таблицы");
        }

        private void загрузитьToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string[] args;
            OpenFileDialog wnd = new OpenFileDialog();
            if (wnd.ShowDialog() == DialogResult.OK)
            {
                args = File.ReadAllLines(wnd.FileName);
                for (int i = 0; i < args.Length; i++)
                {
                    Match mtch = new Regex(@"\w+").Match(args[i]);
                    dataGridView1.Rows.Add();

                    int tik = 0;
                    while (mtch.Success)
                    {
                        dataGridView1[tik, i].Value = mtch.Value;
                        mtch = mtch.NextMatch();
                        tik++;
                    }

                    if (tik != 7)
                    {
                        MessageBox.Show("Неверный формат данных");
                        break;
                    }
                }
            }

            сформироватьТестToolStripMenuItem.Enabled = true;
            сформироватьУчителяToolStripMenuItem.Enabled = true;
            тестироватьToolStripMenuItem.Enabled = true;
        }
        private void сформироватьТестToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TestCreate wnd = new TestCreate();
            if (wnd.ShowDialog() == DialogResult.OK)
            {
                int step = Math.Abs((dataGridView1.Rows.Count-1) * wnd.GetPrc() / 100);
                int tak = 0;
                for (int i = wnd.GetPrc() > 0 ? 0 : 1; i < (dataGridView1.Rows.Count - 1); i += 
                    (dataGridView1.Rows.Count - 1) / step)
                {
                    int tik = 0;
                    dataGridView2.Rows.Add();
                    while (tik < 7)
                    {
                        dataGridView2[tik, tak].Value = dataGridView1[tik, i].Value;
                        tik++;
                    }
                    tak++;
                }
            }
        }
        private void сформироватьУчителяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TestCreate wnd = new TestCreate();
            if (wnd.ShowDialog() == DialogResult.OK)
            {
                int step = Math.Abs((dataGridView1.Rows.Count - 1) * wnd.GetPrc() / 100);
                int tak = 0;
                for (int i = wnd.GetPrc() > 0 ? 0 : 1; i < (dataGridView1.Rows.Count - 1); i +=
                    (dataGridView1.Rows.Count - 1) / step)
                {
                    int tik = 0;
                    dataGridView3.Rows.Add();
                    while (tik < 7)
                    {
                        dataGridView3[tik, tak].Value = dataGridView1[tik, i].Value;
                        tik++;
                    }
                    tak++;
                }
            }
        }       
        private void тестироватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool error = false;

            for (int i = 0; i < dataGridView2.Rows.Count - 1; i++)
            {
                int tik = 0;
                List<double> image = new List<double>();
                while (tik < 6)
                {
                    double elem = parse(dataGridView2[tik, i].Value.ToString());
                    image.Add(elem);
                    tik++;

                    if (elem == -1)
                        error = true;
                }

                if (error)
                {
                    MessageBox.Show("Неверный формат таблицы");
                    break;
                }

                if (type == 1)
                    if (work.rush_aktiv(image)[0] >= 0.5)
                        dataGridView2[7, i].Value = "NB";
                    else
                        dataGridView2[7, i].Value = "B";
                if (type == 2)
                    if (work.rush_Stat(image)[0] >= 0.5)
                        dataGridView2[7, i].Value = "NB";
                    else
                        dataGridView2[7, i].Value = "B";
            }   
            
            rOCАнализToolStripMenuItem.Enabled = true;
        }
        private void rOCАнализToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool error = false;

            double[,] args = new double[dataGridView2.Rows.Count - 1, 2];
            double[] foo = new double[dataGridView2.Rows.Count - 1];

            for (int i = 0; i < dataGridView2.Rows.Count - 1; i++)
            {
                int tik = 0;
                while (tik < 2)
                {
                    double elem = parse(dataGridView2[tik + 6, i].Value.ToString());
                    args[i, tik] = elem;
                    tik++;

                    if (elem == -1)
                        error = true;
                }

                tik = 0;
                List<double> image = new List<double>();
                while (tik < 6)
                {
                    double elem = parse(dataGridView2[tik, i].Value.ToString());
                    image.Add(elem);
                    tik++;

                    if (elem == -1)
                        error = true;
                }

                if (type == 1)
                    foo[i] = work.rush_aktiv(image)[0];
                if (type == 2)
                    foo[i] = work.rush_Stat(image)[0];
            }

            if (!error)
            {
                RocAn wnd = new RocAn(args, foo);
                wnd.Show();
            }
            else
                MessageBox.Show("Неверный формат таблицы");
        }

        private void выборМетодаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Metod wnd = new Metod(type);
            if (wnd.ShowDialog() == DialogResult.OK)
                type = wnd.GetWindState();
        }

        private double parse(string str)
        {
            switch (str)
            {
                case "P": return 1;
                case "A": return 0.5;
                case "N": return 0;
                case "B": return 0;
                case "NB": return 1;
            }
            return -1;
        }
    }
}
