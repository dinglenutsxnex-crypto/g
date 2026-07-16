using System;
using System.Collections.Generic;
using Godot;
namespace SF3.Moves
{
	[Serializable]
	public partial class AnimationBinaries : IHasFileName
	{
		private string _fileName;
		public string name { get; private set; }
		public AnimationFrame[] frames { get; private set; }
		public int[] bonesIDs { get; private set; }
		public int bonesCount
		{
			get
			{
				return bonesIDs.Length;
			}
		}
		public AnimationBinaries animationTangents { get; private set; }
		public string FileName { get; set; }
		private AnimationBinaries(string fileNameVal)
		{
			if (fileNameVal.Length > 0)
			{
				_fileName = fileNameVal.Replace(".bin", ".bytes");
				name = _fileName.Substring(0, _fileName.LastIndexOf('.'));
			}
			else
			{
				_fileName = (name = string.Empty);
			}
			bonesIDs = new int[0];
			animationTangents = null;
		}
		public static AnimationBinaries LoadFromFile(string fileNameVal)
		{
			AnimationBinaries animationBinaries = new AnimationBinaries(fileNameVal);
			if (animationBinaries._fileName.Length == 0)
			{
				GD.PrintErr("AnimationPlayer file name is empty");
				return null;
			}
			byte[] loadBytesInternal = GlobalLoad.GetLoadBytesInternal("animations", "binaries/" + animationBinaries._fileName);
			if (loadBytesInternal == null || loadBytesInternal.Length == 0)
			{
				GD.PrintErr(string.Format("Cant load AnimationPlayer: {0}", animationBinaries._fileName));
				return null;
			}
			using (BinaryReaderNekki binaryReaderNekki = new BinaryReaderNekki(loadBytesInternal))
			{
				long num = binaryReaderNekki.ReadInt64();
				if (num != 457546134634732L)
				{
					GD.PrintErr("Wrong AnimationPlayer file type");
					return null;
				}
				short num2 = binaryReaderNekki.ReadInt16();
				long[] array = new long[num2];
				for (int i = 0; i < num2; i++)
				{
					array[i] = binaryReaderNekki.ReadInt64();
				}
				animationBinaries.LoadAnimationPlayer(binaryReaderNekki);
				if (num2 > 1)
				{
					animationBinaries.animationTangents = new AnimationBinaries(string.Empty);
					animationBinaries.animationTangents.LoadAnimationPlayer(binaryReaderNekki);
				}
			}
			return animationBinaries;
		}
		private void LoadAnimationPlayer(BinaryReaderNekki br)
		{
			frames = new AnimationFrame[br.ReadInt32()];
			bonesIDs = new int[br.ReadInt32()];
			for (int i = 0; i < bonesIDs.Length; i++)
			{
				bonesIDs[i] = br.ReadInt16();
			}
			float[] array = br.ConvertByteArrayToFloat(frames.Length * bonesIDs.Length * 7);
			Vector3 newPosition = default(Vector3);
			Quaternion newRotation = default(Quaternion);
			for (int j = 0; j < frames.Length; j++)
			{
				frames[j] = new AnimationFrame();
				frames[j].bonesAnimationPlayer = new AnimatedNode3D[bonesIDs.Length];
				for (int k = 0; k < bonesIDs.Length; k++)
				{
					int num = j * bonesIDs.Length * 7 + k * 7;
					newPosition.x = array[num];
					newPosition.y = array[num + 1];
					newPosition.z = array[num + 2];
					newRotation.x = array[num + 3];
					newRotation.y = array[num + 4];
					newRotation.z = array[num + 5];
					newRotation.w = array[num + 6];
					frames[j].bonesAnimationPlayer[k] = new AnimatedNode3D(newPosition, newRotation);
				}
			}
		}
		public void CopyFrameTransformByIndex(int frameNumber, int boneIndex, AnimatedNode3D copyTo)
		{
			if (boneIndex < 0)
			{
				AnimatedNode3D.CopyBoneNode3D(AnimatedNode3D.IDENTITY, copyTo);
			}
			else
			{
				AnimatedNode3D.CopyBoneNode3D(frames[frameNumber].bonesAnimationPlayer[boneIndex], copyTo);
			}
		}
		public void CopyFrameTransformByID(int frameNumber, int boneID, AnimatedNode3D copyTo)
		{
			int boneIndexByID = GetBoneIndexByID(boneID);
			if (boneIndexByID < 0)
			{
				AnimatedNode3D.CopyBoneNode3D(AnimatedNode3D.IDENTITY, copyTo);
			}
			else
			{
				AnimatedNode3D.CopyBoneNode3D(frames[frameNumber].bonesAnimationPlayer[boneIndexByID], copyTo);
			}
		}
		public void CopyFrameTransforms(int frameNumber, AnimatedNode3D[] copyTo)
		{
			if (frameNumber >= frames.Length)
			{
				throw new Exception(string.Format("frameNumber {0} is out of range frames {1}", frameNumber, frames.Length));
			}
			for (int i = 0; i < frames[frameNumber].bonesAnimationPlayer.Length; i++)
			{
				AnimatedNode3D.CopyBoneNode3D(frames[frameNumber].bonesAnimationPlayer[i], copyTo[i]);
			}
		}
		public void CopyFrameTransforms(int frameNumber, Dictionary<int, AnimatedNode3D> copyTo)
		{
			for (int i = 0; i < frames[frameNumber].bonesAnimationPlayer.Length; i++)
			{
				if (copyTo.ContainsKey(bonesIDs[i]))
				{
					AnimatedNode3D.CopyBoneNode3D(frames[frameNumber].bonesAnimationPlayer[i], copyTo[bonesIDs[i]]);
					copyTo[bonesIDs[i]].animateThisFrame = true;
				}
			}
		}
		private int GetBoneIndexByID(int boneID)
		{
			for (int i = 0; i < bonesIDs.Length; i++)
			{
				if (bonesIDs[i] == boneID)
				{
					return i;
				}
			}
			return -1;
		}
	}
}

