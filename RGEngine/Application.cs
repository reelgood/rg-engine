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
			_init();


		}



		private void _init()
		{
			_graphics = new GraphicSystem(_mainLoop);
			_graphics.Start();
		}



		private void _mainLoop()
		{
			_graphics.test();
		}
	}
}
