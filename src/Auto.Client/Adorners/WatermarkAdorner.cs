using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Auto.Client.Adorners;

public class WatermarkAdorner : Adorner
{
    private readonly TextBlock _textBlock;

    public WatermarkAdorner(UIElement element, string watermarkText) : base(element)
    {
        _textBlock = new TextBlock
        {
            Text = watermarkText,
            Foreground = Brushes.Gray,
            Margin = new Thickness(5, 0, 0, 0),
            VerticalAlignment = VerticalAlignment.Center
        };

        AddVisualChild(_textBlock);
    }

    protected override int VisualChildrenCount => 1;

    protected override Visual GetVisualChild(int index) => _textBlock;

    protected override Size ArrangeOverride(Size finalSize)
    {
        _textBlock.Arrange(new Rect(new Point(0, 0), finalSize));
        return finalSize;
    }
}