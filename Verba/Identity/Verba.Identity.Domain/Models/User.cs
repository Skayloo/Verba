using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Verba.Identity.Domain.Models
{
    public class User : IdentityUser
    {
        public DateTime CreatedDatetime { get; set; }

        public string Inn { get; set; }

        public string OrgName { get; set; }
    }
}
