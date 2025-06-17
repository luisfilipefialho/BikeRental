using BikeRental.Application.DTOs.Bike;
using BikeRental.Application.Exceptions;
using BikeRental.Application.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

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

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [SwaggerOperation(Summary = "Creates a new bike", Description = "Registers a new bike in the system.")]
    [SwaggerResponse(StatusCodes.Status201Created, "Bike Created")]
    [SwaggerResponse(StatusCodes.Status409Conflict, "Conflict: Bike already exists")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error")]
    public async Task<IActionResult> Create([FromBody] CreateBikeRequest request)
    {
        try
        {
            await _bikeService.CreateAsync(request);
            return Created(string.Empty, request);
        }
        catch (DomainConflictException ex)
        {
            return Conflict(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Get bike by ID", Description = "Returns a single bike based on the provided identifier")]
    [SwaggerResponse(StatusCodes.Status200OK, "Bike details returned successfully", typeof(GetBikeResponse))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Bike not found")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error")]
    public async Task<IActionResult> GetById(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return BadRequest("Null or White Space not allowed in Identifier");
        try
        {
            var bike = await _bikeService.GetByIdAsync(id);
            return Ok(bike);
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    [SwaggerOperation(Summary = "Get all bikes", Description = "Returns a list of bikes, optionally filtered by license plate")]
    [SwaggerResponse(StatusCodes.Status200OK, "Bikes listed successfully", typeof(IEnumerable<GetBikeResponse>))]
    [SwaggerResponse(StatusCodes.Status204NoContent, "No bikes found")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error")]
    public async Task<IActionResult> GetAll([FromQuery] string? licensePlate)
    {
        var bikes = await _bikeService.GetAllAsync(licensePlate);
        if (!bikes.Any())
            return NoContent();
        return Ok(bikes);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}/licensePlate")]
    [SwaggerOperation(Summary = "Update license plate", Description = "Updates the license plate of a specific bike")]
    [SwaggerResponse(StatusCodes.Status200OK, "License plate updated")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid data")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Bike not found")]
    [SwaggerResponse(StatusCodes.Status409Conflict, "Conflict: License plate already exists")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error")]
    public async Task<IActionResult> UpdatePlate(string id, [FromBody] UpdateBikeRequest request)
    {
        if (!ModelState.IsValid && string.IsNullOrWhiteSpace(id))
            return BadRequest("Invalid data");

        try
        {
            await _bikeService.UpdatePlateAsync(id, request);
            return Ok("License plate updated");
        }
        catch (DomainConflictException ex)
        {
            return Conflict(ex.Message);
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Delete bike", Description = "Deletes a bike if it has no rentals")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Bike deleted")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Bike has rentals and cannot be deleted")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Bike not found")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            await _bikeService.DeleteAsync(id);
            return NoContent();
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (HasRentalException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
