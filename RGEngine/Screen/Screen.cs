using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RGEngine
{
    public class Screen : gameController
    {
        static Screen _activescreen;
        static List<Screen> _screenList = new List<Screen>();
        private bool _creationByActivation = false;
        public static Screen ActiveScreen { get { return _activescreen; } }

        /// <summary>
        /// Returns true if this is the active screen, otherwise false.
        /// </summary>
        public bool IsActive { get { return (this == _activescreen); } }
        /// <summary>
        /// Invoked when the screen is activated.
        /// The boolean is true if its the first time the screen activates
        /// </summary>
        public event Action<bool> OnActivate;
        /// <summary>
        /// Invoked when the screen gets deactivated from another screen getting activated.
        /// This is allways invoked if this is the active screen, and another screen is activated.
        /// This doesn't unload the screen, and on next activate, the OnActivate will pass false on its parameter.
        /// </summary>
        public event Action OnDeactivate;
        /// <summary>
        /// Invoked when the screen gets a message to unload itself.
        /// This will make the OnActivate invoke with a true next time the screen is activated
        /// </summary>
        public event Action OnUnload;

        /// <summary>
        /// Invoked whenever the screen changes. The parameter passes the activated screen as the first
        /// parameter, and the disabled screen as the second parameter.
        /// </summary>
        public static event Action<Screen,Screen> OnScreenChange;

        /// <summary>
        /// Returns the instance of the screentemplate. If the screen is not loaded, it will
        /// run the screens constructor, but not it's OnActivate.
        /// </summary>
        /// <typeparam name="ScreenTemplate">The screen to return an istance of</typeparam>
        /// <returns>A screen instance from the Screenlist</returns>
        public static Screen GetScreenInstance<ScreenTemplate>()
        {
            if (typeof(ScreenTemplate).Name == "Screen") { throw new InvalidCastException("Cannot fetch a non derived Screen object"); }
            if (!typeof(ScreenTemplate).IsSubclassOf(typeof(Screen))) { throw new InvalidCastException("Cannot fetch a class not derived from Screen"); }
            if (!_screenList.Any(s => s.GetType() == typeof(ScreenTemplate))) //The screen doesn't exists
            {
                LoadScreen<ScreenTemplate>();
            }
            return _screenList.First(s => s.GetType() == typeof(ScreenTemplate));
        }
        /// <summary>
        /// Activates a screen by type. This will instantiate a new screen if there is none created from earlier
        /// The OnActivate will get invoked with a true parameter if this is the first time, and false if the screen
        /// is allready loaded.
        /// The OnActivate(true) can also be used instead of a constructor, but they basically do the same thing.
        /// </summary>
        /// <typeparam name="ScreenTemplate">The screen to activate</typeparam>
        internal static void ActivateScreen<ScreenTemplate>()
        {
            if (typeof(ScreenTemplate).Name == "Screen") { throw new InvalidCastException("Cannot activate a non derived Screen object"); }
            if (!typeof(ScreenTemplate).IsSubclassOf(typeof(Screen))) { throw new InvalidCastException("Cannot activate a class not derived from Screen"); }
            if (_screenList.Any(s => s.GetType() == typeof(ScreenTemplate))) //The screen exists
            {
                if (_activescreen.GetType() == typeof(ScreenTemplate)) //The activatescreen tries to activate the allready active screen... so... eh..
                {
                    return;
                }
                _screenList.ForEach(s => { if (s.GetType() == typeof(ScreenTemplate)) { s._activate(false); } });
            }
            else
            {
                Screen _made = Activator.CreateInstance(typeof(ScreenTemplate)) as Screen;
                _made._creationByActivation = true;
                _screenList.Add(_made);
                _made._activate(true);
            }
        }
        /// <summary>
        /// Loads, but doesn't activate a screen by type. This will instantiate a new screen if there is none created from earlier
        /// The OnActivate will not get invoked until ActivateScreen is called.
        /// </summary>
        /// <typeparam name="ScreenTemplate">The screen to activate</typeparam>
        internal static void LoadScreen<ScreenTemplate>()
        {
            if (typeof(ScreenTemplate).Name == "Screen") { throw new InvalidCastException("Cannot load a non derived Screen object"); }
            if (!typeof(ScreenTemplate).IsSubclassOf(typeof(Screen))) { throw new InvalidCastException("Cannot load a class not derived from Screen"); }
            if (_screenList.Any(s => s.GetType() == typeof(ScreenTemplate))) //The screen exists
            {
                //No need to load a screen that allready exists.
                return;
            }
            else
            {
                Screen _made = Activator.CreateInstance(typeof(ScreenTemplate)) as Screen;
                _made._creationByActivation = true;
                _screenList.Add(_made);
            }
        }
        internal static bool UnloadScreen<ScreenTemplate>()
        {
            if (typeof(ScreenTemplate).Name == "Screen") { throw new InvalidCastException("Cannot unload a non derived Screen object"); }
            if (!typeof(ScreenTemplate).IsSubclassOf(typeof(Screen))) { throw new InvalidCastException("Cannot unload a class not derived from Screen"); }
            if (_screenList.Any(s => s.GetType() == typeof(ScreenTemplate))) //The screen exists
            {
                if (_activescreen.GetType() == typeof(ScreenTemplate)) //The UnloadScreen tries to unload the allready active screen... so... eh.. no...
                {
                    return false;
                }
                for (int i = 0; i < _screenList.Count; i++)
                {
                    if (_screenList[i].GetType() == typeof(ScreenTemplate))
                    {
                        _screenList[i]._unload();
                        _screenList[i] = null; //Is this necessary ?
                        _screenList.RemoveAt(i);
                        return true;
                    }
                }
            }
            return false;
        }
        public static implicit operator string(Screen d)
        {
            return "Why whould you cast this to a string?!?";
        }
        void _activate(bool first_time_activation)
        {
            if (!_creationByActivation)
            {
                //The screen was created by a new statement somewhere else than Screen.ActivateScreen<T>
                if (_screenList.Any(s => s.GetType() == this.GetType())) //The screen allready exists
                {
                    throw new OverflowException(this.GetType().ToString() + " instantiated twice");
                }
                _screenList.Add(this);
                _creationByActivation = true;
            }
            if (OnScreenChange != null && _activescreen != null) { OnScreenChange.Invoke(this, _activescreen); }
            _activescreen = this;
            if (OnActivate != null) { OnActivate.Invoke(first_time_activation); }
        }
        void _deactivate()
        {
            if (OnDeactivate != null) { OnDeactivate.Invoke(); }
        }
        void _unload()
        {
            if (OnUnload != null) { OnUnload.Invoke(); }
        }
    }
}
