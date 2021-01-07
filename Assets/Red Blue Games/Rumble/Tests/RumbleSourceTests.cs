using NUnit.Framework;
using UnityEngine;

namespace RedBlueGames.Rumble.Tests
{
    public class RumbleSourceTests
    {
        private const float DefaultRadius = 10.0f; 
        private readonly RumbleAsset defaultRumbleAsset = ScriptableObject.CreateInstance<RumbleAsset>();

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
            CreateAndInitializeRumbleSource(defaultRumbleAsset, DefaultRadius);

            Assert.That(rumbleSource.IsAlive);
        }

        [Test]
        public void Initialize_DeadSource_IsAlive()
        {
            CreateAndInitializeRumbleSource(defaultRumbleAsset, DefaultRadius);
            KillRumbleSource();

            rumbleSource.Initialize(defaultRumbleAsset, 1.0f);

            Assert.That(rumbleSource.IsAlive);
        }

        [Test]
        public void IsAlive_QueriedWhileTimeRemains_IsTrue()
        {
            CreateAndInitializeRumbleSource(defaultRumbleAsset, DefaultRadius);

            // Not we only test that Alive and Dead are not equal in the IsAlive and IsDead tests, so that we can assume
            // it in other tests.
            Assert.That(rumbleSource.IsAlive);
            Assert.That(rumbleSource.IsDead, Is.False,
                "Expected RumbleSource to be Not Dead while time remains, but it is Dead.");
        }

        [Test]
        public void IsDead_WithZeroRemainingTime_IsTrue()
        {
            CreateAndInitializeRumbleSource(defaultRumbleAsset, DefaultRadius);
            KillRumbleSource();

            Assert.That(rumbleSource.IsAlive, Is.False,
                "Expected RumbleSource to be Not Alive when lifetime expired, but it is Alive.");
            Assert.That(rumbleSource.IsDead);
        }

        [Test]
        public void Tick_TimeRemains_StillAlive()
        {
            CreateAndInitializeRumbleSource(defaultRumbleAsset, DefaultRadius);

            rumbleSource.Tick(defaultRumbleAsset.Lifetime * 0.5f);

            Assert.That(rumbleSource.IsAlive);
        }

        [Test]
        public void Tick_TimeExpires_IsDead()
        {
            CreateAndInitializeRumbleSource(defaultRumbleAsset, DefaultRadius);

            rumbleSource.Tick(defaultRumbleAsset.Lifetime);

            Assert.That(rumbleSource.IsDead);
        }

        [Test]
        public void Tick_DeadSource_StillDead()
        {
            CreateAndInitializeRumbleSource(defaultRumbleAsset, DefaultRadius);
            KillRumbleSource();

            rumbleSource.Tick(defaultRumbleAsset.Lifetime);

            Assert.That(rumbleSource.IsDead);
        }

        [Test]
        public void EvaluateRumble_ListenerAtCenter_FullStrength()
        {
            var rumbleAsset = ScriptableObject.CreateInstance<RumbleAsset>();
            rumbleAsset.Lifetime = 1.0f;
            var expectedForceFeedback = new ForceFeedbackIntensities(1.0f, 0.7f);
            var expectedScreenShake = new ScreenShakeIntensities(Vector2.one, 5);
            rumbleAsset.Intensity = new RumbleIntensity(expectedForceFeedback, expectedScreenShake);

            CreateAndInitializeRumbleSource(rumbleAsset, 10.0f);

            var rumble = rumbleSource.EvaluateRumble(rumbleSource.transform.position);

            Assert.That(rumble.ForceFeedback, Is.EqualTo(expectedForceFeedback));
            Assert.That(rumble.ScreenShake, Is.EqualTo(expectedScreenShake));
        }

        [Test]
        public void EvaluateRumble_ListenerOutOfRange_ZeroRumble()
        {
            var rumbleAsset = ScriptableObject.CreateInstance<RumbleAsset>();
            rumbleAsset.Lifetime = 1.0f;
            var expectedForceFeedback = new ForceFeedbackIntensities(1.0f, 0.7f);
            var expectedScreenShake = new ScreenShakeIntensities(Vector2.one, 5);
            rumbleAsset.Intensity = new RumbleIntensity(expectedForceFeedback, expectedScreenShake);
            float sourceRadius = DefaultRadius;
            CreateAndInitializeRumbleSource(rumbleAsset, sourceRadius);

            var atEdgeOfRadius = rumbleSource.transform.position + Vector3.one.normalized * (sourceRadius + .01f);
            var rumble = rumbleSource.EvaluateRumble(atEdgeOfRadius);

            Assert.That(rumble.ForceFeedback, Is.EqualTo(ForceFeedbackIntensities.Zero));
            Assert.That(rumble.ScreenShake, Is.EqualTo(new ScreenShakeIntensities(Vector2.zero, 5)));
        }

        [Test]
        public void EvaluateRumble_ListenerHalfwayLinearFalloff_HalfRumble()
        {
            var rumbleAsset = ScriptableObject.CreateInstance<RumbleAsset>();
            rumbleAsset.Lifetime = 1.0f;
            rumbleAsset.Intensity = new RumbleIntensity(
                new ForceFeedbackIntensities(1.0f, 0.7f),
                new ScreenShakeIntensities(Vector2.one, 5));
            rumbleAsset.CurveDistanceModulator = new CurveDistanceModulator(
                AnimationCurve.Linear(0.0f, 1.0f, 1.0f, 0.0f));
            float sourceRadius = DefaultRadius;
            CreateAndInitializeRumbleSource(rumbleAsset, sourceRadius);

            var expectedForceFeedback = new ForceFeedbackIntensities(0.5f, 0.35f);
            var expectedScreenShake = new ScreenShakeIntensities(new Vector2(0.5f, 0.5f), 5);
            var halfRadiusFromSource = rumbleSource.transform.position + Vector3.one.normalized * sourceRadius * 0.5f;
            var rumble = rumbleSource.EvaluateRumble(halfRadiusFromSource);

            Assert.That(rumble.ForceFeedback, Is.EqualTo(expectedForceFeedback));
            Assert.That(rumble.ScreenShake, Is.EqualTo(expectedScreenShake));
        }

        private void CreateAndInitializeRumbleSource(RumbleAsset rumbleAsset, float radius)
        {
            var sourceGameObject = new GameObject("RumbleSource");
            rumbleSource = sourceGameObject.AddComponent<RumbleSource>();
            rumbleSource.Initialize(rumbleAsset, radius);
        }

        private void KillRumbleSource()
        {
            // Tick the rumbleIntensity source by its lifetime to make sure it dies correctly
            rumbleSource.Tick(defaultRumbleAsset.Lifetime);
        }
    }
}