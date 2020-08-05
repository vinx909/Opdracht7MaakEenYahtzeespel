using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Opdracht7MaakEenYahtzeespel
{
    public partial class Form1 : Form
    {
        private Yahtzee yahtzee;
        public Form1()
        {
            InitializeComponent();
            yahtzee = new Yahtzee();
            LabelTest.Text = yahtzee.RunTest();
        }
    }
}
