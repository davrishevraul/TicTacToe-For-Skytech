using UnityEngine;

public class ApplicationQuitSystem : MonoBehaviour
{
	[SerializeField] private string _quitText = "Do you want to quit the game?";

	public void TryQuit() => MessageSystem.ShowWithQuestionYesOrNo(_quitText, Quit);

	private void Quit()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}
}