using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Verba.Abstractions.Application.Settings;

public class JwtAuthSettings
{
    public string JwtIssuer { get; set; }

    public string JwtKey { get; set; }

    public int JwtExpirationMinutes { get; set; }
}
