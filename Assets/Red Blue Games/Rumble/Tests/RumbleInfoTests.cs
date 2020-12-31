using NUnit.Framework;
using UnityEngine;

namespace RedBlueGames.Rumble.Tests
{
    public class RumbleInfoTests
    {
        [Test]
        public void CalculateRumbleFromDistanceSquaredAtTime_NoFalloffCenteredAtHalfTime_FullRumble()
        {
            var rumbleInfo = ScriptableObject.CreateInstance<RumbleInfo>();
            rumbleInfo.Radius = 10.0f;
            rumbleInfo.RumbleSettings = Rumble.One;
            rumbleInfo.FalloffFunction = RumbleInfo.RumbleFalloffFunction.None;

            var rumble = RumbleInfo.CalculateRumbleFromDistanceSquaredAtTime(rumbleInfo, 0.0f, rumbleInfo.Lifetime * 0.5f);

            Assert.That(rumble.ForceFeedback, Is.EqualTo(ForceFeedbackIntensities.One));
            Assert.That(rumble.ScreenShake, Is.EqualTo(ScreenShakeIntensities.One));
        }

        [Test]
        public void CalculateRumbleFromDistanceSquaredAtTime_NoFalloffCenteredAtTimeGreaterThanlifetime_ZeroRumble()
        {
            var rumbleInfo = ScriptableObject.CreateInstance<RumbleInfo>();
            rumbleInfo.Radius = 10.0f;
            rumbleInfo.RumbleSettings = Rumble.One;
            rumbleInfo.FalloffFunction = RumbleInfo.RumbleFalloffFunction.None;

            var rumble = RumbleInfo.CalculateRumbleFromDistanceSquaredAtTime(rumbleInfo, 0.0f, rumbleInfo.Lifetime * 2.0f);

            Assert.That(rumble.ForceFeedback, Is.EqualTo(ForceFeedbackIntensities.Zero));
            Assert.That(rumble.ScreenShake, Is.EqualTo(new ScreenShakeIntensities(Vector2.zero)));
        }

        [Test]
        public void CalculateRumbleFromDistanceSquaredAtTime_NoFalloffCenteredAtZeroTime_FullRumble()
        {
            var rumbleInfo = ScriptableObject.CreateInstance<RumbleInfo>();
            rumbleInfo.Radius = 10.0f;
            rumbleInfo.RumbleSettings = Rumble.One;
            rumbleInfo.FalloffFunction = RumbleInfo.RumbleFalloffFunction.None;

            var rumble = RumbleInfo.CalculateRumbleFromDistanceSquaredAtTime(rumbleInfo, 0.0f, 0.0f);

            Assert.That(rumble.ForceFeedback, Is.EqualTo(ForceFeedbackIntensities.One));
            Assert.That(rumble.ScreenShake, Is.EqualTo(ScreenShakeIntensities.One));
        }

        [Test]
        public void CalculateRumbleFromDistanceSquaredAtTime_NoFalloffAtHalfDistance_FullRumble()
        {
            var rumbleInfo = ScriptableObject.CreateInstance<RumbleInfo>();
            rumbleInfo.Radius = 10.0f;
            rumbleInfo.RumbleSettings = Rumble.One;
            rumbleInfo.FalloffFunction = RumbleInfo.RumbleFalloffFunction.None;

            var halfDistanceSquared = Mathf.Pow(rumbleInfo.Radius * 0.5f, 2);
            var rumble = RumbleInfo.CalculateRumbleFromDistanceSquaredAtTime(rumbleInfo, halfDistanceSquared, 0.0f);

            Assert.That(rumble.ForceFeedback, Is.EqualTo(ForceFeedbackIntensities.One));
            Assert.That(rumble.ScreenShake, Is.EqualTo(ScreenShakeIntensities.One));
        }

        [Test]
        public void CalculateRumbleFromDistanceSquaredAtTime_NoFalloffOutOfRange_ZeroRumble()
        {
            var rumbleInfo = ScriptableObject.CreateInstance<RumbleInfo>();
            rumbleInfo.Radius = 10.0f;
            rumbleInfo.RumbleSettings = Rumble.One;
            rumbleInfo.FalloffFunction = RumbleInfo.RumbleFalloffFunction.None;

            var distanceSquared = (rumbleInfo.Radius * rumbleInfo.Radius) + 1.0f;
            var rumble = RumbleInfo.CalculateRumbleFromDistanceSquaredAtTime(rumbleInfo, distanceSquared, 0.0f);

            Assert.That(rumble.ForceFeedback, Is.EqualTo(ForceFeedbackIntensities.Zero));
            Assert.That(rumble.ScreenShake, Is.EqualTo(new ScreenShakeIntensities(Vector2.zero)));
        }

        [Test]
        public void CalculateRumbleFromDistanceSquaredAtTime_LinearFalloffCentered_FullRumble()
        {
            var rumbleInfo = ScriptableObject.CreateInstance<RumbleInfo>();
            rumbleInfo.Radius = 10.0f;
            rumbleInfo.RumbleSettings = Rumble.One;
            rumbleInfo.FalloffFunction = RumbleInfo.RumbleFalloffFunction.Linear;

            var rumble = RumbleInfo.CalculateRumbleFromDistanceSquaredAtTime(rumbleInfo, 0.0f, 0.0f);

            Assert.That(rumble.ForceFeedback, Is.EqualTo(ForceFeedbackIntensities.One));
            Assert.That(rumble.ScreenShake, Is.EqualTo(ScreenShakeIntensities.One));
        }

        [Test]
        public void CalculateRumbleFromDistanceSquaredAtTime_LinearFalloffAtHalfDistance_HalfRumble()
        {
            var rumbleInfo = ScriptableObject.CreateInstance<RumbleInfo>();
            rumbleInfo.Radius = 10.0f;
            rumbleInfo.RumbleSettings = Rumble.One;
            rumbleInfo.FalloffFunction = RumbleInfo.RumbleFalloffFunction.Linear;

            var halfDistanceSquared = Mathf.Pow(rumbleInfo.Radius * 0.5f, 2);
            var rumble = RumbleInfo.CalculateRumbleFromDistanceSquaredAtTime(rumbleInfo, halfDistanceSquared, 0.0f);

            Assert.That(rumble.ForceFeedback, Is.EqualTo( new ForceFeedbackIntensities(0.5f, 0.5f)));
            Assert.That(rumble.ScreenShake, Is.EqualTo(new ScreenShakeIntensities(new Vector2(0.5f, 0.5f), 1)));
        }

        [Test]
        public void CalculateRumbleFromDistanceSquaredAtTime_LinearFalloffOutOfRange_ZeroRumble()
        {
            var rumbleInfo = ScriptableObject.CreateInstance<RumbleInfo>();
            rumbleInfo.Radius = 10.0f;
            rumbleInfo.RumbleSettings = Rumble.One;
            rumbleInfo.FalloffFunction = RumbleInfo.RumbleFalloffFunction.Linear;

            var distanceSquared = (rumbleInfo.Radius * rumbleInfo.Radius) + 1.0f;
            var rumble = RumbleInfo.CalculateRumbleFromDistanceSquaredAtTime(rumbleInfo, distanceSquared, 0.0f);

            Assert.That(rumble.ForceFeedback, Is.EqualTo(ForceFeedbackIntensities.Zero));
            Assert.That(rumble.ScreenShake, Is.EqualTo(new ScreenShakeIntensities(Vector2.zero)));
        }

        [Test]
        public void CalculateRumbleFromDistanceSquaredAtTime_ExponentialFalloffCentered_FullRumble()
        {
            var rumbleInfo = ScriptableObject.CreateInstance<RumbleInfo>();
            rumbleInfo.Radius = 10.0f;
            rumbleInfo.RumbleSettings = Rumble.One;
            rumbleInfo.FalloffFunction = RumbleInfo.RumbleFalloffFunction.Exponential;

            var rumble = RumbleInfo.CalculateRumbleFromDistanceSquaredAtTime(rumbleInfo, 0.0f, 0.0f);

            Assert.That(rumble.ForceFeedback, Is.EqualTo(ForceFeedbackIntensities.One));
            Assert.That(rumble.ScreenShake, Is.EqualTo(ScreenShakeIntensities.One));
        }

        [Test]
        public void CalculateRumbleFromDistanceSquaredAtTime_ExponentialFalloffAtHalfDistance_QuarterRumble()
        {
            var rumbleInfo = ScriptableObject.CreateInstance<RumbleInfo>();
            rumbleInfo.Radius = 10.0f;
            rumbleInfo.RumbleSettings = Rumble.One;
            rumbleInfo.FalloffFunction = RumbleInfo.RumbleFalloffFunction.Exponential;

            var halfDistanceSquared = Mathf.Pow(rumbleInfo.Radius * 0.5f, 2);
            var rumble = RumbleInfo.CalculateRumbleFromDistanceSquaredAtTime(rumbleInfo, halfDistanceSquared, 0.0f);

            Assert.That(rumble.ForceFeedback, Is.EqualTo( new ForceFeedbackIntensities(0.25f, 0.25f)));
            Assert.That(rumble.ScreenShake, Is.EqualTo(new ScreenShakeIntensities(new Vector2(0.25f, 0.25f), 1)));
        }

        [Test]
        public void CalculateRumbleFromDistanceSquaredAtTime_ExponentialFalloffOutOfRange_ZeroRumble()
        {
            var rumbleInfo = ScriptableObject.CreateInstance<RumbleInfo>();
            rumbleInfo.Radius = 10.0f;
            rumbleInfo.RumbleSettings = Rumble.One;
            rumbleInfo.FalloffFunction = RumbleInfo.RumbleFalloffFunction.Exponential;

            var distanceSquared = (rumbleInfo.Radius * rumbleInfo.Radius) + 1.0f;
            var rumble = RumbleInfo.CalculateRumbleFromDistanceSquaredAtTime(rumbleInfo, distanceSquared, 0.0f);

            Assert.That(rumble.ForceFeedback, Is.EqualTo(ForceFeedbackIntensities.Zero));
            Assert.That(rumble.ScreenShake, Is.EqualTo(new ScreenShakeIntensities(Vector2.zero)));
        }
    }
}