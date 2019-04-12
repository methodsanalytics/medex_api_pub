using MedicalExaminer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalExaminer.API.Models.v1.CaseBreakdown
{
    public interface IEventItem : IEvent
    {
        string UsersFullName { get; set; }

        string UsersRole { get; set; }
    }
}
