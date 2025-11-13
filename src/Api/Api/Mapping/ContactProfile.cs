using Api.Domain;
using Api.Dtos;
using AutoMapper;

namespace Api.Mapping;

public class ContactProfile : Profile
{
    public ContactProfile()
    {
        // Entity to DTO mappings
        CreateMap<Contact, ContactListItemDto>();
        CreateMap<Contact, ContactDetailDto>();

        // DTO to Entity mappings
        CreateMap<CreateContactDto, Contact>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

        CreateMap<UpdateContactDto, Contact>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
    }
}

