using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Paint
{
    public partial class WelcomeScreen : Form
    {
        public WelcomeScreen()
        {
            InitializeComponent();
        }

        private void btnNewProject_Click(object sender, EventArgs e)
        {
            Main main = new Main();
            this.Hide();
            main.ShowDialog();
            this.Show();
        }

        private void btnAuthor_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Author : Cvetina Cekina \n Faculty number : 1601681055 \n Faculty:FMI \n University : Paisii Hilendarski ");
        }
    }
}
