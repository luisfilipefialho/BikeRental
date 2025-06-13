using BikeRental.Application.DTOs.Customer;
using BikeRental.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<IActionResult> Create([FromBody] CreateCustomerRequest request)
    {
        await _customerService.CreateAsync(request);
        return Created("", null);
    }

    [HttpPost("{id}/upload-cnh")]
    public async Task<IActionResult> UploadCnh(Guid id, [FromBody] UploadCnhRequest request)
    {
        await _customerService.UploadCnhAsync(id, request);
        return NoContent();
    }
}
