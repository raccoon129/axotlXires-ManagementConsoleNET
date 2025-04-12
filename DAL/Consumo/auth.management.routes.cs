using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using DAL.Modelos;

namespace DAL.Consumo
{
    public class auth
    {
        public static async Task<LoginRespuesta> LoginAdministradorAsync(string correo, string contrasena)
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri(Parametros.ApiBaseAddress);

            ModeloLogin datos = new ModeloLogin
            {
                correo = correo,
                contrasena = contrasena
            };

            try
            {
                HttpResponseMessage respuesta = await client.PostAsJsonAsync("/api/management/auth/login", datos);

                if (respuesta.IsSuccessStatusCode)
                {
                    var login = await respuesta.Content.ReadFromJsonAsync<LoginRespuesta>();
                    Parametros.token = login.datos.token; // Guardamos el token en memoria para uso posterior
                    return login;
                }
                else
                {
                    var error = await respuesta.Content.ReadFromJsonAsync<LoginRespuesta>();
                    return error;
                }
            }
            catch (Exception ex)
            {
                return new LoginRespuesta
                {
                    status = "error",
                    mensaje = $"Error al conectar con el servidor: {ex.Message}"
                };
            }
        }
    }
}
