using System.Windows.Forms.DataVisualization.Charting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CarWashManagementSystem;

namespace CarX.Forms.Modules
{
    public partial class Dashboard : Form
    {
        MainForm main;
        public Dashboard(MainForm mainForm)
        {
            InitializeComponent();
            main = mainForm;
            
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            decimal revenus = decimal.Parse(main.lblRevenus.Text);
            decimal cog = decimal.Parse(main.lblCostofGood.Text);
            decimal gprofit = revenus - cog;

            // Setările graficului
            chart1.Series.Clear();
            chart1.ChartAreas.Clear();
            ChartArea chartArea = chart1.ChartAreas.Add("chartArea");

            Series venitSeries = chart1.Series.Add("Revenus");
            venitSeries.ChartType = SeriesChartType.Column;
            venitSeries.Points.Add((double)revenus);
            venitSeries.Color = Color.Blue;

            Series costSeries = chart1.Series.Add("Cost of Good");
            costSeries.ChartType = SeriesChartType.Column;
            costSeries.Points.Add((double)cog);
            costSeries.Color = Color.Red;

            Series profitSeries = chart1.Series.Add("Gross Profit");
            profitSeries.ChartType = SeriesChartType.Column;
            profitSeries.Points.Add((double)gprofit);
            profitSeries.Color = Color.Green;
            chartArea.AxisX.Interval = 1;
            chartArea.AxisY.Minimum = 0;
            chart1.Invalidate();
        }
    }
}
