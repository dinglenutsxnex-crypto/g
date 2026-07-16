using System;
using Nekki.Yaml;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Nekki;
using Nekki.Yaml;
using SimpleJSON;
using sf3DTO;

public class GenericBattleInfo : IBattleInfo, ICloneable
{
	private List<BattleInfo> _battles;

	private int _currentBattleIndex;

	private int _battleCounter;

	private bool _available = true;

	private bool _hidden;

	private DateTime _expirationTime = DateTime.MinValue;

	private DateTime _genTime = DateTime.MinValue;

	private GenericBattleInfo()
	{
		_battles = new List<BattleInfo>();
		_battleCounter = 0;
		_currentBattleIndex = 0;
	}

	public GenericBattleInfo(List<BattleInfo> battleArr)
		: this()
	{
		_battles = battleArr;
	}

	public static GenericBattleInfo Create(Mapping battleMap)
	{
		GenericBattleInfo genericBattleInfo = new GenericBattleInfo();
		Sequence sequence = battleMap.GetSequence("Battles");
		foreach (Mapping item2 in sequence.nodesInside)
		{
			BattleInfo item = BattleInfo.Create(item2);
			genericBattleInfo._battles.Add(item);
		}
		genericBattleInfo.MergeWith(battleMap);
		return genericBattleInfo;
	}

	private void MergeWith(Mapping battleMap)
	{
		Scalar text = battleMap.GetText("CurrentBattleIndex");
		if (text != null)
		{
			SetBattleCounter(int.Parse(text.text));
		}
		text = battleMap.GetText("Available");
		if (text != null)
		{
			_available = text.text.Equals("1");
		}
		text = battleMap.GetText("Hidden");
		if (text != null)
		{
			_hidden = text.text.Equals("1");
		}
		text = battleMap.GetText("BattleCounter");
		if (text != null)
		{
			SetBattleCounter(int.Parse(text.text));
		}
		_battles[_currentBattleIndex].SetCurrentFight(0);
	}

	private void IncBattleCounter()
	{
		SetBattleCounter(_battleCounter + 1);
	}

	private void SetBattleCounter(int value)
	{
		_battleCounter = value;
		_currentBattleIndex = _battleCounter % _battles.Count;
	}

	public BattleInfo GetBattleInfo()
	{
		if (_battles.Count > 0)
		{
			return _battles[_currentBattleIndex];
		}
		return null;
	}

	public FightInfo GetCurrentFight()
	{
		return GetBattleInfo().GetCurrentFight();
	}

	public void SetTestBattleType()
	{
		foreach (BattleInfo battle in _battles)
		{
			battle.SetTestBattleType();
		}
	}

	public int GetID()
	{
		return _battles[_currentBattleIndex].id;
	}

	public BattleType GetBattleType()
	{
		return _battles[_currentBattleIndex].GetBattleType();
	}

	public List<FightInfo> GetFights()
	{
		return _battles[_currentBattleIndex].fights;
	}

	public int GetWonFights()
	{
		return _battles[_currentBattleIndex].wonFights;
	}

	public object Clone()
	{
		GenericBattleInfo genericBattleInfo = MemberwiseClone() as GenericBattleInfo;
		genericBattleInfo._battles = _battles.Select((BattleInfo bv) => bv.Clone() as BattleInfo).ToList();
		return genericBattleInfo;
	}

	public int GetBattleCounter()
	{
		return _battleCounter;
	}

	public Mapping ToYAML()
	{
		Mapping mapping = new Mapping("ID", new Scalar("ID", GetID().ToString()));
		mapping.Add(new Scalar("CurrentBattleIndex", _currentBattleIndex.ToString()));
		mapping.Add(new Scalar("Hidden", (!_hidden) ? string.Empty : "1"));
		mapping.Add(new Scalar("Available", _available ? string.Empty : "0"));
		mapping.Add(new Scalar("BattleCounter", _battleCounter.ToString()));
		mapping.Add(new Sequence("Battles", _battles.Select((BattleInfo bt) => bt.ToYAML()).Cast<YamlNode>().ToList()));
		return mapping;
	}

	public JSONClass ToJSON()
	{
		JSONClass json = new JSONClass();
		json["ID"] = new JSONData(GetID().ToString());
		json["CurrentBattleIndex"] = new JSONData(_currentBattleIndex.ToString());
		json["Hidden"] = new JSONData(_hidden ? "1" : string.Empty);
		json["Available"] = new JSONData(_available ? string.Empty : "0");
		json["BattleCounter"] = new JSONData(_battleCounter.ToString());
		JSONArray battlesArr = new JSONArray();
		foreach (var bt in _battles)
			battlesArr.Add(bt.ToJSON());
		json["Battles"] = battlesArr;
		return json;
	}

	public void SetExpirationTime()
	{
		_expirationTime = DateTime.UtcNow.AddHours(1);
	}

	public void MergeWith(Battle battleValue, int battleIndex)
	{
		if (battleValue != null)
		{
			_battleCounter = battleValue.BattleCounter;
			_currentBattleIndex = _battleCounter % _battles.Count;
			if (_currentBattleIndex < 0) _currentBattleIndex = 0;
		}
	}

	public void SetBattleHidden(bool value)
	{
		_hidden = value;
	}

	public void SetBattleAvailable(bool value)
	{
		_available = value;
	}

	public bool GetIsHidden()
	{
		return _hidden;
	}

	public bool GetIsAvailable()
	{
		return _available && !GetIsCompleted();
	}

	public LocationInfo GetLocation()
	{
		return _battles[_currentBattleIndex].location;
	}

	public long GetCooldown()
	{
		return _battles[_currentBattleIndex].GetCooldown();
	}

	public bool HasCooldown()
	{
		return _battles[_currentBattleIndex].HasCooldown();
	}

	public DateTime GetGenerationTime()
	{
		return _genTime;
	}

	public DateTime GetExpirationTime()
	{
		return _expirationTime;
	}

	public DateTime GetFinishTime()
	{
		return _battles[_currentBattleIndex].GetFinishTime();
	}

	public bool HasExpirationTime()
	{
		return _expirationTime != DateTime.MinValue;
	}

	public void ClearExpirationTime()
	{
		_expirationTime = DateTime.MinValue;
	}

	public void CompleteFight(FightResult resultFight)
	{
		_battles[_currentBattleIndex].CompleteFight(sf3DTO.FightResult.Win);
		if (_battles[_currentBattleIndex].GetIsCompleted())
		{
			IncBattleCounter();
		}
	}

	public bool GetIsCompleted()
	{
		return _battleCounter >= _battles.Count;
	}

	public int[] GetChapters()
	{
		return _battles[_currentBattleIndex].GetChapters();
	}

	public void SetCurrentFight(int fightIndex)
	{
		_battles[_currentBattleIndex].SetCurrentFight(fightIndex);
	}
}
