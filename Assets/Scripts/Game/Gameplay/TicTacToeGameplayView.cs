using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TicTacToeGameplayView : MonoBehaviour
{
	[Header("Sprites")]
	[SerializeField] private Sprite _spriteX;
	[SerializeField] private Sprite _spriteO;

	[Header("Colors")]
	[SerializeField] private Color32 _colorX = new(255, 127, 0, 255);
	[SerializeField] private Color32 _colorO = new(0, 200, 255, 255);

	[Header("Win Lines")]
	[SerializeField] private List<Image> _winLines;

	[Header("Players Info")]
	[SerializeField] private PlayerInfoView _playerInfo;
	[SerializeField] private PlayerInfoView _botInfo;

	[Header("Result Panel")]
	[SerializeField] private PanelGameResult _gameResultPanel;
	[SerializeField, Range(1.0f, 3.0f)] private float _showGameResultPanelDelay = 2.0f;

	public Sprite SpriteX => _spriteX;
	public Sprite SpriteO => _spriteO;

	public Color32 ColorX => _colorX;
	public Color32 ColorO => _colorO;

	private void Start() => _playerInfo.UpdateNicknameText(GlobalData.PlayerNickName);

	public void SetActivatePlayerTurnIcon(bool active) => _playerInfo.SetActivateTurnIcon(active);

	public void UpdatePlayerMarkIcon(Sprite sprite, Color32 color)
	{
		_playerInfo.UpdateMarkIconSprite(sprite);
		_playerInfo.UpdateMarkIconColor(color);
	}

	public void SetActivateBotTurnIcon(bool active) => _botInfo.SetActivateTurnIcon(active);

	public void UpdateBotMarkIcon(Sprite sprite, Color32 color)
	{
		_botInfo.UpdateMarkIconSprite(sprite);
		_botInfo.UpdateMarkIconColor(color);
	}

	public void ShowWinLine(TicTacToeMark winnedMark, int lineID)
	{
		if (lineID < 0 || lineID > 7)
		{
			Debug.LogError("Incorrect line ID");
			return;
		}

		var line = _winLines[lineID];

		switch (winnedMark)
		{
			case TicTacToeMark.X:
				{
					line.color = _colorX;
					break;
				}
			case TicTacToeMark.O:
				{
					line.color = _colorO;
					break;
				}
		}

		line.gameObject.SetActive(true);
	}

	public void DisableAllWinLines()
	{
		foreach (var line in _winLines)
		{
			line.gameObject.SetActive(false);
		}
	}

	public void ShowGameResultPanel() => StartCoroutine(nameof(ShowGameResultPanelAfterDelay));

	private IEnumerator ShowGameResultPanelAfterDelay()
	{
		yield return new WaitForSecondsRealtime(_showGameResultPanelDelay);
		_gameResultPanel.Show();
	}
}