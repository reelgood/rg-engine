using System.Collections.Generic;

namespace RGEngine
{
	public enum HookType
	{
		Update,
		Render,
		GUI, 
		FixedUpdate,
		LateUpdate,
		PreRender,
		PostRender,
		Dispose
	}

	public static class Hook
	{
		public delegate void Function();

		static internal event Function UpdateLoop;
		static internal event Function RenderLoop;
		static internal event Function GUILoop;
		static internal event Function FixedUpdateLoop;
		static internal event Function LateUpdateLoop;
		static internal event Function PreRenderLoop;
		static internal event Function PostRenderLoop;
		static internal event Function DisposeFunc;

		static internal void DoUpdate() { if (UpdateLoop != null) UpdateLoop(); }
		static internal void DoRender() { if (RenderLoop != null) RenderLoop(); }
		static internal void DoGUI() { if (GUILoop != null) GUILoop(); }
		static internal void DoFixedUpdate() { if (FixedUpdateLoop != null) FixedUpdateLoop(); }
		static internal void DoLateUpdateLoop() { if (LateUpdateLoop != null) LateUpdateLoop(); }
		static internal void DoPreRenderLoop() { if (PreRenderLoop != null) PreRenderLoop(); }
		static internal void DoPostRenderLoop() { if (PostRenderLoop != null) PostRenderLoop(); }
		static internal void DoDispose() { if (DisposeFunc != null) DisposeFunc(); }
				
		static public void AddMethod(Function func, HookType type)
		{
			switch (type)
			{
				case HookType.Update:
					UpdateLoop += func;
					break;
				case HookType.Render:
					RenderLoop += func;
					break;
				case HookType.GUI:
					GUILoop += func;
					break;
				case HookType.FixedUpdate:
					FixedUpdateLoop += func;
					break;
				case HookType.LateUpdate:
					LateUpdateLoop += func;
					break;
				case HookType.PreRender:
					PreRenderLoop += func;
					break;
				case HookType.PostRender:
					PostRenderLoop += func;
					break;
				case HookType.Dispose:
					DisposeFunc += func;
					break;
			}
		}
		
		static public void RemoveMethod(Function func)
		{
			UpdateLoop -= func;
		}



		static private Queue<Function> _startFuncQueue = null;
		static internal void AddStarterMethod(Function func)
		{
			if (_startFuncQueue == null) _startFuncQueue = new Queue<Function>();
			_startFuncQueue.Enqueue(func);
		}
		static internal void RunStartMethods()
		{
			if (_startFuncQueue == null) return;
			while (_startFuncQueue.Count > 0)
				_startFuncQueue.Dequeue().Invoke();
			_startFuncQueue = null;
		}
	}
}
