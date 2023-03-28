using HospitalSanJoseModel;
using HospitalSanJoseModel.DTO.PersonalInfo;
namespace HospitalSanJose.Functions
{
    public class PersonalInfosService : APIServices
    {
        private readonly string ControllerUrl = "api/PersonalInfos";
        public async Task<IEnumerable<PersonalInfo>> GetList()
        {

            return await Get<IEnumerable<PersonalInfo>>($"{ControllerUrl}");
        }


        public async Task<PersonalInfo?> GetById(int? id)
        {

            return await Get<PersonalInfo?>($"{ControllerUrl}/{id}");

        }
        public async Task<PersonalInfo?> GetByUserId(int? id)
        {

            return await Get<PersonalInfo?>($"{ControllerUrl}/User/{id}");

        }
        public async Task<PersonalInfo> Post(PersonalInfo personalInfo)
        {
            return await Post(personalInfo, $"{ControllerUrl}");
        }

        public async Task<PersonalInfo> Put(PersonalInfo personalInfo, int id)
        {
            return await Put(personalInfo, $"{ControllerUrl}/{id}");
        }
        public async Task Delete(int? id)
        {
            await Delete($"{ControllerUrl}/{id}");
        }

    }
}
