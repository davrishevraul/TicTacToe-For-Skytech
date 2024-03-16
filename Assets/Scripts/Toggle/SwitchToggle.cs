using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class SwitchToggle : MonoBehaviour
{
	[SerializeField] private RectTransform _handle;

	[SerializeField, Range(0, 1)] private float _animationDuration = 0.1f;

	public Toggle Toggle { get; private set; }

	private Vector2 _handlePosition;

	private void Awake()
	{
		Toggle = GetComponent<Toggle>();

		_handlePosition = _handle.anchoredPosition;

		Toggle.onValueChanged.AddListener(Switch);
	}

	private void OnEnable()
	{
		Switch(Toggle.isOn);
	}

	private void OnDestroy()
	{
		Toggle.onValueChanged.RemoveListener(Switch);

		_handle.DOKill();
	}

	protected virtual void Switch(bool isOn)
	{
		_handle.DOAnchorPos(isOn ? -(_handlePosition) : _handlePosition, _animationDuration);
	}
}