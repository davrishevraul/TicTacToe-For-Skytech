using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class ShopItem : MonoBehaviour
{
	[Header("UI References")]
	[SerializeField] private TMP_Text _nameText;
	[SerializeField] private TMP_Text _priceText;

	[SerializeField, Space(10)] private Button _buyButton;

	public void SetNameText(string text) => _nameText.text = text;
	public void SetPriceText(string text) => _priceText.text = text;
	public void SetButButtonAction(Action onClick) => _buyButton.onClick.AddListener(() => onClick?.Invoke());
}