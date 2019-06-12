using System;
using System.Collections.Generic;
using System.Linq;
using MedicalExaminer.Models;

namespace MedicalExaminer.API.Extensions.Data
{
    /// <summary>
    /// class that finds the next available appointment of an examination
    /// </summary>
    public class AppointmentFinder
    {
        /// <summary>
        /// finds the next available appointment in the patients details
        /// </summary>
        /// <param name="representatives">Representatives.</param>
        /// <returns>Representative.</returns>
        public Representative FindAppointment(IEnumerable<Representative> representatives)
        {
            if (representatives == null)
            {
                return null;
            }

            return representatives
                .OrderByDescending(x => x.AppointmentDate)
                .FirstOrDefault(repAppointment => repAppointment.AppointmentDate >= DateTime.Now);
        }
    }
}
