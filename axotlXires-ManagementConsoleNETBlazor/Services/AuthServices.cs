using Microsoft.JSInterop;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace axotlXires_ManagementConsoleNETBlazor.Services
{
    public class AuthService
    {
        private readonly IJSRuntime _jsRuntime;
        private const string TOKEN_KEY = "auth_token";
        private const string USER_DATA_KEY = "user_data";
        private bool _isInitialized = false;
        private string _cachedToken = null;
        private string _cachedUserData = null;

        public AuthService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        /// <summary>
        /// Inicializa de forma segura el servicio después del renderizado
        /// </summary>
        public async Task InitializeAsync()
        {
            if (!_isInitialized)
            {
                try
                {
                    _cachedToken = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", TOKEN_KEY);
                    _cachedUserData = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", USER_DATA_KEY);

                    if (!string.IsNullOrEmpty(_cachedToken))
                    {
                        DAL.Parametros.token = _cachedToken;
                    }

                    _isInitialized = true;
                }
                catch
                {
                    // En caso de que el JS interop falle (durante prerenderizado)
                    // simplemente seguimos sin inicializar
                }
            }
        }

        public async Task GuardarSesion(string token, string userData)
        {
            try
            {
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", TOKEN_KEY, token);
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", USER_DATA_KEY, userData);

                _cachedToken = token;
                _cachedUserData = userData;
                DAL.Parametros.token = token;
                _isInitialized = true;
            }
            catch
            {
                // Si falla el JS interop, guardamos solo en caché
                _cachedToken = token;
                _cachedUserData = userData;
                DAL.Parametros.token = token;
            }
        }

        public async Task<string> ObtenerToken()
        {
            if (!_isInitialized)
            {
                // Si no estamos inicializados, devolvemos el token en caché o nulo
                return _cachedToken;
            }

            try
            {
                _cachedToken = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", TOKEN_KEY);
                if (!string.IsNullOrEmpty(_cachedToken))
                {
                    DAL.Parametros.token = _cachedToken;
                }
                return _cachedToken;
            }
            catch
            {
                // Si falla el JS interop, devolvemos el token en caché
                return _cachedToken;
            }
        }

        public async Task<string> ObtenerDatosUsuario()
        {
            if (!_isInitialized)
            {
                return _cachedUserData;
            }

            try
            {
                _cachedUserData = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", USER_DATA_KEY);
                return _cachedUserData;
            }
            catch
            {
                return _cachedUserData;
            }
        }

        public async Task CerrarSesion()
        {
            try
            {
                await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", TOKEN_KEY);
                await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", USER_DATA_KEY);
            }
            catch
            {
                // Si falla el JS interop, solo limpiamos la caché
            }

            _cachedToken = null;
            _cachedUserData = null;
            DAL.Parametros.token = null;
        }
    }
}
