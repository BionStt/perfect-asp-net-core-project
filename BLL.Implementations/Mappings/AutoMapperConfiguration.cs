using AutoMapper;
using BLL.Implementations.Mapping;

namespace BLL.Implementations.Mappings
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(x =>
            {
                x.AddProfile<DomainToDTOMappingProfile>();
                x.AddProfile<DTOToDomainMappingProfile>();
                x.AddProfile<DomainToDomainMappingProfile>();
            });
        }
    }
}