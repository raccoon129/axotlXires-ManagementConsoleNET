using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DAL.Modelos
{
    /// <summary>
    /// Modelos para la gestión de perfiles administrativos
    /// </summary>
    public class ModeloAdministradoresUsuarios
    {
        #region Modelos para consulta de perfil administrativo

        /// <summary>
        /// Respuesta al consultar el perfil de un usuario administrativo
        /// </summary>
        public class RespuestaPerfilAdministrativo
        {
            /// <summary>
            /// Estado de la operación (success/error)
            /// </summary>
            public string Status { get; set; }

            /// <summary>
            /// Mensaje descriptivo del resultado
            /// </summary>
            public string Mensaje { get; set; }

            /// <summary>
            /// Datos del perfil administrativo
            /// </summary>
            public PerfilAdministrativo Datos { get; set; }
        }

        /// <summary>
        /// Datos del perfil de un usuario administrativo
        /// </summary>
        public class PerfilAdministrativo
        {
            /// <summary>
            /// Identificador único del usuario
            /// </summary>
            [JsonPropertyName("id_usuario")]
            public int IdUsuario { get; set; }

            /// <summary>
            /// Nombre completo del usuario
            /// </summary>
            public string Nombre { get; set; }

            /// <summary>
            /// Correo electrónico del usuario
            /// </summary>
            public string Correo { get; set; }

            /// <summary>
            /// Rol del usuario en el sistema (administrador, moderador, etc.)
            /// </summary>
            public string Rol { get; set; }

            /// <summary>
            /// Cargo o nombramiento del usuario
            /// </summary>
            public string Nombramiento { get; set; }

            /// <summary>
            /// Fecha de creación de la cuenta
            /// </summary>
            [JsonPropertyName("fecha_creacion")]
            public DateTime FechaCreacion { get; set; }

            /// <summary>
            /// Fecha y hora del último acceso al sistema
            /// </summary>
            [JsonPropertyName("ultimo_acceso")]
            public string UltimoAcceso { get; set; }

            /// <summary>
            /// Nombre del archivo de la foto de perfil
            /// </summary>
            [JsonPropertyName("foto_perfil")]
            public string FotoPerfil { get; set; }

            /// <summary>
            /// Estadísticas del usuario
            /// </summary>
            public EstadisticasAdministrativas Estadisticas { get; set; }
        }

        /// <summary>
        /// Estadísticas de un usuario administrativo
        /// </summary>
        public class EstadisticasAdministrativas
        {
            /// <summary>
            /// Total de publicaciones creadas por el usuario
            /// </summary>
            [JsonPropertyName("total_publicaciones")]
            public int TotalPublicaciones { get; set; }

            /// <summary>
            /// Total de revisiones realizadas por el usuario
            /// </summary>
            [JsonPropertyName("total_revisiones")]
            public int TotalRevisiones { get; set; }
        }

        #endregion

        #region Modelos para actualización de información básica

        /// <summary>
        /// Modelo para actualizar la información básica de un perfil administrativo
        /// </summary>
        public class ActualizacionInfoBasica
        {
            /// <summary>
            /// Nuevo nombre completo del usuario
            /// </summary>
            public string Nombre { get; set; }

            /// <summary>
            /// Nuevo cargo o nombramiento del usuario
            /// </summary>
            public string Nombramiento { get; set; }
        }

        /// <summary>
        /// Respuesta a la actualización de información básica
        /// </summary>
        public class RespuestaActualizacionInfoBasica
        {
            /// <summary>
            /// Estado de la operación (success/error)
            /// </summary>
            public string Status { get; set; }

            /// <summary>
            /// Mensaje descriptivo del resultado
            /// </summary>
            public string Mensaje { get; set; }
        }

        #endregion

        #region Modelos para actualización de foto de perfil

        /// <summary>
        /// Respuesta a la actualización de la foto de perfil
        /// </summary>
        public class RespuestaActualizacionFoto
        {
            /// <summary>
            /// Estado de la operación (success/error)
            /// </summary>
            public string Status { get; set; }

            /// <summary>
            /// Mensaje descriptivo del resultado
            /// </summary>
            public string Mensaje { get; set; }

            /// <summary>
            /// Datos de la foto actualizada
            /// </summary>
            public DatosFoto Datos { get; set; }
        }

        /// <summary>
        /// Datos de la foto actualizada
        /// </summary>
        public class DatosFoto
        {
            /// <summary>
            /// Nombre del archivo de la foto
            /// </summary>
            public string Filename { get; set; }

            /// <summary>
            /// URL completa de la foto
            /// </summary>
            public string Url { get; set; }
        }

        #endregion

        #region Modelos para cambio de contraseña

        /// <summary>
        /// Modelo para solicitar el cambio de contraseña
        /// </summary>
        public class CambioContrasena
        {
            /// <summary>
            /// Nueva contraseña del usuario
            /// </summary>
            public string NuevaContrasena { get; set; }
        }

        /// <summary>
        /// Respuesta al cambio de contraseña
        /// </summary>
        public class RespuestaCambioContrasena
        {
            /// <summary>
            /// Estado de la operación (success/error)
            /// </summary>
            public string Status { get; set; }

            /// <summary>
            /// Mensaje descriptivo del resultado
            /// </summary>
            public string Mensaje { get; set; }
        }

        #endregion

        #region Modelos para errores comunes

        /// <summary>
        /// Modelo para respuesta de error genérico
        /// </summary>
        public class RespuestaError
        {
            /// <summary>
            /// Estado de la operación (siempre "error" en este caso)
            /// </summary>
            public string Status { get; set; } = "error";

            /// <summary>
            /// Mensaje descriptivo del error
            /// </summary>
            public string Mensaje { get; set; }
        }

        #endregion
    }
}