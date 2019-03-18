﻿using System;
using System.Collections.Generic;
using AutoMapper;
using FluentAssertions;
using MedicalExaminer.API.Extensions.Data;
using MedicalExaminer.API.Models.v1.Examinations;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;
using Xunit;

namespace MedicalExaminer.API.Tests.Mapper
{
    /// <summary>
    ///     Mapper Examination Profile Tests
    /// </summary>
    public class MapperExaminationProfileTests
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="MapperExaminationProfileTests" /> class.
        /// </summary>
        public MapperExaminationProfileTests()
        {
            var config = new MapperConfiguration(cfg => { cfg.AddProfile<ExaminationProfile>(); });

            mapper = config.CreateMapper();
        }

        private const string ExaminationId = "expectedExaminationId";
        private const string AltLink = "altLink";
        private const bool AnyImplants = true;
        private const bool AnyPersonalEffects = true;
        private const bool ChildPriority = true;
        private const bool Completed = true;
        private const bool CoronerPriority = true;
        private readonly CoronerStatus CoronerStatus = CoronerStatus.SentAwaitingConfirm;
        private const string County = "Cheshire";
        private const string Country = "England";
        private const bool CulturalPriority = true;
        private readonly DateTime DateOfBirth = new DateTime(1990, 2, 24);
        private readonly DateTime DateOfDeath = new DateTime(2019, 2, 24);
        private const string FuneralDirectors = "funeralDirectors";
        private const bool FaithPriority = true;
        private const string GivenNames = "givenNames";
        private readonly ExaminationGender Gender = ExaminationGender.Male;
        private const string GenderDetails = "genderDetails";
        private const string HospitalNumber_1 = "hospitalNumber_1";
        private const string HospitalNumber_2 = "hospitalNumber_2";
        private const string HospitalNumber_3 = "hospitalNumber_3";
        private const string HouseNameNumber = "houseNameNumber";
        private const string ImplantDetails = "implantDetails";
        private const string LastOccupation = "lastOccupation";
        private const string MedicalExaminerOfficeResponsible = "medicalExaminerOfficeResponsible";
        private readonly ModeOfDisposal ModeOfDisposal = ModeOfDisposal.BuriedAtSea;
        private const string NhsNumber = "123456789";
        private const string OrganisationCareBeforeDeathLocationId = "organisationCareBeforeDeathLocationId";
        private const bool OtherPriority = true;
        private const bool OutOfHours = true;
        private const string PersonalEffectDetails = "personalEffectDetails";
        private const string Postcode = "postcode";
        private const string PlaceDeathOccured = "placeDeathOccured";
        private const string PriorityDetails = "priorityDetails";
        private const string Surname = "surname";
        private const string Street = "street";
        private const string Town = "town";
        private readonly TimeSpan TimeOfDeath = new TimeSpan(11, 30, 00);

        private readonly IEnumerable<Representative> Representatives = new List<Representative>
        {
            new Representative
            {
                AppointmentDate = new DateTime(2019, 2, 24),
                AppointmentTime = new TimeSpan(11, 30, 0),
                FullName = "fullName",
                Informed = Informed.Yes,
                PhoneNumber = "123456789",
                PresentAtDeath = PresentAtDeath.Yes,
                Relationship = "relationship"
            }
        };

        /// <summary>
        ///     Mapper.
        /// </summary>
        private readonly IMapper mapper;

        private IEnumerable<Representative> GetRepresentatives(int numberToCreate)
        {
            var representatives = new List<Representative>(numberToCreate);
            for (var counter = 0; counter < numberToCreate; counter++)
                representatives.Add(new Representative
                {
                    AppointmentDate = new DateTime(2019, 2, 24),
                    AppointmentTime = new TimeSpan(11, 30, 0),
                    FullName = "fullName",
                    Informed = Informed.Yes,
                    PhoneNumber = "123456789",
                    PresentAtDeath = PresentAtDeath.Yes,
                    Relationship = "relationship"
                });

            return representatives;
        }


        /// <summary>
        ///     Test Mapping Examination to ExaminationItem.
        /// </summary>
        [Fact]
        public void Examination_To_ExaminationItem()
        {
            var expectedExaminationId = "expectedExaminationId";

            var examination = new Examination
            {
                ExaminationId = ExaminationId,
                AltLink = AltLink,
                AnyImplants = AnyImplants,
                AnyPersonalEffects = AnyPersonalEffects,
                ChildPriority = ChildPriority,
                Completed = Completed,
                CoronerPriority = CoronerPriority,
                CoronerStatus = CoronerStatus,
                County = County,
                Country = Country,
                CulturalPriority = CulturalPriority,
                DateOfBirth = DateOfBirth,
                DateOfDeath = DateOfDeath,
                FuneralDirectors = FuneralDirectors,
                FaithPriority = FaithPriority,
                GivenNames = GivenNames,
                Gender = Gender,
                GenderDetails = GenderDetails,
                HospitalNumber_1 = HospitalNumber_1,
                HospitalNumber_2 = HospitalNumber_2,
                HospitalNumber_3 = HospitalNumber_3,
                HouseNameNumber = HouseNameNumber,
                ImplantDetails = ImplantDetails,
                LastOccupation = LastOccupation,
                MedicalExaminerOfficeResponsible = MedicalExaminerOfficeResponsible,
                ModeOfDisposal = ModeOfDisposal,
                NhsNumber = NhsNumber,
                OrganisationCareBeforeDeathLocationId = OrganisationCareBeforeDeathLocationId,
                OtherPriority = OtherPriority,
                OutOfHours = OutOfHours,
                PersonalEffectDetails = PersonalEffectDetails,
                Postcode = Postcode,
                PlaceDeathOccured = PlaceDeathOccured,
                PriorityDetails = PriorityDetails,
                Representatives = Representatives,
                Surname = Surname,
                Street = Street,
                Town = Town,
                TimeOfDeath = TimeOfDeath
            };

            var response = mapper.Map<ExaminationItem>(examination);
            response.GenderDetails.Should().Be(GenderDetails);
            response.ExaminationId.Should().Be(expectedExaminationId);
            response.GivenNames.Should().Be(GivenNames);
            response.DateOfBirth.Should().Be(DateOfBirth);
            response.DateOfDeath.Should().Be(DateOfDeath);
            response.Gender.Should().Be(Gender);
            response.GivenNames.Should().Be(GivenNames);
            response.HospitalNumber_1.Should().Be(HospitalNumber_1);
            response.HospitalNumber_2.Should().Be(HospitalNumber_2);
            response.HospitalNumber_3.Should().Be(HospitalNumber_3);
            response.MedicalExaminerOfficeResponsible.Should().Be(MedicalExaminerOfficeResponsible);
            response.NhsNumber.Should().Be(NhsNumber);
            response.OutOfHours.Should().Be(OutOfHours);
            response.PlaceDeathOccured.Should().Be(PlaceDeathOccured);
            response.Surname.Should().Be(Surname);
            response.TimeOfDeath.Should().Be(TimeOfDeath);
        }

        [Fact]
        public void Examination_To_GetExaminationResponse()
        {
            var examination = new Examination
            {
                ExaminationId = ExaminationId,
                AltLink = AltLink,
                AnyImplants = AnyImplants,
                AnyPersonalEffects = AnyPersonalEffects,
                ChildPriority = ChildPriority,
                Completed = Completed,
                CoronerPriority = CoronerPriority,
                CoronerStatus = CoronerStatus,
                County = County,
                Country = Country,
                CulturalPriority = CulturalPriority,
                DateOfBirth = DateOfBirth,
                DateOfDeath = DateOfDeath,
                FuneralDirectors = FuneralDirectors,
                FaithPriority = FaithPriority,
                GivenNames = GivenNames,
                Gender = Gender,
                GenderDetails = GenderDetails,
                HospitalNumber_1 = HospitalNumber_1,
                HospitalNumber_2 = HospitalNumber_2,
                HospitalNumber_3 = HospitalNumber_3,
                HouseNameNumber = HouseNameNumber,
                ImplantDetails = ImplantDetails,
                LastOccupation = LastOccupation,
                MedicalExaminerOfficeResponsible = MedicalExaminerOfficeResponsible,
                ModeOfDisposal = ModeOfDisposal,
                NhsNumber = NhsNumber,
                OrganisationCareBeforeDeathLocationId = OrganisationCareBeforeDeathLocationId,
                OtherPriority = OtherPriority,
                OutOfHours = OutOfHours,
                PersonalEffectDetails = PersonalEffectDetails,
                Postcode = Postcode,
                PlaceDeathOccured = PlaceDeathOccured,
                PriorityDetails = PriorityDetails,
                Representatives = Representatives,
                Surname = Surname,
                Street = Street,
                Town = Town,
                TimeOfDeath = TimeOfDeath
            };

            var result = mapper.Map<GetExaminationResponse>(examination);

            result.AnyPersonalEffects.Should().Be(AnyPersonalEffects);
            result.CoronerPriority.Should().Be(CoronerPriority);
            result.ChildPriority.Should().Be(ChildPriority);
            result.Completed.Should().Be(Completed);
            result.CoronerStatus.Should().Be(CoronerStatus);
            result.Country.Should().Be(Country);
            result.County.Should().Be(County);
            result.CulturalPriority.Should().Be(CulturalPriority);
            result.DateOfBirth.Should().Be(DateOfBirth);
            result.DateOfDeath.Should().Be(DateOfDeath);
            result.FaithPriority.Should().Be(FaithPriority);
            result.FuneralDirectors.Should().Be(FuneralDirectors);
            result.GivenNames.Should().Be(GivenNames);
            result.Gender.Should().Be(Gender);
            result.GenderDetails.Should().Be(GenderDetails);
            result.HospitalNumber_1.Should().Be(HospitalNumber_1);
            result.HospitalNumber_2.Should().Be(HospitalNumber_2);
            result.HospitalNumber_3.Should().Be(HospitalNumber_3);
            result.HouseNameNumber.Should().Be(HouseNameNumber);
            result.LastOccupation.Should().Be(LastOccupation);
            result.MedicalExaminerOfficeResponsible.Should().Be(MedicalExaminerOfficeResponsible);
            result.ModeOfDisposal.Should().Be(ModeOfDisposal);
            result.NhsNumber.Should().Be(NhsNumber);
            result.OutOfHours.Should().Be(OutOfHours);
            result.OrganisationCareBeforeDeathLocationId.Should().Be(OrganisationCareBeforeDeathLocationId);
            result.OtherPriority.Should().Be(OtherPriority);
            result.Postcode.Should().Be(Postcode);
            result.PersonalEffectDetails.Should().Be(PersonalEffectDetails);
            result.PriorityDetails.Should().Be(PriorityDetails);
            result.Street.Should().Be(Street);
            result.Surname.Should().Be(Surname);
            result.TimeOfDeath.Should().Be(TimeOfDeath);
            result.Town.Should().Be(Town);
        }

        [Fact]
        public void PostNewCaseRequest_To_Examination()
        {
            var postNewCaseRequest = new PostNewCaseRequest
            {
                DateOfDeath = DateOfDeath,
                DateOfBirth = DateOfBirth,
                GivenNames = GivenNames,
                Gender = Gender,
                GenderDetails = GenderDetails,
                HospitalNumber_1 = HospitalNumber_1,
                HospitalNumber_2 = HospitalNumber_2,
                HospitalNumber_3 = HospitalNumber_3,
                MedicalExaminerOfficeResponsible = MedicalExaminerOfficeResponsible,
                NhsNumber = NhsNumber,
                OutOfHours = OutOfHours,
                PlaceDeathOccured = PlaceDeathOccured,
                Surname = Surname,
                TimeOfDeath = TimeOfDeath
            };

            var result = mapper.Map<Examination>(postNewCaseRequest);

            result.DateOfBirth.Should().Be(DateOfBirth);
            result.DateOfDeath.Should().Be(DateOfDeath);
            result.GivenNames.Should().Be(GivenNames);
            result.Gender.Should().Be(Gender);
            result.GenderDetails.Should().Be(GenderDetails);
            result.HospitalNumber_1.Should().Be(HospitalNumber_1);
            result.HospitalNumber_2.Should().Be(HospitalNumber_2);
            result.HospitalNumber_3.Should().Be(HospitalNumber_3);
            result.MedicalExaminerOfficeResponsible.Should().Be(MedicalExaminerOfficeResponsible);
            result.NhsNumber.Should().Be(NhsNumber);
            result.OutOfHours.Should().Be(OutOfHours);
            result.PlaceDeathOccured.Should().Be(PlaceDeathOccured);
            result.Surname.Should().Be(Surname);
            result.TimeOfDeath.Should().Be(TimeOfDeath);
        }

        /// <summary>
        ///     Test Mapping Examination to GetExaminationResponse.
        /// </summary>
        [Fact]
        public void TestGetExaminationResponse()
        {
            var expectedExaminationId = "expectedExaminationId";

            var examination = new Examination
            {
                ExaminationId = expectedExaminationId
            };

            var response = mapper.Map<GetExaminationResponse>(examination);

            response.ExaminationId.Should().Be(expectedExaminationId);
        }
    }
}