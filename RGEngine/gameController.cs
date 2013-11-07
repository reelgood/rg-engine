using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RGEngine
{
	public abstract class gameController : gameElement
	{
        public gameController()
        {
            Hook.AddMethod(Update, HookType.Update);
            Hook.AddMethod(Render, HookType.Render);
            Hook.AddMethod(GUI, HookType.GUI);
        }
		protected virtual void Update() { }
		protected virtual void Render() { }
		protected virtual void GUI() { }
	}
}
