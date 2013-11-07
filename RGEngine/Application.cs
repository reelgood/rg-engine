using System;
using SharpDX;
using SharpDX.Direct3D9;

namespace RGEngine
{
	public class Application : gameController
	{
		private bool _isRunning;
		private GraphicSystem _graphics;

		public GraphicSystem Graphics { get { return _graphics; } }

		public void Run()
		{
			Controllers.GameObjectController = new GameObjectController();
			_graphics = new GraphicSystem(_logic, _render);
			Controllers.Application = this;
			Controllers.GraphicSystem = _graphics;
			_initialize();

			_isRunning = true;
			_graphics.StartRenderLoop(); // MUST ALWAYS BE LAST!
			_graphics.Dispose();		// sortoff
		}


		private void _initialize()
		{
			Time.Initialize();
			Debug.Initialize(_graphics.Device);
			Input.Initialize(_graphics.Window);
		}

		private void _logic()
		{
			Hook.RunStartMethods();
			Time.Update();
			Input.GatherInput();
			Hook.DoUpdate();
		}

		private void _render()
		{
			Hook.DoPreRenderLoop();
			_graphics.Device.Clear(ClearFlags.ZBuffer | ClearFlags.Target, Color.Black, 0.0f, 0);
			_graphics.Device.BeginScene();

			Hook.DoRender();
			Hook.DoGUI();
			Debug.Print();

			_graphics.Device.EndScene();
			_graphics.Device.Present();
			Hook.DoPostRenderLoop();
			
			Hook.RunEndMethods();
			if (!_isRunning) _graphics.StopRenderLoop();
		}

		protected void Exit()
		{
			_isRunning = false;
		}
	}
}
