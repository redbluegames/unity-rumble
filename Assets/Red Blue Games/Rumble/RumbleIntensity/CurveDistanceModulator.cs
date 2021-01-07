namespace RedBlueGames.Rumble
{
    using UnityEngine;
    
    [System.Serializable]
    public class CurveDistanceModulator : IDistanceModulator
    {
        [SerializeField]
        private AnimationCurve curve;

        public CurveDistanceModulator(AnimationCurve curve)
        {
            this.curve = curve;
        }

        public RumbleIntensity CalculateFalloff(RumbleIntensity rumbleIntensity, float percentFromCenter)
        {
            float scale = curve.Evaluate(percentFromCenter);
            return rumbleIntensity * scale;
        }
    }
}