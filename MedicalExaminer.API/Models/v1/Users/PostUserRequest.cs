﻿using System.ComponentModel.DataAnnotations;

namespace MedicalExaminer.API.Models.v1.Users
{
    /// <summary>
    ///     Post User Request.
    /// </summary>
    public class PostUserRequest
    {
        /// <summary>
        ///     The User's email address.
        /// </summary>
        [EmailAddress]
        public string Email { get; set; }
    }
}