using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using Godot;

namespace Nekki.Zip
{
	public static class ZipUtils
	{
		public static Dictionary<string, string> UnzipToStrings(string path, List<string> fileNames = null)
		{
			using var archive = ZipFile.OpenRead(path);
			return UnzipToStrings(archive, fileNames);
		}

		public static Dictionary<string, string> UnzipToStrings(Stream stream, List<string> fileNames = null)
		{
			using var archive = new ZipArchive(stream, ZipArchiveMode.Read);
			return UnzipToStrings(archive, fileNames);
		}

		private static Dictionary<string, string> UnzipToStrings(ZipArchive archive, List<string> fileNames = null)
		{
			var result = new Dictionary<string, string>();
			foreach (var entry in archive.Entries)
			{
				if (fileNames == null || fileNames.Contains(entry.FullName))
				{
					using var reader = new StreamReader(entry.Open());
					result.Add(entry.FullName, reader.ReadToEnd());
				}
			}
			return result;
		}

		public static string UnzipToString(string path, string filename)
		{
			var dict = UnzipToStrings(path, new List<string> { filename });
			if (dict.ContainsKey(filename))
				return dict[filename];
			GD.PrintErr("Unzip File not found - " + filename);
			return null;
		}

		public static string UnzipToString(Stream stream, string filename)
		{
			var dict = UnzipToStrings(stream, new List<string> { filename });
			return dict.ContainsKey(filename) ? dict[filename] : null;
		}

		public static Dictionary<string, byte[]> UnzipToBinaries(string path, List<string> fileNames = null)
		{
			var result = new Dictionary<string, byte[]>();
			using var archive = ZipFile.OpenRead(path);
			foreach (var entry in archive.Entries)
			{
				if (fileNames == null || fileNames.Contains(entry.FullName))
				{
					using var ms = new MemoryStream();
					entry.Open().CopyTo(ms);
					result.Add(entry.FullName, ms.ToArray());
				}
			}
			return result;
		}

		public static byte[] UnzipToBinary(string path, string filename)
		{
			var dict = UnzipToBinaries(path, new List<string> { filename });
			if (dict.ContainsKey(filename))
				return dict[filename];
			GD.PrintErr("Unzip File not found - " + filename);
			return null;
		}

		public static byte[] ReadStreamToBinary(Stream input)
		{
			using var ms = new MemoryStream();
			input.CopyTo(ms);
			return ms.ToArray();
		}
	}
}
