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
    public partial class CashService : Form
    {
        SqlCommand command = new SqlCommand();
        DbConnection connection = new DbConnection();
        string title = "Carx Management System";
        SqlDataReader dataReader;
        Cash cash;
        public CashService(Cash cashForm)
        {
            InitializeComponent();
            cash = cashForm;
            LoadServices();
        }


        private void textSearch_TextChanged(object sender, EventArgs e)
        {
            LoadServices();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dr in dgvService.Rows)
            {
                bool chkbox = Convert.ToBoolean(dr.Cells["Select"].Value);
                if (chkbox)
                {
                    try
                    {
                        command = new SqlCommand("IF NOT EXISTS (SELECT * FROM Cash WHERE sid=@sid AND transno = @transno)  INSERT INTO Cash(transno,cid,sid,vid,price,date) VALUES (@transno,@cid,@sid,@vid,@price,@date)",connection.Connect());
                        command.Parameters.AddWithValue("@transno",cash.lblTransno.Text);
                        command.Parameters.AddWithValue("@cid", cash.customerId);
                        command.Parameters.AddWithValue("@sid", dr.Cells[1].Value.ToString());
                        command.Parameters.AddWithValue("@vid",cash.vehicleTypeId);
                        command.Parameters.AddWithValue("@price", dr.Cells[3].Value.ToString());
                        command.Parameters.AddWithValue("@date", DateTime.Now);
                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                        cash.btnCash.Enabled = true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message,title);
                    }
                }
            }

            this.Dispose();
            cash.panelCash.Height = 1;
            cash.LoadCash();
        }

        private void dgvService_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }



        public void LoadServices()
        {
            try
            {
                int i = 0;
                dgvService.Rows.Clear();
                command = new SqlCommand("SELECT * FROM Service WHERE name LIKE '%" + textSearch.Text + "%'", connection.Connect());
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
