using NUnit.Framework;
using UnityEngine;

namespace RedBlueGames.Rumble.Tests
{
    public class RumbleSourceTests
    {
        private readonly RumbleInfo defaultRumbleInfo = ScriptableObject.CreateInstance<RumbleInfo>();

        private RumbleSource rumbleSource;

        [TearDown]
        public void TearDown()
        {
            Object.Destroy(rumbleSource);
            rumbleSource = null;
        }

        [Test]
        public void Initialize_Default_IsAlive()
        {
            CreateAndInitializeRumbleSource(defaultRumbleInfo);

            Assert.That(rumbleSource.IsAlive);
        }

        [Test]
        public void Initialize_DeadSource_IsAlive()
        {
            CreateAndInitializeRumbleSource(defaultRumbleInfo);
            KillRumbleSource();

            rumbleSource.Initialize(defaultRumbleInfo);

            Assert.That(rumbleSource.IsAlive);
        }

        [Test]
        public void IsAlive_QueriedWhileTimeRemains_IsTrue()
        {
            CreateAndInitializeRumbleSource(defaultRumbleInfo);

            // Not we only test that Alive and Dead are not equal in the IsAlive and IsDead tests, so that we can assume
            // it in other tests.
            Assert.That(rumbleSource.IsAlive);
            Assert.That(rumbleSource.IsDead, Is.False,
                "Expected RumbleSource to be Not Dead while time remains, but it is Dead.");
        }

        [Test]
        public void IsDead_WithZeroRemainingTime_IsTrue()
        {
            CreateAndInitializeRumbleSource(defaultRumbleInfo);
            KillRumbleSource();

            Assert.That(rumbleSource.IsAlive, Is.False,
                "Expected RumbleSource to be Not Alive when lifetime expired, but it is Alive.");
            Assert.That(rumbleSource.IsDead);
        }

        [Test]
        public void Tick_TimeRemains_StillAlive()
        {
            CreateAndInitializeRumbleSource(defaultRumbleInfo);

            rumbleSource.Tick(defaultRumbleInfo.Lifetime * 0.5f);

            Assert.That(rumbleSource.IsAlive);
        }

        [Test]
        public void Tick_TimeExpires_IsDead()
        {
            CreateAndInitializeRumbleSource(defaultRumbleInfo);

            rumbleSource.Tick(defaultRumbleInfo.Lifetime);

            Assert.That(rumbleSource.IsDead);
        }

        [Test]
        public void Tick_DeadSource_StillDead()
        {
            CreateAndInitializeRumbleSource(defaultRumbleInfo);
            KillRumbleSource();

            rumbleSource.Tick(defaultRumbleInfo.Lifetime);

            Assert.That(rumbleSource.IsDead);
        }

        [Test]
        public void EvaluateRumble_ListenerAtCenter_FullStrength()
        {
            var expectedForceFeedback = new ForceFeedbackIntensities(1.0f, 0.7f);
            var expectedScreenShake = new ScreenShakeIntensities(Vector2.one, 5);
            var info = ScriptableObject.CreateInstance<RumbleInfo>();
            info.Radius = 10.0f;
            info.RumbleSettings = new Rumble(expectedForceFeedback, expectedScreenShake);
            CreateAndInitializeRumbleSource(info);

            var rumble = rumbleSource.EvaluateRumble(rumbleSource.transform.position);

            Assert.That(rumble.ForceFeedback, Is.EqualTo(expectedForceFeedback));
            Assert.That(rumble.ScreenShake, Is.EqualTo(expectedScreenShake));
        }

        [Test]
        public void EvaluateRumble_ListenerOutOfRange_ZeroRumble()
        {
            var info = ScriptableObject.CreateInstance<RumbleInfo>();
            info.Radius = 10.0f;
            info.RumbleSettings = new Rumble(ForceFeedbackIntensities.One, ScreenShakeIntensities.One);
            CreateAndInitializeRumbleSource(info);
            var atEdgeOfRadius = rumbleSource.transform.position + Vector3.one.normalized * (info.Radius + .01f);

            var rumble = rumbleSource.EvaluateRumble(atEdgeOfRadius);

            Assert.That(rumble.ForceFeedback, Is.EqualTo(ForceFeedbackIntensities.Zero));
            Assert.That(rumble.ScreenShake, Is.EqualTo(ScreenShakeIntensities.Zero));
        }

        [Test]
        public void EvaluateRumble_ListenerHalfwayLinearFalloff_HalfRumble()
        {
            var info = ScriptableObject.CreateInstance<RumbleInfo>();
            info.Radius = 10.0f;
            info.RumbleSettings = new Rumble(ForceFeedbackIntensities.One, ScreenShakeIntensities.One);
            info.FalloffFunction = RumbleInfo.RumbleFalloffFunction.Linear;
            CreateAndInitializeRumbleSource(info);
            var expectedForceFeedback = new ForceFeedbackIntensities(0.5f, 0.5f);
            var expectedScreenShake = new ScreenShakeIntensities(
                new Vector2(0.5f, 0.5f),
                ScreenShakeIntensities.One.Vibrato);
            var halfRadiusFromSource = rumbleSource.transform.position + Vector3.one.normalized * info.Radius * 0.5f;

            var rumble = rumbleSource.EvaluateRumble(halfRadiusFromSource);

            Assert.That(rumble.ForceFeedback, Is.EqualTo(expectedForceFeedback));
            Assert.That(rumble.ScreenShake, Is.EqualTo(expectedScreenShake));
        }

        private void CreateAndInitializeRumbleSource(RumbleInfo info)
        {
            var sourceGameObject = new GameObject("RumbleSource");
            rumbleSource = sourceGameObject.AddComponent<RumbleSource>();
            rumbleSource.Initialize(info);
        }

        private void KillRumbleSource()
        {
            // Tick the rumble source by its lifetime to make sure it dies correctly
            rumbleSource.Tick(defaultRumbleInfo.Lifetime);
        }
    }
}