using AutoMapper;
using HospitalSanJoseAPI.Models;

namespace HospitalSanJose.Config
{
    public class MapperConf : Profile
    {
        public MapperConf() {
            #region "User Model"
            CreateMap<HospitalSanJoseModel.User, User>().ReverseMap();
            CreateMap<HospitalSanJoseModel.UserCreateDTO, User>().ReverseMap();
            CreateMap<HospitalSanJoseModel.UserUpdateDTO, User>().ReverseMap();
            #endregion
            #region "UserRole Model"
            CreateMap<HospitalSanJoseModel.UserRole, UserRole>().ReverseMap();
            #endregion
        }
    }
}
