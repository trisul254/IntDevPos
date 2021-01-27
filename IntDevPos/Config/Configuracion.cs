using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntDevPos.Config
{
    public class Configuracion
    {
        public static string RutaBase = @"J:\BUSINSAS\MERSAS\";
        // Direccion de ordenes
        public static string PruebasDevOrd = @"Z:\ArchivosTest\Orders\";
        public static string ProduccionOrd = @"Z:\Archivos\Orders\";
        // Direccion de respuestas
        public static string PruebasDevRes = @"Z:\ArchivosTest\";
        public static string ProduccionRes = @"Z:\Archivos\";
        // Direccion de tablas
        public static string PruebaUTab = @"U:\BUSIMS3\MERSAS3\DATOS\";
        public static string ProduccionTab = RutaBase + @"DATOS\";
        // Direccion de ordenes para guardar 
        public static string OrdenesJ = RutaBase + @"DEVINMOTION\ExeDevIn\Ordenes\";
        public static string OrdenesZ = @"Z:\Archivos\ProcesadosMer\";


        public string rutaGetOrd()
        {
            string ruta = "";
            int pOpcion = configArch(1);

            switch (pOpcion)
            {
                case 1:
                    ruta = PruebasDevOrd;
                    break;
                case 2:
                    ruta = ProduccionOrd;
                    break;
            }

            return ruta;
        }

        public string rutaResp()
        {
            string ruta = "";
            int pOpcion = configArch(2);

            switch (pOpcion)
            {
                case 1:
                    ruta = PruebasDevRes;
                    break;
                case 2:
                    ruta = ProduccionRes;
                    break;
            }
            
            return ruta;

        }

        public string rutaTab(string tabla)
        {
            string ruta = "";
            int pOpcion = configArch(3);
            switch (pOpcion)
            {
                // Pruebas
                case 1:
                    ruta = PruebaUTab + tabla + ".DBF";
                    break;
                // Produccion
                case 2:
                    ruta = ProduccionTab + tabla + ".DBF";
                    break;
            }

            return ruta;
        }

        public string rutaOrdenMer()
        {
            string ruta = "";
            int pOpcion = configArch(4);
            switch (pOpcion)
            {
                case 1:
                    ruta = OrdenesJ;
                    break;
                case 2:
                    ruta = OrdenesZ;
                    break;
            }

            return ruta;

        }
        
        public static int configArch(int pConfig)
        {
            int Datos = 0;
            string dirConfig = "";
            
            switch (pConfig)
            {
                case 1:
                    dirConfig = RutaBase + @"DEVINMOTION\ExeDevIn\ConfigOrders\ConfigOrder.txt";
                    break;
                case 2:
                    dirConfig = RutaBase + @"DEVINMOTION\ExeDevIn\ConfigOrders\ConfigResp.txt";
                    break;
                case 3:
                    dirConfig = RutaBase + @"DEVINMOTION\ExeDevIn\ConfigOrders\ConfigTablas.txt";
                    break;
                case 4:
                    dirConfig = RutaBase + @"DEVINMOTION\ExeDevIn\ConfigOrders\ConfigCopyOrd.txt";
                    break;
            }
            if (File.Exists(dirConfig))
            {
                StreamReader archivoCon = File.OpenText(dirConfig);
                Datos = Int32.Parse(archivoCon.ReadLine());
                archivoCon.Close();
            }
            else
            {
                Console.WriteLine("NO SE ENCUENTRAN ARCHIVOS DE CONFIGURACIÓN");
            }

            return Datos;
        }

    }
}
