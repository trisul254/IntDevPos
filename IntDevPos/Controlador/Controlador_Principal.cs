using IntDevPos.Conexion;
using IntDevPos.Modelos;
using IntDevPos.Modelos_pos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntDevPos.Controlador
{
    public class Controlador_Principal
    {
        public static ConeFox con = new ConeFox();
        public string CarpetaOrdenes = @"C:\Users\DESARROLLO4\Desktop\ORDENES\";
        public string CarpetaOrdenesInsertadas = @"C:\Users\DESARROLLO4\Desktop\ORDENES_EXITOSAS\";
        public string CarpetaOrdenesErroneas = @"C:\Users\DESARROLLO4\Desktop\ORDENES_ERRONEAS\";

        public void LeerJsonTxt()
        {
            List<string> Ordenes = new List<string>();  // Listado de todas las ordenes que encuentre en la carpeta.
            if (Directory.Exists(CarpetaOrdenes))
            {
                // Obtiene todos los archivos de ordenes y los agrega al list
                Ordenes.AddRange(Directory.GetFiles(CarpetaOrdenes));
                if (Ordenes.Count > 0) // Si encuentra ordenes en la carpeta continua con el proceso.
                {
                    foreach (var reg in Ordenes)
                    {
                        // Ruta nueva para Json Exitosos 
                        //string RutaNuevaI = CarpetaOrdenesInsertadas + Path.GetFileName(reg);
                        string RutaNuevaI = CarpetaOrdenesInsertadas + "Procesado-" + Path.GetFileName(reg);
                        // Ruta nueva para Json Erroneos 
                        string RutaNuevaE = CarpetaOrdenesErroneas + "Erronea-" + Path.GetFileName(reg);

                        /* Se agrega trycatch para el deserializar el json
                         * Dado el caso sea exitoso copia la orden a la carpeta procesadas
                         * Dado el caso sea erroneo copia la orden a la carpeta erroneas
                        */
                        try
                        {
                            // Cumplio al deserializar.
                            if (DeserializarJson(reg))
                            {
                                if (!File.Exists(RutaNuevaI))
                                {
                                    File.Copy(reg, RutaNuevaI);
                                }
                            }
                            else
                            {
                                if (!File.Exists(RutaNuevaE))
                                {
                                    File.Copy(reg, RutaNuevaE);
                                }
                            }
                        }
                        catch (Exception )
                        {
                            if (!File.Exists(RutaNuevaE))
                            {
                                File.Copy(reg, RutaNuevaE);
                            } 
                        }
                        // Elimina la orden independientemente fuera exitosa o erronea
                        File.Delete(reg);
                    }
                }

            }
        }

        public bool DeserializarJson(string ArchivoJson)
        {
            bool isValid = false;
            // Se abre el archivo 
            using (StreamReader JsonStream = File.OpenText(ArchivoJson))
            {
                var JsonRead = JsonStream.ReadToEnd(); // Hace la lectura del archivo
                var JsonConv = JsonConvert.DeserializeObject<Principal>(JsonRead);

                // Valida que el Id este lleno para continuar.
                if (!string.IsNullOrEmpty(JsonConv.OrderreferenceId))
                {

                    if (RecorrerOrden(JsonConv))
                    {
                        isValid = true;
                    }

                }

            }

            return isValid;

        }

        private bool RecorrerOrden(Principal modelPrin)
        {
            int contador = 0;
            // Cabecera
            string IdDevInMotion  = modelPrin.OrderreferenceId;
            string Identification = modelPrin.customer.identification;
            string ExpeditionDate = modelPrin.customer.expeditionDate;
            string PrimerNombre   = modelPrin.customer.firstName;
            string PrimerApellido = modelPrin.customer.secondName;
            string Cumpleanos     = modelPrin.customer.birthDate;
            string email          = modelPrin.customer.email;
            string celular        = modelPrin.customer.phoneNumber;
            string direccion      = modelPrin.customer.Address;

            // Agrega la cabecera 
            if (InsertarCab(IdDevInMotion,Identification,ExpeditionDate,PrimerNombre,PrimerApellido,Cumpleanos,email,celular,direccion))
            {
                // Recorre salesLinesList para agregar el detalle.
                foreach (var pOrden in modelPrin.salesLinesList)
                {
                    // Detalle
                    if (InsertarDet(IdDevInMotion, pOrden.productId, pOrden.salesQty, pOrden.salesPrice))
                    {
                        contador++;
                    }
                }
            }
            

            return contador > 0 ? true : false;
        }

        public bool InsertarDet(string IdDev, string Codigo, int Cant, string Precio)
        {
            bool isInsertDet = false;
            string sqlDetInt = "INSERT INTO " + Constantes_T.TBL_DETWEB + "(" +
                            Det_web.IdDevInMotion + "," + Det_web.Codigo + "," +
                            Det_web.Cantidad + "," + Det_web.Valor + "," +
                            Det_web.Usado + ") " +
                            "VALUES('" + IdDev + "','" + Codigo + "'," + Cant +
                            "," + Precio + "," + 0 + ")";
            // Hace conexion con la tabla detalle.
            if (con.Connect(Det_web.RutaTabla))
            {
                if (con.ejecutar(sqlDetInt)>0)
                {
                    isInsertDet = true;
                }
                
            }

            return isInsertDet ;
        }

        public bool InsertarCab(string IdDev, string identification, string expeditionDate, string primerNombre, string primerApellido, string cumpleanos, string email, string celular, string direccion,string payterm, string observa)
        {
            bool isInsertCab = false;

            string sqlCabInt = "INSERT INTO " + Constantes_T.TBL_CABWEB + "(" +
                            Cab_web.IdDevInMotion + "," + Cab_web.Identification + "," +
                            Cab_web.ExpeditionDate + ","+ Cab_web.FirstName + ","+ 
                            Cab_web.SecondName + "," + Cab_web.BirthDay + "," + 
                            Cab_web.Email + ","+ Cab_web.PhoneNumbrer + ","+
                            Cab_web.Address + ","+ Cab_web.PayTerm + ","+Cab_web.Observations+
                            ") VALUES('" + IdDev + "','"+ identification+"','"+ expeditionDate + "','"+
                            primerNombre + "','"+ primerApellido + "','"+ cumpleanos+ "','"+ email + 
                            "','"+ celular + "','"+ direccion + "','"+ payterm + "','"+ observa + "')";

            // Hace conexion con la tabla cabecera.
            if (con.Connect(Cab_web.RutaTabla))
            {
                if (con.ejecutar(sqlCabInt) > 0)
                {
                    isInsertCab = true;
                }
            }

            return isInsertCab;
        }

    }
}
