using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace OneInc.Services
{
    public interface IRecognizeService
    {
        int Recognize(Stream stream);
    }
}