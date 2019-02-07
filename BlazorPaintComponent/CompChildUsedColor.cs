using BlazorSvgHelper;
using BlazorSvgHelper.Classes.SubClasses;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorPaintComponent
{
    public class CompChildUsedColor : ComponentBase
    {

        [Parameter]
        protected ComponentBase parent { get; set; }

        [Parameter]
        protected string color { get; set; }

        public Action<string> ActionClicked { get; set; }

        private SvgHelper SvgHelper1 = new SvgHelper();

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            Console.WriteLine(color.ToString());
            int par_id = (parent as CompUsedColors).UsedColors_List.IndexOf(color);
            Console.WriteLine(par_id.ToString());
            circle c = new circle()
            {
                cx = (9 - par_id) * 30 + 15,
                cy = 15,
                r = 10,
                fill = color,
                stroke = "black",
                stroke_width = 1,
                onclick = "notEmpty",
            };


            SvgHelper1.Cmd_Render(c, 0, builder);

            base.BuildRenderTree(builder);

        }


        protected override void OnAfterRender()
        {
            SvgHelper1.ActionClicked = ComponentClicked;
            (parent as CompUsedColors).Curr_CompChildUsedColor_List.Add(this);

        }

        public void ComponentClicked(UIMouseEventArgs e)
        {
            (parent as CompUsedColors).ColorSelected(color);
        }


        public void Refresh()
        {
            StateHasChanged();
        }

        public void Dispose()
        {

        }



    }
}
