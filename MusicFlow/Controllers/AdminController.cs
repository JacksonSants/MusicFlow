using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
[ApiController]
[Route("api/[controller]")]
public class AdminController : Controller
{
    private readonly IUserRepository _userRepository;
    private IMapper _mapper;

    public AdminController(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    // POST: AdminController/CreateAdmin
    [HttpPost("CreateAdmin/{level}")]
    public async Task<ActionResult> CreateUserAdmin([FromBody] CreateUserDto userDto, [FromQuery] string level)
    {
        try
        {
            if (userDto == null)
            {
                return BadRequest("User data is required.");
            }
            var userClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var levelClaim = User.FindFirst("level")?.Value;

            if(userClaim == null || levelClaim == null)
                return Unauthorized("Token inválido ou expirado.");

            if (!string.Equals(levelClaim, UserLevel.superAdmin.ToString(), StringComparison.OrdinalIgnoreCase))
                throw new UserValidationException("Acesso negado. Usuário não tem permissão para acessar esta rota.");

            User user = _mapper.Map<User>(userDto);
            user.Validate();

            switch (level.ToLower())
            {
                case "admin":
                    user.UpdateLevel(UserLevel.admin);
                    break;
                case "ordinary_user":
                    user.UpdateLevel(UserLevel.ordinary_user);
                    break;
                case "super_admin":
                    user.UpdateLevel(UserLevel.superAdmin);
                    break;
                default:
                    return BadRequest("Nível de usuário inválido. Use 'admin', 'ordinary_user' ou 'super_admin'.");
            }

            if (await _userRepository.EmailExistsAsync(user.Email))
            {
                return BadRequest("Já existe umm usuário com este Email..");
            }

            await _userRepository.CreateUser(user);

            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);

        }
        catch (UserValidationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (System.Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
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

            if (!string.Equals(levelClaim, UserLevel.superAdmin.ToString(), StringComparison.OrdinalIgnoreCase))
                throw new UserValidationException("Acesso negado. Usuário não tem permissão para acessar esta rota.");

            var users = await _userRepository.GetAllUsers();

            var result = users.Select(user => new
            {
                userId = user.Id,
                userName = user.Username,
                userEmail = user.Email,
                userProfileImage = user.ProfileImage,
                userDateCreated = user.DateCreated,
                userLastLogin = user.LastLogin,
                userSubscription = user.Subscription,
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

    // POST: AdminController/Edit/5
    [HttpPut]
    public async Task<IActionResult> UpdateUserAdmin([FromBody] UpdateUserDto userDto)
    {
        try{

            if (userDto == null)
            {
                return BadRequest("User data is required.");
            }
            var userClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var levelClaim = User.FindFirst("level")?.Value;

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId) || userClaim == null || levelClaim == null)
                return Unauthorized("Token inválido.");

            if (!string.Equals(levelClaim, UserLevel.superAdmin.ToString(), StringComparison.OrdinalIgnoreCase))
                throw new UserValidationException("Acesso negado. Usuário não tem permissão para acessar esta rota.");

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
            return BadRequest(ex.Message);
        }
        catch (System.Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPatch("UpdateAdminField")]
    public async Task<IActionResult> UpdateAdminPatch(JsonPatchDocument<UpdateUserDto> patch)
    {
        try
        {
            if (patch == null)
                return BadRequest("Patch data is required.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var adminClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var levelClaim = User.FindFirst("level")?.Value;

            if (string.IsNullOrEmpty(adminClaim) || string.IsNullOrEmpty(levelClaim))
                return Unauthorized("Token inválido.");

            if (!string.Equals(levelClaim, UserLevel.superAdmin.ToString(), StringComparison.OrdinalIgnoreCase))
                throw new UserValidationException("Acesso negado. Usuário não tem permissão para acessar esta rota.");

            var existingUser = await _userRepository.GetUserById(adminClaim);
            if (existingUser == null)
                return NotFound("Usuário não encontrado.");

            var patchUser = _mapper.Map<UpdateUserDto>(existingUser);

            patch.ApplyTo(patchUser, ModelState);

            if (!TryValidateModel(patchUser))
                return BadRequest(ModelState);

            _mapper.Map(patchUser, existingUser);

            // ✅ Validação após o mapeamento
            existingUser.Validate(isUpdate: true);

            await _userRepository.UpdateUser(adminClaim, existingUser);

            return NoContent();
        }
        catch (UserValidationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (System.Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }


    [HttpPatch("DeleteAdmin")]
    public async Task<IActionResult> DeleteAdmin(string Id)
    {
        try
        {
            if (!ModelState.IsValid || string.IsNullOrEmpty(Id))
                return BadRequest("ID inválido ou dados inválidos.");

            var adminClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var levelClaim = User.FindFirst("level")?.Value;

            if (string.IsNullOrEmpty(adminClaim))
                return Unauthorized("Token inválido.");

            if (!string.Equals(levelClaim, UserLevel.superAdmin.ToString(), StringComparison.OrdinalIgnoreCase))
                throw new UserValidationException("Acesso negado. Usuário não tem permissão para acessar esta rota.");

            var existingAdmin = await _userRepository.GetUserById(levelClaim);
            var userToDelete = await _userRepository.GetUserById(Id);

            if (existingAdmin == null || userToDelete == null)
                throw new UserNotFoundException("Usuário não encontrado.");

            var deletedUser = await _userRepository.DeleteUserById(Id);

            return Ok(new
            {
                message = "Usuário deletado com sucesso.",
                userId = deletedUser.Id,
                name = deletedUser.Username
            });
        }
        catch (UserNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (System.Exception ex)
        {
            return StatusCode(500, $"Erro interno: {ex.Message}");
        }
    }

}
