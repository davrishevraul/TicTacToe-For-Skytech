using System;
using UnityEngine;

public class PlayerScoreSystem
{
	public static int CurrentScore { get; private set; }

	private const int DEFAULT_SCORE_VALUE = 0;
	private const string PLAYER_PREFS_PLAYER_SCORE = "PLAYER_SCORE";

	public const int SCORES_PER_GAME = 100;

	public static event Action ScoreChanged;

	[RuntimeInitializeOnLoadMethod]
	private static void LoadScoreFromPlayerPrefs() => CurrentScore = PlayerPrefs.GetInt(PLAYER_PREFS_PLAYER_SCORE, DEFAULT_SCORE_VALUE);

	public static void OnWon()
	{
		CurrentScore += SCORES_PER_GAME;
		ScoreChanged?.Invoke();
	}

	public static void OnLose()
	{
		CurrentScore -= SCORES_PER_GAME;
		ScoreChanged?.Invoke();
	}

	public static void SaveScoreToPlayerPrefs()
	{
		PlayerPrefs.SetInt(PLAYER_PREFS_PLAYER_SCORE, CurrentScore);
		PlayerPrefs.Save();
	}
}