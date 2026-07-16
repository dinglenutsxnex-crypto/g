using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SF3.GameModels
{
	public class RepulsionRect
	{
		public float widthScale = 1f;
		public float heightScale = 1f;
		public int uniqueKey;
		public float minimumSize;
		public List<string> exclusiveKeys;
		public Dictionary<string, Node3D> points;

		public Vector3 center { get; private set; }
		public Vector2 size { get; private set; }
		public float pointLeft { get; private set; }
		public float pointRight { get; private set; }
		public float pointUp { get; private set; }
		public float pointBot { get; private set; }

		public RepulsionRect(Dictionary<string, Node3D> _points, float widthScale, float heightScale, float? minimumSize = null)
		{
			points = _points;
			this.widthScale = widthScale;
			this.heightScale = heightScale;
			if (minimumSize.HasValue)
			{
				this.minimumSize = minimumSize.Value;
			}
		}

		public RepulsionRect(List<Node3D> _points, float widthScale, float heightScale, float? minimumSize = null)
		{
			points = new Dictionary<string, Node3D>();
			foreach (Node3D _point in _points)
			{
				points.Add(uniqueKey++.ToString(), _point);
			}
			this.widthScale = widthScale;
			this.heightScale = heightScale;
			if (minimumSize.HasValue)
			{
				this.minimumSize = minimumSize.Value;
			}
		}

		public void Calclulate()
		{
			if (points == null || points.Count == 0)
			{
				return;
			}
			int num = 0;
			Vector3 vector = Vector3.Zero;
			Node3D value = points.First().Value;
			pointLeft = value.Position.x;
			pointRight = value.Position.x;
			pointUp = value.Position.y;
			pointBot = value.Position.y;
			foreach (KeyValuePair<string, Node3D> point in points)
			{
				if (exclusiveKeys == null || !exclusiveKeys.Contains(point.Key))
				{
					Node3D value2 = point.Value;
					vector += value2.Position;
					if (value2.Position.x < pointLeft)
					{
						pointLeft = value2.Position.x;
					}
					if (value2.Position.x > pointRight)
					{
						pointRight = value2.Position.x;
					}
					if (value2.Position.y > pointUp)
					{
						pointUp = value2.Position.y;
					}
					if (value2.Position.y < pointBot)
					{
						pointBot = value2.Position.y;
					}
					num++;
				}
			}
			center = vector / num;
			size = new Vector2((pointRight - pointLeft) * widthScale, (pointUp - pointBot) * heightScale);
			if (size.x < minimumSize)
			{
				size = new Vector2(minimumSize, size.y);
			}
			if (size.y < minimumSize)
			{
				size = new Vector2(size.x, minimumSize);
			}
			pointLeft = center.x - size.x / 2f;
			pointRight = center.x + size.x / 2f;
			pointBot = center.y - size.y / 2f;
			pointUp = center.y + size.y / 2f;
		}

		public void SetScale(Vector2 scale)
		{
			widthScale = scale.x;
			heightScale = scale.y;
		}

		public float GetMinYPosition()
		{
			float y = points.First().Value.Position.y;
			foreach (KeyValuePair<string, Node3D> point in points)
			{
				Node3D value = point.Value;
				if (value.Position.y < y)
				{
					y = value.Position.y;
				}
			}
			return y;
		}
	}
}
