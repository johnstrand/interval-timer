using Microsoft.Maui.Graphics;
using System;

namespace IntervalTimer.Controls;

public class CircularProgressBar : GraphicsView
{
    public static readonly BindableProperty ProgressProperty =
        BindableProperty.Create(nameof(Progress), typeof(double), typeof(CircularProgressBar), 1.0, propertyChanged: OnPropertyChanged);

    public static readonly BindableProperty ProgressColorProperty =
        BindableProperty.Create(nameof(ProgressColor), typeof(Color), typeof(CircularProgressBar), Colors.Green, propertyChanged: OnPropertyChanged);

    public double Progress
    {
        get => (double)GetValue(ProgressProperty);
        set => SetValue(ProgressProperty, value);
    }

    public Color ProgressColor
    {
        get => (Color)GetValue(ProgressColorProperty);
        set => SetValue(ProgressColorProperty, value);
    }

    public CircularProgressBar()
    {
        Drawable = new CircularProgressDrawable(this);
    }

    private static void OnPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is CircularProgressBar bar)
        {
            bar.Invalidate();
        }
    }

    private class CircularProgressDrawable : IDrawable
    {
        private readonly CircularProgressBar _bar;

        public CircularProgressDrawable(CircularProgressBar bar)
        {
            _bar = bar;
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            float centerX = dirtyRect.Width / 2;
            float centerY = dirtyRect.Height / 2;
            float radius = (Math.Min(dirtyRect.Width, dirtyRect.Height) / 2) - 12;

            // Draw Background Track
            canvas.StrokeColor = Color.FromArgb("#1E1E1E"); // Surface color
            canvas.StrokeSize = 12;
            canvas.StrokeLineCap = LineCap.Round;
            canvas.DrawCircle(centerX, centerY, radius);

            // Draw Progress
            if (_bar.Progress > 0)
            {
                canvas.StrokeColor = _bar.ProgressColor;
                canvas.StrokeSize = 12;
                
                // MAUI Graphics DrawArc uses startAngle and endAngle, not sweepAngle.
                // 90 degrees is the top in MAUI Graphics.
                float startAngle = 90;
                float endAngle = startAngle - (float)(360 * _bar.Progress);
                
                RectF arcRect = new RectF(centerX - radius, centerY - radius, radius * 2, radius * 2);
                
                // false for clockwise parameter because MAUI Graphics standard angles go counter-clockwise (90 is top, 0 is right)
                canvas.DrawArc(arcRect, startAngle, endAngle, true, false);
            }
        }
    }
}
