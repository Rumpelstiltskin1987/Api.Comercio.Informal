using Api.Entities;
using Api.Entities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Interfaces
{
    public interface IRol
    {
        public Task<List<string>> GetAll();
    }
}
