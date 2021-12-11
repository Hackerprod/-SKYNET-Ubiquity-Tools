using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

internal class FlatColorPalette : Control
{
    private int W;

    private int H;

    private Color _Red;

    private Color _Cyan;

    private Color _Blue;

    private Color _LimeGreen;

    private Color _Orange;

    private Color _Purple;

    private Color _Black;

    private Color _Gray;

    private Color _White;

    [Category("Colors")]
    public Color Red
    {
        get
        {
            return _Red;
        }
        set
        {
            _Red = value;
        }
    }

    [Category("Colors")]
    public Color Cyan
    {
        get
        {
            return _Cyan;
        }
        set
        {
            _Cyan = value;
        }
    }

    [Category("Colors")]
    public Color Blue
    {
        get
        {
            return _Blue;
        }
        set
        {
            _Blue = value;
        }
    }

    [Category("Colors")]
    public Color LimeGreen
    {
        get
        {
            return _LimeGreen;
        }
        set
        {
            _LimeGreen = value;
        }
    }

    [Category("Colors")]
    public Color Orange
    {
        get
        {
            return _Orange;
        }
        set
        {
            _Orange = value;
        }
    }

    [Category("Colors")]
    public Color Purple
    {
        get
        {
            return _Purple;
        }
        set
        {
            _Purple = value;
        }
    }

    [Category("Colors")]
    public Color Black
    {
        get
        {
            return _Black;
        }
        set
        {
            _Black = value;
        }
    }

    [Category("Colors")]
    public Color Gray
    {
        get
        {
            return _Gray;
        }
        set
        {
            _Gray = value;
        }
    }

    [Category("Colors")]
    public Color White
    {
        get
        {
            return _White;
        }
        set
        {
            _White = value;
        }
    }

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        base.Width = 180;
        base.Height = 80;
    }

    public FlatColorPalette()
    {
        _Red = Color.FromArgb(220, 85, 96);
        _Cyan = Color.FromArgb(10, 154, 157);
        _Blue = Color.FromArgb(0, 128, 255);
        _LimeGreen = Color.FromArgb(35, 168, 109);
        _Orange = Color.FromArgb(253, 181, 63);
        _Purple = Color.FromArgb(155, 88, 181);
        _Black = Color.FromArgb(45, 47, 49);
        _Gray = Color.FromArgb(63, 70, 73);
        _White = Color.FromArgb(243, 243, 243);
        SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, value: true);
        DoubleBuffered = true;
        BackColor = Color.FromArgb(60, 70, 73);
        base.Size = new Size(160, 80);
        Font = new Font("Segoe UI", 12f);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        Helpers.B = new Bitmap(base.Width, base.Height);
        Helpers.G = Graphics.FromImage(Helpers.B);
        checked
        {
            W = base.Width - 1;
            H = base.Height - 1;
            Graphics g = Helpers.G;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            g.Clear(ColorSystem.FontColorLabels);
            g.FillRectangle(new SolidBrush(_Red), new Rectangle(0, 0, 20, 40));
            g.FillRectangle(new SolidBrush(_Cyan), new Rectangle(20, 0, 20, 40));
            g.FillRectangle(new SolidBrush(_Blue), new Rectangle(40, 0, 20, 40));
            g.FillRectangle(new SolidBrush(_LimeGreen), new Rectangle(60, 0, 20, 40));
            g.FillRectangle(new SolidBrush(_Orange), new Rectangle(80, 0, 20, 40));
            g.FillRectangle(new SolidBrush(_Purple), new Rectangle(100, 0, 20, 40));
            g.FillRectangle(new SolidBrush(_Black), new Rectangle(120, 0, 20, 40));
            g.FillRectangle(new SolidBrush(_Gray), new Rectangle(140, 0, 20, 40));
            g.FillRectangle(new SolidBrush(_White), new Rectangle(160, 0, 20, 40));
            g.DrawString("Color Palette", Font, new SolidBrush(_White), new Rectangle(0, 22, W, H), Helpers.CenterSF);
            base.OnPaint(e);
            Helpers.G.Dispose();
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            e.Graphics.DrawImageUnscaled(Helpers.B, 0, 0);
            Helpers.B.Dispose();
        }
    }
}
