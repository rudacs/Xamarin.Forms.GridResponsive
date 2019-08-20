using System;
namespace Xamarin.Forms.GridResponsive
{
    internal class ChildLayout
    {
        public View Element { get; set; }
        public int Row { get; set; }
        public int ColumnSpan { get; set; }
        public int Order { get; set; }
    }
}
