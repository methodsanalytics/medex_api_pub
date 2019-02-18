﻿using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Medical_Examiner_API.Models
{
    public enum CoronerStatus 
    {
        None = 0,
        PendingSend = 1,
        SentAwaitingConfirm = 2,
        SentConfirmed = 3
    }
}
