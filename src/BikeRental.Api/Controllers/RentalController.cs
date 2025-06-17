using BikeRental.Application.DTOs.Rental;
using BikeRental.Application.Exceptions;
using BikeRental.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BikeRental.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RentalController : ControllerBase
{
    private readonly IRentalService _rentalService;

    public RentalController(IRentalService rentalService)
    {
        _rentalService = rentalService;
    }

    [Authorize(Roles = "Costumer")]
    [HttpPost]
    [SwaggerOperation(Summary = "Create a rental", Description = "Creates a new rental for a bike and customer")]
    [SwaggerResponse(StatusCodes.Status201Created, "Rental created successfully", typeof(CreateRentalRequest))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid rental input")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Bike or customer not found")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error")]
    public async Task<IActionResult> Create([FromBody] CreateRentalRequest request)
    {
        try
        {
            await _rentalService.CreateAsync(request);
            return Created(string.Empty, request);
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(ex.Message);
        }

        catch (InvalidInputException ex)
        {
            return BadRequest(ex.Message);
        }

        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }

    [Authorize(Roles = "Costumer")]
    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Get rental by ID", Description = "Retrieves a rental's details by ID")]
    [SwaggerResponse(StatusCodes.Status200OK, "Rental found", typeof(GetRentalResponse))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid rental ID")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Rental not found")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error")]
    public async Task<IActionResult> GetById(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return BadRequest(new { message = "Invalid data" });

        try
        {
            var rental = await _rentalService.GetByIdAsync(id);

            if (rental is null)
                return NotFound(new { message = "Rental not found" });

            return Ok(rental);
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

    [Authorize(Roles = "Costumer")]
    [HttpPut("{id}/return")]
    [SwaggerOperation(Summary = "Update return date", Description = "Updates the return date for a rental")]
    [SwaggerResponse(StatusCodes.Status200OK, "Return date updated successfully")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid input")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Rental not found")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error")]
    public async Task<IActionResult> UpdateReturn(string id, [FromBody] UpdateRentalRequest request)
    {
        if (string.IsNullOrWhiteSpace(id) || !ModelState.IsValid)
            return BadRequest(new { message = "Invalid data" });

        try
        {
            await _rentalService.UpdateReturnDateAsync(id, request);
            return Ok(new { message = "Return date updated successfully" });
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidInputException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
