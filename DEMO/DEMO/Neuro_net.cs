using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace DEMO
{
    [Serializable]
    public class Neuro_net
    {
        public List<List<Neuron>> net;

        public Neuro_net(List<int> init)
        {
            net = new List<List<Neuron>>();

            for (int i = 0; i < init.Count; i++)
            {
                List<Neuron> temp = new List<Neuron>();
                for (int j = 0; j < init[i]; j++)
                {
                    int tik = 0;
                    if (i != 0) tik = init[i - 1];
                    Neuron neuro_temp = new Neuron(tik);
                    temp.Add(neuro_temp);
                }
                net.Add(temp);
            }
        }
        public Neuro_net(Neuro_net arg)
        {
            net = new List<List<Neuron>>();
            for (int i = 0; i < arg.get_Count_layer(); i++)
                net.Add(new List<Neuron>(arg.get_layer(i)));
        }
        static public Neuro_net operator +(Neuro_net arg1, Neuro_net arg2)
        {
            List<List<Neuron>> temp = new List<List<Neuron>>();
            for (int i = 0; i < arg2.get_Count_layer(); i++)
                temp.Add(new List<Neuron>(arg2.get_layer(i)));
            arg1.net = temp;

            return arg1;
        }

        public List<double> rush_aktiv(List<double> arr)
        {
            for (int j = 0; j < net[0].Count; j++)
                net[0][j].y_aktiv = arr[j];

            for (int i = 1; i < net.Count(); i++)
            {
                for (int j = 0; j < net[i].Count; j++)
                {
                    double summ = 0;
                    for (int n = 0; n < net[i - 1].Count; n++)
                        summ += net[i][j][n] * net[i - 1][n].y_aktiv;
                    net[i][j].y_aktiv=(fnc(summ - net[i][j].porog));
                }
            }

            List<double> ret = new List<double>();
            for (int i = 0; i < net[net.Count - 1].Count; i++)
                ret.Add(net[net.Count - 1][i].y_aktiv);

            return ret;
        }
        public double back_aktiv(List<double> etolon)
        {
            for (int i = 0; i < net[net.Count - 1].Count; i++)
                net[net.Count - 1][i].gamma=(net[net.Count - 1][i].y_aktiv - etolon[i]);

            for (int i = net.Count - 2; i > 0; i--)
            {
                for (int j = 0; j < net[i].Count; j++)
                {
                    double summ = 0;
                    for (int n = 0; n < net[i + 1].Count; n++)
                        summ += net[i + 1][n][j] * net[i + 1][n].gamma * 
                            fnc_proisw(net[i + 1][n].y_aktiv);
                    net[i][j].gamma = summ;
                }
            }

            double quad_error = 0;
            for (int i = 1; i < net.Count; i++)
                for (int j = 0; j < net[i].Count; j++)
                    quad_error += (double)Math.Pow(net[i][j].gamma, 2);

            return (double)quad_error / 2;
        }
        public void obichenie(double step)
        {
            for (int i = 1; i < net.Count; i++)
            {
                for (int j = 0; j < net[i].Count; j++)
                {
                    for (int n = 0; n < net[i - 1].Count; n++)
                        net[i][j][n]=(net[i][j][n] - step * net[i][j].gamma *
                        net[i - 1][n].y_aktiv * fnc_proisw(net[i][j].y_aktiv));

                    net[i][j].porog = (net[i][j].porog + step * net[i][j].gamma *
                    fnc_proisw(net[i][j].y_aktiv));
                }
            }
        }

        public List<double> rush_Stat(List<double> arr)
        {
            List<double> ret = new List<double>();

            double summ = 0;
            for (int i = 0; i < arr.Count; i++)
                summ += arr[i];
            ret.Add(summ / arr.Count);

            return ret;
        }

        private double fnc(double a)
        {
            return (double)1 / (1 + (double)Math.Pow(Math.E, -a));
        }
        private double fnc_proisw(double a)
        {
            return (double)a * (1 - a);
        }

        public List<Neuron> get_layer(int i)
        {
            return net[i];
        }
        public int get_Count_layer()
        {
            return net.Count;
        }

        public void SaveObj(string fileName)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, this);
            }    
        }
        public Neuro_net LoadObj(string fileName)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                Neuro_net temp = (Neuro_net)formatter.Deserialize(fs);
                return temp;
            }
        }
    }
}
