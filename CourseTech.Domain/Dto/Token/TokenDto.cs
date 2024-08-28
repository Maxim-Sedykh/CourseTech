using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Dto.Token
{
    public class TokenDto
    {
        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }
    }
}
