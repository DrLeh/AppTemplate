using AutoMapper;
using MyApp.Core.Models;
using System.Collections.Generic;
using System.Text;
using MyApp.Core.Models;
using MyApp.Contracts;

namespace MyApp.Core.Mappers
{
    public class MyEntityProfile : Profile
    {
        public MyEntityProfile()
        {
            CreateMap<MyEntity, MyEntityView>()
                .ReverseMap()
                ;
        }
    }
}
