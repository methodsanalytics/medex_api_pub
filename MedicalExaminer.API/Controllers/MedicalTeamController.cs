﻿using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MedicalExaminer.API.Filters;
using MedicalExaminer.API.Models.v1.MedicalTeams;
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
    ///     Medical Team Controller.
    /// </summary>
    [ApiVersion("1.0")]
    [Route("/v{api-version:apiVersion}/examinations/{examinationId}")]
    [ApiController]
    [Authorize]
    public class MedicalTeamController : BaseController
    {
        private readonly IAsyncQueryHandler<CreateExaminationQuery, Examination> _examinationCreationService;
        private readonly IAsyncQueryHandler<ExaminationRetrievalQuery, Examination> _examinationRetrievalService;
        private readonly IAsyncQueryHandler<ExaminationsRetrievalQuery, IEnumerable<Examination>> _examinationsRetrievalService;
        private readonly IAsyncUpdateDocumentHandler _medicalTeamUpdateService;

        /// <summary>
        /// Initialise a new instance of the <see cref="MedicalTeamController"/>.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="mapper">Mapper.</param>
        /// <param name="examinationCreationService">Examination Creation Service.</param>
        /// <param name="examinationRetrievalService">Examination Retrieval Service.</param>
        /// <param name="examinationsRetrievalService">Examinations Retrieval Service.</param>
        /// <param name="medicalTeamUpdateService">Medical Team Update Service.</param>
        public MedicalTeamController(
            IMELogger logger,
            IMapper mapper,
            IAsyncQueryHandler<CreateExaminationQuery, Examination> examinationCreationService,
            IAsyncQueryHandler<ExaminationRetrievalQuery, Examination> examinationRetrievalService,
            IAsyncQueryHandler<ExaminationsRetrievalQuery, IEnumerable<Examination>> examinationsRetrievalService,
            IAsyncUpdateDocumentHandler medicalTeamUpdateService)
            : base(logger, mapper)
        {
            _examinationCreationService = examinationCreationService;
            _examinationRetrievalService = examinationRetrievalService;
            _examinationsRetrievalService = examinationsRetrievalService;
            _medicalTeamUpdateService = medicalTeamUpdateService;
        }

        /// <summary>
        ///     Get Medical Team.
        /// </summary>
        /// ///
        /// <param name="examinationId">The ID of the examination.</param>
        /// <returns>A GetMedicalTeamResponse.</returns>
        [HttpGet("medical_team/")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<GetMedicalTeamResponse>> GetMedicalTeam(string examinationId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new GetMedicalTeamResponse());
            }

            var examination = await _examinationRetrievalService.Handle(new ExaminationRetrievalQuery(examinationId));
            if (examination?.MedicalTeam == null)
            {
                return NotFound(new GetMedicalTeamResponse());
            }

            var getMedicalTeamResponse = Mapper.Map<GetMedicalTeamResponse>(examination.MedicalTeam);

            return Ok(getMedicalTeamResponse);
        }

        /// <summary>
        ///     Post Medical Team.
        /// </summary>
        /// ///
        /// <param name="examinationId">The ID of the examination that the medical team object is to be posted to.</param>
        /// <param name="postMedicalTeamRequest">The PostMedicalTeamRequest.</param>
        /// <returns>A PutExaminationResponse.</returns>
        [HttpPost("medical_team/")]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public async Task<ActionResult<PutMedicalTeamResponse>> PostMedicalTeam(string examinationId, [FromBody] PostMedicalTeamRequest postMedicalTeamRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new PutMedicalTeamResponse());
            }

            var medicalTeamRequest = Mapper.Map<MedicalTeam>(postMedicalTeamRequest);

            if (medicalTeamRequest == null)
            {
                return BadRequest(new PutMedicalTeamResponse());
            }

            var examination = await _examinationRetrievalService.Handle(new ExaminationRetrievalQuery(examinationId));
            if (examination == null)
            {
                return NotFound();
            }

            examination.MedicalTeam = medicalTeamRequest;

            var returnedExaminationId = await _medicalTeamUpdateService.Handle(examination);

            if (returnedExaminationId == null)
            {
                return BadRequest(new PutMedicalTeamResponse());
            }

            var res = new PutMedicalTeamResponse()
            {
                ExaminationId = examinationId
            };

            return Ok(res);
        }
    }
}