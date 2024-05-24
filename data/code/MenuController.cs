using System.Collections;
using System.Collections.Generic;
using Unigine;

[Component(PropertyGuid = "b30bcd66e4e3742afae168747ab6b0cf4f186772")]
public class MenuController : Component
{
	public delegate void GameStartHandler();
	public static GameStartHandler GameStartEvent;

	private Gui gui = null;
	private WidgetVBox menuBox = null;
	private WidgetLabel menuLabel = null;
	private WidgetLabel resultLabel = null;
	private WidgetButton menuResetButton = null;
	private WidgetButton menuExitButton = null;

	private int _fontSize = 30;
	private int? _score;

	void Init()
	{
		ScoreController.ScoreUpdate += OnScoreUpdate;
		GameStartEvent += OnGameStart;
		GameController.GameOverEvent += OnGameOver;
		InitGUI();
	}

	private void OnGameStart()
	{
		menuBox.DeleteLater();
		menuLabel.DeleteLater();
		resultLabel.DeleteLater();
		menuResetButton.DeleteLater();
		menuExitButton.DeleteLater();
	}

	private void OnGameOver()
	{
		InitGUI();
	}

	void InitGUI()
	{
		gui = Gui.GetCurrent();

		menuBox = new WidgetVBox(gui, 20, 20)
		{
			Background = 1,
			BackgroundColor = vec4.GREY,
		};

		menuLabel = new WidgetLabel(gui, "Меню")
		{
			FontSize = _fontSize,
			FontColor = vec4.WHITE,
		};

		string resultText = _score == null ? "" : $"Ваш счет {_score}";
		resultLabel = new WidgetLabel(gui, resultText)
		{
			FontSize = _fontSize,
			FontColor = vec4.WHITE,
		};
		
		string resetText = _score == null ? "Запустить игру" : "Перезапустить игру";
		menuResetButton = new WidgetButton(gui, resetText)
		{
			FontSize = _fontSize,
			FontColor = vec4.WHITE,
		};
		menuResetButton.EventClicked.Connect(StartGame);

		menuExitButton = new WidgetButton(gui, "Выйти")
		{
			FontSize = _fontSize,
			FontColor = vec4.WHITE,
		};
		menuExitButton.EventClicked.Connect(ExitGame);

		menuBox.AddChild(menuLabel, Gui.ALIGN_TOP);
		menuBox.AddChild(resultLabel, Gui.ALIGN_TOP);
		menuBox.AddChild(menuResetButton, Gui.ALIGN_TOP);
		menuBox.AddChild(menuExitButton, Gui.ALIGN_TOP);
		gui.AddChild(menuBox, Gui.ALIGN_CENTER);
	}

	private void StartGame()
	{
		GameStartEvent?.Invoke();
	}

	private void ExitGame()
	{
		Engine.Quit();
	}

	private void OnScoreUpdate(int score)
	{
		_score = score;
	}
}