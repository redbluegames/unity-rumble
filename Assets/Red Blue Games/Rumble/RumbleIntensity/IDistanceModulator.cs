namespace RedBlueGames.Rumble
{
    public interface IDistanceModulator
    {
        RumbleIntensity CalculateFalloff(RumbleIntensity rumbleIntensity, float percentFromCenter);
    }

}