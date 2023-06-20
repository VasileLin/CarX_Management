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
using CarWashManagementSystem.Forms;
using CarX.Classes;
using System.IO;

namespace CarWashManagementSystem
{
    public partial class Employer : Form
    {
        SqlCommand command = new SqlCommand();
        DbConnection connection = new DbConnection();
        string title = "Carx Management System";
        SqlDataReader dataReader ;
        ExportData exportData = new ExportData();
        public Employer()
        {
            InitializeComponent();
            LoadEmployers();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            EmployerModule employerModule = new EmployerModule(this);
            employerModule.btnSave.Visible = true;
            employerModule.btnUpdate.Visible = false;
            employerModule.ShowDialog();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadEmployers();
        }

        private void dgvEmployer_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvEmployer.Columns[e.ColumnIndex].Name;

            if (colName=="Edit")
            {
                EmployerModule module = new EmployerModule(this);
                module.lblEid.Text = dgvEmployer.Rows[e.RowIndex].Cells[1].Value.ToString();
                module.txtName.Text = dgvEmployer.Rows[e.RowIndex].Cells[2].Value.ToString();
                module.txtPhone.Text = dgvEmployer.Rows[e.RowIndex].Cells[3].Value.ToString();
                module.txtAddress.Text = dgvEmployer.Rows[e.RowIndex].Cells[4].Value.ToString();
                module.dtDob.Text = dgvEmployer.Rows[e.RowIndex].Cells[5].Value.ToString();
                module.rdMale.Checked = dgvEmployer.Rows[e.RowIndex].Cells[6].Value.ToString() == "Male" ? true : false;
                module.cbRole.Text = dgvEmployer.Rows[e.RowIndex].Cells[7].Value.ToString();
                module.txtSalary.Text = dgvEmployer.Rows[e.RowIndex].Cells[8].Value.ToString();
                module.txtPassword.Text = dgvEmployer.Rows[e.RowIndex].Cells[9].Value.ToString();
                module.btnSave.Visible = false;
                module.ShowDialog();

            }
            else if(colName=="Delete")
            {
                try
                {
                    if (MessageBox.Show("Are you sure you want to delete this record?","Delete Record?",MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes)
                    {
                        command = new SqlCommand($"DELETE FROM Employer WHERE id = {dgvEmployer.Rows[e.RowIndex].Cells[1].Value}",connection.Connect());
                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                        MessageBox.Show("Employer data has been successfully removed!",title,MessageBoxButtons.OK,MessageBoxIcon.Information);
                        LoadEmployers();
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message, title);
                }
            }
        }

        public void LoadEmployers()
        {
            try
            {
                int i = 0;
                dgvEmployer.Rows.Clear();
                command = new SqlCommand("SELECT * FROM Employer WHERE CONCAT (name,address,role) LIKE '%"+txtSearch.Text+"%'",connection.Connect());
                connection.Open();
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    i++;
                    dgvEmployer.Rows.Add(i, dataReader[0].ToString(), dataReader[1].ToString(), dataReader[2].ToString(), dataReader[3].ToString(), DateTime.Parse(dataReader[4].ToString()).ToShortDateString(), dataReader[5].ToString(), dataReader[6].ToString(), dataReader[7].ToString(), dataReader[8].ToString());
                }
                connection.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message,title);
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            string filePath = Path.Combine(exportData.desktopPath, "RaportEmployers.xlsx");
            exportData.ExportToExcel(dgvEmployer, filePath);
        }
    }
}
