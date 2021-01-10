using NUnit.Framework;
using UnityEngine;

namespace RedBlueGames.Rumble.Tests
{
    public class CircleBoundsTests
    {
        [Test]
        public void GetPercentFromCenter_AtCenter_IsZero()
        {
            var bounds = new CircleBounds(Vector3.one, 10.0f);
            var percentFromCenter = bounds.GetPercentFromCenter(Vector3.one);
            Assert.That(percentFromCenter, Is.EqualTo(0.0f));
        }
        
        [Test]
        public void GetPercentFromCenter_AtRadius_IsOne()
        {
            var radius = 10.0f;
            var startPosition = Vector3.zero;
            var bounds = new CircleBounds(startPosition, radius);
            var percentFromCenter = bounds.GetPercentFromCenter(
                startPosition + new Vector3(1.0f, 0.0f, 0.0f) * radius);
            Assert.That(percentFromCenter, Is.EqualTo(1.0f));
        }
        
        [Test]
        public void GetPercentFromCenter_OutOfBounds_IsGreaterThanOne()
        {
            var radius = 10.0f;
            var startPosition = Vector3.zero;
            var bounds = new CircleBounds(startPosition, radius);
            var percentFromCenter = bounds.GetPercentFromCenter(
                startPosition + new Vector3(1.0f, 0.0f, 0.0f) * radius * 2.0f);
            Assert.That(percentFromCenter, Is.EqualTo(2.0f));
        }
    }
}
