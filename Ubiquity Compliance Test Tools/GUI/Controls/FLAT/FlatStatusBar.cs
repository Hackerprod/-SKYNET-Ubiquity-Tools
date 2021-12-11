
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
internal class FlatStatusBar : Control
{
    private int W;

    private int H;

    private bool _ShowTimeDate;

    private Color _BaseColor;

    private Color _TextColor;

    private Color _RectColor;

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

    [Category("Colors")]
    public Color RectColor
    {
        get
        {
            return _RectColor;
        }
        set
        {
            _RectColor = value;
        }
    }

    public bool ShowTimeDate
    {
        get
        {
            return _ShowTimeDate;
        }
        set
        {
            _ShowTimeDate = value;
        }
    }

    protected override void CreateHandle()
    {
        base.CreateHandle();
    }

    protected override void OnTextChanged(EventArgs e)
    {
        base.OnTextChanged(e);
        Invalidate();
    }

    public string GetTimeDate()
    {
        return Conversions.ToString(DateTime.Now.Date) + " " + Conversions.ToString(DateTime.Now.Hour) + ":" + Conversions.ToString(DateTime.Now.Minute);
    }

    public FlatStatusBar()
    {
        _ShowTimeDate = false;
        _BaseColor = Color.FromArgb(45, 47, 49);
        _TextColor = Color.White;
        _RectColor = Helpers._FlatColor;
        SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, value: true);
        DoubleBuffered = true;
        Font = new Font("Segoe UI", 8f);
        ForeColor = Color.White;
        base.Size = new Size(base.Width, 20);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        Helpers.B = new Bitmap(base.Width, base.Height);
        Helpers.G = Graphics.FromImage(Helpers.B);
        W = base.Width;
        H = base.Height;
        Rectangle rect = new Rectangle(0, 0, W, H);
        Graphics g = Helpers.G;
        g.SmoothingMode = SmoothingMode.HighQuality;
        g.PixelOffsetMode = PixelOffsetMode.HighQuality;
        g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
        g.Clear(BaseColor);
        g.FillRectangle(new SolidBrush(BaseColor), rect);
        g.DrawString(Text, Font, Brushes.White, new Rectangle(10, 4, W, H), Helpers.NearSF);
        g.FillRectangle(new SolidBrush(_RectColor), new Rectangle(4, 4, 4, 14));
        if (ShowTimeDate)
        {
            g.DrawString(GetTimeDate(), Font, new SolidBrush(_TextColor), new Rectangle(-4, 2, W, H), new StringFormat
            {
                Alignment = StringAlignment.Far,
                LineAlignment = StringAlignment.Center
            });
        }
        g = null;
        base.OnPaint(e);
        Helpers.G.Dispose();
        e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
        e.Graphics.DrawImageUnscaled(Helpers.B, 0, 0);
        Helpers.B.Dispose();
    }
}
