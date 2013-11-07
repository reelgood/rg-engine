using System;
using SharpDX.Direct3D9;

namespace RGEngine
{
	public class GraphicsDevice : gameElement
	{
		private Device _device;
		private Direct3D _direct3D;
		private GraphicSystem _graphicsSystem;
		private PresentParameters _presentParameters;

		public Device Device { get { return _device; } }
		public Direct3D Direct3D { get { return _direct3D; } }

		internal GraphicsDevice(GraphicSystem graphicSystem)
		{
			_graphicsSystem = graphicSystem;
			_direct3D = new Direct3D();
			_presentParameters = new PresentParameters();
			_presentParameters.InitDefaults();
			_presentParameters.BackBufferWidth = graphicSystem.DefaultScreenWidth;
			_presentParameters.BackBufferHeight = graphicSystem.DefaultScreenHeight;
			_presentParameters.Windowed = true;

			ToDispose(_device);
			ToDispose(_direct3D);
		}

		internal void CreateDevice(IntPtr FormHandle)
		{
			_device = new Device(_direct3D, 0, DeviceType.Hardware, FormHandle, 
				CreateFlags.HardwareVertexProcessing, _presentParameters);
		}

		internal void CreateDevice(IntPtr FormHandle, PresentParameters PresentParams)
		{
			if (_device !=null) 
			{
				_device.Dispose();
				_device = null;
			}
			_device = new Device(_direct3D, 0, DeviceType.Hardware, FormHandle,
				CreateFlags.HardwareVertexProcessing, PresentParams);
		}
	}
}
