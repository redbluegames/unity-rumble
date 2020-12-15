using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace RedBlueGames.Rumble.Tests
{
    public class RumbleManagerTests
    {
        private RumbleManager rumbleManager;
        
        [UnitySetUp]
        public IEnumerator UnitySetup()
        {
            var asyncOperation = SceneManager.LoadSceneAsync("RumbleManagerTests", LoadSceneMode.Single);
            yield return new WaitUntil( () => asyncOperation.isDone);
            
            var rumbleManagerGameObject = new GameObject("RumbleManager");
            rumbleManager = rumbleManagerGameObject.AddComponent<RumbleManager>();
         }
        
        [Test]
        public void StartRumble_FirstRumble_ReturnsOneRumbleSource()
        {
            var info = ScriptableObject.CreateInstance<RumbleInfo>();
            info.Lifetime = 1.0f;
            
            var source = rumbleManager.StartRumble(Vector3.zero, info);

            Assert.That(source, Is.Not.Null);
            Assert.That(
                rumbleManager.ActiveRumbleSources.Count, Is.EqualTo(1),
                "Expected one rumble source outstanding but there were not.");
        }
        
        [UnityTest]
        public IEnumerator DestroyRumble_OneValidRumble_ZeroActive()
        {
            var info = ScriptableObject.CreateInstance<RumbleInfo>();
            info.Lifetime = 1.0f;
            
            var source = rumbleManager.StartRumble(Vector3.zero, info);
            yield return new WaitForSeconds(info.Lifetime * 0.1f);
            rumbleManager.DestroyRumble(source);

            Assert.That(
                rumbleManager.ActiveRumbleSources.Count, Is.EqualTo(0),
                "Expected zero outstanding rumble sources but there were not");
        }
        
        [UnityTest]
        public IEnumerator Update_ActiveSources_DestroyAfterLifetime()
        {
            var info = ScriptableObject.CreateInstance<RumbleInfo>();
            info.Lifetime = 0.1f;
            
            rumbleManager.StartRumble(Vector3.zero, info);
            yield return new WaitForSeconds(info.Lifetime + 0.2f);

            Assert.That(
                rumbleManager.ActiveRumbleSources.Count, Is.EqualTo(0),
                "Expected zero outstanding rumble sources but there were not");
        }
        
        [UnityTest]
        public IEnumerator ActiveRumbleSources_LifetimeCircumventedOnRumble_IsCleared()
        {
            var info = ScriptableObject.CreateInstance<RumbleInfo>();
            info.Lifetime = 0.1f;
            var rumble = rumbleManager.StartRumble(Vector3.zero, info);
            
            Object.Destroy(rumble.gameObject);
            yield return null;

            Assert.That(
                rumbleManager.ActiveRumbleSources.Count, Is.EqualTo(0),
                "Expected zero outstanding rumble sources but there were not");
        }
    }
}
