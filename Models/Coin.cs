namespace MauiGomokuNarabeGame.Models;

public enum Coin
{
    RedCoin,
    YellowCoin
}

public static class Extension
{
    public static Coin Next(this Coin coin)
    {
        return coin switch
        {
            Coin.RedCoin => Coin.YellowCoin,
            Coin.YellowCoin => Coin.RedCoin,
            _ => throw new Exception("Unknown coin")
        };
    }
}