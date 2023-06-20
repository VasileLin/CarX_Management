using CarWashManagementSystem.Forms;
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
    public partial class Setting : Form
    {
        SqlCommand command = new SqlCommand();
        DbConnection connection = new DbConnection();
        string title = "Carx Management System";
        bool hasdetail = false;
        SqlDataReader dataReader;
        public Setting()
        {
            InitializeComponent();
            LoadVehicleTypes();
            LoadCostofGood();
            LoadCompany();
        }


        public void LoadVehicleTypes()
        {
            try
            {
                int i = 0;
                dgvVehicleType.Rows.Clear();
                command = new SqlCommand("SELECT * FROM VehicleType WHERE CONCAT (name,class) LIKE '%" + txtSearchVT.Text + "%'", connection.Connect());
                connection.Open();
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    i++;
                    dgvVehicleType.Rows.Add(i, dataReader[0].ToString(), dataReader[1].ToString(), dataReader[2].ToString());
                }
                connection.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, title);
            }
        }

        private void txtSearchVT_TextChanged(object sender, EventArgs e)
        {
            LoadVehicleTypes();
        }

        private void btnAddVT_Click(object sender, EventArgs e)
        {
            ManageVehicleType module = new ManageVehicleType(this);
            module.btnUpdate.Visible = false;
            module.ShowDialog();
        }

        private void dgvVehicleType_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvVehicleType.Columns[e.ColumnIndex].Name;

            if (colName == "Edit")
            {
                ManageVehicleType module = new ManageVehicleType(this);
                module.lblVid.Text = dgvVehicleType.Rows[e.RowIndex].Cells[1].Value.ToString();
                module.txtName.Text = dgvVehicleType.Rows[e.RowIndex].Cells[2].Value.ToString();
                module.cbClass.Text = dgvVehicleType.Rows[e.RowIndex].Cells[3].Value.ToString();
                module.btnSave.Visible = false;
                module.ShowDialog();

            }
            else if (colName == "Delete")
            {
                try
                {
                    if (MessageBox.Show("Are you sure you want to delete this record?", "Delete Record?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        command = new SqlCommand($"DELETE FROM VehicleType WHERE id = {dgvVehicleType.Rows[e.RowIndex].Cells[1].Value}", connection.Connect());
                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                        MessageBox.Show("Vehicle type data has been successfully removed!", title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadVehicleTypes();
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message, title);
                }
            }
        }



        #region CostofGoodSold
        private void btnAddCoG_Click(object sender, EventArgs e)
        {
            ManageCoGSold coGModule = new ManageCoGSold(this);
            coGModule.btnUpdate.Visible = false;
            coGModule.ShowDialog();
        }   

        private void searchCog_TextChanged(object sender, EventArgs e)
        {
            LoadCostofGood();
        }

        private void dgvCoGSold_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvCoGSold.Columns[e.ColumnIndex].Name;
            if (colName == "EditCog")
            {
                ManageCoGSold module = new ManageCoGSold(this);
                module.lblCid.Text = dgvCoGSold.Rows[e.RowIndex].Cells[1].Value.ToString();
                module.txtCostName.Text = dgvCoGSold.Rows[e.RowIndex].Cells[2].Value.ToString();
                module.txtCost.Text = dgvCoGSold.Rows[e.RowIndex].Cells[3].Value.ToString();
                module.dtCog.Text = dgvCoGSold.Rows[e.RowIndex].Cells[4].Value.ToString();
                module.btnSave.Visible = false;
                module.ShowDialog();

            }
            else if (colName == "DeleteCog")
            {
                try
                {
                    if (MessageBox.Show("Are you sure you want to delete this record?", "Delete Record?",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        command = new SqlCommand($"DELETE FROM CostofGood WHERE id = {dgvCoGSold.Rows[e.RowIndex].Cells[1].Value}", connection.Connect());
                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                        MessageBox.Show("Cost of good sold data has been successfully removed!", title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message, title);
                }
            }
            LoadCostofGood();
        }


        public void LoadCostofGood()
        {
            try
            {
                int i = 0;
                dgvCoGSold.Rows.Clear();
                command = new SqlCommand("SELECT * FROM CostofGood WHERE CONCAT (costname,date) LIKE '%" + searchCog.Text + "%'", connection.Connect());
                connection.Open();
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    i++;
                    dgvCoGSold.Rows.Add(i, dataReader[0].ToString(), dataReader[1].ToString(),dataReader[2].ToString(),
                        DateTime.Parse(dataReader[3].ToString()).ToShortDateString());
                }
                connection.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, title);
            }
        }
        #endregion CostofGoodSold


        #region Company Detail
        public void LoadCompany()
        {
            try
            {
                connection.Open();
                command = new SqlCommand("SELECT * FROM Company", connection.Connect());
                dataReader = command.ExecuteReader();
                dataReader.Read();

                if (dataReader.HasRows)
                {
                    hasdetail = true;
                    txtComName.Text = dataReader["name"].ToString();
                    txtComAddress.Text = dataReader["address"].ToString();
                    txtIBAN.Text = dataReader["iban"].ToString();
                }
                else
                {
                    txtComName.Clear();
                    txtComAddress.Clear();
                }
                dataReader.Close();
                connection.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message,title);
            }

            
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Save company detail?","Confirm",MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes)
                {
                    if (hasdetail)
                    {
                        connection.ExecuteQuery("UPDATE Company SET name = '"+txtComName.Text+"',address='"+txtComAddress.Text+ "'," +
                            "iban='"+txtIBAN.Text+"'");
                    }
                    else
                    {
                        connection.ExecuteQuery("INSERT INTO Company (name,address,iban) VALUES ('"+txtComName.Text+"','"+txtComAddress.Text+ "','"+txtIBAN.Text+"')");
                    }
                    MessageBox.Show("Company details has been successfully saved!","Save Record",MessageBoxButtons.OK,MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, title);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtComName.Clear();
            txtComAddress.Clear();
            txtIBAN.Clear();
        }

        #endregion Company Detail


    }
}
