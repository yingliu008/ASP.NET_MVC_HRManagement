using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Collections.Specialized;
using MyProject.Helper;

namespace MyProject.Helpers
{
    public class JqGridHelper
    {
        public JqGridResult GenerateResults<T>(IEnumerable<T> data, string idProperty, string properties, int currentPage, int pageSize)
        {
            List<string> props = properties.Split(',').ToList(); 
            for (int i = 0; i < props.Count; i++)
            {
                props[i] = props[i].Trim();
            }
            return GenerateResults<T>(data, idProperty, props, currentPage, pageSize);
        }


        public JqGridResult GenerateResults<T>(IEnumerable<T> data, string idProperty, List<string> properties, int currentPage, int pageSize)
        {
            List<PropertyInfo> props = properties.Select(property => typeof(T).GetProperty(property)).ToList();

            var idProp = typeof(T).GetProperty(idProperty);

            var results = new List<JqGridRowResult>();
            foreach (T datum in data)
            {
                var result = new JqGridRowResult { id = idProp.GetValue(datum).ToString() };
                foreach (var prop in props)
                {
                    var val = prop.GetValue(datum);
                    result.cell.Add(val == null ? "" : val.ToString());
                }
                results.Add(result);
            }

            var totalGridResult = new JqGridResult
            {
                Page = currentPage.ToString(),
                Records = results.Count().ToString(),
                Total = pageSize.ToString(),
                Rows = results
            };

            return totalGridResult;
        }

    }

    public class JqGridRowResult
    {
        private List<string> _cellData = null;

        public string id { get; set; }

        public List<string> cell
        {
            get
            {
                if (_cellData == null)
                    _cellData = new List<string>();
                return _cellData;
            }
        }
    }

    public class JqGridResult
    {
        public string Total { get; set; }
        public string Page { get; set; }
        public string Records { get; set; }
        public List<JqGridRowResult> Rows { get; set; }
    }

    public class JqGridParameters
    {
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public string SortOrder { get; set; }
        public string SortExpression { get; set; }

        public JqGridParameters()
        {
            PageSize = 200;
            CurrentPage = 1;
        }

        public JqGridParameters(NameValueCollection collection)
        {
            PageSize = Convert.ToInt32(collection["rows"]);
            CurrentPage = Convert.ToInt32(collection["page"]);
            SortOrder = collection["sord"].ToString(CultureInfo.InvariantCulture);
            var sortExpression = collection["sidx"].ToString(CultureInfo.InvariantCulture);

            switch (sortExpression)
            {
                case "id":
                    sortExpression = "Id {0}";
                    break;
                case "FullName":
                    sortExpression = "LastName {0}, FirstName {0}";
                    break;
                default:
                    sortExpression = sortExpression + " {0}";
                    break;
            }

            SortExpression = String.Format(sortExpression, SortOrder);
        }


        public JqGridParameters(string sortOrder, string sortExpression)
        {
            SortOrder = sortOrder;

            switch (sortExpression)
            {
                case "id":
                    sortExpression = "Id {0}";
                    break;
                case "FullName":
                    sortExpression = "LastName {0}, FirstName {0}";
                    break;
                default:
                    sortExpression = sortExpression + " {0}";
                    break;
            }

            SortExpression = String.Format(sortExpression, SortOrder);

        }

        public QueryOptions GenerateQueryOptions()
        {
            return new QueryOptions(SortExpression, PageSize, CurrentPage);
        }
    }
}
