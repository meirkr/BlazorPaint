using BlazorPaintComponent.classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorPaintComponent
{
    public static class BPaintFunctions
    {

        public static MyPointRect Get_Border_Points(BPaintHandDraw Par_obj)
        {

            MyPointRect result = new MyPointRect();

            result.x = Par_obj.data.Min(j => j.x);
            result.y = Par_obj.data.Min(j => j.y);
            result.width = Par_obj.data.Max(j => j.x) - result.x;
            result.height = Par_obj.data.Max(j => j.y) - result.y;

            Set_Padding(result);

            return result;


        }


        public static MyPointRect Get_Border_Points(BPaintLine Par_obj)
        {

            MyPointRect result = new MyPointRect();


            List<MyPoint> data = new List<MyPoint>();
            data.Add(Par_obj.start);
            data.Add(Par_obj.end);

            result.x = data.Min(j => j.x);
            result.y = data.Min(j => j.y);
            result.width = data.Max(j => j.x) - result.x;
            result.height = data.Max(j => j.y) - result.y;

            Set_Padding(result);

            return result;


        }


        public static void Set_Padding(MyPointRect r)
        {

            int padding = 15;

            r.x -= padding;
            r.y -= padding;
            r.width += padding * 2;
            r.height += padding * 2;

          

        }
        }
}
