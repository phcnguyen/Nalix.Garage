using System.Windows;
using System.Windows.Controls;

namespace Auto.Client.Behaviors;

public static class PasswordBehaviors
{
    public static readonly DependencyProperty BoundPasswordProperty =
        DependencyProperty.RegisterAttached("BoundPassword", typeof(string),
            typeof(PasswordBehaviors), new PropertyMetadata(string.Empty, OnBoundPasswordChanged));

    public static void SetBoundPassword(DependencyObject dp, string value)
        => dp?.SetValue(BoundPasswordProperty, value);

    public static string? GetBoundPassword(DependencyObject dp)
        => dp?.GetValue(BoundPasswordProperty) as string;

    private static void OnBoundPasswordChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
    {
        if (dp is PasswordBox passwordBox && e.NewValue is string newPassword)
        {
            passwordBox.PasswordChanged -= PasswordBox_PasswordChanged;
            passwordBox.Password = newPassword;
            passwordBox.PasswordChanged += PasswordBox_PasswordChanged;
        }
    }

    private static void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (sender is PasswordBox passwordBox)
        {
            SetBoundPassword(passwordBox, passwordBox.Password);
        }
    }
}
