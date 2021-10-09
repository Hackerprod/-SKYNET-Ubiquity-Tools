
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
internal class FlatNumeric : Control
{
    private int W;

    private int H;

    private int x;

    private int y;

    private long _Value;

    private long _Min;

    private long _Max;

    private bool Bool;

    private Color _BaseColor;

    private Color _ButtonColor;

    public long Value
    {
        get
        {
            return _Value;
        }
        set
        {
            if ((value <= _Max) & (value >= _Min))
            {
                _Value = value;
            }
            Invalidate();
        }
    }

    public long Maximum
    {
        get
        {
            return _Max;
        }
        set
        {
            if (value > _Min)
            {
                _Max = value;
            }
            if (_Value > _Max)
            {
                _Value = _Max;
            }
            Invalidate();
        }
    }

    public long Minimum
    {
        get
        {
            return _Min;
        }
        set
        {
            if (value < _Max)
            {
                _Min = value;
            }
            if (_Value < _Min)
            {
                _Value = Minimum;
            }
            Invalidate();
        }
    }

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
    public Color ButtonColor
    {
        get
        {
            return _ButtonColor;
        }
        set
        {
            _ButtonColor = value;
        }
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        x = e.Location.X;
        y = e.Location.Y;
        Invalidate();
        if (e.X < checked(base.Width - 23))
        {
            Cursor = Cursors.IBeam;
        }
        else
        {
            Cursor = Cursors.Hand;
        }
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        base.OnMouseDown(e);
        checked
        {
            if (x > base.Width - 21 && x < base.Width - 3)
            {
                if (y < 15)
                {
                    if (Value + 1 <= _Max)
                    {
						long value = _Value;
						long reference = value;
                        value = reference + 1;
                    }
                }
                else if (Value - 1 >= _Min)
                {
					long value2 = _Value;
					long reference = value2;
                    value2 = reference - 1;
                }
            }
            else
            {
                Bool = !Bool;
                Focus();
            }
            Invalidate();
        }
    }

    protected override void OnKeyPress(KeyPressEventArgs e)
    {
        base.OnKeyPress(e);
        try
        {
            if (Bool)
            {
                _Value = Conversions.ToLong(Conversions.ToString(_Value) + e.KeyChar.ToString());
            }
            if (_Value > _Max)
            {
                _Value = _Max;
            }
            Invalidate();
        }
        catch (Exception projectError)
        {
            ProjectData.SetProjectError(projectError);
            ProjectData.ClearProjectError();
        }
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);
        if (e.KeyCode == Keys.Back)
        {
            Value = 0L;
        }
    }

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        base.Height = 30;
    }

    public FlatNumeric()
    {
        _BaseColor = Color.FromArgb(45, 47, 49);
        _ButtonColor = Helpers._FlatColor;
        SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, value: true);
        DoubleBuffered = true;
        Font = new Font("Segoe UI", 10f);
        BackColor = Color.FromArgb(60, 70, 73);
        ForeColor = Color.White;
        _Min = 0L;
        _Max = 9999999L;
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
        g.Clear(ColorSystem.FontColorLabels);
        g.FillRectangle(new SolidBrush(ColorSystem.BackDark), rect);
        checked
        {
            g.FillRectangle(new SolidBrush(ColorSystem.BackDark), new Rectangle(base.Width - 24, 0, 24, H));
            g.DrawString("+", new Font("Segoe UI", 12f), new SolidBrush(ColorSystem.FontColorButtons), new Point(base.Width - 12, 8), Helpers.CenterSF);
            g.DrawString("-", new Font("Segoe UI", 10f, FontStyle.Bold), new SolidBrush(ColorSystem.FontColorButtons), new Point(base.Width - 12, 22), Helpers.CenterSF);
            g.DrawString(Conversions.ToString(Value), Font, new SolidBrush(ColorSystem.FontColorButtons), new Rectangle(5, 1, W, H), new StringFormat
            {
                LineAlignment = StringAlignment.Center
            });
            base.OnPaint(e);
            Helpers.G.Dispose();
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            e.Graphics.DrawImageUnscaled(Helpers.B, 0, 0);
            Helpers.B.Dispose();
        }
    }
}

