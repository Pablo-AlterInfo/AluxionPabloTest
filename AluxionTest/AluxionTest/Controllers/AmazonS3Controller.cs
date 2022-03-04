using AluxionTest.Models.Dtos;
using AluxionTest.Repositories;
using AluxionTest.Repositories.Interfaces;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace AluxionTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AmazonS3Controller : ControllerBase
    {
        private readonly IAmazonS3 amazonS3;
        private readonly IAmazonS3Repository _amazonS3Repository;
        private readonly IUnsplashRepository _unsplashRepository;
        protected ResponseDto _response;

        public AmazonS3Controller(IAmazonS3 amazonS3, IAmazonS3Repository amazonS3Repository, IUnsplashRepository unsplashRepository)
        {
            this.amazonS3 = amazonS3;
            _amazonS3Repository = amazonS3Repository;
            _response = new ResponseDto();
            _unsplashRepository = unsplashRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] IFormFile file)
        {
            var response = await _amazonS3Repository.UploadFile(file);
            if (response == null)
            {
                _response.IsSuccess = false;
                _response.DisplayMessage = "Error al subir el fichero";
                return BadRequest(_response);
            }

            _response.DisplayMessage = "Se subió el fichero de manera exitosa";

            return Ok(_response);

        }


        [HttpPost("FromImageId")]
        public async Task<IActionResult> PostFromImageId(string id)
        {
            var image = await _unsplashRepository.SelectImageById(id);
            if (image == null)
            {
                _response.IsSuccess = false;
                _response.DisplayMessage = "Error al subir el fichero";
                return BadRequest(_response);
            }
            var streamData = await _unsplashRepository.DownloadImage(image);
            var response = await _amazonS3Repository.UploadFileFromStream(streamData);
            if (response == null)
            {
                _response.IsSuccess = false;
                _response.DisplayMessage = "Error al subir el fichero";
                return BadRequest(_response);
            }

            _response.DisplayMessage = "Se subió el fichero de manera exitosa";
            return Ok(_response);
        }

        [HttpGet("Object")]
        public async Task<IActionResult> Get([FromQuery] string fileName)
        {
            FileStreamResult result = await _amazonS3Repository.DownloadFile(fileName, "bucket-prueba-pablo");
            if (result == null)
            {
                _response.IsSuccess = false;
                _response.DisplayMessage = "Error al descargar el fichero";
                return BadRequest(_response);
            }
            return result;
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] string fileName)
        {
            var result = _amazonS3Repository.DeleteS3Object(fileName);
            if (result == null)
            {
                _response.IsSuccess = false;
                _response.DisplayMessage = "No se pudo eliminar el archivo";
                return BadRequest(_response);
            }
            _response.IsSuccess = true;
            _response.DisplayMessage = "Archivo eliminado de manera exitosa";
            //_response.Result = result;
            return Ok(_response);
        }

        [HttpGet("List")]
        public async Task<IActionResult> List(string bucketName)
        {
            var result = await _amazonS3Repository.ListBucketObjects("asd");
            return Ok(result);
        }
        [HttpGet("ObjectURL")]
        public async Task<IActionResult> ObjectURL(string objectName)
        {
            var result = await _amazonS3Repository.Get3ObjectURL(objectName);
            if (result == "error")
            {
                _response.IsSuccess = false;
                _response.DisplayMessage = "Ocurrió un error";
                return BadRequest(_response);
            }
            _response.IsSuccess = true;

            _response.DisplayMessage = "URL obtenida con éxito";
            return Ok(result);
        }
        [HttpPut("Rename")]
        public async Task<IActionResult> RenameS3Object(string objectName, string newObjectName)
        {
            var resultRename = await _amazonS3Repository.CopyS3OBject(objectName, newObjectName);
            if (resultRename == null)
            {
                _response.IsSuccess = false;
                _response.DisplayMessage = "Error al copiar el objeto original";
                return BadRequest(_response);
            }
            var resultDelete = await _amazonS3Repository.DeleteS3Object(objectName);
            if (resultDelete == null)
            {
                _response.IsSuccess = false;
                _response.DisplayMessage = "Error al eliminar el objeto original";
                return BadRequest(_response);
            }
            _response.DisplayMessage = "Objeto renombrado de manera exitosa";

            return Ok(_response);
        }
    }
}
