using Microsoft.VisualBasic.CompilerServices;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;

internal class FlatTrackBar : Control
{
    [Flags]
    public enum _Style
    {
        Slider = 0x0,
        Knob = 0x1
    }

    public delegate void ScrollEventHandler(object sender);

    private int W;

    private int H;

    private int Val;

    private bool Bool;

    private Rectangle Track;

    private Rectangle Knob;

    private _Style Style_;

    [CompilerGenerated]
    private ScrollEventHandler ScrollEvent;

    private int _Minimum;

    private int _Maximum;

    private int _Value;

    private bool _ShowValue;

    private Color BaseColor;

    private Color _TrackColor;

    private Color SliderColor;

    private Color _HatchColor;

    public _Style Style
    {
        get
        {
            return Style_;
        }
        set
        {
            Style_ = value;
        }
    }

    [Category("Colors")]
    public Color TrackColor
    {
        get
        {
            return _TrackColor;
        }
        set
        {
            _TrackColor = value;
        }
    }

    [Category("Colors")]
    public Color HatchColor
    {
        get
        {
            return _HatchColor;
        }
        set
        {
            _HatchColor = value;
        }
    }

    public int Minimum
    {
        get
        {
            int result = default(int);
            return result;
        }
        set
        {
            _Minimum = value;
            if (value > _Value)
            {
                _Value = value;
            }
            if (value > _Maximum)
            {
                _Maximum = value;
            }
            Invalidate();
        }
    }

    public int Maximum
    {
        get
        {
            return _Maximum;
        }
        set
        {
            _Maximum = value;
            if (value < _Value)
            {
                _Value = value;
            }
            if (value < _Minimum)
            {
                _Minimum = value;
            }
            Invalidate();
        }
    }

    public int Value
    {
        get
        {
            return _Value;
        }
        set
        {
            if (value != _Value)
            {
                if (value <= _Maximum)
                {
                    int minimum = _Minimum;
                }
                _Value = value;
                Invalidate();
                ScrollEvent?.Invoke(this);
            }
        }
    }

    public bool ShowValue
    {
        get
        {
            return _ShowValue;
        }
        set
        {
            _ShowValue = value;
        }
    }

    public event ScrollEventHandler Scroll
    {
        [CompilerGenerated]
        add
        {
            ScrollEventHandler scrollEventHandler = ScrollEvent;
            ScrollEventHandler scrollEventHandler2;
            do
            {
                scrollEventHandler2 = scrollEventHandler;
                ScrollEventHandler value2 = (ScrollEventHandler)Delegate.Combine(scrollEventHandler2, value);
                scrollEventHandler = Interlocked.CompareExchange(ref ScrollEvent, value2, scrollEventHandler2);
            }
            while ((object)scrollEventHandler != scrollEventHandler2);
        }
        [CompilerGenerated]
        remove
        {
            ScrollEventHandler scrollEventHandler = ScrollEvent;
            ScrollEventHandler scrollEventHandler2;
            do
            {
                scrollEventHandler2 = scrollEventHandler;
                ScrollEventHandler value2 = (ScrollEventHandler)Delegate.Remove(scrollEventHandler2, value);
                scrollEventHandler = Interlocked.CompareExchange(ref ScrollEvent, value2, scrollEventHandler2);
            }
            while ((object)scrollEventHandler != scrollEventHandler2);
        }
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        base.OnMouseDown(e);
        checked
        {
            if (e.Button == MouseButtons.Left)
            {
                Val = (int)Math.Round(unchecked((double)checked(_Value - _Minimum) / (double)checked(_Maximum - _Minimum) * (double)checked(base.Width - 11)));
                Track = new Rectangle(Val, 0, 10, 20);
                Bool = Track.Contains(e.Location);
            }
        }
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        checked
        {
            if (Bool && e.X > -1 && e.X < base.Width + 1)
            {
                Value = _Minimum + (int)Math.Round(unchecked((double)checked(_Maximum - _Minimum) * ((double)e.X / (double)base.Width)));
            }
        }
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        base.OnMouseUp(e);
        Bool = false;
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);
        checked
        {
            if (e.KeyCode == Keys.Subtract)
            {
                if (Value != 0)
                {
                    Value--;
                }
            }
            else if (e.KeyCode == Keys.Add && Value != _Maximum)
            {
                Value++;
            }
        }
    }

    protected override void OnTextChanged(EventArgs e)
    {
        base.OnTextChanged(e);
        Invalidate();
    }

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        base.Height = 23;
    }

    public FlatTrackBar()
    {
        _Maximum = 10;
        _ShowValue = false;
        BaseColor = Color.FromArgb(45, 47, 49);
        _TrackColor = Helpers._FlatColor;
        SliderColor = Color.FromArgb(25, 27, 29);
        _HatchColor = Color.FromArgb(23, 148, 92);
        SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, value: true);
        DoubleBuffered = true;
        base.Height = 18;
        BackColor = Color.FromArgb(60, 70, 73);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        Helpers.B = new Bitmap(base.Width, base.Height);
        Helpers.G = Graphics.FromImage(Helpers.B);
        checked
        {
            W = base.Width - 1;
            H = base.Height - 1;
            Rectangle rect = new Rectangle(1, 6, W - 2, 8);
            GraphicsPath graphicsPath = new GraphicsPath();
            GraphicsPath graphicsPath2 = new GraphicsPath();
            Graphics g = Helpers.G;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            g.Clear(BackColor);
            Val = (int)Math.Round(unchecked((double)checked(_Value - _Minimum) / (double)checked(_Maximum - _Minimum) * (double)checked(W - 10)));
            Track = new Rectangle(Val, 0, 10, 20);
            Knob = new Rectangle(Val, 4, 11, 14);
            graphicsPath.AddRectangle(rect);
            g.SetClip(graphicsPath);
            g.FillRectangle(new SolidBrush(BaseColor), new Rectangle(0, 7, W, 8));
            g.FillRectangle(new SolidBrush(_TrackColor), new Rectangle(0, 7, Track.X + Track.Width, 8));
            g.ResetClip();
            HatchBrush brush = new HatchBrush(HatchStyle.Plaid, HatchColor, _TrackColor);
            g.FillRectangle(brush, new Rectangle(-10, 7, Track.X + Track.Width, 8));
            switch (Style)
            {
                case _Style.Slider:
                    graphicsPath2.AddRectangle(Track);
                    g.FillPath(new SolidBrush(SliderColor), graphicsPath2);
                    break;
                case _Style.Knob:
                    graphicsPath2.AddEllipse(Knob);
                    g.FillPath(new SolidBrush(SliderColor), graphicsPath2);
                    break;
            }
            if (ShowValue)
            {
                g.DrawString(Conversions.ToString(Value), new Font("Segoe UI", 8f), Brushes.White, new Rectangle(1, 6, W, H), new StringFormat
                {
                    Alignment = StringAlignment.Far,
                    LineAlignment = StringAlignment.Far
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
}