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
            CreateMap<DTO.Auth.Register, User>().ReverseMap();
            #endregion

            #region "PersonalInfo Model"
            CreateMap<HospitalSanJoseModel.PersonalInfo, PersonalInfo>().ReverseMap();
            CreateMap<DTO.PersonalInfo.PersonalInfoCreate, PersonalInfo>().ReverseMap();
            CreateMap<DTO.PersonalInfo.PersonalInfoUpdate, PersonalInfo>().ReverseMap();
            #endregion

            #region "Role Model"
            CreateMap<HospitalSanJoseModel.Role, Role>().ReverseMap();
            #endregion

            #region "UserRole Model"
            CreateMap<HospitalSanJoseModel.UserRole, UserRole>().ReverseMap();
            CreateMap<DTO.UserRoles.UserRolesCreate, UserRole>().ReverseMap();
            #endregion

            #region "Department"
            CreateMap<HospitalSanJoseModel.Department, Department>().ReverseMap();
            CreateMap<DTO.Department.DepartmentCreate, Department>().ReverseMap();
            #endregion

            #region "Doctor"

            CreateMap<HospitalSanJoseModel.Doctor, Doctor>().ReverseMap();
            CreateMap<DTO.Doctor.DoctorCreate, Doctor>().ReverseMap();
            #endregion

            #region "DoctorDepartment"

            CreateMap<HospitalSanJoseModel.DoctorDepartment, DoctorDepartment>().ReverseMap();
            CreateMap<DTO.DoctorDepartment.DoctorDepartmentCreate, DoctorDepartment>().ReverseMap();
            #endregion
            #region "Appointments"
            CreateMap<HospitalSanJoseModel.Appointment, Appointment>().ReverseMap();
            CreateMap<DTO.Appointment.AppointmentCreate, Appointment>().ReverseMap();
            #endregion
        }
    }
}
