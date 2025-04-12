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
    /// Clase para consultar publicaciones en diferentes estados
    /// </summary>
    public class consultarPublicaciones
    {
        // Constantes con los endpoints para cada tipo de consulta
        private const string ENDPOINT_CAMBIOS_SOLICITADOS = "/api/management/publicaciones/solicitar-cambios";
        private const string ENDPOINT_APROBADAS = "/api/management/publicaciones/aprobadas";
        private const string ENDPOINT_RECHAZADAS = "/api/management/publicaciones/rechazadas";
        private const string ENDPOINT_TODAS = "/api/management/publicaciones/todas";

        /// <summary>
        /// Método genérico para obtener publicaciones de cualquier endpoint
        /// </summary>
        /// <param name="token">Token JWT de autenticación</param>
        /// <param name="endpoint">Endpoint específico a consultar</param>
        /// <returns>Lista de publicaciones o null si ocurre un error</returns>
        private static async Task<List<PublicacionConsulta>> ObtenerPublicacionesGenerico(string token, string endpoint)
        {
            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine("Error: Token no proporcionado para consultar publicaciones");
                return null;
            }

            try
            {
                using var client = new HttpClient();
                client.BaseAddress = new Uri(Parametros.UrlBaseApi);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage respuesta = await client.GetAsync(endpoint);

                if (respuesta.IsSuccessStatusCode)
                {
                    var contenido = await respuesta.Content.ReadFromJsonAsync<RespuestaConsultaPublicaciones>();

                    if (contenido.Status == "success" && contenido.Datos?.Publicaciones != null)
                    {
                        return contenido.Datos.Publicaciones;
                    }
                    else
                    {
                        Console.WriteLine($"Error en la respuesta: {contenido?.Status ?? "Respuesta vacía"}");
                        return null;
                    }
                }
                else
                {
                    string mensajeError = await respuesta.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error al consultar publicaciones. Código: {respuesta.StatusCode}, Mensaje: {mensajeError}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Excepción al consultar publicaciones: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Obtiene la lista de publicaciones con cambios solicitados
        /// </summary>
        /// <param name="token">Token JWT de autenticación</param>
        /// <returns>Lista de publicaciones o null si ocurre un error</returns>
        public static async Task<List<PublicacionConsulta>> ObtenerPublicacionesCambiosSolicitadosAsync(string token)
        {
            return await ObtenerPublicacionesGenerico(token, ENDPOINT_CAMBIOS_SOLICITADOS);
        }

        /// <summary>
        /// Obtiene la lista de publicaciones aprobadas
        /// </summary>
        /// <param name="token">Token JWT de autenticación</param>
        /// <returns>Lista de publicaciones o null si ocurre un error</returns>
        public static async Task<List<PublicacionConsulta>> ObtenerPublicacionesAprobadasAsync(string token)
        {
            return await ObtenerPublicacionesGenerico(token, ENDPOINT_APROBADAS);
        }

        /// <summary>
        /// Obtiene la lista de publicaciones rechazadas
        /// </summary>
        /// <param name="token">Token JWT de autenticación</param>
        /// <returns>Lista de publicaciones o null si ocurre un error</returns>
        public static async Task<List<PublicacionConsulta>> ObtenerPublicacionesRechazadasAsync(string token)
        {
            return await ObtenerPublicacionesGenerico(token, ENDPOINT_RECHAZADAS);
        }

        /// <summary>
        /// Obtiene la lista de todas las publicaciones
        /// </summary>
        /// <param name="token">Token JWT de autenticación</param>
        /// <returns>Lista de publicaciones o null si ocurre un error</returns>
        public static async Task<List<PublicacionConsulta>> ObtenerTodasPublicacionesAsync(string token)
        {
            return await ObtenerPublicacionesGenerico(token, ENDPOINT_TODAS);
        }
    }
}