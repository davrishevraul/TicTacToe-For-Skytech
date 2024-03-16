using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreenSystem : PersistentMonoBehaviourSingleton<LoadingScreenSystem>
{
	[Header("UI References")]
	[SerializeField] private Canvas _canvas;
	[SerializeField] private Slider _progressBar;
	[SerializeField] private TMP_Text _loadingText;

	public const int PROGRESS_BAR_MIN_VALUE = 0;
	public const int PROGRESS_BAR_MAX_VALUE = 100;
	public const string DEFAULT_LOADING_TEXT = "Loading...";

	public static int ProgressBarCurrentValue => GetProgressBarCurrentValue();

	public static bool IsActive = false;

	protected override void Awake()
	{
		base.Awake();

		SetActivateCanvas(false);
		InitializeProgressBar();
	}

	public static void Show()
	{
		if (IsActive)
		{
			Debug.LogWarning("Loading Screen: loading already active.");
			return;
		}

		if (Instance == null)
		{
			OnNullInstance();
			return;
		}

		Instance.SetProgressBarValue(PROGRESS_BAR_MIN_VALUE);
		Instance.SetLoadingText(DEFAULT_LOADING_TEXT);
		Instance.SetActivateCanvas(true);
	}

	public static void ShowForSeconds(float seconds) => Instance.StartCoroutine(Instance.LoadingCoroutine(seconds));

	public static void Hide()
	{
		if (Instance == null)
		{
			OnNullInstance();
			return;
		}

		Instance.StopAllCoroutines();
		Instance.SetActivateCanvas(false);
	}

	private void InitializeProgressBar()
	{
		_progressBar.maxValue = PROGRESS_BAR_MAX_VALUE;
		_progressBar.minValue = PROGRESS_BAR_MIN_VALUE;

		SetProgressBarValue(PROGRESS_BAR_MIN_VALUE);
	}

	public static void UpdateProgressBar(int percents)
	{
		if (IsActive == false)
		{
			OnObjectDisabled();
			return;
		}

		if (percents < PROGRESS_BAR_MIN_VALUE || percents > PROGRESS_BAR_MAX_VALUE)
		{
			Debug.Log("Loading Screen: incorrect progress bar percents.");
			return;
		}

		Instance.SetProgressBarValue(percents);
	}

	public static void UpdateLoadingText(string text)
	{
		if (IsActive == false)
		{
			OnObjectDisabled();
			return;
		}

		Instance.SetLoadingText(text);
	}

	private static int GetProgressBarCurrentValue()
	{
		if (Instance == null)
		{
			return PROGRESS_BAR_MIN_VALUE;
		}

		return (int)Instance._progressBar.value;
	}

	private void SetActivateCanvas(bool active)
	{
		_canvas.gameObject.SetActive(active);
		IsActive = active;
	}

	private void SetProgressBarValue(int value) => _progressBar.value = value;
	private void SetLoadingText(string text) => _loadingText.text = text;

	private static void OnNullInstance() => Debug.LogError("Loading Screen: the object is not placed on the scene. ### Launch 'Login' scene fist ###");
	private static void OnObjectDisabled() => Debug.LogError("Loading Screen: the object is not active. First call the Show method.");

	private IEnumerator LoadingCoroutine(float seconds)
	{
		Show();

		float secondsPerPercent = seconds / (PROGRESS_BAR_MAX_VALUE - PROGRESS_BAR_MIN_VALUE);
		int currentPercents = PROGRESS_BAR_MIN_VALUE;

		while (currentPercents < PROGRESS_BAR_MAX_VALUE)
		{
			currentPercents += 1;
			UpdateProgressBar(currentPercents);

			yield return new WaitForSeconds(secondsPerPercent);
		}

		yield return new WaitForSeconds(secondsPerPercent * 5);

		Hide();
	}
}