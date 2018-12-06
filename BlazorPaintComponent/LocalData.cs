using BlazorPaintComponent.classes;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorPaintComponent
{
    public static class LocalData
    {

        public static MyPoint SVGPosition = new MyPoint(0, 0);

        [JSInvokable]
        public static void invokeFromjs_UpdateSVGPosition(double par_x, double par_y)
        {
            SVGPosition = new MyPoint(par_x, par_y);

        }

      
    }
}
