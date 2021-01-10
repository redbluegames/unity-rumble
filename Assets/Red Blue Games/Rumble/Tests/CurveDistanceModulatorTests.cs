using NUnit.Framework;
using UnityEngine;

namespace RedBlueGames.Rumble.Tests
{
    public class CurveDistanceModulatorTests
    {
        private readonly AnimationCurve linearOneToZero = AnimationCurve.Linear(0.0f, 1.0f, 1.0f, 0.0f);
        
        [Test]
        public void CalculateFalloff_LinearOneToZeroAtCenterPercent_IsOne()
        {
            var modulator = new CurveDistanceModulator(linearOneToZero);
            var intensity = modulator.CalculateFalloff(RumbleIntensity.One, 0.0f);
            Assert.That(intensity, Is.EqualTo(RumbleIntensity.One));
        }
        
        [Test]
        public void CalculateFalloff_LinearOneToZeroAtHalfPercent_IsHalf()
        {
            var modulator = new CurveDistanceModulator(linearOneToZero);
            var intensity = modulator.CalculateFalloff(RumbleIntensity.One, 0.5f);
            Assert.That(intensity, Is.EqualTo(
                new RumbleIntensity(
                    ForceFeedbackIntensities.One * 0.5f, 
                    ScreenShakeIntensities.One * 0.5f)));
        }
        
        [Test]
        public void CalculateFalloff_LinearOneToZeroAtFullPercent_IsZero()
        {
            var modulator = new CurveDistanceModulator(linearOneToZero);
            var intensity = modulator.CalculateFalloff(RumbleIntensity.One, 1.0f);
            Assert.That(intensity, Is.EqualTo(
                new RumbleIntensity(
                    ForceFeedbackIntensities.One * 0.0f, 
                    ScreenShakeIntensities.One * 0.0f)));
        }
    }
}
