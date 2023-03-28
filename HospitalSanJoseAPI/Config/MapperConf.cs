using AutoMapper;
using HospitalSanJoseAPI.Models;
using DTO = HospitalSanJoseModel.DTO;
namespace HospitalSanJose.Config
{
    public class MapperConf : Profile
    {
        public MapperConf() {
            #region "User Model"
            CreateMap<HospitalSanJoseModel.User, User>().ReverseMap();
            CreateMap<DTO.User.UserCreate, User>().ReverseMap();
            CreateMap<DTO.User.UserUpdate, User>().ReverseMap();
            #endregion

            #region "PersonalInfo Model"
            CreateMap<HospitalSanJoseModel.PersonalInfo, PersonalInfo>().ReverseMap();
            CreateMap<DTO.PersonalInfo.PersonalInfoCreate, PersonalInfo>().ReverseMap();
            CreateMap<DTO.PersonalInfo.PersonalInfoUpdate, PersonalInfo>().ReverseMap();
            #endregion

            #region "UserRole Model"
            CreateMap<HospitalSanJoseModel.UserRole, UserRole>().ReverseMap();
            #endregion
        }
    }
}
