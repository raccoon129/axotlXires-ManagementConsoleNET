using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Modelos
{
    public class ModeloLogin
    {
        public string correo { get; set; }
        public string contrasena { get; set; }
    }

    public class DatosUsuario
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string correo { get; set; }
        public string rol { get; set; }
    }

    public class DatosLoginRespuesta
    {
        public DatosUsuario usuario { get; set; }
        public string token { get; set; }
    }

    public class LoginRespuesta
    {
        public string status { get; set; }
        public string mensaje { get; set; }
        public DatosLoginRespuesta datos { get; set; }
    }
}
