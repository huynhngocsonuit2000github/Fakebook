using Fakebook.AIO.Entity;
using Fakebook.AIO.Models;
using Fakebook.AIO.Services;
using Microsoft.AspNetCore.Mvc;

namespace Fakebook.AIO.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class CasesController : ControllerBase
{

    private readonly IConfiguration _configuration;
    private readonly ICaseService _caseService;
    public CasesController(IConfiguration configuration, ICaseService caseService)
    {
        _configuration = configuration;
        _caseService = caseService;
    }


    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var data = await _caseService.GetAllAsync();

        return Ok(data);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(string id)
    {
        var cas = await _caseService.GetByIdAsync(id);

        if (cas is null)
            return NotFound("Case not found");

        return Ok(cas);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(CaseCreateModel cas)
    {
        var newCas = await _caseService.CreateAsync(cas.ToCase());

        return Ok(newCas);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateAsync(string id, CaseUpdateModel cas)
    {
        await _caseService.UpdateAsync(id, cas.ToCase());

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(string id)
    {
        await _caseService.DeleteAsync(id);

        return NoContent();
    }
}
