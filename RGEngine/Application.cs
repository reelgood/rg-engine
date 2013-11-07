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
			_graphics.StartRenderLoop();
            _gob_controller = new GameObjectController();

			Hook.AddMethod(Render, HookType.Render);
			Hook.AddMethod(GUI, HookType.GUI);
		}


		private void Initialize()
		{
			Time.Initialize();
			Debug.Initialize(_graphics.graphicDevice);
			Input.Initialize(_graphics.renderForm);
		}

		private void _mainLoop()
		{
			// TODO: Run start functions here

			Time.Update();
			Input.GatherInput();
			Update();
			Hook.DoUpdate();
		}



		protected void Exit()
		{
			_graphics.StopRenderLoop();
		}
	}
}
