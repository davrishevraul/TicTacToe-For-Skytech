using System;
using System.Collections;
using UnityEngine;

public class MainMenuLastGameResultHandler : MonoBehaviour
{
	[SerializeField, Range(1.0f, 10.0f)] private float _showDelay = 2.0f;

	private void Start()
	{
		if (GlobalData.LastGameStatistic == null)
		{
			return;
		}

		if (GlobalData.LastGameStatistic.PlayedGames == 0)
		{
			return;
		}

		StartCoroutine(nameof(Show));
	}

	private IEnumerator Show()
	{
		yield return new WaitForSeconds(_showDelay);

		var statistic = GlobalData.LastGameStatistic;

		string message =
			$"Last Game \n\n winner: {statistic.WinnerNickName} " +
			$"\n played games: {statistic.PlayedGames} " +
			$"\n wins: {statistic.Winnings} " +
			$"\n loses: {statistic.Loses} " +
			$"\n played time: {TimeSpan.FromSeconds(statistic.TimeInGame).ToString(Stopwatch.STRING_FORMAT)}";

		MessageSystem.Show(message);
	}
}