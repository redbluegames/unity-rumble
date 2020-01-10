/*****************************************************************************
 *  Copyright (C) 2014-2015 Red Blue Games, LLC
 *  
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 2 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
 ****************************************************************************/

namespace RedBlueGames.Tools
{
    using UnityEngine;

    /// <summary>
    /// Code grabbed from http://wiki.unity3d.com/index.php/Singleton
    /// Be aware this will not prevent a non singleton constructor
    ///   such as `T myT = new T();`
    /// To prevent that, add `protected T () {}` to your singleton class.
    /// As a note, this is made as MonoBehaviour because we need Coroutines.
    /// </summary>
    /// <typeparam name="T">Underlying type for the Singleton</typeparam>
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static bool applicationIsQuitting = false;
        private static T instance;

        /// <summary>
        /// Gets the instance for the Singleton
        /// </summary>
        /// <value>The instance.</value>
        public static T Instance
        {
            get
            {
                return instance;
            }
        }

        /// <summary>
        /// Initialize members and other initial data for the object. Called from Awake.
        /// </summary>
        protected virtual void Awake()
        {
            if (Instance != null)
            {
                var error = string.Format(
                                "Singleton [{0}] trying to assign itself as the Instance," +
                                "but an Instance [{1}] is already assigned. Destroying new singleton",
                                this.name,
                                Instance.name);
                Debug.LogError(error, this);

                GameObject.Destroy(this);
                return;
            }

            instance = (T)this.GetComponent<T>();
        }

        /// <summary>
        /// Unity's Destroy message, called when objects are destroyed in the scene
        /// </summary>
        protected void OnDestroy()
        {
            if (!applicationIsQuitting)
            {
                if (this.gameObject.IsDontDestroyOnLoad())
                {
                    var error = string.Format(
                                    "Singleton [{0}] is being destroyed, but the game is not quitting." +
                                    " Singletons should never be destroyed during regular app flow.",
                                    this.name);
                    Debug.LogError(error, this);
                }

                instance = null;
            }
        }

        /// <summary>
        /// When Unity quits, it destroys objects in a random order.
        /// If any script calls Instance after it have been destroyed, 
        ///   it will create a buggy ghost object that will stay on the Editor scene
        ///   even after stopping playing the Application. Really bad!
        /// So, this was made to be sure we're not creating that buggy ghost object.
        /// </summary>
        protected virtual void OnApplicationQuit()
        {
            applicationIsQuitting = true;
        }
    }
}