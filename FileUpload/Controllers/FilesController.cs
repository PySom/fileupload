using AuthWebApi.Asset.Contract;
using AuthWebApi.Asset.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;

namespace AuthWebApi.Presentation
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class FilesController : ControllerBase
    {
        public FilesController()
        {

        }
        protected readonly IImageService _img;
        public FilesController(IImageService image)
        {
            _img = image;
        }
        [HttpPost("upload")]
        public IActionResult Post([FromForm]FileViewModel model)
        {
            if(ModelState.IsValid && model.File != null)
            {
                if(_img.Create(model.File, out string path))
                {
                    return Ok(new { Name = path });
                }
                return BadRequest(new { Message = "We could not add this resource. Please try again" });
            }
            return BadRequest(new { Message = "Your data is bad" });
        }

        [HttpPut("edit")]
        public IActionResult Put([FromForm]FileEditViewModel model)
        {
            if (ModelState.IsValid && model.File != null)
            {
                if (_img.Edit(model.File, model.OldImage, out string path))
                {
                    return Ok(new { Name = path });
                }
                return BadRequest(new { Message = "We could not add this resource. Please try again" });
            }
            return BadRequest(new { Message = "Your data is bad" });
        }

        [HttpGet("download")]
        public IActionResult Get([FromQuery]string url)
        {

            try
            {
                if (!string.IsNullOrEmpty(url))
                {
                    return File(System.IO.File.OpenRead(url), "image/png", Path.GetFileName(url));
                }
                return NotFound();

            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpDelete("delete")]
        public IActionResult Delete([FromBody]FileDeleteViewModel model)
        {
            if (ModelState.IsValid)
            {
                _img.Delete(model.Image);
                return NoContent();
            }
            return BadRequest(new { Message = "you need to supply an image to remove" });
        }
    }
}


