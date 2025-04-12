using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DAL.Modelos
{
    /// <summary>
    /// Modelos para consultas de publicaciones por diferentes estados
    /// </summary>
    public class RespuestaConsultaPublicaciones
    {
        /// <summary>
        /// Estado de la respuesta ("success" o "error")
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Datos de las publicaciones
        /// </summary>
        public DatosConsultaPublicaciones Datos { get; set; }
    }

    /// <summary>
    /// Datos que contienen la lista de publicaciones
    /// </summary>
    public class DatosConsultaPublicaciones
    {
        /// <summary>
        /// Lista de publicaciones
        /// </summary>
        public List<PublicacionConsulta> Publicaciones { get; set; }
    }

    /// <summary>
    /// Modelo de datos para una publicación en consultas
    /// </summary>
    public class PublicacionConsulta
    {
        /// <summary>
        /// Identificador único de la publicación
        /// </summary>
        [JsonPropertyName("id_publicacion")]
        public int IdPublicacion { get; set; }

        /// <summary>
        /// Identificador del usuario creador
        /// </summary>
        [JsonPropertyName("id_usuario")]
        public int IdUsuario { get; set; }

        /// <summary>
        /// Identificador del tipo de publicación
        /// </summary>
        [JsonPropertyName("id_tipo")]
        public int IdTipo { get; set; }

        /// <summary>
        /// Título de la publicación
        /// </summary>
        public string Titulo { get; set; }

        /// <summary>
        /// Resumen o descripción breve de la publicación
        /// </summary>
        public string Resumen { get; set; }

        /// <summary>
        /// Estado actual de la publicación (borrador, en_revision, publicado, etc.)
        /// </summary>
        public string Estado { get; set; }

        /// <summary>
        /// Ruta de la imagen de portada (si existe)
        /// </summary>
        [JsonPropertyName("imagen_portada")]
        public string ImagenPortada { get; set; }

        /// <summary>
        /// Indica si la publicación es privada
        /// </summary>
        [JsonPropertyName("es_privada")]
        public int EsPrivada { get; set; }

        /// <summary>
        /// Fecha de creación de la publicación
        /// </summary>
        [JsonPropertyName("fecha_creacion")]
        public DateTime FechaCreacion { get; set; }

        /// <summary>
        /// Fecha en que se publicó (puede ser null)
        /// </summary>
        [JsonPropertyName("fecha_publicacion")]
        public DateTime? FechaPublicacion { get; set; }

        /// <summary>
        /// Nombre descriptivo del tipo de publicación
        /// </summary>
        [JsonPropertyName("tipos_publicacion")]
        public string TipoPublicacion { get; set; }

        /// <summary>
        /// Nombre del autor
        /// </summary>
        public string Autor { get; set; }

        /// <summary>
        /// Identificador del autor
        /// </summary>
        [JsonPropertyName("id_autor")]
        public int IdAutor { get; set; }

        /// <summary>
        /// Nombramiento o título académico del autor
        /// </summary>
        [JsonPropertyName("nombramiento_autor")]
        public string NombramientoAutor { get; set; }

        /// <summary>
        /// Total de comentarios en la publicación
        /// </summary>
        [JsonPropertyName("total_comentarios")]
        public int TotalComentarios { get; set; }

        /// <summary>
        /// Total de favoritos o "me gusta" en la publicación
        /// </summary>
        [JsonPropertyName("total_favoritos")]
        public int TotalFavoritos { get; set; }

        /// <summary>
        /// Propiedad auxiliar que permite determinar si es privada como un booleano
        /// </summary>
        [JsonIgnore]
        public bool EsPrivadaBool => EsPrivada != 0;
    }
}