using System;
using System.Web;

namespace NonFactors.Mvc.Grid
{
    public interface IHtmlGrid<T> : IHtmlString
    {
        IGrid<T> Grid { get; }

        String PartialViewName { get; set; }
    }
}
