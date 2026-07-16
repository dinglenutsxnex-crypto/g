using System;
using Nekki.Yaml;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Nekki.Yaml;
using Nekki;
using Nekki.Yaml;
using SF3.Items;
using SimpleJSON;
using sf3DTO;

public class FightRewards
{
	public class RoundReward
	{
		public List<Equipment> equipments { get; private set; }

		public List<SF3.Items.Perk> perks { get; private set; }

		public List<SF3.Items.Booster> boosters { get; private set; }

		public List<Currency> currencies { get; private set; }

		public long experience { get; private set; }

		public RoundReward()
		{
			experience = 0L;
			currencies = new List<Currency>();
			equipments = new List<Equipment>();
			perks = new List<SF3.Items.Perk>();
			boosters = new List<SF3.Items.Booster>();
		}

		public static RoundReward Create(Loot lootData)
		{
			RoundReward resultLoot = new RoundReward();
			if (lootData.Equipments.Count > 0)
			{
				lootData.Equipments.ForEach(delegate(Item equipmentValue)
				{
					resultLoot.equipments.Add(Equipment.Create(equipmentValue));
				});
			}
			if (lootData.Perks.Count > 0)
			{
				resultLoot.perks.AddRange(lootData.Perks.Select(SF3.Items.Perk.Create).ToList());
			}
			if (lootData.Boosters.Count > 0)
			{
				resultLoot.boosters.AddRange(SF3.Items.Booster.Create(lootData.Boosters));
			}
			if (lootData.Currencies.Count > 0)
			{
				resultLoot.currencies.AddRange(lootData.Currencies);
			}
			resultLoot.experience = lootData.Experience;
			return resultLoot;
		}

		public static RoundReward Create(Mapping lootData)
		{
			RoundReward roundReward = new RoundReward();
			foreach (var item in lootData.nodesInside)
			{
				switch (item.key)
				{
				case "Currencies":
				{
					Sequence sequence4 = item as Sequence;
					foreach (Mapping item2 in sequence4.nodesInside)
					{
						roundReward.currencies.Add(new Currency
						{
							CurrencyType = EnumsCompliancer.GetEnumerator<CurrencyType>(item2.GetText("Type").text),
							Value = long.Parse(item2.GetText("Value").text)
						});
					}
					break;
				}
				case "Experience":
				{
					Scalar scalar = item as Scalar;
					roundReward.experience = long.Parse(scalar.text);
					break;
				}
				case "Equipments":
				{
					Sequence sequence3 = item as Sequence;
					foreach (Mapping item3 in sequence3.nodesInside)
					{
						Equipment equipment;
						ItemsManager.TryGetEquipmentById(int.Parse(item3.GetText("ID").text), out equipment);
						equipment.SetStackLevel(double.Parse(item3.GetText("StackLevel").text));
						roundReward.equipments.Add(equipment);
					}
					break;
				}
				case "Perks":
				{
					Sequence sequence2 = item as Sequence;
					foreach (Mapping item4 in sequence2.nodesInside)
					{
						SF3.Items.Perk perk;
						ItemsManager.TryGetPerkById(int.Parse(item4.GetText("ID").text), out perk);
						perk.SetStackLevel(double.Parse(item4.GetText("StackLevel").text));
						roundReward.perks.Add(perk);
					}
					break;
				}
				}
			}
			return roundReward;
		}

		public RoundReward Clone()
		{
			RoundReward result = MemberwiseClone() as RoundReward;
			result.currencies = new List<Currency>(currencies);
			result.equipments = new List<Equipment>(equipments);
			result.perks = new List<SF3.Items.Perk>(perks);
			return result;
		}

		public Mapping ToYAML()
		{
			List<YamlNode> list = new List<YamlNode>();
			list.Add(new Scalar("Experience", experience.ToString()));
			if (currencies.Count > 0)
			{
				List<YamlNode> list2 = new List<YamlNode>(currencies.Count);
				foreach (Currency currency in currencies)
				{
					list2.Add(new Mapping("Currency", new List<YamlNode>
					{
						new Scalar("Type", currency.CurrencyType.ToString()),
						new Scalar("Value", currency.Value.ToString())
					}));
				}
				list.Add(new Sequence("Currencies", list2));
			}
			if (equipments.Count > 0)
			{
				List<YamlNode> list3 = new List<YamlNode>(equipments.Count);
				foreach (Equipment equipment in equipments)
				{
					list3.Add(new Mapping("Equipment", equipment.ToYaml()));
				}
				list.Add(new Sequence("Equipments", list3));
			}
			if (perks.Count > 0)
			{
				List<YamlNode> list4 = new List<YamlNode>(perks.Count);
				foreach (SF3.Items.Perk perk in perks)
				{
					list4.Add(new Mapping("Perk", perk.ToYaml()));
				}
				list.Add(new Sequence("Perks", list4));
			}
			return new Mapping("FightReward", list);
		}
	}

	private List<RoundReward> _rewardsByRoundWins;

	private List<BaseRewardInfo> _rewardsIcon;

	public List<BaseRewardInfo> rewardsIcon
	{
		get
		{
			return _rewardsIcon;
		}
	}

	public FightRewards()
	{
		_rewardsByRoundWins = new List<RoundReward>();
	}

	public void SetRewards(List<Loot> loots)
	{
		_rewardsByRoundWins.Clear();
		loots.ForEach(delegate(Loot lootValue)
		{
			_rewardsByRoundWins.Add(RoundReward.Create(lootValue));
		});
	}

	public void SetRewards(Sequence loots)
	{
		_rewardsByRoundWins.Clear();
		loots.nodesInside.ForEach(delegate(YamlNode lootMap)
		{
			_rewardsByRoundWins.Add(RoundReward.Create((Mapping)lootMap));
		});
	}

	public RoundReward GetRewardsByRoundsWin(int roundsWin)
	{
		if (_rewardsByRoundWins.Count == 0)
		{
			return new RoundReward();
		}
		if (_rewardsByRoundWins.Count <= roundsWin)
		{
			return _rewardsByRoundWins[_rewardsByRoundWins.Count - 1];
		}
		return _rewardsByRoundWins[roundsWin];
	}

	public long GetCurrencyByRoundsWinForMultiplies(int roundsWin, CurrencyType type)
	{
		long currencyByRoundsWin = GetCurrencyByRoundsWin(roundsWin, type);
		if (currencyByRoundsWin > 0)
		{
			return currencyByRoundsWin;
		}
		for (int i = 0; i < _rewardsByRoundWins.Count; i++)
		{
			Currency currencyByTypeOrNull = GetCurrencyByTypeOrNull(_rewardsByRoundWins[i].currencies, type);
			if (currencyByTypeOrNull != null)
			{
				return currencyByTypeOrNull.Value;
			}
		}
		return currencyByRoundsWin;
	}

	public long GetCurrencyByRoundsWin(int roundsWin, CurrencyType type)
	{
		Currency currencyByTypeOrNull = GetCurrencyByTypeOrNull(GetRewardsByRoundsWin(roundsWin).currencies, type);
		return (currencyByTypeOrNull == null) ? 0 : currencyByTypeOrNull.Value;
	}

	private Currency GetCurrencyByTypeOrNull(IEnumerable<Currency> currency, CurrencyType type)
	{
		return currency.FirstOrDefault((Currency c) => c.CurrencyType == type && c.Value > 0);
	}

	public FightRewards Clone()
	{
		FightRewards result = MemberwiseClone() as FightRewards;
		result._rewardsByRoundWins = new List<RoundReward>(_rewardsByRoundWins);
		return result;
	}

	public bool HasRewards()
	{
		return _rewardsByRoundWins.Count > 0;
	}

	public Sequence ToYAML()
	{
		return new Sequence("Rewards", _rewardsByRoundWins.Select((RoundReward rewardValue) => rewardValue.ToYAML()).Cast<YamlNode>().ToList());
	}
}
