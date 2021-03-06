﻿using System;
using SharpDX;
using SharpDX.Direct3D9;

namespace RGEngine
{
	public class Application : gameController
	{
		private GraphicSystem _graphics;

		public void Run()
		{
			Controllers.GameObjectController = new GameObjectController();
			_graphics = new GraphicSystem(_logic, _render);
			Controllers.Application = this;
			Controllers.GraphicSystem = _graphics;
			Initialize();

			_graphics.StartRenderLoop(); // MUST ALWAYS BE LAST!
		}


		private void Initialize()
		{
			Time.Initialize();
			Debug.Initialize(_graphics.graphicDevice);
			Input.Initialize(_graphics.renderForm);
		}

		private void _logic()
		{
			Hook.RunStartMethods();
			Time.Update();
			Input.GatherInput();
			Hook.DoUpdate();
		}

		private void _render()
		{
			Hook.DoPreRenderLoop();
			_graphics.graphicDevice.Clear(ClearFlags.ZBuffer | ClearFlags.Target, Color.Black, 0.0f, 0);
			_graphics.graphicDevice.BeginScene();

			Hook.DoRender();
			Hook.DoGUI();
			Debug.Print();

			_graphics.graphicDevice.EndScene();
			_graphics.graphicDevice.Present();
			Hook.DoPostRenderLoop();
		}

		protected void Exit()
		{
			_graphics.StopRenderLoop();
		}
	}
}
