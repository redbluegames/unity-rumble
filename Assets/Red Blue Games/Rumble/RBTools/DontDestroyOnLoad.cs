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
    /// Flags the object not to destroy on load. This is mostly used for containers of objects
    /// that are already flagged DontDestroyOnLoad (such as Singletons)
    /// </summary>
    public class DontDestroyOnLoad : MonoBehaviour
    {
        protected void Awake()
        {
            GameObject.DontDestroyOnLoad(this.gameObject);
        }
    }
}
