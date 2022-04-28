using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagementBE.Repositories.Requests;

public class PagingResult<T> : PagingRequest
{
    public List<T>? Items { get; set; }
}

