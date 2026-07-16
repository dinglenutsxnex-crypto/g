using System;
using Nekki.Yaml;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Nekki.Yaml;
using Nekki;
using Nekki.Yaml;
using SF3.Items;
using sf3DTO;

namespace SF3.UserData
{
	public class UserShopData
	{
		private Dictionary<ShopCategoryType, UserShopCategory> _userShopCategories;

		private Sequence _shopDataNode;

		private Action<bool> _OnDataUpdate = delegate
		{
		};

		public UserShopData(Sequence shopDataNodeValue, bool parseNode, Action<bool> OnConfigurationUpdateCallback = null)
		{
			_shopDataNode = shopDataNodeValue;
			_OnDataUpdate = OnConfigurationUpdateCallback;
			_userShopCategories = new Dictionary<ShopCategoryType, UserShopCategory>();
			if (!parseNode)
			{
				return;
			}
			foreach (Mapping item in _shopDataNode.nodesInside)
			{
				UserShopCategory userShopCategory = new UserShopCategory(item, _OnDataUpdate);
				_userShopCategories.Add(userShopCategory.categoryType, userShopCategory);
			}
		}

		public void UpdateData(Shop shopData)
		{
			List<sf3DTO.ShopItem> list = shopData.Items?.ToList() ?? new List<sf3DTO.ShopItem>();
			Dictionary<ShopCategoryType, List<ShopItem>> dictionary = new Dictionary<ShopCategoryType, List<ShopItem>>();
			foreach (ShopCategoryType enumerator5 in EnumsCompliancer.GetEnumerators<ShopCategoryType>())
			{
				if (enumerator5 != 0)
				{
					dictionary.Add(enumerator5, new List<ShopItem>());
				}
			}
			foreach (sf3DTO.ShopItem item3 in list)
			{
				BaseItem item = ItemsManager.GetItem(item3);
				if (item != null)
				{
					ShopItem item2 = new ShopItem(item, item3.PurchaseCount);
					if (dictionary.ContainsKey(ShopCategoryType.Equipment))
					{
						dictionary[ShopCategoryType.Equipment].Add(item2);
					}
				}
			}
			_userShopCategories.Clear();
			_shopDataNode.nodesInside.Clear();
			foreach (var item5 in dictionary)
			{
				if (item5.Value.Count != 0)
				{
					Mapping mapping = new Mapping("Category", new Scalar("Category", item5.Key.ToString()));
					mapping.Add(new Scalar("NewItemNotification", "1"));
					mapping.Add(new Sequence("Items", item5.Value.Select((ShopItem itemValue) => itemValue.ToYAML()).Cast<YamlNode>().ToList()));
					_shopDataNode.AddNode(mapping);
					UserShopCategory userShopCategory = new UserShopCategory(mapping, item5.Key, item5.Value, _OnDataUpdate);
					_userShopCategories.Add(userShopCategory.categoryType, userShopCategory);
				}
			}
			_OnDataUpdate(false);
		}

		public UserShopCategory GetShopCategoryData(ShopCategoryType categoryType)
		{
			UserShopCategory result = null;
			if (_userShopCategories.ContainsKey(categoryType))
			{
				result = _userShopCategories[categoryType];
			}
			return result;
		}
	}
}
