using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace NonFactors.Mvc.Grid
{
    public interface IGridFilters
    {
        IGridFilter GetFilter(Type type, String method);
        IEnumerable<SelectListItem> GetFilterOptions<T, TValue>(IGridColumn<T, TValue> column);

        void Register(Type type, String method, Type filter);
        void Unregister(Type type, String method);
    }
}
