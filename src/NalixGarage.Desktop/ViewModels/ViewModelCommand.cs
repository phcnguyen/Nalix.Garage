using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NalixGarage.Desktop.ViewModels;

/// <summary>
/// Lớp <c>ViewModelCommand</c> giúp thực thi lệnh trong MVVM.
/// Hỗ trợ cả phương thức đồng bộ (<c>void</c>) và bất đồng bộ (<c>async Task</c>).
/// </summary>
public sealed class ViewModelCommand : ICommand
{
    private readonly Func<Task>? _executeAsync;
    private readonly Action? _execute;
    private readonly Func<bool>? _canExecute;
    private bool _isExecuting;

    /// <summary>
    /// Sự kiện được kích hoạt khi trạng thái có thể thực thi của lệnh thay đổi.
    /// </summary>
    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    /// <summary>
    /// Khởi tạo một <c>ViewModelCommand</c> với phương thức đồng bộ (<c>void</c>).
    /// </summary>
    /// <param name="execute">Phương thức đồng bộ được thực thi khi lệnh chạy.</param>
    /// <param name="canExecute">Hàm kiểm tra xem lệnh có thể thực thi không.</param>
    /// <exception cref="ArgumentNullException">Ném ra nếu <paramref name="execute"/> là <c>null</c>.</exception>
    public ViewModelCommand(Action execute, Func<bool>? canExecute = null)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    /// <summary>
    /// Khởi tạo một <c>ViewModelCommand</c> với phương thức bất đồng bộ (<c>async Task</c>).
    /// </summary>
    /// <param name="executeAsync">Phương thức bất đồng bộ được thực thi khi lệnh chạy.</param>
    /// <param name="canExecute">Hàm kiểm tra xem lệnh có thể thực thi không.</param>
    /// <exception cref="ArgumentNullException">Ném ra nếu <paramref name="executeAsync"/> là <c>null</c>.</exception>
    public ViewModelCommand(Func<Task> executeAsync, Func<bool>? canExecute = null)
    {
        _executeAsync = executeAsync ?? throw new ArgumentNullException(nameof(executeAsync));
        _canExecute = canExecute;
    }

    /// <summary>
    /// Xác định xem lệnh có thể thực thi hay không.
    /// </summary>
    /// <param name="parameter">Tham số truyền vào (không được sử dụng).</param>
    /// <returns><c>true</c> nếu lệnh có thể thực thi, ngược lại <c>false</c>.</returns>
    public bool CanExecute(object? parameter) => !_isExecuting && (_canExecute?.Invoke() ?? true);

    /// <summary>
    /// Thực thi lệnh với tham số được cung cấp.
    /// Hỗ trợ cả phương thức đồng bộ và bất đồng bộ.
    /// </summary>
    /// <param name="parameter">Tham số truyền vào (không được sử dụng).</param>
    public async void Execute(object? parameter)
    {
        if (!CanExecute(parameter)) return;

        _isExecuting = true;
        CommandManager.InvalidateRequerySuggested();

        try
        {
            if (_executeAsync is not null)
                await _executeAsync();
            else
                _execute?.Invoke();
        }
        finally
        {
            _isExecuting = false;
            CommandManager.InvalidateRequerySuggested();
        }
    }
}