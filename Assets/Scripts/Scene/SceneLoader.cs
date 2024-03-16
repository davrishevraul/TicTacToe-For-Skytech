using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : PersistentMonoBehaviourSingleton<SceneLoader>
{
	[SerializeField] private int _mainMenuBuildIndex;

	[SerializeField, Range(0, 5.0f)] private float _loadSceneDelay = 1.0f;

	public static event Action LoadingStarted;

	public static event Action LoadingFinished;

	public void LoadMainMenu() => LoadAsync(_mainMenuBuildIndex);

	public void LoadNextScene()
	{
		int allScenesCount = SceneManager.sceneCountInBuildSettings;
		int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

		if (nextSceneIndex >= allScenesCount)
		{
			Debug.LogError("SceneLoader: There are no next scene.");
			return;
		}

		LoadAsync(nextSceneIndex);
	}

	public void LoadPreviousScene()
	{
		int previousSceneIndex = SceneManager.GetActiveScene().buildIndex - 1;

		if (previousSceneIndex < 0)
		{
			Debug.LogError("SceneLoader: There are no previous scene.");
		}

		LoadAsync(previousSceneIndex);
	}

	public void RestartScene() => LoadAsync(SceneManager.GetActiveScene().buildIndex);

	public void LoadAsync(int scenebuildIndex)
	{
		if (scenebuildIndex > SceneManager.sceneCountInBuildSettings)
		{
			Debug.LogError("The scene was not added to the build settings.");
			return;
		}

		StartCoroutine(LoadSceneAsync(scenebuildIndex));
	}

	private void OnStartLoading() => LoadingStarted?.Invoke();

	private void OnFinishLoading() => LoadingFinished?.Invoke();

	private IEnumerator LoadSceneAsync(int sceneBuildIndex)
	{
		OnStartLoading();

		var asyncLoad = SceneManager.LoadSceneAsync(sceneBuildIndex);
		asyncLoad.allowSceneActivation = false;

		while (!asyncLoad.isDone)
		{
			if (asyncLoad.progress >= 0.9f)
			{
				yield return new WaitForSecondsRealtime(_loadSceneDelay);

				asyncLoad.allowSceneActivation = true;
			}

			yield return null;
		}

		OnFinishLoading();
	}
}