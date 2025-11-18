using Application.Features.Hotels.Constants;
using Application.Features.Hotels.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities; // Namespace'e dikkat
using Domain.ValueObjects; // Address için gerekli
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;
using static Application.Features.Hotels.Constants.HotelsOperationClaims;

namespace Application.Features.Hotels.Commands.Create;

public class CreateHotelCommand : IRequest<CreatedHotelResponse>, ISecuredRequest, ICacheRemoverRequest, ILoggableRequest, ITransactionalRequest
{
    public required string Name { get; set; }
    public required string City { get; set; } // Otelin bulunduðu ana þehir

    // --- V2 GÜNCELLEMESÝ: Adres Parçalarý ---
    // Artýk tek bir "Address" string'i yerine detaylý bilgi alýyoruz.
    public required string AddressStreet { get; set; }
    public required string AddressCity { get; set; }
    public required string AddressCountry { get; set; }
    public required string AddressZipCode { get; set; }

    public string[] Roles => [Admin, Write, HotelsOperationClaims.Create];

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetHotels"];

    public class CreateHotelCommandHandler : IRequestHandler<CreateHotelCommand, CreatedHotelResponse>
    {
        private readonly IMapper _mapper;
        private readonly IHotelRepository _hotelRepository;
        private readonly HotelBusinessRules _hotelBusinessRules;

        public CreateHotelCommandHandler(IMapper mapper, IHotelRepository hotelRepository,
                                         HotelBusinessRules hotelBusinessRules)
        {
            _mapper = mapper;
            _hotelRepository = hotelRepository;
            _hotelBusinessRules = hotelBusinessRules;
        }

        public async Task<CreatedHotelResponse> Handle(CreateHotelCommand request, CancellationToken cancellationToken)
        {
            // 1. Önce Value Object'i oluþturuyoruz (Validasyonlar Address constructor'ýnda çalýþýr)
            Address address = new(
                request.AddressStreet,
                request.AddressCity,
                request.AddressCountry,
                request.AddressZipCode
            );

            // 2. Hotel Entity'sini oluþturuyoruz
            // AutoMapper kullanmak yerine manuel oluþturuyoruz çünkü input alanlarýmýz (AddressStreet vs.)
            // ile Entity alanýmýz (Address objesi) birebir eþleþmiyor.
            Hotel hotel = new()
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                City = request.City,
                Address = address // Oluþturduðumuz nesneyi atýyoruz
            };

            // 3. Kayýt (Repo)
            await _hotelRepository.AddAsync(hotel);

            // 4. Response Dönüþü
            CreatedHotelResponse response = _mapper.Map<CreatedHotelResponse>(hotel);
            return response;
        }
    }
}