﻿namespace MedicalExaminer.API.Models.V1.Users
{
    /// <inheritdoc />
    /// <summary>
    ///     Response for Put User.
    /// </summary>
    public class PutUserResponse : ResponseBase
    {
        /// <summary>
        ///     The User identifier.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        ///     The User's email address.
        /// </summary>
        public string Email { get; set; }
    }
}