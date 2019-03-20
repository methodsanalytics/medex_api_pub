﻿using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using MedicalExaminer.API.Models.v1.Users;
using MedicalExaminer.Common;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

namespace MedicalExaminer.API.Attributes
{
    /// <summary>
    ///     Base class for validators of UserItem objects
    /// </summary>
    public abstract class ValidUserBase : RequiredAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            throw new NotImplementedException();
        }

        protected ValidationResult IsValid(
            object value,
            ValidationContext context,
            UserRoles userRole,
            string userTypeName)
        {
            var userPersistence = (IUserPersistence)context.GetService(typeof(IUserPersistence));
            var mapper = (IMapper)context.GetService(typeof(IMapper));

            if (!(value is UserItem userItem))
            {
                return new ValidationResult($"Item not recognised as of type useritem for {userTypeName}");
            }

            var meUser = mapper.Map<MeUser>(userItem);

            if (meUser == null || string.IsNullOrEmpty(meUser.UserId))
            {
                return new ValidationResult($"Cannot get id for {userTypeName}");
            }

            try
            {
                if (userPersistence == null)
                {
                    throw new NullReferenceException("User Persistence is null");
                }

                var returnedDocument = userPersistence.GetUserAsync(meUser.UserId).Result;
                if (returnedDocument.UserRole != userRole)
                {
                    return new ValidationResult($"The user is not a {userTypeName}");
                }
            }
            catch (ArgumentException e)
            {
                return new ValidationResult($"The {userTypeName} has not been found");
            }

            return ValidationResult.Success;
        }
    }
}