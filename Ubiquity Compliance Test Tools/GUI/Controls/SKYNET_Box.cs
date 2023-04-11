using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace SKYNET.Controls
{
    [DefaultEvent("BoxClicked")]
    public partial class SKYNET_Box : UserControl
    {
        private Color _Color;
        private Color _FocusedColor;
        private bool  _MenuMode;
        private int   _MenuSeparation;
        private int   _ImageSize;

        [Category("SKYNET")]
        public bool MenuMode
        {
            get { return _MenuMode; }
            set { _MenuMode = value; }
        }

        [Category("SKYNET")]
        public int ImageSize
        {
            get { return _ImageSize; }
            set
            {
                _ImageSize = value;
                PB_Image.Size = new Size(value, value);
                CenterImage();
            }
        }

        [Category("SKYNET")]
        public int MenuSeparation
        {
            get { return _MenuSeparation; }
            set { _MenuSeparation = value; }
        }

        [Category("SKYNET")]
        public event EventHandler BoxClicked;

        [Category("SKYNET")]
        public Color Color
        {
            get
            {
                return _Color;
            }
            set
            {
                _Color = value;
                BackColor = value;

                int R = value.R < 245 ? value.R + 10 : 255;
                int G = value.G < 245 ? value.G + 10 : 255;
                int B = value.B < 245 ? value.B + 10 : 255;

                FocusedColor = Color.FromArgb(R, G, B);
            }
        }

        [Category("SKYNET")]
        public Color FocusedColor
        {
            get
            {
                return _FocusedColor;
            }
            set
            {
                _FocusedColor = value;
            }
        }

        [Category("SKYNET")]
        public Image Image
        {
            get
            {
                return PB_Image.Image;
            }
            set
            {
                PB_Image.Image = value;
            }
        }

        public SKYNET_Box()
        {
            InitializeComponent();

            _MenuSeparation = 8;
            ImageSize = 10;
        }

        private void OnClicked(object sender, MouseEventArgs e)
        {
            BoxClicked?.Invoke(this, new EventArgs());
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            BackColor = _FocusedColor;
        }

        private void OnMouseLeave(object sender, EventArgs e)
        {
            BackColor = _Color;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (_MenuMode)
            {
                MenuPaint(e);
                return;
            }

            base.OnPaint(e);
        }

        private void MenuPaint(PaintEventArgs e)
        {
            Bitmap B = new Bitmap(base.Width, base.Height);
            Graphics G = Graphics.FromImage(B);
            var W = base.Width;
            var H = base.Height;
            Rectangle rect = new Rectangle(0, 0, W, H);
            checked
            {
                Rectangle rect2 = new Rectangle(W - 40, 0, W, H);
                GraphicsPath graphicsPath = new GraphicsPath();
                G.SmoothingMode = SmoothingMode.HighQuality;
                G.PixelOffsetMode = PixelOffsetMode.HighQuality;
                G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                G.FillRectangle(new SolidBrush(BackColor), rect);
                graphicsPath.Reset();
                graphicsPath.AddRectangle(rect2);
                G.SetClip(graphicsPath);

                G.FillRectangle(new SolidBrush(BackColor), rect2); 
                G.ResetClip();

                int mSeparator = (H - (_MenuSeparation * 2)) + _MenuSeparation / 2 + 1;
                G.DrawLine(new Pen(Color.White), W - _MenuSeparation, _MenuSeparation, _MenuSeparation, _MenuSeparation);
                G.DrawLine(new Pen(Color.White), W - _MenuSeparation, mSeparator, _MenuSeparation, mSeparator);
                G.DrawLine(new Pen(Color.White), W - _MenuSeparation, H - _MenuSeparation, _MenuSeparation, H - _MenuSeparation);

                G.Dispose();
                e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                e.Graphics.DrawImageUnscaled(B, 0, 0);
                B.Dispose();
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            CenterImage();
            base.OnSizeChanged(e);
        }

        private void CenterImage()
        {
            var X = (this.Width / 2) - (PB_Image.Width / 2);
            var Y = (this.Height / 2) - (PB_Image.Height / 2);
            PB_Image.Location = new Point(X, Y);
        }
    }
}
