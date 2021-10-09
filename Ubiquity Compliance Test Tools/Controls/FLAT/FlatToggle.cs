using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
internal class FlatToggle : Control
{
    public delegate void CheckedChangedEventHandler(object sender);

    [Flags]
    public enum _Options
    {
        Style1 = 0x0,
        Style2 = 0x1,
        Style3 = 0x2,
        Style4 = 0x3,
        Style5 = 0x4
    }

    private int W;

    private int H;

    private _Options O;

    private bool _Checked;

    [CompilerGenerated]
    private CheckedChangedEventHandler CheckedChangedEvent;

    private Color BaseColor;

    private Color BaseColorRed;

    private Color BGColor;

    private Color ToggleColor;

    private Color TextColor;

    [Category("Options")]
    public _Options Options
    {
        get
        {
            return O;
        }
        set
        {
            O = value;
        }
    }

    [Category("Options")]
    public bool Checked
    {
        get
        {
            return _Checked;
        }
        set
        {
            _Checked = value;
        }
    }

    public event CheckedChangedEventHandler CheckedChanged
    {
        [CompilerGenerated]
        add
        {
            CheckedChangedEventHandler checkedChangedEventHandler = CheckedChangedEvent;
            CheckedChangedEventHandler checkedChangedEventHandler2;
            do
            {
                checkedChangedEventHandler2 = checkedChangedEventHandler;
                CheckedChangedEventHandler value2 = (CheckedChangedEventHandler)Delegate.Combine(checkedChangedEventHandler2, value);
                checkedChangedEventHandler = Interlocked.CompareExchange(ref CheckedChangedEvent, value2, checkedChangedEventHandler2);
            }
            while ((object)checkedChangedEventHandler != checkedChangedEventHandler2);
        }
        [CompilerGenerated]
        remove
        {
            CheckedChangedEventHandler checkedChangedEventHandler = CheckedChangedEvent;
            CheckedChangedEventHandler checkedChangedEventHandler2;
            do
            {
                checkedChangedEventHandler2 = checkedChangedEventHandler;
                CheckedChangedEventHandler value2 = (CheckedChangedEventHandler)Delegate.Remove(checkedChangedEventHandler2, value);
                checkedChangedEventHandler = Interlocked.CompareExchange(ref CheckedChangedEvent, value2, checkedChangedEventHandler2);
            }
            while ((object)checkedChangedEventHandler != checkedChangedEventHandler2);
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
        base.Width = 76;
        base.Height = 33;
    }

    protected override void OnMouseEnter(EventArgs e)
    {
        base.OnMouseEnter(e);
        Invalidate();
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        base.OnMouseDown(e);
        Invalidate();
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        base.OnMouseLeave(e);
        Invalidate();
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        base.OnMouseUp(e);
        Invalidate();
    }

    protected override void OnClick(EventArgs e)
    {
        base.OnClick(e);
        _Checked = !_Checked;
        CheckedChangedEvent?.Invoke(this);
    }

    public FlatToggle()
    {
        _Checked = false;
        BaseColor = Helpers._FlatColor;
        BaseColorRed = Color.FromArgb(220, 85, 96);
        BGColor = Color.FromArgb(84, 85, 86);
        ToggleColor = Color.FromArgb(45, 47, 49);
        TextColor = Color.FromArgb(243, 243, 243);
        SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, value: true);
        DoubleBuffered = true;
        BackColor = Color.Transparent;
        base.Size = new Size(44, checked(base.Height + 1));
        Cursor = Cursors.Hand;
        Font = new Font("Segoe UI", 10f);
        base.Size = new Size(76, 33);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        Helpers.B = new Bitmap(base.Width, base.Height);
        Helpers.G = Graphics.FromImage(Helpers.B);
        checked
        {
            W = base.Width - 1;
            H = base.Height - 1;
            GraphicsPath graphicsPath = new GraphicsPath();
            GraphicsPath graphicsPath2 = new GraphicsPath();
            Rectangle rectangle = new Rectangle(0, 0, W, H);
            Rectangle rectangle2 = new Rectangle(unchecked(W / 2), 0, 38, H);
            Graphics g = Helpers.G;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            g.Clear(ColorSystem.FontColorLabels);
            switch (O)
            {
                case _Options.Style1:
                    graphicsPath = Helpers.RoundRec(rectangle, 6);
                    graphicsPath2 = Helpers.RoundRec(rectangle2, 6);
                    g.FillPath(new SolidBrush(BGColor), graphicsPath);
                    g.FillPath(new SolidBrush(ToggleColor), graphicsPath2);
                    g.DrawString("OFF", Font, new SolidBrush(BGColor), new Rectangle(19, 1, W, H), Helpers.CenterSF);
                    if (Checked)
                    {
                        graphicsPath = Helpers.RoundRec(rectangle, 6);
                        graphicsPath2 = Helpers.RoundRec(new Rectangle(unchecked(W / 2), 0, 38, H), 6);
                        g.FillPath(new SolidBrush(ToggleColor), graphicsPath);
                        g.FillPath(new SolidBrush(BaseColor), graphicsPath2);
                        g.DrawString("ON", Font, new SolidBrush(BaseColor), new Rectangle(8, 7, W, H), Helpers.NearSF);
                    }
                    break;
                case _Options.Style2:
                    graphicsPath = Helpers.RoundRec(rectangle, 6);
                    rectangle2 = new Rectangle(4, 4, 36, H - 8);
                    graphicsPath2 = Helpers.RoundRec(rectangle2, 4);
                    g.FillPath(new SolidBrush(BaseColorRed), graphicsPath);
                    g.FillPath(new SolidBrush(ToggleColor), graphicsPath2);
                    g.DrawLine(new Pen(BGColor), 18, 20, 18, 12);
                    g.DrawLine(new Pen(BGColor), 22, 20, 22, 12);
                    g.DrawLine(new Pen(BGColor), 26, 20, 26, 12);
                    g.DrawString("r", new Font("Marlett", 8f), new SolidBrush(TextColor), new Rectangle(19, 2, base.Width, base.Height), Helpers.CenterSF);
                    if (Checked)
                    {
                        graphicsPath = Helpers.RoundRec(rectangle, 6);
                        rectangle2 = new Rectangle(unchecked(W / 2) - 2, 4, 36, H - 8);
                        graphicsPath2 = Helpers.RoundRec(rectangle2, 4);
                        g.FillPath(new SolidBrush(BaseColor), graphicsPath);
                        g.FillPath(new SolidBrush(ToggleColor), graphicsPath2);
                        g.DrawLine(new Pen(BGColor), unchecked(W / 2) + 12, 20, unchecked(W / 2) + 12, 12);
                        g.DrawLine(new Pen(BGColor), unchecked(W / 2) + 16, 20, unchecked(W / 2) + 16, 12);
                        g.DrawLine(new Pen(BGColor), unchecked(W / 2) + 20, 20, unchecked(W / 2) + 20, 12);
                        g.DrawString("ü", new Font("Wingdings", 14f), new SolidBrush(TextColor), new Rectangle(8, 7, base.Width, base.Height), Helpers.NearSF);
                    }
                    break;
                case _Options.Style3:
                    graphicsPath = Helpers.RoundRec(rectangle, 16);
                    rectangle2 = new Rectangle(W - 28, 4, 22, H - 8);
                    graphicsPath2.AddEllipse(rectangle2);
                    g.FillPath(new SolidBrush(ToggleColor), graphicsPath);
                    g.FillPath(new SolidBrush(BaseColorRed), graphicsPath2);
                    g.DrawString("OFF", Font, new SolidBrush(BaseColorRed), new Rectangle(-12, 2, W, H), Helpers.CenterSF);
                    if (Checked)
                    {
                        graphicsPath = Helpers.RoundRec(rectangle, 16);
                        rectangle2 = new Rectangle(6, 4, 22, H - 8);
                        graphicsPath2.Reset();
                        graphicsPath2.AddEllipse(rectangle2);
                        g.FillPath(new SolidBrush(ToggleColor), graphicsPath);
                        g.FillPath(new SolidBrush(BaseColor), graphicsPath2);
                        g.DrawString("ON", Font, new SolidBrush(BaseColor), new Rectangle(12, 2, W, H), Helpers.CenterSF);
                    }
                    break;
                case _Options.Style4:
                    if (!Checked)
                    {
                    }
                    break;
                case _Options.Style5:
                    {
                        bool @checked = Checked;
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
