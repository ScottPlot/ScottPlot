using System;
using System.Collections.Generic;
using Eto.Forms;
using Eto.Drawing;

namespace ScottPlot.Demo.Eto
{
    class BackgroundImageControl : Drawable
    {
        public Image BackgroundImage { get; set; }
        protected override void OnPaint(PaintEventArgs e)
        {
            if (BackgroundImage != null)
                for (PointF p = PointF.Empty; p.X < e.ClipRectangle.Width; p.X += BackgroundImage.Width)
                    for (PointF p2 = p; p2.Y < e.ClipRectangle.Height; p2.Y += BackgroundImage.Height)
                        e.Graphics.DrawImage(BackgroundImage, p2);

            base.OnPaint(e);
        }
    }

    public static class Helpers
    {
        public static void ShowDialog(this Form form)
        {
            form.Show();
        }

        public static TreeGridItemCollection Nodes(this TreeGridView gv)
        {
            if (gv.DataStore == null)
                gv.DataStore = new TreeGridItemCollection();

            gv.DataStore = gv.DataStore;

            return gv.DataStore as TreeGridItemCollection;
        }

        public static void SetPaddingRecursive(this Panel panel, Padding padding)
        {
            foreach (var control in panel.Controls)
                if (control is Panel)
                    SetPaddingRecursive(control as Panel, padding);

            panel.Padding = padding;
        }
    }
}
