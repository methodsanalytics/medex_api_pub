using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MedicalExaminer.Models.Enums
{
    /// <summary>
    /// Overall Circumstances of Death.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum OverallCircumstancesOfDeath
    {
        /// <summary>
        /// Expected.
        /// </summary>
        Expected = 1,

        /// <summary>
        /// Unexpected.
        /// </summary>
        Unexpected = 2,

        /// <summary>
        /// Sudden by not unexpected.
        /// </summary>
        SuddenButNotUnexpected = 3,

        /// <summary>
        /// Part of an individualised end of life care plan.
        /// </summary>
        PartOfAnIndividualisedEndOfLifeCarePlan = 4
    }
}
