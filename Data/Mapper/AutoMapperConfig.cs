using AutoMapper;
using Data.Domain.Models;
using Data.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapper
{
    public static class AutoMapperConfig
    {
        public static IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Car, CarDto>();
                cfg.CreateMap<Company, CompanyDto>();
                cfg.CreateMap<Customer, CustomerDto>();
                
                cfg.CreateMap<Invoice, InvoiceDto>();

                cfg.CreateMap<Quote, QuoteDto>();
                
                cfg.CreateMap<Service, ServiceDto>();
                
                cfg.CreateMap<ServiceIndex, ServiceIndexDto>();
                
                cfg.CreateMap<User, UserDto>();
            });

            return config.CreateMapper();
        }
    }
}
