using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Auto.Client.Behaviors
{
    public static class WatermarkBehavior
    {
        // Tạo Attached Property "Watermark"
        public static readonly DependencyProperty WatermarkProperty =
            DependencyProperty.RegisterAttached(
                "Watermark",
                typeof(string),
                typeof(WatermarkBehavior),
                new PropertyMetadata(null, OnWatermarkChanged));

        public static string GetWatermark(DependencyObject obj) => (string)obj.GetValue(WatermarkProperty);

        public static void SetWatermark(DependencyObject obj, string value) => obj.SetValue(WatermarkProperty, value);

        private static void OnWatermarkChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBox textBox)
            {
                textBox.Loaded += (sender, args) => ApplyWatermark(textBox, (string)e.NewValue);
                textBox.GotFocus += (sender, args) => RemoveWatermark(textBox, (string)e.NewValue);
                textBox.LostFocus += (sender, args) => ApplyWatermark(textBox, (string)e.NewValue);
            }
            else if (d is PasswordBox passwordBox)
            {
                passwordBox.Loaded += (sender, args) => ApplyWatermark(passwordBox, (string)e.NewValue);
                passwordBox.PasswordChanged += (sender, args) => RemoveWatermark(passwordBox, (string)e.NewValue);
                passwordBox.GotFocus += (sender, args) => RemoveWatermark(passwordBox, (string)e.NewValue);
                passwordBox.LostFocus += (sender, args) => ApplyWatermark(passwordBox, (string)e.NewValue);
            }
        }

        private static void ApplyWatermark(TextBox textBox, string watermark)
        {
            if (string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Text = watermark;
                textBox.Foreground = Brushes.Gray;
            }
        }

        private static void RemoveWatermark(TextBox textBox, string watermark)
        {
            if (textBox.Text == watermark)
            {
                textBox.Text = "";
                textBox.Foreground = Brushes.Black;
            }
        }

        private static void ApplyWatermark(PasswordBox passwordBox, string watermark)
        {
            if (string.IsNullOrEmpty(passwordBox.Password))
            {
                passwordBox.Tag = watermark;
                passwordBox.Foreground = Brushes.Gray;
                passwordBox.PasswordChanged += (sender, args) => passwordBox.Foreground = Brushes.Black;
            }
        }

        private static void RemoveWatermark(PasswordBox passwordBox, string watermark)
        {
            if ((string)passwordBox.Tag == watermark)
            {
                passwordBox.Tag = "";
                passwordBox.Foreground = Brushes.Black;
            }
        }
    }
}