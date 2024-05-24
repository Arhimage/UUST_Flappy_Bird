using System.Collections;
using System.Collections.Generic;
using Unigine;

[Component(PropertyGuid = "39951cd4af9108227f7459b2ce24328796e80139")]
public class PipeController : Component
{
	public delegate void SpawnHandler(Node spawnedNode);
	public static SpawnHandler SpawnNodeEvent;

	private static List<Node> _pipeList = new();

	[ShowInEditor]
	[ParameterFile (Title = "Образец трубы", Filter = ".node")]
	private string _pipeSample = "";

	[ShowInEditor]
	[ParameterSlider (Title = "Скорость перемещения труб", Min = 0, Max = 100)]
	private float _pipeSpeed = 2.0f;

	[ShowInEditor]
	[Parameter (Title = "Смещение спавна (горизонт)")]
	private float _horizontalOffset = 20.0f;

	[ShowInEditor]
	[Parameter (Title = "Высота спавна от")]
	private float _verticalMinOffset = 4.2f;

	[ShowInEditor]
	[Parameter (Title = "Высота спавна до")]
	private float _verticalMaxOffset = 13.8f;

	[ShowInEditor]
	[Parameter (Title = "Дельта времени")]
	private float _timeOffset = 8.0f;

	private float _spawnTimer = 100;
	private Random _random = new();

	void Init()
	{
		if (_pipeSample == "")
		{
			Log.ErrorLine($"Не все данные заполнены для {this}");
			this.Enabled = false;
		}
		SpawnNodeEvent += OnPipeSpawn;
		MenuController.GameStartEvent += OnGameStart;
		GameController.GameOverEvent += OnGameOver;
		this.Enabled = false;
	}

	private void OnGameStart()
	{
		_spawnTimer = 100;
		Enabled = true;
	}

	private void OnGameOver()
	{
		for (int i = 0; i < _pipeList.Count; i++)
		{
			DeletePipe(_pipeList[i]);
			_pipeList.RemoveAt(i);
			i--;
		}
		Enabled = false;
	}

	private void SpawnPipe()
	{
		Node spawnedNode = World.LoadNode(_pipeSample);
		SpawnNodeEvent?.Invoke(spawnedNode);
	}

	private void OnPipeSpawn(Node spawnedPipe)
	{
		_pipeList.Add(spawnedPipe);
		spawnedPipe.WorldPosition = new vec3(0, _horizontalOffset, _random.Float(_verticalMinOffset, _verticalMaxOffset));
	}

	private void MovePipe(Node pipe)
	{
		pipe.Translate(new vec3(0, -_pipeSpeed * Game.IFps, 0));
	}

	private void DeletePipe(Node pipe)
	{
		pipe.DeleteLater();
	}
	
	void Update()
	{
		SpawnPipes();
		ManagePipes();
	}

	private void SpawnPipes()
	{
		if (_spawnTimer > _timeOffset)
		{
			SpawnPipe();
			_spawnTimer = 0;
		}
		_spawnTimer += Game.IFps;
	}

	private void ManagePipes()
	{
		for (int i = 0; i < _pipeList.Count; i++)
		{
			MovePipe(_pipeList[i]);
			if (_pipeList[i].WorldPosition.y < -_horizontalOffset)
			{
				DeletePipe(_pipeList[i]);
				_pipeList.RemoveAt(i);
				i--;
			}
		}
	}
}