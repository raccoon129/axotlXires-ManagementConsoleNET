﻿// DAL/Modelos/ModeloPublicaciones.cs
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace DAL.Modelos
{
    #region Constantes de estados de publicación

    /// <summary>
    /// Constantes para los posibles estados de una publicación
    /// </summary>
    public static class EstadoPublicacion
    {
        public const string Borrador = "borrador";
        public const string EnRevision = "en_revision";
        public const string SolicitaCambios = "solicita_cambios";
        public const string Publicado = "publicado";
        public const string Rechazado = "rechazado";
    }

    #endregion

    #region Constantes de decisiones de revisión

    /// <summary>
    /// Constantes para las posibles decisiones de una revisión
    /// </summary>
    public static class DecisionRevision
    {
        public const string Aprobar = "aprobar";
        public const string SolicitarCambios = "solicitar_cambios";
        public const string Rechazar = "rechazar";
    }

    #endregion

    #region Modelos para listado de publicaciones pendientes

    /// <summary>
    /// Modelo principal para la respuesta de listado de publicaciones pendientes
    /// </summary>
    public class RespuestaPublicacionesPendientes
    {
        public string Status { get; set; }
        public List<PublicacionPendiente> Datos { get; set; }
    }

    /// <summary>
    /// Información básica de una publicación pendiente de revisión
    /// </summary>
    public class PublicacionPendiente
    {
        [JsonPropertyName("id_publicacion")]
        public int IdPublicacion { get; set; }

        public string Titulo { get; set; }

        public string Resumen { get; set; }

        [JsonPropertyName("fecha_creacion")]
        public DateTime FechaCreacion { get; set; }

        [JsonPropertyName("tipo_publicacion")]
        public string TipoPublicacion { get; set; }

        public string Autor { get; set; }

        [JsonPropertyName("id_autor")]
        public int IdAutor { get; set; }

        [JsonPropertyName("total_revisiones")]
        public int TotalRevisiones { get; set; }

        [JsonPropertyName("ultima_revision")]
        public object UltimaRevision { get; set; }
    }


    /// <summary>
    /// Datos básicos de una revisión para mostrar en listado
    /// </summary>
    public class RevisionBasica
    {
        [JsonPropertyName("id_revision")]
        public int IdRevision { get; set; }

        [JsonPropertyName("fecha_creacion")]
        public DateTime FechaCreacion { get; set; }

        [JsonPropertyName("tipo_revision")]
        public string TipoRevision { get; set; }

        public string Revisor { get; set; }
    }
    #endregion

    #region Modelos para detalle de publicación

    /// <summary>
    /// Modelo principal para la respuesta de detalle de publicación
    /// </summary>
    public class RespuestaDetallePublicacion
    {
        public string Status { get; set; }
        public DetallePublicacion Datos { get; set; }
    }

    /// <summary>
    /// Datos completos de la publicación y sus revisiones
    /// </summary>
    public class DetallePublicacion
    {
        public PublicacionCompleta Publicacion { get; set; }
        public List<RevisionCompleta> Revisiones { get; set; }
        public List<TipoRevision> TiposRevision { get; set; }
    }

    /// <summary>
    /// Información completa de una publicación
    /// </summary>
    public class PublicacionCompleta
    {
        [JsonPropertyName("id_publicacion")]
        public int IdPublicacion { get; set; }

        [JsonPropertyName("id_usuario")]
        public int IdUsuario { get; set; }

        [JsonPropertyName("id_tipo")]
        public int IdTipo { get; set; }

        public string Titulo { get; set; }

        public string Resumen { get; set; }

        public string Contenido { get; set; }

        public string Referencias { get; set; }

        public string Estado { get; set; }

        [JsonPropertyName("imagen_portada")]
        public string ImagenPortada { get; set; }

        [JsonPropertyName("es_privada")]
        [JsonConverter(typeof(IntToBoolConverter))]
        public bool EsPrivada { get; set; }

        [JsonPropertyName("fecha_creacion")]
        public DateTime FechaCreacion { get; set; }

        [JsonPropertyName("fecha_publicacion")]
        public DateTime? FechaPublicacion { get; set; }

        [JsonPropertyName("eliminado")]
        [JsonConverter(typeof(IntToBoolConverter))]
        public bool Eliminado { get; set; }

        [JsonPropertyName("fecha_eliminacion")]
        public DateTime? FechaEliminacion { get; set; }

        public string Autor { get; set; }

        [JsonPropertyName("id_autor")]
        public int IdAutor { get; set; }

        [JsonPropertyName("nombramiento_autor")]
        public string NombramientoAutor { get; set; }

        [JsonPropertyName("tipo_publicacion")]
        public string TipoPublicacion { get; set; }
    }

    /// <summary>
    /// Información completa de una revisión
    /// </summary>
    public class RevisionCompleta
    {
        [JsonPropertyName("id_revision")]
        public int IdRevision { get; set; }

        public bool? Aprobado { get; set; }

        [JsonPropertyName("fecha_creacion")]
        public DateTime FechaCreacion { get; set; }

        [JsonPropertyName("tipo_revision")]
        public string TipoRevision { get; set; }

        [JsonPropertyName("descripcion_revision")]
        public string DescripcionRevision { get; set; }

        public string Revisor { get; set; }

        [JsonPropertyName("id_revisor")]
        public int IdRevisor { get; set; }

        public List<ComentarioRevision> Comentarios { get; set; }
    }

    /// <summary>
    /// Información de un comentario de revisión
    /// </summary>
    public class ComentarioRevision
    {
        [JsonPropertyName("id_comentario")]
        public int IdComentario { get; set; }

        public string Contenido { get; set; }

        [JsonPropertyName("fecha_creacion")]
        public DateTime FechaCreacion { get; set; }

        [JsonPropertyName("autor_comentario")]
        public string AutorComentario { get; set; }

        [JsonPropertyName("id_usuario")]
        public int IdUsuario { get; set; }
    }

    /// <summary>
    /// Información de un detalle de revisión (antes llamado tipo de revisión)
    /// </summary>
    public class TipoRevision
    {
        [JsonPropertyName("id_detalle_revision")]
        public int IdDetalleRevision { get; set; }

        [JsonPropertyName("detalle_revision")]
        public string DetalleRevision { get; set; }

        [JsonPropertyName("descripcion_revision")]
        public string DescripcionRevision { get; set; }
    }

    #endregion

    #region Modelos para operaciones de revisión

    /// <summary>
    /// Modelo para agregar un comentario a una revisión
    /// </summary>
    public class NuevoComentario
    {
        public string Contenido { get; set; }
    }

    /// <summary>
    /// Modelo para el proceso unificado de revisión
    /// </summary>
    public class ProcesoRevision
    {
        /// <summary>
        /// Detalle de la revisión (nombre/tipo)
        /// </summary>
        [JsonPropertyName("detalle_revision")]
        public string DetalleRevision { get; set; }

        /// <summary>
        /// Descripción de la revisión
        /// </summary>
        [JsonPropertyName("descripcion_revision")]
        public string DescripcionRevision { get; set; }

        /// <summary>
        /// Comentario de retroalimentación
        /// </summary>
        public string Comentario { get; set; }

        /// <summary>
        /// Decisión tomada: "aprobar", "solicitar_cambios" o "rechazar"
        /// </summary>
        public string Decision { get; set; }
    }

    /// <summary>
    /// Respuesta al ejecutar el proceso unificado de revisión
    /// </summary>
    public class RespuestaProcesoRevision
    {
        /// <summary>
        /// Estado de la respuesta: "success" o "error"
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Mensaje descriptivo del resultado
        /// </summary>
        public string Mensaje { get; set; }

        /// <summary>
        /// Datos del resultado del proceso
        /// </summary>
        public DatosProcesoRevision Datos { get; set; }
    }

    /// <summary>
    /// Datos del resultado del proceso de revisión
    /// </summary>
    public class DatosProcesoRevision
    {
        /// <summary>
        /// ID de la revisión creada
        /// </summary>
        [JsonPropertyName("id_revision")]
        public int IdRevision { get; set; }

        /// <summary>
        /// Detalle de la revisión
        /// </summary>
        [JsonPropertyName("detalle_revision")]
        public string DetalleRevision { get; set; }

        /// <summary>
        /// Descripción de la revisión
        /// </summary>
        [JsonPropertyName("descripcion_revision")]
        public string DescripcionRevision { get; set; }

        /// <summary>
        /// Nuevo estado de la publicación tras la revisión
        /// </summary>
        [JsonPropertyName("nuevo_estado")]
        public string NuevoEstado { get; set; }
    }

    /// <summary>
    /// Respuesta al agregar un comentario
    /// </summary>
    public class RespuestaComentario
    {
        public string Status { get; set; }
        public string Mensaje { get; set; }
        public DatosComentarioCreado Datos { get; set; }
    }

    /// <summary>
    /// Datos del comentario creado
    /// </summary>
    public class DatosComentarioCreado
    {
        [JsonPropertyName("id_comentario")]
        public int IdComentario { get; set; }
    }

    #endregion

    /// <summary>
    /// Convertidor personalizado que permite convertir valores enteros (0/1) a booleanos
    /// </summary>
    public class IntToBoolConverter : JsonConverter<bool>
    {
        public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Manejar diferentes tipos de valores que podría devolver la API
            if (reader.TokenType == JsonTokenType.True || reader.TokenType == JsonTokenType.False)
            {
                return reader.GetBoolean(); // Si ya es un booleano, simplemente devolverlo
            }
            else if (reader.TokenType == JsonTokenType.Number)
            {
                return reader.GetInt32() != 0; // Si es un número, 0 = false, todo lo demás = true
            }
            else if (reader.TokenType == JsonTokenType.String)
            {
                string value = reader.GetString().ToLower();
                // Convertir cadenas como "true", "1", "yes" a true
                return value == "true" || value == "1" || value == "yes" || value == "t" || value == "y";
            }
            else
            {
                // Si no es nada reconocible, asumir false por defecto
                return false;
            }
        }

        public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
        {
            // Al serializar, simplemente escribir el valor booleano
            writer.WriteBooleanValue(value);
        }
    }
}