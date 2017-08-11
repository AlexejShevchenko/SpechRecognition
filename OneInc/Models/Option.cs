using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneInc.Models
{
    public class Option
    {
        public int Id { get; set; }
        public string Value { get; set; }

        public int QuestionId {get;set;}
        public Question Question { get; set; }

        public virtual List<Result> Results { get; set; }

        public Option()
        {
            Results = new List<Result>();
        }
    }
}