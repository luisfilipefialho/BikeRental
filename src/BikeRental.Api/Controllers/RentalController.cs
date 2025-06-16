using BikeRental.Application.DTOs.Rental;
using BikeRental.Application.Exceptions;
using BikeRental.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

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

    [HttpPost]
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

    [HttpGet("{id}")]
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

    [HttpPut("{id}/return")]
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
            return BadRequest(ex.Message);
        }
    }
}
