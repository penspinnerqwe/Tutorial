using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace DEMO
{
    public partial class TestCreate : Form
    {
        public TestCreate()
        {
            InitializeComponent();
            radioButton1.Checked = true;
        }

        public int GetPrc()
        {
            if (radioButton1.Checked == true)
                return Convert.ToInt32(new Regex(@"\d+").Match(comboBox1.Text).Value);
            else
                return -Convert.ToInt32(new Regex(@"\d+").Match(comboBox1.Text).Value);
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
