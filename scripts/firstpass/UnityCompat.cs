// Stubs for miscellaneous Unity types used across the project
using Godot;
using System;
using System.Collections;
using System.Collections.Generic;

// ---- Unity Coroutine helpers ----
/// <summary>Yield instruction: wait for end of current frame.</summary>
public sealed class WaitForEndOfFrame { }

// ---- Unity Physics ----
/// <summary>Generic Collider stub (Unity physics) — plain C# to avoid GD0001.</summary>
public class Collider
{
    public bool enabled { get; set; } = true;
}

// ---- Unity Renderer ----
/// <summary>SkinnedMeshRenderer stub (Unity skinned mesh) — plain C# to avoid GD0001.</summary>
public class SkinnedMeshRenderer
{
    public bool enabled { get; set; } = true;
    public Godot.Mesh sharedMesh { get; set; }
}

// ---- Godot alias ----
/// <summary>Alias: Godot 4 uses MeshInstance3D; 'SkinnedMeshInstance3D' is a Unity→Godot name.</summary>
public partial class SkinnedMeshInstance3D : MeshInstance3D { }

// ---- Extension helpers ----
public static class VectorExtensions
{
    public static Vector3 ToVector3(this Vector2 v, float z = 0f) => new Vector3(v.X, v.Y, z);
    public static Vector2 ToVector2(this Vector3 v) => new Vector2(v.X, v.Y);
}

// ---- Loading/UI stubs ----
public partial class LoadingIcon : Control
{
    public void Show(bool visible = true) { Visible = visible; }
    public void Hide() { Visible = false; }
}

// TriggerActionHighlightUIButton — defined in SF3/Moves/ once TriggerAction compiles
