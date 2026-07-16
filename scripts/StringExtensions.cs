using System;

public static class StringExtensions
{
	public static int CountChars(this string str, char ch)
	{
		if (string.IsNullOrEmpty(str))
			return 0;

		int num = 0;
		foreach (char c in str)
		{
			if (c == ch)
			{
				num++;
			}
		}
		return num;
	}
}