namespace RedBlueGames.Rumble
{
    public interface ITimeModulator
    {
        RumbleIntensity CalculateIntensity(RumbleIntensity rumbleIntensity, float time);
    }
}