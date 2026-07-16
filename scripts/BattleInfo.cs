using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Godot;
using Nekki;
using Nekki.Yaml;
using SF3;
using SF3.Settings;
using SimpleJSON;
using sf3DTO;

public class BattleInfo : IBattleInfo, ICloneable
{
	[Flags]
	public enum CompletionType
	{
		WIN = 1,
		LOSS = 2,
		SURRENDER = 4
	}

	protected int _id;

	private sf3DTO.BattleType _battleType = sf3DTO.BattleType.Test;

	protected string _name = string.Empty;

	protected string _alias = string.Empty;

	protected string _description = string.Empty;

	protected string _icon = string.Empty;

	protected string _image = string.Empty;

	protected string _stageIcon = FightSettings.defaultStageIcon;

	protected LocationInfo _location;

	protected List<FightInfo> _fights = new List<FightInfo>();

	private bool _available = true;

	private bool _hidden;

	private long _cooldownMilliseconds;

	private DateTime _expirationTime = DateTime.MinValue;

	private DateTime _genTime = DateTime.MinValue;

	private DateTime _finishTime = DateTime.MinValue;

	private int _currentFightIndex;

	private int[] _chapters;

	private CompletionType _completion = CompletionType.WIN;

	public int id
	{
		get
		{
			return _id;
		}
	}

	public sf3DTO.BattleType battleType
	{
		get
		{
			return _battleType;
		}
	}

	public string name
	{
		get
		{
			return _name;
		}
	}

	public string alias
	{
		get
		{
			return _alias;
		}
	}

	public string description
	{
		get
		{
			return _description;
		}
	}

	public string icon
	{
		get
		{
			return _icon;
		}
	}

	public string image
	{
		get
		{
			return _image;
		}
	}

	public string stageIcon
	{
		get
		{
			return _stageIcon;
		}
	}

	public LocationInfo location
	{
		get
		{
			return _location;
		}
	}

	public List<FightInfo> fights
	{
		get
		{
			return _fights;
		}
	}

	public int wonFights
	{
		get
		{
			return _currentFightIndex;
		}
	}

	protected BattleInfo()
	{
	}

	public static BattleInfo Create(Mapping battleMap)
	{
		BattleInfo battleInfo = new BattleInfo();
		Scalar text = battleMap.GetText("ID");
		if (text != null)
		{
			battleInfo._id = int.Parse(text.text);
		}
		battleInfo.MergeWith(battleMap);
		return battleInfo;
	}

	protected void MergeWith(Mapping battleMap)
	{
		Scalar text = battleMap.GetText("Available");
		if (text != null)
		{
			_available = text.text.Equals("1");
		}
		text = battleMap.GetText("Hidden");
		if (text != null)
		{
			_hidden = text.text.Equals("1");
		}
		text = battleMap.GetText("WonFights");
		if (text != null)
		{
			SetCurrentFight(int.Parse(text.text));
		}
		text = battleMap.GetText("ExpirationTime");
		if (text != null)
		{
			_expirationTime = NekkiUtils.GetUnixDateTimeFromMilliseconds(long.Parse(text.text));
		}
		text = battleMap.GetText("GenTime");
		if (text != null && _genTime == DateTime.MinValue)
		{
			_genTime = NekkiUtils.GetUnixDateTimeFromMilliseconds(long.Parse(text.text));
		}
		Sequence sequence = battleMap.GetSequence("Fights");
		if (sequence != null)
		{
			for (int i = 0; i < sequence.nodesInside.Count; i++)
			{
				_fights[i].MergeWith(battleMap);
			}
		}
	}

	public FightInfo GetCurrentFight()
	{
		FightInfo result = null;
		if (fights.Count > 0 && !GetIsCompleted())
		{
			result = fights[_currentFightIndex];
		}
		return result;
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("BattleInfo:");
		stringBuilder.AppendFormat("ID: [{0}]; Type: [{1}]; Available: [{2}]; Hidden: [{3}];", _id, _battleType, _available, _hidden);
		stringBuilder.AppendFormat("alias {0}\ndescription {1}\nicon {2}\nimage {3}\nstageIcon {4}", alias, description, icon, image, stageIcon);
		stringBuilder.AppendLine("\n Fights:");
		foreach (FightInfo fight in fights)
		{
			stringBuilder.Append("[ ");
			stringBuilder.Append(fight.ToString());
			stringBuilder.Append(" ]\n");
		}
		return stringBuilder.ToString();
	}

	public BattleInfo GetBattleInfo()
	{
		return this;
	}

	public void SetTestBattleType()
	{
		_battleType = sf3DTO.BattleType.Test;
	}

	public int GetID()
	{
		return id;
	}

	public sf3DTO.BattleType GetBattleType()
	{
		return _battleType;
	}

	public bool GetIsCompleted()
	{
		return _currentFightIndex >= fights.Count;
	}

	public List<FightInfo> GetFights()
	{
		return fights;
	}

	public int GetWonFights()
	{
		return wonFights;
	}

	public virtual object Clone()
	{
		BattleInfo result = MemberwiseClone() as BattleInfo;
		result._fights = new List<FightInfo>();
		_fights.ForEach(delegate(FightInfo fightValue)
		{
			result._fights.Add(fightValue.Clone() as FightInfo);
		});
		return result;
	}

	public DateTime GetFinishTime()
	{
		return _finishTime;
	}

	public int GetBattleCounter()
	{
		return GetIsCompleted() ? 1 : 0;
	}

	public Mapping ToYAML()
	{
		Mapping mapping = new Mapping(_id.ToString(), new Scalar("ID", _id.ToString()));
		mapping.Add(new Scalar("Hidden", (!_hidden) ? string.Empty : "1"));
		mapping.Add(new Scalar("Available", _available ? string.Empty : "0"));
		mapping.Add(new Scalar("WonFights", _currentFightIndex.ToString()));
		if (HasExpirationTime())
		{
			mapping.Add(new Scalar("ExpirationTime", _expirationTime.GetUnixTimeStampMilliseconds().ToString()));
		}
		mapping.Add(new Scalar("GenTime", _genTime.GetUnixTimeStampMilliseconds().ToString()));
		if (_fights.Count > 0)
		{
			Mapping[] mappingInside = _fights.Select((FightInfo fightNodes) => fightNodes.ToYAML()).ToArray();
			Sequence entry = new Sequence("Fights", mappingInside);
			mapping.Add(entry);
		}
		return mapping;
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
		return _available;
	}

	public LocationInfo GetLocation()
	{
		return _location;
	}

	public long GetCooldown()
	{
		return _cooldownMilliseconds;
	}

	public bool HasCooldown()
	{
		return _cooldownMilliseconds != 0;
	}

	public DateTime GetGenerationTime()
	{
		return _genTime;
	}

	public DateTime GetExpirationTime()
	{
		return _expirationTime;
	}

	public bool HasExpirationTime()
	{
		return _expirationTime != DateTime.MinValue;
	}

	public void ClearExpirationTime()
	{
		_expirationTime = DateTime.MinValue;
	}

	public void CompleteFight(sf3DTO.FightResult resultFight)
	{
		if (!GetIsCompleted() && ((uint)_completion & (uint)resultFight) == (uint)resultFight)
		{
			_currentFightIndex++;
		}
	}

	public int[] GetChapters()
	{
		return _chapters;
	}

	public void SetCurrentFight(int fightIndex)
	{
		_currentFightIndex = fightIndex;
	}
}
