using CarWashManagementSystem;
using CarX.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarX.Forms.Modules
{
    public partial class SplashScreen : Form
    {
        RoundControls roundControls = new RoundControls();
        public SplashScreen()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            panel2.Width += 3;
            if (panel2.Width >= 700)
            {

                timer1.Stop();
                Login loginForm = new Login();
                this.Hide();
                loginForm.ShowDialog();
               
            }
        }

        private void SplashScreen_Load(object sender, EventArgs e)
        {
            int cornerRadius = 10;
            Region = roundControls.CreateRoundedRegion(ClientRectangle, cornerRadius);
            timer1.Start();
        }
    }
}
