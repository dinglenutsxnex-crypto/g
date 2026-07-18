using System;
using System.Collections.Generic;
using Godot;

/// <summary>
/// Stub debug console. Registers commands and can execute them by name.
/// Commands may return a string result (Func) or be fire-and-forget (Action).
/// </summary>
public static class NekkiConsole
{
	// Handlers that return a string result
	private static readonly Dictionary<string, Func<string[], string>> _handlers =
		new Dictionary<string, Func<string[], string>>(StringComparer.OrdinalIgnoreCase);

	// Descriptions map: name → description
	private static readonly Dictionary<string, string> _descriptions =
		new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

	/// <summary>All registered command descriptions (name → description).</summary>
	public static IReadOnlyDictionary<string, string> Descriptions => _descriptions;

	/// <summary>Register a command whose handler returns a string result.</summary>
	public static void RegisterCommand(string name, Func<string[], string> handler, string description = "")
	{
		_handlers[name] = handler;
		_descriptions[name] = description;
	}

	/// <summary>Register a command whose handler returns void.</summary>
	public static void RegisterCommand(string name, Action<string[]> handler, string description = "")
	{
		_handlers[name] = args => { handler(args); return string.Empty; };
		_descriptions[name] = description;
	}

	/// <summary>Execute a previously registered command by name.</summary>
	public static string ExecuteCommand(string name, params string[] args)
	{
		if (_handlers.TryGetValue(name, out var handler))
			return handler(args) ?? string.Empty;
		GD.PrintErr($"[NekkiConsole] Unknown command: {name}");
		return string.Empty;
	}

	/// <summary>Log a line to the console output.</summary>
	public static void Log(string message) => GD.Print($"[Console] {message}");
}
