using System;
using SharpDX;
using SharpDX.Windows;
using SharpDX.Direct3D9;

namespace RGEngine
{
	public class GraphicSystem : baseClass1
	{
		GraphicsDevice _graphicsDevice;
		RenderForm _form;

		RenderLoop.RenderCallback _mainLoop;

		public GraphicSystem(RenderLoop.RenderCallback mainLoopMethod)
		{
			_mainLoop = mainLoopMethod;
			_form = new RenderForm();
			
		}

		public void Start()
		{
			RenderLoop.Run(_form, _mainLoop);
		}

		public void test()
		{

		}
	}
}
