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
            CreateMap<Customer, CustomerDto>();
            CreateMap<Invoice, InvoiceDto>();
            CreateMap<Service, ServiceDto>();
            CreateMap<ServiceIndex, ServiceIndexDto>();
            CreateMap<Quote, QuoteDto>();

            // Dto
            CreateMap<CarDto, Car>();
            CreateMap<CompanyDto, Company>();
            CreateMap<CustomerDto, Customer>();
            CreateMap<InvoiceDto, Invoice>();
            CreateMap<ServiceDto, Service>();
            CreateMap<ServiceIndexDto, ServiceIndex>();
            CreateMap<UserDto, User>();
            CreateMap<QuoteDto, Quote>();

            // Others
            CreateMap<PaginationQuery, PaginationFilter>();
            CreateMap<PaginationFilter, PaginationQuery>();

            //
            CreateMap<User, UserDto>()
                .ForMember(x => x.Username, m => m.MapFrom(z => z.UserName))
                .ForMember(x => x.Password, m => m.MapFrom(z => ""))
                .ForMember(x => x.UserId, m => m.MapFrom(z => z.Id))
                ;
                
        }
    }
}
