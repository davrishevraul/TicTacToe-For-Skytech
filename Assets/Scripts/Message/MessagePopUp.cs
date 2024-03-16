using TMPro;
using UnityEngine;

public class MessagePopUp : PopUp
{
	[SerializeField] private TMP_Text _message;

	public void Set(string message) => SetMessage(message);

	private void SetMessage(string message) => _message.text = message;
}