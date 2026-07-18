using System.Text;
using SimpleJSON;

/// <summary>
/// SimpleJSON compatibility shim. In the old SimpleJSON API, JSONData was a
/// leaf-value node (string/number/bool). Maps to JSONString internally.
/// </summary>
public class JSONData : JSONNode
{
	private string _value;

	public JSONData(string value)  { _value = value ?? string.Empty; }
	public JSONData(int value)     { _value = value.ToString(); }
	public JSONData(long value)    { _value = value.ToString(); }
	public JSONData(float value)   { _value = value.ToString(); }
	public JSONData(double value)  { _value = value.ToString(); }
	public JSONData(bool value)    { _value = value ? "true" : "false"; }

	public override JSONNodeType Tag => JSONNodeType.String;

	public override string Value
	{
		get => _value;
		set => _value = value;
	}

	public override Enumerator GetEnumerator() => new Enumerator();

	internal override void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode)
	{
		aSB.Append('"');
		aSB.Append(Escape(_value));
		aSB.Append('"');
	}

	public override JSONNode Clone() => new JSONData(_value);
}
