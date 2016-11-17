using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace DEMO
{
    public partial class Training : Form
    {

        static double[,] images;
        static Neuro_net work;
        static double error;

        static ManualResetEvent _event1 = new ManualResetEvent(false);
        static ManualResetEvent _event2 = new ManualResetEvent(false);

        public Training(Neuro_net _work, double[,] imarg)
        {
            InitializeComponent();
            work = new Neuro_net(_work);
            work += _work;
            images = imarg;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

            error = 0;
            for (int i = 0; i < images.Length / 7; i++)
            {
                List<double> temp = new List<double>();
                for (int j = 0; j < 6; j++)
                    temp.Add(images[i, j]);

                work.rush_aktiv(temp);
                error += work.back_aktiv(new List<double>() { images[i, 6] });
            }
            textBox1.Text = error.ToString();
            textBox2.Text = (0.1).ToString();
        }

        static void tichThread()
        {
            while (true)
            {
                _event1.WaitOne();
                _event1.Reset();

                error = 0;
                for (int i = 0; i < images.Length/7; i++)
                {
                    List<double> temp = new List<double>();
                    for (int j = 0; j < 6; j++)
                        temp.Add(images[i, j]);

                    work.rush_aktiv(temp);
                    error += work.back_aktiv(new List<double>() { images[i, 6] });
                    work.obichenie(Convert.ToDouble(0.1));
                }

                _event2.Set();
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            Thread thread1 = new Thread(tichThread);  
            List<double> errorList = new List<double>();

            _event1.Set();
            thread1.Start();

            while (true)
            {
                _event2.WaitOne();
                _event2.Reset();

                Bitmap map = new Bitmap(300, 250);
                Graphics graph = Graphics.FromImage(map);

                drowLines(graph);
                errorList.Add(error);
                drowInfo(graph, errorList);
 
                pictureBox1.Image = map;
                pictureBox1.Image.RotateFlip(RotateFlipType.Rotate180FlipX);

                graph.DrawString(Math.Round(error,9).ToString(), new Font("Calibri", 9F, FontStyle.Regular),
                    new SolidBrush(Color.Red), new PointF(210,20));
                pictureBox1.Refresh();    
                Thread.SpinWait(1);

                try
                {
                    if (error <= Convert.ToDouble(textBox1.Text))
                        break;
                }
                catch
                {
                    MessageBox.Show("Неверный формат!");
                    break;    
                }

                _event1.Set();
            }

            thread1.Abort();
            MessageBox.Show("Готово");
        }

        static public void drowLines(Graphics graph)
        {
            for (int i = 24; i < 245; i += 5)
                graph.DrawLine(new Pen(Color.Black), 24, i, 26, i);
            graph.DrawLine(new Pen(Color.Black), 25, 25, 25, 250);
            graph.DrawLine(new Pen(Color.Black), 23, 245, 25, 250);
            graph.DrawLine(new Pen(Color.Black), 27, 245, 25, 250);
            for (int i = 25; i < 300; i += 5)
                graph.DrawLine(new Pen(Color.Black), i, 23, i, 25);
            graph.DrawLine(new Pen(Color.Black), 25, 24, 300, 24);
            graph.DrawLine(new Pen(Color.Black), 295, 22, 300, 24);
            graph.DrawLine(new Pen(Color.Black), 295, 26, 300, 24);      
        }
        static public void drowInfo(Graphics graph, List<double> errorList)
        {
            if (errorList.Count >= 50)
            {
                int x = 275;
                for (int i = errorList.Count - 2; i > errorList.Count - 51; i--)
                {
                    graph.DrawLine(new Pen(Color.Blue), x, (int)Math.Round(errorList[i + 1] * 200 / errorList[0], 0) + 25,
                        x - 5, (int)Math.Round(errorList[i] * 200 / errorList[0], 0) + 25);
                    x -= 5;
                }
            }
            else
            {
                int x = 25;
                for (int i = 1; i < errorList.Count; i++)
                {
                    graph.DrawLine(new Pen(Color.Blue), x, (int)Math.Round(errorList[i - 1] * 200 / errorList[0], 0) + 25,
                        x + 5, (int)Math.Round(errorList[i] * 200 / errorList[0], 0) + 25);
                    x += 5;
                }
            }
        }

        public Neuro_net GetNet()
        {
            return work;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
