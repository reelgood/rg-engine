using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RGEngine
{
	public class GameObject : gameComponent
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
            RGEngine.HelperObjects.Controllers.GameObjectController.UpdateMe(this);
			_components = new List<Component>();
			_setGameObject(this);
			AddComponent(typeof(Transform));
		}

		/// <summary>
		/// Creates a new GameObject, with name
		/// </summary>
		/// <param name="Name">Name of the GameObject</param>
		public GameObject(string Name)
		{
			_components = new List<Component>();
			_setGameObject(this);
			AddComponent(typeof(Transform));
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


		protected override void Update()
		{
			if (_components == null) return;
			for (int i = 0; i < _components.Count; i++) _components[i]._update();
		}

		public static void Destroy(GameObject gameObject)
		{
            RGEngine.HelperObjects.Controllers.GameObjectController.ForgetMe(this);
			for (int i = 0; i < gameObject._components.Count; i++)
			{
				gameObject._components[i].Dispose();
				gameObject._components[i] = null;
			}
		}




		protected override void OnDispose()
		{

			Debug.Log("DISPOSING GOB!!", 500);
		}
	}
}
