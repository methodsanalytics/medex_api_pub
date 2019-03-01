﻿using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.Azure.Documents;
using DataType = System.ComponentModel.DataAnnotations.DataType;

namespace MedicalExaminer.Models
{
    public abstract class Record : Document
    {
        [Required]
        [Display(Name = "modified_by")]
        [DataType(DataType.Text)]
        public string LastModifiedBy { get; set; }

        [Required]
        [Display(Name = "modified_at")]
        [DataType(DataType.DateTime)]
        public DateTimeOffset ModifiedAt { get; set; }

        [Required]
        [Display(Name = "created_at")]
        [DataType(DataType.DateTime)]
        public DateTimeOffset CreatedAt { get; set; }

        [Display(Name = "deleted_at")]
        [DataType(DataType.DateTime)]
        public DateTimeOffset? DeletedAt { get; set; }
    }
}