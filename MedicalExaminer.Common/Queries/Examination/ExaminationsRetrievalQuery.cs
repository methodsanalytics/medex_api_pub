using System.Collections.Generic;
using MedicalExaminer.Models;
using MedicalExaminer.Models.Enums;

namespace MedicalExaminer.Common.Queries.Examination
{
    /// <summary>
    /// Examinations Retrieval Query.
    /// </summary>
    public class ExaminationsRetrievalQuery : IQuery<IEnumerable<Models.Examination>>, IQuery<ExaminationsOverview>
    {
        /// <summary>
        /// Initialise a new instance of <see cref="ExaminationsRetrievalQuery"/>.
        /// </summary>
        /// <param name="permissedLocations">Permissed locations.</param>
        /// <param name="filterCaseStatus">case status filter</param>
        /// <param name="filterLocationId">location id filter</param>
        /// <param name="filterOrderBy">order by</param>
        /// <param name="filterPageNumber">page number</param>
        /// <param name="filterPageSize">page size</param>
        /// <param name="filterUserId">user id filter</param>
        /// <param name="filterOpenCases">open cases filter</param>
        public ExaminationsRetrievalQuery(
            IEnumerable<string> permissedLocations,
            CaseStatus? filterCaseStatus,
            string filterLocationId,
            ExaminationsOrderBy? filterOrderBy,
            int filterPageNumber,
            int filterPageSize,
            string filterUserId,
            bool filterOpenCases)
        {
            PermissedLocations = permissedLocations;
            FilterCaseStatus = filterCaseStatus;
            FilterLocationId = filterLocationId;
            FilterOrderBy = filterOrderBy;
            FilterPageNumber = filterPageNumber;
            FilterPageSize = filterPageSize;
            FilterUserId = filterUserId;
            FilterOpenCases = filterOpenCases;
        }

        /// <summary>
        /// Permissed Locations.
        /// </summary>
        public IEnumerable<string> PermissedLocations { get; }

        /// <summary>
        /// Filter Case Status.
        /// </summary>
        public CaseStatus? FilterCaseStatus { get; }

        /// <summary>
        /// Filter Location Id.
        /// </summary>
        public string FilterLocationId { get; }

        /// <summary>
        /// Filter Order By.
        /// </summary>
        public ExaminationsOrderBy? FilterOrderBy { get; }

        /// <summary>
        /// Filter Page Number.
        /// </summary>
        public int FilterPageNumber { get; }

        /// <summary>
        /// Filter Page Size.
        /// </summary>
        public int FilterPageSize { get; }

        /// <summary>
        /// Filter User Id.
        /// </summary>
        public string FilterUserId { get; }

        /// <summary>
        /// Filter Open Cases.
        /// </summary>
        public bool FilterOpenCases { get; }
    }
}