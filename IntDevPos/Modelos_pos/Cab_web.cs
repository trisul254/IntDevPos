using IntDevPos.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntDevPos.Modelos_pos
{
    public class Cab_web
    {
        public static Configuracion cf = new Configuracion();

        public static readonly string IdDevInMotion  = "Id_devin";
        public static readonly string Identification = "Identi";
        public static readonly string ExpeditionDate = "Expdate";
        public static readonly string FirstName      = "Nombre";
        public static readonly string SecondName     = "Apellido";
        public static readonly string BirthDay       = "Cumple";
        public static readonly string Email          = "Email";
        public static readonly string PhoneNumbrer   = "Celular";
        public static readonly string Address        = "Direccion";
        public static readonly string Observations   = "Observa";
        public static readonly string PayTerm        = "Fpago";
        public static readonly string Usado          = "Usado";
        public static readonly string Respuesta      = "Respuesta";
        public static readonly string Valor          = "Valor";
        public static readonly string Facturando     = "Facturando";
        public static readonly string Domicilio      = "Domicilio";
        public static readonly string NOrden         = "Orden";

        // No se llenan 
        public static readonly string Sucursal = "Sucursal";
        public static readonly string Fecha  = "Fecha";
        public static readonly string Docum  = "Docum";
        public static readonly string Numero = "Numero";
        public static readonly string Hora_Fact = "Hora_Fact";
        
        // Si se llenan
        public static readonly string Fecha_in = "Fecha_In";
        public static readonly string Hora_in = "Hora_In";
        public static readonly string IdPos   = "IdPos";
        public static readonly string Impreso = "Impreso";

        //public static readonly string RutaTabla = @"U:\BUSIMS3\MERSAS3\DATOS\CAB_WEB.dbf";
        public static readonly string RutaTabla = cf.rutaTab("CAB_WEB");
    }
}
