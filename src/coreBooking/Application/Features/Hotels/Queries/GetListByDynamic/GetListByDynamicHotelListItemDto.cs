using NArchitecture.Core.Application.Dtos;

namespace Application.Features.Hotels.Queries.GetListByDynamic;

public class GetListByDynamicHotelListItemDto : IDto
{
    public string Name { get; set; }
    public string City { get; set; }
    public string Address { get; set; }
}
