using NUnit.Framework;
using UnityEngine;

namespace RedBlueGames.Rumble.Tests
{
    public class CurveTimeModulatorTests
    {
        private readonly AnimationCurve linearOneToZero = AnimationCurve.Linear(0.0f, 1.0f, 1.0f, 0.0f);
        
        [Test]
        public void CalculateIntensity_LinearOneToZeroAtTimeZero_IsOne()
        {
            var modulator = new CurveTimeModulator(linearOneToZero, 3.0f);
            var intensity = modulator.CalculateIntensity(RumbleIntensity.One, 0.0f);
            Assert.That(intensity, Is.EqualTo(RumbleIntensity.One));
        }
        
        [Test]
        public void CalculateIntensity_LinearOneToZeroAtHalfTime_IsHalf()
        {
            var modulator = new CurveTimeModulator(linearOneToZero, 1.0f);
            var intensity = modulator.CalculateIntensity(RumbleIntensity.One, 0.5f);
            Assert.That(intensity, Is.EqualTo(
                new RumbleIntensity(
                    ForceFeedbackIntensities.One * 0.5f, 
                    ScreenShakeIntensities.One * 0.5f)));
        }
        
        [Test]
        public void CalculateIntensity_HalfTimeAfterLoop_IsHalf()
        {
            var modulator = new CurveTimeModulator(linearOneToZero, 2.0f);
            var intensity = modulator.CalculateIntensity(RumbleIntensity.One, 3.0f);
            Assert.That(intensity, Is.EqualTo(
                new RumbleIntensity(
                    ForceFeedbackIntensities.One * 0.5f, 
                    ScreenShakeIntensities.One * 0.5f)));
        }
    }
}
