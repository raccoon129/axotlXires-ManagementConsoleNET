using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DAL.Modelos
{
    /// <summary>
    /// Modelo para manejar las respuestas básicas al consultar imágenes
    /// </summary>
    public class RespuestaConsultaImagen
    {
        /// <summary>
        /// Estado de la respuesta
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Mensaje descriptivo del resultado de la operación
        /// </summary>
        public string Mensaje { get; set; }

        /// <summary>
        /// Datos de la imagen consultada
        /// </summary>
        public DatosImagen Datos { get; set; }
    }

    /// <summary>
    /// Datos específicos de la imagen consultada
    /// </summary>
    public class DatosImagen
    {
        /// <summary>
        /// URL completa o ruta de la imagen
        /// </summary>
        [JsonPropertyName("url_imagen")]
        public string UrlImagen { get; set; }

        /// <summary>
        /// Identificador asociado a la imagen (ID de usuario o publicación)
        /// </summary>
        [JsonPropertyName("id_asociado")]
        public int IdAsociado { get; set; }

        /// <summary>
        /// Tipo de la imagen (foto_perfil, portada, etc.)
        /// </summary>
        [JsonPropertyName("tipo_imagen")]
        public string TipoImagen { get; set; }

        /// <summary>
        /// Fecha de última actualización de la imagen
        /// </summary>
        [JsonPropertyName("fecha_actualizacion")]
        public DateTime FechaActualizacion { get; set; }
    }

    /// <summary>
    /// Modelo para solicitudes de imágenes de perfil
    /// </summary>
    public class SolicitudImagenPerfil
    {
        /// <summary>
        /// ID del usuario cuya foto de perfil se desea obtener
        /// </summary>
        [JsonPropertyName("id_usuario")]
        public int IdUsuario { get; set; }
    }

    /// <summary>
    /// Modelo para solicitudes de imágenes de portada
    /// </summary>
    public class SolicitudImagenPortada
    {
        /// <summary>
        /// ID de la publicación cuya imagen de portada se desea obtener
        /// </summary>
        [JsonPropertyName("id_publicacion")]
        public int IdPublicacion { get; set; }
    }
}
