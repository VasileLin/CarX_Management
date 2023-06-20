using CarX.Classes;
using CarX.Forms;
using CarX.Forms.Modules;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarWashManagementSystem
{
    public partial class MainForm : Form
    {
        string title = "CarX Management System";
        RoundControls roundControls = new RoundControls();
        public MainForm()
        {
            InitializeComponent();
            LoadGrossProfit();
            OpenChildForm(new Dashboard(this));
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            panelSlide.Height = btnDashboard.Height;
            panelSlide.Top = btnDashboard.Top;
            OpenChildForm(new Dashboard(this));
        }

        private void btnEmployer_Click(object sender, EventArgs e)
        {
            panelSlide.Height = btnEmployer.Height;
            panelSlide.Top = btnEmployer.Top;
            OpenChildForm(new Employer());
        }

        private void btnCustomer_Click(object sender, EventArgs e)
        {
            panelSlide.Height = btnCustomer.Height;
            panelSlide.Top = btnCustomer.Top;
            OpenChildForm(new Customer());
        }

        private void btnService_Click(object sender, EventArgs e)
        {
            panelSlide.Height = btnService.Height;
            panelSlide.Top = btnService.Top;
            OpenChildForm(new Service());
        }

        private void btnCash_Click(object sender, EventArgs e)
        {
            panelSlide.Height = btnCash.Height;
            panelSlide.Top = btnCash.Top;
            OpenChildForm(new Cash(this));
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            panelSlide.Height = btnReport.Height;
            panelSlide.Top = btnReport.Top;
            OpenChildForm(new Report());
        }

        private void btnSetting_Click(object sender, EventArgs e)
        {
            panelSlide.Height = btnSetting.Height;
            panelSlide.Top = btnSetting.Top;
            OpenChildForm(new Setting());
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            panelSlide.Height = btnLogout.Height;
            panelSlide.Top = btnLogout.Top;
            if (MessageBox.Show("Are you sure you want to log out?", title, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
               
                Login login = new Login();
                login.emp = null;
                login.Show();
                this.Dispose();
            }
        }


        private Form activeForm = null;
        public void OpenChildForm(Form childForm)
        {
            if (activeForm != null)
                activeForm.Close();        
            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panelChild.Controls.Add(childForm);
            panelChild.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }




        public void LoadGrossProfit()
        {
            Report module = new Report();
            lblRevenus.Text = module.ExtractData($"SELECT ISNULL(SUM(price),0) AS total FROM Cash WHERE date > '{DateTime.Now.AddDays(-7)}' AND status LIKE 'Sold'").ToString("0.00");
            lblCostofGood.Text = module.ExtractData($"SELECT ISNULL(SUM(cost),0) FROM CostofGood WHERE date >'{DateTime.Now.AddDays(-7)}'").ToString("0.00");
            lblGrossProfit.Text = (double.Parse(lblRevenus.Text) - double.Parse(lblCostofGood.Text)).ToString("0.00");
            double revlast7 = module.ExtractData($"SELECT ISNULL(SUM(price),0) AS total FROM Cash WHERE date BETWEEN '{DateTime.Now.AddDays(-14)}' AND '{DateTime.Now.AddDays(-7)}' AND status LIKE 'Sold'");
            double coglast7 = module.ExtractData($"SELECT ISNULL(SUM(cost),0) FROM CostofGood WHERE date BETWEEN '{DateTime.Now.AddDays(-14)}' AND '{DateTime.Now.AddDays(-7)}'");
            double gplast7 = revlast7 - coglast7;

            if (revlast7> double.Parse(lblRevenus.Text))
            {
                picRevenus.Image = CarX.Properties.Resources.down_35px;
            }
            else
            {
                picRevenus.Image = CarX.Properties.Resources.up_35px;
            }

            if (gplast7>double.Parse(lblGrossProfit.Text))
            {
                picGrossProfit.Image = CarX.Properties.Resources.down_35px;
                lblGrossProfit.ForeColor = Color.Red;
            }
            else
            {
                picGrossProfit.Image = CarX.Properties.Resources.up_35px;
                lblGrossProfit.ForeColor = Color.Green;
            }

        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                Application.Exit();
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            int cornerRadius = 10;
            panelRevenus.Region = roundControls.CreateRoundedRegion(panelRevenus.ClientRectangle, cornerRadius);
            panelCoG.Region = roundControls.CreateRoundedRegion(panelCoG.ClientRectangle, cornerRadius);
            panelProfit.Region = roundControls.CreateRoundedRegion(panelProfit.ClientRectangle, cornerRadius);
            panelChild.Region = roundControls.CreateRoundedRegion(panelChild.ClientRectangle, cornerRadius);
        }


       
    }
}
