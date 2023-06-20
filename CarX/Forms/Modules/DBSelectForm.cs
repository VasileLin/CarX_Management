using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace CarX.Forms.Modules
{
    public partial class DBSelectForm : Form
    {
        public DBSelectForm()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnSelecteaza_Click(object sender, EventArgs e)
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string databaseDirectory = Path.Combine(baseDirectory, "Database");

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = databaseDirectory;

            DialogResult result = openFileDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                string selectedFilePath = openFileDialog.FileName;

                string outputFile = "DBPatch.txt";
                if (!File.Exists(outputFile))
                {
                    File.Create(outputFile).Dispose();
                }
                File.WriteAllText(outputFile, selectedFilePath);

                MessageBox.Show("DB Patch Saved!");
                Application.Restart();
            }
        }
    }
}
