using AutoMapper;
using BikeStore.Core.Models;
using BikeStoreWebApi.DTOs;
using BikeStoreWebApi.DTOs.Order;
using BikeStoreWebApi.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeStoreWebApi.Mapping
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            // Domain to Resource
            CreateMap<Bike, BikeDto>();
            CreateMap<Bike, SaveBikeDto>();
            CreateMap<Order, SaveOrderDto>();
            CreateMap<Category, CategoryDto>();
            CreateMap<Category, SaveCategoryDto>();
            CreateMap<Brand, BrandDto>();
            CreateMap<Brand, SaveBrandDto>();
            CreateMap<User, UserDto>();
            CreateMap<Order, OrderDto>();

            // Resource to Domain
            CreateMap<BikeDto, Bike>();
            CreateMap<SaveBikeDto, Bike>();
            CreateMap<CategoryDto, Category>();
            CreateMap<SaveCategoryDto, Category>();
            CreateMap<BrandDto, Brand>();
            CreateMap<SaveBrandDto, Brand>();
            CreateMap<RegistrationUserDto, User>();
            CreateMap<AdminRegistrationDto, User>();
            CreateMap<OrderDto, Order>();
            CreateMap<SaveOrderDto, Order>();
        }
    }
}
