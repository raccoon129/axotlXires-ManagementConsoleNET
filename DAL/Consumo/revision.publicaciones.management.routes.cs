// DAL/Consumo/revision.publicaciones.management.routes.cs
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
    /// Clase para gestionar las operaciones de revisión de publicaciones
    /// </summary>
    public class revisionPublicaciones
    {
        /// <summary>
        /// Obtiene el listado de publicaciones pendientes de revisión
        /// </summary>
        /// <param name="token">Token JWT de autenticación</param>
        /// <returns>Lista de publicaciones pendientes o null si ocurre un error</returns>
        public static async Task<List<PublicacionPendiente>> ObtenerPublicacionesPendientesAsync(string token)
        {
            // Validación básica del token
            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine("Error: Token no proporcionado para obtener publicaciones pendientes");
                return null;
            }

            try
            {
                // Crear cliente HTTP con la configuración necesaria
                using var client = new HttpClient();
                client.BaseAddress = new Uri(Parametros.UrlBaseApi);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // Realizar la petición GET
                HttpResponseMessage respuesta = await client.GetAsync("/api/management/revision/pendientes");

                // Verificar si la respuesta fue exitosa
                if (respuesta.IsSuccessStatusCode)
                {
                    // Leer y deserializar la respuesta
                    var contenido = await respuesta.Content.ReadFromJsonAsync<RespuestaPublicacionesPendientes>();

                    // Verificar que el estado sea exitoso y que haya datos
                    if (contenido.Status == "success" && contenido.Datos != null)
                    {
                        return contenido.Datos;
                    }
                    else
                    {
                        Console.WriteLine($"Error en la respuesta: {contenido.Status}");
                        return null;
                    }
                }
                else
                {
                    // Manejar error de la API
                    string mensajeError = await respuesta.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error al obtener publicaciones pendientes. Código: {respuesta.StatusCode}, Mensaje: {mensajeError}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                // Manejar excepciones generales
                Console.WriteLine($"Excepción al obtener publicaciones pendientes: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Obtiene los detalles completos de una publicación específica
        /// </summary>
        /// <param name="token">Token JWT de autenticación</param>
        /// <param name="idPublicacion">ID de la publicación a consultar</param>
        /// <returns>Detalles de la publicación o null si ocurre un error</returns>
        public static async Task<DetallePublicacion> ObtenerDetallePublicacionAsync(string token, int idPublicacion)
        {
            // Validaciones básicas
            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine("Error: Token no proporcionado para obtener detalle de publicación");
                return null;
            }

            if (idPublicacion <= 0)
            {
                Console.WriteLine("Error: ID de publicación no válido");
                return null;
            }

            try
            {
                // Crear cliente HTTP
                using var client = new HttpClient();
                client.BaseAddress = new Uri(Parametros.UrlBaseApi);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // Realizar la petición GET
                HttpResponseMessage respuesta = await client.GetAsync($"/api/management/revision/publicacion/{idPublicacion}");

                // Verificar respuesta
                if (respuesta.IsSuccessStatusCode)
                {
                    var contenido = await respuesta.Content.ReadFromJsonAsync<RespuestaDetallePublicacion>();

                    if (contenido.Status == "success" && contenido.Datos != null)
                    {
                        return contenido.Datos;
                    }
                    else
                    {
                        Console.WriteLine($"Error en la respuesta: {contenido.Status}");
                        return null;
                    }
                }
                else
                {
                    string mensajeError = await respuesta.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error al obtener detalle de publicación. Código: {respuesta.StatusCode}, Mensaje: {mensajeError}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Excepción al obtener detalle de publicación: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Inicia una nueva revisión para una publicación
        /// </summary>
        /// <param name="token">Token JWT de autenticación</param>
        /// <param name="idPublicacion">ID de la publicación a revisar</param>
        /// <param name="idTipoRevision">ID del tipo de revisión a realizar</param>
        /// <param name="comentarioInicial">Comentario inicial opcional para la revisión</param>
        /// <returns>Respuesta con resultado de la operación</returns>
        public static async Task<RespuestaCreacionRevision> IniciarRevisionAsync(string token, int idPublicacion, int idTipoRevision, string comentarioInicial = null)
        {
            // Validaciones básicas
            if (string.IsNullOrEmpty(token))
            {
                return new RespuestaCreacionRevision
                {
                    Status = "error",
                    Mensaje = "Token no proporcionado"
                };
            }

            if (idPublicacion <= 0)
            {
                return new RespuestaCreacionRevision
                {
                    Status = "error",
                    Mensaje = "ID de publicación no válido"
                };
            }

            if (idTipoRevision <= 0)
            {
                return new RespuestaCreacionRevision
                {
                    Status = "error",
                    Mensaje = "Debe seleccionar un tipo de revisión válido"
                };
            }

            try
            {
                // Crear cliente HTTP
                using var client = new HttpClient();
                client.BaseAddress = new Uri(Parametros.UrlBaseApi);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // Preparar datos para enviar
                var iniciarRevision = new IniciarRevision
                {
                    IdTipoRevision = idTipoRevision,
                    ComentarioInicial = comentarioInicial
                };

                // Realizar la petición POST
                HttpResponseMessage respuesta = await client.PostAsJsonAsync($"/api/management/revision/iniciar/{idPublicacion}", iniciarRevision);

                // Procesar respuesta
                if (respuesta.IsSuccessStatusCode)
                {
                    return await respuesta.Content.ReadFromJsonAsync<RespuestaCreacionRevision>();
                }
                else
                {
                    // Intentar leer el mensaje de error
                    try
                    {
                        var error = await respuesta.Content.ReadFromJsonAsync<RespuestaCreacionRevision>();
                        if (error != null)
                        {
                            return error;
                        }
                    }
                    catch { /* Ignorar errores al leer respuesta de error */ }

                    // Si no se pudo leer un mensaje de error específico
                    return new RespuestaCreacionRevision
                    {
                        Status = "error",
                        Mensaje = $"Error al iniciar revisión. Código: {respuesta.StatusCode}"
                    };
                }
            }
            catch (Exception ex)
            {
                return new RespuestaCreacionRevision
                {
                    Status = "error",
                    Mensaje = $"Error al iniciar revisión: {ex.Message}"
                };
            }
        }

        /// <summary>
        /// Finaliza una revisión con una decisión y comentario
        /// </summary>
        /// <param name="token">Token JWT de autenticación</param>
        /// <param name="idRevision">ID de la revisión a finalizar</param>
        /// <param name="decision">Decisión: "aprobar", "solicitar_cambios" o "rechazar"</param>
        /// <param name="comentario">Comentario de retroalimentación</param>
        /// <returns>Respuesta con resultado de la operación</returns>
        public static async Task<RespuestaFinalizacionRevision> FinalizarRevisionAsync(string token, int idRevision, string decision, string comentario)
        {
            // Validaciones básicas
            if (string.IsNullOrEmpty(token))
            {
                return new RespuestaFinalizacionRevision
                {
                    Status = "error",
                    Mensaje = "Token no proporcionado"
                };
            }

            if (idRevision <= 0)
            {
                return new RespuestaFinalizacionRevision
                {
                    Status = "error",
                    Mensaje = "ID de revisión no válido"
                };
            }

            // Validar que la decisión sea válida según las constantes definidas
            if (string.IsNullOrEmpty(decision) ||
                (decision != DecisionRevision.Aprobar &&
                 decision != DecisionRevision.SolicitarCambios &&
                 decision != DecisionRevision.Rechazar))
            {
                return new RespuestaFinalizacionRevision
                {
                    Status = "error",
                    Mensaje = "Decisión debe ser 'aprobar', 'solicitar_cambios' o 'rechazar'"
                };
            }

            if (string.IsNullOrEmpty(comentario))
            {
                return new RespuestaFinalizacionRevision
                {
                    Status = "error",
                    Mensaje = "Debe proporcionar un comentario de retroalimentación"
                };
            }

            try
            {
                // Crear cliente HTTP
                using var client = new HttpClient();
                client.BaseAddress = new Uri(Parametros.UrlBaseApi);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // Preparar datos para enviar
                var finalizarRevision = new FinalizarRevision
                {
                    Decision = decision,
                    Comentario = comentario
                };

                // Realizar la petición PUT
                HttpResponseMessage respuesta = await client.PutAsJsonAsync($"/api/management/revision/finalizar/{idRevision}", finalizarRevision);

                // Procesar respuesta
                if (respuesta.IsSuccessStatusCode)
                {
                    return await respuesta.Content.ReadFromJsonAsync<RespuestaFinalizacionRevision>();
                }
                else
                {
                    // Intentar leer el mensaje de error
                    try
                    {
                        var error = await respuesta.Content.ReadFromJsonAsync<RespuestaFinalizacionRevision>();
                        if (error != null)
                        {
                            return error;
                        }
                    }
                    catch { /* Ignorar errores al leer respuesta de error */ }

                    // Si no se pudo leer un mensaje de error específico
                    return new RespuestaFinalizacionRevision
                    {
                        Status = "error",
                        Mensaje = $"Error al finalizar revisión. Código: {respuesta.StatusCode}"
                    };
                }
            }
            catch (Exception ex)
            {
                return new RespuestaFinalizacionRevision
                {
                    Status = "error",
                    Mensaje = $"Error al finalizar revisión: {ex.Message}"
                };
            }
        }

        /// <summary>
        /// Agrega un comentario adicional a una revisión existente
        /// </summary>
        /// <param name="token">Token JWT de autenticación</param>
        /// <param name="idRevision">ID de la revisión</param>
        /// <param name="contenido">Contenido del comentario</param>
        /// <returns>Respuesta con resultado de la operación</returns>
        public static async Task<RespuestaComentario> AgregarComentarioAsync(string token, int idRevision, string contenido)
        {
            // Validaciones básicas
            if (string.IsNullOrEmpty(token))
            {
                return new RespuestaComentario
                {
                    Status = "error",
                    Mensaje = "Token no proporcionado"
                };
            }

            if (idRevision <= 0)
            {
                return new RespuestaComentario
                {
                    Status = "error",
                    Mensaje = "ID de revisión no válido"
                };
            }

            if (string.IsNullOrEmpty(contenido))
            {
                return new RespuestaComentario
                {
                    Status = "error",
                    Mensaje = "El contenido del comentario no puede estar vacío"
                };
            }

            try
            {
                // Crear cliente HTTP
                using var client = new HttpClient();
                client.BaseAddress = new Uri(Parametros.UrlBaseApi);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // Preparar datos para enviar
                var nuevoComentario = new NuevoComentario { Contenido = contenido };

                // Realizar la petición POST
                HttpResponseMessage respuesta = await client.PostAsJsonAsync($"/api/management/revision/comentario/{idRevision}", nuevoComentario);

                // Procesar respuesta
                if (respuesta.IsSuccessStatusCode)
                {
                    return await respuesta.Content.ReadFromJsonAsync<RespuestaComentario>();
                }
                else
                {
                    // Intentar leer el mensaje de error
                    try
                    {
                        var error = await respuesta.Content.ReadFromJsonAsync<RespuestaComentario>();
                        if (error != null)
                        {
                            return error;
                        }
                    }
                    catch { /* Ignorar errores al leer respuesta de error */ }

                    // Si no se pudo leer un mensaje de error específico
                    return new RespuestaComentario
                    {
                        Status = "error",
                        Mensaje = $"Error al agregar comentario. Código: {respuesta.StatusCode}"
                    };
                }
            }
            catch (Exception ex)
            {
                return new RespuestaComentario
                {
                    Status = "error",
                    Mensaje = $"Error al agregar comentario: {ex.Message}"
                };
            }
        }

        /// <summary>
        /// Inicia una nueva revisión con tipo y descripción personalizados
        /// </summary>
        /// <param name="token">Token JWT de autenticación</param>
        /// <param name="idPublicacion">ID de la publicación a revisar</param>
        /// <param name="tipoRevision">Texto del tipo de revisión</param>
        /// <param name="descripcionRevision">Descripción de la revisión</param>
        /// <param name="comentarioInicial">Comentario inicial</param>
        /// <returns>Respuesta con resultado de la operación</returns>
        public static async Task<RespuestaCreacionRevision> IniciarRevisionCustomAsync(
            string token,
            int idPublicacion,
            string tipoRevision,
            string descripcionRevision,
            string comentarioInicial)
        {
            // Validaciones básicas
            if (string.IsNullOrEmpty(token))
            {
                return new RespuestaCreacionRevision
                {
                    Status = "error",
                    Mensaje = "Token no proporcionado"
                };
            }

            if (idPublicacion <= 0)
            {
                return new RespuestaCreacionRevision
                {
                    Status = "error",
                    Mensaje = "ID de publicación no válido"
                };
            }

            if (string.IsNullOrWhiteSpace(tipoRevision))
            {
                return new RespuestaCreacionRevision
                {
                    Status = "error",
                    Mensaje = "Debe proporcionar un tipo de revisión"
                };
            }

            if (string.IsNullOrWhiteSpace(descripcionRevision))
            {
                return new RespuestaCreacionRevision
                {
                    Status = "error",
                    Mensaje = "Debe proporcionar una descripción de la revisión"
                };
            }

            try
            {
                // Crear cliente HTTP
                using var client = new HttpClient();
                client.BaseAddress = new Uri(Parametros.UrlBaseApi);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // Preparar datos para enviar - ajustando a las propiedades esperadas por la API
                var iniciarRevision = new
                {
                    tipo_revision = tipoRevision,
                    descripcion_revision = descripcionRevision,
                    comentario_inicial = comentarioInicial
                };

                // Usar el endpoint correcto
                HttpResponseMessage respuesta = await client.PostAsJsonAsync($"/api/management/revision/iniciar/{idPublicacion}", iniciarRevision);

                // Procesar respuesta
                if (respuesta.IsSuccessStatusCode)
                {
                    return await respuesta.Content.ReadFromJsonAsync<RespuestaCreacionRevision>();
                }
                else
                {
                    // Intentar leer el mensaje de error
                    try
                    {
                        var error = await respuesta.Content.ReadFromJsonAsync<RespuestaCreacionRevision>();
                        if (error != null)
                        {
                            return error;
                        }
                    }
                    catch { /* Ignorar errores al leer respuesta de error */ }

                    // Si no se pudo leer un mensaje de error específico
                    return new RespuestaCreacionRevision
                    {
                        Status = "error",
                        Mensaje = $"Error al iniciar revisión. Código: {respuesta.StatusCode}"
                    };
                }
            }
            catch (Exception ex)
            {
                return new RespuestaCreacionRevision
                {
                    Status = "error",
                    Mensaje = $"Error al iniciar revisión: {ex.Message}"
                };
            }
        }

        /// <summary>
        /// Obtiene el historial de revisiones de una publicación
        /// </summary>
        /// <param name="token">Token JWT de autenticación</param>
        /// <param name="idPublicacion">ID de la publicación</param>
        /// <returns>Lista de revisiones o null si ocurre un error</returns>
        public static async Task<List<RevisionCompleta>> ObtenerHistorialRevisionesAsync(string token, int idPublicacion)
        {
            // Validaciones básicas
            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine("Error: Token no proporcionado para obtener historial de revisiones");
                return null;
            }

            if (idPublicacion <= 0)
            {
                Console.WriteLine("Error: ID de publicación no válido");
                return null;
            }

            try
            {
                // Crear cliente HTTP
                using var client = new HttpClient();
                client.BaseAddress = new Uri(Parametros.UrlBaseApi);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // Realizar la petición GET
                HttpResponseMessage respuesta = await client.GetAsync($"/api/management/revision/historial/{idPublicacion}");

                // Verificar respuesta
                if (respuesta.IsSuccessStatusCode)
                {
                    var contenido = await respuesta.Content.ReadFromJsonAsync<RespuestaPublicacionesPendientes>();

                    if (contenido.Status == "success" && contenido.Datos != null)
                    {
                        // Deserializar a una lista de RevisionCompleta
                        // (esto asume que la API devuelve una estructura compatible)
                        var historial = await respuesta.Content.ReadFromJsonAsync<RespuestaHistorialRevisiones>();
                        return historial.Datos;
                    }
                    else
                    {
                        Console.WriteLine($"Error en la respuesta: {contenido.Status}");
                        return null;
                    }
                }
                else
                {
                    string mensajeError = await respuesta.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error al obtener historial de revisiones. Código: {respuesta.StatusCode}, Mensaje: {mensajeError}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Excepción al obtener historial de revisiones: {ex.Message}");
                return null;
            }
        }
    }

    /// <summary>
    /// Modelo auxiliar para deserializar la respuesta del historial de revisiones
    /// </summary>
    internal class RespuestaHistorialRevisiones
    {
        public string Status { get; set; }
        public List<RevisionCompleta> Datos { get; set; }
    }
}
