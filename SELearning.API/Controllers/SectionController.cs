﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using SELearning.Infrastructure.Authorization;

namespace SELearning.API.Controllers;

[ApiController]
[Authorize]
[Route("/Api/[controller]")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class SectionController : ControllerBase
{
    private readonly ILogger<SectionController> _logger;
    private readonly ISectionService _service;

    public SectionController(ILogger<SectionController> logger, ISectionService service)
    {
        _logger = logger;
        _service = service;
    }

    /// <summary>
    /// <c>GetContentsBySectionID</c> returns all contents in the section with the given section ID.
    /// </summary>
    /// <param name="id">The ID of the section.</param>
    /// <returns>A collection of contents in the section if it exists, otherwise response type 404: Not Found.</returns>
    [HttpGet("{id:int}/Content")]
    [ProducesResponseType(typeof(IReadOnlyCollection<ContentDTO>), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<IReadOnlyCollection<ContentDTO>>> GetContentsBySectionId(int id)
    {
        try
        {
            return Ok(await _service.GetContentInSection(id));
        }
        catch (SectionNotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// <c>GetSection</c> returns the section with the given ID.
    /// </summary>
    /// <param name="id">The ID of the section.</param>
    /// <returns>A section with the given ID if it exists, otherwise response type 404: Not Found.</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(SectionDTO), 200)]
    [ProducesResponseType(404)]
    [ActionName(nameof(GetSection))]
    public async Task<ActionResult<SectionDTO>> GetSection(int id)
    {
        try
        {
            return Ok(await _service.GetSection(id));
        }
        catch (SectionNotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// <c>GetSections</c> returns all sections.
    /// </summary>
    /// <returns>all sections if they can be found, otherwise response type 404: Not Found.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<SectionDTO>), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<IReadOnlyCollection<SectionDTO>>> GetAllSections()
    {
        return Ok(await _service.GetSections());
    }

    /// <summary>
    /// <c>CreateSection</c> creates a section.
    /// </summary>
    /// <param name="section">The record of the section.</param>
    /// <returns>A response type 201: Created</returns>
    [HttpPost]
    [ProducesResponseType(201)]
    [AuthorizePermission(Permission.CreateSection)]
    public async Task<IActionResult> CreateSection(SectionCreateDTO section)
    {
        var createdSection = await _service.AddSection(section);
        return CreatedAtAction(nameof(GetSection), new { ID = createdSection.Id }, createdSection);
    }

    /// <summary>
    /// <c>UpdateSection</c> updates the section with the given ID.
    /// </summary>
    /// <param name="id">The ID of the section.</param>
    /// <param name="section">The record of the updated section.</param>
    /// <returns>A response type 204: No Content if the section exists, otherwise response type 404: Not Found.</returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [AuthorizePermission(Permission.EditSection)]
    public async Task<IActionResult> UpdateSection(int id, SectionUpdateDTO section)
    {
        try
        {
            await _service.UpdateSection(id, section);
            return NoContent();
        }
        catch (SectionNotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// <c>DeleteSection</c> deletes the section with the given ID.
    /// </summary>
    /// <param name="id">The ID of the section.</param>
    /// <returns>A response type 204: No Content if the section exists, otherwise response type 404: Not Found.</returns>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [AuthorizePermission(Permission.DeleteSection)]
    public async Task<IActionResult> DeleteSection(int id)
    {
        try
        {
            await _service.DeleteSection(id);
            return NoContent();
        }
        catch (SectionNotFoundException)
        {
            return NotFound();
        }
    }
}
