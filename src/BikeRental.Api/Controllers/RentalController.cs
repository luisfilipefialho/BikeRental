using BikeRental.Application.DTOs.Rental;
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
        await _rentalService.CreateAsync(request);
        return Created("", null);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetRentalResponse>> GetById(Guid id)
    {
        var rental = await _rentalService.GetByIdAsync(id);
        return rental is not null ? Ok(rental) : NotFound();
    }

    [HttpPut("{id}/return")]
    public async Task<IActionResult> UpdateReturn(Guid id, [FromBody] UpdateRentalRequest request)
    {
        await _rentalService.UpdateReturnDateAsync(id, request);
        return NoContent();
    }
}
