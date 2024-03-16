using UnityEngine;

public static class GlobalData
{
	public const string DEFAULT_NICKNAME = "Player#1";
	public const string PLAYER_NICKNAME_PLAYER_PREFS = "PLAYER_NICKNAME";

	public static string PlayerNickName { get; set; }
	public static GameStatistic LastGameStatistic { get; set; }

	public static bool IsBotHard { get; set; }

	[RuntimeInitializeOnLoadMethod]
	private static void LoadNicknameFromPlayerPrefs() => PlayerNickName = PlayerPrefs.GetString(PLAYER_NICKNAME_PLAYER_PREFS, DEFAULT_NICKNAME);

	public static void UpdateNickname(string nickname)
	{
		if (string.IsNullOrEmpty(nickname))
		{
			Debug.LogError("Incorrect nickname format.");
			return;
		}

		PlayerNickName = nickname;

		SaveNicknameToPlayerPrefs();
	}

	private static void SaveNicknameToPlayerPrefs()
	{
		PlayerPrefs.SetString(PLAYER_NICKNAME_PLAYER_PREFS, PlayerNickName);
		PlayerPrefs.Save();
	}
}