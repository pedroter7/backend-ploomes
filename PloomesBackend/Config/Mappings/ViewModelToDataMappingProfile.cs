using AutoMapper;
using PloomesBackend.Data.Models;
using PloomesBackend.ViewModels;

namespace PloomesBackend.Config.Mappings
{
    public class ViewModelToDataMappingProfile : Profile
    {
        public ViewModelToDataMappingProfile()
        {
            CreateMap<CriarClienteViewModel, CriarClienteModel>();
        }
    }
}
