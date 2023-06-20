using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using CarX;
using CarX.Classes;

namespace CarWashManagementSystem.Forms
{
    public partial class EmployerModule : Form
    {
        SqlCommand command = new SqlCommand();
        DbConnection dbConnection = new DbConnection();
        string title = "CarX Management System";
        bool check = false;
        Employer employer;
        public EmployerModule(Employer emp)
        {
            InitializeComponent();
            cbRole.SelectedIndex = 2;
            this.employer = emp;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                CheckFields();

                if (check)
                {
                    if (MessageBox.Show("Are you sure you want to register this employer?", "Employer Registration", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        command = new SqlCommand("INSERT INTO Employer(name,phone,address,dob,gender,role,salary,password) VALUES(@name,@phone,@address,@dob,@gender,@role,@salary,@password)", dbConnection.Connect());
                        command.Parameters.AddWithValue("@name", txtName.Text);
                        command.Parameters.AddWithValue("@phone", txtPhone.Text);
                        command.Parameters.AddWithValue("@address", txtAddress.Text);
                        command.Parameters.AddWithValue("@dob", dtDob.Value);
                        command.Parameters.AddWithValue("@gender", rdMale.Checked ? "Male" : "Female");
                        command.Parameters.AddWithValue("@role", cbRole.Text);
                        command.Parameters.AddWithValue("@salary", txtSalary.Text);
                        command.Parameters.AddWithValue("@password", txtPassword.Text);

                        dbConnection.Open();
                        command.ExecuteNonQuery();
                        dbConnection.Close();
                        MessageBox.Show("Employer has been successfully registered!", title);
                        check = false;
                        Clear();
                        employer.LoadEmployers();
                    }
                }

               
            }
            catch (Exception ex)
            {

                MessageBox.Show("Add data error\n"+ ex.Message, title);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                CheckFields();

                if (check)
                {
                    if (MessageBox.Show("Are you sure you want to edit this record?", "Employer Editing", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        command = new SqlCommand("UPDATE Employer SET name=@name,phone=@phone,address=@address,dob=@dob,gender=@gender,role=@role,salary=@salary,password=@password WHERE id=@id", dbConnection.Connect());
                        command.Parameters.AddWithValue("@id", lblEid.Text);
                        command.Parameters.AddWithValue("@name", txtName.Text);
                        command.Parameters.AddWithValue("@phone", txtPhone.Text);
                        command.Parameters.AddWithValue("@address", txtAddress.Text);
                        command.Parameters.AddWithValue("@dob", dtDob.Value);
                        command.Parameters.AddWithValue("@gender", rdMale.Checked ? "Male" : "Female");
                        command.Parameters.AddWithValue("@role", cbRole.Text);
                        command.Parameters.AddWithValue("@salary", txtSalary.Text);
                        command.Parameters.AddWithValue("@password", txtPassword.Text);

                        dbConnection.Open();
                        command.ExecuteNonQuery();
                        dbConnection.Close();
                        MessageBox.Show("Record has been successfully edited!", title);
                        Clear();
                        this.Dispose();
                        employer.LoadEmployers();
                    }
                }


            }
            catch (Exception ex)
            {

                MessageBox.Show("Add data error\n" + ex.Message, title);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
            btnUpdate.Visible = false;
            btnSave.Visible = true;
        }

        public void Clear()
        {
            txtAddress.Clear();
            txtName.Clear();
            txtPassword.Clear();
            txtPhone.Clear();
            txtSalary.Clear();

            dtDob.Value = DateTime.Now;
            cbRole.SelectedIndex = 2;
            txtName.Focus();
        }

        public void CheckFields()
        {
            //Verificarea tuturor campurilor
            if (txtAddress.Text == "" || txtName.Text == ""|| txtPhone.Text == "" || txtSalary.Text=="")
            {
                MessageBox.Show("Complete all required fields", "Warning!");
                return;
            }

            if (CheckAge(dtDob.Value)<18)
            {
                MessageBox.Show("Employer is under 18!", "Warning!");
                return;
            }

            if (txtPassword.TextLength<8 && cbRole.Text!="Worker")
            {
                MessageBox.Show("Password must be at least 8 characters!", "Warning!");
                return;
            }
            check = true;
        }

        private static int CheckAge(DateTime dateOfBirth)
        {
            int age = DateTime.Now.Year - dateOfBirth.Year;
            if (DateTime.Now.DayOfYear<dateOfBirth.DayOfYear)
            {
                age = age - 1;
            }
            return age;
        }

        private void txtSalary_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar !='.')
            {
                e.Handled = true;
            }

            if ((e.KeyChar =='.') && (sender as TextBox).Text.IndexOf('.')>-1)
            {
                e.Handled = true;
            }
        }

        private void cbRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbRole.Text=="Supervisor" || cbRole.Text=="Worker")
            {
                txtPassword.Clear();
                lblPass.Visible = false;
                txtPassword.Visible = false;
            }
            else
            {
                lblPass.Visible = true;
                txtPassword.Visible = true;
            }
        }

        private void EmployerModule_Load(object sender, EventArgs e)
        {
            RoundControls roundControls = new RoundControls();
            int radius = 10;
            btnUpdate.Region = roundControls.CreateRoundedRegion(btnUpdate.ClientRectangle, radius);
            btnSave.Region = roundControls.CreateRoundedRegion(btnSave.ClientRectangle, radius);
            btnCancel.Region = roundControls.CreateRoundedRegion(btnCancel.ClientRectangle, radius);
        }

        private void chbxShow_CheckedChanged(object sender, EventArgs e)
        {
            if (txtPassword.UseSystemPasswordChar==true)
            {
                txtPassword.UseSystemPasswordChar = false;
            }
            else
            {
                txtPassword.UseSystemPasswordChar = true;
            }
        }
    }
}
