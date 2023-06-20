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

namespace CarX.Forms
{
    public partial class CustomerModule : Form
    {
        SqlCommand command = new SqlCommand();
        DbConnection dbConnection = new DbConnection();
        string title = "CarX Management System";
        public int vid = 0;
        bool check = false;
        Customer customer;
        public CustomerModule(Customer cust)
        {
            InitializeComponent();
            customer = cust;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                CheckFields();

                if (check)
                {
                    if (MessageBox.Show("Are you sure you want to register this Customer?", 
                        "Customer Registration", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        command = new SqlCommand("INSERT INTO tbCustomer(vehicleid,name,phone,carno,carmodel,address,points) VALUES(@vehicleid,@name,@phone,@carno,@carmodel,@address,@points)", dbConnection.Connect());
                        command.Parameters.AddWithValue("@vehicleid", cbCarType.SelectedValue);
                        command.Parameters.AddWithValue("@name", txtName.Text);
                        command.Parameters.AddWithValue("@phone", txtPhone.Text);
                        command.Parameters.AddWithValue("@carno", txtCarNo.Text);
                        command.Parameters.AddWithValue("@carmodel", txtCarModel.Text);
                        command.Parameters.AddWithValue("@address", txtAddress.Text);
                        command.Parameters.AddWithValue("@points", udPoints.Text);

                        dbConnection.Open();
                        command.ExecuteNonQuery();
                        dbConnection.Close();
                        MessageBox.Show("Customer has been successfully registered!", title);
                        check = false;
                        Clear();
                    }
                }
                customer.LoadCustomers();

            }
            catch (Exception ex)
            {

                MessageBox.Show("Add data error\n" + ex.Message, title);
            }
        }

        private void Clear()
        {
            txtAddress.Clear();
            txtCarModel.Clear();
            txtCarNo.Clear();
            txtName.Clear();
            txtPhone.Clear();
            cbCarType.SelectedIndex = 0;
            udPoints.Value = 0;
            btnSave.Visible = true;
            btnUpdate.Visible = false;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                CheckFields();

                if (check)
                {
                    if (MessageBox.Show("Are you sure you want to edit this Customer?", "Customer Editing", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        command = new SqlCommand("UPDATE tbCustomer SET vehicleid=@vehicleid,name=@name,phone=@phone,carno=@carno,carmodel=@carmodel,address=@address,points=@points WHERE id=@id", dbConnection.Connect());
                        command.Parameters.AddWithValue("@id", lblCid.Text);
                        command.Parameters.AddWithValue("@vehicleid", cbCarType.SelectedValue);
                        command.Parameters.AddWithValue("@name", txtName.Text);
                        command.Parameters.AddWithValue("@phone", txtPhone.Text);
                        command.Parameters.AddWithValue("@carno", txtCarNo.Text);
                        command.Parameters.AddWithValue("@carmodel", txtCarModel.Text);
                        command.Parameters.AddWithValue("@address", txtAddress.Text);
                        command.Parameters.AddWithValue("@points", udPoints.Text);

                        dbConnection.Open();
                        command.ExecuteNonQuery();
                        dbConnection.Close();
                        MessageBox.Show("Customer has been successfully edited!", title);
                        customer.LoadCustomers();
                        this.Dispose();
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
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        public DataTable VehicleType()
        {
            command = new SqlCommand("SELECT * FROM VehicleType",dbConnection.Connect());
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable dataTable = new DataTable();

            adapter.SelectCommand = command;
            adapter.Fill(dataTable);
            return dataTable;
        }

        private void CustomerModule_Load(object sender, EventArgs e)
        {
            cbCarType.DataSource = VehicleType();
            cbCarType.DisplayMember = "name";
            cbCarType.ValueMember = "id";

            if (vid>0)
            {
                cbCarType.SelectedValue = vid;
            }

            RoundControls roundControls = new RoundControls();
            int radius = 10;
            btnUpdate.Region = roundControls.CreateRoundedRegion(btnUpdate.ClientRectangle, radius);
            btnSave.Region = roundControls.CreateRoundedRegion(btnSave.ClientRectangle, radius);
            btnCancel.Region = roundControls.CreateRoundedRegion(btnCancel.ClientRectangle, radius);
        }

        public void CheckFields()
        {
            if (txtAddress.Text == "" || txtName.Text == "" || txtPhone.Text == "" || txtCarNo.Text == "" || txtCarModel.Text == "")
            {
                MessageBox.Show("Complete all required fields", "Warning!");
                return;
            }
            check = true;
        }
    }
}
