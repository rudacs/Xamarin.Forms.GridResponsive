using System;
namespace Xamarin.Forms.GridResponsive
{
    public struct GridLayout
    {
        public int? ExtraSmall { get; set; }
        public int? Small { get; set; }
        public int? Medium { get; set; }
        public int? Large { get; set; }
        public int? Order { get; set; }

        public GridLayout(int? xs = null, int? sm = null, int? md = null, int? lg = null, int? order = null)
        {
            ExtraSmall = xs;
            Small = sm;
            Medium = md;
            Large = lg;
            Order = order;
        }
    }
}
