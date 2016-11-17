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
    public partial class Metod : Form
    {
        public Metod(int a)
        {
            InitializeComponent();
            if (a == 1)
                radioButton1.Checked = true;
            if (a == 2)
                radioButton2.Checked = true;          
        }

        public int GetWindState()
        {
            if (radioButton1.Checked == true)
                return 1;
            if (radioButton2.Checked == true)
                return 2;
            return 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
