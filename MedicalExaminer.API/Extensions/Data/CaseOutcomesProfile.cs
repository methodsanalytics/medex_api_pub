using AutoMapper;
using MedicalExaminer.API.Models.v1.Examinations;
using MedicalExaminer.Models;

namespace MedicalExaminer.API.Extensions.Data
{
    /// <summary>
    /// New Examination Profile.
    /// </summary>
    public class CaseOutcomesProfile : Profile
    {
        /// <summary>
        /// Initialise a new instance of <see cref="CaseOutcomesProfile"/>.
        /// </summary>
        public CaseOutcomesProfile()
        {
            CreateMap<CaseOutcomes, GetExaminationsResponse>()
                .ForMember(x => x.Errors, opt => opt.Ignore());

        }
    }
}