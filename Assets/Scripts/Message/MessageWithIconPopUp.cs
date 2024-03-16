using UnityEngine;
using UnityEngine.UI;

public class MessageWithIconPopUp : MessagePopUp
{
	[SerializeField] private Image _icon;

	public void SetIcon(Sprite icon) => _icon.sprite = icon;
}