using BlazorPaintComponent.classes;
using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Blazor.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorPaintComponent
{
    public class CompBlazorPaint_Logic: BlazorComponent
    {
        bool IsCompLoaded = false;

        int CurrObjectID = 0;


        protected CompUsedColors_Logic Curr_CompUsedColors = new CompUsedColors_Logic();
        protected CompMySVG Curr_CompMySVG = new CompMySVG();


        public List<BPaintObject> ObjectsList = new List<BPaintObject>();


        protected string Color1 = "#ffffff";//"#fc3807";

        protected int LineWidth1 = 3;

        protected int StepSize = 5;


        protected int modeCode = 1;


        protected override void OnInit()
        {
            

            base.OnInit();
        }



        protected override void OnAfterRender()
        {

            if (IsCompLoaded == false)
            {
                GetBoundingClientRect("PaintArea1");

                Curr_CompUsedColors.ActionColorClicked = ColorSelected;

                IsCompLoaded = true;
            }

            base.OnAfterRender();

        }


        private void ColorSelected(string a)
        {
            Color1 = a;
            StateHasChanged();
        }

        public void cmd_clear()
        {
            if (ObjectsList.Any())
            {
                ObjectsList = new List<BPaintObject>();
                cmd_RefreshSVG();
            }
        }


        public void cmd_undo()
        {
            if (ObjectsList.Any())
            {
                ObjectsList.Remove(ObjectsList.Last());
                cmd_RefreshSVG();
            }
        }


        public void cmd_onmousedown(UIMouseEventArgs e)
        {
            if (modeCode == 1)
            {
                cmd_prepareDraw(e);
            }

            if (modeCode == 2)
            {
                cmd_prepareLine(e);
            }

        }



        public void cmd_prepareDraw(UIMouseEventArgs e)
        {
            MyPoint CurrPosition = new MyPoint() { x = e.ClientX - LocalData.SVGPosition.x, y = e.ClientY - LocalData.SVGPosition.y };

            BPaintHandDraw new_BPaintHandDraw = new BPaintHandDraw();


            if (ObjectsList.Any())
            {
                new_BPaintHandDraw.ObjectID = ObjectsList.Max(x => x.ObjectID) + 1;
            }
            else
            {
                new_BPaintHandDraw.ObjectID = 1;
            }

            new_BPaintHandDraw.ObjectType = BPaintOpbjectType.HandDraw;
            new_BPaintHandDraw.Color = Color1;
            new_BPaintHandDraw.width = LineWidth1;
            new_BPaintHandDraw.data = new List<MyPoint>();
            new_BPaintHandDraw.data.Add(new MyPoint() { x = CurrPosition.x, y = CurrPosition.y });

            ObjectsList.Add(new_BPaintHandDraw);


            CurrObjectID = new_BPaintHandDraw.ObjectID;

        }


        public void cmd_prepareLine(UIMouseEventArgs e)
        {
            MyPoint CurrPosition = new MyPoint() { x = e.ClientX - LocalData.SVGPosition.x, y = e.ClientY - LocalData.SVGPosition.y };

            BPaintLine new_BPaintLine = new BPaintLine();


            if (ObjectsList.Any())
            {
                new_BPaintLine.ObjectID = ObjectsList.Max(x => x.ObjectID) + 1;


                foreach (var item in ObjectsList.Where(x => x.Selected))
                {
                    item.Selected = false;
                }  
            }
            else
            {
                new_BPaintLine.ObjectID = 1;
            }


            
           


            new_BPaintLine.ObjectType = BPaintOpbjectType.Line;
            new_BPaintLine.Selected = true;
            new_BPaintLine.Color = Color1;
            new_BPaintLine.width = LineWidth1;
            new_BPaintLine.start = new MyPoint() { x = CurrPosition.x, y = CurrPosition.y };
            new_BPaintLine.end = new_BPaintLine.start;
            ObjectsList.Add(new_BPaintLine);


            CurrObjectID = new_BPaintLine.ObjectID;

        }


        public void cmd_onmousemove(UIMouseEventArgs e)
        {
            if (CurrObjectID > 0)
            {
                BPaintJsInterop.log("a");
                if (modeCode == 1)
                {
                    cmd_draw(e);
                }

                if (modeCode == 2)
                {
                    cmd_Line(e);
                }
            }
        }


        public void cmd_draw(UIMouseEventArgs e)
        {
            if (CurrObjectID > 0)
            {


                MyPoint CurrPosition = new MyPoint() { x = e.ClientX - LocalData.SVGPosition.x, y = e.ClientY - LocalData.SVGPosition.y };


                BPaintHandDraw Curr_Object = (BPaintHandDraw)ObjectsList.Single(x => x.ObjectID == CurrObjectID);

                if (Curr_Object.data.Any())
                {

                    MyPoint LastPoint = Curr_Object.data.Last();

                    if (LastPoint.x != CurrPosition.x || LastPoint.y != CurrPosition.y)
                    {
                        Curr_Object.data.Add(CurrPosition);
                        cmd_RefreshSVG();
                    }
                }
                else
                {
                    Curr_Object.data.Add(CurrPosition);
                    cmd_RefreshSVG();
                }
            }
        }

        public void cmd_Line(UIMouseEventArgs e)
        {
            if (CurrObjectID > 0)
            {
                MyPoint CurrPosition = new MyPoint() { x = e.ClientX - LocalData.SVGPosition.x, y = e.ClientY - LocalData.SVGPosition.y };


                BPaintLine Curr_Object = (BPaintLine)ObjectsList.Single(x => x.ObjectID == CurrObjectID);


                if (Curr_Object.end.x != CurrPosition.x || Curr_Object.end.y != CurrPosition.y)
                {
                    Curr_Object.end = CurrPosition;
                    cmd_RefreshSVG();
                }
            }
        }


        public void cmd_onmouseup(UIMouseEventArgs e)
        {

            CurrObjectID = 0;

        }


        void cmd_RefreshSVG()
        {
            Curr_CompMySVG.Refresh();
            StateHasChanged();

        }



        protected void cmd_ColorChange(UIChangeEventArgs e)
        {
            if (e?.Value != null)
            {
                Color1 = e.Value as string;

                if (Curr_CompUsedColors.UsedColors_List.Any(x => x == Color1))
                {
                    Curr_CompUsedColors.UsedColors_List.Remove(Curr_CompUsedColors.UsedColors_List.Single(x => x == Color1));
                }

                if (Curr_CompUsedColors.UsedColors_List.Count > 9)
                {
                    Curr_CompUsedColors.UsedColors_List.RemoveAt(0);
                }

                Curr_CompUsedColors.UsedColors_List.Add(Color1);


                Cmd_RefreshUsedColorsSVG();
            }
        }

        public void Cmd_RefreshUsedColorsSVG()
        {
            Curr_CompUsedColors.Refresh();
            StateHasChanged();
        }

        public void Dispose()
        {

        }



        


        protected void cmd_Move(BPaintMoveDirection Par_Direction)
        {

            bool returnZeroID = CurrObjectID == 0;

            if (ObjectsList.Any())
            {
                if (returnZeroID)
                {

                    CurrObjectID = ObjectsList.Last().ObjectID;
    
                }
                

                BPaintObject Curr_Object = ObjectsList.Single(x => x.ObjectID == CurrObjectID);


                switch (Par_Direction)
                {
                    case BPaintMoveDirection.left:
                        Curr_Object.PositionChange.x -= StepSize;
                        break;
                    case BPaintMoveDirection.right:
                        Curr_Object.PositionChange.x += StepSize;
                        break;
                    case BPaintMoveDirection.up:
                        Curr_Object.PositionChange.y -= StepSize;
                        break;
                    case BPaintMoveDirection.down:
                        Curr_Object.PositionChange.y += StepSize;
                        break;
                    default:
                        break;
                }

                

                cmd_RefreshSVG();

                if (returnZeroID)
                {
                    CurrObjectID = 0;
                }
            }
          
        }


        public void GetBoundingClientRect(string ElementID)
        {
            BPaintJsInterop.GetElementBoundingClientRect(ElementID, new DotNetObjectRef(this));
        }

        [JSInvokable]
        public void invokeFromjs(string id, double par_x, double par_y)
        {
          LocalData.SVGPosition = new MyPoint() { x = par_x, y = par_y };
            
        }

    }
}
