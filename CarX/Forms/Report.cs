using CarX.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarX.Forms
{
    public partial class Report : Form
    {
        SqlCommand command = new SqlCommand();
        DbConnection connection = new DbConnection();
        string title = "Carx Management System";
        SqlDataReader dataReader;
        ExportData exportData = new ExportData();
        public Report()
        {
            InitializeComponent();
            LoadTopSelling();
            LoadRevenus();
            LoadCostofGood();
            LoadGrossProfit();
        }

        private void dtFromTopSelling_ValueChanged(object sender, EventArgs e)
        {
            LoadTopSelling();
        }

        private void dtToTopSelling_ValueChanged(object sender, EventArgs e)
        {
            LoadTopSelling();
        }

        public void LoadTopSelling()
        {
            try
            {
                int i = 0;
                dgvTopSelling.Rows.Clear();
                command = new SqlCommand($"SELECT TOP 10 se.name,count(ca.sid) AS qty, ISNULL(SUM(ca.price),0) AS total FROM Cash AS ca JOIN Service AS se ON ca.sid=se.id WHERE ca.date BETWEEN '{dtFromTopSelling.Value}' AND '{dtToTopSelling.Value}' AND status LIKE 'Sold' GROUP BY se.name ORDER BY qty DESC", connection.Connect());
                connection.Open();
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    i++;
                    dgvTopSelling.Rows.Add(i, dataReader[0].ToString(), dataReader[1].ToString(), dataReader[2].ToString());
                }
                dataReader.Close();
                connection.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message,title);
            }
 
        }




        private void dtFromRevenus_ValueChanged(object sender, EventArgs e)
        {
            LoadRevenus();
        }

        private void dtToRevenus_ValueChanged(object sender, EventArgs e)
        {
            LoadRevenus();
        }

        public void LoadRevenus()
        {
            try
            {
                int i = 0;
                dgvRevenus.Rows.Clear();
                double total = 0;
                command = new SqlCommand($"SELECT date,ISNULL(SUM(price),0) AS total FROM Cash WHERE date BETWEEN '{dtFromRevenus.Value}' AND '{dtToRevenus.Value}' AND status LIKE 'Sold' GROUP BY date", connection.Connect());
                connection.Open();
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    i++;
                    dgvRevenus.Rows.Add(i, DateTime.Parse(dataReader[0].ToString()).ToShortDateString(), dataReader[1].ToString());
                    total += double.Parse(dataReader[1].ToString());
                }
                lblRevenus.Text = total.ToString("#,##0.00");
                dataReader.Close();
                connection.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, title);
            }
        }

        private void dtFormCoG_ValueChanged(object sender, EventArgs e)
        {
            LoadCostofGood();
        }

        private void dtToCoG_ValueChanged(object sender, EventArgs e)
        {
            LoadCostofGood();
        }

        public void LoadCostofGood()
        {
            try
            {
                int i = 0;
                dgvCostofGood.Rows.Clear();
                double total = 0;
                command = new SqlCommand($"SELECT costname,cost,date FROM CostofGood WHERE date" +
                    $" BETWEEN '{dtFormCoG.Value}' AND '{dtToCoG.Value}'", connection.Connect());
                connection.Open();
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    i++;
                    dgvCostofGood.Rows.Add(i, dataReader[0].ToString(), dataReader[1].ToString(),
                        DateTime.Parse(dataReader[2].ToString()).ToShortDateString());
                    total += double.Parse(dataReader[1].ToString());
                }
                lblCoG.Text = total.ToString("#,##0.00");
                dataReader.Close();
                connection.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, title);
            }
        }




        private void dtFromGP_ValueChanged(object sender, EventArgs e)
        {
            LoadGrossProfit();
        }

        private void dtToGP_ValueChanged(object sender, EventArgs e)
        {
            LoadGrossProfit();
        }

        public void LoadGrossProfit()
        {
            txtRevenus.Text = ExtractData($"SELECT ISNULL(SUM(price),0) AS total FROM Cash WHERE date BETWEEN '{dtFromGP.Value}' AND '{dtToGP.Value}'").ToString("0.00");
            txtCoG.Text = ExtractData($"SELECT ISNULL(SUM(cost),0) FROM CostofGood WHERE date BETWEEN '{dtFromGP.Value}' AND '{dtToGP.Value}'").ToString("0.00");
            txtGrossProfit.Text = (double.Parse(txtRevenus.Text)-double.Parse(txtCoG.Text)).ToString("0.00");

            if (double.Parse(txtGrossProfit.Text)<0)
            {
                txtGrossProfit.ForeColor = Color.Red;
            }
            else
            {
                txtGrossProfit.ForeColor = Color.Green;
            }
        }

        public double ExtractData(string sql)
        {
            connection.Open();
            command = new SqlCommand(sql,connection.Connect());
            double data = double.Parse(command.ExecuteScalar().ToString());
            connection.Close();
            return data;
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            string filePath = Path.Combine(exportData.desktopPath, "RaportCostOfGood.xlsx");
            exportData.ExportToExcelReport(dgvCostofGood, filePath);
        }

        private void pbExportRevenus_Click(object sender, EventArgs e)
        {
            string filePath = Path.Combine(exportData.desktopPath, "RaportRevenus.xlsx");
            exportData.ExportToExcelReport(dgvRevenus, filePath);
        }
    }
}
