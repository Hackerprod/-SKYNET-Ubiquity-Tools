
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
internal class FlatPanel : ContainerControl
{
    private int W;

    private int H;

    private bool _Hameleon;

    [Category("Appearance")]
    public bool Hameleon
    {
        get
        {
            return _Hameleon;
        }
        set
        {
            _Hameleon = value;
        }
    }

    public FlatPanel()
    {
        _Hameleon = true;
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
            Rectangle rect = new Rectangle(0, 0, W, H);
            Graphics g = Helpers.G;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            g.Clear(Color.Transparent);
            if (Hameleon)
            {
                g.FillRectangle(new SolidBrush(Color.Transparent), rect);
            }
            else
            {
                g.FillRectangle(new SolidBrush(ColorSystem.AllBackColor), rect);
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