using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DAL.Modelos;

namespace DAL.Consumo
{
    /// <summary>
    /// Clase para gestionar las operaciones de usuarios desde el panel de administración
    /// </summary>
    public class gestionUsuarios
    {
        /// <summary>
        /// Obtiene el listado completo de usuarios
        /// </summary>
        /// <param name="token">Token JWT de autenticación</param>
        /// <returns>Lista de usuarios o null si ocurre un error</returns>
        public static async Task<List<UsuarioListado>> ObtenerListadoUsuariosAsync(string token)
        {
            // Validación básica del token
            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine("Error: Token no proporcionado para obtener listado de usuarios");
                return null;
            }

            try
            {
                // Crear cliente HTTP con la configuración necesaria
                using var client = new HttpClient();
                client.BaseAddress = new Uri(Parametros.UrlBaseApi);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // Realizar la petición GET
                HttpResponseMessage respuesta = await client.GetAsync("/api/management/gestion/usuarios/lista");

                // Verificar si la respuesta fue exitosa
                if (respuesta.IsSuccessStatusCode)
                {
                    // Leer y deserializar la respuesta
                    var contenido = await respuesta.Content.ReadFromJsonAsync<RespuestaListadoUsuarios>();

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
                    Console.WriteLine($"Error al obtener usuarios. Código: {respuesta.StatusCode}, Mensaje: {mensajeError}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                // Manejar excepciones generales
                Console.WriteLine($"Excepción al obtener usuarios: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Obtiene los detalles completos de un usuario específico
        /// </summary>
        /// <param name="token">Token JWT de autenticación</param>
        /// <param name="idUsuario">ID del usuario a consultar</param>
        /// <returns>Detalles del usuario o null si ocurre un error</returns>
        public static async Task<UsuarioDetalle> ObtenerDetalleUsuarioAsync(string token, int idUsuario)
        {
            // Validaciones básicas
            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine("Error: Token no proporcionado para obtener detalle de usuario");
                return null;
            }

            if (idUsuario <= 0)
            {
                Console.WriteLine("Error: ID de usuario no válido");
                return null;
            }

            try
            {
                // Crear cliente HTTP
                using var client = new HttpClient();
                client.BaseAddress = new Uri(Parametros.UrlBaseApi);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // Realizar la petición GET
                HttpResponseMessage respuesta = await client.GetAsync($"/api/management/gestion/usuarios/detalle/{idUsuario}");

                // Verificar respuesta
                if (respuesta.IsSuccessStatusCode)
                {
                    var contenido = await respuesta.Content.ReadFromJsonAsync<RespuestaDetalleUsuario>();

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
                    Console.WriteLine($"Error al obtener detalle de usuario. Código: {respuesta.StatusCode}, Mensaje: {mensajeError}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Excepción al obtener detalle de usuario: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Cambia el rol de un usuario específico
        /// </summary>
        /// <param name="token">Token JWT de autenticación</param>
        /// <param name="idUsuario">ID del usuario a modificar</param>
        /// <param name="nuevoRol">Nuevo rol a asignar</param>
        /// <returns>Respuesta con resultado de la operación</returns>
        public static async Task<RespuestaGeneral> CambiarRolUsuarioAsync(string token, int idUsuario, string nuevoRol)
        {
            // Validaciones
            if (string.IsNullOrEmpty(token))
            {
                return new RespuestaGeneral
                {
                    Status = "error",
                    Mensaje = "Token no proporcionado"
                };
            }

            if (idUsuario <= 0)
            {
                return new RespuestaGeneral
                {
                    Status = "error",
                    Mensaje = "ID de usuario no válido"
                };
            }

            if (string.IsNullOrEmpty(nuevoRol))
            {
                return new RespuestaGeneral
                {
                    Status = "error",
                    Mensaje = "Debe especificar un rol válido"
                };
            }

            try
            {
                // Crear cliente HTTP
                using var client = new HttpClient();
                client.BaseAddress = new Uri(Parametros.UrlBaseApi);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // Preparar datos para enviar
                var cambioRol = new CambioRolUsuario { Rol = nuevoRol };

                // Realizar la petición PUT
                HttpResponseMessage respuesta = await client.PutAsJsonAsync($"/api/management/gestion/usuarios/{idUsuario}/rol", cambioRol);

                // Procesar respuesta
                RespuestaGeneral resultado = await respuesta.Content.ReadFromJsonAsync<RespuestaGeneral>();

                // Si hay error de status code, asegurarse de devolver un objeto válido
                if (!respuesta.IsSuccessStatusCode && resultado == null)
                {
                    resultado = new RespuestaGeneral
                    {
                        Status = "error",
                        Mensaje = $"Error en la solicitud: {respuesta.StatusCode}"
                    };
                }

                return resultado;
            }
            catch (Exception ex)
            {
                return new RespuestaGeneral
                {
                    Status = "error",
                    Mensaje = $"Error al cambiar rol: {ex.Message}"
                };
            }
        }

        /// <summary>
        /// Restablece la contraseña de un usuario
        /// </summary>
        /// <param name="token">Token JWT de autenticación</param>
        /// <param name="idUsuario">ID del usuario</param>
        /// <param name="nuevaContrasena">Nueva contraseña a establecer</param>
        /// <returns>Respuesta con resultado de la operación</returns>
        public static async Task<RespuestaGeneral> RestablecerContrasenaAsync(string token, int idUsuario, string nuevaContrasena)
        {
            // Validaciones
            if (string.IsNullOrEmpty(token))
            {
                return new RespuestaGeneral
                {
                    Status = "error",
                    Mensaje = "Token no proporcionado"
                };
            }

            if (idUsuario <= 0)
            {
                return new RespuestaGeneral
                {
                    Status = "error",
                    Mensaje = "ID de usuario no válido"
                };
            }

            if (string.IsNullOrEmpty(nuevaContrasena))
            {
                return new RespuestaGeneral
                {
                    Status = "error",
                    Mensaje = "Debe especificar una contraseña válida"
                };
            }

            try
            {
                // Crear cliente HTTP
                using var client = new HttpClient();
                client.BaseAddress = new Uri(Parametros.UrlBaseApi);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // Preparar datos para enviar
                var reseteo = new ReseteoContrasena { NuevaContrasena = nuevaContrasena };

                // Realizar la petición POST
                HttpResponseMessage respuesta = await client.PostAsJsonAsync($"/api/management/gestion/usuarios/{idUsuario}/reset-password", reseteo);

                // Procesar respuesta
                RespuestaGeneral resultado = await respuesta.Content.ReadFromJsonAsync<RespuestaGeneral>();

                // Verificar si se obtuvo un resultado
                if (!respuesta.IsSuccessStatusCode && resultado == null)
                {
                    resultado = new RespuestaGeneral
                    {
                        Status = "error",
                        Mensaje = $"Error en la solicitud: {respuesta.StatusCode}"
                    };
                }

                return resultado;
            }
            catch (Exception ex)
            {
                return new RespuestaGeneral
                {
                    Status = "error",
                    Mensaje = $"Error al restablecer contraseña: {ex.Message}"
                };
            }
        }

        /// <summary>
        /// Obtiene las notificaciones de un usuario específico
        /// </summary>
        /// <param name="token">Token JWT de autenticación</param>
        /// <param name="idUsuario">ID del usuario</param>
        /// <returns>Lista de notificaciones o null si ocurre un error</returns>
        public static async Task<List<NotificacionUsuario>> ObtenerNotificacionesUsuarioAsync(string token, int idUsuario)
        {
            // Validaciones
            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine("Error: Token no proporcionado para obtener notificaciones");
                return null;
            }

            if (idUsuario <= 0)
            {
                Console.WriteLine("Error: ID de usuario no válido");
                return null;
            }

            try
            {
                // Crear cliente HTTP
                using var client = new HttpClient();
                client.BaseAddress = new Uri(Parametros.UrlBaseApi);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // Realizar la petición GET
                HttpResponseMessage respuesta = await client.GetAsync($"/api/management/gestion/usuarios/{idUsuario}/notificaciones");

                // Verificar respuesta
                if (respuesta.IsSuccessStatusCode)
                {
                    var contenido = await respuesta.Content.ReadFromJsonAsync<RespuestaNotificacionesUsuario>();

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
                    Console.WriteLine($"Error al obtener notificaciones. Código: {respuesta.StatusCode}, Mensaje: {mensajeError}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Excepción al obtener notificaciones: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Crea un nuevo usuario en el sistema
        /// </summary>
        /// <param name="token">Token JWT de autenticación</param>
        /// <param name="usuario">Datos del nuevo usuario</param>
        /// <returns>Respuesta con resultado de la operación</returns>
        public static async Task<RespuestaNuevoUsuario> CrearUsuarioAsync(string token, NuevoUsuario usuario)
        {
            // Validaciones
            if (string.IsNullOrEmpty(token))
            {
                return new RespuestaNuevoUsuario
                {
                    Status = "error",
                    Mensaje = "Token no proporcionado"
                };
            }

            if (usuario == null || string.IsNullOrEmpty(usuario.Correo) || string.IsNullOrEmpty(usuario.Contrasena))
            {
                return new RespuestaNuevoUsuario
                {
                    Status = "error",
                    Mensaje = "Datos de usuario incompletos"
                };
            }

            try
            {
                // Crear cliente HTTP
                using var client = new HttpClient();
                client.BaseAddress = new Uri(Parametros.UrlBaseApi);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // Realizar la petición POST
                HttpResponseMessage respuesta = await client.PostAsJsonAsync("/api/management/gestion/usuarios/crear", usuario);

                // Verificar respuesta
                if (respuesta.IsSuccessStatusCode)
                {
                    var resultado = await respuesta.Content.ReadFromJsonAsync<RespuestaNuevoUsuario>();
                    return resultado;
                }
                else
                {
                    // Intentar leer el mensaje de error
                    try
                    {
                        var error = await respuesta.Content.ReadFromJsonAsync<RespuestaNuevoUsuario>();
                        if (error != null)
                        {
                            return error;
                        }
                    }
                    catch { /* Ignorar errores al leer respuesta de error */ }

                    // Si no se pudo leer un mensaje de error específico
                    return new RespuestaNuevoUsuario
                    {
                        Status = "error",
                        Mensaje = $"Error al crear usuario. Código: {respuesta.StatusCode}"
                    };
                }
            }
            catch (Exception ex)
            {
                return new RespuestaNuevoUsuario
                {
                    Status = "error",
                    Mensaje = $"Error al crear usuario: {ex.Message}"
                };
            }
        }
    }
}
