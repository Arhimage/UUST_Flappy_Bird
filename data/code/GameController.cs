using System.Collections;
using System.Collections.Generic;
using Unigine;

[Component(PropertyGuid = "9df99984d396a177e7c63ae23e0b34dfacc4fea9")]
public class GameController : Component
{
	public delegate void GameOverHandler();
	public static GameOverHandler GameOverEvent;

	[ShowInEditor]
	[Parameter (Title = "Сила толчка")]
	private float force = 10.0f;

	[ShowInEditor]
	[ParameterFile (Title = "Птица", Filter = ".node")]
	private string _birdPath = "";

	[ShowInEditor]
	[Parameter (Title = "Высота появления")]
	private float _spawnHeight = 12.0f;

	private Node _bird = null;

	void Init()
	{
		if (_birdPath == "")
		{
			Log.ErrorLine($"Не все данные заполнены для {this}");
			this.Enabled = false;
		}
		MenuController.GameStartEvent += OnGameStart;
		GameOverEvent += OnGameOver;
	}

	private void OnGameStart()
	{
		_bird = World.LoadNode(_birdPath);
		_bird.WorldPosition = new vec3(0, 0, _spawnHeight);
		_bird.ObjectBodyRigid.EventContactEnter.Connect(OnCollision);
	}

	private void OnGameOver()
	{
		if (_bird != null)
		{
			_bird.DeleteLater();
			_bird = null;
		}
	}

	private void OnCollision()
	{
		GameOverEvent?.Invoke();
	}
	
	void Update()
	{
		if (Input.IsKeyDown(Input.KEY.SPACE) && _bird != null)
		{
			_bird.ObjectBodyRigid.AddWorldImpulse(_bird.WorldPosition, new vec3(0, 0, force));
		}
	}


}