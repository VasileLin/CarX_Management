using CarWashManagementSystem;
using CarX.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarX.Forms.Modules
{
    public partial class Login : Form
    {
        DbConnection connection;
        SqlCommand command = new SqlCommand();
        string title = "Carx Management System";
        SqlDataReader dataReader;
        public EmployerClass emp;
        MainForm mainForm;
        RoundControls roundControls = new RoundControls();
        public Login()
        {
            InitializeComponent();
            KeyPreview = true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void PerformLoginAction()
        {


            string[] name = txtLogin.Text.Split(' ');


            try
            {
                if (txtLogin.Text == "admin" && txtPassword.Text == "@dm1n15t7@t07")
                {
                    this.Hide();
                    mainForm = new MainForm();
                    mainForm.btnEmployer.Enabled = true;
                    mainForm.btnSetting.Enabled = true;
                    mainForm.btnService.Enabled = true;
                    mainForm.btnReport.Enabled = true;
                    mainForm.lblUser.Text = $"Welcome Administrator";
                    mainForm.ShowDialog();
                    return;
                }


                connection = new DbConnection();
                command = new SqlCommand($"Select name,role FROM Employer WHERE name LIKE '{name[0]}%' AND name LIKE '%{name[1]}'" +
                    $" AND password='{txtPassword.Text}'", connection.Connect());
                connection.Open();
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    emp = new EmployerClass
                    {

                        FirstName = name[0],
                        LastName = name[1],
                        Role = dataReader[1].ToString()
                    };
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\nFolositi butonul din stanga-sus pentru a alege calea catre baza de date!", title,
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {

                if (dataReader.HasRows)
                {

                    switch (emp.Role)
                    {
                        case "Cashier":
                            this.Hide();
                            mainForm = new MainForm();
                            mainForm.btnEmployer.Enabled = false;
                            mainForm.btnSetting.Enabled = false;
                            mainForm.btnService.Enabled = false;
                            mainForm.btnReport.Enabled = false;
                            mainForm.lblUser.Text = $"Welcome {emp.FirstName} {emp.LastName}";
                            mainForm.ShowDialog();
                            break;

                        case "Manager":
                            this.Hide();
                            mainForm = new MainForm();
                            mainForm.btnEmployer.Enabled = true;
                            mainForm.btnSetting.Enabled = true;
                            mainForm.btnService.Enabled = true;
                            mainForm.btnReport.Enabled = true;
                            mainForm.lblUser.Text = $"Welcome {emp.FirstName} {emp.LastName}";
                            mainForm.ShowDialog();
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    MessageBox.Show($"Invalid username or password ", "ACCESS DENIED", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                dataReader.Close();
                connection.Close();






            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, title);
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            PerformLoginAction();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtLogin.Clear();
            txtPassword.Clear();
            txtLogin.Focus();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (chkpass.Checked)
            {
                txtPassword.UseSystemPasswordChar = false;
            }
            else
            {
                txtPassword.UseSystemPasswordChar = true;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            DBSelectForm dBSelectForm = new DBSelectForm();
            dBSelectForm.ShowDialog();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            int cornerRadius = 10;
            Region = roundControls.CreateRoundedRegion(ClientRectangle, cornerRadius);
            panel1.Region = roundControls.CreateRoundedRegion(panel1.ClientRectangle, cornerRadius);
            btnLogin.Region = roundControls.CreateRoundedRegion(btnLogin.ClientRectangle, cornerRadius);
            btnCancel.Region = roundControls.CreateRoundedRegion(btnCancel.ClientRectangle, cornerRadius);
        }

        private void Login_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                PerformLoginAction();
            }
        }
    }
}
