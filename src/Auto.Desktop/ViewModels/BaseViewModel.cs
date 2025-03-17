using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Auto.Desktop.ViewModels;

/// <summary>
/// Lớp cơ sở triển khai <see cref="INotifyPropertyChanged"/> để hỗ trợ thông báo khi thuộc tính thay đổi.
/// </summary>
public class BaseViewModel : INotifyPropertyChanged
{
    /// <summary>
    /// Sự kiện được kích hoạt khi giá trị của một thuộc tính thay đổi.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Gửi thông báo rằng một thuộc tính đã thay đổi.
    /// </summary>
    /// <param name="propertyName">Tên của thuộc tính đã thay đổi. Giá trị mặc định là tên của phương thức gọi nó.</param>
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    /// <summary>
    /// Cập nhật giá trị của một thuộc tính và gửi thông báo nếu giá trị thay đổi.
    /// </summary>
    /// <typeparam name="T">Kiểu dữ liệu của thuộc tính.</typeparam>
    /// <param name="storage">Tham chiếu đến biến lưu trữ giá trị của thuộc tính.</param>
    /// <param name="value">Giá trị mới của thuộc tính.</param>
    /// <param name="propertyName">Tên của thuộc tính được cập nhật. Giá trị mặc định là tên của phương thức gọi nó.</param>
    /// <returns>
    /// Trả về <c>true</c> nếu giá trị đã được thay đổi; ngược lại trả về <c>false</c>.
    /// </returns>
    protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string? propertyName = null)
    {
        if (Equals(storage, value))
            return false;

        storage = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}
