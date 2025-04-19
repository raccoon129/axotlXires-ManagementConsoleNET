using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DAL.Modelos;

namespace DAL.Consumo
{
    /// <summary>
    /// Clase para consultar imágenes del sistema (fotos de perfil y portadas)
    /// </summary>
    public class consultarImagenes
    {
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public consultarImagenes()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(Parametros.UrlBaseApi)
            };

            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        /// <summary>
        /// Obtiene la URL de la foto de perfil de un usuario
        /// </summary>
        /// <param name="token">Token de autenticación</param>
        /// <param name="idUsuario">ID del usuario</param>
        /// <returns>RespuestaConsultaImagen con los datos de la imagen</returns>
        public async Task<RespuestaConsultaImagen> ObtenerImagenPerfilAsync(string token, int idUsuario)
        {
            try
            {
                // Configurar el header de autenticación
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // Realizar petición GET para obtener la imagen de perfil
                var respuesta = await _httpClient.GetAsync($"/api/management/imagenes/foto-perfil/{idUsuario}");

                // Verificar si la petición fue exitosa
                if (respuesta.IsSuccessStatusCode)
                {
                    var contenido = await respuesta.Content.ReadAsStringAsync();
                    var resultado = JsonSerializer.Deserialize<RespuestaConsultaImagen>(contenido, _jsonOptions);
                    return resultado;
                }
                else
                {
                    // Si hay error, crear respuesta de error
                    return new RespuestaConsultaImagen
                    {
                        Status = "error",
                        Mensaje = $"Error al obtener la imagen de perfil: {respuesta.StatusCode}",
                        Datos = null
                    };
                }
            }
            catch (Exception ex)
            {
                // En caso de excepción, devolver error
                return new RespuestaConsultaImagen
                {
                    Status = "error",
                    Mensaje = $"Error al consultar la imagen de perfil: {ex.Message}",
                    Datos = null
                };
            }
        }

        /// <summary>
        /// Obtiene la imagen de portada de una publicación
        /// </summary>
        /// <param name="token">Token de autenticación</param>
        /// <param name="idPublicacion">ID de la publicación</param>
        /// <returns>RespuestaConsultaImagen con los datos de la imagen</returns>
        public async Task<RespuestaConsultaImagen> ObtenerImagenPortadaAsync(string token, int idPublicacion)
        {
            try
            {
                // Configurar el header de autenticación
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // Realizar petición GET para obtener la imagen de portada
                var respuesta = await _httpClient.GetAsync($"/api/management/imagenes/portada/{idPublicacion}");

                // Verificar si la petición fue exitosa
                if (respuesta.IsSuccessStatusCode)
                {
                    var contenido = await respuesta.Content.ReadAsStringAsync();
                    var resultado = JsonSerializer.Deserialize<RespuestaConsultaImagen>(contenido, _jsonOptions);
                    return resultado;
                }
                else
                {
                    // Si hay error, crear respuesta de error
                    return new RespuestaConsultaImagen
                    {
                        Status = "error",
                        Mensaje = $"Error al obtener la imagen de portada: {respuesta.StatusCode}",
                        Datos = null
                    };
                }
            }
            catch (Exception ex)
            {
                // En caso de excepción, devolver error
                return new RespuestaConsultaImagen
                {
                    Status = "error",
                    Mensaje = $"Error al consultar la imagen de portada: {ex.Message}",
                    Datos = null
                };
            }
        }

        /// <summary>
        /// Obtiene directamente la imagen como array de bytes
        /// </summary>
        /// <param name="token">Token de autenticación</param>
        /// <param name="idUsuario">ID del usuario</param>
        /// <returns>Array de bytes de la imagen o null si hay error</returns>
        public async Task<byte[]> ObtenerImagenPerfilBytesAsync(string token, int idUsuario)
        {
            try
            {
                // Configurar el header de autenticación
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // Realizar petición GET para obtener la imagen de perfil como bytes
                var respuesta = await _httpClient.GetAsync($"/api/management/imagenes/foto-perfil/{idUsuario}");

                // Verificar si la petición fue exitosa
                if (respuesta.IsSuccessStatusCode)
                {
                    var bytes = await respuesta.Content.ReadAsByteArrayAsync();
                    return bytes;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Obtiene directamente la imagen de portada como array de bytes
        /// </summary>
        /// <param name="token">Token de autenticación</param>
        /// <param name="idPublicacion">ID de la publicación</param>
        /// <returns>Array de bytes de la imagen de portada o null si hay error</returns>
        public async Task<byte[]> ObtenerImagenPortadaBytesAsync(string token, int idPublicacion)
        {
            try
            {
                // Configurar el header de autenticación
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // Realizar petición GET para obtener la imagen de portada como bytes
                var respuesta = await _httpClient.GetAsync($"/api/management/imagenes/portada/{idPublicacion}");

                // Verificar si la petición fue exitosa
                if (respuesta.IsSuccessStatusCode)
                {
                    var bytes = await respuesta.Content.ReadAsByteArrayAsync();
                    return bytes;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Obtiene la URL de la imagen con token incluido para uso directo en componentes
        /// </summary>
        /// <param name="token">Token de autenticación</param>
        /// <param name="idUsuario">ID del usuario</param>
        /// <returns>URL completa para acceder a la imagen de perfil</returns>
        public string ObtenerUrlImagenPerfil(string token, int idUsuario)
        {
            return $"{Parametros.UrlBaseApi}/api/management/imagenes/foto-perfil/{idUsuario}";
        }

        /// <summary>
        /// Obtiene la URL de la imagen de portada con token incluido para uso directo en componentes
        /// </summary>
        /// <param name="token">Token de autenticación</param>
        /// <param name="idPublicacion">ID de la publicación</param>
        /// <returns>URL completa para acceder a la imagen de portada</returns>
        public string ObtenerUrlImagenPortada(string token, int idPublicacion)
        {
            return $"{Parametros.UrlBaseApi}/api/management/imagenes/portada/{idPublicacion}";
        }
    }
}
