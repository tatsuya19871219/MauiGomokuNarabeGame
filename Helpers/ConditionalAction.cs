namespace MauiGomokuNarabeGame.Helpers;

internal class ConditionalAction
{
    required public Action ReservedAction { get; init; }

    public async void InvokeIf(Func<bool> condition, int millisecondsDelay = 100, int millisecondsTimeout = 1000)
    {
        if (condition is null) throw new ArgumentNullException(nameof(condition));
        if (millisecondsDelay <= 0) throw new ArgumentOutOfRangeException(nameof(millisecondsDelay));
        if (millisecondsTimeout <= 0) throw new ArgumentOutOfRangeException(nameof(millisecondsTimeout));

        int elapsedDelay = 0;
        while (true)
        {
            if (elapsedDelay > millisecondsTimeout) throw new TimeoutException("Reserved action is never executed");

            if (condition.Invoke()) break;

            elapsedDelay += millisecondsDelay;
            await Task.Delay(millisecondsDelay);
        }

        ReservedAction.Invoke();
    }
}
