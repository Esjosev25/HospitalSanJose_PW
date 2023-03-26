using AutoMapper;
using HospitalSanJoseAPI.Models;

namespace HospitalSanJose.Config
{
    public class MapperConf : Profile
    {
        public MapperConf() {
            //User Model
            CreateMap<HospitalSanJoseModel.User, User>().ReverseMap();
            CreateMap<HospitalSanJoseModel.UserCreateDTO, User>().ReverseMap();
            CreateMap<HospitalSanJoseModel.UserUpdateDTO, User>().ReverseMap();
            //UserRole Model
            CreateMap<HospitalSanJoseModel.UserRole, UserRole>().ReverseMap();
        }
    }
}
