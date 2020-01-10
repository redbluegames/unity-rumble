namespace RedBlueGames.Rumble
{
    using UnityEngine;

    public class ScreenShakeLogger : IScreenShakeResponder
    {
        public void ApplyScreenShake(ScreenShakeIntensities intensities)
        {
            Debug.Log($"Applying Screen Shake: {intensities}");
        }
    }
}