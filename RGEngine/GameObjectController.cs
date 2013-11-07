using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RGEngine
{
    public class GameObjectController : gameController
    {
        private static GameObjectController _goc;
        private List<GameObject> _gol;
		public GameObjectController()
		{
            if (_goc != null) { throw new InvalidOperationException("GameObjectController created twice"); }
            _goc = this;
            _gol = new List<GameObject>();
		}
        /// <summary>
        /// Places the gameobject in the Controller overview, which allows it to be updated, GUId, and Renderd
        /// </summary>
        /// <param name="gameObject">The gameobject to controll</param>
        /// <returns>Returns false if the gameobject is allready being updated by the controller, othervise true</returns>
        internal bool UpdateMe(GameObject gameObject)
        {
            if (_gol.Any(go => go == gameObject)) { return false; }
            _gol.Add(gameObject);
            return true;
        }
        /// <summary>
        /// Removes the gameobject from the Controller overview, which disables its updated, GUId, and Renderd methods
        /// </summary>
        /// <param name="gameObject">The gameobject to remove</param>
        /// <returns>Returns false if the gameobject is not being updated by the controller, othervise true if removed successfully, BUDDY!</returns>
        internal bool ForgetMe(GameObject gameObject)
        {
            if (_gol.Any(go => go == gameObject))
            {
                _gol.Remove(gameObject);
                return true;
            }
            return false;
        }
        protected override void Update()
        {
            //LINQ, Where go.Enabled, select and run its _update
			_gol.ForEach(go => go._update());

        }
        protected override void GUI()
        {
            //LINQ, Where go.Enabled, select and run its _gui
			_gol.ForEach(go => go._gui());

        }
        protected override void Render()
        {
			_gol.ForEach(go => go._render());
        }
    }
}
