using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OneInc.Models
{
    public class Result
    {
        public int Id { get; set; }

        //[MaxLength(16), Column(TypeName = "Binary")]
        public byte[] Voice { get; set; }

        public int OptionId { get; set; }
        public Option Option { get; set; }

        [NotMapped]
        public string VoiceBase64 { get
            {
                if (Voice == null)
                    return null;
                return Convert.ToBase64String(Voice);
            }
        }
    }
}