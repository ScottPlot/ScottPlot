using Eto.Forms;
using System;
using System.Collections.Generic;
using System.Text;
using ScottPlot.Control;
using SkiaSharp;
using Eto.Drawing;
using System.Runtime.InteropServices;
using System.IO;

namespace ScottPlot.Eto
{
    public class EtoPlot : Drawable, IPlotControl
    {
        public Plot Plot { get; } = new();

        public Interaction Interaction { get; private set; }

        private readonly List<FileFilter> fileDialogFilters = new()
        {
            new() { Name = "PNG Files", Extensions = new string[] { "png" } },
            new() { Name = "JPEG Files", Extensions = new string[] { "jpg", "jpeg" } },
            new() { Name = "BMP Files", Extensions = new string[] { "bmp" } },
            new() { Name = "WebP Files", Extensions = new string[] { "webp" } },
            new() { Name = "All Files", Extensions = new string[] { "*" } },
        };

        public EtoPlot()
        {
            Interaction = new(this)
            {
                ContextMenuItems = GetDefaultContextMenuItems()
            };

            this.MouseDown += OnMouseDown;
            this.MouseUp += OnMouseUp;
            this.MouseMove += OnMouseMove;
            this.MouseWheel += OnMouseWheel;
            this.KeyDown += OnKeyDown;
            this.KeyUp += OnKeyUp;
            this.MouseDoubleClick += OnDoubleClick;
            this.SizeChanged += OnSizeChanged;
        }
        private ContextMenuItem[] GetDefaultContextMenuItems()
        {
            ContextMenuItem saveImage = new() { Label = "Save Image", OnInvoke = OpenSaveImageDialog };
            ContextMenuItem copyImage = new() { Label = "Copy to Clipboard", OnInvoke = CopyImageToClipboard };

            return new ContextMenuItem[] { saveImage, copyImage };
        }

        private ContextMenu GetContextMenu()
        {
            ContextMenu menu = new();
            foreach (var curr in Interaction.ContextMenuItems)
            {
                var menuItem = new ButtonMenuItem() { Text = curr.Label };
                menuItem.Click += (s, e) => curr.OnInvoke();

                menu.Items.Add(menuItem);
            }

            return menu;
        }

        public void Replace(Interaction interaction)
        {
            Interaction = interaction;
        }

        public void Refresh()
        {
            Invalidate();
        }

        public void ShowContextMenu(Pixel position)
        {
            var menu = GetContextMenu();
            menu.Show(this, new Point((int)position.X, (int)position.Y));
        }

        protected override void OnPaint(PaintEventArgs args)
        {
            base.OnPaint(args);

            SKImageInfo imageInfo = new((int)Bounds.Width, (int)Bounds.Height);

            using var surface = SKSurface.Create(imageInfo);
            if (surface is null)
                return;

            Plot.Render(surface);

            SKImage img = surface.Snapshot();
            SKPixmap pixels = img.ToRasterImage().PeekPixels();
            byte[] bytes = pixels.GetPixelSpan().ToArray();

            var bmp = new Bitmap((int)Bounds.Width, (int)Bounds.Height, PixelFormat.Format32bppRgba);

            using (var data = bmp.Lock())
            {
                Marshal.Copy(bytes, 0, data.Data, bytes.Length);
            }

            args.Graphics.DrawImage(bmp, 0, 0);
        }

        private void OnMouseDown(object? sender, MouseEventArgs e)
        {
            Focus();

            Interaction.MouseDown(e.Pixel(), e.ToButton());
        }

        private void OnMouseUp(object? sender, MouseEventArgs e)
        {
            Interaction.MouseUp(e.Pixel(), e.ToButton());
        }

        private void OnMouseMove(object? sender, MouseEventArgs e)
        {
            Interaction.OnMouseMove(e.Pixel());
        }

        private void OnMouseWheel(object? sender, MouseEventArgs e)
        {
            Interaction.MouseWheelVertical(e.Pixel(), e.Delta.Height);
        }

        private void OnKeyDown(object? sender, KeyEventArgs e)
        {
            Interaction.KeyDown(e.Key());
        }

        private void OnKeyUp(object? sender, KeyEventArgs e)
        {
            Interaction.KeyUp(e.Key());
        }

        private void OnDoubleClick(object? sender, MouseEventArgs e)
        {
            Interaction.DoubleClick();
        }

        private void OnSizeChanged(object? sender, EventArgs e)
        {
            Refresh();
        }

        private void OpenSaveImageDialog()
        {
            SaveFileDialog dialog = new() { FileName = "ScottPlot" };

            foreach (var curr in fileDialogFilters)
                dialog.Filters.Add(curr);

            if (dialog.ShowDialog(this) == DialogResult.Ok)
            {
                var filename = dialog.FileName;

                if (string.IsNullOrEmpty(filename))
                    return;

                // Eto doesn't add the extension for you when you select a filter :/
                if (!Path.HasExtension(filename))
                    filename += $".{dialog.CurrentFilter.Extensions[0]}";

                var format = ImageFormatHelpers.FromFilePath(filename);
                if (!format.HasValue)
                {
                    return;
                }

                try
                {
                    Plot.Save(filename, format: format.Value);
                }
                catch (Exception)
                {
                    // TODO: Not sure if we can meaningfully do anything except perhaps show an error dialog?
                }
            }
        }

        private void CopyImageToClipboard()
        {
            using var bmp = new Bitmap(new MemoryStream(Plot.GetImage().GetImageBytes()));

            Clipboard.Instance.Image = bmp;
        }
    }
}
