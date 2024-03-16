using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MessageWithTwoButtonChoicePopUp : MessagePopUp
{
	[Header("Buttons")]

	[SerializeField] private Button _button1;
	[SerializeField] private TMP_Text _buttonText1;

	[SerializeField] private Button _button2;
	[SerializeField] private TMP_Text _buttonText2;

	public void SetButtonsOnClickAction(Action buttonAction1, Action buttonAction2)
	{
		ResetButtonListener(_button1, buttonAction1);
		ResetButtonListener(_button2, buttonAction2);
	}

	public void SetButtonsText(string buttonText1, string buttonText2)
	{
		_buttonText1.text = buttonText1;
		_buttonText2.text = buttonText2;
	}

	private void ResetButtonListener(Button button, Action onClick)
	{
		button.onClick.RemoveAllListeners();
		button.onClick.AddListener(() => onClick?.Invoke());
		button.onClick.AddListener(() => SetActivatePopUp(false));
	}

	private void SetActivatePopUp(bool active) => gameObject.SetActive(active);
}
