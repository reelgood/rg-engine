using System;
using SharpDX;
using SharpDX.Windows;
using SharpDX.Direct3D9;

namespace RGEngine
{
	public class GraphicSystem : gameElement
	{
		public event Action OnDeviceResetStart;
		public event Action<Device> OnDeviceResetDone;

		public readonly int DefaultScreenWidth = 800;
		public readonly int DefaultScreenHeight = 600;

		private string _windowCaption = "MyApp";
		private GraphicsDevice  _graphicsDevice;
		private RenderForm _renderForm;
		private RenderLoop.RenderCallback _logicMethod;
		private RenderLoop.RenderCallback _renderMethod;

		public Device Device { get { return _graphicsDevice.Device; } }
		public Direct3D Direct3D { get { return _graphicsDevice.Direct3D; } }
		public RenderForm Window { get { return _renderForm; } }
		public string WindowCaption { get { return _windowCaption; } set { _windowCaption = value; if(_renderForm != null) _renderForm.Text = value; } }

		private bool _settingsChanged = false;
		private int _screenWidth;
		private int _screenHeight;
		private bool _windowed;

		internal bool _callReset = false;

		public bool Windowed
		{
			get { return _windowed; }
			set 
			{
				if (_windowed = !value) _settingsChanged = true;
				_windowed = value;
			}
		}
		
		public int ScreenHeight
		{
			get { return _screenHeight; }
			set 
			{
				if (_screenHeight != value) _settingsChanged = true;
				_screenHeight = value; 
			}
		}

		public int ScreenWidth
		{
			get { return _screenWidth; }
			set 
			{
				if (_screenWidth != value) _settingsChanged = true;
				_screenWidth = value;
			}
		}

		public GraphicSystem(RenderLoop.RenderCallback LogicMethod, RenderLoop.RenderCallback RenderMethod)
		{
			_logicMethod = LogicMethod;
			_renderMethod = RenderMethod;

			_windowCaption = "MyApp";
			_screenWidth = DefaultScreenWidth;
			_screenHeight = DefaultScreenHeight;
			_windowed = true;

			SetupRenderForm();
			
			_graphicsDevice = new GraphicsDevice(this);
			_graphicsDevice.CreateDevice(_renderForm.Handle);
		}
		
		internal void StartRenderLoop()
		{
			RenderLoop.Run(_renderForm, loop);
			
		}

		private void loop()
		{
			_logicMethod();
			_renderMethod();
		}

		internal void StopRenderLoop()
		{
			_renderForm.Close();
		}


		private void SetupRenderForm()
		{
			_renderForm = new RenderForm();
			_renderForm.AllowUserResizing = false;
			_renderForm.ClientSize = new System.Drawing.Size(_screenWidth, _screenHeight);
		}


		private bool isResetting = false;
		public void ApplyChanges()
		{
			if (isResetting == false)
			{
				Hook.AddEndMethod(_resetAndUpdate);
				_settingsChanged = false;
				isResetting = true;
			}
		}

		internal void _resetAndUpdate()
		{
			_renderForm.ClientSize = new System.Drawing.Size(_screenWidth, _screenHeight);
			PresentParameters par = new PresentParameters(_screenWidth, _screenHeight) { Windowed = _windowed };

			OnDeviceResetStart();
			_graphicsDevice.Device.Reset(par);
			OnDeviceResetDone(_graphicsDevice.Device);

			isResetting = false;
		}
	}
}
