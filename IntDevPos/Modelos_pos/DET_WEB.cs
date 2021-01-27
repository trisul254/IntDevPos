using IntDevPos.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntDevPos.Modelos_pos
{
    public class Det_web
    {
        public static Configuracion cf = new Configuracion();

        public static readonly string RutaTabla = cf.rutaTab("DET_WEB");
        //public static readonly string RutaTabla = @"U:\BUSIMS3\MERSAS3\DATOS\DET_WEB.dbf";

        public static readonly string IdDevInMotion = "ID_DEVIN";
        public static readonly string Codigo = "Codigo";
        public static readonly string Cantidad = "Cant";
        public static readonly string Valor = "Valor";
        public static readonly string LineDisc = "LineDisc";

    }
}
