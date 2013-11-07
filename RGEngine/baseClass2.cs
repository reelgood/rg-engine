using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RGEngine
{
	public abstract class baseClass2 : baseClass1
	{
		public baseClass2()
		{
			Hook.AddStarterMethod(Start);
		}

		protected virtual void Start() { }
		protected virtual void Update() { }
		protected virtual void Render() { }
		protected virtual void OnGUI() { }
	}
}
