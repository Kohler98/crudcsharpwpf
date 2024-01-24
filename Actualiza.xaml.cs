using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Conexion_Gestion_Pedidos
{
    /// <summary>
    /// Lógica de interacción para Actualiza.xaml
    /// </summary>
    public partial class Actualiza : Window
    {
        SqlConnection miConexionSql;
        private int z;
        public Actualiza(int id)
        {
            InitializeComponent();
            z = id;
            string miConexion = ConfigurationManager.ConnectionStrings["Conexion_Gestion_Pedidos.Properties.Settings.GestionPedidosConnectionString"].ConnectionString;
            miConexionSql = new SqlConnection(miConexion);
        }
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            string consulta = "UPDATE CLIENTE SET nombre=@nombre WHERE Id="+z;

            SqlCommand sqlCommand = new SqlCommand(consulta, miConexionSql);

            miConexionSql.Open();

            sqlCommand.Parameters.AddWithValue("@nombre", nombreCliente.Text);

            sqlCommand.ExecuteNonQuery();

            miConexionSql.Close();

            this.Close();
 
        }
    }
}
