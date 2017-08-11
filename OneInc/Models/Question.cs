using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneInc.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<Option> Options { get; set; }
    }
}