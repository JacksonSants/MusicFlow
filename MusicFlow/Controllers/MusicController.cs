
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicFlow.Data.Service.MusicService;

namespace MusicFlow.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MusicController : ControllerBase
{
    private readonly IMusicService _musicService;

    public MusicController(IMusicService musicService)
    {
        _musicService = musicService;
    }

    //[HttpGet]
    //public async Task<IActionResult> GetAll()
    //{
    //    var musics = await _musicService.GetMusicsWithArtistAsync();
    //    return Ok(musics);
    //}

    //[HttpGet("with-artist")]
    //public async Task<IActionResult> GetMusicsWithArtist()
    //{
    //    var musics = await _musicService.GetMusicsWithArtistAsync();
    //    return Ok(musics);
    //}

    //[HttpPost]
    //public async Task<IActionResult> Create([FromBody] CreateMusicDto music)
    //{
    //     implementar criação no serviço
    //    return Ok();
    //}

    //// POST: MusicController/Create
    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //public ActionResult Create(IFormCollection collection)
    //{
    //    try
    //    {
    //        return RedirectToAction(nameof(Index));
    //    }
    //    catch
    //    {
    //        return View();
    //    }
    //}

    //// GET: MusicController/Edit/5
    //public ActionResult Edit(int id)
    //{
    //    return View();
    //}

    //// POST: MusicController/Edit/5
    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //public ActionResult Edit(int id, IFormCollection collection)
    //{
    //    try
    //    {
    //        return RedirectToAction(nameof(Index));
    //    }
    //    catch
    //    {
    //        return View();
    //    }
    //}

    //// GET: MusicController/Delete/5
    //public ActionResult Delete(int id)
    //{
    //    return View();
    //}

    //// POST: MusicController/Delete/5
    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //public ActionResult Delete(int id, IFormCollection collection)
    //{
    //    try
    //    {
    //        return RedirectToAction(nameof(Index));
    //    }
    //    catch
    //    {
    //        return View();
    //    }
    //}
}
