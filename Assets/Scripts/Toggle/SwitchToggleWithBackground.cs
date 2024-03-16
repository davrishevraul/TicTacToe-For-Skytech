using UnityEngine;
using UnityEngine.UI;

public class SwitchToggleWithBackground : SwitchToggle
{
	[Header("Background")]
	[Space(5)]
	[SerializeField] private Image _background;

	[SerializeField, ColorUsage(showAlpha: false)] private Color _activeColor = new(0.188f, 0.858f, 0.356f);
	[SerializeField, ColorUsage(showAlpha: false)] private Color _inactiveColor = new(0.682f, 0.682f, 0.698f);

	protected override void Switch(bool isOn)
	{
		base.Switch(isOn);

		_background.color = Toggle.isOn ? _activeColor : _inactiveColor;
	}
}
