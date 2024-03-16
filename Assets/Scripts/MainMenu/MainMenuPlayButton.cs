using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class MainMenuPlayButton : MonoBehaviour
{
	[SerializeField, Range(0.1f, 10.0f)] private float _loadingDurationInSeconds = 2.0f;
	[SerializeField] private string _loadingText = "Loading a game scene...";
	[SerializeField] private string _botTypeQuestion = "Select bot type";

	private const string TEXT_HARD = "Hard";
	private const string TEXT_EASY = "Easy";

	private Button _button;

	private void Awake() => _button = GetComponent<Button>();

	private void Start() => AddButtonListener();

	private void AddButtonListener() => _button.onClick.AddListener(ShowSelectionBotTypePopUp);

	private void ShowSelectionBotTypePopUp() => MessageSystem.ShowWithTwoButtonChoice(_botTypeQuestion, OnClickEaseBot, OnClickHardBot, TEXT_EASY, TEXT_HARD);

	private void OnClickHardBot()
	{
		GlobalData.IsBotHard = true;
		OnClick();
	}

	private void OnClickEaseBot()
	{
		GlobalData.IsBotHard = false;
		OnClick();
	}

	private void OnClick()
	{
		SceneLoader.Instance.LoadNextScene();

		LoadingScreenSystem.ShowForSeconds(_loadingDurationInSeconds);
		LoadingScreenSystem.UpdateLoadingText(_loadingText);
	}
}