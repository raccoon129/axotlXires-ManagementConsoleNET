using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using DAL.Modelos;

namespace DAL.Consumo
{
    /// <summary>
    /// Clase para gestionar las operaciones con tipos de publicación
    /// </summary>
    public class tiposPublicacion
    {
        private const string BASE_ENDPOINT = "/api/management/tipos-publicacion";

        /// <summary>
        /// Obtiene todos los tipos de publicación
        /// </summary>
        /// <param name="token">Token JWT de autenticación (opcional)</param>
        /// <returns>Lista de tipos de publicación o null si ocurre un error</returns>
        public static async Task<List<TipoPublicacion>> ObtenerTiposPublicacionAsync(string token = null)
        {
            try
            {
                using var client = new HttpClient();
                client.BaseAddress = new Uri(Parametros.UrlBaseApi);

                // Añadir token de autenticación si está disponible
                if (!string.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                HttpResponseMessage respuesta = await client.GetAsync(BASE_ENDPOINT);

                if (respuesta.IsSuccessStatusCode)
                {
                    var contenido = await respuesta.Content.ReadFromJsonAsync<RespuestaListaTiposPublicacion>();

                    if (contenido?.Status == "success" && contenido.Datos != null)
                    {
                        return contenido.Datos;
                    }
                }

                Console.WriteLine($"Error al obtener tipos de publicación. Código: {respuesta.StatusCode}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Excepción al obtener tipos de publicación: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Obtiene un tipo de publicación específico por su ID
        /// </summary>
        /// <param name="idTipo">ID del tipo de publicación</param>
        /// <param name="token">Token JWT de autenticación (opcional)</param>
        /// <returns>Tipo de publicación o null si no se encuentra</returns>
        public static async Task<TipoPublicacion> ObtenerTipoPublicacionPorIdAsync(int idTipo, string token = null)
        {
            if (idTipo <= 0)
            {
                Console.WriteLine("ID de tipo de publicación inválido");
                return null;
            }

            try
            {
                using var client = new HttpClient();
                client.BaseAddress = new Uri(Parametros.UrlBaseApi);

                if (!string.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                HttpResponseMessage respuesta = await client.GetAsync($"{BASE_ENDPOINT}/{idTipo}");

                if (respuesta.IsSuccessStatusCode)
                {
                    var contenido = await respuesta.Content.ReadFromJsonAsync<RespuestaDetalleTipoPublicacion>();

                    if (contenido?.Status == "success" && contenido.Datos != null)
                    {
                        return contenido.Datos;
                    }
                }

                Console.WriteLine($"Error al obtener tipo de publicación. Código: {respuesta.StatusCode}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Excepción al obtener tipo de publicación: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Crea un nuevo tipo de publicación
        /// </summary>
        /// <param name="token">Token JWT de autenticación</param>
        /// <param name="nuevoTipo">Datos del nuevo tipo de publicación</param>
        /// <returns>Tipo de publicación creado o null si hay error</returns>
        public static async Task<RespuestaDetalleTipoPublicacion> CrearTipoPublicacionAsync(string token, NuevoTipoPublicacion nuevoTipo)
        {
            if (string.IsNullOrEmpty(token))
            {
                return new RespuestaDetalleTipoPublicacion
                {
                    Status = "error",
                    Mensaje = "Token de autenticación no proporcionado"
                };
            }

            if (nuevoTipo == null || string.IsNullOrWhiteSpace(nuevoTipo.Nombre) || string.IsNullOrWhiteSpace(nuevoTipo.Descripcion))
            {
                return new RespuestaDetalleTipoPublicacion
                {
                    Status = "error",
                    Mensaje = "Los datos del tipo de publicación son incompletos"
                };
            }

            try
            {
                using var client = new HttpClient();
                client.BaseAddress = new Uri(Parametros.UrlBaseApi);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage respuesta = await client.PostAsJsonAsync(BASE_ENDPOINT, nuevoTipo);

                var contenidoRespuesta = await respuesta.Content.ReadFromJsonAsync<RespuestaDetalleTipoPublicacion>();

                if (respuesta.IsSuccessStatusCode && contenidoRespuesta?.Status == "success")
                {
                    return contenidoRespuesta;
                }
                else
                {
                    // Si ya tenemos una respuesta con mensaje de error, la devolvemos
                    if (contenidoRespuesta != null)
                    {
                        return contenidoRespuesta;
                    }

                    // Si no, creamos un mensaje genérico
                    return new RespuestaDetalleTipoPublicacion
                    {
                        Status = "error",
                        Mensaje = $"Error al crear tipo de publicación. Código: {respuesta.StatusCode}"
                    };
                }
            }
            catch (Exception ex)
            {
                return new RespuestaDetalleTipoPublicacion
                {
                    Status = "error",
                    Mensaje = $"Excepción al crear tipo de publicación: {ex.Message}"
                };
            }
        }

        /// <summary>
        /// Actualiza un tipo de publicación existente
        /// </summary>
        /// <param name="token">Token JWT de autenticación</param>
        /// <param name="idTipo">ID del tipo de publicación a actualizar</param>
        /// <param name="actualizarTipo">Datos a actualizar</param>
        /// <returns>Tipo de publicación actualizado o null si hay error</returns>
        public static async Task<RespuestaDetalleTipoPublicacion> ActualizarTipoPublicacionAsync(string token, int idTipo, ActualizarTipoPublicacion actualizarTipo)
        {
            if (string.IsNullOrEmpty(token))
            {
                return new RespuestaDetalleTipoPublicacion
                {
                    Status = "error",
                    Mensaje = "Token de autenticación no proporcionado"
                };
            }

            if (idTipo <= 0)
            {
                return new RespuestaDetalleTipoPublicacion
                {
                    Status = "error",
                    Mensaje = "ID de tipo de publicación inválido"
                };
            }

            if (actualizarTipo == null || (string.IsNullOrWhiteSpace(actualizarTipo.Nombre) && string.IsNullOrWhiteSpace(actualizarTipo.Descripcion)))
            {
                return new RespuestaDetalleTipoPublicacion
                {
                    Status = "error",
                    Mensaje = "Debe proporcionar al menos un campo para actualizar (nombre o descripción)"
                };
            }

            try
            {
                using var client = new HttpClient();
                client.BaseAddress = new Uri(Parametros.UrlBaseApi);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage respuesta = await client.PutAsJsonAsync($"{BASE_ENDPOINT}/{idTipo}", actualizarTipo);

                var contenidoRespuesta = await respuesta.Content.ReadFromJsonAsync<RespuestaDetalleTipoPublicacion>();

                if (respuesta.IsSuccessStatusCode && contenidoRespuesta?.Status == "success")
                {
                    return contenidoRespuesta;
                }
                else
                {
                    if (contenidoRespuesta != null)
                    {
                        return contenidoRespuesta;
                    }

                    return new RespuestaDetalleTipoPublicacion
                    {
                        Status = "error",
                        Mensaje = $"Error al actualizar tipo de publicación. Código: {respuesta.StatusCode}"
                    };
                }
            }
            catch (Exception ex)
            {
                return new RespuestaDetalleTipoPublicacion
                {
                    Status = "error",
                    Mensaje = $"Excepción al actualizar tipo de publicación: {ex.Message}"
                };
            }
        }

        /// <summary>
        /// Elimina un tipo de publicación
        /// </summary>
        /// <param name="token">Token JWT de autenticación</param>
        /// <param name="idTipo">ID del tipo de publicación a eliminar</param>
        /// <returns>Resultado de la operación de eliminación</returns>
        public static async Task<RespuestaEliminacionTipoPublicacion> EliminarTipoPublicacionAsync(string token, int idTipo)
        {
            if (string.IsNullOrEmpty(token))
            {
                return new RespuestaEliminacionTipoPublicacion
                {
                    Status = "error",
                    Mensaje = "Token de autenticación no proporcionado"
                };
            }

            if (idTipo <= 0)
            {
                return new RespuestaEliminacionTipoPublicacion
                {
                    Status = "error",
                    Mensaje = "ID de tipo de publicación inválido"
                };
            }

            try
            {
                using var client = new HttpClient();
                client.BaseAddress = new Uri(Parametros.UrlBaseApi);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage respuesta = await client.DeleteAsync($"{BASE_ENDPOINT}/{idTipo}");

                var contenidoRespuesta = await respuesta.Content.ReadFromJsonAsync<RespuestaEliminacionTipoPublicacion>();

                if (respuesta.IsSuccessStatusCode && contenidoRespuesta?.Status == "success")
                {
                    return contenidoRespuesta;
                }
                else
                {
                    if (contenidoRespuesta != null)
                    {
                        return contenidoRespuesta;
                    }

                    return new RespuestaEliminacionTipoPublicacion
                    {
                        Status = "error",
                        Mensaje = $"Error al eliminar tipo de publicación. Código: {respuesta.StatusCode}"
                    };
                }
            }
            catch (Exception ex)
            {
                return new RespuestaEliminacionTipoPublicacion
                {
                    Status = "error",
                    Mensaje = $"Excepción al eliminar tipo de publicación: {ex.Message}"
                };
            }
        }

        /// <summary>
        /// Obtiene estadísticas de uso de tipos de publicación
        /// </summary>
        /// <param name="token">Token JWT de autenticación</param>
        /// <returns>Lista de estadísticas de uso por tipo de publicación</returns>
        public static async Task<List<EstadisticaTipoPublicacion>> ObtenerEstadisticasUsoAsync(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine("Token de autenticación no proporcionado");
                return null;
            }

            try
            {
                using var client = new HttpClient();
                client.BaseAddress = new Uri(Parametros.UrlBaseApi);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage respuesta = await client.GetAsync($"{BASE_ENDPOINT}/estadisticas/uso");

                if (respuesta.IsSuccessStatusCode)
                {
                    var contenido = await respuesta.Content.ReadFromJsonAsync<RespuestaEstadisticasTiposPublicacion>();

                    if (contenido?.Status == "success" && contenido.Datos != null)
                    {
                        return contenido.Datos;
                    }
                }

                Console.WriteLine($"Error al obtener estadísticas de uso. Código: {respuesta.StatusCode}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Excepción al obtener estadísticas de uso: {ex.Message}");
                return null;
            }
        }
    }
}
