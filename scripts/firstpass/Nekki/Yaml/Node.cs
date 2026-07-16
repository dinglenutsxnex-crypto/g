using System;
using System.Collections;

namespace Nekki.Yaml
{
	[Serializable]
	public abstract class Node : IEnumerable
	{
		public string key { get; protected set; }
		public object value { get; protected set; }
		public string typeNode { get; protected set; }

		public override string ToString()
		{
			return value != null ? value.ToString() : "";
		}

		public string GetTypeNode() { return typeNode; }
		public string GetKey() { return key; }

		public virtual IEnumerator GetEnumerator()
		{
			yield return this;
		}
	}
}
