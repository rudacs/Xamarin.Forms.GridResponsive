using System;

namespace Xamarin.Forms.GridResponsive
{
    public static class GridResponsiveGlobal
    {
        public static int Columns { get; set; } = 8;

        public static double ColumnSpacing { get; set; } = 6;
        public static double RowSpacing { get; set; } = 6;

        public static double MaxWidthExtraSmall { get; set; } = 400;
        public static double MaxWidthSmall { get; set; } = 800;
        public static double MaxWidthMedium { get; set; } = 1200;
    }
}
