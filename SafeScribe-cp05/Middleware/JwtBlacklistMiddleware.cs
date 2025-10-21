using Microsoft.AspNetCore.Http;
using SafeScribe_cp05.Interface;
using System.IdentityModel.Tokens.Jwt; // Para acessar JwtRegisteredClaimNames
using System.Threading.Tasks;

namespace SafeScribe_cp05.Middleware
{
    public class JwtBlacklistMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtBlacklistMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ITokenBlacklistService blacklistService) // Injeção de serviço
        {
            // 1. O Middleware deve ser executado APENAS para requisições que já foram autenticadas.
            if (context.User.Identity?.IsAuthenticated == true)
            {
                // 2. Extrair o JTI (ID do JWT)
                var jti = context.User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;

                if (!string.IsNullOrEmpty(jti))
                {
                    // 3. Verificar se o JTI está na lista negra
                    var isBlacklisted = await blacklistService.IsBlacklistedAsync(jti);

                    if (isBlacklisted)
                    {
                        // 4. Interromper a requisição e retornar 401 Unauthorized
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Response.ContentType = "application/json";

                        // Retorna um JSON de erro para ser mais claro que um 401 vazio
                        await context.Response.WriteAsJsonAsync(new
                        {
                            Message = "Token revogado (Logout realizado). Por favor, faça login novamente."
                        });
                        return; // Interrompe o pipeline
                    }
                }
            }

            // Continuar para o próximo middleware (Controller)
            await _next(context);
        }
    }
}