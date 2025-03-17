using System;
using System.Windows.Input;

namespace Auto.Application.Helpers;

/// <summary>
/// Lớp triển khai <see cref="ICommand"/> để hỗ trợ tạo lệnh có thể thực thi trong MVVM.
/// </summary>
/// <remarks>
/// Cho phép xác định hành động khi thực thi và điều kiện để có thể thực thi.
/// </remarks>
public class RelayCommand(Action execute, Func<bool>? canExecute = null) : ICommand
{
    private readonly Action _execute = execute;
    private readonly Func<bool>? _canExecute = canExecute;

    /// <summary>
    /// Sự kiện được kích hoạt khi trạng thái có thể thực thi của lệnh thay đổi.
    /// </summary>
    public event EventHandler? CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }

    /// <summary>
    /// Thực thi lệnh với tham số được cung cấp.
    /// </summary>
    /// <param name="parameter">Tham số được truyền vào lệnh.</param>
    public void Execute(object? parameter) => _execute();

    /// <summary>
    /// Xác định xem lệnh có thể thực thi với tham số hiện tại hay không.
    /// </summary>
    /// <param name="parameter">Tham số cần kiểm tra.</param>
    /// <returns>
    /// Trả về <c>true</c> nếu lệnh có thể thực thi, ngược lại trả về <c>false</c>.
    /// </returns>
    public bool CanExecute(object? parameter) => _canExecute?.Invoke() ?? true;
}