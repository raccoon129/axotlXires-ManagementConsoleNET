using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DAL.Modelos
{
    #region Modelos para listado de usuarios

    /// <summary>
    /// Modelo principal para la respuesta de listado de usuarios
    /// </summary>
    public class RespuestaListadoUsuarios
    {
        public string Status { get; set; }
        public List<UsuarioListado> Datos { get; set; }
    }

    /// <summary>
    /// Información básica de un usuario para mostrar en listados
    /// </summary>
    public class UsuarioListado
    {
        [JsonPropertyName("id_usuario")]
        public int IdUsuario { get; set; }

        public string Nombre { get; set; }

        public string Correo { get; set; }

        public string Rol { get; set; }

        public string Nombramiento { get; set; }

        [JsonPropertyName("foto_perfil")]
        public string FotoPerfil { get; set; }

        [JsonPropertyName("fecha_creacion")]
        public DateTime FechaCreacion { get; set; }

        [JsonPropertyName("ultimo_acceso")]
        public String? UltimoAcceso { get; set; }

        public EstadisticasUsuario Estadisticas { get; set; }

        /// <summary>
        /// Estado de activación del usuario (1: activo, 0: inactivo)
        /// </summary>
        public int Activo { get; set; } = 1;
    }

    /// <summary>
    /// Estadísticas básicas de actividad de un usuario
    /// </summary>
    public class EstadisticasUsuario
    {
        public PublicacionesUsuario Publicaciones { get; set; }

        public int Comentarios { get; set; }

        public int Favoritos { get; set; }
    }

    /// <summary>
    /// Información de publicaciones de un usuario
    /// </summary>
    public class PublicacionesUsuario
    {
        [JsonPropertyName("total_publicaciones")]
        public int TotalPublicaciones { get; set; }

        public int Publicadas { get; set; }

        [JsonPropertyName("en_revision")]
        public int EnRevision { get; set; }
    }
    #endregion

    #region Modelos para detalle de usuario

    /// <summary>
    /// Modelo principal para la respuesta de detalle de usuario
    /// </summary>
    public class RespuestaDetalleUsuario
    {
        public string Status { get; set; }
        public UsuarioDetalle Datos { get; set; }
    }

    /// <summary>
    /// Información detallada de un usuario individual
    /// </summary>
    public class UsuarioDetalle
    {
        [JsonPropertyName("id_usuario")]
        public int IdUsuario { get; set; }

        public string Nombre { get; set; }

        public string Correo { get; set; }

        public string Rol { get; set; }

        public string Nombramiento { get; set; }

        [JsonPropertyName("foto_perfil")]
        public string FotoPerfil { get; set; }

        [JsonPropertyName("fecha_creacion")]
        public DateTime FechaCreacion { get; set; }

        [JsonPropertyName("ultimo_acceso")]
        public DateTime UltimoAcceso { get; set; }

        public EstadisticasDetalladasUsuario Estadisticas { get; set; }

        public List<PublicacionUsuario> Publicaciones { get; set; }

        [JsonPropertyName("comentarios_recientes")]
        public List<ComentarioReciente> ComentariosRecientes { get; set; }

        [JsonPropertyName("notificaciones_recientes")]
        public List<NotificacionUsuario> NotificacionesRecientes { get; set; }

        /// <summary>
        /// Estado de activación del usuario (1: activo, 0: inactivo)
        /// </summary>
        public int Activo { get; set; } = 1;
    }

    /// <summary>
    /// Estadísticas detalladas de actividad de un usuario
    /// </summary>
    public class EstadisticasDetalladasUsuario
    {
        public PublicacionesDetalladasUsuario Publicaciones { get; set; }

        [JsonPropertyName("total_comentarios")]
        public int TotalComentarios { get; set; }

        [JsonPropertyName("total_notificaciones")]
        public int TotalNotificaciones { get; set; }

        [JsonPropertyName("ultimo_acceso")]
        public DateTime UltimoAcceso { get; set; }
    }

    /// <summary>
    /// Información detallada de publicaciones de un usuario
    /// </summary>
    public class PublicacionesDetalladasUsuario
    {
        [JsonPropertyName("total_publicaciones")]
        public int TotalPublicaciones { get; set; }

        public int Publicadas { get; set; }

        [JsonPropertyName("pendientes_revision")]
        public int PendientesRevision { get; set; }

        public int Rechazadas { get; set; }

        public int Borradores { get; set; }
    }

    /// <summary>
    /// Información de una publicación específica de un usuario
    /// </summary>
    public class PublicacionUsuario
    {
        [JsonPropertyName("id_publicacion")]
        public int IdPublicacion { get; set; }

        public string Titulo { get; set; }

        public string Resumen { get; set; }

        public string Estado { get; set; }

        [JsonPropertyName("fecha_creacion")]
        public DateTime FechaCreacion { get; set; }

        [JsonPropertyName("fecha_publicacion")]
        public DateTime? FechaPublicacion { get; set; }
    }

    /// <summary>
    /// Información de un comentario reciente del usuario
    /// </summary>
    public class ComentarioReciente
    {
        [JsonPropertyName("id_comentario")]
        public int IdComentario { get; set; }

        public string Contenido { get; set; }

        [JsonPropertyName("fecha_creacion")]
        public DateTime FechaCreacion { get; set; }

        [JsonPropertyName("id_publicacion")]
        public int IdPublicacion { get; set; }

        [JsonPropertyName("titulo_publicacion")]
        public string TituloPublicacion { get; set; }
    }
    #endregion

    #region Modelos para notificaciones de usuario

    /// <summary>
    /// Modelo principal para la respuesta de notificaciones de usuario
    /// </summary>
    public class RespuestaNotificacionesUsuario
    {
        public string Status { get; set; }
        public List<NotificacionUsuario> Datos { get; set; }
    }

    /// <summary>
    /// Información de una notificación de usuario
    /// </summary>
    public class NotificacionUsuario
    {
        [JsonPropertyName("id_notificacion")]
        public int IdNotificacion { get; set; }

        public string Tipo { get; set; }

        public string Contenido { get; set; }

        public int Leida { get; set; }

        [JsonPropertyName("fecha_creacion")]
        public DateTime FechaCreacion { get; set; }

        [JsonPropertyName("nombre_origen")]
        public string NombreOrigen { get; set; }
    }
    #endregion

    #region Modelos para cambio de rol

    /// <summary>
    /// Modelo para solicitar cambio de rol
    /// </summary>
    public class CambioRolUsuario
    {
        public string Rol { get; set; }
    }

    /// <summary>
    /// Respuesta genérica para operaciones exitosas o fallidas
    /// </summary>
    public class RespuestaGeneral
    {
        public string Status { get; set; }
        public string Mensaje { get; set; }
        public object Datos { get; set; }
    }
    #endregion

    #region Modelos para reseteo de contraseña

    /// <summary>
    /// Modelo para solicitar reseteo de contraseña
    /// </summary>
    public class ReseteoContrasena
    {
        public string NuevaContrasena { get; set; }
    }
    #endregion

    #region Modelos para creación de usuario

    /// <summary>
    /// Modelo para crear un nuevo usuario
    /// </summary>
    public class NuevoUsuario
    {
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Contrasena { get; set; }
        public string Rol { get; set; }
        public string Nombramiento { get; set; }
    }

    /// <summary>
    /// Respuesta después de crear un nuevo usuario
    /// </summary>
    public class RespuestaNuevoUsuario
    {
        public string Status { get; set; }
        public string Mensaje { get; set; }
        public DatosNuevoUsuario Datos { get; set; }
    }

    /// <summary>
    /// Datos básicos del usuario recién creado
    /// </summary>
    public class DatosNuevoUsuario
    {
        [JsonPropertyName("id_usuario")]
        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Rol { get; set; }
    }
    #endregion

    #region Modelos para activación/desactivación de usuario

    /// <summary>
    /// Modelo para solicitar cambio de estado de activación
    /// </summary>
    public class CambioActivacionUsuario
    {
        /// <summary>
        /// Estado de activación: 1 para activar, 0 para desactivar
        /// </summary>
        public int Activo { get; set; }
    }

    /// <summary>
    /// Respuesta a la activación/desactivación de usuario
    /// </summary>
    public class RespuestaActivacionUsuario
    {
        public string Status { get; set; }
        public string Mensaje { get; set; }
        public DatosActivacionUsuario Datos { get; set; }
    }

    /// <summary>
    /// Datos de la operación de activación/desactivación
    /// </summary>
    public class DatosActivacionUsuario
    {
        [JsonPropertyName("id_usuario")]
        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        public int Activo { get; set; }
    }
    #endregion

    #region Modelos para actualización de detalles de usuario

    /// <summary>
    /// Modelo para actualizar los detalles de un usuario
    /// </summary>
    public class ActualizacionDetallesUsuario
    {
        public string Nombre { get; set; }
        public string Nombramiento { get; set; }
        public string Rol { get; set; }
    }

    /// <summary>
    /// Respuesta a la actualización de detalles de usuario
    /// </summary>
    public class RespuestaActualizacionDetalles
    {
        public string Status { get; set; }
        public string Mensaje { get; set; }
        public DatosActualizacionUsuario Datos { get; set; }
    }

    /// <summary>
    /// Datos de la operación de actualización de detalles
    /// </summary>
    public class DatosActualizacionUsuario
    {
        [JsonPropertyName("id_usuario")]
        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string Nombramiento { get; set; }
        public string Rol { get; set; }
    }
    #endregion

    //Nueva iteracion
    #region Modelos para comentarios detallados de un usuario

    /// <summary>
    /// Respuesta a la consulta de comentarios detallados de un usuario
    /// </summary>
    public class RespuestaComentariosUsuario
    {
        public string Status { get; set; }
        public string Mensaje { get; set; }
        public DatosComentariosUsuario Datos { get; set; }
    }

    /// <summary>
    /// Datos de la consulta de comentarios de un usuario
    /// </summary>
    public class DatosComentariosUsuario
    {
        public UsuarioBasico Usuario { get; set; }

        [JsonPropertyName("total_comentarios")]
        public int TotalComentarios { get; set; }

        public List<ComentarioDetallado> Comentarios { get; set; }
    }

    /// <summary>
    /// Información básica de un usuario para respuestas
    /// </summary>
    public class UsuarioBasico
    {
        [JsonPropertyName("id_usuario")]
        public int IdUsuario { get; set; }

        public string Nombre { get; set; }
    }

    /// <summary>
    /// Información detallada de un comentario
    /// </summary>
    public class ComentarioDetallado
    {
        [JsonPropertyName("id_comentario")]
        public int IdComentario { get; set; }

        [JsonPropertyName("id_publicacion")]
        public int IdPublicacion { get; set; }

        public string Contenido { get; set; }

        [JsonPropertyName("fecha_creacion")]
        public DateTime FechaCreacion { get; set; }

        [JsonPropertyName("titulo_publicacion")]
        public string TituloPublicacion { get; set; }

        [JsonPropertyName("autor_publicacion")]
        public int AutorPublicacion { get; set; }
    }
    #endregion

    #region Modelos para notificaciones con estado de atención

    /// <summary>
    /// Respuesta a la consulta de notificaciones con estado de atención
    /// </summary>
    public class RespuestaNotificacionesEstadoUsuario
    {
        public string Status { get; set; }
        public string Mensaje { get; set; }
        public DatosNotificacionesEstado Datos { get; set; }
    }

    /// <summary>
    /// Datos de notificaciones con estado de atención
    /// </summary>
    public class DatosNotificacionesEstado
    {
        public UsuarioBasico Usuario { get; set; }
        public EstadisticasNotificaciones Estadisticas { get; set; }
        public List<NotificacionConEstado> Notificaciones { get; set; }
    }

    /// <summary>
    /// Estadísticas de notificaciones
    /// </summary>
    public class EstadisticasNotificaciones
    {
        public int Total { get; set; }
        public int Atendidas { get; set; }
        public int Pendientes { get; set; }
    }

    /// <summary>
    /// Notificación con estado de atención
    /// </summary>
    public class NotificacionConEstado : NotificacionUsuario
    {
        [JsonPropertyName("id_usuario")]
        public int IdUsuario { get; set; }

        [JsonPropertyName("id_origen")]
        public int IdOrigen { get; set; }

        [JsonPropertyName("tipo_referencia")]
        public string TipoReferencia { get; set; }

        [JsonPropertyName("estado_atencion")]
        public string EstadoAtencion { get; set; }
    }
    #endregion

    #region Modelos para restablecimiento de foto de perfil

    /// <summary>
    /// Respuesta al restablecimiento de foto de perfil
    /// </summary>
    public class RespuestaRestablecerFotoPerfil
    {
        public string Status { get; set; }
        public string Mensaje { get; set; }
        public DatosRestablecerFotoPerfil Datos { get; set; }
    }

    /// <summary>
    /// Datos del restablecimiento de foto de perfil
    /// </summary>
    public class DatosRestablecerFotoPerfil
    {
        [JsonPropertyName("id_usuario")]
        public int IdUsuario { get; set; }

        public string Nombre { get; set; }

        [JsonPropertyName("foto_perfil")]
        public string FotoPerfil { get; set; }
    }
    #endregion

    #region Modelos para eliminación de comentarios

    /// <summary>
    /// Respuesta a la eliminación de un comentario
    /// </summary>
    public class RespuestaEliminarComentario
    {
        public string Status { get; set; }
        public string Mensaje { get; set; }
        public DatosComentarioEliminado Datos { get; set; }
    }

    /// <summary>
    /// Datos del comentario eliminado
    /// </summary>
    public class DatosComentarioEliminado
    {
        [JsonPropertyName("id_comentario")]
        public int IdComentario { get; set; }

        [JsonPropertyName("id_publicacion")]
        public int IdPublicacion { get; set; }

        [JsonPropertyName("id_usuario_afectado")]
        public int IdUsuarioAfectado { get; set; }
    }
    #endregion


}
