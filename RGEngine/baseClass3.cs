using System;

namespace RGEngine
{
	public abstract class baseClass3 : baseClass2
	{


		private GameObject _gameObject;

		public Transform Transform
		{
			get
			{
				return _gameObject.Transform;
			}
		}

		public Camera Camera
		{
			get
			{
				return _gameObject.Camera;
			}
		}

		public Renderer Renderer
		{
			get
			{
				return _gameObject._renderer;
			}
		}

		public MeshFilter MeshFilter
		{
			get
			{
				return _gameObject._meshFilter;
			}
		}

		public GameObject GameObject
		{
			get
			{
				return _gameObject;
			}
		}

		internal void _setGameObject(GameObject gameObject)
		{
			if (_gameObject != null)
				throw new System.Exception("Cant set gameObject twice");
			_gameObject = gameObject;
		}

	}
}
