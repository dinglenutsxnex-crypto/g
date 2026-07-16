using Godot;
using System;
using System.Collections.Generic;
using SF3.Effects;

namespace SF3.GameModels
{
	public partial class ModelSkin : Node
	{
		[Export]
		private string _rootBoneName;

		[Export]
		private string[] _bindBonesName;

		private bool? _mirroringDuplicate;

		private bool isShowCashed;

		private bool isMirroredCashed;

		private bool forceHide;

		public Material shadowFormMaterial;

		private Material dissolveMaterial;

		public Color enemyMaskColor = Colors.White;

		public Color playerMaskColor = Colors.White;

		public Material simpleMaterial;

		public Material matcapMaterial;

		private bool shadowFormMatInited;

		public GradientInfo gradientInfo;

		public bool droppable;

		private bool isTransparent;

		public bool isGradientSource;

		private bool updateGlowMeshConstantly;

		public bool updateGlowBoundsFromBone;

		public Vector3 glowBoudsMeshExpansion = Vector3.One;

		private int defaultMaterialDepth;

		private bool shaderOverrideEnabled;

		private float shaderOverrideTransitionTime;

		public Node3D glowBoundsMesh;

		private Godot.ImageTexture mainTexturePreRender;

		private Godot.ImageTexture shadowFormTexturePreRender;

		private bool isTrail;

		private Dictionary<string, Material> replacementMaterials;

		private float currentTransitionTime;

		public bool? mirroringDuplicate
		{
			get
			{
				return _mirroringDuplicate;
			}
		}

		public Material defaultMaterial { get; private set; }

		// STUB: SkinnedMeshRenderer -> SkinnedMeshInstance3D
		public bool IsNeedsMirrorSwitch(bool isMirrored)
		{
			return false;
		}

		public void SetIsMirroringDupcicate(bool? isMirroringDuplicate)
		{
			_mirroringDuplicate = isMirroringDuplicate;
			forceHide = false;
		}

		public void Init()
		{
			GD.Print("STUB: ModelSkin.Init - needs SkinnedMeshInstance3D binding");
		}

		public void BoundToBones(IBonesHolder model)
		{
			GD.Print("STUB: ModelSkin.BoundToBones(IBonesHolder) - needs SkinnedMeshInstance3D binding");
		}

		public void BoundToBones(ModelObject modelobject)
		{
			GD.Print("STUB: ModelSkin.BoundToBones(ModelObject) - needs SkinnedMeshInstance3D binding");
		}

		public void ShowSkin(bool _isShow, bool isMirrored, bool cashing = true)
		{
			Visible = _isShow && !forceHide;
		}

		public override void _ExitTree()
		{
			if (ModelsManager.Instance != null && ModelsManager.Instance.BlurBounds != null)
			{
				ModelsManager.Instance.BlurBounds.Remove(glowBoundsMesh);
			}
		}

		public void SetShadowFormMaterial()
		{
			GD.Print("STUB: ModelSkin.SetShadowFormMaterial");
		}

		public void UpdateShadowFormBlend(float blendProgress)
		{
			GD.Print("STUB: ModelSkin.UpdateShadowFormBlend");
		}

		public void DisableDissolve()
		{
			GD.Print("STUB: ModelSkin.DisableDissolve");
		}

		public void ReturnDefaultMaterial()
		{
			GD.Print("STUB: ModelSkin.ReturnDefaultMaterial");
		}

		public void GetMatcapTexture()
		{
			GD.Print("STUB: ModelSkin.GetMatcapTexture");
		}

		public void ApplyMaskColor()
		{
			GD.Print("STUB: ModelSkin.ApplyMaskColor");
		}

		public void SetMainColor(Color color, bool useAlpha = true)
		{
			GD.Print("STUB: ModelSkin.SetMainColor");
		}

		public void SetOpaque()
		{
			GD.Print("STUB: ModelSkin.SetOpaque");
		}

		public void SetDissolveWeaponMaterial()
		{
			GD.Print("STUB: ModelSkin.SetDissolveWeaponMaterial");
		}

		public void DissolveWeaponMeshrendererOff()
		{
			GD.Print("STUB: ModelSkin.DissolveWeaponMeshrendererOff");
		}

		public void DissolveWeaponMeshrendererOn()
		{
			GD.Print("STUB: ModelSkin.DissolveWeaponMeshrendererOn");
		}

		public void DissolveWeaponMaterialOff()
		{
			GD.Print("STUB: ModelSkin.DissolveWeaponMaterialOff");
		}

		public void SetTransparent(float value)
		{
			GD.Print("STUB: ModelSkin.SetTransparent");
		}

		public void SetSkinColor(Color color)
		{
			GD.Print("STUB: ModelSkin.SetSkinColor");
		}

		public void SwitchMaterial(bool toSimple)
		{
			GD.Print("STUB: ModelSkin.SwitchMaterial");
		}

		public void EnableGradient()
		{
			GD.Print("STUB: ModelSkin.EnableGradient");
		}

		public void DisableGradient()
		{
			GD.Print("STUB: ModelSkin.DisableGradient");
		}

		public override void _Process(double delta)
		{
			// STUB: Update glow bounds logic
		}

		public void OverrideMaterial(string materialName, bool value, float transitionTime)
		{
			GD.Print("STUB: ModelSkin.OverrideMaterial");
		}
	}
}
