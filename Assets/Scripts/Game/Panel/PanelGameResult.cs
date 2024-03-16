using System;
using TMPro;
using UnityEngine;

public class PanelGameResult : MonoBehaviour
{
	[Header("Gameplay")]
	[SerializeField] private TicTacToeGameplay _gameplay;

	[Header("Statistics")]
	[SerializeField] private TMP_Text _timeInGame;
	[SerializeField] private TMP_Text _playedGames;
	[SerializeField] private TMP_Text _drawGames;
	[SerializeField] private TMP_Text _loses;
	[SerializeField] private TMP_Text _winnings;

	[Header("Game Result")]
	[SerializeField] private TMP_Text _gameResult;
	[SerializeField] private TMP_Text _scoreChange;

	[Header("Result Text Colors")]
	[SerializeField] private Color32 _drawResultColor = Color.cyan;
	[SerializeField] private Color32 _winningResultColor = Color.green;
	[SerializeField] private Color32 _loseResultColor = Color.red;

	[Header("Result Texts")]
	[SerializeField] private string _drawResultText = "Draw Game";
	[SerializeField] private string _winningResultText = "You Won!";
	[SerializeField] private string _loseResultText = "You Lose";

	[SerializeField, Space(10)] private TMP_Text _currentScore;

	public void Show()
	{
		var statistics = _gameplay.Statistic;

		UpdateGameResultsText();

		_timeInGame.text = TimeSpan.FromSeconds(statistics.TimeInGame).ToString(Stopwatch.STRING_FORMAT);
		_playedGames.text = $"{statistics.PlayedGames}";
		_drawGames.text = $"{statistics.DrawGames}";
		_loses.text = $"{statistics.Loses}";
		_winnings.text = $"{statistics.Winnings}";

		_currentScore.text = $"{PlayerScoreSystem.CurrentScore}";

		gameObject.SetActive(true);
	}

	private void UpdateGameResultsText()
	{
		string resultText = string.Empty;
		Color32 resultTextColor = Color.white;
		string scoreChangeText = string.Empty;

		switch (_gameplay.LastGameResult)
		{
			case TicTacToeResult.Draw:
				resultText = _drawResultText;
				resultTextColor = _drawResultColor;
				scoreChangeText = string.Empty;
				break;
			case TicTacToeResult.Win:
				resultText = _winningResultText;
				resultTextColor = _winningResultColor;
				scoreChangeText = $"+{PlayerScoreSystem.SCORES_PER_GAME}";
				break;
			case TicTacToeResult.Lose:
				resultText = _loseResultText;
				resultTextColor = _loseResultColor;
				scoreChangeText = $"-{PlayerScoreSystem.SCORES_PER_GAME}";
				break;
		}

		_gameResult.text = resultText;
		_gameResult.color = resultTextColor;

		_scoreChange.text = scoreChangeText;
		_scoreChange.color = resultTextColor;
	}
}