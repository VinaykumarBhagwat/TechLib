using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechLibrary.Contracts.Requests
{
    /// <summary>
    /// Search book request model
    /// </summary>
    public class SearchBooksRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
