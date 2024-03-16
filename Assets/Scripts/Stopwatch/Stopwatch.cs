using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Stopwatch : MonoBehaviour
{
	[SerializeField] private TMP_Text _text;

	public int ElapsedTimeInSeconds { get; private set; }

	public const string STRING_FORMAT = @"mm\:ss";

	public bool IsPaused { get; private set; }

	public void Restart()
	{
		Stop();
		StartStopwatch();
	}

	public void Stop()
	{
		Pause();

		ElapsedTimeInSeconds = 0;
	}

	public void Pause()
	{
		StopCoroutine(nameof(CountingCoroutine));

		IsPaused = true;
	}

	public void Resume() => StartStopwatch();

	private void StartStopwatch() => StartCoroutine(nameof(CountingCoroutine));

	private void UpdateText(string value) => _text.text = value;

	private IEnumerator CountingCoroutine()
	{
		IsPaused = false;

		TimeSpan time;

		while (true)
		{
			yield return new WaitForSecondsRealtime(1);

			ElapsedTimeInSeconds++;

			time = TimeSpan.FromSeconds(ElapsedTimeInSeconds);

			UpdateText(time.ToString(STRING_FORMAT));
		}
	}
}