using NArchitecture.Core.Application.Dtos;

namespace Application.Features.Hotels.Queries.GetList;

public class GetListHotelListItemDto : IDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string City { get; set; }
    public string Address { get; set; }
}