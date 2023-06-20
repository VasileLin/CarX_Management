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
    public partial class CashCustomer : Form
    {
        SqlCommand command = new SqlCommand();
        DbConnection connection = new DbConnection();
        string title = "Carx Management System";
        SqlDataReader dataReader;
        Cash cash;
        public CashCustomer(Cash cashForm)
        {
            InitializeComponent();
            LoadCustomers();
            this.cash = cashForm;   
        }

        private void textSearch_TextChanged(object sender, EventArgs e)
        {
            LoadCustomers();
        }


        public void LoadCustomers()
        {
            try
            {
                int i = 0;
                dgvCustomer.Rows.Clear();
                command = new SqlCommand("SELECT * FROM tbCustomer WHERE CONCAT (name,phone,address) LIKE '%" + textSearch.Text + "%'", connection.Connect());
                connection.Open();
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    i++;
                    dgvCustomer.Rows.Add(i, dataReader[0].ToString(), dataReader[1].ToString(), dataReader[2].ToString(), dataReader[3].ToString(), dataReader[4].ToString(), dataReader[5].ToString(), dataReader[6].ToString(), dataReader[7].ToString());
                }
                connection.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, title);
            }
        }

        private void dgvCustomer_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvCustomer.Columns[e.ColumnIndex].Name;

            if (colName == "Select")
            {
                cash.customerId = int.Parse(dgvCustomer.Rows[e.RowIndex].Cells[1].Value.ToString());
                cash.vehicleTypeId = int.Parse(dgvCustomer.Rows[e.RowIndex].Cells[2].Value.ToString());
            }
            else
            {
                return;
            }

            this.Dispose();
            cash.panelCash.Height = 1;
        }
    }
}
