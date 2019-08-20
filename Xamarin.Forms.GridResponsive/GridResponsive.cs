using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Xamarin.Forms.GridResponsive
{
    public class GridResponsive : Grid
    {
        public static readonly BindableProperty ResponsiveColumnProperty = BindableProperty.Create("ResponsiveColumn", typeof(GridLayout), typeof(GridResponsive));

        public static readonly BindableProperty ResponsiveRowProperty = BindableProperty.Create("ResponsiveRow", typeof(int), typeof(GridResponsive));

        public static readonly BindableProperty ColumnsProperty = BindableProperty.Create(nameof(Columns), typeof(int), typeof(GridResponsive));

        public static readonly BindableProperty MaxWidthExtraSmallProperty = BindableProperty.Create(nameof(MaxWidthExtraSmall), typeof(double), typeof(GridResponsive));

        public static readonly BindableProperty MaxWidthSmallProperty = BindableProperty.Create(nameof(MaxWidthSmall), typeof(double), typeof(GridResponsive));

        public static readonly BindableProperty MaxWidthMediumProperty = BindableProperty.Create(nameof(MaxWidthMedium), typeof(double), typeof(GridResponsive));

        public int Columns
        {
            get => (int)GetValue(ColumnsProperty);
            set => SetValue(ColumnsProperty, value);
        }

        public double MaxWidthExtraSmall
        {
            get => (int)GetValue(MaxWidthExtraSmallProperty);
            set => SetValue(MaxWidthExtraSmallProperty, value);
        }

        public double MaxWidthSmall
        {
            get => (int)GetValue(MaxWidthSmallProperty);
            set => SetValue(MaxWidthSmallProperty, value);
        }

        public double MaxWidthMedium
        {
            get => (int)GetValue(MaxWidthMediumProperty);
            set => SetValue(MaxWidthMediumProperty, value);
        }

        public GridResponsive()
        {
            Columns = GridResponsiveGlobal.Columns;
            ColumnSpacing = GridResponsiveGlobal.ColumnSpacing;
            RowSpacing = GridResponsiveGlobal.RowSpacing;

            MaxWidthExtraSmall = GridResponsiveGlobal.MaxWidthExtraSmall;
            MaxWidthSmall = GridResponsiveGlobal.MaxWidthSmall;
            MaxWidthMedium = GridResponsiveGlobal.MaxWidthMedium;
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == ColumnsProperty.PropertyName)
            {
                ColumnDefinitions.Clear();

                for (var i = 0; i < Columns; i++)
                {
                    ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Star });
                }
            }
            else if (propertyName == MaxWidthExtraSmallProperty.PropertyName ||
                     propertyName == MaxWidthSmallProperty.PropertyName ||
                     propertyName == MaxWidthMediumProperty.PropertyName)
            {
                InvalidateLayout();
            }
        }

        public static void SetResponsiveColumn(View bindable, GridLayout layout)
        {
            bindable.SetValue(ResponsiveColumnProperty, layout);

            (bindable.Parent as GridResponsive)?.InvalidateLayout();
        }

        public static void SetResponsiveColumn(View bindable, int? xs = null, int? sm = null, int? md = null, int? lg = null, int? order = null)
        {
            SetResponsiveColumn(bindable, new GridLayout(xs, sm, md, lg, order));
        }

        public static void SetResponsiveRow(View bindable, int value)
        {
            bindable.SetValue(ResponsiveColumnProperty, value);

            (bindable.Parent as GridResponsive)?.InvalidateLayout();
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            if (Children.Count == 0 || width <= 0)
            {
                base.OnSizeAllocated(width, height);
                return;
            }

            BatchBegin();

            var type = GetLayoutType(width);            

            var layouts = new List<ChildLayout>();

            var order = 0;

            foreach (var child in Children)
            {
                var rw = GetResponsiveRow(child);
                var cs = GetResponsiveColumnSpan(child, type);
                var or = GetResponsiveOrder(child);

                if (!or.HasValue)
                {
                    or = order;
                    order++;
                }

                layouts.Add(new ChildLayout()
                {
                    Element = child,
                    Row = rw,
                    ColumnSpan = cs,
                    Order = or.Value
                });
            }

            var rowReal = 0;
            var rows = layouts.Max(m => m.Row);

            for (var row = 0; row <= rows; row++)
            {
                var views = layouts.Where(w => w.Row == row).OrderBy(o => o.Order).ToList();

                var col = 0;

                foreach (var view in views)
                {
                    if (col + view.ColumnSpan > Columns)
                    {
                        col = 0;
                        rowReal++;
                    }

                    SetColumn(view.Element, col);
                    SetColumnSpan(view.Element, view.ColumnSpan);
                    SetRow(view.Element, rowReal);

                    col += view.ColumnSpan;
                }

                rowReal++;
            }

            BatchCommit();

            base.OnSizeAllocated(width, height);
        }

        private int GetLayoutType(double width)
        {
            if (width <= MaxWidthExtraSmall)
            {
                return 1;
            }

            if (width <= MaxWidthSmall)
            {
                return 2;
            }

            if (width <= MaxWidthMedium)
            {
                return 3;
            }

            return 4;
        }

        private int GetResponsiveRow(View view)
        {
            return (int)view.GetValue(ResponsiveRowProperty);
        }

        private int? GetResponsiveOrder(View view)
        {
            var layout = (GridLayout)view.GetValue(ResponsiveColumnProperty);
            return layout.Order;
        }

        private int GetResponsiveColumnSpan(View view, int type)
        {
            var layout = (GridLayout)view.GetValue(ResponsiveColumnProperty);

            var cs = layout.ExtraSmall.GetValueOrDefault(1);

            if (type >= 2 && layout.Small.GetValueOrDefault() > 0)
            {
                cs = layout.Small.Value;
            }

            if (type >= 3 && layout.Medium.GetValueOrDefault() > 0)
            {
                cs = layout.Medium.Value;
            }

            if (type >= 4 && layout.Large.GetValueOrDefault() > 0)
            {
                cs = layout.Large.Value;
            }

            return cs;
        }
    }
}
