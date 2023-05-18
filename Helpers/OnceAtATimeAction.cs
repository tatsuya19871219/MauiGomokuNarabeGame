namespace MauiGomokuNarabeGame.Helpers;

internal class OnceAtATimeAction
{
    bool _isRunning = false;
    internal bool IsRunning => _isRunning;

    Func<Task> _action;

    public OnceAtATimeAction(Func<Task> action)
    {
        _action = action;
    }

    public async Task<bool> TryInvokeAsync()
    {
        if (_isRunning) return false;

        _isRunning = true;

        await _action.Invoke();

        _isRunning = false;

        return true;
    }
}

internal class OnceAtATimeAction<T>
{
    bool _isRunning = false;
    internal bool IsRunning => _isRunning;

    Func<T, Task> _action;

    public OnceAtATimeAction(Func<T, Task> action)
    {
        _action = action;
    }

    public async Task<bool> TryInvokeAsync(T param)
    {
        if (_isRunning) return false;

        _isRunning = true;

        await _action.Invoke(param);

        _isRunning = false;

        return true;
    }
}
