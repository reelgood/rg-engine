using SharpDX;
using SharpDX.Windows;
using SharpDX.DirectInput;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RGEngine
{
	public enum MouseButton
	{
		Left = 0,
		Right = 1,
		Middle = 2,
	}

	static public class Input
	{
		static private Keyboard _keyboard = null;
		static private Mouse _mouse = null;
		static private RenderForm _form = null;
		static private Vector2 _mousePositionVector2 = Vector2.Zero;

		// Container for keys that are currently being held down
		static private HashSet<Key> _keyPressed;

		// Container for keys that have been pressed down in a frame
		static private HashSet<Key> _keyDown;

		// Container for keys that have been released in a frame
		static private HashSet<Key> _keyUp;

		// Container for mouse buttons
		static private HashSet<int> _mouseHeld;
		static private HashSet<int> _mouseDown;
		static private HashSet<int> _mouseUp;

		// Gets the current location of the mouse
		static public System.Drawing.Point MousePositionPoint { get; private set; }
		static public Vector2 MousePosition
		{
			get
			{
				_mousePositionVector2.X = MousePositionPoint.X;
				_mousePositionVector2.Y = MousePositionPoint.Y;
				return _mousePositionVector2;
			}
		}

		public static bool GetKey(Key key) { return _keyPressed.Contains(key); }
		public static bool GetKeyDown(Key key) { return _keyDown.Contains(key); }
		public static bool GetKeyUp(Key key) { return _keyUp.Contains(key); }

		public static bool MouseButtonHeld(int button) { return _mouseHeld.Contains(button); }
		public static bool MouseButtonDown(int button) { return _mouseDown.Contains(button); }
		public static bool MouseButtonUp(int button) { return _mouseUp.Contains(button); }

		public static bool MouseButtonHeld(MouseButton button) { return _mouseHeld.Contains((int)button); }
		public static bool MouseButtonDown(MouseButton button) { return _mouseDown.Contains((int)button); }
		public static bool MouseButtonUp(MouseButton button) { return _mouseUp.Contains((int)button); }

		internal static void Initialize(RenderForm form)
		{
			_form = form;
			_keyPressed = new HashSet<Key>();
			_keyUp = new HashSet<Key>();
			_keyDown = new HashSet<Key>();

			_mouseHeld = new HashSet<int>();
			_mouseDown = new HashSet<int>();
			_mouseUp = new HashSet<int>();

			DirectInput directInput = new DirectInput();

			_keyboard = new Keyboard(directInput);
			//_keyboard.SetCooperativeLevel(form.Handle, CooperativeLevel.Exclusive | CooperativeLevel.Foreground );
			_keyboard.Properties.BufferSize = 128;

			_mouse = new Mouse(directInput);
			_mouse.Properties.AxisMode = DeviceAxisMode.Relative;
			_mouse.Properties.BufferSize = 128;
			//_mouse.SetCooperativeLevel(form.Handle, CooperativeLevel.Foreground | CooperativeLevel.NonExclusive);

			form.MouseMove += new MouseEventHandler(form_MouseMove);
			MousePositionPoint = form.PointToClient(Cursor.Position);
		}

		/// <summary>
		/// Gathers input from connected devices
		/// </summary>
		internal static void GatherInput()
		{
			// Return if the window does not have focus
			//if (_form == null) return;

			if (!_form.Focused)
			{
				_keyUp.Clear();
				_keyDown.Clear();
				_keyPressed.Clear();

				_mouseDown.Clear();
				_mouseUp.Clear();
				_mouseHeld.Clear();
				return;
			}

			// Poll keyboard
			_keyboard.Acquire();
			_keyboard.Poll();
			ParseKeyboardData(_keyboard.GetBufferedData());
		
			// Poll mouse
			_mouse.Acquire();
			_mouse.Poll();
			ParseMouseData(_mouse.GetBufferedData());
		}


		private static void ParseKeyboardData(KeyboardUpdate[] data)
		{
			_keyUp.Clear();
			_keyDown.Clear();
			for (int i = 0; i < data.Length; i++)
			{
				if (data[i].IsPressed) HandleKeyDown(data[i].Key);
				if (data[i].IsReleased) HandleKeyUp(data[i].Key);
			}
		}

		private static void HandleKeyDown(Key key)
		{
			_keyDown.Add(key);
			_keyPressed.Add(key);
		}

		private static void HandleKeyUp(Key key)
		{
			_keyUp.Add(key);
			_keyPressed.Remove(key);
		}

		private static void ParseMouseData(MouseUpdate[] data)
		{
			_mouseDown.Clear();
			_mouseUp.Clear();

			for (int i = 0; i < data.Length; i++)
			{
				if (data[i].IsButton)
				{
					int buttonVal = int.Parse(data[i].Offset.ToString().Replace("Buttons", ""));
					if (data[i].Value != 0) HandleMouseDown(buttonVal); // Pressed
					if (data[i].Value == 0) HandleMouseUp(buttonVal); // Released
				}
			}
		}

		private static void HandleMouseDown(int button)
		{
			_mouseDown.Add(button);
			_mouseHeld.Add(button);
		}

		private static void HandleMouseUp(int button)
		{
			_mouseUp.Add(button);
			_mouseHeld.Remove(button);
		}

		private static void form_MouseMove(object sender, MouseEventArgs e)
		{
			if (!_form.Focused) return;
			MousePositionPoint = e.Location;
		}
	}
}
