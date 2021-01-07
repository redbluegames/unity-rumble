namespace RedBlueGames.Rumble
{
    using UnityEngine;
    
    [System.Serializable]
    public class CurveTimeModulator : ITimeModulator
    {
        [SerializeField]
        private float timePerCycle;

        [SerializeField]
        private AnimationCurve curve;

        public CurveTimeModulator(AnimationCurve curve, float timePerCycle)
        {
            this.curve = curve;
            this.timePerCycle = timePerCycle;
        }
        
        public RumbleIntensity CalculateIntensity(RumbleIntensity rumbleIntensity, float time)
        {
            float t = (time % timePerCycle) / timePerCycle;
            float scale = curve.Evaluate(t);
            return rumbleIntensity * scale;
        }
    }
}