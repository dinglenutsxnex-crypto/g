using Godot;
using System;
using System.Collections.Generic;
using Nekki.Yaml;

namespace SF3.Moves
{
	public class IntervalExcludeRepulsion : IntervalAnimation
	{
		public List<string> bonesToExclude;
		public List<string> bonesToExcludeMirrored;

		public IntervalExcludeRepulsion()
		{
			bonesToExclude = new List<string>();
			bonesToExcludeMirrored = new List<string>();
		}

		public override List<IntervalAnimation> Parse(Mapping intervalNode)
		{
			base.Parse(intervalNode);
			Sequence sequence = intervalNode.GetSequence("Bones");
			if (sequence != null)
			{
				List<string> list = new List<string>();
				for (int i = 0; i < sequence.nodesInside.Count; i++)
				{
					list.Add(sequence.nodesInside[i].ToString());
				}
				bonesToExclude = list;
			}
			sequence = intervalNode.GetSequence("BonesMirrored");
			if (sequence != null)
			{
				List<string> list2 = new List<string>();
				for (int j = 0; j < sequence.nodesInside.Count; j++)
				{
					list2.Add(sequence.nodesInside[j].ToString());
				}
				bonesToExcludeMirrored = list2;
			}
			else
			{
				bonesToExcludeMirrored = bonesToExclude;
			}
			List<IntervalAnimation> list3 = new List<IntervalAnimation>();
			list3.Add(this);
			return list3;
		}
	}
}
