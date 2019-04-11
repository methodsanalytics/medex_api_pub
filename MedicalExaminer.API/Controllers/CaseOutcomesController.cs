using System;
using System.Threading.Tasks;
using AutoMapper;
using MedicalExaminer.API.Filters;
using MedicalExaminer.API.Models.v1.CaseOutcomes;
using MedicalExaminer.Common.Loggers;
using MedicalExaminer.Common.Queries.CaseOutcomes;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;
using Microsoft.AspNetCore.Mvc;

namespace MedicalExaminer.API.Controllers
{
    public class CaseOutcomesController : AuthenticatedBaseController
    {
        private IAsyncQueryHandler<CaseOutcomesRetrievalQuery, Examination> _caseOutcomeRetrievalService;
        private IAsyncQueryHandler<ExaminationRetrievalQuery, Examination> _examinationRetrievalService;

        public CaseOutcomesController(
            IMELogger logger,
            IMapper mapper,
            IAsyncQueryHandler<CaseOutcomesRetrievalQuery, Examination> caseOutcomeRetrievalService,
            IAsyncQueryHandler<ExaminationRetrievalQuery, Examination> examinationRetrievalService,
            IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser> usersRetrievalByEmailService)
            : base(logger, mapper, usersRetrievalByEmailService)
        {
            _caseOutcomeRetrievalService = caseOutcomeRetrievalService;
            _examinationRetrievalService = examinationRetrievalService;
        }

        /// <summary>
        /// Get Case Outcomes.
        /// </summary>
        /// <param name="examinationId">Examination Id</param>
        /// <returns>Case Outcomes Response</returns>
        [HttpGet]
        [Route("{examinationId}/caseOutcomes")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<GetCaseOutcomesResponse>> GetCaseOutcomes(string examinationId)
        {
            if (string.IsNullOrEmpty(examinationId))
            {
                return BadRequest(new GetCaseOutcomesResponse());
            }

            if (!Guid.TryParse(examinationId, out _))
            {
                return BadRequest(new GetCaseOutcomesResponse());
            }

            var currentUser = await CurrentUser();

            var examination = await _examinationRetrievalService.Handle(new ExaminationRetrievalQuery(examinationId, currentUser));

            if (examination == null)
            {
                return new NotFoundObjectResult(new GetCaseOutcomesResponse());
            }

            var result = await _caseOutcomeRetrievalService.Handle(new CaseOutcomesRetrievalQuery(examinationId));

            return Ok(Mapper.Map<GetCaseOutcomesResponse>(result));
        }
    }
}