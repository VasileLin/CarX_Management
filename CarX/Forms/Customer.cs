using CarWashManagementSystem.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using CarX.Classes;

namespace CarX.Forms
{
    public partial class Customer : Form
    {
        SqlCommand command = new SqlCommand();
        static DbConnection connection = new DbConnection();
        ExportData exportData = new ExportData();
        string title = "Carx Management System";
        SqlDataReader dataReader;
        public Customer()
        {
            InitializeComponent();
            LoadCustomers();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            CustomerModule customerModule = new CustomerModule(this);
            customerModule.btnUpdate.Visible = false;
            customerModule.ShowDialog();
        }

        private void dgvCustomer_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvCustomer.Columns[e.ColumnIndex].Name;

            if (colName == "Edit")
            {
                CustomerModule module = new CustomerModule(this);
                module.lblCid.Text = dgvCustomer.Rows[e.RowIndex].Cells[1].Value.ToString();
                module.txtName.Text = dgvCustomer.Rows[e.RowIndex].Cells[2].Value.ToString();
                module.txtPhone.Text = dgvCustomer.Rows[e.RowIndex].Cells[3].Value.ToString();
                module.txtCarNo.Text = dgvCustomer.Rows[e.RowIndex].Cells[4].Value.ToString();
                module.txtCarModel.Text = dgvCustomer.Rows[e.RowIndex].Cells[5].Value.ToString();
                module.vid = VehicleIdByName(dgvCustomer.Rows[e.RowIndex].Cells[6].Value.ToString());
                module.txtAddress.Text = dgvCustomer.Rows[e.RowIndex].Cells[7].Value.ToString();
                module.udPoints.Text = dgvCustomer.Rows[e.RowIndex].Cells[8].Value.ToString();
                module.btnSave.Visible = false;
                module.udPoints.Enabled = true;
                module.ShowDialog();

            }
            else if (colName == "Delete")
            {
                try
                {
                    if (MessageBox.Show("Are you sure you want to delete this record?", "Delete Record?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        command = new SqlCommand($"DELETE FROM tbCustomer WHERE id = {dgvCustomer.Rows[e.RowIndex].Cells[1].Value}", connection.Connect());
                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                        MessageBox.Show("Customer data has been successfully removed!", title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message, title);
                }
            }

            LoadCustomers();
        }


        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadCustomers();
        }

        public void LoadCustomers()
        {
            try
            {
                int i = 0;
                dgvCustomer.Rows.Clear();
                command = new SqlCommand("SELECT C.id,C.name,phone,carno,carmodel,V.name,address,points FROM tbCustomer AS C INNER JOIN VehicleType AS V ON C.vehicleid=V.id WHERE CONCAT (C.name,carno,carmodel,address) LIKE '%" + txtSearch.Text + "%'", connection.Connect());
                connection.Open();
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    i++;
                    dgvCustomer.Rows.Add(i, dataReader[0].ToString(), dataReader[1].ToString(), dataReader[2].ToString(), dataReader[3].ToString(),dataReader[4].ToString(), dataReader[5].ToString(), dataReader[6].ToString(), dataReader[7].ToString());
                }
                connection.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, title);
            }
        }

        public int VehicleIdByName(string str)
        {
            int i = 0;
            command = new SqlCommand("SELECT id FROM VehicleType WHERE name LIKE '"+str+"'",connection.Connect());
            connection.Open();
            dataReader = command.ExecuteReader();
            dataReader.Read();
            if (dataReader.HasRows)
            {
                i = int.Parse(dataReader["id"].ToString());
            }
            connection.Close();
            return i;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {        
            string filePath = Path.Combine(exportData.desktopPath, "RaportCustomers.xlsx");
            exportData.ExportToExcel(dgvCustomer, filePath);
        }


        
    }
}
