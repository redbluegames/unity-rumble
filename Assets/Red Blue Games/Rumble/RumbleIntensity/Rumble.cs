namespace RedBlueGames.Rumble
{
    /// <summary>
    ///     Info that describes RumbleIntensity. RumbleIntensity results in Force Feedback and Screenshake for a
    ///     RumbleListener.
    /// </summary>
    public class Rumble
    {
        public RumbleIntensity FullRumbleIntensity { get; set; }
        public float Lifetime { get; set; }
        public ITimeModulator TimeModulator { get; set; }
        public IDistanceModulator DistanceModulator { get; set; }

        public Rumble(
            RumbleIntensity rumbleIntensity,
            float lifetime,
            ITimeModulator timeModulator,
            IDistanceModulator distanceModulator)
        {
            FullRumbleIntensity = rumbleIntensity;
            Lifetime = lifetime;
            TimeModulator = timeModulator;
            DistanceModulator = distanceModulator;
        }

        public RumbleIntensity CalculateRumbleFromPercentFromCenterAtTime(
            float percentFromCenter,
            float time)
        {
            if (Lifetime >= 0.0f && time > Lifetime) return RumbleIntensity.Zero;

            var rumbleAtTime = TimeModulator.CalculateIntensity(FullRumbleIntensity, time);
            var rumbleAtDistance = DistanceModulator.CalculateFalloff(rumbleAtTime, percentFromCenter);
            return rumbleAtDistance;
        }
    }
}