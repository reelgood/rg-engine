using SharpDX.Direct3D9;

namespace RGEngine
{
	public class GraphicsDevice : baseClass1
	{
		Device _device;
		Direct3D _direct3D;

		public GraphicsDevice(System.IntPtr FormHandle)
		{
			_direct3D = new Direct3D();
			_device = new Device(_direct3D, 0, DeviceType.Hardware, FormHandle, CreateFlags.HardwareVertexProcessing, new PresentParameters(800, 600) { Windowed = true });

			
		}
	}
}
