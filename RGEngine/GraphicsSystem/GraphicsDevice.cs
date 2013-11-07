using System;
using SharpDX.Direct3D9;

namespace RGEngine
{
	public class GraphicsDevice : baseClass1
	{
		Device _device;
		Direct3D _direct3D;
		GraphicSystem _graphicsSystem;
		PresentParameters _presentParameters;

		public Device Device { get { return _device; } }
		public Direct3D Direct3D { get { return _direct3D; } }

		internal GraphicsDevice(GraphicSystem graphicSystem)
		{
			_direct3D = new Direct3D();
			_graphicsSystem = graphicSystem;
			_presentParameters = new PresentParameters();
			_presentParameters.InitDefaults();
			_presentParameters.BackBufferWidth = graphicSystem.DefaultScreenWidth;
			_presentParameters.BackBufferHeight = graphicSystem.DefaultScreenHeight;
		}

		internal void CreateDevice(IntPtr FormHandle)
		{
			_device = new Device(_direct3D, 0, DeviceType.Hardware, FormHandle, 
				CreateFlags.HardwareVertexProcessing, _presentParameters);
		}


	}
}
