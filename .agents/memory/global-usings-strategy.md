---
name: GlobalUsings strategy
description: Why global using Godot causes ambiguity and the fix pattern
---

## Rule
`global using Godot;` is in `scripts/GlobalUsings.cs` (alongside `global using YamlNode = Nekki.Yaml.Node;`).
It causes CS0104 ambiguity in any file that also imports a namespace containing a same-named type:

| Conflicting import | Conflicting type | Fix applied |
|--------------------|-----------------|-------------|
| `using Nekki.Yaml;` | `Node` | Add `using Node = Nekki.Yaml.Node;` to file |
| `using sf3DTO;` | `Color` | Add `using Color = sf3DTO.Color;` to file |
| `using System.Linq.Expressions;` | `Expression` | Add `using Expression = System.Linq.Expressions.Expression;` to file |
| `using Google.Protobuf.WellKnownTypes;` | `Type` | Add `using Type = System.Type;` to file |

**Why:** The Godot SDK does NOT auto-provide `global using Godot;`. We added it explicitly so ~7 plain C# utility classes (NekkiUtils, StaticObjectsManager, etc.) that call `GD.Print` don't need individual using directives. Removing it causes ~2000 extra errors in previously-working files.

**How to apply:** When a new CS0104 appears, check what two namespaces conflict and add the local alias for the one that should NOT win globally.
