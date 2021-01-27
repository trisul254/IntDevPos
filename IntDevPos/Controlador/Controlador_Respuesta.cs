using IntDevPos.Config;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntDevPos.Controlador
{
    public class Controlador_Respuesta
    {
        public static Configuracion cf = new Configuracion();

        public bool RespuestaOrden(Object obj, string nombreTxt)
        {
            bool isValid = false; 

            if (obj != null)
            {
                try
                {
                    String serializedResult = JsonConvert.SerializeObject(obj, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });
                    if (crearTxt(nombreTxt,serializedResult))
                    {
                        isValid = true;
                    }
                }
                catch (Exception)
                {
            
                }
            }
            return isValid;
        }

        public bool crearTxt(string NomTxt, String ObjSerialized)
        {
            bool isValid = false;
            //string Location = @"C:\Users\DESARROLLO4\Desktop\RESPUESTAS\";
            //string Location = @"Z:\ArchivosTest\";
            string Location = cf.rutaResp();
            
            if (Directory.Exists(Location))
            {
                if (File.Exists(Location + NomTxt))
                {
                    File.Delete(Location + NomTxt);

                    crearTxt(NomTxt,ObjSerialized);
                }
                else
                {
                    FileStream newFile = File.Create(Location + NomTxt);
                    newFile.Close();
                    StreamWriter datosFile = File.AppendText(Location + NomTxt);
                    datosFile.WriteLine(ObjSerialized);
                    datosFile.Close();
                    isValid = true;
                }
            }
            return isValid;
        }

    }
}
