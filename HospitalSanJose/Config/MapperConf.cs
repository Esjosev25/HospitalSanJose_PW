using AutoMapper;
using HospitalSanJoseModel;

namespace HospitalSanJose.Config
{
    public class MapperConf : Profile
    {
        public MapperConf() {

            CreateMap<PersonalInfo, HospitalSanJoseModel.DTO.Profile.ProfileChangePassword>().ReverseMap();
        }
    }
}
