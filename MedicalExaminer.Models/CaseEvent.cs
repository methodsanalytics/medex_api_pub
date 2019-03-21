﻿using System;

namespace MedicalExaminer.Models
{
    public class CaseEvent
    {
        public string EventId { get; set; }

        public string EventText { get; set; }

        public MeUser User { get; set; }

        public DateTime DateTimeEntered { get; set; }
    }
}