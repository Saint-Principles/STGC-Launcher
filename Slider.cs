using System;
using System.Drawing;
using System.Windows.Forms;

public partial class Slider : UserControl
{
    private int _value = 50;
    private int _min = 0;
    private int _max = 100;
    private bool _dragging = false;

    public event EventHandler ValueChanged;

    public int Value
    {
        get => _value;
        set
        {
            _value = Math.Max(_min, Math.Min(_max, value));
            OnValueChanged();
            Invalidate();
        }
    }

    public int Minimum
    {
        get => _min;
        set
        {
            _min = value;
            if (_value < _min) _value = _min;

            Invalidate();
        }
    }

    public int Maximum
    {
        get => _max;
        set
        {
            _max = value;
            if (_value > _max) _value = _max;

            Invalidate();
        }
    }

    public Color TrackColor { get; set; } = Color.FromArgb(70, 130, 180);
    public Color SliderColor { get; set; } = Color.White;

    public Slider()
    {
        SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        SetStyle(ControlStyles.UserPaint, true);
        SetStyle(ControlStyles.ResizeRedraw, true);

        BackColor = Color.Transparent;
        Height = 25;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        var graphics = e.Graphics;
        graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

        var trackHeight = 4;
        var trackY = (Height - trackHeight) / 2;
        using (var trackBrush = new SolidBrush(Color.FromArgb(100, TrackColor)))
        {
            graphics.FillRectangle(trackBrush, 0, trackY, Width, trackHeight);
        }

        var filledWidth = (int)((Value - Minimum) / (float)(Maximum - Minimum) * Width);
        using (var filledBrush = new SolidBrush(TrackColor))
        {
            graphics.FillRectangle(filledBrush, 0, trackY, filledWidth, trackHeight);
        }

        var sliderX = filledWidth - 8;
        sliderX = Math.Max(0, Math.Min(Width - 17, sliderX));
        using (var sliderBrush = new SolidBrush(SliderColor))
        {
            graphics.FillEllipse(sliderBrush, sliderX, trackY - 6, 16, 16);
        }
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        base.OnMouseDown(e);
        _dragging = true;
        UpdateValue(e.X);
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);

        if (_dragging) UpdateValue(e.X);
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        base.OnMouseUp(e);
        _dragging = false;
    }

    private void UpdateValue(int mouseX)
    {
        var percentage = Math.Max(0, Math.Min(1, mouseX / (float)Width));
        var newValue = Minimum + (int)(percentage * (Maximum - Minimum));

        if (newValue != Value) Value = newValue;
    }

    protected virtual void OnValueChanged()
    {
        ValueChanged?.Invoke(this, EventArgs.Empty);
    }
}