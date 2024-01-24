using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace Conexion_Gestion_Pedidos
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            string miConexion = ConfigurationManager.ConnectionStrings["Conexion_Gestion_Pedidos.Properties.Settings.GestionPedidosConnectionString"].ConnectionString;
            miConexionSql = new SqlConnection(miConexion);
            MuestraClientes();
            MuestraTodosPedidos();
        }

        private void MuestraClientes()
        {
            try
            {


                string consulta = "SELECT * FROM CLIENTE";

                SqlDataAdapter miAdaptadorSql = new SqlDataAdapter(consulta, miConexionSql);

                using (miAdaptadorSql)
                {
                    DataTable clientesTabla = new DataTable();
                    miAdaptadorSql.Fill(clientesTabla);

                    listaClientes.DisplayMemberPath = "nombre";
                    listaClientes.SelectedValuePath = "Id";
                    listaClientes.ItemsSource = clientesTabla.DefaultView;
                }
            }
            catch (Exception e) {
                MessageBox.Show(e.ToString());
            }
        }        
        private void MuestraPedidos()
        {
            try
            {

            
            string consulta = "SELECT * FROM PEDIDO P INNER JOIN CLIENTE C ON C.ID=P.cCliente"+
                " WHERE C.ID=@ClienteId";
            SqlCommand sqlCommand = new SqlCommand(consulta, miConexionSql);
            SqlDataAdapter miAdaptadorSql = new SqlDataAdapter(sqlCommand);

            using (miAdaptadorSql)
            {
                sqlCommand.Parameters.AddWithValue("@ClienteId", listaClientes.SelectedValue);

                DataTable pedidosTabla = new DataTable();
                miAdaptadorSql.Fill(pedidosTabla);

                listaPedidos.DisplayMemberPath = "fechaPedido";
                listaPedidos.SelectedValuePath = "Id";
                listaPedidos.ItemsSource = pedidosTabla.DefaultView;
            }
        }
            catch (Exception e) {
                MessageBox.Show(e.ToString());
            }
}
        private void MuestraTodosPedidos()
        {
            try
            {

            
            string consulta = "SELECT *, CONCAT(CCliente,' ', fechaPedido,' ',formapago) AS INFOCOMPLETA FROM PEDIDO";

            SqlDataAdapter miAdaptadorSql = new SqlDataAdapter( consulta, miConexionSql);

            using (miAdaptadorSql)
            {
                DataTable pedidosTabla = new DataTable();

                miAdaptadorSql.Fill(pedidosTabla);

                todosPedidos.DisplayMemberPath = "INFOCOMPLETA";
                todosPedidos.SelectedValuePath = "Id";
                todosPedidos.ItemsSource = pedidosTabla.DefaultView;
            }
        }
            catch (Exception e) {
                MessageBox.Show(e.ToString());
            }
}

        SqlConnection miConexionSql;

 

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(todosPedidos.SelectedValue.ToString());

            string consulta = "DELETE FROM PEDIDO WHERE ID=@PEDIDOID";

            SqlCommand sqlCommand = new SqlCommand(consulta, miConexionSql);

            miConexionSql.Open();

            sqlCommand.Parameters.AddWithValue("@PEDIDOID", todosPedidos.SelectedValue);

            sqlCommand.ExecuteNonQuery();

            miConexionSql.Close();

            MuestraTodosPedidos();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            string consulta = "DELETE FROM CLIENTE WHERE ID=@CLIENTEID";

            SqlCommand sqlCommand = new SqlCommand(consulta, miConexionSql);

            miConexionSql.Open();

            sqlCommand.Parameters.AddWithValue("@CLIENTEID", listaClientes.SelectedValue);

            sqlCommand.ExecuteNonQuery();

            miConexionSql.Close();

            MuestraClientes();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

            string consulta = "INSERT INTO CLIENTE (nombre) VALUES (@nombre)";

            SqlCommand sqlCommand = new SqlCommand(consulta, miConexionSql);

            miConexionSql.Open();

            sqlCommand.Parameters.AddWithValue("@nombre", insertaCliente.Text);

            sqlCommand.ExecuteNonQuery();

            miConexionSql.Close();

            MuestraClientes();
            insertaCliente.Text = " ";
        }

        private void listaClientes_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MuestraPedidos();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
          
            Actualiza ventanaActualizar = new Actualiza((int)listaClientes.SelectedValue);



            try
            {


                string consulta = "SELECT nombre FROM CLIENTE WHERE Id=@ClienteId";
                SqlCommand sqlCommand = new SqlCommand(consulta,miConexionSql);
                SqlDataAdapter miAdaptadorSql = new SqlDataAdapter(sqlCommand);

                using (miAdaptadorSql)
                {
                    sqlCommand.Parameters.AddWithValue("@ClienteId", listaClientes.SelectedValue);
                    DataTable clientesTabla = new DataTable();
                    miAdaptadorSql.Fill(clientesTabla);

                    ventanaActualizar.nombreCliente.Text = clientesTabla.Rows[0]["nombre"].ToString();
                }
                ventanaActualizar.ShowDialog();
                MuestraClientes();

            }
            catch (Exception excep)
            {
                MessageBox.Show(excep.ToString());
            }
        }
    }
}
