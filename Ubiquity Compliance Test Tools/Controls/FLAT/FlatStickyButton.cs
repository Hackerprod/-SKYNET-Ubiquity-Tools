using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;


internal class FlatStickyButton : Control
{
    private int W;

    private int H;

    private MouseState State;

    private bool _Rounded;

    private Color _BaseColor;

    private Color _TextColor;

    private Rectangle Rect => new Rectangle(base.Left, base.Top, base.Width, base.Height);

    [Category("Colors")]
    public Color BaseColor
    {
        get
        {
            return _BaseColor;
        }
        set
        {
            _BaseColor = value;
        }
    }

    [Category("Colors")]
    public Color TextColor
    {
        get
        {
            return _TextColor;
        }
        set
        {
            _TextColor = value;
        }
    }

    [Category("Options")]
    public bool Rounded
    {
        get
        {
            return _Rounded;
        }
        set
        {
            _Rounded = value;
        }
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        base.OnMouseDown(e);
        State = MouseState.Down;
        Invalidate();
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        base.OnMouseUp(e);
        State = MouseState.Over;
        Invalidate();
    }

    protected override void OnMouseEnter(EventArgs e)
    {
        base.OnMouseEnter(e);
        State = MouseState.Over;
        Invalidate();
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        base.OnMouseLeave(e);
        State = MouseState.None;
        Invalidate();
    }

    private bool[] GetConnectedSides()
    {
        bool[] array = new bool[4];
        IEnumerator enumerator = default(IEnumerator);
        try
        {
            enumerator = base.Parent.Controls.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Control control = (Control)enumerator.Current;
                if (control is FlatStickyButton && !((control == this) | !Rect.IntersectsWith(Rect)))
                {
                    double num = Math.Atan2((double)checked(base.Left - control.Left), (double)checked(base.Top - control.Top)) * 2.0 / 3.1415926535897931;
                    if ((double)(checked((long)Math.Round(num)) / 1) == num)
                    {
                        checked
                        {
                            array[(int)Math.Round(unchecked(num + 1.0))] = true;
                        }
                    }
                }
            }
            return array;
        }
        finally
        {
            if (enumerator is IDisposable)
            {
                (enumerator as IDisposable).Dispose();
            }
        }
    }

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
    }

    protected override void OnCreateControl()
    {
        base.OnCreateControl();
    }

    public FlatStickyButton()
    {
        State = MouseState.None;
        _Rounded = false;
        _BaseColor = Helpers._FlatColor;
        _TextColor = Color.FromArgb(243, 243, 243);
        SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, value: true);
        DoubleBuffered = true;
        base.Size = new Size(106, 32);
        BackColor = Color.Transparent;
        Font = new Font("Segoe UI", 12f);
        Cursor = Cursors.Hand;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        Helpers.B = new Bitmap(base.Width, base.Height);
        Helpers.G = Graphics.FromImage(Helpers.B);
        W = base.Width;
        H = base.Height;
        GraphicsPath graphicsPath = new GraphicsPath();
        bool[] connectedSides = GetConnectedSides();
        GraphicsPath graphicsPath2 = Helpers.RoundRect(0f, 0f, (float)W, (float)H, 0.3f, !(connectedSides[2] | connectedSides[1]), !(connectedSides[1] | connectedSides[0]), !(connectedSides[3] | connectedSides[0]), !(connectedSides[3] | connectedSides[2]));
        Rectangle rectangle = new Rectangle(0, 0, W, H);
        Graphics g = Helpers.G;
        g.SmoothingMode = SmoothingMode.HighQuality;
        g.PixelOffsetMode = PixelOffsetMode.HighQuality;
        g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
        g.Clear(ColorSystem.FontColorLabels);
        switch (State)
        {
            case MouseState.None:
                if (Rounded)
                {
                    graphicsPath = graphicsPath2;
                    g.FillPath(new SolidBrush(_BaseColor), graphicsPath);
                    g.DrawString(Text, Font, new SolidBrush(_TextColor), rectangle, Helpers.CenterSF);
                }
                else
                {
                    g.FillRectangle(new SolidBrush(_BaseColor), rectangle);
                    g.DrawString(Text, Font, new SolidBrush(_TextColor), rectangle, Helpers.CenterSF);
                }
                break;
            case MouseState.Over:
                if (Rounded)
                {
                    graphicsPath = graphicsPath2;
                    g.FillPath(new SolidBrush(_BaseColor), graphicsPath);
                    g.FillPath(new SolidBrush(Color.FromArgb(20, Color.White)), graphicsPath);
                    g.DrawString(Text, Font, new SolidBrush(_TextColor), rectangle, Helpers.CenterSF);
                }
                else
                {
                    g.FillRectangle(new SolidBrush(_BaseColor), rectangle);
                    g.FillRectangle(new SolidBrush(Color.FromArgb(20, Color.White)), rectangle);
                    g.DrawString(Text, Font, new SolidBrush(_TextColor), rectangle, Helpers.CenterSF);
                }
                break;
            case MouseState.Down:
                if (Rounded)
                {
                    graphicsPath = graphicsPath2;
                    g.FillPath(new SolidBrush(_BaseColor), graphicsPath);
                    g.FillPath(new SolidBrush(Color.FromArgb(20, Color.Black)), graphicsPath);
                    g.DrawString(Text, Font, new SolidBrush(_TextColor), rectangle, Helpers.CenterSF);
                }
                else
                {
                    g.FillRectangle(new SolidBrush(_BaseColor), rectangle);
                    g.FillRectangle(new SolidBrush(Color.FromArgb(20, Color.Black)), rectangle);
                    g.DrawString(Text, Font, new SolidBrush(_TextColor), rectangle, Helpers.CenterSF);
                }
                break;
        }
        g = null;
        base.OnPaint(e);
        Helpers.G.Dispose();
        e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
        e.Graphics.DrawImageUnscaled(Helpers.B, 0, 0);
        Helpers.B.Dispose();
    }
}

