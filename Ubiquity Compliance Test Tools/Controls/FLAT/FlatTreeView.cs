
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
internal class FlatTreeView : TreeView
{
    private TreeNodeStates State;

    private Color _BaseColor;

    private Color _LineColor;

    protected override void OnDrawNode(DrawTreeNodeEventArgs e)
    {
        checked
        {
            try
            {
                Rectangle rect = new Rectangle(e.Bounds.Location.X, e.Bounds.Location.Y, e.Bounds.Width, e.Bounds.Height);
                switch (State)
                {
                    case TreeNodeStates.Default:
                        e.Graphics.FillRectangle(Brushes.Red, rect);
                        e.Graphics.DrawString(e.Node.Text, new Font("Segoe UI", 8f), Brushes.LimeGreen, new Rectangle(rect.X + 2, rect.Y + 2, rect.Width, rect.Height), Helpers.NearSF);
                        Invalidate();
                        break;
                    case TreeNodeStates.Checked:
                        e.Graphics.FillRectangle(Brushes.Green, rect);
                        e.Graphics.DrawString(e.Node.Text, new Font("Segoe UI", 8f), Brushes.Black, new Rectangle(rect.X + 2, rect.Y + 2, rect.Width, rect.Height), Helpers.NearSF);
                        Invalidate();
                        break;
                    case TreeNodeStates.Selected:
                        e.Graphics.FillRectangle(Brushes.Green, rect);
                        e.Graphics.DrawString(e.Node.Text, new Font("Segoe UI", 8f), Brushes.Black, new Rectangle(rect.X + 2, rect.Y + 2, rect.Width, rect.Height), Helpers.NearSF);
                        Invalidate();
                        break;
                }
            }
            catch (Exception ex)
            {
                ProjectData.SetProjectError(ex);
                Exception ex2 = ex;
                ProjectData.ClearProjectError();
            }
            base.OnDrawNode(e);
        }
    }

    public FlatTreeView()
    {
        _BaseColor = Color.FromArgb(45, 47, 49);
        _LineColor = Color.FromArgb(25, 27, 29);
        SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, value: true);
        DoubleBuffered = true;
        BackColor = _BaseColor;
        ForeColor = Color.White;
        base.LineColor = _LineColor;
        base.DrawMode = TreeViewDrawMode.OwnerDrawAll;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        Helpers.B = new Bitmap(base.Width, base.Height);
        Helpers.G = Graphics.FromImage(Helpers.B);
        Rectangle rect = new Rectangle(0, 0, base.Width, base.Height);
        Graphics g = Helpers.G;
        g.SmoothingMode = SmoothingMode.HighQuality;
        g.PixelOffsetMode = PixelOffsetMode.HighQuality;
        g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
        g.Clear(BackColor);
        g.FillRectangle(new SolidBrush(_BaseColor), rect);
        g.DrawString(Text, new Font("Segoe UI", 8f), Brushes.Black, checked(new Rectangle(base.Bounds.X + 2, base.Bounds.Y + 2, base.Bounds.Width, base.Bounds.Height)), Helpers.NearSF);
        base.OnPaint(e);
        Helpers.G.Dispose();
        e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
        e.Graphics.DrawImageUnscaled(Helpers.B, 0, 0);
        Helpers.B.Dispose();
    }
}
