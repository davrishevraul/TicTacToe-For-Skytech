using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class GameGridCell : MonoBehaviour
{
	[SerializeField, Range(0, 8)] private int _id; // Starting from Top Left Corner
	[SerializeField] private TicTacToeGameplay _gameplaySystem;

	[Header("Mark")]
	[SerializeField] private Image _markIcon;

	public int ID => _id;
	public bool IsFilled { get; private set; } = false;

	public TicTacToeMark FilledMark { get; private set; }

	private Button _button;

	private void Awake()
	{
		_button = GetComponent<Button>();
		AddButtonListener();
	}

	private void Start() => SetActivateMarkIcon(false);

	public void Fill(TicTacToeMark mark, Sprite markSprite, Color32 markColor)
	{
		if (IsFilled)
		{
			Debug.LogError("Cell already filled.");
			return;
		}

		FilledMark = mark;
		IsFilled = true;

		UpdateMarkIconSprite(markSprite);
		UpdateMarkIconColor(markColor);
		SetActivateMarkIcon(true);
	}

	public void Clear()
	{
		IsFilled = false;
		SetActivateMarkIcon(false);
	}

	private void AddButtonListener() => _button.onClick.AddListener(OnClick);

	private void OnClick()
	{
		if (IsFilled)
		{
			return;
		}

		_gameplaySystem.OnClickCell(this);
	}

	private void SetActivateMarkIcon(bool active) => _markIcon.gameObject.SetActive(active);
	private void UpdateMarkIconSprite(Sprite sprite) => _markIcon.sprite = sprite;
	private void UpdateMarkIconColor(Color32 color) => _markIcon.color = color;
}