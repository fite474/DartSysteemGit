using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dartssystem.Json
{
    class JsonHelper
    {
        public static T ToConcreteType<T>(dynamic type)
        {
            return (type as JObject).ToObject<T>();
        }
    }
}