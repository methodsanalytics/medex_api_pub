﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MedicalExaminer.Models.Enums;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

namespace MedicalExaminer.API.Attributes
{
    /// <summary>
    /// Validates that a user exists and is in the role of medical examiner officer
    /// </summary>
    public class ValidMedicalExaminerOfficer : ValidUserBase
    {
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            return IsValid(value, context, UserRoles.MedicalExaminerOfficer, "medical examiner officer");
        }
    }
}