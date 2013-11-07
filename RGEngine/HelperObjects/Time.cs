using System;
using System.Diagnostics;


namespace RGEngine
{
	public static class Time
	{
		#region Fields
		
		// Time Keeping
		static private Stopwatch dTWatch = null;
		static private Stopwatch gTWatch = null;
		static private long _dTTick = 0;
		static private double _dTMs = 0.0;
		static private double _dTSec = 0.0;
		static private int _targetFPS = 60;
		static private long _targetFrameTime = 0;
		static private float _realtimeSinceStartup = 0.0f;

		// FPS counter stuff
		static private float _updateInterval = 0.05f;
		static private float _lastInterval = 0.0f;
		static private int _frames = 0;
		static private float _frameRate = 0.0f;
		#endregion

		#region Properties
		static public long DeltaTimeTicks { get { return _dTTick; } }
		static public double DeltaTimeDouble { get { return _dTSec; } }
		static public float DeltaTime { get { return (float)_dTSec; } }
		static public int TargetFrameRate { get { return _targetFPS; } set { _targetFPS = value; _targetFrameTime = TimeSpan.TicksPerSecond / _targetFPS; } }
		static public float realtimeSinceStartup { get { return _realtimeSinceStartup; } }
		static public float FPS { get { return _frameRate; } }
		static public float FPSUpdaterIntervall { get { return _updateInterval; } set { _updateInterval = value; } }
		#endregion

		#region Methods

		static internal void Initialize()
		{
			dTWatch = new Stopwatch();
			gTWatch = new Stopwatch();
			dTWatch.Start();
			gTWatch.Start();

			_targetFrameTime = TimeSpan.TicksPerSecond / _targetFPS;
		}


		static internal void Update()
		{
			// Update runtime
			_realtimeSinceStartup = (float)gTWatch.Elapsed.Ticks / TimeSpan.TicksPerSecond;

			// Update dT
			_dTTick = dTWatch.Elapsed.Ticks;
			// Hold the Gameloop until we reach desirable Frametime.
			while (_dTTick < _targetFrameTime)
				_dTTick = dTWatch.Elapsed.Ticks;
			dTWatch.Restart();

			// Update dT conversions
			_dTMs = (double)_dTTick / TimeSpan.TicksPerMillisecond;
			_dTSec = _dTMs / 1000;

			// Fix targetFrameTime, just incase it has changed
			_targetFrameTime = TimeSpan.TicksPerSecond / _targetFPS;

			// Calculate framerate
			_frames++;
			float timeNow = Time.realtimeSinceStartup;
			if (timeNow > _lastInterval + _updateInterval)
			{
				_frameRate = _frames / (timeNow - _lastInterval);
				_frames = 0;
				_lastInterval = timeNow;
			}
		}
		#endregion
	}
}
