
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

internal class FlatClose : Control
{
    private MouseState State;

    protected override void OnMouseEnter(EventArgs e)
    {
        base.OnMouseEnter(e);
        State = MouseState.Over;
        Invalidate();
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        base.OnMouseDown(e);
        State = MouseState.Down;
        Invalidate();
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        base.OnMouseLeave(e);
        State = MouseState.None;
        Invalidate();
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        base.OnMouseUp(e);
        State = MouseState.Over;
        Invalidate();
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        Invalidate();
    }

    protected override void OnClick(EventArgs e)
    {
        base.OnClick(e);
        Environment.Exit(0);
    }

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        base.Size = new Size(18, 18);
    }

    public FlatClose()
    {
        State = MouseState.None;
        SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, value: true);
        DoubleBuffered = true;
        BackColor = Color.White;
        base.Size = new Size(18, 18);
        Anchor = (AnchorStyles.Top | AnchorStyles.Right);
        Font = new Font("Marlett", 10f);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        Bitmap bitmap = new Bitmap(base.Width, base.Height);
        Graphics graphics = Graphics.FromImage(bitmap);
        Rectangle rect = new Rectangle(0, 0, base.Width, base.Height);
        Graphics graphics2 = graphics;
        graphics2.SmoothingMode = SmoothingMode.HighQuality;
        graphics2.PixelOffsetMode = PixelOffsetMode.HighQuality;
        graphics2.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
        graphics2.Clear(ColorSystem.FontColorLabels);
        graphics2.FillRectangle(new SolidBrush(ColorSystem.BackDark), rect);
        graphics2.DrawString("r", Font, new SolidBrush(ColorSystem.FontColorButtons), new Rectangle(0, 0, base.Width, base.Height), Helpers.CenterSF);
        switch (State)
        {
            case MouseState.Over:
                graphics2.FillRectangle(new SolidBrush(Color.FromArgb(30, Color.White)), rect);
                break;
            case MouseState.Down:
                graphics2.FillRectangle(new SolidBrush(Color.FromArgb(30, Color.Black)), rect);
                break;
        }
        graphics2 = null;
        base.OnPaint(e);
        graphics.Dispose();
        e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
        e.Graphics.DrawImageUnscaled(bitmap, 0, 0);
        bitmap.Dispose();
    }
}
