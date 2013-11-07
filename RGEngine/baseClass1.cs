using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RGEngine
{
	public abstract class baseClass1 : IObject, IDisposable
	{
		List<IDisposable> _toDispose;

		protected virtual void ToDispose(IDisposable obj)
		{
			if (_toDispose == null) _toDispose = new List<IDisposable>();
			_toDispose.Add(obj);
		}

		public virtual void Dispose()
		{
			for (int i = 0; i < _toDispose.Count; i++)
			{
				_toDispose[i].Dispose();
			}
			_toDispose.Clear();
			_toDispose = null; 
		}
	}
}
