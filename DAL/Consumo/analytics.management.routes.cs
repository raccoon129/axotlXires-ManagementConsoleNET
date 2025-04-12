// DAL/Consumo/analyticsRoutes.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DAL.Modelos;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Net.Http.Json;

namespace DAL.Consumo
{
    public class analyticsRoutes
    {
        public class AnaliticasManagementRoutes
        {
            protected readonly HttpClient _httpClient;

            public AnaliticasManagementRoutes(HttpClient httpClient)
            {
                _httpClient = httpClient;
            }

            public async Task<ModeloDashboardAdm?> ObtenerDashboardAsync(string token)
            {
                if (string.IsNullOrEmpty(token))
                {
                    Console.WriteLine("Error: Token no válido");
                    return null;
                }

                try
                {
                    // Crear una copia del HttpClient para no modificar el original
                    using var client = new HttpClient();
                    client.BaseAddress = new Uri(DAL.Parametros.UrlBaseApi);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    // Realizar la petición
                    var response = await client.GetAsync("/api/management/analytics/dashboard");

                    // Verificar si la respuesta fue exitosa
                    if (!response.IsSuccessStatusCode)
                    {
                        string mensajeError = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"Error al obtener dashboard: {response.StatusCode} - {mensajeError}");
                        return null;
                    }

                    // Leer y deserializar la respuesta
                    var jsonString = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Respuesta recibida: {jsonString}");

                    var jsonDoc = JsonDocument.Parse(jsonString);

                    // Verificar que la respuesta tenga el formato esperado
                    if (!jsonDoc.RootElement.TryGetProperty("datos", out JsonElement datos))
                    {
                        Console.WriteLine("La respuesta no contiene la propiedad 'datos'");
                        return null;
                    }

                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    return JsonSerializer.Deserialize<ModeloDashboardAdm>(datos.ToString(), options);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Excepción al obtener dashboard: {ex.Message}");
                    return null;
                }
            }
        }
    }
}
