using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DAL.Modelos
{
    #region Modelos para respuestas API

    /// <summary>
    /// Respuesta genérica para operaciones con tipos de publicación
    /// </summary>
    public class RespuestaTipoPublicacion
    {
        /// <summary>
        /// Estado de la respuesta: "success" o "error"
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Mensaje descriptivo de la operación (opcional)
        /// </summary>
        public string Mensaje { get; set; }

        /// <summary>
        /// Datos de la respuesta (opcional)
        /// </summary>
        public object Datos { get; set; }
    }

    /// <summary>
    /// Respuesta para listar todos los tipos de publicación
    /// </summary>
    public class RespuestaListaTiposPublicacion
    {
        /// <summary>
        /// Estado de la respuesta: "success" o "error"
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Lista de tipos de publicación
        /// </summary>
        public List<TipoPublicacion> Datos { get; set; }
    }

    /// <summary>
    /// Respuesta para operaciones individuales con tipos de publicación
    /// </summary>
    public class RespuestaDetalleTipoPublicacion
    {
        /// <summary>
        /// Estado de la respuesta: "success" o "error"
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Mensaje descriptivo de la operación (opcional)
        /// </summary>
        public string Mensaje { get; set; }

        /// <summary>
        /// Datos del tipo de publicación
        /// </summary>
        public TipoPublicacion Datos { get; set; }
    }

    /// <summary>
    /// Respuesta para la operación de eliminación
    /// </summary>
    public class RespuestaEliminacionTipoPublicacion
    {
        /// <summary>
        /// Estado de la respuesta: "success" o "error"
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Mensaje descriptivo de la operación
        /// </summary>
        public string Mensaje { get; set; }

        /// <summary>
        /// Datos de la eliminación
        /// </summary>
        public DatosEliminacionTipoPublicacion Datos { get; set; }
    }

    /// <summary>
    /// Datos de confirmación de eliminación
    /// </summary>
    public class DatosEliminacionTipoPublicacion
    {
        /// <summary>
        /// ID del tipo de publicación eliminado
        /// </summary>
        [JsonPropertyName("id_tipo")]
        public int IdTipo { get; set; }
    }

    /// <summary>
    /// Respuesta para estadísticas de uso de tipos de publicación
    /// </summary>
    public class RespuestaEstadisticasTiposPublicacion
    {
        /// <summary>
        /// Estado de la respuesta: "success" o "error"
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Lista de estadísticas de tipos de publicación
        /// </summary>
        public List<EstadisticaTipoPublicacion> Datos { get; set; }
    }

    #endregion

    #region Modelos de datos

    /// <summary>
    /// Modelo para un tipo de publicación
    /// </summary>
    public class TipoPublicacion
    {
        /// <summary>
        /// Identificador único del tipo de publicación
        /// </summary>
        [JsonPropertyName("id_tipo")]
        public int IdTipo { get; set; }

        /// <summary>
        /// Nombre del tipo de publicación
        /// </summary>
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
        public string Nombre { get; set; }

        /// <summary>
        /// Descripción detallada del tipo de publicación
        /// </summary>
        [Required(ErrorMessage = "La descripción es obligatoria")]
        [StringLength(500, ErrorMessage = "La descripción no puede exceder los 500 caracteres")]
        public string Descripcion { get; set; }
    }

    /// <summary>
    /// Modelo para registrar un nuevo tipo de publicación
    /// </summary>
    public class NuevoTipoPublicacion
    {
        /// <summary>
        /// Nombre del tipo de publicación
        /// </summary>
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
        public string Nombre { get; set; }

        /// <summary>
        /// Descripción detallada del tipo de publicación
        /// </summary>
        [Required(ErrorMessage = "La descripción es obligatoria")]
        [StringLength(500, ErrorMessage = "La descripción no puede exceder los 500 caracteres")]
        public string Descripcion { get; set; }
    }

    /// <summary>
    /// Modelo para actualizar un tipo de publicación existente
    /// </summary>
    public class ActualizarTipoPublicacion
    {
        /// <summary>
        /// Nombre del tipo de publicación (opcional para actualización)
        /// </summary>
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
        public string Nombre { get; set; }

        /// <summary>
        /// Descripción detallada del tipo de publicación (opcional para actualización)
        /// </summary>
        [StringLength(500, ErrorMessage = "La descripción no puede exceder los 500 caracteres")]
        public string Descripcion { get; set; }
    }

    /// <summary>
    /// Modelo para estadísticas de uso de un tipo de publicación
    /// </summary>
    public class EstadisticaTipoPublicacion : TipoPublicacion
    {
        /// <summary>
        /// Total de publicaciones que usan este tipo
        /// </summary>
        [JsonPropertyName("total_publicaciones")]
        public int TotalPublicaciones { get; set; }

        /// <summary>
        /// Número de publicaciones publicadas
        /// </summary>
        public int Publicadas { get; set; }

        /// <summary>
        /// Número de publicaciones en proceso de revisión
        /// </summary>
        [JsonPropertyName("en_revision")]
        public int EnRevision { get; set; }

        /// <summary>
        /// Número de publicaciones en estado borrador
        /// </summary>
        public int Borradores { get; set; }
    }

    #endregion
}
