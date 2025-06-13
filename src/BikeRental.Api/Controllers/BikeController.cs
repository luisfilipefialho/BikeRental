using BikeRental.Application.DTOs.Bike;
using BikeRental.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BikeRental.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BikeController : ControllerBase
{
    private readonly IBikeService _bikeService;

    public BikeController(IBikeService bikeService)
    {
        _bikeService = bikeService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBikeRequest request)
    {
        await _bikeService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = request.Identifier }, null);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetBikeResponse>> GetById(Guid id)
    {
        var bike = await _bikeService.GetByIdAsync(id);
        return Ok(bike);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetBikeResponse>>> GetAll([FromQuery] string? plate)
    {
        var bikes = await _bikeService.GetAllAsync(plate);
        return Ok(bikes);
    }

    [HttpPut("{id}/placa")]
    public async Task<IActionResult> UpdatePlate(Guid id, [FromBody] UpdateBikeRequest request)
    {
        await _bikeService.UpdatePlateAsync(id, request);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _bikeService.DeleteAsync(id);
        return NoContent();
    }
}