using TMPro;
using UnityEngine;

public class PlayerNicknameInputField : MonoBehaviour
{
	[SerializeField] private TMP_InputField _inputField;

	[SerializeField] private string _prefixOnMinLength = "Player#";
	[SerializeField, Range(2, 5)] private int _minLength = 2;

	private void Start() => _inputField.text = GlobalData.PlayerNickName;

	private void OnEnable() => _inputField.onEndEdit?.AddListener(OnEndEdit);

	private void OnDisable() => _inputField.onEndEdit?.RemoveListener(OnEndEdit);

	private void OnEndEdit(string text)
	{
		var formattedText = GetFormattedText();

		_inputField.text = formattedText;

		UpdateNicknameData(formattedText);
	}

	private void UpdateNicknameData(string nickname)
	{
		if (nickname == GlobalData.PlayerNickName)
		{
			return;
		}

		GlobalData.UpdateNickname(nickname);
	}

	private string GetFormattedText()
	{
		string text = _inputField.text;

		text = text.Trim();
		text = text.Replace(" ", "");

		if (text.Length <= _minLength)
		{
			text = _prefixOnMinLength + text;
		}

		return text;
	}
}