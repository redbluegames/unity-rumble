using UnityEngine;

namespace RedBlueGames.Rumble
{
    public interface IRumbleBounds
    {
        float GetPercentFromCenter(Vector3 position);

        void DrawGizmo();
    }
}