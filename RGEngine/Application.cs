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

			// Always call StartRenderLoop last
			_graphics.StartRenderLoop();
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

		private void _graphicLoop()
		{

		}


		protected void Exit()
		{
			_graphics.StopRenderLoop();
		}
	}
}
