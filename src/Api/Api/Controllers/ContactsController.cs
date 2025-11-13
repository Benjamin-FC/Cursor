using Api.Data;
using Api.Dtos;
using Api.Domain;
using Api.Validation;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContactsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public ContactsController(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<ContactListResponse>> GetContacts(
        [FromQuery] string? query = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string sort = "lastName",
        [FromQuery] string dir = "asc",
        [FromQuery] bool? isActive = null)
    {
        var contactsQuery = _context.Contacts.AsQueryable();

        // Apply active filter
        if (isActive.HasValue)
        {
            contactsQuery = contactsQuery.Where(c => c.IsActive == isActive.Value);
        }

        // Apply search filter
        if (!string.IsNullOrEmpty(query))
        {
            contactsQuery = contactsQuery.Where(c =>
                c.FirstName.Contains(query) ||
                c.LastName.Contains(query) ||
                c.Email.Contains(query) ||
                (c.Phone != null && c.Phone.Contains(query)) ||
                (c.Company != null && c.Company.Contains(query)));
        }

        // Get total count before pagination
        var total = await contactsQuery.CountAsync();

        // Apply sorting and pagination
        var contacts = await contactsQuery
            .ApplySorting(sort, dir)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var items = _mapper.Map<List<ContactListItemDto>>(contacts);

        return Ok(new ContactListResponse
        {
            Items = items,
            Total = total,
            Page = page,
            PageSize = pageSize,
            Sort = sort,
            Dir = dir
        });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ContactDetailDto>> GetContact(Guid id)
    {
        var contact = await _context.Contacts.FindAsync(id);

        if (contact == null) return NotFound();

        var dto = _mapper.Map<ContactDetailDto>(contact);
        return Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<ContactDetailDto>> CreateContact(
        [FromBody] CreateContactDto createDto,
        [FromServices] IValidator<CreateContactDto> validator)
    {
        var validationResult = await validator.ValidateAsync(createDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.ToDictionary());
        }

        var contact = _mapper.Map<Contact>(createDto);
        contact.Id = Guid.NewGuid();
        contact.CreatedAt = DateTimeOffset.UtcNow;
        contact.UpdatedAt = DateTimeOffset.UtcNow;

        _context.Contacts.Add(contact);
        await _context.SaveChangesAsync();

        var resultDto = _mapper.Map<ContactDetailDto>(contact);
        return CreatedAtAction(nameof(GetContact), new { id = contact.Id }, resultDto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ContactDetailDto>> UpdateContact(
        Guid id,
        [FromBody] UpdateContactDto updateDto,
        [FromServices] IValidator<UpdateContactDto> validator)
    {
        var validationResult = await validator.ValidateAsync(updateDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.ToDictionary());
        }

        var contact = await _context.Contacts.FindAsync(id);
        if (contact == null) return NotFound();

        _mapper.Map(updateDto, contact);
        contact.UpdatedAt = DateTimeOffset.UtcNow;

        await _context.SaveChangesAsync();

        var resultDto = _mapper.Map<ContactDetailDto>(contact);
        return Ok(resultDto);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteContact(Guid id)
    {
        var contact = await _context.Contacts.FindAsync(id);
        if (contact == null) return NotFound();

        _context.Contacts.Remove(contact);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}

