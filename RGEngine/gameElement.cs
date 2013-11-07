using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace RGEngine
{
	public abstract class gameElement : IObject, IDisposable
	{
		List<IDisposable> _toDispose;

		protected virtual void ToDispose(IDisposable obj)
		{
			if (_toDispose == null) _toDispose = new List<IDisposable>();
			_toDispose.Add(obj);
		}

		public void Dispose()
		{
			if (_toDispose == null) return;
			for (int i = 0; i < _toDispose.Count; i++)
			{
				_toDispose[i].Dispose();
				if (_toDispose[i] is gameElement)
				{
					var tmp = _toDispose[i] as gameElement;
					tmp.OnDispose();
				}
			}
			_toDispose.Clear();
			_toDispose = null;
		}

		protected virtual void OnDispose() { }
	}
}
