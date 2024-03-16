using System;
using UnityEngine;

public class MessageSystem : PersistentMonoBehaviourSingleton<MessageSystem>
{
	[Header("Pop Ups")]
	[SerializeField] private MessagePopUp _messagePopUp;
	[SerializeField] private MessageWithIconPopUp _messageWithIconPopUp;
	[SerializeField] private MessageWithTwoButtonChoicePopUp _messageWithTwoButtonChoice;

	public const int MAX_MESSAGE_LENGTH = 200;

	public static event Action Showed;

	public static void Show(string message)
	{
		if (Instance == null)
		{
			OnNullInstance();
			return;
		}

		if (message == null)
		{
			Debug.LogWarning("Message must be not null.");
			return;
		}

		message = GetTrimmedMessage(message);

		Instance._messagePopUp.Set(message);

		ShowPopUp(Instance._messagePopUp);
	}

	public static void ShowWithIcon(string message, Sprite icon)
	{
		if (Instance == null)
		{
			OnNullInstance();
			return;
		}

		if (message == null)
		{
			Debug.LogWarning("Message must be not null.");
			return;
		}

		if (icon == null)
		{
			Debug.LogWarning("Icon must be not null.");
			return;
		}

		message = GetTrimmedMessage(message);

		Instance._messageWithIconPopUp.Set(message);
		Instance._messageWithIconPopUp.SetIcon(icon);

		ShowPopUp(Instance._messageWithIconPopUp);
	}

	public static void ShowWithTwoButtonChoice(string message, Action buttonAction1, Action buttonAction2, string buttonText1, string buttonText2)
	{
		if (Instance == null)
		{
			OnNullInstance();
			return;
		}

		if (message == null)
		{
			Debug.LogWarning("Message must be not null.");
			return;
		}

		message = GetTrimmedMessage(message);

		Instance._messageWithTwoButtonChoice.Set(message);
		Instance._messageWithTwoButtonChoice.SetButtonsOnClickAction(buttonAction1, buttonAction2);
		Instance._messageWithTwoButtonChoice.SetButtonsText(buttonText1, buttonText2);

		ShowPopUp(Instance._messageWithTwoButtonChoice);
	}

	public static void ShowWithQuestionYesOrNo(string message, Action onClickYes) => ShowWithTwoButtonChoice(message, onClickYes, null, "Yes", "No");

	private static void ShowPopUp(PopUp popUp)
	{
		popUp.gameObject.SetActive(true);

		Showed?.Invoke();
	}

	private static string GetTrimmedMessage(string message)
	{
		if (message.Length > MAX_MESSAGE_LENGTH)
		{
			Debug.LogWarning("Message length too long.");
			return message[..MAX_MESSAGE_LENGTH];
		}

		return message;
	}

	private static void OnNullInstance() => Debug.LogError("Message System: the object is not placed on the scene. ### Launch 'Login' scene fist ###");
}