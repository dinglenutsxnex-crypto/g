---
name: Build state & custom-class fixes
description: What was done to clear the CS0104/CS0246/GD0001 layer; current build state
---

## Build checkpoint (custom-class layer cleared)
- All GD0001 (missing partial) fixed via bulk sed script across ~128 files
- All GD0102/GD0101/GD0103 (invalid [Export]) fixed by removing [Export] from affected fields
- All CS0104 Node-ambiguity fixed by adding `using Node = Nekki.Yaml.Node;` to each file that imports both `using Nekki.Yaml;` and has `global using Godot;` in scope
- CS0104 Color-ambiguity in RuleAppearance.cs fixed with `using Color = sf3DTO.Color;`
- CS0104 Expression-ambiguity in ReflectionUtil.cs fixed with `using Expression = System.Linq.Expressions.Expression;`
- CS0104 Type-ambiguity in EventType.cs fixed with `using Type = System.Type;`
- CS0508 return-type mismatches fixed by ensuring Node alias is consistent across BattleItem/Equipment/Perk
- GodotPackedArrayStubs.cs created in scripts/firstpass/ for PackedVector3/2/Float32/Int32Array
- TriggerActionAnimation.cs class is `TriggerActionAnimationPlayer` (not TriggerActionAnimation)
- TriggerActionPauseAnimation.cs class is `TriggerActionPauseAnimationPlayer`
- TriggerActionShakeCamera.cs class is `TriggerActionShakeCamera3D`
- TriggerAction.Init() updated to use actual class names and correct EActionType enum values (ANIMATION, PAUSE_ANIMATION, SHAKE_CAMERA)

**Why:** `global using Godot;` is needed for ~7 utility files that use GD.* but causes ambiguity wherever files also have `using Nekki.Yaml;` (Node clash) or import Google.Protobuf/System.Linq.Expressions (Expression/Type clash).

**Current state:** 0 custom-class errors; 2031 API-usage errors remain.
