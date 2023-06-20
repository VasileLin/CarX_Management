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
    public partial class ManageCoGSold : Form
    {
        SqlCommand command = new SqlCommand();
        DbConnection dbConnection = new DbConnection();
        string title = "CarX Management System";
        bool check = false;
        Setting setting;
        public ManageCoGSold(Setting stg)
        {
            InitializeComponent();
            this.setting = stg;
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
                    if (MessageBox.Show("Are you sure you want to register this cost of sold?", "Cost of Good Sold Registration", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        command = new SqlCommand("INSERT INTO CostofGood(costname,cost,date) VALUES(@costname,@cost,@date)", dbConnection.Connect());
                        command.Parameters.AddWithValue("@costname", txtCostName.Text);
                        command.Parameters.AddWithValue("@cost", txtCost.Text);
                        command.Parameters.AddWithValue("@date", dtCog.Value);
                        dbConnection.Open();
                        command.ExecuteNonQuery();
                        dbConnection.Close();
                        MessageBox.Show("Cost of good sold has been successfully registered!", title);
                        setting.LoadCostofGood();
                        Clear();
                    }
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
                CheckFields();
                if (check)
                {
                    if (MessageBox.Show("Are you sure you want to edit this cost of sold?", "Cost of Good Sold Editing", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        command = new SqlCommand("UPDATE CostofGood SET costname=@costname, cost=@cost,date=@date WHERE id=@id", dbConnection.Connect());
                        command.Parameters.AddWithValue("@id", lblCid.Text);
                        command.Parameters.AddWithValue("@costname", txtCostName.Text);
                        command.Parameters.AddWithValue("@cost", txtCost.Text);
                        command.Parameters.AddWithValue("@date", dtCog.Value);
                        dbConnection.Open();
                        command.ExecuteNonQuery();
                        dbConnection.Close();
                        MessageBox.Show("Cost of good sold has been successfully edited!", title);
                        Clear();
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

        public void CheckFields()
        {
            if (txtCostName.Text == ""|| txtCost.Text=="")
            {
                MessageBox.Show("Required data fields are not completed!", "Warning");
                return;
            }
            check = true;
        }

        private void Clear()
        {
            txtCost.Clear();
            txtCostName.Clear();
            dtCog.Value = DateTime.Now;
            btnSave.Visible = true;
            btnUpdate.Visible = false;
        }

        private void txtCost_KeyPress(object sender, KeyPressEventArgs e)
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

        private void ManageCoGSold_Load(object sender, EventArgs e)
        {
            RoundControls roundControls = new RoundControls();
            int radius = 10;
            btnUpdate.Region = roundControls.CreateRoundedRegion(btnUpdate.ClientRectangle, radius);
            btnSave.Region = roundControls.CreateRoundedRegion(btnSave.ClientRectangle, radius);
            btnCancel.Region = roundControls.CreateRoundedRegion(btnCancel.ClientRectangle, radius);
        }
    }
}
