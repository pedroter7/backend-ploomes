using AutoMapper;
using PloomesBackend.Data.Models;
using PloomesBackend.ViewModels;

namespace PloomesBackend.Config.Mappings
{
    public class DataToViewModelMappingProfile : Profile
    {
        public DataToViewModelMappingProfile()
        {
            CreateMap<ClienteModel, ClienteViewModel>()
                .ForMember(vm => vm.CriadoData, c => c.MapFrom(m => m.DataCriacao));
        }
    }
}
