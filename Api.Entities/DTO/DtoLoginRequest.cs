using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Entities.DTO
{
    public class DtoLoginRequest
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
    }
}
