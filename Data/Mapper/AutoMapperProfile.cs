using AutoMapper;
using Data.Domain.Models;
using Data.DTO;
using System;

namespace Data.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Car, CarDto>();
            CreateMap<Company, CompanyDto>();
            CreateMap<Service, ServiceDto>();
            CreateMap<ServiceIndex, ServiceIndexDto>();
            CreateMap<RefreshToken, RefreshTokenDto>();

            // Dto
            CreateMap<CarDto, Car>();
            CreateMap<CompanyDto, Company>();
            CreateMap<ServiceDto, Service>();
            CreateMap<ServiceIndexDto, ServiceIndex>();
            CreateMap<UserDto, User>();
            CreateMap<RefreshTokenDto, RefreshToken>();


            // mapping
            CreateMap<Invoice, InvoiceDto>()
                .ForMember(x => x.InvoiceDate, m => m.MapFrom(z => z.InvoiceDate.ToString("yyyy-MM-dd")))
                ;

            CreateMap<Quote, QuoteDto>()
                .ForMember(x => x.QuoteDate, m => m.MapFrom(z => z.QuoteDate.ToString("yyyy-MM-dd")))
                ;


            Func<string, DateTime> InvoiceStringDateTime = delegate (string stringDate)
            {
                if (DateTime.TryParse(stringDate, out var result))
                {
                    if (result.Kind == DateTimeKind.Utc)
                        result = result.ToLocalTime();
                    return result.Date;
                }
                return default;
            };

            CreateMap<InvoiceDto, Invoice>()
                .ForMember(x => x.InvoiceDate, m => m.MapFrom(z => InvoiceStringDateTime(z.InvoiceDate)));

            CreateMap<QuoteDto, Quote>()
                .ForMember(x => x.QuoteDate, m => m.MapFrom(z => InvoiceStringDateTime(z.QuoteDate)));


            CreateMap<User, UserDto>()
                .ForMember(x => x.Username, m => m.MapFrom(z => z.UserName))
                .ForMember(x => x.Password, m => m.MapFrom(z => ""))
                .ForMember(x => x.UserId, m => m.MapFrom(z => z.Id))
                ;
                
        }
    }
}
