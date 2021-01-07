using NUnit.Framework;
using UnityEngine;

namespace RedBlueGames.Rumble.Tests
{
    public class RumbleTests
    {
        private class MockTimeModulator : ITimeModulator
        {
            private RumbleIntensity intensity;
            
            public MockTimeModulator(RumbleIntensity intensityOut)
            {
                this.intensity = intensityOut;
            }

            public RumbleIntensity CalculateIntensity(RumbleIntensity rumbleIntensity, float time) => this.intensity;
        }

        private class MockDistanceModulator : IDistanceModulator
        {
            private RumbleIntensity intensity;
            
            public MockDistanceModulator(RumbleIntensity intensityOut)
            {
                this.intensity = intensityOut;
            }

            public RumbleIntensity CalculateFalloff(RumbleIntensity rumbleIntensity, float percentFromCenter) =>
                this.intensity;
        } 
        
        [Test]
        public void CalculateRumbleFromPercentFromCenterAtTime_WhileAlive_FullRumble()
        {
            var rumble = new Rumble(
                RumbleIntensity.One, 
                1.0f,
                new MockTimeModulator(RumbleIntensity.One),
                new MockDistanceModulator(RumbleIntensity.One));
            
            var intensity = rumble.CalculateRumbleFromPercentFromCenterAtTime(
                0.0f,
                rumble.Lifetime * 0.5f);

            Assert.That(intensity, Is.EqualTo(RumbleIntensity.One));
        }
        
        [Test]
        public void CalculateRumbleFromPercentFromCenterAtTime_WhileDead_ZeroRumble()
        {
            var rumble = new Rumble(
                RumbleIntensity.One, 
                1.0f,
                new MockTimeModulator(RumbleIntensity.One),
                new MockDistanceModulator(RumbleIntensity.One));
            
            var intensity = rumble.CalculateRumbleFromPercentFromCenterAtTime(
                0.0f,
                rumble.Lifetime * 1.1f);

            Assert.That(intensity, Is.EqualTo(RumbleIntensity.Zero));
        }
        
        /*
        [Test]
        public void CalculateRumbleFromDistanceSquaredAtTime_NoFalloffCenteredAtHalfTime_FullRumble()
        {
            var rumbleAsset = ScriptableObject.CreateInstance<RumbleAsset>();
            rumbleAsset.Intensity = RumbleIntensity.One;
            rumbleAsset.FalloffFunction = RumbleAsset.RumbleFalloffFunction.None;

            var rumble = rumbleAsset.CalculateRumbleFromPercentFromCenterAtTime(
                0.0f,
                rumbleAsset.Lifetime * 0.5f);

            Assert.That(rumble.ForceFeedback, Is.EqualTo(ForceFeedbackIntensities.One));
            Assert.That(rumble.ScreenShake, Is.EqualTo(ScreenShakeIntensities.One));
        }

        [Test]
        public void CalculateRumbleFromDistanceSquaredAtTime_NoFalloffCenteredAtTimeGreaterThanlifetime_ZeroRumble()
        {
            var rumbleAsset = ScriptableObject.CreateInstance<RumbleAsset>();
            rumbleAsset.Intensity = RumbleIntensity.One;
            rumbleAsset.FalloffFunction = RumbleAsset.RumbleFalloffFunction.None;

            var rumble = RumbleAsset.CalculateRumbleFromDistanceSquaredAtTime(rumbleAsset, 0.0f, rumbleAsset.Lifetime * 2.0f);

            Assert.That(rumble.ForceFeedback, Is.EqualTo(ForceFeedbackIntensities.Zero));
            Assert.That(rumble.ScreenShake, Is.EqualTo(new ScreenShakeIntensities(Vector2.zero)));
        }

        [Test]
        public void CalculateRumbleFromDistanceSquaredAtTime_NoFalloffCenteredAtZeroTime_FullRumble()
        {
            var rumbleAsset = ScriptableObject.CreateInstance<RumbleAsset>();
            rumbleAsset.Intensity = RumbleIntensity.One;
            rumbleAsset.FalloffFunction = RumbleAsset.RumbleFalloffFunction.None;

            var rumble = RumbleAsset.CalculateRumbleFromDistanceSquaredAtTime(rumbleAsset, 0.0f, 0.0f);

            Assert.That(rumble.ForceFeedback, Is.EqualTo(ForceFeedbackIntensities.One));
            Assert.That(rumble.ScreenShake, Is.EqualTo(ScreenShakeIntensities.One));
        }

        [Test]
        public void CalculateRumbleFromDistanceSquaredAtTime_NoFalloffAtHalfDistance_FullRumble()
        {
            var rumbleAsset = ScriptableObject.CreateInstance<RumbleAsset>();
            rumbleAsset.Intensity = RumbleIntensity.One;
            rumbleAsset.FalloffFunction = RumbleAsset.RumbleFalloffFunction.None;

            var halfDistanceSquared = Mathf.Pow(rumbleAsset.Radius * 0.5f, 2);
            var rumble = RumbleAsset.CalculateRumbleFromDistanceSquaredAtTime(rumbleAsset, halfDistanceSquared, 0.0f);

            Assert.That(rumble.ForceFeedback, Is.EqualTo(ForceFeedbackIntensities.One));
            Assert.That(rumble.ScreenShake, Is.EqualTo(ScreenShakeIntensities.One));
        }

        [Test]
        public void CalculateRumbleFromDistanceSquaredAtTime_NoFalloffOutOfRange_ZeroRumble()
        {
            var rumbleAsset = ScriptableObject.CreateInstance<RumbleAsset>();
            rumbleAsset.Intensity = RumbleIntensity.One;
            rumbleAsset.FalloffFunction = RumbleAsset.RumbleFalloffFunction.None;

            var distanceSquared = (rumbleAsset.Radius * rumbleAsset.Radius) + 1.0f;
            var rumble = RumbleAsset.CalculateRumbleFromDistanceSquaredAtTime(rumbleAsset, distanceSquared, 0.0f);

            Assert.That(rumble.ForceFeedback, Is.EqualTo(ForceFeedbackIntensities.Zero));
            Assert.That(rumble.ScreenShake, Is.EqualTo(new ScreenShakeIntensities(Vector2.zero)));
        }

        [Test]
        public void CalculateRumbleFromDistanceSquaredAtTime_LinearFalloffCentered_FullRumble()
        {
            var rumbleAsset = ScriptableObject.CreateInstance<RumbleAsset>();
            rumbleAsset.Intensity = RumbleIntensity.One;
            rumbleAsset.FalloffFunction = RumbleAsset.RumbleFalloffFunction.Linear;

            var rumble = RumbleAsset.CalculateRumbleFromDistanceSquaredAtTime(rumbleAsset, 0.0f, 0.0f);

            Assert.That(rumble.ForceFeedback, Is.EqualTo(ForceFeedbackIntensities.One));
            Assert.That(rumble.ScreenShake, Is.EqualTo(ScreenShakeIntensities.One));
        }

        [Test]
        public void CalculateRumbleFromDistanceSquaredAtTime_LinearFalloffAtHalfDistance_HalfRumble()
        {
            var rumbleAsset = ScriptableObject.CreateInstance<RumbleAsset>();
            rumbleAsset.Intensity = RumbleIntensity.One;
            rumbleAsset.FalloffFunction = RumbleAsset.RumbleFalloffFunction.Linear;

            var halfDistanceSquared = Mathf.Pow(rumbleAsset.Radius * 0.5f, 2);
            var rumble = RumbleAsset.CalculateRumbleFromDistanceSquaredAtTime(rumbleAsset, halfDistanceSquared, 0.0f);

            Assert.That(rumble.ForceFeedback, Is.EqualTo( new ForceFeedbackIntensities(0.5f, 0.5f)));
            Assert.That(rumble.ScreenShake, Is.EqualTo(new ScreenShakeIntensities(new Vector2(0.5f, 0.5f), 1)));
        }

        [Test]
        public void CalculateRumbleFromDistanceSquaredAtTime_LinearFalloffOutOfRange_ZeroRumble()
        {
            var rumbleAsset = ScriptableObject.CreateInstance<RumbleAsset>();
            rumbleAsset.Intensity = RumbleIntensity.One;
            rumbleAsset.FalloffFunction = RumbleAsset.RumbleFalloffFunction.Linear;

            var distanceSquared = (rumbleAsset.Radius * rumbleAsset.Radius) + 1.0f;
            var rumble = RumbleAsset.CalculateRumbleFromDistanceSquaredAtTime(rumbleAsset, distanceSquared, 0.0f);

            Assert.That(rumble.ForceFeedback, Is.EqualTo(ForceFeedbackIntensities.Zero));
            Assert.That(rumble.ScreenShake, Is.EqualTo(new ScreenShakeIntensities(Vector2.zero)));
        }

        [Test]
        public void CalculateRumbleFromDistanceSquaredAtTime_ExponentialFalloffCentered_FullRumble()
        {
            var rumbleAsset = ScriptableObject.CreateInstance<RumbleAsset>();
            rumbleAsset.Intensity = RumbleIntensity.One;
            rumbleAsset.FalloffFunction = RumbleAsset.RumbleFalloffFunction.Exponential;

            var rumble = RumbleAsset.CalculateRumbleFromDistanceSquaredAtTime(rumbleAsset, 0.0f, 0.0f);

            Assert.That(rumble.ForceFeedback, Is.EqualTo(ForceFeedbackIntensities.One));
            Assert.That(rumble.ScreenShake, Is.EqualTo(ScreenShakeIntensities.One));
        }

        [Test]
        public void CalculateRumbleFromDistanceSquaredAtTime_ExponentialFalloffAtHalfDistance_QuarterRumble()
        {
            var rumbleAsset = ScriptableObject.CreateInstance<RumbleAsset>();
            rumbleAsset.Intensity = RumbleIntensity.One;
            rumbleAsset.FalloffFunction = RumbleAsset.RumbleFalloffFunction.Exponential;

            var halfDistanceSquared = Mathf.Pow(rumbleAsset.Radius * 0.5f, 2);
            var rumble = RumbleAsset.CalculateRumbleFromDistanceSquaredAtTime(rumbleAsset, halfDistanceSquared, 0.0f);

            Assert.That(rumble.ForceFeedback, Is.EqualTo( new ForceFeedbackIntensities(0.25f, 0.25f)));
            Assert.That(rumble.ScreenShake, Is.EqualTo(new ScreenShakeIntensities(new Vector2(0.25f, 0.25f), 1)));
        }

        [Test]
        public void CalculateRumbleFromDistanceSquaredAtTime_ExponentialFalloffOutOfRange_ZeroRumble()
        {
            var rumbleAsset = ScriptableObject.CreateInstance<RumbleAsset>();
            rumbleAsset.Intensity = RumbleIntensity.One;
            rumbleAsset.FalloffFunction = RumbleAsset.RumbleFalloffFunction.Exponential;

            var distanceSquared = (rumbleAsset.Radius * rumbleAsset.Radius) + 1.0f;
            var rumble = RumbleAsset.CalculateRumbleFromDistanceSquaredAtTime(rumbleAsset, distanceSquared, 0.0f);

            Assert.That(rumble.ForceFeedback, Is.EqualTo(ForceFeedbackIntensities.Zero));
            Assert.That(rumble.ScreenShake, Is.EqualTo(new ScreenShakeIntensities(Vector2.zero)));
        }
        */
    }
}