# Build Errors - 716 total

## CAT 1: CS0104 Ambiguous references (100)
### Node (58) - Nekki.Yaml.Node vs Godot.Node
### Color (42) - sf3DTO.Color vs Godot.Color
FIX: Add `using Node = Godot.Node;` / `using Color = Godot.Color;` in affected files

## CAT 2: CS0426 Missing enum in ConstantsSF3 (36)
### ELocationSceneModule (30)
FIX: Add enum to ConstantsSF3
### DecorationType (2)
FIX: Add enum
### IBadgeItem (4)
FIX: Implement interface

## CAT 3: CS0260 Missing partial (8)
SplitTriggers, TriggerActionQuestQueue, TriggerActionShowSfIcon, TriggerActionShowStatusIcon
FIX: Add partial keyword

## CAT 4: CS0101 Duplicate type (2)
MapBattleStack duplicated in scripts/ and scripts/SF3/
FIX: Remove duplicate

## CAT 5: CS0234 Missing namespace (34)
Core(2), UI(22), Utils(4), Zip(6) - Nekki.* namespaces
FIX: Port firstpass source files

## CAT 6: CS0246 Type not found (350)
### Firstpass types to port:
- NekkiWebRequest (18)
- NekkiUILabel (14)
- NekkiUISprite (2)
- NekkiLabel (2)
- MultiTweenTransition (10)
- RpnParser (6)
- RpnValue<> (24)
- EquationLine (2)
- InfoAnimationPlayer (24)
- IntervalAnimation (10)
- DevicePreset/QualityDeviceTypes/QualityPresets (18)
- BundleConfig (4)
- ConfigsSourceResolver (2)
- ScreenTexture (2)
- SpriteAlphaAnimationPlayer (2)
- SpriteSlicer2DSliceInfo (2)
- TrailRenderer2D (2)
- LocaleImport/LocalizationText (8)
- YamlDocumentNekki/YamlNode (10)
- InternalSettings (2)
- PingResponse (2)
- RewardStatus (2)
- ICardAnimation/IReelItemAnimationPlayer (4)
- SFSProtocol (2)
- TriggerEvent (2)
- OfflineRequestData (2)
- RectNode3D (2)
### Unity→Godot API replacements:
- Coroutine (6) → Task/Tween
- AnimationCurve (2) → Curve
- AnimationClip (2) → Animation
- KeyCode (6) → Key
- RenderTexture (10) → ImageTexture/Viewport
- Canvas (2) → Control
- Rigidbody/Collider/BoxCollider/SphereCollider/CapsuleCollider/CharacterJoint/Collision (28)
- SystemLanguage (4)
- TextAsset (4)
- Button/Label (12) → Godot.Button/Godot.Label
- State (2)
- Color (4)
- SkinnedMeshInstance3D (2)
### NGUI→Godot:
- UISprite/UILabel/UIButton/UIWidget/UIAnchor (52)
- TweenAlpha/TweenPosition (6)
### Missing protobuf types:
- IMessage (2), FileDescriptor (2), MessageParser<>, ByteString
FIX: Add Google.Protobuf using directives
### Ionic.Zip (4)
FIX: Port Ionic.Zip or use System.IO.Compression

## CAT 7: CS0102 Duplicate members (22)
SplitTriggers (14), NetworkConnection (2), TriggerActionQuestQueue (2), TriggerActionShowSfIcon (4)
FIX: Remove duplicate fields/methods

## CAT 8: CS0111 Already defines member (40)
SplitTriggers (22), TriggerActionQuestQueue (6), TriggerActionShowSfIcon (4), JSONArray(2), JSONObject(2), LoadScreen(2), DebugInfoHider(2)
FIX: Remove duplicate definitions

## CAT 9: CS0115 No suitable override (48)
Various methods marked override but base has no such method
FIX: Remove override keyword where base doesn't have virtual/abstract

## CAT 10: CS0507 Cannot change access modifiers (14)
BattleInterface._Process, CurrencyUI._ExitTree, DebugInfoHider._Ready, DebugMenu._Ready, GUIEffect._Ready, SpriteAnimationEffect._Ready, TutorialComponentNative._Process
FIX: Make overrides public

## CAT 11: CS0513 Abstract in non-abstract (4)
TCPNetworkState.TCPStart/TCPStop
FIX: Make class abstract or implement methods

## CAT 12: CS0508 Return type mismatch (2)
BattleItem.ToYaml()
FIX: Fix return type

## CAT 13: CS0535 Missing interface impl (54)
BattleInfo(6), GenericBattleInfo(6), Equipment(36), Model(4), UserManager(2)
FIX: Implement missing interface members

## CAT 14: CS0738 Does not implement interface (2)
Model
FIX: Implement missing members

## PHASE PLAN:
1. Fix Node/Color ambiguity (CS0104)
2. Fix missing partial (CS0260)
3. Fix duplicate types/members (CS0101, CS0102, CS0111)
4. Fix ELocationSceneModule (CS0426)
5. Port firstpass source files (CS0234, CS0246 for Nekki.*)
6. Replace Unity API with Godot (CS0246)
7. Fix overrides (CS0115, CS0507, CS0513)
8. Fix interface implementations (CS0535, CS0738, CS0508)
