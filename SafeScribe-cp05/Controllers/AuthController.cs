// ... usings ...

using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SafeScribe_cp05.DTO;
using SafeScribe_cp05.Interface;

namespace SafeScribe_cp05.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;

        public AuthController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        // ----------------------------------------------------------------------
        // ENDPOINT 1: REGISTRAR (Ajustado para usar user.Password)
        // ----------------------------------------------------------------------
        [HttpPost("registrar")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Registrar([FromBody] UserRegisterDTO user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // CORREÇÃO: Chama RegisterUserAsync passando user.Password (senha limpa)
            var success = await _tokenService.RegisterUserAsync(
                user.Username,
                user.Password, // << Passando a senha limpa
                user.Role);

            if (!success)
            {
                return BadRequest(new { Message = "Nome de usuário já está em uso." });
            }

            return StatusCode(StatusCodes.Status201Created, new { Message = "Usuário registrado com sucesso." });
        }


        // ----------------------------------------------------------------------
        // ENDPOINT 2: LOGIN (Ajustado para usar user.Password)
        // ----------------------------------------------------------------------
        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // CORREÇÃO: Chama LoginAndGenerateTokenAsync passando user.Password (senha limpa)
            var token = await _tokenService.LoginAndGenerateTokenAsync(
                user.Username,
                user.Password); // << Passando a senha limpa

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized(new { Message = "Credenciais inválidas." });
            }

            return Ok(new { Token = token });
        }

        // ----------------------------------------------------------------------
        // ENDPOINT DE TESTE AUTORIZADO (Requerido no final)
        // ----------------------------------------------------------------------
        [HttpGet("dados-protegidos")]
        [Authorize]
        public IActionResult GetDadosProtegidos()
        {
            // Captura o ID do usuário e a Role a partir do token (Claims)
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            return Ok($"Olá Usuário {userId}, sua função é {userRole}! Acesso protegido concedido.");
        }
    }
}