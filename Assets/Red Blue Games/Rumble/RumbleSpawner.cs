using UnityEngine;

namespace RedBlueGames.Rumble
{
    public class RumbleSpawner : MonoBehaviour
    {
        [SerializeField] [Tooltip("The rumble info to bind to the spawned rumble.")]
        private RumbleInfo rumbleInfo;

        // Start is called before the first frame update
        void Start()
        {
            var rumble = RumbleManager.Instance.StartRumble(this.transform.position, this.rumbleInfo);
            rumble.transform.SetParent(this.transform);
        }
    }
}
