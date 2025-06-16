using BikeRental.Application.DTOs.Customer;
using BikeRental.Application.Exceptions;
using BikeRental.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BikeRental.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomerController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Create a new customer", Description = "Registers a new customer")]
    [SwaggerResponse(StatusCodes.Status201Created, "Customer created", typeof(CreateCustomerRequest))]
    [SwaggerResponse(StatusCodes.Status409Conflict, "Conflict: CNPJ or CNH number already exists")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error")]
    public async Task<IActionResult> Create([FromBody] CreateCustomerRequest request)
    {
        try
        {
            await _customerService.CreateAsync(request);
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

    [HttpPost("{id}/upload-cnh")]
    [SwaggerOperation(Summary = "Upload CNH image", Description = "Uploads a CNH image for a customer")]
    [SwaggerResponse(StatusCodes.Status200OK, "CNH uploaded successfully")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid base64 data or unsupported file type")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Customer not found")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error")]
    public async Task<IActionResult> UploadCnh(string id, [FromBody] UploadCnhRequest request)
    {
        try
        {
            await _customerService.UploadCnhAsync(id, request);
            return Ok("Uploaded");
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
