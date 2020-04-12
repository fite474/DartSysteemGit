using Dartssystem.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DartssystemServer.Json
{
    class JsonResponse : IJsonData
    {
        public string Error { get; set; }
        public string Message { get; set; }
    }
}
