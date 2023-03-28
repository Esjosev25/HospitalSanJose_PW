using AutoMapper;
using HospitalSanJose.Models;
using HospitalSanJoseModel;

namespace HospitalSanJose.Config
{
    public class MapperConf : Profile
    {
        public MapperConf() {
            CreateMap<HospitalSanJoseModel.User, Models.User>().ReverseMap();
            CreateMap<HospitalSanJoseModel.UserRole, Models.UserRole>().ReverseMap();


            CreateMap<HospitalSanJoseModel.PersonalInfo, HospitalSanJoseModel.DTO.Profile.ProfileChangePassword>().ReverseMap();
        }
    }
}
