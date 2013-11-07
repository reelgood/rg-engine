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
		PostRender
	}

	/// <summary>
	/// A static controller class for hooking up methods to be called by the main game loop.
	/// </summary>
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

		static private Queue<Function> _startFuncQueue = null;
		static private Queue<Function> _endFuncQueue = null;

		static internal void DoUpdate() { if (UpdateLoop != null) UpdateLoop(); }
		static internal void DoRender() { if (RenderLoop != null) RenderLoop(); }
		static internal void DoGUI() { if (GUILoop != null) GUILoop(); }
		static internal void DoFixedUpdate() { if (FixedUpdateLoop != null) FixedUpdateLoop(); }
		static internal void DoLateUpdateLoop() { if (LateUpdateLoop != null) LateUpdateLoop(); }
		static internal void DoPreRenderLoop() { if (PreRenderLoop != null) PreRenderLoop(); }
		static internal void DoPostRenderLoop() { if (PostRenderLoop != null) PostRenderLoop(); }
	
		/// <summary>
		/// Adds a method to the update queue
		/// </summary>
		/// <param name="func">The function to be called</param>
		/// <param name="type">The type of function. Results in the time the function is called.</param>
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
			}
		}
		
		/// <summary>
		/// Removes a method from the update queue
		/// </summary>
		/// <param name="func">The method to remove</param>
		static public void RemoveMethod(Function func)
		{
			UpdateLoop -= func;
		}
			
		/// <summary>
		/// Adds a function to be called at the very BEGINNING of the game loop
		/// </summary>
		/// <param name="func">The function to be called</param>
		static public void AddStartMethod(Function func)
		{
			if (_startFuncQueue == null) _startFuncQueue = new Queue<Function>();
			_startFuncQueue.Enqueue(func);
		}
		
		/// <summary>
		/// Adds a function to be called at the very END of the game loop
		/// </summary>
		/// <param name="func"></param>
		static public void AddEndMethod(Function func)
		{
			if (_endFuncQueue == null) _endFuncQueue = new Queue<Function>();
			_endFuncQueue.Enqueue(func);
		}

		/// <summary>
		/// Executes enqueued start functions
		/// </summary>
		static internal void RunStartMethods()
		{
			if (_startFuncQueue == null) return;
			while (_startFuncQueue.Count > 0)
				_startFuncQueue.Dequeue().Invoke();
			_startFuncQueue = null;
		}
		
		/// <summary>
		/// Executes enqueued end functions
		/// </summary>
		static internal void RunEndMethods()
		{
			if (_endFuncQueue == null) return;
			while (_endFuncQueue.Count > 0)
				_endFuncQueue.Dequeue().Invoke();
			_endFuncQueue = null;
		}
	}
}
