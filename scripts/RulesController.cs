using System;
using System.Collections.Generic;
using Godot;
using Nekki.Yaml;
using SF3;
using SF3.GameModels;
using SF3.Items;
using sf3DTO;

public class RulesController
{
	private Dictionary<string, string> _ruleAttributesMap;

	private Dictionary<string, Rule> _rules;

	private ModelInfo _playerInfo;

	private ModelInfo _enemyInfo;

	private RoundInfo _round;

	public Action ClearRules;

	private static RulesController _instance;

	public static RulesController Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new RulesController();
			}
			return _instance;
		}
	}

	public RulesController()
	{
		_rules = new Dictionary<string, Rule>();
		_ruleAttributesMap = new Dictionary<string, string>();
		InitGlobalRules();
	}

	public static void Clear()
	{
		_instance = null;
	}

	public void Initialize(RoundInfo round, ModelInfo playerInfo, ModelInfo enemyInfo)
	{
		_round = round;
		_playerInfo = playerInfo;
		_enemyInfo = enemyInfo;
		ApplyToRules();
		StartRules();
	}

	public bool HasActiveRule(string ruleName)
	{
		ruleName = ruleName.ToLower();
		return _round.rules.Find((Rule e) => e.Type == ruleName) != null;
	}

	private void ApplyToRules()
	{
		_playerInfo.rules.Clear();
		_enemyInfo.rules.Clear();
		foreach (Rule rule in _round.rules)
		{
			if (rule.playerType == EPlayerType.This)
			{
				_playerInfo.rules.Add(rule);
			}
			else
			{
				_enemyInfo.rules.Add(rule);
			}
		}
	}

	private void StartRules()
	{
		ClearRules = null;
		StartRulesModel(_playerInfo, 1);
		StartRulesModel(_enemyInfo, 2);
	}

	private void StartRulesModel(ModelInfo modelInfo, int modelID)
	{
		foreach (Rule rule in modelInfo.rules)
		{
			rule.Cache(modelInfo);
			ClearRules = (Action)Delegate.Combine(ClearRules, new Action(rule.Reset));
		}
		StartApplyGender(modelInfo);
		StartApplyAlias(modelInfo);
		StartApplyItem(modelInfo);
		StartApplyPerk(modelInfo);
		StartApplyTag(modelInfo);
	}

	private void StartApplyAlias(ModelInfo modelInfo)
	{
		List<Rule> rulesByType = modelInfo.GetRulesByType("SetAlias");
		foreach (Rule item in rulesByType)
		{
			modelInfo.SetAlias(item.GetAttributeStringByType("Alias"));
		}
	}

	private void StartApplyGender(ModelInfo modelInfo)
	{
		List<Rule> rulesByType = modelInfo.GetRulesByType("SetGender");
		foreach (Rule item in rulesByType)
		{
			modelInfo.SetGender((Gender)item.GetAttributeNumberByType("Gender").Value);
		}
	}

	private void StartApplyItem(ModelInfo modelInfo)
	{
		List<Rule> rulesByType = modelInfo.GetRulesByType("GiveItem");
		foreach (Rule item in rulesByType)
		{
			double? attributeNumberByType = item.GetAttributeNumberByType("ID");
			if (attributeNumberByType.HasValue)
			{
				Equipment equipment;
				if (ItemsManager.TryGetEquipmentById((int)attributeNumberByType.Value, out equipment))
				{
					modelInfo.EquipItemNotExisted(equipment);
				}
			}
		}
	}

	private void StartApplyPerk(ModelInfo modelInfo)
	{
		List<Rule> rulesByType = modelInfo.GetRulesByType("GivePerk");
		foreach (Rule item in rulesByType)
		{
			double? attributeNumberByType = item.GetAttributeNumberByType("ID");
			if (attributeNumberByType.HasValue)
			{
				SF3.Items.Perk perk;
				if (ItemsManager.TryGetPerkById((int)attributeNumberByType.Value, out perk))
				{
					modelInfo.AddPerkInCollection(perk);
				}
			}
		}
	}

	private void StartApplyTag(ModelInfo modelInfo)
	{
		List<Rule> rulesByType = modelInfo.GetRulesByType("GiveTag");
		foreach (Rule item in rulesByType)
		{
			string attributeStringByType = item.GetAttributeStringByType("Tag");
			if (!attributeStringByType.Equals(string.Empty))
			{
				modelInfo.AddTag(attributeStringByType);
			}
		}
	}

	private void InitGlobalRules()
	{
		foreach (var globalRule in JS.Instance.GetScope("RoundRules").AsDictionary())
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			var dictionary2 = globalRule.Value.AsDictionary();
			string text = dictionary2["ID"].ToString();
			string key = globalRule.Key;
			_rules.Add(text, Rule.RuleFabric(text, key, dictionary));
		}
	}

	public Rule GetGlobalRule(string ID)
	{
		if (_rules.ContainsKey(ID))
		{
			return _rules[ID];
		}
		GD.PrintErr("Rule:" + ID + " Not Found");
		return null;
	}

	public List<RoundRule> ConvertYaml(Sequence rulesNode)
	{
		List<RoundRule> list = new List<RoundRule>();
		if (rulesNode != null)
		{
			foreach (Mapping item in rulesNode.nodesInside)
			{
				foreach (Mapping item2 in item.nodesInside)
				{
					RoundRule roundRule = new RoundRule();
					roundRule.RuleId = int.Parse(item2.key);
					foreach (var item3 in item2.nodesInside)
					{
						RoundRuleAttribute roundRuleAttribute = new RoundRuleAttribute();
						roundRuleAttribute.AttrId = int.Parse(item3.key);
						roundRuleAttribute.AttrValue = item3.value.ToString();
						roundRule.Attrs.Add(roundRuleAttribute);
					}
					list.Add(roundRule);
				}
			}
		}
		return list;
	}
}
