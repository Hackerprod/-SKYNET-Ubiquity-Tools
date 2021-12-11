using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

internal class FlatLabel : Control
{
    [Flags]
    public enum TxtAlign
    {
        Left = 0x0,
        Center = 0x1,
        Right = 0x2
    }

    private TxtAlign _TextAlignment;

    [Category("Appearance")]
    public TxtAlign TextAligment
    {
        get
        {
            return _TextAlignment;
        }
        set
        {
            _TextAlignment = value;
        }
    }

    protected override void OnTextChanged(EventArgs e)
    {
        base.OnTextChanged(e);
        Invalidate();
    }

    public FlatLabel()
    {
        _TextAlignment = TxtAlign.Left;
        SetStyle(ControlStyles.SupportsTransparentBackColor, value: true);
        base.Size = new Size(80, 18);
        Font = new Font("Segoe UI", 9.75f);
        Text = Text;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        Helpers.B = new Bitmap(base.Width, base.Height);
        Helpers.G = Graphics.FromImage(Helpers.B);
        new GraphicsPath();
        Rectangle r = checked(new Rectangle(0, 0, base.Width - 1, base.Height - 1));
        Graphics g = Helpers.G;
        g.SmoothingMode = SmoothingMode.HighQuality;
        g.TextRenderingHint = TextRenderingHint.AntiAlias;
        g.Clear(Color.Transparent);
        if (_TextAlignment == TxtAlign.Center)
        {
            g.DrawString(Text, Font, new SolidBrush(ColorSystem.FontColorLabels), r, Helpers.CenterSF);
        }
        else if (_TextAlignment == TxtAlign.Left)
        {
            g.DrawString(Text, Font, new SolidBrush(ColorSystem.FontColorLabels), r, Helpers.NearSF);
        }
        else if (_TextAlignment == TxtAlign.Right)
        {
            g.DrawString(Text, Font, new SolidBrush(ColorSystem.FontColorLabels), r, Helpers.FarSF);
        }
        g = null;
        base.OnPaint(e);
        Helpers.G.Dispose();
        e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
        e.Graphics.DrawImageUnscaled(Helpers.B, 0, 0);
        Helpers.B.Dispose();
    }
}
