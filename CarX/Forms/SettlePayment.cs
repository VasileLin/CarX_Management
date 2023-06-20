using CarWashManagementSystem;
using CarX.Forms.Modules;
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
    public partial class SettlePayment : Form
    {
        SqlCommand command = new SqlCommand();
        DbConnection connection = new DbConnection();
        string title = "Carx Management System";
        Cash cash;
        public SettlePayment(Cash cashForm)
        {
            InitializeComponent();
            this.cash = cashForm;
            txtCash.Focus();
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btn7_Click(object sender, EventArgs e)
        {
            txtCash.Text += btn7.Text;
        }

        private void btn8_Click(object sender, EventArgs e)
        {
            txtCash.Text += btn8.Text;
        }

        private void btn9_Click(object sender, EventArgs e)
        {
            txtCash.Text += btn9.Text;
        }

        private void btnClean_Click(object sender, EventArgs e)
        {
            txtCash.Clear();
            txtCash.Focus();
        }

        private void btn00_Click(object sender, EventArgs e)
        {
            txtCash.Text += btn00.Text;
        }

        private void btn6_Click(object sender, EventArgs e)
        {
            txtCash.Text += btn6.Text;
        }

        private void btn5_Click(object sender, EventArgs e)
        {
            txtCash.Text += btn5.Text;
        }

        private void btn4_Click(object sender, EventArgs e)
        {
            txtCash.Text += btn4.Text;
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            txtCash.Text += btn1.Text;
        }

        private void btn2_Click(object sender, EventArgs e)
        {
            txtCash.Text += btn2.Text;
        }

        private void btn3_Click(object sender, EventArgs e)
        {
            txtCash.Text += btn3.Text;
        }

        private void btnEnter_Click(object sender, EventArgs e)
        {
            try
            {
                if ((double.Parse(txtChange.Text)<0) || (txtCash.Text.Equals("")))
                {
                    MessageBox.Show("Insuficient amount, Please enther the corect amount","Warning",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    for (int i = 0; i < cash.dgvCash.Rows.Count ; i++)
                    {
                        connection.ExecuteQuery("UPDATE Cash SET status='Sold',price='" + cash.dgvCash.Rows[i].Cells[9].Value.ToString() +"' WHERE id = '"+ cash.dgvCash.Rows[i].Cells[1].Value.ToString() + "'");
                        connection.ExecuteQuery($"UPDATE tbCustomer SET points += {1} WHERE id='{cash.customerId}'");
                    }

                    Receipt receiptModule = new Receipt(cash);
                    receiptModule.LoadReceipt(txtCash.Text,txtChange.Text);
                    receiptModule.ShowDialog();

                    

                    MessageBox.Show("Payment successfully saved","Payment",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    cash.LoadCash();
                    this.Dispose();
                    cash.btnAddCustomer.Enabled = true;
                    cash.btnAddService.Enabled = false;
                    cash.GetTransNo();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, title);
            }
        }

        private void btnPoint_Click(object sender, EventArgs e)
        {
            txtCash.Text += btnPoint.Text;
        }

        private void btn0_Click(object sender, EventArgs e)
        {
            txtCash.Text += btn0.Text;
        }

        private void txtCash_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double charge = double.Parse(txtCash.Text) - double.Parse(txtSale.Text);
                txtChange.Text = charge.ToString("#,##0.00");
            }
            catch (Exception)
            {
                txtChange.Text = "0.00";
            }
        }

        private void btnEnter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                btnEnter.PerformClick();
            }
            else if (e.KeyCode==Keys.Escape)
            {
                this.Dispose();
            }
        }
    }
}
