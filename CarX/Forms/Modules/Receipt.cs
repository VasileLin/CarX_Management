using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CarX.Reports;
using Microsoft.Reporting.WinForms;

namespace CarX.Forms.Modules
{
    public partial class Receipt : Form
    {
        SqlCommand command = new SqlCommand();
        DbConnection connection = new DbConnection();
        string title = "Carx Management System";
        SqlDataReader dataReader;
        string company, address,iban;
        Cash cash;
        public Receipt(Cash cashForm)
        {
            InitializeComponent();
            cash = cashForm;
            LoadCompany();
        }

        private void Receipt_Load(object sender, EventArgs e)
        {

            this.reportViewer1.RefreshReport();
        }

        public void LoadCompany()
        {
            command = new SqlCommand("SELECT * FROM Company",connection.Connect());
            connection.Open();
            dataReader = command.ExecuteReader();
            dataReader.Read();

            if (dataReader.HasRows)
            {
                company = dataReader["name"].ToString();
                address = dataReader["address"].ToString();
                iban = dataReader["iban"].ToString();

            }
            dataReader.Close();
            connection.Close();
        }

        private void reportViewer1_Load(object sender, EventArgs e)
        {

        }

        public void LoadReceipt(string pcash,string pchange)
        {
            ReportDataSource reportDataSource;

            try
            {
                this.reportViewer1.LocalReport.ReportPath = Application.StartupPath + @"\Reports\rptRecept.rdlc";
                this.reportViewer1.LocalReport.DataSources.Clear();

                ReportDataSet dataSet = new ReportDataSet();
                SqlDataAdapter dataAdapter = new SqlDataAdapter();
                connection.Open();
                dataAdapter.SelectCommand = new SqlCommand($"SELECT s.name,c.price FROM Cash AS c INNER JOIN Service AS s ON c.sid=s.id WHERE c.transno LIKE '{cash.lblTransno.Text}'",connection.Connect());
                dataAdapter.Fill(dataSet.Tables["Receipt"]);
                connection.Close();


                connection.Open();
                command = new SqlCommand($"SELECT s.name FROM Cash AS c INNER JOIN tbCustomer AS s ON c.cid=s.id WHERE c.transno LIKE '{cash.lblTransno.Text}'", connection.Connect());
                dataReader = command.ExecuteReader();
                string customerName="";
                while (dataReader.Read())
                {
                    customerName = $"{dataReader[0]}";       
                }
                connection.Close();

                ReportParameter pCompany = new ReportParameter("pCompany",company);
                ReportParameter pAddress = new ReportParameter("pAddress",address);
                ReportParameter pChange = new ReportParameter("pChange",pchange);
                ReportParameter pCash = new ReportParameter("pCash",pcash);
                ReportParameter pTotal = new ReportParameter("pTotal",cash.lblTotal.Text);
                ReportParameter pTransaction = new ReportParameter("pTransaction",cash.lblTransno.Text);
                ReportParameter pCarno = new ReportParameter("pCarno",cash.carno);
                ReportParameter pCarModel = new ReportParameter("pCarModel",cash.carmodel);
                ReportParameter pCustomer = new ReportParameter("pCustomer",customerName);
                ReportParameter pIBAN = new ReportParameter("pIBAN",iban);


                reportViewer1.LocalReport.SetParameters(pCompany);
                reportViewer1.LocalReport.SetParameters(pAddress);
                reportViewer1.LocalReport.SetParameters(pChange);
                reportViewer1.LocalReport.SetParameters(pCash);
                reportViewer1.LocalReport.SetParameters(pTotal);
                reportViewer1.LocalReport.SetParameters(pTransaction);
                reportViewer1.LocalReport.SetParameters(pCarno);
                reportViewer1.LocalReport.SetParameters(pCarModel);
                reportViewer1.LocalReport.SetParameters(pCustomer);
                reportViewer1.LocalReport.SetParameters(pIBAN);


                reportDataSource = new ReportDataSource("DataSet1", dataSet.Tables["Receipt"]);
                reportViewer1.LocalReport.DataSources.Add(reportDataSource);
                reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
                reportViewer1.ZoomMode = ZoomMode.PageWidth;


               

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, title);
            }
        }
    }
}
