namespace MedicalExaminer.Common.Authorization
{
    /// <summary>
    /// Permission.
    /// </summary>
    public enum Permission
    {
        /// <summary>
        /// Get Users.
        /// </summary>
        GetUsers,

        /// <summary>
        /// Get User.
        /// </summary>
        GetUser,

        /// <summary>
        /// Invite User.
        /// </summary>
        InviteUser,

        /// <summary>
        /// Suspend User.
        /// </summary>
        SuspendUser,

        /// <summary>
        /// Enable User.
        /// </summary>
        EnableUser,

        /// <summary>
        /// Delete User.
        /// </summary>
        DeleteUser,

        /// <summary>
        /// Update User.
        /// </summary>
        UpdateUser,

        /// <summary>
        /// Get User Permissions.
        /// </summary>
        GetUserPermissions,

        /// <summary>
        /// Get User Permission.
        /// </summary>
        GetUserPermission,

        /// <summary>
        /// Create User Permission.
        /// </summary>
        CreateUserPermission,

        /// <summary>
        /// Update User Permission.
        /// </summary>
        UpdateUserPermission,

        /// <summary>
        /// Delete User Permission.
        /// </summary>
        DeleteUserPermission,

        /// <summary>
        /// Get Locations.
        /// </summary>
        GetLocations,

        /// <summary>
        /// Get Location.
        /// </summary>
        GetLocation,

        /// <summary>
        /// Get Examinations.
        /// </summary>
        GetExaminations,

        /// <summary>
        /// Get Examination.
        /// </summary>
        GetExamination,

        /// <summary>
        /// Create Examination.
        /// </summary>
        CreateExamination,

        /// <summary>
        /// Assign Examination to Medical Examiner.
        /// </summary>
        AssignExaminationToMedicalExaminer,

        /// <summary>
        /// Update Examination.
        /// </summary>
        UpdateExamination,

        /// <summary>
        /// Update Examination State.
        /// </summary>
        UpdateExaminationState,

        /// <summary>
        /// Add Event to Examination.
        /// </summary>
        AddEventToExamination,

        /// <summary>
        /// Get Examination Events.
        /// </summary>
        GetExaminationEvents,

        /// <summary>
        /// Get examination Event.
        /// </summary>
        GetExaminationEvent,

        /// <summary>
        /// Get Profile.
        /// </summary>
        GetProfile,

        /// <summary>
        /// Update Profile.
        /// </summary>
        UpdateProfile,

        /// <summary>
        /// Get Profile Permissions.
        /// </summary>
        GetProfilePermissions,

        /// <summary>
        /// Add Bereaved Discussion Event.
        /// </summary>
        BereavedDiscussionEvent,

        /// <summary>
        /// Add MEO Summary Event
        /// </summary>
        MeoSummaryEvent,

        /// <summary>
        /// Add QAP Discussion Event.
        /// </summary>
        QapDiscussionEvent,

        /// <summary>
        /// Add Other Event.
        /// </summary>
        OtherEvent,

        /// <summary>
        /// Add Admission Event.
        /// </summary>
        AdmissionEvent,

        /// <summary>
        /// Add Medical History Event.
        /// </summary>
        MedicalHistoryEvent,

        /// <summary>
        /// Add Pre Scrutiny Event.
        /// </summary>
        PreScrutinyEvent
    }
}
