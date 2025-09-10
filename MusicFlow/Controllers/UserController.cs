using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using MusicFlow.Data.Dto.CreateUserDto;
using MusicFlow.Data.Dto.User;
using MusicFlow.Data.Service.Authentication;
using MusicFlow.Data.Service.UserService;
using MusicFlow.Exception.UserException;
using MusicFlow.Model.User;

namespace MusicFlow.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly Authentication _authService;
    private IMapper _mapper; 

    public UserController(IUserRepository userRepository, IMapper mapper, Authentication authService)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _authService = authService;
    }

    // GET: api/User/login
    [AllowAnonymous]
    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            if (request == null || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
                return BadRequest("Email e senha são obrigatórios.");

            var user = await _userRepository.Authenticate(request.Email, request.Password);
            if (user == null)
                return Unauthorized("Usuário ou senha inválidos.");

            var token = _authService.GenerateToken(user);

            return Ok(new
            {
                token,
                userId = user.Id,
                userName = user.Username,
                userEmail = user.Email,
                level = user.Level
            });
        }
        catch (System.Exception ex)
        {
            return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
        }
    }

    [HttpGet("GetAllUsers")]
    public async Task<IActionResult> GetAllUsers()
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var levelClaim = User.FindFirst("level")?.Value;

            if (userIdClaim == null || levelClaim == null)
                return Unauthorized("Token inválido ou expirado.");

            if (!string.Equals(levelClaim, UserLevel.admin.ToString(), StringComparison.OrdinalIgnoreCase) && !string.Equals(levelClaim, UserLevel.superAdmin.ToString(), StringComparison.OrdinalIgnoreCase)
)
            {
                throw new UserValidationException("Acesso negado. Usuário não tem permissão para acessar esta rota.");
            }


            var users = await _userRepository.GetAllUsers();

            var result = users
                .Where(user => user.Level != UserLevel.superAdmin)
                .Select(user => new
                {
                    userId = user.Id,
                    userName = user.Username,
                    userEmail = user.Email,
                    level = user.Level.ToString()
    });


            return Ok(new { users = result });
        }
        catch (UserValidationException ex)
        {
            return BadRequest(new { erro = ex.Message });
        }
        catch (System.Exception ex)
        {
            return StatusCode(500, new { erro = $"Erro interno do servidor: {ex.Message}" });
        }
    }


    // GET: api/user/GetMyUser
    [HttpGet("GetById")]
    public async Task<IActionResult> GetById()
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Token inválido.");

            var user = await _userRepository.GetUserById(userId);
            if (user == null)
                return NotFound("Usuário não encontrado.");

            return Ok(new
            {
                userId = user.Id,
                userName = user.Username,
                userEmail = user.Email,
                active = user.Level,
                dateCreated = user.DateCreated
            });
        }
        catch (System.Exception ex)
        {
            return StatusCode(500, new { erro = $"Erro interno do servidor: {ex.Message}" });
        }
    }

    // POST: api/user/CreateUser
    [AllowAnonymous]
    [HttpPost("CreateUser")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDto userRequest)
    {
        try
        {
            User user = _mapper.Map<User>(userRequest);
            user.Validate();

            if (await _userRepository.EmailExistsAsync(user.Email))
                return BadRequest("Já existe um usuário com este email.");

            await _userRepository.CreateUser(user);

            return CreatedAtAction(nameof(GetById), new { id = user.Id }, new
            {
                userId = user.Id,
                userName = user.Username,
                userEmail = user.Email,
                active = user.Level,
            });
        }
        catch (UserValidationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (System.Exception ex)
        {
            return StatusCode(500, $"Erro interno: {ex.Message}");
        }
    }

    // POST: api/user/CreateUser
    [HttpPut("UpdateUser")]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDto userDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (userDto == null)
                return BadRequest("Dados do usuário são obrigatórios.");

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Token inválido.");

            var existingUser = await _userRepository.GetUserById(userId);
            if (existingUser == null)
                return NotFound("Usuário não encontrado.");

            _mapper.Map(userDto, existingUser);

            // ✅ Validação após o mapeamento
            existingUser.Validate(isUpdate: true);

            await _userRepository.UpdateUser(userId, existingUser);

            return NoContent();
        }
        catch (UserValidationException ex)
        {
            return BadRequest(new { erro = ex.Message });
        }
        catch (System.Exception ex)
        {
            return StatusCode(500, new { erro = $"Erro interno do servidor: {ex.Message}" });
        }
    }

    // POST: api/user/UpdateFieldUser/ab760c1f-fe4c-4b36-b90a-0ab38ed13fa9
    [HttpPatch("UpdateFieldUser")]
    public async Task<IActionResult> UpdateUserByField([FromBody] JsonPatchDocument<UpdateUserDto> patch)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (patch == null)
                return BadRequest("Patch inválido ou vazio.");

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Token inválido.");

            var existingUser = await _userRepository.GetUserById(userId);
            if (existingUser == null)
                return NotFound("Usuário não encontrado.");

            var patchUser = _mapper.Map<UpdateUserDto>(existingUser);

            patch.ApplyTo(patchUser, ModelState);

            if (!TryValidateModel(patchUser))
                return BadRequest(ModelState);

            _mapper.Map(patchUser, existingUser);
            await _userRepository.UpdateUser(userId, existingUser);

            return NoContent();
        }
        catch (UserValidationException ex)
        {
            return BadRequest(new { erro = ex.Message });
        }
        catch (System.Exception ex)
        {
            return StatusCode(500, new { erro = $"Erro interno do servidor: {ex.Message}" });
        }
    }

    // DELETE api/User/DeleteUser/ab760c1f-fe4c-4b36-b90a-0ab38ed13fa9
    [HttpDelete("DeleteUser")]
    public async Task<IActionResult> UserDeleteById()
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Token inválido.");

            var existingUser = await _userRepository.GetUserById(userId);
            if (existingUser == null)
                return NotFound("Usuário não encontrado.");

            var deletedUser = await _userRepository.DeleteUserById(userId);

            return Ok(new
            {
                message = "Usuário deletado com sucesso.",
                deletedUserId = deletedUser.Id,
                deletedUserName = deletedUser.Username
            });
        }
        catch (UserValidationException ex)
        {
            return BadRequest(new { erro = ex.Message });
        }
        catch (System.Exception)
        {
            return StatusCode(500, "Erro interno ao deletar usuário.");
        }
    }

}
