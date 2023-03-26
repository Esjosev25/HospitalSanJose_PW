using AutoMapper;
using HospitalSanJoseAPI.Models;

namespace HospitalSanJose.Config
{
    public class MapperConf : Profile
    {
        public MapperConf() {
            CreateMap<HospitalSanJoseModel.User, User>().ReverseMap();
            CreateMap<HospitalSanJoseModel.UserRole, UserRole>().ReverseMap();
        }
    }
}
