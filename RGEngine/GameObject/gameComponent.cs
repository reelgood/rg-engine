using System;

namespace RGEngine
{
	public abstract class gameComponent : gameElement
	{

		private bool _enabled;
		private GameObject _gameObject;

		public bool Enabled { get { return _enabled; } set { _enabled = value; } }


		public gameComponent()
		{
			Hook.AddStartMethod(Start);
		}

        protected virtual void Start() { }
        protected virtual void Update() { }
        protected virtual void Render() { }
		protected virtual void GUI() { }
		
		internal void _update()
		{
			if (_enabled)
			{
				Update();
			}
		}

		internal void _render()
		{
			if (_enabled)
			{
				Render();
			}
		}

		internal void _gui()
		{
			if (_enabled)
			{
				GUI();
			}
		}

		
		public Transform Transform
		{
			get
			{
				return _gameObject._transform;
			}
		}

		public Camera Camera
		{
			get
			{
				return _gameObject._camera;
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
