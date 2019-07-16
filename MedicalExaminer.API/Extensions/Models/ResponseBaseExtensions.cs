﻿using System;
using MedicalExaminer.API.Models.v1;
using MedicalExaminer.Models.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MedicalExaminer.API.Extensions.Models
{
    /// <summary>
    ///     Response Base Extensions
    /// </summary>
    public static class ResponseBaseExtensions
    {
        /// <summary>
        ///     Add the model state errors to the response.
        /// </summary>
        /// <param name="responseBase">The response base.</param>
        /// <param name="modelState">The model state.</param>
        public static void AddModelErrors(this ResponseBase responseBase, ModelStateDictionary modelState)
        {
            foreach (var item in modelState)
            {
                foreach (var error in item.Value.Errors)
                {
                    responseBase.AddError(item.Key, error.ErrorMessage);
                }
            }
        }

        public static void AddModelErrors(this ResponseBaseEnumErrors responseBase, ModelStateDictionary modelState)
        {
            foreach (var item in modelState)
            {
                foreach (var error in item.Value.Errors)
                {
                    responseBase.AddError(item.Key, ParseEnum<SystemValidationErrors>(error.ErrorMessage));
                }
            }
        }

        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}