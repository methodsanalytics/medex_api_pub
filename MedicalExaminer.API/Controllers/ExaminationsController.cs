﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MedicalExaminer.API.Filters;
using MedicalExaminer.API.Models.v1.Examinations;
using MedicalExaminer.Common.Loggers;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicalExaminer.API.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// Examinations Controller
    /// </summary>
    [Route("examinations")]
    [ApiController]
    [Authorize]
    public class ExaminationsController : BaseController
    {
        private readonly IAsyncQueryHandler<CreateExaminationQuery, Examination> _examinationCreationService;
        private readonly IAsyncQueryHandler<ExaminationRetrievalQuery, Examination> _examinationRetrievalService;
        private readonly IAsyncQueryHandler<ExaminationsRetrievalQuery, IEnumerable<Examination>> _examinationsRetrievalService;
        /// <summary>
        /// Initialise a new instance of the Examiantions Controller.
        /// </summary>
        /// <param name="examinationPersistence">The Examination Persistance.</param>
        /// <param name="logger">The Logger.</param>
        /// <param name="mapper">The Mapper.</param>
        /// <param name="examinationCreationService"></param>
        /// <param name="examinationRetrievalService"></param>
        /// <param name="examinationsRetrievalService"></param>
        public ExaminationsController(
            IMELogger logger,
            IMapper mapper,
            IAsyncQueryHandler<CreateExaminationQuery, Examination> examinationCreationService,
            IAsyncQueryHandler<ExaminationRetrievalQuery, Examination> examinationRetrievalService,
            IAsyncQueryHandler<ExaminationsRetrievalQuery, IEnumerable<Examination>> examinationsRetrievalService)
            : base(logger, mapper)
        {
            _examinationCreationService = examinationCreationService;
            _examinationRetrievalService = examinationRetrievalService;
            _examinationsRetrievalService = examinationsRetrievalService;
        }

    /// <summary>
    /// Get All Examinations as a list of <see cref="ExaminationItem"/>.
    /// </summary>
    /// <returns>A list of examinations.</returns>
    [HttpGet]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<GetExaminationsResponse>> GetExaminations([FromBody]GetExaminationsRequest filter)
        {
            var examinations = await _examinationsRetrievalService.Handle(
                new ExaminationsRetrievalQuery(filter.CaseStatus, filter.LocationId, filter.OrderBy, filter.PageNumber, filter.PageSize, filter.UserId));

            return Ok(new GetExaminationsResponse
            {
                Examinations = examinations.Select(e => Mapper.Map<PatientCardItem>(e)).ToList()
            });
        }

        /// <summary>
        /// Get Examination by ID
        /// </summary>
        /// <param name="examinationId"></param>
        /// <returns>A GetExaminationResponse.</returns>
        [HttpGet("{examinationId}")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<GetExaminationResponse>> GetExamination(string examinationId)
        {
                Examination result = await _examinationRetrievalService.Handle(new ExaminationRetrievalQuery(examinationId));
                if (result == null)
                {
                    return NotFound();
                }

                return Ok(Mapper.Map<GetExaminationResponse>(result));
        }

        /// <summary>
        /// Create a new case.
        /// </summary>
        /// <param name="postNewCaseRequest">The PostNewCaseRequest.</param>
        /// <returns>A PostNewCaseResponse.</returns>
        // POST api/examinations
        [HttpPost]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<PutExaminationResponse>> CreateNewCase([FromBody]PostNewCaseRequest postNewCaseRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new PutExaminationResponse());
            }

            var examination = Mapper.Map<Examination>(postNewCaseRequest);
            
            var result =await _examinationCreationService.Handle(new CreateExaminationQuery(examination));
            var res = new PutExaminationResponse()
            {
                ExaminationId = result.Id
            };

            return Ok(res);
        }
    }
}