using TMPro;
using UnityEngine;

public class PackItem : MonoBehaviour
{
	[Header("UI References")]
	[SerializeField] private TMP_Text _keyText;
	[SerializeField] private TMP_Text _valueText;

	public void SetKeyText(string text) => _keyText.text = text;
	public void SetValueText(string text) => _valueText.text = text;
}
