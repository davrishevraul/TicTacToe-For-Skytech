using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeviceBackTriggerSystem : PersistentMonoBehaviourSingleton<DeviceBackTriggerSystem>
{
	private static readonly Stack<Button> _stack = new();

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			InvokeCurrentTrigger();
		}
	}

	public static void AddToStack(Button button)
	{
		if (button == null)
		{
			return;
		}

		_stack.Push(button);
	}

	public static void RemoveFromStack() => _stack.Pop();

	private void InvokeCurrentTrigger()
	{
		if (_stack.Count < 1)
		{
			return;
		}

		_stack.Peek().onClick.Invoke();
	}
}