using Business.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarImagesController : ControllerBase
    {
        ICarImageService _carImageService;

        public CarImagesController(ICarImageService carImageService)
        {
            _carImageService = carImageService;
        }
        [HttpPost("Add")]
        public IActionResult Add([FromForm] CarImageDto carImageDTO)
        {
            var result = _carImageService.Add(carImageDTO);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("Update")]
        public IActionResult Update([FromForm] CarImageDto carImageDTO)
        {
            var result = _carImageService.Update(carImageDTO);
            if (result.Success) 
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPost("Delete")]
        public IActionResult Delete(int id)
        {
            CarImage carToDelete = _carImageService.GetById(id).Data;
            var result = _carImageService.Delete(carToDelete);
            if (result.Success) 
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("GetByCarId")]
        public IActionResult GetByCarId(int id)
        {
            var result = _carImageService.GetByCarId(id);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var result = _carImageService.GetAll();
            if (result.Success) 
                return Ok(result);
            return BadRequest(result);
        }

    }
}
