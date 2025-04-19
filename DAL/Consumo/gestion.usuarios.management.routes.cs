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

        /// <summary>
        /// Activa o desactiva un usuario en el sistema
        /// </summary>
        /// <param name="token">Token JWT de autenticación</param>
        /// <param name="idUsuario">ID del usuario a modificar</param>
        /// <param name="activo">Estado de activación: 1 para activar, 0 para desactivar</param>
        /// <returns>Respuesta con el resultado de la operación</returns>
        public static async Task<RespuestaActivacionUsuario> CambiarActivacionUsuarioAsync(string token, int idUsuario, int activo)
        {
            // Validaciones básicas
            if (string.IsNullOrEmpty(token))
            {
                return new RespuestaActivacionUsuario
                {
                    Status = "error",
                    Mensaje = "Token no proporcionado"
                };
            }

            if (idUsuario <= 0)
            {
                return new RespuestaActivacionUsuario
                {
                    Status = "error",
                    Mensaje = "ID de usuario no válido"
                };
            }

            if (activo != 0 && activo != 1)
            {
                return new RespuestaActivacionUsuario
                {
                    Status = "error",
                    Mensaje = "El campo activo debe ser 0 (inactivo) o 1 (activo)"
                };
            }

            try
            {
                // Crear cliente HTTP
                using var client = new HttpClient();
                client.BaseAddress = new Uri(Parametros.UrlBaseApi);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // Preparar datos para enviar
                var cambioActivacion = new CambioActivacionUsuario { Activo = activo };

                // Realizar la petición PUT
                HttpResponseMessage respuesta = await client.PutAsJsonAsync($"/api/management/gestion/usuarios/{idUsuario}/activacion", cambioActivacion);

                // Verificar respuesta
                if (respuesta.IsSuccessStatusCode)
                {
                    var resultado = await respuesta.Content.ReadFromJsonAsync<RespuestaActivacionUsuario>();
                    return resultado;
                }
                else
                {
                    // Intentar leer el mensaje de error
                    try
                    {
                        var error = await respuesta.Content.ReadFromJsonAsync<RespuestaActivacionUsuario>();
                        if (error != null)
                        {
                            return error;
                        }
                    }
                    catch { /* Ignorar errores al leer respuesta de error */ }

                    // Si no se pudo leer un mensaje de error específico
                    string mensajeError = await respuesta.Content.ReadAsStringAsync();
                    return new RespuestaActivacionUsuario
                    {
                        Status = "error",
                        Mensaje = $"Error al cambiar estado de activación. Código: {respuesta.StatusCode}, Mensaje: {mensajeError}"
                    };
                }
            }
            catch (Exception ex)
            {
                return new RespuestaActivacionUsuario
                {
                    Status = "error",
                    Mensaje = $"Error al cambiar estado de activación: {ex.Message}"
                };
            }
        }

        /// <summary>
        /// Actualiza los detalles de un usuario
        /// </summary>
        /// <param name="token">Token JWT de autenticación</param>
        /// <param name="idUsuario">ID del usuario a modificar</param>
        /// <param name="nombre">Nuevo nombre completo (opcional)</param>
        /// <param name="nombramiento">Nuevo nombramiento (opcional)</param>
        /// <param name="rol">Nuevo rol (opcional)</param>
        /// <returns>Respuesta con el resultado de la operación</returns>
        public static async Task<RespuestaActualizacionDetalles> ActualizarDetallesUsuarioAsync(
            string token, int idUsuario, string nombre = null, string nombramiento = null, string rol = null)
        {
            // Validaciones básicas
            if (string.IsNullOrEmpty(token))
            {
                return new RespuestaActualizacionDetalles
                {
                    Status = "error",
                    Mensaje = "Token no proporcionado"
                };
            }

            if (idUsuario <= 0)
            {
                return new RespuestaActualizacionDetalles
                {
                    Status = "error",
                    Mensaje = "ID de usuario no válido"
                };
            }

            // Verificar que al menos un campo tenga un valor para actualizar
            if (string.IsNullOrEmpty(nombre) && string.IsNullOrEmpty(nombramiento) && string.IsNullOrEmpty(rol))
            {
                return new RespuestaActualizacionDetalles
                {
                    Status = "error",
                    Mensaje = "Debe proporcionar al menos un campo para actualizar"
                };
            }

            // Validar el rol si se proporciona
            if (!string.IsNullOrEmpty(rol))
            {
                rol = rol.ToLower();
                if (rol != "usuario" && rol != "revisor" && rol != "moderador")
                {
                    return new RespuestaActualizacionDetalles
                    {
                        Status = "error",
                        Mensaje = "Rol no válido. Debe ser: usuario, revisor o moderador"
                    };
                }
            }

            try
            {
                // Crear cliente HTTP
                using var client = new HttpClient();
                client.BaseAddress = new Uri(Parametros.UrlBaseApi);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // Preparar datos para enviar
                var detalles = new ActualizacionDetallesUsuario
                {
                    Nombre = nombre,
                    Nombramiento = nombramiento,
                    Rol = rol
                };

                // Realizar la petición PUT
                HttpResponseMessage respuesta = await client.PutAsJsonAsync($"/api/management/gestion/usuarios/{idUsuario}/detalles", detalles);

                // Verificar respuesta
                if (respuesta.IsSuccessStatusCode)
                {
                    var resultado = await respuesta.Content.ReadFromJsonAsync<RespuestaActualizacionDetalles>();
                    return resultado;
                }
                else
                {
                    // Intentar leer el mensaje de error
                    try
                    {
                        var error = await respuesta.Content.ReadFromJsonAsync<RespuestaActualizacionDetalles>();
                        if (error != null)
                        {
                            return error;
                        }
                    }
                    catch { /* Ignorar errores al leer respuesta de error */ }

                    // Si no se pudo leer un mensaje de error específico
                    string mensajeError = await respuesta.Content.ReadAsStringAsync();
                    return new RespuestaActualizacionDetalles
                    {
                        Status = "error",
                        Mensaje = $"Error al actualizar detalles. Código: {respuesta.StatusCode}, Mensaje: {mensajeError}"
                    };
                }
            }
            catch (Exception ex)
            {
                return new RespuestaActualizacionDetalles
                {
                    Status = "error",
                    Mensaje = $"Error al actualizar detalles del usuario: {ex.Message}"
                };
            }
        }

        // Nueva iteracion

        /// <summary>
        /// Obtiene los comentarios detallados de un usuario específico
        /// </summary>
        /// <param name="token">Token JWT de autenticación</param>
        /// <param name="idUsuario">ID del usuario</param>
        /// <returns>Datos de comentarios o null si ocurre un error</returns>
        public static async Task<DatosComentariosUsuario> ObtenerComentariosUsuarioAsync(string token, int idUsuario)
        {
            // Validaciones básicas
            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine("Error: Token no proporcionado para obtener comentarios");
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
                HttpResponseMessage respuesta = await client.GetAsync($"/api/management/gestion/usuarios/{idUsuario}/comentarios");

                // Verificar respuesta
                if (respuesta.IsSuccessStatusCode)
                {
                    var contenido = await respuesta.Content.ReadFromJsonAsync<RespuestaComentariosUsuario>();

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
                    Console.WriteLine($"Error al obtener comentarios. Código: {respuesta.StatusCode}, Mensaje: {mensajeError}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Excepción al obtener comentarios del usuario: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Obtiene las notificaciones con estado de atención de un usuario específico
        /// </summary>
        /// <param name="token">Token JWT de autenticación</param>
        /// <param name="idUsuario">ID del usuario</param>
        /// <returns>Datos de notificaciones con estado o null si ocurre un error</returns>
        public static async Task<DatosNotificacionesEstado> ObtenerNotificacionesEstadoUsuarioAsync(string token, int idUsuario)
        {
            // Validaciones básicas
            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine("Error: Token no proporcionado para obtener notificaciones con estado");
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
                HttpResponseMessage respuesta = await client.GetAsync($"/api/management/gestion/usuarios/{idUsuario}/notificaciones-estado");

                // Verificar respuesta
                if (respuesta.IsSuccessStatusCode)
                {
                    var contenido = await respuesta.Content.ReadFromJsonAsync<RespuestaNotificacionesEstadoUsuario>();

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
                    Console.WriteLine($"Error al obtener notificaciones con estado. Código: {respuesta.StatusCode}, Mensaje: {mensajeError}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Excepción al obtener notificaciones con estado del usuario: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Restablece la foto de perfil de un usuario a la imagen por defecto
        /// </summary>
        /// <param name="token">Token JWT de autenticación</param>
        /// <param name="idUsuario">ID del usuario</param>
        /// <returns>Respuesta con el resultado de la operación</returns>
        public static async Task<RespuestaRestablecerFotoPerfil> RestablecerFotoPerfilUsuarioAsync(string token, int idUsuario)
        {
            // Validaciones básicas
            if (string.IsNullOrEmpty(token))
            {
                return new RespuestaRestablecerFotoPerfil
                {
                    Status = "error",
                    Mensaje = "Token no proporcionado"
                };
            }

            if (idUsuario <= 0)
            {
                return new RespuestaRestablecerFotoPerfil
                {
                    Status = "error",
                    Mensaje = "ID de usuario no válido"
                };
            }

            try
            {
                // Crear cliente HTTP
                using var client = new HttpClient();
                client.BaseAddress = new Uri(Parametros.UrlBaseApi);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // Realizar la petición PUT sin cuerpo
                HttpResponseMessage respuesta = await client.PutAsync($"/api/management/gestion/usuarios/{idUsuario}/restablecer-foto-perfil", null);

                // Verificar respuesta
                if (respuesta.IsSuccessStatusCode)
                {
                    var resultado = await respuesta.Content.ReadFromJsonAsync<RespuestaRestablecerFotoPerfil>();
                    return resultado;
                }
                else
                {
                    // Intentar leer el mensaje de error
                    try
                    {
                        var error = await respuesta.Content.ReadFromJsonAsync<RespuestaRestablecerFotoPerfil>();
                        if (error != null)
                        {
                            return error;
                        }
                    }
                    catch { /* Ignorar errores al leer respuesta de error */ }

                    // Si no se pudo leer un mensaje de error específico
                    string mensajeError = await respuesta.Content.ReadAsStringAsync();
                    return new RespuestaRestablecerFotoPerfil
                    {
                        Status = "error",
                        Mensaje = $"Error al restablecer foto de perfil. Código: {respuesta.StatusCode}, Mensaje: {mensajeError}"
                    };
                }
            }
            catch (Exception ex)
            {
                return new RespuestaRestablecerFotoPerfil
                {
                    Status = "error",
                    Mensaje = $"Error al restablecer foto de perfil: {ex.Message}"
                };
            }
        }
        /// <summary>
        /// Elimina un comentario específico por su ID
        /// </summary>
        /// <param name="token">Token JWT de autenticación</param>
        /// <param name="idComentario">ID del comentario a eliminar</param>
        /// <returns>Respuesta con el resultado de la operación</returns>
        public static async Task<RespuestaEliminarComentario> EliminarComentarioAsync(string token, int idComentario)
        {
            // Validaciones básicas
            if (string.IsNullOrEmpty(token))
            {
                return new RespuestaEliminarComentario
                {
                    Status = "error",
                    Mensaje = "Token no proporcionado"
                };
            }

            if (idComentario <= 0)
            {
                return new RespuestaEliminarComentario
                {
                    Status = "error",
                    Mensaje = "ID de comentario no válido"
                };
            }

            try
            {
                // Crear cliente HTTP
                using var client = new HttpClient();
                client.BaseAddress = new Uri(Parametros.UrlBaseApi);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // Realizar la petición DELETE
                HttpResponseMessage respuesta = await client.DeleteAsync($"/api/management/gestion/usuarios/comentarios/{idComentario}");

                // Verificar respuesta
                if (respuesta.IsSuccessStatusCode)
                {
                    var resultado = await respuesta.Content.ReadFromJsonAsync<RespuestaEliminarComentario>();
                    return resultado;
                }
                else
                {
                    // Intentar leer el mensaje de error
                    try
                    {
                        var error = await respuesta.Content.ReadFromJsonAsync<RespuestaEliminarComentario>();
                        if (error != null)
                        {
                            return error;
                        }
                    }
                    catch { /* Ignorar errores al leer respuesta de error */ }

                    // Si no se pudo leer un mensaje de error específico
                    string mensajeError = await respuesta.Content.ReadAsStringAsync();
                    return new RespuestaEliminarComentario
                    {
                        Status = "error",
                        Mensaje = $"Error al eliminar comentario. Código: {respuesta.StatusCode}, Mensaje: {mensajeError}"
                    };
                }
            }
            catch (Exception ex)
            {
                return new RespuestaEliminarComentario
                {
                    Status = "error",
                    Mensaje = $"Error al eliminar comentario: {ex.Message}"
                };
            }
        }
    }
}
