using UnityEngine;

namespace RedBlueGames.Rumble
{
    [CreateAssetMenu]
    public class RumbleAsset : ScriptableObject
    {
        [SerializeField]
        private RumbleIntensity fullRumbleIntensity;

        [SerializeField]
        private float lifetime;

        [SerializeField]
        private CurveTimeModulator curveModulatorOverTime = new CurveTimeModulator(AnimationCurve.Constant(0.0f, 1.0f, 1.0f), 1.0f);

        [SerializeField]
        private CurveDistanceModulator curveFalloffCalculator = new CurveDistanceModulator(AnimationCurve.Linear(0.0f, 1.0f, 1.0f, 0.0f));

        private Rumble modulator;

        public float Lifetime => this.lifetime;

        private ITimeModulator TimeModulator => curveModulatorOverTime;

        private IDistanceModulator FalloffCalculator => curveFalloffCalculator;

        public RumbleIntensity CalculateRumbleFromPercentFromCenterAtTime(
            float percentFromCenter,
            float time)
        {
            // Reapply settings to the rumble so that we can tweak the values on a ScriptableObject while editor
            // is running and see the results
            modulator.FullRumbleIntensity = fullRumbleIntensity;
            modulator.Lifetime = lifetime;
            modulator.TimeModulator = TimeModulator;
            modulator.DistanceModulator = FalloffCalculator;

            return modulator.CalculateRumbleFromPercentFromCenterAtTime(percentFromCenter, time);
        }

        private void OnEnable()
        {
            modulator = new Rumble(
                fullRumbleIntensity,
                lifetime,
                TimeModulator,
                FalloffCalculator
            );
        }
    }
}