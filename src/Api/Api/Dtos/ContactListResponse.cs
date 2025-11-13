namespace Api.Dtos;

public class ContactListResponse
{
    public List<ContactListItemDto> Items { get; set; } = new();
    public int Total { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public string? Sort { get; set; }
    public string? Dir { get; set; }
}

