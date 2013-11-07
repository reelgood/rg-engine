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
		RenderForm _renderForm;

		RenderLoop.RenderCallback _logicMethod;
		RenderLoop.RenderCallback _renderMethod;

		public Device graphicDevice { get { return _graphicsDevice.Device; } }
		public RenderForm renderForm { get { return _renderForm; } }

		public GraphicSystem(RenderLoop.RenderCallback LogicMethod, RenderLoop.RenderCallback RenderMethod)
		{
			_logicMethod = LogicMethod;
			_renderMethod = RenderMethod;
			_renderForm = new RenderForm();
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



		private void _renderLoop()
		{



		}


		internal void StopRenderLoop()
		{
			_renderForm.Close();
		}
	}
}
