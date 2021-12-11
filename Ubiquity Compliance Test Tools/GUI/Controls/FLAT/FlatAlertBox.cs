using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

public class FlatAlertBox : Control
{
    [Flags]
    public enum _Kind
    {
        Success = 0x0,
        Error = 0x1,
        Info = 0x2,
        Personalizado = 0x3
    }

    private int W;

    private int H;

    private _Kind K;

    private string _Text;

    private MouseState State;

    [CompilerGenerated]
    [AccessedThroughProperty("T")]
    private Timer _T;

    private Color SuccessColor;

    private Color SuccessText;

    private Color PersonalizadoColor;

    private Color PersonalizadoText;

    private Color ErrorColor;

    private Color ErrorText;

    private Color InfoColor;

    private Color InfoText;

    public virtual Timer T
    {
        [CompilerGenerated]
        get
        {
            return _T;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        [CompilerGenerated]
        set
        {
            EventHandler value2 = T_Tick;
            Timer t = _T;
            if (t != null)
            {
                t.Tick -= value2;
            }
            _T = value;
            t = _T;
            if (t != null)
            {
                t.Tick += value2;
            }
        }
    }

    [Category("Options")]
    public _Kind kind
    {
        get
        {
            return K;
        }
        set
        {
            K = value;
        }
    }

    [Category("Options")]
    public override string Text
    {
        get
        {
            return base.Text;
        }
        set
        {
            base.Text = value;
            if (_Text != null)
            {
                _Text = value;
            }
        }
    }

    [Category("Options")]
    public new bool Visible
    {
        get
        {
            return !base.Visible;
        }
        set
        {
            base.Visible = value;
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
        base.Height = 42;
    }

    public void ShowControl(_Kind Kind, string Str, int Interval)
    {
        K = Kind;
        Text = Str;
        Visible = true;
        T = new Timer();
        T.Interval = Interval;
        T.Enabled = true;
    }

    private void T_Tick(object sender, EventArgs e)
    {
        Visible = false;
        T.Enabled = false;
        T.Dispose();
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

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        Invalidate();
    }

    protected override void OnClick(EventArgs e)
    {
        base.OnClick(e);
        Visible = false;
    }

    public FlatAlertBox()
    {
        State = MouseState.None;

        SuccessColor = Color.FromArgb(60, 85, 79);
        SuccessText = Color.FromArgb(35, 169, 110);

        PersonalizadoColor = Color.FromArgb(43, 54, 68); //Color del Panel  
        PersonalizadoText = Color.FromArgb(7, 164, 245); //Color del texto

        ErrorColor = Color.FromArgb(43, 54, 68);
        ErrorText = Color.Red;

        InfoColor = Color.FromArgb(70, 91, 94);
        InfoText = Color.FromArgb(97, 185, 186);

        SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, value: true);
        DoubleBuffered = true;
        base.Size = new Size(576, 42);
        base.Location = new Point(10, 61);
        Font = new Font("Segoe UI", 10f);
        Cursor = Cursors.Hand;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        Helpers.B = new Bitmap(base.Width, base.Height);
        Helpers.G = Graphics.FromImage(Helpers.B);
        checked
        {
            W = base.Width - 1;
            H = base.Height - 1;
            Rectangle rect = new Rectangle(0, 0, W, H);
            Graphics g = Helpers.G;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            switch (K)
            {
                case _Kind.Success:
                    {
                        g.FillRectangle(new SolidBrush(SuccessColor), rect);
                        g.FillEllipse(new SolidBrush(SuccessText), new Rectangle(8, 9, 24, 24));
                        g.FillEllipse(new SolidBrush(SuccessColor), new Rectangle(10, 11, 20, 20));
                        g.DrawString("ü", new Font("Wingdings", 22f), new SolidBrush(SuccessText), new Rectangle(7, 7, W, H), Helpers.NearSF);
                        g.DrawString(Text, Font, new SolidBrush(SuccessText), new Rectangle(48, 12, W, H), Helpers.NearSF);
                        g.FillEllipse(new SolidBrush(Color.FromArgb(35, Color.Black)), new Rectangle(W - 30, H - 29, 17, 17));
                        g.DrawString("r", new Font("Marlett", 8f), new SolidBrush(SuccessColor), new Rectangle(W - 28, 16, W, H), Helpers.NearSF);
                        MouseState state2 = State;
                        if (state2 == MouseState.Over)
                        {
                            g.DrawString("r", new Font("Marlett", 8f), new SolidBrush(Color.FromArgb(25, Color.White)), new Rectangle(W - 28, 16, W, H), Helpers.NearSF);
                        }
                        break;
                    }
                case _Kind.Error:
                    {
                        g.FillRectangle(new SolidBrush(ErrorColor), rect);
                        g.FillEllipse(new SolidBrush(ErrorText), new Rectangle(8, 9, 24, 24));
                        g.FillEllipse(new SolidBrush(ErrorColor), new Rectangle(10, 11, 20, 20));
                        g.DrawString("r", new Font("Marlett", 16f), new SolidBrush(ErrorText), new Rectangle(6, 11, W, H), Helpers.NearSF);
                        g.DrawString(Text, Font, new SolidBrush(ErrorText), new Rectangle(48, 12, W, H), Helpers.NearSF);
                        g.FillEllipse(new SolidBrush(Color.Red), new Rectangle(W - 32, H - 29, 17, 17));
                        g.DrawString("r", new Font("Marlett", 8f), new SolidBrush(ErrorColor), new Rectangle(W - 30, 17, W, H), Helpers.NearSF);
                        MouseState state3 = State;
                        if (state3 == MouseState.Over)
                        {
                            g.DrawString("r", new Font("Marlett", 8f), new SolidBrush(Color.FromArgb(25, Color.White)), new Rectangle(W - 30, 15, W, H), Helpers.NearSF);
                        }
                        break;
                    }
                case _Kind.Info:
                    {
                        g.FillRectangle(new SolidBrush(InfoColor), rect);
                        g.FillEllipse(new SolidBrush(InfoText), new Rectangle(8, 9, 24, 24));
                        g.FillEllipse(new SolidBrush(InfoColor), new Rectangle(10, 11, 20, 20));
                        g.DrawString("¡", new Font("Segoe UI", 20f, FontStyle.Bold), new SolidBrush(InfoText), new Rectangle(12, -4, W, H), Helpers.NearSF);
                        g.DrawString(Text, Font, new SolidBrush(InfoText), new Rectangle(48, 12, W, H), Helpers.NearSF);
                        g.FillEllipse(new SolidBrush(Color.FromArgb(35, Color.Black)), new Rectangle(W - 32, H - 29, 17, 17));
                        g.DrawString("r", new Font("Marlett", 8f), new SolidBrush(InfoColor), new Rectangle(W - 30, 17, W, H), Helpers.NearSF);
                        MouseState state = State;
                        if (state == MouseState.Over)
                        {
                            g.DrawString("r", new Font("Marlett", 8f), new SolidBrush(Color.FromArgb(25, Color.White)), new Rectangle(W - 30, 17, W, H), Helpers.NearSF);
                        }
                        break;
                    }
                case _Kind.Personalizado:
                    {
                        g.FillRectangle(new SolidBrush(PersonalizadoColor), rect); //Color del panel
                        g.FillEllipse(new SolidBrush(PersonalizadoText), new Rectangle(8, 9, 24, 24)); //Color del icono
                        g.FillEllipse(new SolidBrush(PersonalizadoColor), new Rectangle(10, 11, 20, 20)); //Color del icono
                        g.DrawString("ü", new Font("Wingdings", 22f), new SolidBrush(PersonalizadoText), new Rectangle(7, 7, W, H), Helpers.NearSF); //Color del icono
                        g.DrawString(Text, Font, new SolidBrush(PersonalizadoText), new Rectangle(48, 12, W, H), Helpers.NearSF); //Color del Texto
                        g.FillEllipse(new SolidBrush(Color.FromArgb(53, 64, 78)), new Rectangle(W - 30, H - 29, 17, 17)); //Circulito de cerrar
                        g.DrawString("r", new Font("Marlett", 8f), new SolidBrush(PersonalizadoColor), new Rectangle(W - 28, 16, W, H), Helpers.NearSF); //Crucecita de cerrar
                        MouseState state2 = State;
                        if (state2 == MouseState.Over)
                        {
                            g.FillEllipse(new SolidBrush(Color.FromArgb(77, 82, 83)), new Rectangle(W - 30, H - 29, 17, 17)); //Circulito de cerrar
                            g.DrawString("r", new Font("Marlett", 8f), new SolidBrush(Color.FromArgb(53, 64, 78)), new Rectangle(W - 28, 16, W, H), Helpers.NearSF); //Crucecita de cerrar
                        }
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

