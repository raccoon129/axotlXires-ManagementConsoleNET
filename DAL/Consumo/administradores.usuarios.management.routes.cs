using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DAL.Modelos;
using static DAL.Modelos.ModeloAdministradoresUsuarios;

namespace DAL.Consumo
{
    /// <summary>
    /// Clase para gestionar los perfiles de usuarios administrativos
    /// </summary>
    public class administradoresUsuarios
    {
        private static readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        #region Consulta de perfil

        /// <summary>
        /// Obtiene el perfil completo de un usuario administrativo
        /// </summary>
        /// <param name="token">Token JWT de autenticación</param>
        /// <param name="idUsuario">ID del usuario a consultar</param>
        /// <returns>Perfil del usuario administrativo o null si ocurre un error</returns>
        public static async Task<PerfilAdministrativo> ObtenerPerfilAdministrativoAsync(string token, int idUsuario)
        {
            // Validación básica de parámetros
            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine("Error: No se proporcionó un token de autenticación");
                return null;
            }

            try
            {
                // Crear cliente HTTP con la configuración necesaria
                using var httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(Parametros.UrlBaseApi);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // Realizar la petición GET
                var respuesta = await httpClient.GetAsync($"/api/management/administradores/usuarios/perfil/{idUsuario}");

                // Verificar si la respuesta fue exitosa
                if (respuesta.IsSuccessStatusCode)
                {
                    // Deserializar la respuesta
                    var contenido = await respuesta.Content.ReadAsStringAsync();
                    var resultado = JsonSerializer.Deserialize<RespuestaPerfilAdministrativo>(contenido, jsonOptions);

                    // Verificar que el estado sea exitoso y que haya datos
                    if (resultado?.Status == "success" && resultado.Datos != null)
                    {
                        return resultado.Datos;
                    }
                    else
                    {
                        Console.WriteLine($"Error en la respuesta: {resultado?.Mensaje ?? "Respuesta vacía"}");
                        return null;
                    }
                }
                else
                {
                    // Manejar error de la API
                    var mensajeError = await respuesta.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error al obtener perfil. Código: {respuesta.StatusCode}, Mensaje: {mensajeError}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                // Manejar excepciones generales
                Console.WriteLine($"Excepción al obtener perfil administrativo: {ex.Message}");
                return null;
            }
        }

        #endregion

        #region Actualización de información básica

        /// <summary>
        /// Actualiza la información básica de un usuario administrativo
        /// </summary>
        /// <param name="token">Token JWT de autenticación</param>
        /// <param name="idUsuario">ID del usuario a actualizar</param>
        /// <param name="nombre">Nuevo nombre completo</param>
        /// <param name="nombramiento">Nuevo cargo o nombramiento</param>
        /// <returns>Respuesta de la actualización con estado y mensaje</returns>
        public static async Task<RespuestaActualizacionInfoBasica> ActualizarInformacionBasicaAsync(
            string token, int idUsuario, string nombre, string nombramiento)
        {
            // Validación básica de parámetros
            if (string.IsNullOrEmpty(token))
            {
                return new RespuestaActualizacionInfoBasica
                {
                    Status = "error",
                    Mensaje = "No se proporcionó un token de autenticación"
                };
            }

            // Validar que al menos un campo tenga valor
            if (string.IsNullOrWhiteSpace(nombre) && string.IsNullOrWhiteSpace(nombramiento))
            {
                return new RespuestaActualizacionInfoBasica
                {
                    Status = "error",
                    Mensaje = "Debe proporcionar al menos un campo para actualizar"
                };
            }

            try
            {
                // Crear cliente HTTP con la configuración necesaria
                using var httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(Parametros.UrlBaseApi);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // Crear el objeto de datos a enviar
                var datos = new ActualizacionInfoBasica
                {
                    Nombre = nombre,
                    Nombramiento = nombramiento
                };

                // Realizar la petición PUT
                var respuesta = await httpClient.PutAsJsonAsync(
                    $"/api/management/administradores/usuarios/perfil/{idUsuario}/info-basica", datos);

                // Verificar si la respuesta fue exitosa
                if (respuesta.IsSuccessStatusCode)
                {
                    // Deserializar la respuesta
                    var contenido = await respuesta.Content.ReadAsStringAsync();
                    var resultado = JsonSerializer.Deserialize<RespuestaActualizacionInfoBasica>(contenido, jsonOptions);
                    return resultado;
                }
                else
                {
                    // Manejar error de la API
                    var mensajeError = await respuesta.Content.ReadAsStringAsync();
                    return new RespuestaActualizacionInfoBasica
                    {
                        Status = "error",
                        Mensaje = $"Error al actualizar información. Código: {respuesta.StatusCode}, Mensaje: {mensajeError}"
                    };
                }
            }
            catch (Exception ex)
            {
                // Manejar excepciones generales
                return new RespuestaActualizacionInfoBasica
                {
                    Status = "error",
                    Mensaje = $"Excepción al actualizar información: {ex.Message}"
                };
            }
        }

        #endregion

        #region Actualización de foto de perfil

        /// <summary>
        /// Actualiza la foto de perfil de un usuario administrativo
        /// </summary>
        /// <param name="token">Token JWT de autenticación</param>
        /// <param name="idUsuario">ID del usuario a actualizar</param>
        /// <param name="imagenBytes">Bytes de la imagen a subir</param>
        /// <param name="nombreArchivo">Nombre del archivo de imagen con extensión</param>
        /// <returns>Respuesta de la actualización con estado, mensaje y datos de la foto</returns>
        public static async Task<RespuestaActualizacionFoto> ActualizarFotoPerfilAsync(
            string token, int idUsuario, byte[] imagenBytes, string nombreArchivo)
        {
            // Validación básica de parámetros
            if (string.IsNullOrEmpty(token))
            {
                return new RespuestaActualizacionFoto
                {
                    Status = "error",
                    Mensaje = "No se proporcionó un token de autenticación"
                };
            }

            if (imagenBytes == null || imagenBytes.Length == 0)
            {
                return new RespuestaActualizacionFoto
                {
                    Status = "error",
                    Mensaje = "No se proporcionó ninguna imagen válida"
                };
            }

            try
            {
                // Crear cliente HTTP con la configuración necesaria
                using var httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(Parametros.UrlBaseApi);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // Crear el contenido multipart
                using var contenido = new MultipartFormDataContent();
                var contenidoImagen = new ByteArrayContent(imagenBytes);
                contenidoImagen.Headers.ContentType = new MediaTypeHeaderValue(ObtenerContentType(nombreArchivo));
                contenido.Add(contenidoImagen, "foto_perfil", nombreArchivo);

                // Realizar la petición PUT
                var respuesta = await httpClient.PutAsync(
                    $"/api/management/administradores/usuarios/perfil/{idUsuario}/foto", contenido);

                // Verificar si la respuesta fue exitosa
                if (respuesta.IsSuccessStatusCode)
                {
                    // Deserializar la respuesta
                    var contenidoRespuesta = await respuesta.Content.ReadAsStringAsync();
                    var resultado = JsonSerializer.Deserialize<RespuestaActualizacionFoto>(contenidoRespuesta, jsonOptions);
                    return resultado;
                }
                else
                {
                    // Manejar error de la API
                    var mensajeError = await respuesta.Content.ReadAsStringAsync();
                    return new RespuestaActualizacionFoto
                    {
                        Status = "error",
                        Mensaje = $"Error al actualizar foto. Código: {respuesta.StatusCode}, Mensaje: {mensajeError}"
                    };
                }
            }
            catch (Exception ex)
            {
                // Manejar excepciones generales
                return new RespuestaActualizacionFoto
                {
                    Status = "error",
                    Mensaje = $"Excepción al actualizar foto: {ex.Message}"
                };
            }
        }

        /// <summary>
        /// Determina el tipo MIME según la extensión del archivo
        /// </summary>
        /// <param name="nombreArchivo">Nombre del archivo con extensión</param>
        /// <returns>Tipo MIME del archivo</returns>
        private static string ObtenerContentType(string nombreArchivo)
        {
            var extension = Path.GetExtension(nombreArchivo).ToLower();
            return extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".bmp" => "image/bmp",
                ".webp" => "image/webp",
                _ => "application/octet-stream"
            };
        }

        #endregion

        #region Cambio de contraseña

        /// <summary>
        /// Cambia la contraseña de un usuario administrativo
        /// </summary>
        /// <param name="token">Token JWT de autenticación</param>
        /// <param name="idUsuario">ID del usuario a actualizar</param>
        /// <param name="nuevaContrasena">Nueva contraseña</param>
        /// <returns>Respuesta del cambio con estado y mensaje</returns>
        public static async Task<RespuestaCambioContrasena> CambiarContrasenaAsync(
            string token, int idUsuario, string nuevaContrasena)
        {
            // Validación básica de parámetros
            if (string.IsNullOrEmpty(token))
            {
                return new RespuestaCambioContrasena
                {
                    Status = "error",
                    Mensaje = "No se proporcionó un token de autenticación"
                };
            }

            if (string.IsNullOrEmpty(nuevaContrasena))
            {
                return new RespuestaCambioContrasena
                {
                    Status = "error",
                    Mensaje = "No se proporcionó una nueva contraseña"
                };
            }

            try
            {
                // Crear cliente HTTP con la configuración necesaria
                using var httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(Parametros.UrlBaseApi);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // Crear el objeto de datos a enviar
                var datos = new CambioContrasena
                {
                    NuevaContrasena = nuevaContrasena
                };

                // Realizar la petición PUT
                var respuesta = await httpClient.PutAsJsonAsync(
                    $"/api/management/administradores/usuarios/perfil/{idUsuario}/contrasena", datos);

                // Verificar si la respuesta fue exitosa
                if (respuesta.IsSuccessStatusCode)
                {
                    // Deserializar la respuesta
                    var contenido = await respuesta.Content.ReadAsStringAsync();
                    var resultado = JsonSerializer.Deserialize<RespuestaCambioContrasena>(contenido, jsonOptions);
                    return resultado;
                }
                else
                {
                    // Manejar error de la API
                    var mensajeError = await respuesta.Content.ReadAsStringAsync();
                    return new RespuestaCambioContrasena
                    {
                        Status = "error",
                        Mensaje = $"Error al cambiar contraseña. Código: {respuesta.StatusCode}, Mensaje: {mensajeError}"
                    };
                }
            }
            catch (Exception ex)
            {
                // Manejar excepciones generales
                return new RespuestaCambioContrasena
                {
                    Status = "error",
                    Mensaje = $"Excepción al cambiar contraseña: {ex.Message}"
                };
            }
        }

        #endregion
    }
}