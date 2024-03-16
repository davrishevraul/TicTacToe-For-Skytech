using UnityEngine;

public class PersistentMonoBehaviourSingleton<T> : MonoBehaviour where T : Component
{
	public static T Instance { get; private set; }

	protected virtual void Awake()
	{
		if (Instance != null)
		{
			Destroy(gameObject);
			return;
		}

		Instance = this as T;

		transform.parent = null;
		DontDestroyOnLoad(gameObject);
	}
}