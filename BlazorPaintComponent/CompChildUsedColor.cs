using BlazorSvgHelper;
using BlazorSvgHelper.Classes.SubClasses;
using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Blazor.Components;
using Microsoft.AspNetCore.Blazor.RenderTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorPaintComponent
{
    public class CompChildUsedColor : BlazorComponent
    {

        [Parameter]
        protected BlazorComponent parent { get; set; }

        [Parameter]
        protected string par_color { get; set; }

        public Action<string> ActionClicked { get; set; }

        private SvgHelper SvgHelper1 = new SvgHelper();

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {

            int par_id = (parent as CompUsedColors).UsedColors_List.IndexOf(par_color);

            circle c = new circle()
            {
                cx = (9- par_id) * 30 + 15,
                cy = 15,
                r = 10,
                fill = par_color,
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
            (parent as CompUsedColors).ColorSelected(par_color);
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
