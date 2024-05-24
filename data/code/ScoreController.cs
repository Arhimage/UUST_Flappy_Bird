using System.Collections;
using System.Collections.Generic;
using Unigine;

[Component(PropertyGuid = "4c88a357184f35e8c29caf81b095422433a910aa")]
public class ScoreController : Component
{
	public delegate void ScoreHandler(int score);
	public static ScoreHandler ScoreUpdate;
	public int CurrentScore { get; private set; } = 0;
	
	private List<Node> _scorePipeList = new();

	private Gui gui = null;

	private WidgetLabel labelScore = null;

	void Init()
	{
		PipeController.SpawnNodeEvent += OnPipeSpawn;
		ScoreUpdate += OnScoreUpdate;
		MenuController.GameStartEvent += OnGameStart;
		GameController.GameOverEvent += OnGameOver;
	}

	private void OnGameStart()
	{
		CurrentScore = 0;
		InitGUI();
	}

	private void OnGameOver()
	{
		labelScore.DeleteLater();
		_scorePipeList.Clear();
	}

	void InitGUI()
	{
		gui = Gui.GetCurrent();

		labelScore = new WidgetLabel(gui, $"Счет: {CurrentScore}")
		{
			FontSize = 30,
			FontColor = vec4.WHITE,
			FontHOffset = 20,
			FontVOffset = 20,
		};

		gui.AddChild(labelScore, Gui.ALIGN_LEFT | Gui.ALIGN_TOP);
	}

	private void OnPipeSpawn(Node spawnedPipe)
	{
		_scorePipeList.Add(spawnedPipe);
	}
	
	private float timer = 0;

	void Update()
	{
		UpdateScore();
	}

	private void UpdateScore()
	{
		for (int i = 0; i < _scorePipeList.Count; i++)
		{
			if (_scorePipeList[i].WorldPosition.y < 0)
			{
				_scorePipeList.RemoveAt(i);
				CurrentScore++;
				ScoreUpdate?.Invoke(CurrentScore);
				return;
			}
		}
	}

	private void OnScoreUpdate(int score)
	{
		labelScore.Text = $"Счет: {score}";
	}
}