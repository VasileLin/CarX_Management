using CarWashManagementSystem;
using CarX.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace CarX.Forms
{
    public partial class ManageVehicleType : Form
    {
        SqlCommand command = new SqlCommand();
        DbConnection dbConnection = new DbConnection();
        string title = "CarX Management System";
        Setting setting;
        public ManageVehicleType(Setting stg)
        {
            InitializeComponent();
            this.setting = stg;
            cbClass.SelectedIndex = 0;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtName.Text=="")
                {
                    MessageBox.Show("Required vehicle type name!","Warning!");
                    return;
                }

                    if (MessageBox.Show("Are you sure you want to register this vehicle type?", "Vehicle Type Registration", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        command = new SqlCommand("INSERT INTO VehicleType(name,class) VALUES(@name,@class)", dbConnection.Connect());
                        command.Parameters.AddWithValue("@name", txtName.Text);
                        command.Parameters.AddWithValue("@class", cbClass.Text);
                        dbConnection.Open();
                        command.ExecuteNonQuery();
                        dbConnection.Close();
                        MessageBox.Show("Vehicle type has been successfully registered!", title);
                        setting.LoadVehicleTypes();
                        Clear();
                        ClearFields();
                    }
                
            }
            catch (Exception ex)
            {

                MessageBox.Show("Add data error\n" + ex.Message, title);
            }
        }

        private void Clear()
        {
            cbClass.SelectedIndex = 0;
            btnSave.Visible = true;
            btnUpdate.Visible = false;
            ClearFields();
        }

        private void ClearFields()
        {
            txtName.Text = "";
            cbClass.SelectedIndex = 0;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure you want to edit this vehicle type?", "Vehicle Type Editing", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        command = new SqlCommand("UPDATE VehicleType SET name=@name,class=@class WHERE id=@id", dbConnection.Connect());
                        command.Parameters.AddWithValue("@id", lblVid.Text);
                        command.Parameters.AddWithValue("@name", txtName.Text);
                        command.Parameters.AddWithValue("@class", cbClass.Text);

                        dbConnection.Open();
                        command.ExecuteNonQuery();
                        dbConnection.Close();
                        MessageBox.Show("Vehicle type has been successfully edited!", title);
                        Clear();
                        this.Dispose();
                        setting.LoadVehicleTypes();
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

        private void ManageVehicleType_Load(object sender, EventArgs e)
        {
            RoundControls roundControls = new RoundControls();
            int radius = 10;
            btnUpdate.Region = roundControls.CreateRoundedRegion(btnUpdate.ClientRectangle, radius);
            btnSave.Region = roundControls.CreateRoundedRegion(btnSave.ClientRectangle, radius);
            btnCancel.Region = roundControls.CreateRoundedRegion(btnCancel.ClientRectangle, radius);
        }
    }
}
