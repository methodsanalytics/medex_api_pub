using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalExaminer.Common.Settings
{
    public class ExaminationVersionSettings
    {
        public ExaminationVersionSettings(int version)
        {
            Version = version;
        }
        public readonly int Version;
    }
}
