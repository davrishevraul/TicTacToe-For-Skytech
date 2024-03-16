using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ShopPackItem : ShopItem
{
	[Header("Items")]
	[SerializeField] private PackItem _packItemPrefab;
	[SerializeField] private LayoutGroup _itemsParent;

	public void InstantiateItems(JArray items)
	{
		foreach (var item in items)
		{
			var itemObject = Instantiate(_packItemPrefab, _itemsParent.transform);
			itemObject.SetKeyText($"{item[JsonPropertyName.KEY]}");

			JObject infoObjects = (JObject)item[JsonPropertyName.INFO];

			foreach (var infoObject in infoObjects)
			{
				itemObject.SetValueText($"{infoObject.Key}: {infoObject.Value}");
				break;
			}
		}
	}
}