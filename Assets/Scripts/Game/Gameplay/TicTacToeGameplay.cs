using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(TicTacToeGameplayView))]
public class TicTacToeGameplay : MonoBehaviour
{
	[Header("Board")]
	[SerializeField] private CanvasGroup _board;
	[SerializeField] private Transform _cellsParent;

	[Header("Stopwatch")]
	[SerializeField] private Stopwatch _stopwatch;

	[Header("Bot")]
	[SerializeField, Range(1.0f, 10.0f)] private float _botMoveDelay = 2.0f;

	[Header("Exit To Main Menu")]
	[SerializeField, Range(1.0f, 10.0f)] private float _loadingDurationInSecondsOnExit = 2.0f;
	[SerializeField] private string _exitQuestion = "Do you want to exit to the main menu?";
	[SerializeField] private string _onExitLoadingText = "Loading main menu...";

	public GameStatistic Statistic { get; private set; } = new();
	public TicTacToeResult LastGameResult { get; private set; }

	public Stopwatch Stopwatch => _stopwatch;

	public bool IsGameFinished { get; private set; }

	public const int GRID_SIZE = 9;

	private readonly GameGridCell[] _cells = new GameGridCell[GRID_SIZE];
	private readonly List<GameGridCell> _freeCells = new();

	private TicTacToeGameplayView _view;

	private TicTacToeMark _playerMark = TicTacToeMark.X;
	private TicTacToeMark _currentTurnMark = TicTacToeMark.X;

	private void Awake() => _view = GetComponent<TicTacToeGameplayView>();

	private void Start()
	{
		InitializeCellsArray();

		StartNewGame();

		Stopwatch.Restart();
	}

	public void StartNewGame()
	{
		IsGameFinished = false;

		StopCoroutine(nameof(BotMove));

		foreach (var cell in _cells)
		{
			cell.Clear();
		}

		InitializeFreeCells();

		_currentTurnMark = TicTacToeMark.X;

		SelectRandomMarkToPlayer();

		UpdatePlayersInfoView();
		UpdateTurnIcons();

		if (_currentTurnMark != _playerMark)
		{
			StartBotMove();
		}

		_view.DisableAllWinLines();
		SetBoardInteractable(true);

		Stopwatch.Resume();
	}

	public void PauseGame() => Stopwatch.Pause();

	public void ResumeGame() => Stopwatch.Resume();

	public void ExitToMainMenu() => MessageSystem.ShowWithQuestionYesOrNo(_exitQuestion, OnClicKExit);

	private void OnClicKExit()
	{
		GlobalData.LastGameStatistic = Statistic;
		PlayerScoreSystem.SaveScoreToPlayerPrefs();

		SceneLoader.Instance.LoadMainMenu();

		LoadingScreenSystem.ShowForSeconds(_loadingDurationInSecondsOnExit);
		LoadingScreenSystem.UpdateLoadingText(_onExitLoadingText);
	}

	private void InitializeCellsArray()
	{
		var cells = _cellsParent.GetComponentsInChildren<GameGridCell>();

		foreach (var cell in cells)
		{
			_cells[cell.ID] = cell;
		}
	}

	private void InitializeFreeCells()
	{
		_freeCells.Clear();

		foreach (var cell in _cells)
		{
			_freeCells.Add(cell);
		}
	}

	public void OnClickCell(GameGridCell cell)
	{
		if (_currentTurnMark != _playerMark)
		{
			return;
		}

		FillCellWithCurrentTurnMark(cell);

		if (IsGameFinished)
		{
			return;
		}

		StartBotMove();
	}

	private void FillCellWithCurrentTurnMark(GameGridCell cell)
	{
		FillCell(cell, _currentTurnMark);
		_freeCells.Remove(cell);

		CheckForGameFinish();

		if (IsGameFinished)
		{
			return;
		}

		SwitchCurrentTurnMark();
		UpdateTurnIcons();
	}

	private void SelectRandomMarkToPlayer() => _playerMark = (TicTacToeMark)Random.Range((int)TicTacToeMark.X, (int)TicTacToeMark.O + 1);

	private void UpdatePlayersInfoView()
	{
		Sprite playerMarkSprite = null;
		Color32 playerMarkColor = new();

		Sprite botMarkSprite = null;
		Color32 botMarkColor = new();

		switch (_playerMark)
		{
			case TicTacToeMark.X:
				{
					playerMarkSprite = _view.SpriteX;
					playerMarkColor = _view.ColorX;

					botMarkSprite = _view.SpriteO;
					botMarkColor = _view.ColorO;

					break;
				}
			case TicTacToeMark.O:
				{
					playerMarkSprite = _view.SpriteO;
					playerMarkColor = _view.ColorO;

					botMarkSprite = _view.SpriteX;
					botMarkColor = _view.ColorX;
				}
				break;
		}

		_view.UpdatePlayerMarkIcon(playerMarkSprite, playerMarkColor);
		_view.UpdateBotMarkIcon(botMarkSprite, botMarkColor);
	}

	private void UpdateTurnIcons()
	{
		_view.SetActivatePlayerTurnIcon(_currentTurnMark == _playerMark);
		_view.SetActivateBotTurnIcon(_currentTurnMark != _playerMark);
	}

	private void SwitchCurrentTurnMark() => _currentTurnMark = _currentTurnMark == TicTacToeMark.X ? TicTacToeMark.O : TicTacToeMark.X;

	private void StartBotMove() => StartCoroutine(nameof(BotMove));

	private IEnumerator BotMove()
	{
		yield return new WaitForSecondsRealtime(_botMoveDelay);

		var freeCell = GetCellForBotMove();

		FillCellWithCurrentTurnMark(freeCell);
	}

	private void FillCell(GameGridCell cell, TicTacToeMark mark)
	{
		Sprite markSprite = null;
		Color32 markColor = new();

		switch (mark)
		{
			case TicTacToeMark.X:
				{
					markSprite = _view.SpriteX;
					markColor = _view.ColorX;
					break;
				}
			case TicTacToeMark.O:
				{
					markSprite = _view.SpriteO;
					markColor = _view.ColorO;
					break;
				}
		}

		cell.Fill(mark, markSprite, markColor);
	}

	private GameGridCell GetCellForBotMove()
	{
		if (GlobalData.IsBotHard == false)
		{
			return GetRandomFreeCell();
		}

		if (_freeCells.Count == 0) // if all cells filled
		{
			return null;
		}

		if (_cells[4].IsFilled == false) // Center cell is free
		{
			return _cells[4];
		}

		if (_freeCells.Count == _cells.Length - 1) // Center filled
		{
			return _freeCells[0];
		}

		foreach (var cell in _freeCells)
		{
			switch (cell.ID)
			{
				case 0:
					{
						if (IsCellsTheSameMarked(1, 2) || IsCellsTheSameMarked(3, 6) || IsCellsTheSameMarked(4, 8))
						{
							return cell;
						}
						break;
					}
				case 1:
					{
						if (IsCellsTheSameMarked(0, 2) || IsCellsTheSameMarked(4, 7))
						{
							return cell;
						}
						break;
					}
				case 2:
					{
						if (IsCellsTheSameMarked(0, 1) || IsCellsTheSameMarked(5, 8) || IsCellsTheSameMarked(4, 6))
						{
							return cell;
						}
						break;
					}
				case 3:
					{
						if (IsCellsTheSameMarked(0, 6) || IsCellsTheSameMarked(4, 5))
						{
							return cell;
						}
						break;
					}
				case 4:
					{
						if (IsCellsTheSameMarked(1, 7) || IsCellsTheSameMarked(3, 5))
						{
							return cell;
						}
						break;
					}
				case 5:
					{
						if (IsCellsTheSameMarked(2, 8) || IsCellsTheSameMarked(3, 4))
						{
							return cell;
						}
						break;
					}
				case 6:
					{
						if (IsCellsTheSameMarked(0, 3) || IsCellsTheSameMarked(7, 8) || IsCellsTheSameMarked(4, 2))
						{
							return cell;
						}
						break;
					}
				case 7:
					{
						if (IsCellsTheSameMarked(1, 4) || IsCellsTheSameMarked(6, 8))
						{
							return cell;
						}
						break;
					}
				case 8:
					{
						if (IsCellsTheSameMarked(0, 4) || IsCellsTheSameMarked(2, 5) || IsCellsTheSameMarked(6, 7))
						{
							return cell;
						}
						break;
					}
			}
		}

		return GetRandomFreeCell();
	}

	private GameGridCell GetRandomFreeCell()
	{
		var randomIndex = Random.Range(0, _freeCells.Count);

		return _freeCells[randomIndex];
	}

	private bool IsCellsTheSameMarked(int id1, int id2)
	{
		if (_cells[id1].IsFilled == false || _cells[id2].IsFilled == false)
		{
			return false;
		}

		return _cells[id1].FilledMark == _cells[id2].FilledMark;
	}

	private bool IsCellsTheSameMarked(int id1, int id2, int id3)
	{
		if (_cells[id1].IsFilled == false || _cells[id2].IsFilled == false || _cells[id3].IsFilled == false)
		{
			return false;
		}

		if (_cells[id1].FilledMark != _cells[id2].FilledMark)
		{
			return false;
		}

		if (_cells[id2].FilledMark != _cells[id3].FilledMark)
		{
			return false;
		}

		return true;
	}

	private void SetBoardInteractable(bool active) => _board.interactable = active;

	private void CheckForGameFinish()
	{
		if (IsCellsTheSameMarked(0, 1, 2)) // Case 0
		{
			FinishGameWithWinLine(0);
			return;
		}

		if (IsCellsTheSameMarked(3, 4, 5)) // Case 1
		{
			FinishGameWithWinLine(1);
			return;
		}

		if (IsCellsTheSameMarked(6, 7, 8)) // Case 2
		{
			FinishGameWithWinLine(2);
			return;
		}

		if (IsCellsTheSameMarked(0, 3, 6)) // Case 3
		{
			FinishGameWithWinLine(3);
			return;
		}

		if (IsCellsTheSameMarked(1, 4, 7)) // Case 4
		{
			FinishGameWithWinLine(4);
			return;
		}

		if (IsCellsTheSameMarked(2, 5, 8)) // Case 5
		{
			FinishGameWithWinLine(5);
			return;
		}

		if (IsCellsTheSameMarked(0, 4, 8)) // Case 6
		{
			FinishGameWithWinLine(6);
			return;
		}

		if (IsCellsTheSameMarked(2, 4, 6)) // Case 7
		{
			FinishGameWithWinLine(7);
			return;
		}

		if (_freeCells.Count == 0)
		{
			LastGameResult = TicTacToeResult.Draw;
			FinishGame();
		}
	}

	private void FinishGameWithWinLine(int winLineID)
	{
		ShowWinLine(winLineID);
		UpdateLastGameResult();
		UpdatePlayerScore();

		FinishGame();
	}

	private void UpdateLastGameResult() => LastGameResult = _currentTurnMark == _playerMark ? TicTacToeResult.Win : TicTacToeResult.Lose;

	private void UpdatePlayerScore()
	{
		if (_currentTurnMark == _playerMark)
		{
			PlayerScoreSystem.OnWon();
		}
		else
		{
			PlayerScoreSystem.OnLose();
		}
	}

	private void FinishGame()
	{
		IsGameFinished = true;
		SetBoardInteractable(false);

		Stopwatch.Pause();

		UpdateStatistic();

		_view.ShowGameResultPanel();
	}

	private void UpdateStatistic()
	{
		Statistic.TimeInGame = Stopwatch.ElapsedTimeInSeconds;
		Statistic.PlayedGames++;

		switch (LastGameResult)
		{
			case TicTacToeResult.Win:
				Statistic.Winnings++;
				break;
			case TicTacToeResult.Lose:
				Statistic.Loses++;
				break;
			case TicTacToeResult.Draw:
				Statistic.DrawGames++;
				break;
		}

		if (Statistic.Winnings == Statistic.Loses)
		{
			Statistic.WinnerNickName = "Draw Game";
		}
		else if (Statistic.Winnings > Statistic.Loses)
		{
			Statistic.WinnerNickName = GlobalData.PlayerNickName;
		}
		else
		{
			Statistic.WinnerNickName = "Bot";
		}
	}

	private void ShowWinLine(int winLineID) => _view.ShowWinLine(_currentTurnMark, winLineID);
}