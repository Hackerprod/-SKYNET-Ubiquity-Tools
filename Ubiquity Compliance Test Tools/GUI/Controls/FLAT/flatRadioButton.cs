
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;


internal class FlatRadioButton : Control
{
    public delegate void CheckedChangedEventHandler(object sender);

    [Flags]
    public enum _Options
    {
        Style1 = 0x0,
        Style2 = 0x1
    }

    private MouseState State;

    private int W;

    private int H;

    private _Options O;

    private bool _Checked;

    [CompilerGenerated]
    private CheckedChangedEventHandler CheckedChangedEvent;

    public bool Checked
    {
        get
        {
            return _Checked;
        }
        set
        {
            _Checked = value;
            InvalidateControls();
            CheckedChangedEvent?.Invoke(this);
            Invalidate();
        }
    }

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

    protected override void OnClick(EventArgs e)
    {
        if (!_Checked)
        {
            Checked = true;
        }
        base.OnClick(e);
    }

    private void InvalidateControls()
    {
        if (base.IsHandleCreated && _Checked)
        {
            IEnumerator enumerator = default(IEnumerator);
            try
            {
                enumerator = base.Parent.Controls.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    Control control = (Control)enumerator.Current;
                    if (control != this && control is RadioButton)
                    {
                        ((RadioButton)control).Checked = false;
                        Invalidate();
                    }
                }
            }
            finally
            {
                if (enumerator is IDisposable)
                {
                    (enumerator as IDisposable).Dispose();
                }
            }
        }
    }

    protected override void OnCreateControl()
    {
        base.OnCreateControl();
        InvalidateControls();
    }

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        base.Height = 22;
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

    public FlatRadioButton()
    {
        State = MouseState.None;
        SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, value: true);
        DoubleBuffered = true;
        Cursor = Cursors.Hand;
        base.Size = new Size(100, 22);
        BackColor = BackColor;
        Font = new Font("Segoe UI", 10f);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        Helpers.B = new Bitmap(base.Width, base.Height);
        Helpers.G = Graphics.FromImage(Helpers.B);
        checked
        {
            W = base.Width - 1;
            H = base.Height - 1;
            Rectangle rect = new Rectangle(0, 2, base.Height - 5, base.Height - 5);
            Rectangle rect2 = new Rectangle(4, 6, H - 12, H - 12);
            Graphics g = Helpers.G;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            g.Clear(ColorSystem.FontColorLabels);
            switch (O)
            {
                case _Options.Style1:
                    g.FillEllipse(new SolidBrush(ColorSystem.BackDark), rect);
                    switch (State)
                    {
                        case MouseState.Over:
                            g.DrawEllipse(new Pen(ColorSystem.FlatColor), rect);
                            break;
                        case MouseState.Down:
                            g.DrawEllipse(new Pen(ColorSystem.FlatColor), rect);
                            break;
                    }
                    if (Checked)
                    {
                        g.FillEllipse(new SolidBrush(ColorSystem.FlatColor), rect2);
                    }
                    break;
                case _Options.Style2:
                    g.FillEllipse(new SolidBrush(ColorSystem.BackDark), rect);
                    switch (State)
                    {
                        case MouseState.Over:
                            g.DrawEllipse(new Pen(ColorSystem.FlatColor), rect);
                            g.FillEllipse(new SolidBrush(Color.FromArgb(118, 213, 170)), rect);
                            break;
                        case MouseState.Down:
                            g.DrawEllipse(new Pen(ColorSystem.FlatColor), rect);
                            g.FillEllipse(new SolidBrush(Color.FromArgb(118, 213, 170)), rect);
                            break;
                    }
                    if (Checked)
                    {
                        g.FillEllipse(new SolidBrush(ColorSystem.FlatColor), rect2);
                    }
                    break;
            }
            g.DrawString(Text, Font, new SolidBrush(ColorSystem.FontColorButtons), new Rectangle(20, 2, W, H), Helpers.NearSF);
            g = null;
            base.OnPaint(e);
            Helpers.G.Dispose();
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            e.Graphics.DrawImageUnscaled(Helpers.B, 0, 0);
            Helpers.B.Dispose();
        }
    }
}
