using CarX.Classes;
using CarX.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarX.Forms
{
    public partial class ServiceModule : Form
    {
        SqlCommand command = new SqlCommand();
        DbConnection dbConnection = new DbConnection();
        string title = "CarX Management System";
        Service service;
        public ServiceModule(Service serv)
        {
            InitializeComponent();
            service = serv;
        }

        private void txtPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            if ((e.KeyChar == '.') && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtName.Text == "" || txtPrice.Text == "")
                {
                    MessageBox.Show("Required data!", "Warning!");
                    return;
                }

                if (MessageBox.Show("Are you sure you want to register this service?", "Service Registration", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    command = new SqlCommand("INSERT INTO Service(name,price) VALUES(@name,@price)", dbConnection.Connect());
                    command.Parameters.AddWithValue("@name", txtName.Text);
                    command.Parameters.AddWithValue("@price", txtPrice.Text);
                    dbConnection.Open();
                    command.ExecuteNonQuery();
                    dbConnection.Close();
                    MessageBox.Show("Service has been successfully registered!", title);
                    Clear();
                    service.LoadServices();

                }

            }
            catch (Exception ex)
            {

                MessageBox.Show("Add data error\n" + ex.Message, title);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtName.Text == "" || txtPrice.Text == "")
                {
                    MessageBox.Show("Required data!", "Warning!");
                    return;
                }

                if (MessageBox.Show("Are you sure you want to edit this service?", "Service Editing", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    command = new SqlCommand("UPDATE Service SET name=@name,price=@price WHERE id=@id", dbConnection.Connect());
                    command.Parameters.AddWithValue("@id", lblSid.Text);
                    command.Parameters.AddWithValue("@name", txtName.Text);
                    command.Parameters.AddWithValue("@price", txtPrice.Text);
                    dbConnection.Open();
                    command.ExecuteNonQuery();
                    dbConnection.Close();
                    MessageBox.Show("Service has been successfully edited!", title);
                    service.LoadServices();
                    this.Dispose();
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
        }

        private void Clear()
        {
            txtName.Clear();
            txtPrice.Clear();

            btnSave.Visible = true;
            btnUpdate.Visible = false;
        }

        private void ServiceModule_Load(object sender, EventArgs e)
        {
            RoundControls roundControls = new RoundControls();
            int radius = 10;
            btnUpdate.Region = roundControls.CreateRoundedRegion(btnUpdate.ClientRectangle, radius);
            btnSave.Region = roundControls.CreateRoundedRegion(btnSave.ClientRectangle, radius);
            btnCancel.Region = roundControls.CreateRoundedRegion(btnCancel.ClientRectangle, radius);

        }
    }
}
