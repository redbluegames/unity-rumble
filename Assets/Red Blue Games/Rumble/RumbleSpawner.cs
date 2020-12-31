using UnityEngine;

namespace RedBlueGames.Rumble
{
    public class RumbleSpawner : MonoBehaviour
    {
        [SerializeField] [Tooltip("The rumbleIntensity info to bind to the spawned rumbleIntensity.")]
        private RumbleInfo rumbleInfo;

        // Start is called before the first frame update
        void Start()
        {
            var rumble = RumbleManager.Instance.StartRumble(this.transform.position, this.rumbleInfo);
            rumble.transform.SetParent(this.transform);
        }
    }
}
