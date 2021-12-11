using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

internal class FlatGroupBox : ContainerControl
{
    private int W;

    private int H;

    private bool _ShowText;

    private Color _BaseColor;

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

    public bool ShowText
    {
        get
        {
            return _ShowText;
        }
        set
        {
            _ShowText = value;
        }
    }

    public FlatGroupBox()
    {
        _ShowText = true;
        _BaseColor = Color.FromArgb(60, 70, 73);
        SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, value: true);
        DoubleBuffered = true;
        BackColor = Color.Transparent;
        base.Size = new Size(240, 180);
        Font = new Font("Segoe ui", 10f);
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
            GraphicsPath graphicsPath3 = new GraphicsPath();
            Rectangle rectangle = new Rectangle(8, 8, W - 16, H - 16);
            Graphics g = Helpers.G;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            g.Clear(ColorSystem.FontColorLabels);
            graphicsPath = Helpers.RoundRec(rectangle, 8);
            g.FillPath(new SolidBrush(_BaseColor), graphicsPath);
            graphicsPath2 = Helpers.DrawArrow(28, 2, flip: false);
            g.FillPath(new SolidBrush(_BaseColor), graphicsPath2);
            graphicsPath3 = Helpers.DrawArrow(28, 8, flip: true);
            g.FillPath(new SolidBrush(Color.FromArgb(60, 70, 73)), graphicsPath3);
            if (ShowText)
            {
                g.DrawString(Text, Font, new SolidBrush(Helpers._FlatColor), new Rectangle(16, 16, W, H), Helpers.NearSF);
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
