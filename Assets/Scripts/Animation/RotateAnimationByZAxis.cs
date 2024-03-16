using DG.Tweening;
using UnityEngine;

public class RotateAnimationByZAxis : MonoBehaviour
{
	[SerializeField, Range(0.1f, 5.0f)] private float _rotationSpeed = 2.0f;

	private Vector3 _rotationEndValue = Vector3.back * 360;

	private Tween _tween;

	private void OnEnable() => StartRotating();
	private void OnDisable() => StopRotating();

	private void StartRotating()
	{
		transform.localEulerAngles = new Vector3(transform.localRotation.x, transform.localRotation.y, 0);

		_tween = transform.DOLocalRotate(_rotationEndValue, _rotationSpeed, RotateMode.FastBeyond360)
			.SetLoops(-1)
			.SetEase(Ease.Linear);
	}

	private void StopRotating() => _tween?.Kill();
}