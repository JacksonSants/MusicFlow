using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicFlow.Data.Dto.Artist;
using MusicFlow.Data.Service.ArtistService;
using MusicFlow.Exception.UserException;
using MusicFlow.Model.Artists;
using MusicFlow.Model.User;

namespace MusicFlow.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ArtistController : Controller
{
    private readonly IArtistService _artistService;
    private IMapper _mapper;

    public ArtistController(IArtistService artistService, IMapper mapper)
    {
        _artistService = artistService;
        _mapper = mapper;
    }

    // POST: api/artist
    [HttpPost]
    public async Task<IActionResult> CreateArtist([FromBody] CreateArtistDto artistDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var levelClaim = User.FindFirst("level")?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized("User ID is required.");

        if (artistDto == null)
            return BadRequest("Artist data is required.");

        if (!string.Equals(levelClaim, UserLevel.admin.ToString(), StringComparison.OrdinalIgnoreCase) && !string.Equals(levelClaim, UserLevel.superAdmin.ToString(), StringComparison.OrdinalIgnoreCase))
            throw new UserValidationException("Acesso negado. Usuário não tem permissão para acessar esta rota.");

        var currentArtist = _mapper.Map<Artist>(artistDto);

        // ✅ Salva o artista no MongoDB
        await _artistService.CreateArtist(currentArtist);

        return CreatedAtAction(nameof(GetArtistById), new { id = currentArtist.Id }, currentArtist);
    }

    [AllowAnonymous]
    // GET: api/artist/GetById/{id}
    [HttpGet("GetById/{id}")]
    public async Task<IActionResult> GetArtistById(string Id)
    {
        try
        {
            if (!Guid.TryParse(Id, out var guidId))
                return BadRequest("Formato de ID inválido.");

            var artist = await _artistService.GetArtistById(Id);
            if (artist == null)
                return NotFound("Artista não encontrado.");

            return Ok(new
            {
                artistId = artist.Id,
                artistName = artist.Name,
                artistGenre = artist.Genre,
                artistImage = artist.ProfileImage,
                artistBiography = artist.Biography,
                artistCountry = artist.Country,
                artistFollowers = artist.Followers,
                artistVerified = artist.Verified,
                artistDateCreated = artist.DateCreated,
                artistMusics = artist.Musics
            });
        }
        catch (System.Exception ex)
        {
            return StatusCode(500, new { erro = $"Erro interno do servidor: {ex.Message}" });
        }
    }

    [AllowAnonymous]
    // GET: api/artist/GetAllArtist
    [HttpGet("GetAllArtist")]
    public async Task<IActionResult> GetAllArtists()
    {
        try
        {
            var artists = await _artistService.GetAllArtists();

            if (artists == null || !artists.Any())
                return NotFound("Nenhum artista encontrado.");

            var result = artists.Select(artist => new
            {
                Id = artist.Id,
                Name = artist.Name,
                Genre = artist.Genre,
                ProfileImage = artist.ProfileImage,
                Biography = artist.Biography,
                Country = artist.Country,
                Followers = artist.Followers,
                Verified = artist.Verified,
                DateCreated = artist.DateCreated
            });

            return Ok(result);
        }
        catch (System.Exception ex)
        {
            return StatusCode(500, new
            {
                error = "Erro interno do servidor.",
                details = ex.Message
            });
        }
    }
    
    // GET: api/artist/GetAllArtist
    [HttpPut("UpdateArtist/{Id}")]
    public async Task<IActionResult> UpdateArtist(string Id, dev dto)
    {
        try
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var levelClaim = User.FindFirstValue("level");

            var existingArtist = await _artistService.GetArtistById(Id);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User não encontrado.");
            if(existingArtist == null)
            {
                return BadRequest("Artist n~so encontrado.");
            }

            if(!string.Equals(levelClaim, UserLevel.admin.ToString(), StringComparison.OrdinalIgnoreCase) && !string.Equals(levelClaim, UserLevel.superAdmin.ToString(), StringComparison.OrdinalIgnoreCase))
                throw new UserValidationException("Acesso negado. Usuário não tem permissão para acessar esta rota.");

            var artist = _mapper.Map(dto, existingArtist);

            await _artistService.UpdateArtist(Id, artist);

            return NoContent();
        }

        catch (UserValidationException ex)
        {
            return Unauthorized(new { error = ex.Message });
        }

        catch (System.Exception ex)
        {
            return StatusCode(500, new { error = $"Erro interno do servidor: {ex.Message}" });
        }
    }
}
