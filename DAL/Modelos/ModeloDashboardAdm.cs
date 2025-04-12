using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Modelos
{
    public class ModeloDashboardAdm
    {
        public UsuariosDashboard Usuarios { get; set; }
        public PublicacionesDashboard Publicaciones { get; set; }
        public InteraccionesDashboard Interacciones { get; set; }
        public RevisionesDashboard Revisiones { get; set; }
    }

    public class UsuariosDashboard
    {
        public int TotalUsuarios { get; set; }
        public int UsuariosRegulares { get; set; }
        public int Moderadores { get; set; }
        public int Administradores { get; set; }
        public int NuevosHoy { get; set; }
    }

    public class PublicacionesDashboard
    {
        public int TotalPublicaciones { get; set; }
        public int Publicadas { get; set; }
        public int EnRevision { get; set; }
        public int Rechazadas { get; set; }
        public int CreadasHoy { get; set; }
    }

    public class InteraccionesDashboard
    {
        public int TotalComentarios { get; set; }
        public int TotalFavoritos { get; set; }
        public int TotalNotasEstudio { get; set; }
    }

    public class RevisionesDashboard
    {
        public int TotalRevisiones { get; set; }
        public int Aprobadas { get; set; }
        public int Rechazadas { get; set; }
    }
}
