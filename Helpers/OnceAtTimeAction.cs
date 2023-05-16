﻿namespace MauiGomokuNarabeGame.Helpers;

internal class OnceAtTimeAction
{
    bool _isRunning = false;
    internal bool IsRunning => _isRunning;

    Func<Task> _action;

    public OnceAtTimeAction(Func<Task> action)
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

internal class OnceAtTimeAction<T>
{
    bool _isRunning = false;
    internal bool IsRunning => _isRunning;

    Func<T, Task> _action;

    public OnceAtTimeAction(Func<T, Task> action)
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
