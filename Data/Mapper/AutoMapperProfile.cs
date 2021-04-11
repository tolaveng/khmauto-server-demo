using AutoMapper;
using Data.Api.Common;
using Data.Domain.Common;
using Data.Domain.Models;
using Data.DTO;
using System;
using System.Collections.Generic;
using System.Text;

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
            CreateMap<Quote, QuoteDto>();

            // Dto
            CreateMap<CarDto, Car>();
            CreateMap<CompanyDto, Company>();
            CreateMap<ServiceDto, Service>();
            CreateMap<ServiceIndexDto, ServiceIndex>();
            CreateMap<UserDto, User>();
            CreateMap<QuoteDto, Quote>();

            // Others
            CreateMap<PaginationQuery, PaginationFilter>();
            CreateMap<PaginationFilter, PaginationQuery>();



            // mapping
            CreateMap<Invoice, InvoiceDto>()
                .ForMember(x => x.InvoiceDate, m => m.MapFrom(z => z.InvoiceDate.ToString("yyyy-MM-dd")))
                ;


            Func<string, DateTime> InvoiceStringDateTime = delegate (string stringDate)
            {
                {
                    if (DateTime.TryParse(stringDate, out var result))
                    {
                        if (result.Kind == DateTimeKind.Utc)
                            result = result.ToLocalTime();
                        return result.Date;
                    }
                    return default;
                }
            };

            CreateMap<InvoiceDto, Invoice>()
                .ForMember(x => x.InvoiceDate, m => m.MapFrom(z => InvoiceStringDateTime(z.InvoiceDate)));


            CreateMap<User, UserDto>()
                .ForMember(x => x.Username, m => m.MapFrom(z => z.UserName))
                .ForMember(x => x.Password, m => m.MapFrom(z => ""))
                .ForMember(x => x.UserId, m => m.MapFrom(z => z.Id))
                ;
                
        }
    }
}
