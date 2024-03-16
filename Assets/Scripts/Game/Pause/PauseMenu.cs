using UnityEngine;

public class PauseMenu : MonoBehaviour
{
	[Header("Gameplay")]
	[SerializeField] private TicTacToeGameplay _gameplay;

	[Header("Bot")]
	[SerializeField] private SwitchToggle _botDifficultyToggle;

	private void OnEnable() => _gameplay.PauseGame();

	private void OnDisable()
	{
		if (IsBotDifficultyToggleValueDifferent() == false)
		{
			return;
		}

		GlobalData.IsBotHard = _botDifficultyToggle.Toggle.isOn;
	}

	private void Start()
	{
		if (IsBotDifficultyToggleValueDifferent())
		{
			_botDifficultyToggle.Toggle.isOn = GlobalData.IsBotHard;
		}
	}

	private bool IsBotDifficultyToggleValueDifferent() => _botDifficultyToggle.Toggle.isOn != GlobalData.IsBotHard;
}