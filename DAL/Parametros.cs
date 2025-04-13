using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public static class Parametros
    {
        /// <summary>
        /// Almacena el JWT para el uso de la API, EXCLUSIVO DE USO DE LA LIBRERIA 
        /// </summary>
        public static string token;
        /// <summary>
        /// Almacena el usuario para el uso de la API
        /// </summary>
        internal static string JWTUser = "Management";
        /// <summary>
        /// Almacena el password para el uso de la API
        /// </summary>
        internal static string JWTPassword = "5jedasD&*Zetk9SmRRwCX";
        /// <summary>
        /// Almacena la URI de la API
        /// </summary>
        internal static string ApiBaseAddress = @"http://localhost:3001";

        public static string UrlBaseApi { get; set; } = "http://localhost:3001";
        public static string Flipbook { get; set; } = "http://localhost:3002";

    }
}
