using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
internal class FlatListBox : Control
{
    public delegate void SelectedIndexChangedEventHandler();

    [CompilerGenerated]
    [AccessedThroughProperty("ListBx")]
    private ListBox _ListBx;

    private string[] _Items;

    private string _SelectedItem;

    private int _SelectedIndex;

    [CompilerGenerated]
    private SelectedIndexChangedEventHandler SelectedIndexChangedEvent;

    internal virtual ListBox ListBx
    {
        [CompilerGenerated]
        get
        {
            return _ListBx;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        [CompilerGenerated]
        set
        {
            DrawItemEventHandler value2 = Drawitem;
            ListBox listBx = _ListBx;
            if (listBx != null)
            {
                listBx.DrawItem -= value2;
            }
            _ListBx = value;
            listBx = _ListBx;
            if (listBx != null)
            {
                listBx.DrawItem += value2;
            }
        }
    }

    [Category("Options")]
    public string[] Items
    {
        get
        {
            return _Items;
        }
        set
        {
            _Items = value;
            ListBx.Items.Clear();
            ListBx.Items.AddRange(value);
            Invalidate();
        }
    }

    public string SelectedItem
    {
        get
        {
            return Conversions.ToString(ListBx.SelectedItem);
        }
        set
        {
            ListBx.SelectedItem = value;
            _SelectedItem = value;
        }
    }

    public int SelectedIndex => ListBx.SelectedIndex;

    public event SelectedIndexChangedEventHandler SelectedIndexChanged
    {
        [CompilerGenerated]
        add
        {
            SelectedIndexChangedEventHandler selectedIndexChangedEventHandler = SelectedIndexChangedEvent;
            SelectedIndexChangedEventHandler selectedIndexChangedEventHandler2;
            do
            {
                selectedIndexChangedEventHandler2 = selectedIndexChangedEventHandler;
                SelectedIndexChangedEventHandler value2 = (SelectedIndexChangedEventHandler)Delegate.Combine(selectedIndexChangedEventHandler2, value);
                selectedIndexChangedEventHandler = Interlocked.CompareExchange(ref SelectedIndexChangedEvent, value2, selectedIndexChangedEventHandler2);
            }
            while ((object)selectedIndexChangedEventHandler != selectedIndexChangedEventHandler2);
        }
        [CompilerGenerated]
        remove
        {
            SelectedIndexChangedEventHandler selectedIndexChangedEventHandler = SelectedIndexChangedEvent;
            SelectedIndexChangedEventHandler selectedIndexChangedEventHandler2;
            do
            {
                selectedIndexChangedEventHandler2 = selectedIndexChangedEventHandler;
                SelectedIndexChangedEventHandler value2 = (SelectedIndexChangedEventHandler)Delegate.Remove(selectedIndexChangedEventHandler2, value);
                selectedIndexChangedEventHandler = Interlocked.CompareExchange(ref SelectedIndexChangedEvent, value2, selectedIndexChangedEventHandler2);
            }
            while ((object)selectedIndexChangedEventHandler != selectedIndexChangedEventHandler2);
        }
    }

    public object IsSelected(int Index)
    {
        if (ListBx.SelectedIndex == Index)
        {
            return true;
        }
        return false;
    }

    public object GetItemAt(int Index)
    {
        return ListBx.Items[Index];
    }

    public object ItemsCount()
    {
        return ListBx.Items.Count;
    }

    public void ScrollToLast()
    {
        ListBx.SetSelected(checked(ListBx.Items.Count - 1), value: false);
    }

    public void Clear()
    {
        ListBx.Items.Clear();
    }

    public void RemoveAt(int Index)
    {
        ListBx.Items.RemoveAt(Index);
    }

    public void Drawitem(object sender, DrawItemEventArgs e)
    {
        if (e.Index >= 0)
        {
            e.DrawBackground();
            e.DrawFocusRectangle();
            Graphics graphics = e.Graphics;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            if (Strings.InStr(e.State.ToString(), "Selected,") > 0)
            {
                graphics.FillRectangle(new SolidBrush(ColorSystem.FlatColor), new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height));
                graphics.DrawString(" " + ListBx.Items[e.Index].ToString(), new Font("Segoe UI", 8f), new SolidBrush(ColorSystem.FontColorButtons), (float)e.Bounds.X, (float)checked(e.Bounds.Y + 2));
            }
            else
            {
                graphics.FillRectangle(new SolidBrush(ColorSystem.BackLight), new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height));
                graphics.DrawString(" " + ListBx.Items[e.Index].ToString(), new Font("Segoe UI", 8f), new SolidBrush(ColorSystem.FontColorButtons), (float)e.Bounds.X, (float)checked(e.Bounds.Y + 2));
            }
            graphics.Dispose();
            graphics = null;
        }
    }

    protected override void OnCreateControl()
    {
        ListBx.SelectedIndexChanged += delegate
        {
            IndexChanged();
        };
        base.OnCreateControl();
        if (!base.Controls.Contains(ListBx))
        {
            base.Controls.Add(ListBx);
        }
    }

    private void IndexChanged()
    {
        if (ListBx.SelectedIndex != _SelectedIndex)
        {
            SelectedIndexChangedEvent?.Invoke();
            _SelectedIndex = ListBx.SelectedIndex;
        }
    }

    public void AddRange(object[] items)
    {
        ListBx.Items.Remove("");
        ListBx.Items.AddRange(items);
    }

    public void AddItem(string item)
    {
        ListBx.Items.Remove("");
        ListBx.Items.Add(item);
    }

    public void SetItemSelected(int Index, bool State)
    {
        if ((Index > -1) & (Index < ListBx.Items.Count))
        {
            ListBx.SetSelected(Index, State);
            SelectedIndexChangedEvent?.Invoke();
        }
    }

    public void Sorted(bool State)
    {
        ListBx.Sorted = State;
    }

    public FlatListBox()
    {
        ListBx = new ListBox();
        _Items = new string[1]
        {
            ""
        };
        _SelectedIndex = -1;
        SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, value: true);
        DoubleBuffered = true;
        ListBx.DrawMode = DrawMode.OwnerDrawFixed;
        ListBx.ScrollAlwaysVisible = false;
        ListBx.HorizontalScrollbar = false;
        ListBx.BorderStyle = BorderStyle.None;
        ListBx.BackColor = ColorSystem.BackLight;
        ListBx.ForeColor = ColorSystem.FontColorButtons;
        ListBx.Location = new Point(3, 3);
        ListBx.Font = new Font("Segoe UI", 8f);
        ListBx.ItemHeight = 20;
        ListBx.Items.Clear();
        ListBx.IntegralHeight = false;
        base.Size = new Size(131, 101);
        BackColor = ColorSystem.BackLight;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        Helpers.B = new Bitmap(base.Width, base.Height);
        Helpers.G = Graphics.FromImage(Helpers.B);
        Graphics g = Helpers.G;
        g.SmoothingMode = SmoothingMode.HighQuality;
        g.PixelOffsetMode = PixelOffsetMode.HighQuality;
        g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
        g.Clear(ColorSystem.BackLight);
        ListBx.Size = checked(new Size(base.Width - 6, base.Height - 6));
        ListBx.BackColor = ColorSystem.BackLight;
        g.FillRectangle(new SolidBrush(ColorSystem.BackLight), new Rectangle(0, 0, base.Width, base.Height));
        base.OnPaint(e);
        Helpers.G.Dispose();
        e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
        e.Graphics.DrawImageUnscaled(Helpers.B, 0, 0);
        Helpers.B.Dispose();
    }


}

