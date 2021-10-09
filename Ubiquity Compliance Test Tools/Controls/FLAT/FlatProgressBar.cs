
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
internal class FlatProgressBar : Control
{
    private int W;

    private int H;

    private int _Value;

    private int _Maximum;

    private Color _BaseColor;

    private Color _ProgressColor;

    private Color _DarkerProgress;

    [Category("Control")]
    public int Maximum
    {
        get
        {
            return _Maximum;
        }
        set
        {
            if (value < _Value)
            {
                _Value = value;
            }
            _Maximum = value;
            Invalidate();
        }
    }

    [Category("Control")]
    public int Value
    {
        get
        {
            if (_Value == 0)
            {
                return 0;
            }
            return _Value;
        }
        set
        {
            if (value > _Maximum)
            {
                value = _Maximum;
                Invalidate();
            }
            _Value = value;
            Invalidate();
        }
    }

    [Category("Colors")]
    public Color ProgressColor
    {
        get
        {
            return _ProgressColor;
        }
        set
        {
            _ProgressColor = value;
        }
    }

    [Category("Colors")]
    public Color DarkerProgress
    {
        get
        {
            return _DarkerProgress;
        }
        set
        {
            _DarkerProgress = value;
        }
    }

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        base.Height = 42;
    }

    protected override void CreateHandle()
    {
        base.CreateHandle();
        base.Height = 42;
    }

    public void Increment(int Amount)
    {
        checked
        {
            Value += Amount;
        }
    }

    public FlatProgressBar()
    {
        _Value = 0;
        _Maximum = 100;
        _BaseColor = Color.FromArgb(45, 47, 49);
        _ProgressColor = Helpers._FlatColor;
        _DarkerProgress = Color.FromArgb(23, 148, 92);
        SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, value: true);
        DoubleBuffered = true;
        BackColor = Color.FromArgb(60, 70, 73);
        base.Height = 42;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        Helpers.B = new Bitmap(base.Width, base.Height);
        Helpers.G = Graphics.FromImage(Helpers.B);
        checked
        {
            W = base.Width - 1;
            H = base.Height - 1;
            Rectangle rect = new Rectangle(0, 24, W, H);
            GraphicsPath graphicsPath = new GraphicsPath();
            GraphicsPath graphicsPath2 = new GraphicsPath();
            GraphicsPath graphicsPath3 = new GraphicsPath();
            Graphics g = Helpers.G;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            g.Clear(ColorSystem.FontColorLabels);
            int num = (int)Math.Round(unchecked((double)_Value / (double)_Maximum * (double)base.Width));
            switch (Value)
            {
                case 0:
                    g.FillRectangle(new SolidBrush(_BaseColor), rect);
                    g.FillRectangle(new SolidBrush(_ProgressColor), new Rectangle(0, 24, num - 1, H - 1));
                    break;
                case 100:
                    g.FillRectangle(new SolidBrush(_BaseColor), rect);
                    g.FillRectangle(new SolidBrush(_ProgressColor), new Rectangle(0, 24, num - 1, H - 1));
                    break;
                default:
                    {
                        g.FillRectangle(new SolidBrush(_BaseColor), rect);
                        graphicsPath.AddRectangle(new Rectangle(0, 24, num - 1, H - 1));
                        g.FillPath(new SolidBrush(_ProgressColor), graphicsPath);
                        HatchBrush brush = new HatchBrush(HatchStyle.Plaid, _DarkerProgress, _ProgressColor);
                        g.FillRectangle(brush, new Rectangle(0, 24, num - 1, H - 1));
                        graphicsPath2 = Helpers.RoundRec(new Rectangle(num - 18, 0, 34, 16), 4);
                        g.FillPath(new SolidBrush(_BaseColor), graphicsPath2);
                        graphicsPath3 = Helpers.DrawArrow(num - 9, 16, flip: true);
                        g.FillPath(new SolidBrush(_BaseColor), graphicsPath3);
                        g.DrawString(Conversions.ToString(Value), new Font("Segoe UI", 10f), new SolidBrush(_ProgressColor), new Rectangle(num - 11, -2, W, H), Helpers.NearSF);
                        break;
                    }
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
