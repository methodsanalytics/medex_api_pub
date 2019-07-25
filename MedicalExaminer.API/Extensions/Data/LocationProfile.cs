using AutoMapper;
using MedicalExaminer.API.Models.v1.Locations;
using MedicalExaminer.Models;

namespace MedicalExaminer.API.Extensions.Data
{
    /// <summary>
    /// Location Profile
    /// </summary>
    public class LocationProfile : Profile
    {
        public LocationProfile()
        {
            CreateMap<Location, LocationItem>();
        }
    }
}
