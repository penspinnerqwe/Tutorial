using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEMO
{
    [Serializable]
    public class Neuron
    {
        private List<double> wesa;
        public double y_aktiv { get; set; }
        public double gamma { get; set; }
        public double porog { get; set; }

        public Neuron(int kol)
        {
            Random rand=new Random();

            wesa = new List<double>();
            for (int i = 0; i < kol; i++)
                wesa.Add((double)rand.Next(0, 1000) / 1000);
            porog = (double)rand.Next(0, 1000) / 1000;

            y_aktiv = 0;
            gamma = 0;
        }
        public Neuron(Neuron arg)
        {
            wesa = new List<double>();
            this.y_aktiv = (arg.y_aktiv);
            this.gamma = (arg.gamma);
            this.porog = (arg.porog);
            for (int i = 0; i < arg.getCount(); i++)
                wesa.Add(arg[i]);
        }
        public double this[int index]
        {
            set { wesa[index] = value; }
            get { return wesa[index]; }
        }
        public int getCount()
        {
            return wesa.Count;
        }
    }
}
