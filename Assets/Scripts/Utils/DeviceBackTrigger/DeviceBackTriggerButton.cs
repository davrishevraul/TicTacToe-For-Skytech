using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class DeviceBackTriggerButton : MonoBehaviour
{
	public Button Button { get; private set; }

	private void Awake()
	{
		Button = GetComponent<Button>();
	}

	private void OnEnable() => DeviceBackTriggerSystem.AddToStack(Button);

	private void OnDisable() => DeviceBackTriggerSystem.RemoveFromStack();
}