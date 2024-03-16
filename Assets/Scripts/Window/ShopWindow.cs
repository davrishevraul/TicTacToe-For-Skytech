using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ShopWindow : MonoBehaviour
{
	[Header("Prefabs")]
	[SerializeField] private ShopSingleItem _singleItemPrefab;
	[SerializeField] private ShopPackItem _packItemPrefab;
	[SerializeField] private LayoutGroup _itemsParent;

	[Header("Data")]
	[SerializeField, TextArea(1, 15)] private string _itemsJsonData;

	[Header("Loading")]
	[SerializeField] private GameObject _loading;
	[SerializeField, Range(0.1f, 10.0f)] private float _loadingDuration = 3.0f;

	private JArray _singleItems = new();
	private JArray _packItems = new();

	private void Start()
	{
		StartCoroutine(nameof(LoadData));
	}

	private IEnumerator LoadData()
	{
		yield return new WaitForEndOfFrame();

		LoadingScreenSystem.ShowForSeconds(1.5f);
		LoadingScreenSystem.UpdateLoadingText("Processing shop items...");

		yield return new WaitForSecondsRealtime(0.5f);

		ProcessData();

		InstantiateSingleItems();
		InstantiatePackItems();
	}

	private void ProcessData()
	{
		var jObject = JsonConvert.DeserializeObject<JObject>(_itemsJsonData);

		JArray shopItems = (JArray)jObject[JsonPropertyName.SHOP_ITEMS];

		foreach (var shopItem in shopItems)
		{
			if ($"{shopItem[JsonPropertyName.TYPE]}" == "single")
			{
				_singleItems.Add(shopItem);
			}
			else if ($"{shopItem[JsonPropertyName.TYPE]}" == "pack")
			{
				_packItems.Add(shopItem);
			}
		}
	}

	private void InstantiateSingleItems()
	{
		foreach (var shopItem in _singleItems)
		{
			var item = Instantiate(_singleItemPrefab, _itemsParent.transform);

			item.SetNameText($"{shopItem[JsonPropertyName.KEY]}");
			item.SetAmountText($"{shopItem[JsonPropertyName.AMOUNT]}");
			item.SetPriceText($"{shopItem[JsonPropertyName.PRICE]} {shopItem[JsonPropertyName.CURRENCY]}");

			item.SetButButtonAction(() => OnClickBuy($"{shopItem[JsonPropertyName.KEY]}"));
		}
	}

	private void InstantiatePackItems()
	{
		foreach (var shopItem in _packItems)
		{
			var item = Instantiate(_packItemPrefab, _itemsParent.transform);

			item.SetNameText($"{shopItem[JsonPropertyName.KEY]}");
			item.SetPriceText($"{shopItem[JsonPropertyName.PRICE]} {shopItem[JsonPropertyName.CURRENCY]}");

			item.InstantiateItems((JArray)shopItem[JsonPropertyName.ITEMS]);

			item.SetButButtonAction(() => OnClickBuy($"{shopItem[JsonPropertyName.KEY]}"));
		}
	}

	private void OnClickBuy(string itemKey)
	{
		string question = $"Do you want to buy this item: {itemKey}?";

		MessageSystem.ShowWithQuestionYesOrNo(question, () => OnAcceptPurchase(itemKey));
	}

	private void OnAcceptPurchase(string itemKey)
	{
		StartCoroutine(FakePurchaseOperation(itemKey));
	}

	private IEnumerator FakePurchaseOperation(string itemKey)
	{
		SetActivateLoading(true);

		yield return new WaitForSecondsRealtime(_loadingDuration);

		SetActivateLoading(false);

		MessageSystem.Show("You have successfully purchased the item: " + itemKey);
	}

	private void SetActivateLoading(bool active) => _loading.SetActive(active);
}