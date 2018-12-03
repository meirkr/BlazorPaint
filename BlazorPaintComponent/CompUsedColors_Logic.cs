using BlazorSvgHelper;
using BlazorSvgHelper.Classes.SubClasses;
using Microsoft.AspNetCore.Blazor.Components;
using Microsoft.AspNetCore.Blazor.RenderTree;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorPaintComponent
{
    public class CompUsedColors_Logic : BlazorComponent, IDisposable
    {
        public List<string> UsedColors_List = new List<string>()
        { "green", "white","red", "blue", "yellow", "gray", "silver","brown", "gold", "black"};
        public List<CompChildUsedColor> Curr_CompChildUsedColor_List = new List<CompChildUsedColor>();

        public Action<string> ActionColorClicked { get; set; }


        protected override void OnInit()
        {
            for (int i = 0; i < UsedColors_List.Count; i++)
            {
                UsedColors_List[i] = Get_Hex_Code_From_Color_Name(UsedColors_List[i]);
            }

            base.OnInit();
        }


        private string Get_Hex_Code_From_Color_Name(string name)
        {
            Color c = Color.FromName(name);
            return string.Format("#{0:X2}{1:X2}{2:X2}", c.R, c.G, c.B); ;

        }



        public void ColorSelected(string a)
        {

            ActionColorClicked?.Invoke(a);
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
