using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechLibrary.Contracts.Requests
{
    public class GetAllBookRequest
    {
        public int PageNumber { get; set; } = 1;

        // This can be configured / read from configuration
        public int PageSize { get; } = 10;
    }
}
