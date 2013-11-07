using System;
using SharpDX;
using SharpDX.Windows;
using SharpDX.Direct3D9;

namespace RGEngine
{
	public class GraphicSystem : gameElement
	{
		public readonly int DefaultScreenWidth = 800;
		public readonly int DefaultScreenHeight = 600;
		
		GraphicsDevice _graphicsDevice;
		RenderForm _form;
		RenderLoop.RenderCallback _mainLoop;

		public Device graphicDevice { get { return _graphicsDevice.Device; } }
		public RenderForm renderForm { get { return _form; } }


		public GraphicSystem(RenderLoop.RenderCallback mainLoopMethod)
		{
			_mainLoop = mainLoopMethod;
			_form = new RenderForm();

			_graphicsDevice = new GraphicsDevice(this);
			_graphicsDevice.CreateDevice(_form.Handle);
		}

		public GraphicSystem(Delegate mainLoopMethod, Delegate renderFunction)
		{
			_mainLoop = mainLoopMethod as RenderLoop.RenderCallback;
			_form = new RenderForm();

			_graphicsDevice = new GraphicsDevice(this);
			_graphicsDevice.CreateDevice(_form.Handle);
		}


		internal void StartRenderLoop()
		{
			RenderLoop.Run(_form, _renderLoop);
		}

		private void _renderLoop()
		{
			_mainLoop();

			Hook.DoPreRenderLoop();
			_graphicsDevice.Device.Clear(ClearFlags.ZBuffer | ClearFlags.Target, Color.Black, 0.0f, 0);
			_graphicsDevice.Device.BeginScene();

			Hook.DoRender();
			Hook.DoGUI();
			Debug.Print();

			_graphicsDevice.Device.EndScene();
			_graphicsDevice.Device.Present();
			Hook.DoPostRenderLoop();
		}


		internal void StopRenderLoop()
		{
			_form.Close();
		}
	}
}
