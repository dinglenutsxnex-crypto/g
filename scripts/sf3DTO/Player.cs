using System;
using System.Collections.Generic;

namespace sf3DTO
{
	public class Player : IEquatable<Player>, ICloneable
	{
		private PublicPlayer publicPlayer_;
		private int permissions_;
		private long experience_;
		private List<Currency> currencies_ = new List<Currency>();
		private Appearance appearance_;
		private Shop shop_;
		private Inventory inventory_;
		private BattleData battleData_;
		private int chapterId_;
		private long offlineStateId_;
		private string abTag_ = string.Empty;
		private LogData logData_;
		private double rating_;
		private BrawlerFight brawlerFight_;

		public PublicPlayer PublicPlayer
		{
			get { return publicPlayer_; }
			set { publicPlayer_ = value; }
		}

		public int Permissions
		{
			get { return permissions_; }
			set { permissions_ = value; }
		}

		public long Experience
		{
			get { return experience_; }
			set { experience_ = value; }
		}

		public List<Currency> Currencies
		{
			get { return currencies_; }
		}

		public Appearance Appearance
		{
			get { return appearance_; }
			set { appearance_ = value; }
		}

		public Shop Shop
		{
			get { return shop_; }
			set { shop_ = value; }
		}

		public Inventory Inventory
		{
			get { return inventory_; }
			set { inventory_ = value; }
		}

		public BattleData BattleData
		{
			get { return battleData_; }
			set { battleData_ = value; }
		}

		public int ChapterId
		{
			get { return chapterId_; }
			set { chapterId_ = value; }
		}

		public long OfflineStateId
		{
			get { return offlineStateId_; }
			set { offlineStateId_ = value; }
		}

		public string AbTag
		{
			get { return abTag_; }
			set { abTag_ = value ?? string.Empty; }
		}

		public LogData LogData
		{
			get { return logData_; }
			set { logData_ = value; }
		}

		public double Rating
		{
			get { return rating_; }
			set { rating_ = value; }
		}

		public BrawlerFight BrawlerFight
		{
			get { return brawlerFight_; }
			set { brawlerFight_ = value; }
		}

		public Player()
		{
		}

		public Player(Player other)
		{
			PublicPlayer = other.publicPlayer_?.Clone() as PublicPlayer;
			permissions_ = other.permissions_;
			experience_ = other.experience_;
			currencies_ = new List<Currency>(other.currencies_);
			Appearance = other.appearance_?.Clone() as Appearance;
			Shop = other.shop_?.Clone() as Shop;
			Inventory = other.inventory_?.Clone() as Inventory;
			BattleData = other.battleData_?.Clone() as BattleData;
			chapterId_ = other.chapterId_;
			offlineStateId_ = other.offlineStateId_;
			abTag_ = other.abTag_;
			LogData = other.logData_?.Clone() as LogData;
			rating_ = other.rating_;
			BrawlerFight = other.brawlerFight_?.Clone() as BrawlerFight;
		}

		public object Clone()
		{
			return new Player(this);
		}

		public override bool Equals(object other)
		{
			return Equals(other as Player);
		}

		public bool Equals(Player other)
		{
			if (ReferenceEquals(other, null))
				return false;
			if (ReferenceEquals(other, this))
				return true;
			if (!object.Equals(PublicPlayer, other.PublicPlayer))
				return false;
			if (Permissions != other.Permissions)
				return false;
			if (Experience != other.Experience)
				return false;
			if (!currencies_.Equals(other.currencies_))
				return false;
			if (!object.Equals(Appearance, other.Appearance))
				return false;
			if (!object.Equals(Shop, other.Shop))
				return false;
			if (!object.Equals(Inventory, other.Inventory))
				return false;
			if (!object.Equals(BattleData, other.BattleData))
				return false;
			if (ChapterId != other.ChapterId)
				return false;
			if (OfflineStateId != other.OfflineStateId)
				return false;
			if (AbTag != other.AbTag)
				return false;
			if (!object.Equals(LogData, other.LogData))
				return false;
			if (Rating != other.Rating)
				return false;
			if (!object.Equals(BrawlerFight, other.BrawlerFight))
				return false;
			return true;
		}

		public override int GetHashCode()
		{
			int num = 1;
			if (publicPlayer_ != null)
				num ^= PublicPlayer.GetHashCode();
			if (Permissions != 0)
				num ^= Permissions.GetHashCode();
			if (Experience != 0)
				num ^= Experience.GetHashCode();
			num ^= currencies_.GetHashCode();
			if (appearance_ != null)
				num ^= Appearance.GetHashCode();
			if (shop_ != null)
				num ^= Shop.GetHashCode();
			if (inventory_ != null)
				num ^= Inventory.GetHashCode();
			if (battleData_ != null)
				num ^= BattleData.GetHashCode();
			if (ChapterId != 0)
				num ^= ChapterId.GetHashCode();
			if (OfflineStateId != 0)
				num ^= OfflineStateId.GetHashCode();
			if (AbTag.Length != 0)
				num ^= AbTag.GetHashCode();
			if (logData_ != null)
				num ^= LogData.GetHashCode();
			if (Rating != 0.0)
				num ^= Rating.GetHashCode();
			if (brawlerFight_ != null)
				num ^= BrawlerFight.GetHashCode();
			return num;
		}
	}
}
