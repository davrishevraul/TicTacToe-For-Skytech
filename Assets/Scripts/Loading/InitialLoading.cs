using UnityEngine;

public class InitialLoading : MonoBehaviour
{
	[SerializeField, Range(1.0f, 10.0f)] private float _loadingDurationInSeconds = 2.0f;

	private void Start()
	{
		SceneLoader.Instance.LoadMainMenu();

		LoadingScreenSystem.ShowForSeconds(_loadingDurationInSeconds);
	}
}
