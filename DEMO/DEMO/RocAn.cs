using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DEMO
{
    public partial class RocAn : Form
    {
        public RocAn(double[,] args,double[] foo)
        {
            InitializeComponent();
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

            int TP = 0, TN = 0, FP = 0, FN = 0;
            int P=0, N=0;
            for (int i = 0; i < args.Length / 2; i++)
            {
                if (args[i, 0] == args[i, 1])
                {
                    if (args[i, 0] == 1)
                        TP++;
                    else
                        FP++;
                }
                else
                {
                    if (args[i, 0] == 1)
                        TN++;
                    else
                        FN++;
                }

                if (args[i, 0] == 1)
                    P++;
                else
                    N++;
            }
            textBox1.Text = TP.ToString();
            textBox2.Text = TN.ToString();
            textBox3.Text = FP.ToString();
            textBox4.Text = FN.ToString();

            double TPR = 0, FPR = 0;         
            if ((TN + FP)!=0)
                FPR = (double)FP / (TN + FP);
             if ((TP + FN)!=0)
                TPR = (double)TP / (TP + FN);
            textBox5.Text = (TPR * 100).ToString() + " %";
            textBox6.Text = (FPR * 100).ToString() + " %";

            List<List<double>> rc = new List<List<double>>(rocCr(args,foo,P,N));

            Bitmap map = new Bitmap(1010, 1010);
            Graphics greph = Graphics.FromImage(map);

            for (int i = 0; i < rc.Count - 1; i++)
                greph.DrawLine(new Pen(Color.Red), (int)rc[i][0]+8, (int)rc[i][1]+8,
                    (int)rc[i + 1][0]+8, (int)rc[i + 1][1]+8);
            greph.DrawLine(new Pen(Color.Blue), 8, 8,1008, 1008);
            drowLines(greph);

            pictureBox1.Image = map;
            pictureBox1.Image.RotateFlip(RotateFlipType.Rotate180FlipX);    
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private List<List<double>> rocCr(double[,] args, double[] foo, int P, int N)
        {
            List<List<double>> res = new List<List<double>>();
            double TP = 0, FP = 0;
            for (int i = 0; i < args.Length / 2; i++)
                for (int j = 0; j < args.Length / 2 - 1; j++)
                    if (foo[j] < foo[j + 1])
                    {
                        double[] tempar = new double[2];
                        tempar[0] = args[j, 0];
                        tempar[1] = args[j, 1];
                        args[j, 0] = args[j + 1, 0];
                        args[j, 1] = args[j + 1, 1];
                        args[j + 1, 0] = tempar[0];
                        args[j + 1, 1] = tempar[1];

                        double temp = foo[j];
                        foo[j] = foo[j + 1];
                        foo[j + 1] = temp;
                    }

            res.Add(new List<double>() { 0, 0 });
            for (int i = 0; i < args.Length / 2; i++)
            {
                if (args[i,0] == 0)
                    FP += (double)1 / N;
                else
                    TP += (double)1 / P;
                res.Add(new List<double>() { FP, TP });
            }

            for (int i = 0; i < res.Count; i++)
            {
                res[i][0] = Math.Round(res[i][0], 3) * 1000;
                res[i][1] = Math.Round(res[i][1], 3) * 1000;
            }

            return res;
        }
        private void drowLines(Graphics greph)
        {
            greph.DrawLine(new Pen(Color.Black), 3, 3, 3, 1006);
            for (int i = 0; i < 1002; i += 50)
                greph.DrawLine(new Pen(Color.Black), 0, i, 6, i);

            greph.DrawLine(new Pen(Color.Black), 1006, 3, 1000, 6);
            greph.DrawLine(new Pen(Color.Black), 1006, 3, 1000, 0);

            greph.DrawLine(new Pen(Color.Black), 3, 3, 1006, 3);
            for (int i = 0; i < 1002; i += 50)
                greph.DrawLine(new Pen(Color.Black), i, 0, i, 6);

            greph.DrawLine(new Pen(Color.Black), 3, 1006, 6, 1000);
            greph.DrawLine(new Pen(Color.Black), 3, 1006, 0, 1000);
        }
    }
}
