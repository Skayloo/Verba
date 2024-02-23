using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Verba.Abstractions.Application.Settings
{
    public class MinioSettings
    {
        public string MinioEndpoint { get; set; }

        public string MinioAccessKey { get; set; }

        public string MinioSecretKey { get; set; }
    }
}
