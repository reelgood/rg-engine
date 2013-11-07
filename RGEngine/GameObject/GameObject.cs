using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RGEngine
{
	public class GameObject : baseClass3
	{
		internal Transform _transform;
		internal Camera _camera;
		internal Renderer _renderer;
		internal MeshFilter _meshFilter;
		private List<Component> _components;



		/// <summary>
		/// Creates a new GameObject
		/// </summary>
		public GameObject()
		{
			_setGameObject(this);
			AddComponent(typeof(Transform));
		}

		/// <summary>
		/// Creates a new GameObject, with name
		/// </summary>
		/// <param name="Name">Name of the GameObject</param>
		public GameObject(string Name)
		{
			_setGameObject(this);
			AddComponent(typeof(Transform));
		}

		internal void _update()
		{
			throw new System.NotImplementedException();
		}

		internal void _render()
		{
			throw new System.NotImplementedException();
		}

		internal void _ongui()
		{
			throw new System.NotImplementedException();
		}

		public Component AddComponent(Type componentType)
		{
			for (int i = 0; i < _components.Count; i++)
			{
				if (_components[i].GetType() == componentType)
				{
					// Already contains this component
					return _components[i];
				}
			}
			Component c = Activator.CreateInstance(componentType) as Component;
			if (c is Transform) _transform = (Transform)c;
			if (c is Renderer) _renderer = (Renderer)c;
			if (c is Camera) _camera = (Camera)c;

			_components.Add(c);
			c._setGameObject(this);

			ToDispose(c);
			return c;
		}

		public Component GetComponent(Type componentType)
		{
			for (int i = 0; i < _components.Count; i++)
			{
				if (_components.GetType() == componentType)
				{
					return _components[i];
				}
			}
			return null;
		}
	}
}
