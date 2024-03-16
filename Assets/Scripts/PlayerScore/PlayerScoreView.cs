using TMPro;
using UnityEngine;

public class PlayerScoreView : MonoBehaviour
{
	[SerializeField] private TMP_Text _scoreText;

	private void OnEnable()
	{
		OnScoreChanged();

		PlayerScoreSystem.ScoreChanged += OnScoreChanged;
	}

	private void OnDisable() => PlayerScoreSystem.ScoreChanged -= OnScoreChanged;

	private void UpdateScoreText(int score) => _scoreText.text = $"{score}";

	private void OnScoreChanged() => UpdateScoreText(PlayerScoreSystem.CurrentScore);
}