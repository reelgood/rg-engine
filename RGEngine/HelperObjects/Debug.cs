using System;
using System.Collections.Generic;
using SharpDX;
using SharpDX.Direct3D9;

namespace RGEngine
{
	public static class Debug
	{
		private struct debugTexts
		{
			public string debugText;
			public float displayTime;
			public float startTime;
			public debugTexts(string Text, float DisplayTime, float StartTime)
			{
				this.debugText = Text;
				this.displayTime = DisplayTime;
				this.startTime = StartTime;
			}
		}

		static int _textHeight = 14;
		static private List<debugTexts> _debugLog;
		static Font _debugTextFont;
		static List<string> _debugTexts;
		static Device _device;

		static public int TextHeight
		{
			get { return _textHeight; }
			set
			{
				FontDescription fontDescription = new FontDescription()
				{
					Height = value,
					Italic = false,
					CharacterSet = FontCharacterSet.Ansi,
					FaceName = "Console",
					MipLevels = 0,
					OutputPrecision = FontPrecision.TrueType,
					PitchAndFamily = FontPitchAndFamily.Default,
					Quality = FontQuality.Antialiased,
					Weight = FontWeight.SemiBold
				};
				_debugTextFont = new Font(_device, fontDescription);
			}
		}

		static internal void Initialize(Device device)
		{
			_debugTexts = new List<string>();
			_debugLog = new List<debugTexts>();
			_device = device;

			// Initialize the Font
			FontDescription fontDescription = new FontDescription()
			{
				Height = _textHeight,
				Italic = false,
				CharacterSet = FontCharacterSet.Ansi,
				FaceName = "Console",
				MipLevels = 0,
				OutputPrecision = FontPrecision.TrueType,
				PitchAndFamily = FontPitchAndFamily.Default,
				Quality = FontQuality.Antialiased,
				Weight = FontWeight.SemiBold
			};
			_debugTextFont = new Font(_device, fontDescription);

			Controllers.GraphicSystem.OnDeviceResetStart += new Action(GraphicSystem_OnDeviceResetStart);
			Controllers.GraphicSystem.OnDeviceResetDone += new Action<Device>(GraphicSystem_OnDeviceResetDone);
		}

		static void GraphicSystem_OnDeviceResetDone(Device obj)
		{
			_debugTextFont.OnResetDevice();
		}

		static void GraphicSystem_OnDeviceResetStart()
		{
			_debugTextFont.OnLostDevice();
		}

		static public void Log(string str, float displayTime = 0)
		{
			_debugLog.Add(new debugTexts(str, displayTime, Time.realtimeSinceStartup));
		}

		static public void Log(object obj, float displayTime = 0)
		{
			Log(obj.ToString(), displayTime);
		}

		static internal void Print()
		{
			Queue<debugTexts> removeQueue = null;
			int textCount = 0;
			for (int i = 0; i < _debugLog.Count; i++)
			{
				// Expired, delete from render queue
				if (Time.realtimeSinceStartup - _debugLog[i].displayTime > _debugLog[i].startTime)
				{
					if (removeQueue == null) removeQueue = new Queue<debugTexts>();
					removeQueue.Enqueue(_debugLog[i]);
				}
				else
				{
					DrawText(_debugLog[i].debugText, _textHeight * textCount);
					textCount++;
				}
			}

			if (removeQueue != null)
			{
				while (removeQueue.Count > 0)
				{
					_debugLog.Remove(removeQueue.Dequeue());
				}
			}
		}

		static private void DrawText(string text, int yOffSet)
		{
			_debugTextFont.DrawText(null, text, 0, yOffSet, Color.White);
		}

		static internal void Dispose()
		{
			_debugTextFont.Dispose();
		}
	}
}
