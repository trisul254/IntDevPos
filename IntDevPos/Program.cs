using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using IntDevPos.Modelos;
using IntDevPos.Modelos_pos;
using IntDevPos.Conexion;
using Newtonsoft.Json;
using IntDevPos.Controlador;

namespace IntDevPos
{
    class Program
    {
        public static ConeFox con = new ConeFox();
        public static Controlador_Principal Cp = new Controlador_Principal();
        static void Main(string[] args)
        {

            Cp.LeerJsonTxt();

            //Console.ReadLine();

        }
    }
}
