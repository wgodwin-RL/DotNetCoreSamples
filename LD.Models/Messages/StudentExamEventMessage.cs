using System;
using System.Collections.Generic;
using System.Text;

namespace LD_Models.Messages
{
    public class StudentExamEventMessage
    {
        public string Event { get; set; }

        public StudentExamData Data { get; set; }
    }
}
