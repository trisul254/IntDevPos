using System;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntDevPos.Conexion
{
    public class ConeFox
    {


        private OleDbConnection connect = null;

        // Metodo que retorna true si hay conexion con la tabla que envian por parametro.
        public bool Connect(string Ruta)
        {
            try
            {
                string rutaConn = @"Provider=VFPOLEDB.1;Data Source=" + Ruta.Trim();
                connect = new OleDbConnection(rutaConn);
                return true;
            }
            catch (Exception e)
            {
                connect = null;
                Console.WriteLine("No se logró hacer conexion con la dbf" + "\n" + e.Message);
                return false;
            }
        }

        public int ejecutar(string sql)
        {
            if (connect != null)
            {
                OleDbCommand ocmd = null;   // Instruccion sql a ejecutar
                int filasAfectadas = 0;
                try
                {
                    
                    connect.Open();      // Abre la conexion con el dbf
                    if (connect.State == ConnectionState.Open)
                    {
                        ocmd = connect.CreateCommand();
                        ocmd.CommandText = sql.Trim();              // Sentencia sql a ejecutar
                        filasAfectadas = ocmd.ExecuteNonQuery();    // Ejecuta y devuelve el numero de filas afectadas
                        connect.Close();
                    }
                }
                catch (OleDbException exp)
                {
                     Console.WriteLine("Error al insertar " + exp.ToString());
                }
                return filasAfectadas;
            }
            else
            {
                return 0;
            }
        }
    }
}
