using System;
using SharpDX;
using SharpDX.Direct3D9;

namespace RGEngine
{
	public class Application : baseClass2
	{
		private GraphicSystem _graphics;
				
		public void Run()
		{
			_graphics = new GraphicSystem(_mainLoop);
			Initialize();
			_graphics.StartRenderLoop();

			Hook.AddMethod(Render, HookType.Render);
			Hook.AddMethod(OnGUI, HookType.GUI);
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
