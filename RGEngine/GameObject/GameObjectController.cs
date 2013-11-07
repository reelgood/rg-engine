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
            Screen.OnScreenChange += Screen_OnScreenChange;
		}
        /// <summary>
        /// Places the gameobject in the Controller overview, which allows it to be updated, GUId, and Renderd
        /// The Object gets placed on the currently active screen
        /// </summary>
        /// <param name="gameObject">The gameobject to controll</param>
        /// <returns>Returns false if the gameobject is allready being updated by the controller, othervise true</returns>
        internal bool UpdateMe(GameObject gameObject)
        {
            return UpdateMe(gameObject, Screen.ActiveScreen);
        }
        /// <summary>
        /// Places the gameobject in the Controller overview, which allows it to be updated, GUId, and Renderd
        /// You specify what screen you want the gameobject put on
        /// </summary>
        /// <param name="gameObject">The gameobject to controll</param>
        /// <param name="PutOnScreen"></param>
        /// <returns>Returns false if the gameobject is allready being updated by the controller, othervise true</returns>
        internal bool UpdateMe(GameObject gameObject, Screen PutOnScreen)
        {
            if (_gol.Any(go => go == gameObject)) { return false; }
            gameObject._hookedScreen = PutOnScreen;
            gameObject._screenActive = PutOnScreen.IsActive;
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
        void Screen_OnScreenChange(Screen activated_screen, Screen deactivated_screen)
        {
            _gol.ForEach(go =>
            {
                if (go._hookedScreen == deactivated_screen) { go._screenActive = false; }
                if (go._hookedScreen == activated_screen) { go._screenActive = true; }
            });
        }
        protected override void Update()
        {
            //Will execute _update on all gameobjects that have _screenActive or _keepUpdatingWhenScreenDisabled
            var items = _gol.Where(go => go._screenActive || go._keepUpdatingWhenScreenDisabled);
            foreach (var go in items)
            {
                go._update();
            }

        }
        protected override void GUI()
        {
            //Will execute _update on all gameobjects that have _screenActive
            //NOTE: will not execute render even though _keepUpdatingWhenScreenDisabled is true
            var items = _gol.Where(go => go._screenActive);
            foreach (var go in items)
            {
                go._gui();
            }

        }
        protected override void Render()
        {
            //Will execute _update on all gameobjects that have _screenActive
            //NOTE: will not execute render even though _keepUpdatingWhenScreenDisabled is true
            var items = _gol.Where(go => go._screenActive);
            foreach (var go in items)
            {
                go._render();
            }
        }
    }
}
