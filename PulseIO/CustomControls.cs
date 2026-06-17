using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace PulseIO
{
    public class RoundedPanel : Panel
    {
        public int CornerRadius { get; set; } = 12;
        public Color BorderColor { get; set; } = Color.FromArgb(226, 232, 240);
        public int BorderWidth { get; set; } = 1;
        public Color FillColor { get; set; } = Color.White;

        public RoundedPanel()
        {
            this.DoubleBuffered = true;
            this.BackColor = Color.Transparent;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            var rect = new Rectangle(0, 0, Width - 1, Height - 1);
            if (rect.Width <= 0 || rect.Height <= 0) return;

            using (var path = GetRoundedPath(rect, CornerRadius))
            {
                using (var brush = new SolidBrush(FillColor))
                {
                    g.FillPath(brush, path);
                }

                if (BorderWidth > 0)
                {
                    using (var pen = new Pen(BorderColor, BorderWidth))
                    {
                        g.DrawPath(pen, path);
                    }
                }
            }
        }

        private GraphicsPath GetRoundedPath(Rectangle rect, int radius)
        {
            var path = new GraphicsPath();
            int r2 = radius * 2;
            if (r2 > rect.Width) r2 = rect.Width;
            if (r2 > rect.Height) r2 = rect.Height;
            if (r2 <= 0) r2 = 1;

            path.AddArc(rect.X, rect.Y, r2, r2, 180, 90);
            path.AddArc(rect.Right - r2, rect.Y, r2, r2, 270, 90);
            path.AddArc(rect.Right - r2, rect.Bottom - r2, r2, r2, 0, 90);
            path.AddArc(rect.X, rect.Bottom - r2, r2, r2, 90, 90);
            path.CloseFigure();
            return path;
        }
    }

    public class ModernCard : GroupBox
    {
        public int CornerRadius { get; set; } = 10;
        public Color BorderColor { get; set; } = Color.FromArgb(226, 232, 240);
        public Color HeaderTextColor { get; set; } = Color.FromArgb(100, 116, 139);
        public Color FillColor { get; set; } = Color.White;

        public ModernCard()
        {
            this.DoubleBuffered = true;
            this.BackColor = Color.Transparent;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            var rect = new Rectangle(0, 0, Width - 1, Height - 1);
            if (rect.Width <= 0 || rect.Height <= 0) return;

            using (var path = GetRoundedPath(rect, CornerRadius))
            {
                // Fill card
                using (var fillBrush = new SolidBrush(FillColor))
                {
                    g.FillPath(fillBrush, path);
                }

                // Draw subtle border
                using (var borderPen = new Pen(BorderColor, 1))
                {
                    g.DrawPath(borderPen, path);
                }
            }

            // Draw header text
            if (!string.IsNullOrEmpty(this.Text))
            {
                using (var font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold))
                using (var brush = new SolidBrush(HeaderTextColor))
                {
                    g.DrawString(this.Text, font, brush, new PointF(16, 12));
                }
            }
        }

        private GraphicsPath GetRoundedPath(Rectangle rect, int radius)
        {
            var path = new GraphicsPath();
            int r2 = radius * 2;
            if (r2 > rect.Width) r2 = rect.Width;
            if (r2 > rect.Height) r2 = rect.Height;
            if (r2 <= 0) r2 = 1;

            path.AddArc(rect.X, rect.Y, r2, r2, 180, 90);
            path.AddArc(rect.Right - r2, rect.Y, r2, r2, 270, 90);
            path.AddArc(rect.Right - r2, rect.Bottom - r2, r2, r2, 0, 90);
            path.AddArc(rect.X, rect.Bottom - r2, r2, r2, 90, 90);
            path.CloseFigure();
            return path;
        }
    }

    public class ModernButton : Button
    {
        public int CornerRadius { get; set; } = 6;
        public Color HoverBackColor { get; set; } = Color.FromArgb(30, 41, 59);
        public Color ActiveBackColor { get; set; } = Color.FromArgb(51, 65, 85);
        private bool isHovered = false;
        private bool isPressed = false;

        public ModernButton()
        {
            this.DoubleBuffered = true;
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;
            this.Cursor = Cursors.Hand;
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            isHovered = true;
            base.OnMouseEnter(e);
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            isHovered = false;
            base.OnMouseLeave(e);
            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            isPressed = true;
            base.OnMouseDown(mevent);
            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            isPressed = false;
            base.OnMouseUp(mevent);
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            var g = pevent.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Color bg = this.BackColor;
            if (isPressed) bg = ActiveBackColor;
            else if (isHovered) bg = HoverBackColor;

            var rect = new Rectangle(0, 0, Width - 1, Height - 1);
            if (rect.Width <= 0 || rect.Height <= 0) return;

            using (var path = GetRoundedPath(rect, CornerRadius))
            {
                using (var brush = new SolidBrush(bg))
                {
                    g.FillPath(brush, path);
                }
            }

            // Draw button text
            TextRenderer.DrawText(
                g,
                this.Text,
                this.Font,
                new Rectangle(0, 0, Width, Height),
                this.ForeColor,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
            );
        }

        private GraphicsPath GetRoundedPath(Rectangle rect, int radius)
        {
            var path = new GraphicsPath();
            int r2 = radius * 2;
            if (r2 > rect.Width) r2 = rect.Width;
            if (r2 > rect.Height) r2 = rect.Height;
            if (r2 <= 0) r2 = 1;

            path.AddArc(rect.X, rect.Y, r2, r2, 180, 90);
            path.AddArc(rect.Right - r2, rect.Y, r2, r2, 270, 90);
            path.AddArc(rect.Right - r2, rect.Bottom - r2, r2, r2, 0, 90);
            path.AddArc(rect.X, rect.Bottom - r2, r2, r2, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
