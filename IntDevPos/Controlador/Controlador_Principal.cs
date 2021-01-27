using IntDevPos.Conexion;
using IntDevPos.Config;
using IntDevPos.Modelos;
using IntDevPos.Modelos_pos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace IntDevPos.Controlador
{
    public class Controlador_Principal : Configuracion
    {
        public static ConeFox con = new ConeFox();
        public static Configuracion cf = new Configuracion();

        //public string CarpetaOrdenes = @"Z:\ArchivosTest\Orders\";
        public string CarpetaOrdenes = cf.rutaGetOrd();        
        public string CarpetaOrdenesInsertadas = @"C:\Users\DESARROLLO4\Desktop\ORDENES_EXITOSAS\";
        public string CarpetaOrdenesErroneas = @"C:\Users\DESARROLLO4\Desktop\ORDENES_ERRONEAS\";

        /*public string CarpetaOrdenes = @"C:\Users\DESARROLLO4\Desktop\ORDENES\";
        public string CarpetaOrdenesInsertadas = @"C:\Users\DESARROLLO4\Desktop\ORDENES_EXITOSAS\";
        public string CarpetaOrdenesErroneas = @"C:\Users\DESARROLLO4\Desktop\ORDENES_ERRONEAS\";*/
        public static Controlador_Respuesta CR = new Controlador_Respuesta();

        public void LeerJsonTxt()
        {
            if (validIn())
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
                            // Nombre del archivo respuesta
                            string RespuestaOrd = "OrderResponse-" + Path.GetFileName(reg);

                            // Copia de ordenes a carpetas configuradas
                            string RutaNuevaBack = rutaOrdenMer() + Path.GetFileName(reg);

                            /* Se agrega trycatch para el deserializar el json
                             * Dado el caso sea exitoso copia la orden a la carpeta procesadas
                             * Dado el caso sea erroneo copia la orden a la carpeta erroneas
                            */
                            try
                            {

                                if (DeserializarJson(reg, RespuestaOrd))
                                {
                                    /*if (!File.Exists(RutaNuevaI))
                                    {
                                        File.Copy(reg, RutaNuevaI);
                                    }*/
                                }
                                else
                                {
                                    /*if (!File.Exists(RutaNuevaE))
                                     {
                                         File.Copy(reg, RutaNuevaE);
                                     }*/
                                }
                            }
                            catch (Exception err)
                            {
                                /*if (!File.Exists(RutaNuevaE))
                                {
                                    File.Copy(reg, RutaNuevaE);
                                } */
                            }

                            // Copia la orden independientemente fuera exitosa o erronea
                            File.Copy(reg, RutaNuevaBack);
                            // Elimina la orden independientemente fuera exitosa o erronea
                            File.Delete(reg);
                        }
                    }

                }
            }
            else
            {
                Console.WriteLine("VALIDAR CONEXIONES AL SERVIDOR DEVINMOTION Y PRODUCCION EN J ");
                Console.ReadLine();
            }
        }

        public bool  DeserializarJson(string ArchivoJson,string Resp)
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
                    if (RecorrerOrden(JsonConv,Resp))
                    {
                        isValid = true;
                    }
                    
                }

            }
            return isValid;

        }

        private bool RecorrerOrden(Principal modelPrin, string Resp)
        {
            int contador = 0;
            bool isValid = false;
            // Cabecera
            string IdDevInMotion  = modelPrin.OrderreferenceId;
            string registro = DateTime.Now.ToString("s") + modelPrin.customer.identification;

            // Agrega la cabecera 
            if (InsertarCab(modelPrin, registro))
            {
                // Recorre salesLinesList para agregar el detalle.
                foreach (var pOrden in modelPrin.salesLinesList)
                {
                    // Detalle
                    if (InsertarDet(IdDevInMotion,pOrden))
                    {
                        contador++;
                    }
                }
            }

            if (contador > 0)
            {
                // Crea respuesta.
                if (crearRespuesta(IdDevInMotion,registro,Resp))
                {
                    isValid = true ;
                }
            }
            else
            {
                if (crearRespuesta(IdDevInMotion,"",Resp))
                {
                    isValid = true;
                }
            }
            return isValid;
        }

        public bool InsertarDet(string IdDev, SalesLinesList pOrden)
        {
            string Codigo  = pOrden.productId;
            int Cant       = pOrden.salesQty;
            string Precio  = pOrden.salesPrice;
            string Preciod = pOrden.lineDisc;

            bool isInsertDet = false;
            string sqlDetInt = "INSERT INTO " + Constantes_T.TBL_DETWEB + "(" +
                            Det_web.IdDevInMotion + "," + Det_web.Codigo + "," +
                            Det_web.Cantidad + "," + Det_web.Valor + "," + Det_web.LineDisc+  ") " +
                            "VALUES('" + IdDev + "','" + Codigo + "'," + Cant +
                            "," + Precio + ","+ Preciod +")";
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

        public bool InsertarCab(Principal modelPrin, string Res)
        {
            bool isValid = false;

            string IdDev = modelPrin.OrderreferenceId;
            string TotalCompra = modelPrin.totalAmount;
            string identification = validarCedula(modelPrin.customer.identification);
            string expeditionDate = modelPrin.customer.expeditionDate;
            string primerNombre   = modelPrin.customer.firstName;
            string primerApellido = modelPrin.customer.secondName;
            string cumpleanos = modelPrin.customer.birthDate;
            string email     = modelPrin.customer.email;
            string celular   = modelPrin.customer.phoneNumber;
            string direccion = validarDir(modelPrin.customer.Address);
            string payterm   = modelPrin.paymTerm;
            string observa   = validarObservacion(modelPrin.observations);
            string Respuesta = Res ;
            string CodOrden  = CodOrd(IdDev);
            string Fecha_va  = "CTOD(' . . ')";
            string Fecha_in  = "Date()";
            string Hora_in   = DateTime.Now.ToString("HH:mm:ss");
            string Domi      = modelPrin.domicileAmount ;
            string Orden     = modelPrin.custPurchaseOrder;

            string sqlCabInt = "INSERT INTO " + Constantes_T.TBL_CABWEB + "(" +
                            Cab_web.IdDevInMotion  + "," + Cab_web.Identification + "," +
                            Cab_web.ExpeditionDate + "," + Cab_web.FirstName + "," + 
                            Cab_web.SecondName + "," + Cab_web.BirthDay      + "," + 
                            Cab_web.Email      + "," + Cab_web.PhoneNumbrer  + "," + Cab_web.Address + ","+ 
                            Cab_web.PayTerm    + "," + Cab_web.Observations  + "," + Cab_web.Usado   + "," +
                            Cab_web.Respuesta  + "," + Cab_web.Valor     + "," + Cab_web.Facturando + ","+
                            Cab_web.Sucursal   + "," + Cab_web.Fecha     + "," + Cab_web.Docum      + "," + 
                            Cab_web.Numero     + "," + Cab_web.Hora_Fact + "," + Cab_web.Fecha_in   + "," +
                            Cab_web.Hora_in    + "," + Cab_web.IdPos     + "," + Cab_web.Impreso    + ","+ 
                            Cab_web.Domicilio  + "," + Cab_web.NOrden    + 
                            ") VALUES('"   + IdDev + "','"+ identification+"','"+ expeditionDate + "','"+
                            primerNombre   + "','" + primerApellido + "','" + cumpleanos + "','" + email    + 
                            "','"+ celular + "','" + direccion      + "','" + payterm    + "','" + observa  + "'," + 0    + ",'" +
                            Respuesta + "',"       + TotalCompra    + ",'"  + "','"      + "',"  + Fecha_va + ",'" + "'," + 0 +
                            ",'" + "',"+ Fecha_in +",'" + Hora_in   + "','" + CodOrden   + "',"  + 0 + ","  + Domi + ",'" + Orden +"')";

            // Hace conexion con la tabla cabecera.
            if (con.Connect(Cab_web.RutaTabla))
            {
                if (con.ejecutar(sqlCabInt) > 0)
                {
                    isValid = true;
                }
            }
            
            return isValid;
        }

        private string validarDir(string address)
        {
            string dir = address.Replace("'", "");

            return dir;
        }

        private string validarObservacion(string observations)
        {
            string observa = observations.Replace("\n","");

            //observa = observa.Substring(0,100);

            return observa;
        }

        private string validarCedula(string identification)
        {
            string cedula = identification.Replace(",","").Replace(".",""); 

            return cedula;
        }

        public bool crearRespuesta(string IdDev, string Respuesta,string nombreTxt)
        {
            bool isValid = false;
            bool Successful = false;
            string mensaje = "Error al cargar detalle";

            if (Respuesta.Length > 0)
            {
                Successful = true;
                mensaje = "Exito";
            }

            var ordResp = new OrderResponse();
            ordResp.orderReferencecode = IdDev;
            ordResp.OrderId = Respuesta;
            ordResp.Successful = Successful;
            ordResp.Mensaje = mensaje;          

            if (CR.RespuestaOrden(ordResp, nombreTxt))
            {
                isValid = true;
            }
            
            return isValid;
        } 
       
        public string CodOrd(string candena)
        {
            candena = candena.Substring(0, 8).ToUpper();
            return candena;
        }

        public bool validIn()
        {
            bool isValid = false;
            if (Directory.Exists(rutaGetOrd()) && Directory.Exists(rutaResp()) && File.Exists(rutaTab("CAB_WEB")) && Directory.Exists(rutaOrdenMer()))
            {
                isValid = true;
            }

            return isValid;
        }
    }
}
