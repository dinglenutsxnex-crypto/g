/* * * * *
 * A simple JSON Parser / builder
 * ------------------------------
 * 
 * It mainly has been written as a simple JSON parser. It can build a JSON string
 * from the node-tree, or generate a node tree from any valid JSON string.
 * 
 * Written by Bunny83 
 * 2012-06-09
 * 
 * Changelog now external. See Changelog.txt
 * 
 * The MIT License (MIT)
 * 
 * Copyright (c) 2012-2022 Markus Göbel (Bunny83)
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 * 
 * * * * */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace SimpleJSON
{
    public enum JSONNodeType
    {
        Array = 1,
        Object = 2,
        String = 3,
        Number = 4,
        NullValue = 5,
        Boolean = 6,
        None = 7,
        Custom = 0xFF,
    }
    public enum JSONTextMode
    {
        Compact = 0,
        Indent
    }

    public abstract partial class JSONNode
    {
        #region Enumerators
        public struct Enumerator
        {
            private enum Type { None, Array, Object }
            private Type type;
            private Dictionary<string, JSONNode>.Enumerator m_Object;
            private List<JSONNode>.Enumerator m_Array;
            public bool IsValid { get { return type != Type.None; } }
            public Enumerator(List<JSONNode>.Enumerator aArrayEnum)
            {
                type = Type.Array;
                m_Object = default(Dictionary<string, JSONNode>.Enumerator);
                m_Array = aArrayEnum;
            }
            public Enumerator(Dictionary<string, JSONNode>.Enumerator aDictEnum)
            {
                type = Type.Object;
                m_Object = aDictEnum;
                m_Array = default(List<JSONNode>.Enumerator);
            }
            public KeyValuePair<string, JSONNode> Current
            {
                get
                {
                    if (type == Type.Array)
                        return new KeyValuePair<string, JSONNode>(string.Empty, m_Array.Current);
                    else if (type == Type.Object)
                        return m_Object.Current;
                    return new KeyValuePair<string, JSONNode>(string.Empty, null);
                }
            }
            public bool MoveNext()
            {
                if (type == Type.Array)
                    return m_Array.MoveNext();
                else if (type == Type.Object)
                    return m_Object.MoveNext();
                return false;
            }
        }
        public struct ValueEnumerator
        {
            private Enumerator m_Enumerator;
            public ValueEnumerator(List<JSONNode>.Enumerator aArrayEnum) : this(new Enumerator(aArrayEnum)) { }
            public ValueEnumerator(Dictionary<string, JSONNode>.Enumerator aDictEnum) : this(new Enumerator(aDictEnum)) { }
            public ValueEnumerator(Enumerator aEnumerator) { m_Enumerator = aEnumerator; }
            public JSONNode Current { get { return m_Enumerator.Current.Value; } }
            public bool MoveNext() { return m_Enumerator.MoveNext(); }
            public ValueEnumerator GetEnumerator() { return this; }
        }
        public struct KeyEnumerator
        {
            private Enumerator m_Enumerator;
            public KeyEnumerator(List<JSONNode>.Enumerator aArrayEnum) : this(new Enumerator(aArrayEnum)) { }
            public KeyEnumerator(Dictionary<string, JSONNode>.Enumerator aDictEnum) : this(new Enumerator(aDictEnum)) { }
            public KeyEnumerator(Enumerator aEnumerator) { m_Enumerator = aEnumerator; }
            public string Current { get { return m_Enumerator.Current.Key; } }
            public bool MoveNext() { return m_Enumerator.MoveNext(); }
            public KeyEnumerator GetEnumerator() { return this; }
        }

        public class LinqEnumerator : IEnumerator<KeyValuePair<string, JSONNode>>, IEnumerable<KeyValuePair<string, JSONNode>>
        {
            private JSONNode m_Node;
            private Enumerator m_Enumerator;
            internal LinqEnumerator(JSONNode aNode)
            {
                m_Node = aNode;
                if (m_Node != null)
                    m_Enumerator = m_Node.GetEnumerator();
            }
            public KeyValuePair<string, JSONNode> Current { get { return m_Enumerator.Current; } }
            object IEnumerator.Current { get { return m_Enumerator.Current; } }
            public bool MoveNext() { return m_Enumerator.MoveNext(); }
            public void Dispose() { m_Node = null; m_Enumerator = default(Enumerator); }
            public IEnumerator<KeyValuePair<string, JSONNode>> GetEnumerator() { return this; }
            public void Reset() { if (m_Node != null) m_Enumerator = m_Node.GetEnumerator(); }
            IEnumerator IEnumerable.GetEnumerator() { return this; }
        }
        #endregion Enumerators

        #region common interface

        public static bool forceASCII = false; // Use Unicode by default
        public static bool longAsString = false; // lazy creator creates long as string?
        public static bool allowLineComments = true; // allow "//"-style comments at the end of a line

        public abstract JSONNodeType Tag { get; }

        public virtual JSONNode this[int aIndex] { get { return null; } set { } }
        public virtual JSONNode this[string aKey] { get { return null; } set { } }
        public virtual string Value { get { return ""; } set { } }
        public virtual int Count { get { return 0; } }
        public virtual bool IsNumber { get { return false; } }
        public virtual bool IsString { get { return false; } }
        public virtual bool IsBoolean { get { return false; } }
        public virtual bool IsNull { get { return false; } }
        public virtual bool IsArray { get { return false; } }
        public virtual bool IsObject { get { return false; } }

        public virtual void Add(string aKey, JSONNode aItem) { }
        public virtual void Add(JSONNode aItem) { Add("", aItem); }

        public virtual JSONNode Remove(string aKey) { return null; }
        public virtual JSONNode Remove(int aIndex) { return null; }
        public virtual JSONNode Remove(JSONNode aNode) { return aNode; }

        public virtual JSONNode Clone() { return null; }

        public virtual IEnumerable<JSONNode> Children
        {
            get
            {
                yield break;
            }
        }

        public IEnumerable<JSONNode> DeepChildren
        {
            get
            {
                foreach (var C in Children)
                    foreach (var D in C.DeepChildren)
                        yield return D;
            }
        }

        public virtual bool HasKey(string aKey)
        {
            return false;
        }

        public virtual JSONNode GetValueOrDefault(string aKey, JSONNode aDefault)
        {
            return aDefault;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            WriteToStringBuilder(sb, 0, 0, JSONTextMode.Compact);
            return sb.ToString();
        }
        public virtual string ToString(int aIndent)
        {
            StringBuilder sb = new StringBuilder();
            WriteToStringBuilder(sb, 0, aIndent, JSONTextMode.Indent);
            return sb.ToString();
        }
        internal abstract void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode);

        public abstract Enumerator GetEnumerator();
        public IEnumerable<KeyValuePair<string, JSONNode>> Linq { get { return new LinqEnumerator(this); } }
        public KeyEnumerator Keys { get { return new KeyEnumerator(GetEnumerator()); } }
        public ValueEnumerator Values { get { return new ValueEnumerator(GetEnumerator()); } }

        #endregion common interface

        #region typecasting properties


        public virtual double AsDouble
        {
            get
            {
                double v = 0.0;
                if (double.TryParse(Value, NumberStyles.Float, CultureInfo.InvariantCulture, out v))
                    return v;
                return 0.0;
            }
            set
            {
                Value = value.ToString(CultureInfo.InvariantCulture);
            }
        }

        public virtual int AsInt
        {
            get { return (int)AsDouble; }
            set { AsDouble = value; }
        }

        public virtual float AsFloat
        {
            get { return (float)AsDouble; }
            set { AsDouble = value; }
        }

        public virtual bool AsBool
        {
            get
            {
                bool v = false;
                if (bool.TryParse(Value, out v))
                    return v;
                return !string.IsNullOrEmpty(Value);
            }
            set
            {
                Value = (value) ? "true" : "false";
            }
        }

        public virtual long AsLong
        {
            get
            {
                long val = 0;
                if (long.TryParse(Value, out val))
                    return val;
                return 0L;
            }
            set
            {
                Value = value.ToString();
            }
        }

        public virtual ulong AsULong
        {
            get
            {
                ulong val = 0;
                if (ulong.TryParse(Value, out val))
                    return val;
                return 0;
            }
            set
            {
                Value = value.ToString();
            }
        }

        public virtual JSONArray AsArray
        {
            get
            {
                return this as JSONArray;
            }
        }

        public virtual JSONObject AsObject
        {
            get
            {
                return this as JSONObject;
            }
        }


        #endregion typecasting properties

        #region operators

        public static implicit operator JSONNode(string s)
        {
            return (s == null) ? (JSONNode)JSONNull.CreateOrGet() : new JSONString(s);
        }
        public static implicit operator string(JSONNode d)
        {
            return (d == null) ? null : d.Value;
        }

        public static implicit operator JSONNode(double n)
        {
            return new JSONNumber(n);
        }
        public static implicit operator double(JSONNode d)
        {
            return (d == null) ? 0 : d.AsDouble;
        }

        public static implicit operator JSONNode(float n)
        {
            return new JSONNumber(n);
        }
        public static implicit operator float(JSONNode d)
        {
            return (d == null) ? 0 : d.AsFloat;
        }

        public static implicit operator JSONNode(int n)
        {
            return new JSONNumber(n);
        }
        public static implicit operator int(JSONNode d)
        {
            return (d == null) ? 0 : d.AsInt;
        }

        public static implicit operator JSONNode(long n)
        {
            if (longAsString)
                return new JSONString(n.ToString());
            return new JSONNumber(n);
        }
        public static implicit operator long(JSONNode d)
        {
            return (d == null) ? 0L : d.AsLong;
        }

        public static implicit operator JSONNode(ulong n)
        {
            if (longAsString)
                return new JSONString(n.ToString());
            return new JSONNumber(n);
        }
        public static implicit operator ulong(JSONNode d)
        {
            return (d == null) ? 0 : d.AsULong;
        }

        public static implicit operator JSONNode(bool b)
        {
            return new JSONBool(b);
        }
        public static implicit operator bool(JSONNode d)
        {
            return (d == null) ? false : d.AsBool;
        }

        public static implicit operator JSONNode(KeyValuePair<string, JSONNode> aKeyValue)
        {
            return aKeyValue.Value;
        }

        public static bool operator ==(JSONNode a, object b)
        {
            if (ReferenceEquals(a, b))
                return true;
            bool aIsNull = a is JSONNull || ReferenceEquals(a, null) || a is JSONLazyCreator;
            bool bIsNull = b is JSONNull || ReferenceEquals(b, null) || b is JSONLazyCreator;
            if (aIsNull && bIsNull)
                return true;
            return !aIsNull && !bIsNull && a.Equals(b);
        }

        public static bool operator !=(JSONNode a, object b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion operators

        [ThreadStatic]
        private static StringBuilder m_EscapeBuilder;
        internal static StringBuilder EscapeBuilder
        {
            get
            {
                if (m_EscapeBuilder == null)
                    m_EscapeBuilder = new StringBuilder();
                return m_EscapeBuilder;
            }
        }
        internal static string Escape(string aText)
        {
            var sb = EscapeBuilder;
            sb.Length = 0;
            if (sb.Capacity < aText.Length + aText.Length / 10)
                sb.Capacity = aText.Length + aText.Length / 10;
            foreach (char c in aText)
            {
                switch (c)
                {
                    case '\\':
                        sb.Append("\\\\");
                        break;
                    case '\"':
                        sb.Append("\\\"");
                        break;
                    case '\n':
                        sb.Append("\\n");
                        break;
                    case '\r':
                        sb.Append("\\r");
                        break;
                    case '\t':
                        sb.Append("\\t");
                        break;
                    case '\b':
                        sb.Append("\\b");
                        break;
                    case '\f':
                        sb.Append("\\f");
                        break;
                    default:
                        if (c < ' ' || (forceASCII && c > 127))
                        {
                            ushort val = c;
                            sb.Append("\\u").Append(val.ToString("X4"));
                        }
                        else
                            sb.Append(c);
                        break;
                }
            }
            string result = sb.ToString();
            sb.Length = 0;
            return result;
        }

        private static JSONNode ParseElement(string token, bool quoted)
        {
            if (quoted)
                return token;
            if (token == "null")
                return JSONNull.CreateOrGet();
            if (token == "true" || token == "false")
                return token;
            string tmp = token.Trim();
            if (tmp.Length <= 0)
                return JSONNull.CreateOrGet();
            if (tmp[0] == '.' || tmp[0] == '-' || tmp[0] == '+' || tmp[0] == '0' || tmp[0] == '1' || tmp[0] == '2' || tmp[0] == '3' || tmp[0] == '4' || tmp[0] == '5' || tmp[0] == '6' || tmp[0] == '7' || tmp[0] == '8' || tmp[0] == '9')
            {
                if (double.TryParse(tmp, NumberStyles.Float, CultureInfo.InvariantCulture, out double val) && tmp[0] != '+')
                    return val;
            }
            return tmp;
        }

        private static JSONNode Parse(string aJSON, ref int index)
        {
            bool result = false;
            return Parse(aJSON, ref index, ref result);
        }

        internal static JSONNode Parse(string aJSON, ref int index, ref bool success)
        {
            int len = aJSON.Length;
            char c;
            if (index >= len)
                return JSONNull.CreateOrGet();
            do
            {
                c = aJSON[index];
            } while (c == ' ' || c == '\r' || c == '\t' || c == '\n' && ++index < len);
            if (index >= len)
                return JSONNull.CreateOrGet();
            switch (c)
            {
                case '{':
                    return JSONObject.Parse(aJSON, ref index, ref success);
                case '[':
                    return JSONArray.Parse(aJSON, ref index, ref success);
                case '"':
                    return JSONString.Parse(aJSON, ref index, ref success);
                case 'n':
                    if (len > index + 4 && aJSON.Substring(index, 4).Equals("null"))
                    {
                        index += 4;
                        return JSONNull.CreateOrGet();
                    }
                    success = false;
                    return JSONNull.CreateOrGet();
                case 't':
                    if (len > index + 4 && aJSON.Substring(index, 4).Equals("true"))
                    {
                        index += 4;
                        return new JSONString("true");
                    }
                    success = false;
                    return JSONNull.CreateOrGet();
                case 'f':
                    if (len > index + 5 && aJSON.Substring(index, 5).Equals("false"))
                    {
                        index += 5;
                        return new JSONString("false");
                    }
                    success = false;
                    return JSONNull.CreateOrGet();
                default:
                    return JSONNumber.Parse(aJSON, ref index, ref success);
            }
        }

        public enum ParseResult
        {
            Parse_Success,
            Parse_File_Not_Found,
            Parse_Invalid_JSON_File
        }

        public static JSONNode Parse(string aJSON)
        {
            int index = 0;
            bool success = true;
            JSONNode result = Parse(aJSON, ref index, ref success);
            if (success && index < aJSON.Length)
            {
                // Check if there are only whitespace characters after the parsed content
                for (int i = index; i < aJSON.Length; i++)
                {
                    char ch = aJSON[i];
                    if (ch != ' ' && ch != '\r' && ch != '\t' && ch != '\n')
                    {
                        success = false;
                        break;
                    }
                }
            }
            if (!success)
                throw new ArgumentException("Parsing failed, unexpected JSON structure at position " + index + " of:\n" + aJSON.Substring(0, Math.Min(aJSON.Length, index + 100)));
            return result;
        }

        public static JSONNode Parse(string aJSON, ref ParseResult parseResult)
        {
            try
            {
                int index = 0;
                bool success = true;
                JSONNode result = Parse(aJSON, ref index, ref success);
                if (success && index < aJSON.Length)
                {
                    // Check if there are only whitespace characters after the parsed content
                    for (int i = index; i < aJSON.Length; i++)
                    {
                        char ch = aJSON[i];
                        if (ch != ' ' && ch != '\r' && ch != '\t' && ch != '\n')
                        {
                            success = false;
                            break;
                        }
                    }
                }
                if (!success)
                {
                    parseResult = ParseResult.Parse_Invalid_JSON_File;
                    return null;
                }
                parseResult = ParseResult.Parse_Success;
                return result;
            }
            catch
            {
                parseResult = ParseResult.Parse_Invalid_JSON_File;
                return null;
            }
        }

        public static JSONNode ParseFromBinary(byte[] data)
        {
            if (data == null || data.Length <= 0)
                return null;
            int index = 0;
            return ParseFromBinary(data, ref index);
        }

        public static JSONNode ParseFromBinary(byte[] aData, ref int aIndex)
        {
            if (aIndex >= aData.Length)
                return null;
            if (aData[aIndex] == 0)
            {
                aIndex++;
                return JSONNull.CreateOrGet();
            }
            if (aData[aIndex] == 1)
            {
                aIndex++;
                return JSONObject.ParseFromBinary(aData, ref aIndex);
            }
            if (aData[aIndex] == 2)
            {
                aIndex++;
                return JSONArray.ParseFromBinary(aData, ref aIndex);
            }
            if (aData[aIndex] == 3)
            {
                aIndex++;
                int length = BitConverter.ToInt32(aData, aIndex);
                aIndex += 4;
                string str = Encoding.UTF8.GetString(aData, aIndex, length);
                aIndex += length;
                return new JSONString(str);
            }
            if (aData[aIndex] == 4)
            {
                aIndex++;
                double d = BitConverter.ToDouble(aData, aIndex);
                aIndex += 8;
                return d;
            }
            if (aData[aIndex] == 5)
            {
                aIndex++;
                return new JSONBool(true);
            }
            if (aData[aIndex] == 6)
            {
                aIndex++;
                return new JSONBool(false);
            }
            return null;
        }

        #region Saving
        public virtual byte[] SerializeToBinary()
        {
            using (var stream = new System.IO.MemoryStream())
            {
                SerializeToBinary(stream);
                return stream.ToArray();
            }
        }

        public virtual void SerializeToBinary(System.IO.Stream aStream)
        {
        }
        #endregion Saving
    }

    internal class JSONLazyCreator : JSONNode
    {
        private JSONNode m_Node = null;
        private string m_Key = null;
        public override JSONNodeType Tag { get { return JSONNodeType.None; } }
        public JSONLazyCreator(JSONNode aNode)
        {
            m_Node = aNode;
            m_Key = null;
        }
        public JSONLazyCreator(JSONNode aNode, string aKey)
        {
            m_Node = aNode;
            m_Key = aKey;
        }
        private T Set<T>(T aVal) where T : JSONNode
        {
            if (m_Key == null)
                m_Node.Add(aVal);
            else
                m_Node.Add(m_Key, aVal);
            m_Node = null; // Be GC friendly.
            return aVal;
        }

        public override JSONNode this[int aIndex]
        {
            get { return new JSONLazyCreator(this); }
            set { Set(new JSONArray { value }); }
        }

        public override JSONNode this[string aKey]
        {
            get { return new JSONLazyCreator(this, aKey); }
            set { Set(new JSONObject { { aKey, value } }); }
        }

        public override void Add(JSONNode aItem)
        {
            Set(new JSONArray { aItem });
        }

        public override void Add(string aKey, JSONNode aItem)
        {
            Set(new JSONObject { { aKey, aItem } });
        }

        public static bool operator ==(JSONLazyCreator a, object b)
        {
            if (ReferenceEquals(a, b))
                return true;
            if (b == null)
                return true;
            return a.Equals(b);
        }

        public static bool operator !=(JSONLazyCreator a, object b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return true;
            return ReferenceEquals(this, obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return "";
        }
        public override string ToString(int aIndent)
        {
            return "";
        }
        internal override void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode)
        {
            aSB.Append("null");
        }

        public override Enumerator GetEnumerator()
        {
            return new Enumerator();
        }
    }

    public class JSONString : JSONNode
    {
        private string m_Data;

        public override JSONNodeType Tag { get { return JSONNodeType.String; } }
        public override bool IsString { get { return true; } }
        public override string Value
        {
            get { return m_Data; }
            set { m_Data = value; }
        }

        public JSONString(string aData)
        {
            m_Data = aData;
        }
        public JSONString(string aData, bool forceASCII)
        {
            m_Data = aData;
            JSONNode.forceASCII = forceASCII;
        }

        public override JSONNode Clone()
        {
            return new JSONString(m_Data);
        }

        internal override void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode)
        {
            aSB.Append('"').Append(Escape(m_Data)).Append('"');
        }

        #region Implicit conversions
        public static implicit operator string(JSONString d)
        {
            return d.Value;
        }
        #endregion Implicit conversions

        public override Enumerator GetEnumerator()
        {
            return new Enumerator();
        }

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
                return true;
            string s = obj as string;
            if (s != null)
                return m_Data == s;
            JSONString s2 = obj as JSONString;
            if (s2 != null)
                return m_Data == s2.m_Data;
            return false;
        }

        public override int GetHashCode()
        {
            return m_Data.GetHashCode();
        }

        public static JSONString Parse(string aJSON, ref int index, ref bool success)
        {
            if (aJSON[index] != '"')
            {
                success = false;
                return null;
            }
            index++;
            StringBuilder jsonString = new StringBuilder();
            while (index < aJSON.Length)
            {
                char c = aJSON[index++];
                if (c == '"')
                {
                    success = true;
                    return new JSONString(jsonString.ToString());
                }
                if (c == '\\')
                {
                    if (index >= aJSON.Length)
                    {
                        success = false;
                        return null;
                    }
                    c = aJSON[index++];
                    switch (c)
                    {
                        case '"':
                            jsonString.Append('"');
                            break;
                        case '\\':
                            jsonString.Append('\\');
                            break;
                        case '/':
                            jsonString.Append('/');
                            break;
                        case 'b':
                            jsonString.Append('\b');
                            break;
                        case 'f':
                            jsonString.Append('\f');
                            break;
                        case 'n':
                            jsonString.Append('\n');
                            break;
                        case 'r':
                            jsonString.Append('\r');
                            break;
                        case 't':
                            jsonString.Append('\t');
                            break;
                        case 'u':
                            {
                                if (index + 3 >= aJSON.Length)
                                {
                                    success = false;
                                    return null;
                                }
                                string unicode = aJSON.Substring(index, 4);
                                if (UInt32.TryParse(unicode, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out uint unicodeVal))
                                {
                                    jsonString.Append((char)unicodeVal);
                                }
                                else
                                {
                                    success = false;
                                    return null;
                                }
                                index += 4;
                                break;
                            }
                        default:
                            jsonString.Append(c);
                            break;
                    }
                }
                else
                {
                    jsonString.Append(c);
                }
            }
            success = false;
            return null;
        }
    }

    public class JSONNumber : JSONNode
    {
        private double m_Data;

        public override JSONNodeType Tag { get { return JSONNodeType.Number; } }
        public override bool IsNumber { get { return true; } }
        public override string Value
        {
            get { return m_Data.ToString(CultureInfo.InvariantCulture); }
            set
            {
                if (double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out double v))
                    m_Data = v;
            }
        }
        public override double AsDouble { get { return m_Data; } set { m_Data = value; } }
        public override long AsLong { get { return (long)m_Data; } set { m_Data = value; } }
        public override ulong AsULong { get { return (ulong)m_Data; } set { m_Data = value; } }

        public JSONNumber(double aData)
        {
            m_Data = aData;
        }
        public JSONNumber(string aData)
        {
            Value = aData;
        }

        public override JSONNode Clone()
        {
            return new JSONNumber(m_Data);
        }

        internal override void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode)
        {
            aSB.Append(Value);
        }

        public override Enumerator GetEnumerator()
        {
            return new Enumerator();
        }

        public static JSONNumber Parse(string aJSON, ref int index, ref bool success)
        {
            int start = index;
            bool hasDot = false;
            bool hasExp = false;
            bool hasNeg = false;
            char c;
            while (index < aJSON.Length)
            {
                c = aJSON[index];
                if (c >= '0' && c <= '9')
                    index++;
                else if (c == '-' && !hasNeg)
                {
                    hasNeg = true;
                    index++;
                }
                else if (c == '.' && !hasDot)
                {
                    hasDot = true;
                    index++;
                }
                else if ((c == 'e' || c == 'E') && !hasExp)
                {
                    hasExp = true;
                    index++;
                    // Check for optional sign after 'e'/'E'
                    if (index < aJSON.Length && (aJSON[index] == '+' || aJSON[index] == '-'))
                        index++;
                }
                else
                    break;
            }
            if (index > start)
            {
                string tmp = aJSON.Substring(start, index - start);
                success = true;
                if (double.TryParse(tmp, NumberStyles.Float, CultureInfo.InvariantCulture, out double val))
                {
                    if (!longAsString || tmp.IndexOf('.') < 0 || tmp.IndexOf('e') >= 0 || tmp.IndexOf('E') >= 0)
                        return new JSONNumber(val);
                    return new JSONString(tmp);
                }
                success = false;
                return null;
            }
            success = false;
            return null;
        }

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
                return true;
            JSONNumber s2 = obj as JSONNumber;
            if (s2 != null)
                return m_Data == s2.m_Data;
            return false;
        }

        public override int GetHashCode()
        {
            return m_Data.GetHashCode();
        }
    }

    public class JSONBool : JSONNode
    {
        private bool m_Data;

        public override JSONNodeType Tag { get { return JSONNodeType.Boolean; } }
        public override bool IsBoolean { get { return true; } }
        public override string Value
        {
            get { return m_Data.ToString(); }
            set
            {
                if (bool.TryParse(value, out bool v))
                    m_Data = v;
            }
        }
        public override bool AsBool { get { return m_Data; } set { m_Data = value; } }

        public JSONBool(bool aData)
        {
            m_Data = aData;
        }
        public JSONBool(string aData)
        {
            Value = aData;
        }

        public override JSONNode Clone()
        {
            return new JSONBool(m_Data);
        }

        internal override void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode)
        {
            aSB.Append((m_Data) ? "true" : "false");
        }

        public override Enumerator GetEnumerator()
        {
            return new Enumerator();
        }

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
                return true;
            bool s = (bool)obj;
            return m_Data == s;
        }

        public override int GetHashCode()
        {
            return m_Data.GetHashCode();
        }
    }

    public class JSONNull : JSONNode
    {
        static JSONNull m_StaticInstance = new JSONNull();
        public static bool reuseSameInstance = true;
        public static JSONNull CreateOrGet()
        {
            if (reuseSameInstance)
                return m_StaticInstance;
            return new JSONNull();
        }
        private JSONNull() { }

        public override JSONNodeType Tag { get { return JSONNodeType.NullValue; } }
        public override bool IsNull { get { return true; } }
        public override string Value
        {
            get { return "null"; }
            set { }
        }
        public override bool AsBool { get { return false; } set { } }

        public override JSONNode Clone()
        {
            return CreateOrGet();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;
            return (obj is JSONNull);
        }
        public override int GetHashCode()
        {
            return 0;
        }

        internal override void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode)
        {
            aSB.Append("null");
        }

        public override Enumerator GetEnumerator()
        {
            return new Enumerator();
        }
    }

    public class JSONArray : JSONNode, IEnumerable
    {
        private List<JSONNode> m_List = new List<JSONNode>();
        public override JSONNodeType Tag { get { return JSONNodeType.Array; } }
        public override bool IsArray { get { return true; } }
        public override JSONNode this[int aIndex]
        {
            get
            {
                if (aIndex < 0 || aIndex >= m_List.Count)
                    return new JSONLazyCreator(this);
                return m_List[aIndex];
            }
            set
            {
                if (value == null)
                    value = JSONNull.CreateOrGet();
                if (aIndex < 0 || aIndex >= m_List.Count)
                    m_List.Add(value);
                else
                    m_List[aIndex] = value;
            }
        }

        public override JSONNode this[string aKey]
        {
            get { return new JSONLazyCreator(this); }
            set
            {
                if (value == null)
                    value = JSONNull.CreateOrGet();
                m_List.Add(value);
            }
        }

        public override int Count
        {
            get { return m_List.Count; }
        }

        public override void Add(string aKey, JSONNode aItem)
        {
            if (aItem == null)
                aItem = JSONNull.CreateOrGet();
            m_List.Add(aItem);
        }

        public override JSONNode Remove(int aIndex)
        {
            if (aIndex < 0 || aIndex >= m_List.Count)
                return null;
            JSONNode tmp = m_List[aIndex];
            m_List.RemoveAt(aIndex);
            return tmp;
        }

        public override JSONNode Remove(JSONNode aNode)
        {
            m_List.Remove(aNode);
            return aNode;
        }

        public override JSONNode Remove(string aKey)
        {
            return Remove(0);
        }

        public override JSONNode Clone()
        {
            var node = new JSONArray();
            foreach (var value in m_List)
                node.Add(value.Clone());
            return node;
        }

        public override IEnumerable<JSONNode> Children
        {
            get
            {
                foreach (JSONNode N in m_List)
                    yield return N;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_List.GetEnumerator();
        }

        public override Enumerator GetEnumerator()
        {
            return new Enumerator(m_List.GetEnumerator());
        }

        internal override void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode)
        {
            aSB.Append('[');
            int count = m_List.Count;
            if (aMode == JSONTextMode.Indent)
                aSB.AppendLine();
            for (int i = 0; i < count; i++)
            {
                if (aMode == JSONTextMode.Indent)
                    aSB.Append(' ', aIndent + aIndentInc);
                m_List[i].WriteToStringBuilder(aSB, aIndent + aIndentInc, aIndentInc, aMode);
                if (i < count - 1)
                {
                    aSB.Append(',');
                    if (aMode == JSONTextMode.Indent)
                        aSB.AppendLine();
                }
                if (i == count - 1 && aMode == JSONTextMode.Indent)
                    aSB.AppendLine();
            }
            if (aMode == JSONTextMode.Indent)
                aSB.Append(' ', aIndent);
            aSB.Append(']');
        }

        public static JSONArray Parse(string aJSON, ref int index, ref bool success)
        {
            JSONArray arr = new JSONArray();
            int len = aJSON.Length;
            if (aJSON[index] != '[')
            {
                success = false;
                return null;
            }
            index++;
            bool next = true;
            while (next && index < len)
            {
                char c;
                do
                {
                    c = aJSON[index];
                } while (c == ' ' || c == '\r' || c == '\t' || c == '\n' && ++index < len);
                if (index >= len)
                {
                    success = false;
                    return null;
                }
                if (c == ']')
                {
                    next = false;
                    index++;
                    break;
                }
                if (c == ',')
                {
                    index++;
                    continue;
                }
                JSONNode child = Parse(aJSON, ref index, ref success);
                if (!success)
                    return null;
                arr.Add(child);
                do
                {
                    c = aJSON[index];
                } while (c == ' ' || c == '\r' || c == '\t' || c == '\n' && ++index < len);
                if (c == ']')
                {
                    next = false;
                    index++;
                    break;
                }
                if (c != ',')
                {
                    success = false;
                    return null;
                }
                index++;
            }
            success = true;
            return arr;
        }

        public override bool HasKey(string aKey)
        {
            return false;
        }

        public override JSONNode GetValueOrDefault(string aKey, JSONNode aDefault)
        {
            return aDefault;
        }

        public override byte[] SerializeToBinary()
        {
            using (var stream = new System.IO.MemoryStream())
            {
                SerializeToBinary(stream);
                return stream.ToArray();
            }
        }

        public override void SerializeToBinary(System.IO.Stream aStream)
        {
            aStream.WriteByte(2);
            aStream.Write(BitConverter.GetBytes(m_List.Count), 0, 4);
            for (int i = 0; i < m_List.Count; i++)
            {
                m_List[i].SerializeToBinary(aStream);
            }
        }

        public static JSONArray ParseFromBinary(byte[] aData, ref int aIndex)
        {
            JSONArray arr = new JSONArray();
            int count = BitConverter.ToInt32(aData, aIndex);
            aIndex += 4;
            for (int i = 0; i < count; i++)
            {
                arr.Add(ParseFromBinary(aData, ref aIndex));
            }
            return arr;
        }
    }

    public class JSONObject : JSONNode, IEnumerable
    {
        private Dictionary<string, JSONNode> m_Dict = new Dictionary<string, JSONNode>();

        public override JSONNodeType Tag { get { return JSONNodeType.Object; } }
        public override bool IsObject { get { return true; } }

        public override JSONNode this[string aKey]
        {
            get
            {
                if (m_Dict.ContainsKey(aKey))
                    return m_Dict[aKey];
                else
                    return new JSONLazyCreator(this, aKey);
            }
            set
            {
                if (value == null)
                    value = JSONNull.CreateOrGet();
                if (m_Dict.ContainsKey(aKey))
                    m_Dict[aKey] = value;
                else
                    m_Dict.Add(aKey, value);
            }
        }

        public override JSONNode this[int aIndex]
        {
            get
            {
                if (aIndex < 0 || aIndex >= m_Dict.Count)
                    return null;
                return m_Dict.ElementAt(aIndex).Value;
            }
            set
            {
                if (value == null)
                    value = JSONNull.CreateOrGet();
                if (aIndex < 0 || aIndex >= m_Dict.Count)
                    return;
                string key = m_Dict.ElementAt(aIndex).Key;
                m_Dict[key] = value;
            }
        }

        public override int Count
        {
            get { return m_Dict.Count; }
        }

        public override void Add(string aKey, JSONNode aItem)
        {
            if (aItem == null)
                aItem = JSONNull.CreateOrGet();
            if (aKey == null)
                aKey = string.Empty;
            if (m_Dict.ContainsKey(aKey))
                m_Dict[aKey] = aItem;
            else
                m_Dict.Add(aKey, aItem);
        }

        public override JSONNode Remove(string aKey)
        {
            if (!m_Dict.ContainsKey(aKey))
                return null;
            JSONNode tmp = m_Dict[aKey];
            m_Dict.Remove(aKey);
            return tmp;
        }

        public override JSONNode Remove(int aIndex)
        {
            if (aIndex < 0 || aIndex >= m_Dict.Count)
                return null;
            var item = m_Dict.ElementAt(aIndex);
            m_Dict.Remove(item.Key);
            return item.Value;
        }

        public override JSONNode Remove(JSONNode aNode)
        {
            try
            {
                var item = m_Dict.Where(k => k.Value == aNode).First();
                m_Dict.Remove(item.Key);
                return aNode;
            }
            catch
            {
                return null;
            }
        }

        public override JSONNode Clone()
        {
            var node = new JSONObject();
            foreach (var n in m_Dict)
                node.Add(n.Key, n.Value.Clone());
            return node;
        }

        public override bool HasKey(string aKey)
        {
            return m_Dict.ContainsKey(aKey);
        }

        public override JSONNode GetValueOrDefault(string aKey, JSONNode aDefault)
        {
            if (m_Dict.TryGetValue(aKey, out JSONNode val))
                return val;
            return aDefault;
        }

        public override IEnumerable<JSONNode> Children
        {
            get
            {
                foreach (KeyValuePair<string, JSONNode> N in m_Dict)
                    yield return N.Value;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_Dict.GetEnumerator();
        }

        public override Enumerator GetEnumerator()
        {
            return new Enumerator(m_Dict.GetEnumerator());
        }

        internal override void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode)
        {
            aSB.Append('{');
            if (aMode == JSONTextMode.Indent)
                aSB.AppendLine();
            bool first = true;
            foreach (var k in m_Dict)
            {
                if (!first)
                {
                    aSB.Append(',');
                    if (aMode == JSONTextMode.Indent)
                        aSB.AppendLine();
                }
                first = false;
                if (aMode == JSONTextMode.Indent)
                    aSB.Append(' ', aIndent + aIndentInc);
                aSB.Append('"').Append(Escape(k.Key)).Append('"');
                if (aMode == JSONTextMode.Compact)
                    aSB.Append(':');
                else
                    aSB.Append(": ");
                k.Value.WriteToStringBuilder(aSB, aIndent + aIndentInc, aIndentInc, aMode);
            }
            if (aMode == JSONTextMode.Indent)
            {
                aSB.AppendLine();
                aSB.Append(' ', aIndent);
            }
            aSB.Append('}');
        }

        public static JSONObject Parse(string aJSON, ref int index, ref bool success)
        {
            JSONObject obj = new JSONObject();
            int len = aJSON.Length;
            if (aJSON[index] != '{')
            {
                success = false;
                return null;
            }
            index++;
            bool next = true;
            while (next && index < len)
            {
                char c;
                do
                {
                    c = aJSON[index];
                } while (c == ' ' || c == '\r' || c == '\t' || c == '\n' && ++index < len);
                if (index >= len)
                {
                    success = false;
                    return null;
                }
                if (c == '}')
                {
                    next = false;
                    index++;
                    break;
                }
                if (c == ',')
                {
                    index++;
                    continue;
                }
                string key = Parse(aJSON, ref index, ref success).Value;
                if (!success)
                    return null;
                do
                {
                    c = aJSON[index];
                } while (c == ' ' || c == '\r' || c == '\t' || c == '\n' && ++index < len);
                if (c != ':')
                {
                    success = false;
                    return null;
                }
                index++;
                JSONNode val = Parse(aJSON, ref index, ref success);
                if (!success)
                    return null;
                obj.Add(key, val);
                do
                {
                    c = aJSON[index];
                } while (c == ' ' || c == '\r' || c == '\t' || c == '\n' && ++index < len);
                if (c == '}')
                {
                    next = false;
                    index++;
                    break;
                }
                if (c != ',')
                {
                    success = false;
                    return null;
                }
                index++;
            }
            success = true;
            return obj;
        }

        public override byte[] SerializeToBinary()
        {
            using (var stream = new System.IO.MemoryStream())
            {
                SerializeToBinary(stream);
                return stream.ToArray();
            }
        }

        public override void SerializeToBinary(System.IO.Stream aStream)
        {
            aStream.WriteByte(1);
            aStream.Write(BitConverter.GetBytes(m_Dict.Count), 0, 4);
            foreach (var k in m_Dict)
            {
                var bytes = Encoding.UTF8.GetBytes(k.Key);
                aStream.Write(BitConverter.GetBytes(bytes.Length), 0, 4);
                aStream.Write(bytes, 0, bytes.Length);
                k.Value.SerializeToBinary(aStream);
            }
        }

        public static JSONObject ParseFromBinary(byte[] aData, ref int aIndex)
        {
            JSONObject obj = new JSONObject();
            int count = BitConverter.ToInt32(aData, aIndex);
            aIndex += 4;
            for (int i = 0; i < count; i++)
            {
                int len = BitConverter.ToInt32(aData, aIndex);
                aIndex += 4;
                string key = Encoding.UTF8.GetString(aData, aIndex, len);
                aIndex += len;
                obj.Add(key, ParseFromBinary(aData, ref aIndex));
            }
            return obj;
        }
    }
    // End of JSONObject

    public static class JSON
    {
        public static JSONNode Parse(string aJSON)
        {
            return JSONNode.Parse(aJSON);
        }

        public static JSONNode ParseFromBinary(byte[] aData)
        {
            return JSONNode.ParseFromBinary(aData);
        }
    }

    public class JSONClass : JSONObject
    {
        public JSONClass() { }
    }
}
