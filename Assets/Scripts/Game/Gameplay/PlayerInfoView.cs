using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoView : MonoBehaviour
{
	[Header("References")]
	[SerializeField] private TMP_Text _nickname;
	[SerializeField] private Image _turnIcon;
	[SerializeField] private Image _markIcon;

	public void SetActivateTurnIcon(bool active) => _turnIcon.gameObject.SetActive(active);

	public void UpdateMarkIconSprite(Sprite sprite) => _markIcon.sprite = sprite;

	public void UpdateMarkIconColor(Color32 color) => _markIcon.color = color;

	public void UpdateNicknameText(string nickname) => _nickname.text = nickname;
}