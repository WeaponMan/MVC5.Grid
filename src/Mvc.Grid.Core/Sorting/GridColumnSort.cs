﻿using System;
using System.Linq;
using System.Linq.Expressions;

namespace NonFactors.Mvc.Grid
{
    public class GridColumnSort<T, TValue> : IGridColumnSort<T, TValue>
    {
        public Boolean? IsEnabled { get; set; }

        public GridSortOrder? Order
        {
            get
            {
                if (OrderIsSet)
                    return OrderValue;

                String prefix = String.IsNullOrEmpty(Column.Grid.Name) ? "" : Column.Grid.Name + "-";
                if (String.Equals(Column.Grid.Query[prefix + "sort"], Column.Name, StringComparison.OrdinalIgnoreCase))
                {
                    String order = Column.Grid.Query[prefix + "order"];

                    if ("asc".Equals(order, StringComparison.OrdinalIgnoreCase))
                        OrderValue = GridSortOrder.Asc;
                    if ("desc".Equals(order, StringComparison.OrdinalIgnoreCase))
                        OrderValue = GridSortOrder.Desc;
                }
                else if (Column.Grid.Query[prefix + "Sort"] == null)
                {
                    OrderValue = InitialOrder;
                }

                OrderIsSet = true;

                return OrderValue;
            }
            set
            {
                OrderValue = value;
                OrderIsSet = true;
            }
        }
        private Boolean OrderIsSet { get; set; }
        private GridSortOrder? OrderValue { get; set; }

        public GridSortOrder? FirstOrder { get; set; }
        public GridSortOrder? InitialOrder { get; set; }

        public IGridColumn<T, TValue> Column { get; set; }

        public GridColumnSort(IGridColumn<T, TValue> column)
        {
            Column = column;
            IsEnabled = column.Expression.Body is MemberExpression ? (Boolean?)null : false;
        }

        public IQueryable<T> Apply(IQueryable<T> items)
        {
            if (IsEnabled != true || Order == null)
                return items;

            if (Order == GridSortOrder.Asc)
                return items.OrderBy(Column.Expression);

            return items.OrderByDescending(Column.Expression);
        }
    }
}
