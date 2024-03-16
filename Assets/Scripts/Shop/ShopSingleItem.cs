using TMPro;
using UnityEngine;

public class ShopSingleItem : ShopItem
{
	[SerializeField] private TMP_Text _amountText;

	public void SetAmountText(string text) => _amountText.text = text;
}