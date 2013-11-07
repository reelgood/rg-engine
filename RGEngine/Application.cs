using System;
using SharpDX;
using SharpDX.Direct3D9;

namespace RGEngine
{
	public class Application : gameController
	{
		private GraphicSystem _graphics;
        private GameObjectController _gob_controller;
				
		public void Run()
		{
			_graphics = new GraphicSystem(_mainLoop);
			Initialize();
			_gob_controller = new GameObjectController();


			_graphics.StartRenderLoop(); // MUST ALWAYS BE LAST!
		}


		private void Initialize()
		{
			Time.Initialize();
			Debug.Initialize(_graphics.graphicDevice);
			Input.Initialize(_graphics.renderForm);
		}

		private void _mainLoop()
		{
			Hook.RunStartMethods();
			Time.Update();
			Input.GatherInput();
			Hook.DoUpdate();
		}

		protected void Exit()
		{
			_graphics.StopRenderLoop();
		}
	}
}
