using NUnit.Framework;
using RedBlueGames.Rumble;
using UnityEngine;

namespace RedBlueGames.Rumble.Tests
{
    public class RumbleIntensityTests
    {
        [Test]
        public void Equality_Copies_AreEqual()
        {
            // Arrange
            var rumbleA = new RumbleIntensity();
            rumbleA.ForceFeedback = new ForceFeedbackIntensities(1.0f, 0.25f);
            rumbleA.ScreenShake = new ScreenShakeIntensities(Vector2.one, 1);

            var rumbleB = new RumbleIntensity();
            rumbleB.ForceFeedback = new ForceFeedbackIntensities(
                rumbleA.ForceFeedback.LeftMotor,
                rumbleA.ForceFeedback.RightMotor);
            rumbleB.ScreenShake = new ScreenShakeIntensities(
                rumbleA.ScreenShake.Strength,
                rumbleA.ScreenShake.Vibrato);

            // Assert
            Assert.True(rumbleA.Equals(rumbleB), "Expected rumbleIntensity copies to be Equal but they are not.");
        }

        [Test]
        public void Equality_UnequalCopies_AreNotEqual()
        {
            // Arrange
            var rumbleA = new RumbleIntensity();
            rumbleA.ForceFeedback = new ForceFeedbackIntensities(1.0f, 0.0f);
            rumbleA.ScreenShake = new ScreenShakeIntensities(Vector2.one, 1);

            var rumbleB = new RumbleIntensity();
            rumbleB.ForceFeedback = new ForceFeedbackIntensities(
                rumbleA.ForceFeedback.LeftMotor,
                0.1f);
            rumbleB.ScreenShake = new ScreenShakeIntensities(
                rumbleA.ScreenShake.Strength,
                rumbleA.ScreenShake.Vibrato);

            // Assert
            Assert.False(rumbleA.Equals(rumbleB), "Expected unexact rumbleIntensity copies not to be Equal but they are.");
        }

        [Test]
        public void Add_NormalCase_Adds()
        {
            // Arrange
            var rumbleA = new RumbleIntensity();
            rumbleA.ForceFeedback = new ForceFeedbackIntensities(1.0f, 0.25f);
            rumbleA.ScreenShake = new ScreenShakeIntensities(Vector2.one, 1);

            var rumbleB = new RumbleIntensity();
            rumbleB.ForceFeedback = new ForceFeedbackIntensities(1.0f, 0.25f);
            rumbleB.ScreenShake = new ScreenShakeIntensities(new Vector2(3.0f, 3.0f), 1);

            var expectedAdd = new RumbleIntensity();
            expectedAdd.ForceFeedback = new ForceFeedbackIntensities(1.0f, 0.5f);
            expectedAdd.ScreenShake = new ScreenShakeIntensities(new Vector2(4.0f, 4.0f), 1);

            // Act

            var aPlusB = rumbleA + rumbleB;

            // Assert
            Assert.AreEqual(expectedAdd, aPlusB, "Expected Added Rumbles to be equal but they are not.");
        }

        [Test]
        public void Scale_NormalCase_Scales()
        {
            // Arrange
            var rumbleA = new RumbleIntensity();
            rumbleA.ForceFeedback = new ForceFeedbackIntensities(1.0f, 0.25f);
            rumbleA.ScreenShake = new ScreenShakeIntensities(Vector2.one, 1);

            float scalar = 0.5f;

            var expectedScaledRumble = new RumbleIntensity();
            expectedScaledRumble.ForceFeedback = new ForceFeedbackIntensities(0.5f, 0.125f);

            // for now only Strength is scaled for Screenshake
            expectedScaledRumble.ScreenShake = new ScreenShakeIntensities(new Vector2(0.5f, 0.5f), 1);

            // Act
            var aScaled = rumbleA * scalar;

            // Assert
            Assert.AreEqual(expectedScaledRumble, aScaled, "ExpectedScaledRumble did not match actual scaled rumbleIntensity.");
        }
    }
}
