using System.Collections.Generic;
using Godot;

namespace SF3
{
	public partial class SlideMenu : UIModuleHolder
	{
		private Dictionary<string, ConstantsSF3.ELocationSceneModule> _menuTypeVsSceneModule = new Dictionary<string, ConstantsSF3.ELocationSceneModule>
		{
			{ "map", ConstantsSF3.ELocationSceneModule.Map },
			{ "shop", ConstantsSF3.ELocationSceneModule.Shop },
			{ "dojo", ConstantsSF3.ELocationSceneModule.DojoInterface },
			{ "inventory", ConstantsSF3.ELocationSceneModule.Inventory },
			{ "boosterpacks", ConstantsSF3.ELocationSceneModule.BoosterpacksScreen }
		};

		private static SlideMenu _instance;

		[Export]
		private Node _subMenuObject;

		[Export]
		private Node _backplane;

		private bool _isOpen;

		public static bool isEnabled
		{
			get
			{
				if (_instance != null)
				{
					return _instance.Enabled;
				}
				return false;
			}
			set
			{
				if (_instance != null)
				{
					_instance.Enabled = value;
				}
			}
		}

		public override void _Ready()
		{
			base._Ready();
			_instance = this;
			_isOpen = false;
		}

		public void OpenMenu(string menu)
		{
			if (_menuTypeVsSceneModule.ContainsKey(menu))
			{
				ConstantsSF3.ELocationSceneModule eLocationSceneModule = _menuTypeVsSceneModule[menu];
				if (eLocationSceneModule != BaseModuleController.CurrentType)
				{
					BaseModuleController.GoToModule(eLocationSceneModule);
				}
			}
			else
			{
				GD.PrintErr(string.Format("Wrong menu name '{0}'", menu));
			}
			CloseWithoutCooldown();
		}

		public void Open()
		{
			Visible = true;
		}

		public void Close()
		{
			Visible = false;
		}

		public void CloseWithoutCooldown()
		{
			Close();
		}
	}
}
