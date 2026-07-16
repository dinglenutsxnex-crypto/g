using System;

namespace sf3DTO
{
	public static class Sf3Reflection
	{
		public static Type[] MessageTypes { get; private set; }

		static Sf3Reflection()
		{
			MessageTypes = new Type[0];
		}
	}
}
