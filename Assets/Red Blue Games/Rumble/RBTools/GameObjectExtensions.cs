namespace RedBlueGames.Tools
{
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Extensions to Unity's GameObject class
    /// </summary>
    public static class GameObjectExtensions
    {
        /// <summary>
        /// Determines if the GameObject is flagged as DontDestroyOnLoad
        /// </summary>
        /// <returns><c>true</c> if the object is flagged not to destroy on load; otherwise, <c>false</c>.</returns>
        /// <param name="obj">Object to test.</param>
        public static bool IsDontDestroyOnLoad(this GameObject obj)
        {
            obj.GetComponentsInChildren<DontDestroyOnLoad> (true);
            var dontDestroyOnLoadObjects = GameObject.FindObjectsOfType<DontDestroyOnLoad>();
            foreach (var dontDestroyOnLoad in dontDestroyOnLoadObjects)
            {
                if (dontDestroyOnLoad.transform == obj.transform)
                {
                    return true;
                }

                var children = dontDestroyOnLoad.transform.GetAllChildrenDepthFirst();
                if (children.Contains(obj.transform))
                {
                    return true;
                }
            }

            return false;
        }
    }
}