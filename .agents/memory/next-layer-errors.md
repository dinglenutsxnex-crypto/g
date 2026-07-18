---
name: Next layer error catalogue
description: Breakdown of 2031 API-usage errors after custom-class layer was cleared
---

## Error count (2031 total)
CS1061: 2094 | CS0117: 844 | CS0103: 326 | CS1503: 140 | CS1501: 110
CS0029: 106 | CS7036: 94 | CS0266: 76 | CS0120: 68 | CS0023: 52

## Top CS1061 (missing instance members)
- Vector3.x/y/z (218 total) -- Unity lowercase; Godot uses X/Y/Z
- Node.Visible (96) -- only on CanvasItem/Node3D, not base Node
- Action.InvokeSafe (62) -- missing extension method
- Node.SetActive (40) -- Unity; Godot: node.Visible = bool
- JsValue.AsDictionary/AsInteger/AsArray/AsNumber (92 total) -- Jint 4.x API changed
- Vector2.x/y (44 total) -- same case issue
- Button.onClick (26) -- Unity NGUI; Godot uses Pressed signal
- Material.SetShaderParameter (28) -- Godot 4: SetShaderParam or Set()
- Label.text (28) -- Godot 4: Text (capital T)
- Node.id (32) -- Unity; Godot: GetInstanceId()

## Top CS0117 (missing statics)
- NetworkEventManager.Add (60) -- stub incomplete
- Engine.GetProcessTime/GetProcessDeltaTime (60 total) -- Godot4: Time.GetTicksMsec()
- LocationColorAnimation.Instance (38) -- singleton not stubbed
- EnumsCompliancer.GetEnumerators/GetEnumeratorName (32) -- stub incomplete
- EffectsManager.PlayEffect/DisposeEffectsByModel/StopAll (42) -- stubs missing
- UserManager.* (many) -- many methods missing from stub

## Top CS0103 (names not in scope)
- Visible (92) -- bare usage in non-Node context; needs this.Visible or base class fix
- Position/GlobalPosition (30) -- same
- Update (10) -- Unity lifecycle; Godot: _Process
- Exit (8) -- GetTree().Quit()
- Vector2D (4) -- wrong name; should be Vector2

## Fix strategy for next pass
1. Bulk sed: Vector3.x/y/z -> .X/.Y/.Z, Vector2.x/y -> .X/.Y
2. Extension method Node.SetActive() + Node.Visible shim
3. Action.InvokeSafe() extension
4. Jint 4.x compatibility layer for JsValue.AsDictionary/AsInteger/AsArray
5. Button.onClick EventDelegate adapter
6. Label.text -> Text rename (bulk)
7. Extend missing stubs: EnumsCompliancer, EffectsManager, UserManager, etc.
8. Bare Visible/Position/Update -> this. prefix or _Process fix
9. Engine.GetProcessTime -> Time.GetTicksMsec() / 1000f
