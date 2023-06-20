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
    public partial class Service : Form
    {
        SqlCommand command = new SqlCommand();
        DbConnection connection = new DbConnection();
        string title = "Carx Management System";
        SqlDataReader dataReader;
        public Service()
        {
            InitializeComponent();
            LoadServices();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadServices();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ServiceModule serviceModule = new ServiceModule(this);
            serviceModule.btnUpdate.Enabled = true;
            serviceModule.ShowDialog();
        }

        private void dgvService_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvService.Columns[e.ColumnIndex].Name;

            if (colName == "Edit")
            {
                ServiceModule module = new ServiceModule(this);
                module.lblSid.Text = dgvService.Rows[e.RowIndex].Cells[1].Value.ToString();
                module.txtName.Text = dgvService.Rows[e.RowIndex].Cells[2].Value.ToString();
                module.txtPrice.Text = dgvService.Rows[e.RowIndex].Cells[3].Value.ToString();
                module.btnSave.Visible = false;
                module.ShowDialog();

            }
            else if (colName == "Delete")
            {
                try
                {
                    if (MessageBox.Show("Are you sure you want to delete this record?", "Delete Record?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        command = new SqlCommand($"DELETE FROM Service WHERE id = {dgvService.Rows[e.RowIndex].Cells[1].Value}", connection.Connect());
                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                        MessageBox.Show("Service has been successfully removed!", title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message, title);
                }
            }
            LoadServices();
        }

        public void LoadServices()
        {
            try
            {
                int i = 0;
                dgvService.Rows.Clear();
                command = new SqlCommand("SELECT * FROM Service WHERE name LIKE '%" + txtSearch.Text + "%'", connection.Connect());
                connection.Open();
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    i++;
                    dgvService.Rows.Add(i, dataReader[0].ToString(), dataReader[1].ToString(), dataReader[2].ToString());
                }
                connection.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, title);
            }
        }
    }
}
