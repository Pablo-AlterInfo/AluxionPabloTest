using AluxionTest.Models.Dtos;
using AluxionTest.Models.Internal;
using AluxionTest.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AluxionTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ImageController : ControllerBase
    {
        private readonly IUnsplashRepository _unsplashRepository;
        protected ResponseDto _response;
        public ImageController(IUnsplashRepository unsplashRepository)
        {
            _unsplashRepository = unsplashRepository;
            _response = new ResponseDto();
        }

        [HttpGet]
        public async Task<IActionResult> SearchImage(string query)
        {
            var image = await _unsplashRepository.ListImagesByQuery(query);
            if (image == null)
            {
                _response.IsSuccess = false;
                _response.DisplayMessage = "Ocurrió un error al obtener las imagenes";

                return BadRequest(_response);
            }

            _response.IsSuccess = true;
            _response.DisplayMessage = "Lista de Imágenes adquirida de manera exitosa";


            return Ok(image);
        }
        [HttpGet("ImageById")]
        public async Task<IActionResult> GetImageId(string imageId)
        {
            var image = await _unsplashRepository.SelectImageById(imageId);
            var streamData = await _unsplashRepository.DownloadImage(image);

            return Ok(image);
        }



    }
}
