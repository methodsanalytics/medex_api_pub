using System;
using AutoMapper;
using FluentAssertions;
using MedicalExaminer.API.Extensions.Data;
using MedicalExaminer.API.Models.V1.Examinations;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;
using Xunit;

namespace MedicalExaminer.API.Tests.Mapper
{
    public class MapperNewExaminationProfileTests
    {
        private const string GivenNames = "givenNames";
        private const string GenderDetails = "genderDetails";
        private const string HospitalNumber_1 = "hospitalNumber_1";
        private const string HospitalNumber_2 = "hospitalNumber_2";
        private const string HospitalNumber_3 = "hospitalNumber_3";
        private const string MedicalExaminerOfficeResponsible = "medicalExaminerOfficeResponsible";
        private const string NhsNumber = "123456789";
        private const string PlaceDeathOccured = "placeDeathOccured";
        private const string Surname = "surname";
        private const ExaminationGender Gender = ExaminationGender.Male;

        private readonly DateTime _dateOfBirth = new DateTime(1990, 2, 24);
        private readonly DateTime _dateOfDeath = new DateTime(2019, 2, 24);
        private readonly TimeSpan _timeOfDeath = new TimeSpan(11, 30, 00);
        private readonly IMapper _mapper;

        public MapperNewExaminationProfileTests()
        {
            var config = new MapperConfiguration(cfg => { cfg.AddProfile<NewExaminationProfile>(); });

            _mapper = config.CreateMapper();
        }

        /// <summary>
        ///     Test Mapping Examination to ExaminationItem.
        /// </summary>
        [Fact]
        public void PostNewCaseRequest_To_ExaminationItem()
        {
            var postNewCaseRequest = new PostExaminationRequest
            {
                DateOfDeath = _dateOfDeath,
                DateOfBirth = _dateOfBirth,
                GivenNames = GivenNames,
                Gender = Gender,
                GenderDetails = GenderDetails,
                HospitalNumber_1 = HospitalNumber_1,
                HospitalNumber_2 = HospitalNumber_2,
                HospitalNumber_3 = HospitalNumber_3,
                MedicalExaminerOfficeResponsible = MedicalExaminerOfficeResponsible,
                NhsNumber = NhsNumber,
                PlaceDeathOccured = PlaceDeathOccured,
                Surname = Surname,
                TimeOfDeath = _timeOfDeath
            };

            var response = _mapper.Map<ExaminationItem>(postNewCaseRequest);
            response.GenderDetails.Should().Be(GenderDetails);
            response.GivenNames.Should().Be(GivenNames);
            response.DateOfBirth.Should().Be(_dateOfBirth);
            response.DateOfDeath.Should().Be(_dateOfDeath);
            response.Gender.Should().Be(Gender);
            response.GivenNames.Should().Be(GivenNames);
            response.HospitalNumber_1.Should().Be(HospitalNumber_1);
            response.HospitalNumber_2.Should().Be(HospitalNumber_2);
            response.HospitalNumber_3.Should().Be(HospitalNumber_3);
            response.MedicalExaminerOfficeResponsible.Should().Be(MedicalExaminerOfficeResponsible);
            response.NhsNumber.Should().Be(NhsNumber);
            response.PlaceDeathOccured.Should().Be(PlaceDeathOccured);
            response.Surname.Should().Be(Surname);
            response.TimeOfDeath.Should().Be(_timeOfDeath);
        }
    }
}