using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Helper
{
    public class QueryOptions
    {
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public string SortBy { get; set; }

        public QueryOptions()
            : this(string.Empty, 200, 1)
        {
        }

        public QueryOptions(string sortBy)
            : this(sortBy, 200, 1)
        {
        }

        public QueryOptions(string sortBy, int pageSize)
            : this(sortBy, pageSize, 1)
        {
        }

        public QueryOptions(string sortBy, int pageSize, int currentPage)
        {
            PageSize = pageSize;
            CurrentPage = currentPage;
            SortBy = (string.IsNullOrEmpty(sortBy)) ? "LastName, FirstName" : sortBy; ;
        }

        public int CalculateSkip()
        {
            return PageSize * (CurrentPage - 1);
        }
    }
}