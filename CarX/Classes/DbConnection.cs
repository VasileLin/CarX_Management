using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarX
{
     public class DbConnection
     {
        SqlCommand command = new SqlCommand();
        static string outputFile = "DBPatch.txt";
        static public string databasePath = File.ReadAllText(outputFile);
        public string projectPatch = databasePath;
       private SqlConnection connection = new SqlConnection($@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={databasePath};
       Integrated Security=True; Connect Timeout=30");


        // Metoda pentru a returna obiectul SqlConnection
        public SqlConnection Connect()
        {
            return connection;
        }


        // Metoda pentru a deschide conexiunea
        public void Open()
        {
            if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
        }

        // Metoda pentru a închide conexiunea
        public void Close()
        {
            if (connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }
        }

        // Metoda pentru a executa o interogare
        public void ExecuteQuery(string sql)
        {
            try
            {
                Open();
                command = new SqlCommand(sql, Connect());
                command.ExecuteNonQuery();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"CarX Management System");
            }
        }
     }
}
