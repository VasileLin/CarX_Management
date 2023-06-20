using CarWashManagementSystem;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarX.Forms
{
    public partial class Cash : Form
    {
        SqlCommand command = new SqlCommand();
        DbConnection connection = new DbConnection();
        string title = "Carx Management System";
        SqlDataReader dataReader;
        public int customerId = 0, vehicleTypeId = 0;
        public string carno, carmodel;
        MainForm main;

        public Cash(MainForm mainForm)
        {
            InitializeComponent();
            GetTransNo();
            LoadCash();
            main = mainForm;
        }

      
        private void btnAddCustomer_Click(object sender, EventArgs e)
        {
            OpenChildForm(new CashCustomer(this));
            btnAddService.Enabled = true;
        }

        private void btnAddService_Click(object sender, EventArgs e)
        {
            OpenChildForm(new CashService(this));
            btnAddCustomer.Enabled = false;
        }

        private void btnCash_Click(object sender, EventArgs e)
        {
            SettlePayment settlePayment = new SettlePayment(this);
            settlePayment.txtSale.Text = lblTotal.Text;
            settlePayment.ShowDialog();
            main.LoadGrossProfit();
        }


        private Form activeForm = null;
        public void OpenChildForm(Form childForm)
        {
            if (activeForm != null)
                activeForm.Close();
            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panelCash.Height = 200;
            panelCash.Controls.Add(childForm);
            panelCash.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        //Transaction generator function
        public void GetTransNo()
        {
            try
            {
                string sdate = DateTime.Now.ToString("yyyyMMdd");
                int count;
                string transno;

                connection.Open();
                command = new SqlCommand("SELECT TOP 1 transno FROM Cash WHERE transno LIKE '"+sdate+"%' ORDER BY id DESC", connection.Connect());
                dataReader = command.ExecuteReader();
                dataReader.Read();

                if (dataReader.HasRows)
                {
                    transno = dataReader[0].ToString();
                    count = int.Parse(transno.Substring(8, 4));
                    lblTransno.Text = sdate + (count + 1);
                }
                else
                {
                    transno = sdate + "1001";
                    lblTransno.Text = transno;
                }
                connection.Close();
                dataReader.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message,title);
            }
        }

        private void dgvCash_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvCash.Columns[e.ColumnIndex].Name;
            if (colName == "Delete")
            {
                try
                {
                    if (MessageBox.Show("Are you sure you want to cancel this service?", "Cancel Service?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        command = new SqlCommand($"DELETE FROM Cash WHERE id = {dgvCash.Rows[e.RowIndex].Cells[1].Value}", connection.Connect());
                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                        MessageBox.Show("Service has been successfully canceled!", title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                       
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message, title);
                }
            }
            LoadCash();
        }

        private void Cash_Load(object sender, EventArgs e)
        {

        }

        public void LoadCash()
        {
            int i = 0;
            double total = 0;
            double price = 0;
            dgvCash.Rows.Clear();
            command = new SqlCommand("SELECT ca.id,ca.transno,Cu.name,Cu.carno,Cu.carmodel,v.name,v.class,s.name,Ca.price,Ca.date FROM Cash AS Ca LEFT JOIN tbCustomer AS Cu ON Ca.cid=Cu.id LEFT JOIN Service AS s ON Ca.sid=s.id LEFT JOIN VehicleType AS v ON Ca.vid=v.id WHERE status LIKE 'Pending' AND Ca.transno='"+lblTransno.Text+"'", connection.Connect());
            connection.Open();
            dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                i++;
                price = int.Parse(dataReader[6].ToString())*double.Parse(dataReader[8].ToString());
                dgvCash.Rows.Add(i, dataReader[0].ToString(), dataReader[1].ToString(), dataReader[2].ToString(), dataReader[3].ToString(), dataReader[4].ToString(), dataReader[5].ToString(), dataReader[6].ToString(), dataReader[7].ToString(), price, dataReader[9].ToString());
                total += price;
                carno = dataReader[3].ToString();
                carmodel = dataReader[4].ToString();
            }
            dataReader.Close();
            connection.Close();
            lblTotal.Text = total.ToString("#,##0.00");
        }

    }
}
