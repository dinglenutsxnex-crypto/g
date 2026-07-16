using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Nekki;
using Nekki.Yaml;
using Node = Nekki.Yaml.Node;
using SF3.GameModels;
using SF3.UserData;
using Godot;
using SF3.Moves;
namespace SF3.Moves
{
	public partial class MovesParser
	{
		public static void ParseSettingsFromContent(string fileName, string contentSettings, Dictionary<string, InfoAnimationPlayer> receiverAnimations, Dictionary<string, AnimationBinaries> receiverAnimationsBinaries)
		{
			YamlDocumentNekki yamlSettings = YamlDocumentNekki.FromYamlContent(contentSettings);
			ParseSettings(fileName, yamlSettings, receiverAnimations, receiverAnimationsBinaries);
		}
		public static void ParseTriggersFromContent(string filename, string contentTriggers, Dictionary<string, InfoTriggerSet> receiverTriggers)
		{
			YamlDocumentNekki yamlTriggers = YamlDocumentNekki.FromYamlContent(contentTriggers);
			ParseTriggers(filename, yamlTriggers, receiverTriggers);
		}
		public static void ParseAllFromContent(string fileName, string contentSettings, Dictionary<string, InfoAnimationPlayer> receiverAnimations, Dictionary<string, AnimationBinaries> receiverAnimationsBinaries, Dictionary<string, InfoTriggerSet> receiverTriggers)
		{
			YamlDocumentNekki yamlDocumentNekki = YamlDocumentNekki.FromYamlContent(contentSettings);
			ParseTriggers(fileName, yamlDocumentNekki, receiverTriggers);
			ParseSettings(fileName, yamlDocumentNekki, receiverAnimations, receiverAnimationsBinaries);
		}
		private static void ParseSettings(string fileName, YamlDocumentNekki yamlSettings, Dictionary<string, InfoAnimationPlayer> receiverAnimations, Dictionary<string, AnimationBinaries> receiverAnimationsBinaries)
		{
			Sequence sequence = yamlSettings.GetRoot().GetSequence("Animations");
			if (sequence == null)
			{
				return;
			}
			foreach (Mapping item in sequence.nodesInside)
			{
				Scalar text = item.GetText("AnimationPlayer");
				if (text == null)
				{
					GD.PrintErr("Cant parse AnimationPlayer name!!!");
					continue;
				}
				Scalar text2 = item.GetText("FileName");
				InfoAnimationPlayer infoAnimationPlayer;
				if (text2 != null)
				{
					string name = text2.text.Substring(0, text2.text.LastIndexOf('.'));
					AnimationBinaries animationBinaries = MovesController.GetBinariesByName(name);
					if (animationBinaries == null)
					{
						animationBinaries = AnimationBinaries.LoadFromFile(text2.text);
						if (animationBinaries != null && fileName != null && !receiverAnimationsBinaries.ContainsKey(fileName))
						{
							animationBinaries.FileName = fileName;
							receiverAnimationsBinaries.Add(fileName, animationBinaries);
						}
					}
					infoAnimationPlayer = new InfoAnimationPlayer(text.text, animationBinaries);
				}
				else
				{
					infoAnimationPlayer = new InfoAnimationPlayer(text.text);
				}
				text = item.GetText("MidFrames");
				if (text != null)
				{
					infoAnimationPlayer.SetMidFrames(int.Parse(text.text));
				}
				text = item.GetText("FirstFrame");
				if (text != null)
				{
					infoAnimationPlayer.SetStartFrame(int.Parse(text.text));
				}
				text = item.GetText("EndFrame");
				if (text != null)
				{
					infoAnimationPlayer.SetEndFrame(int.Parse(text.text));
				}
				text = item.GetText("Looped");
				if (text != null)
				{
					infoAnimationPlayer.SetLooped(text.text.Equals("1") ? true : false);
				}
				text = item.GetText("Mirrored");
				if (text != null)
				{
					infoAnimationPlayer.SetForceMirrored(int.Parse(text.text));
				}
				text = item.GetText("BlendingFrames");
				if (text != null)
				{
					infoAnimationPlayer.SetBlendingFrames(int.Parse(text.text));
				}
				text = item.GetText("SelfNoMirroring");
				if (text != null)
				{
					infoAnimationPlayer.SetSelfNoMirroring(text.text.Equals("1") ? true : false);
				}
				Sequence sequence2 = item.GetSequence("WeaponSwitchFrames");
				if (sequence2 != null)
				{
					ParseSwitchFrames(sequence2, infoAnimationPlayer);
				}
				Mapping mapping2 = item.GetMapping("Align");
				if (mapping2 != null)
				{
					ParseAlign(mapping2, infoAnimationPlayer);
				}
				mapping2 = item.GetMapping("Velocity");
				if (mapping2 != null)
				{
					Vector3 zero = Vector3.Zero;
					text = mapping2.GetText("X");
					if (text != null)
					{
						zero.X = float.Parse(text.text, CultureInfo.InvariantCulture);
					}
					text = mapping2.GetText("Y");
					if (text != null)
					{
						zero.Y = float.Parse(text.text, CultureInfo.InvariantCulture);
					}
					text = mapping2.GetText("Z");
					if (text != null)
					{
						zero.Z = float.Parse(text.text, CultureInfo.InvariantCulture);
					}
					infoAnimationPlayer.SetVelocity(zero);
				}
				mapping2 = item.GetMapping("Rotation");
				if (mapping2 != null)
				{
					Vector3 zero2 = Vector3.Zero;
					text = mapping2.GetText("X");
					if (text != null)
					{
						zero2.X = float.Parse(text.text, CultureInfo.InvariantCulture);
					}
					text = mapping2.GetText("Y");
					if (text != null)
					{
						zero2.Y = float.Parse(text.text, CultureInfo.InvariantCulture);
					}
					text = mapping2.GetText("Z");
					if (text != null)
					{
						zero2.Z = float.Parse(text.text, CultureInfo.InvariantCulture);
					}
					infoAnimationPlayer.SetRotation(zero2);
				}
				mapping2 = item.GetMapping("SetDirection");
				infoAnimationPlayer.InitDirection(mapping2);
				sequence2 = item.GetSequence("Groups");
				if (sequence2 != null)
				{
					ParseGroups(sequence2, infoAnimationPlayer);
				}
				sequence2 = item.GetSequence("Intervals");
				if (sequence2 != null)
				{
					ParseIntervals(sequence2, infoAnimationPlayer);
				}
				sequence2 = item.GetSequence("Transitions");
				if (sequence2 != null)
				{
					ParseTransitions(sequence2, infoAnimationPlayer);
				}
				sequence2 = item.GetSequence("EndsStage");
				if (sequence2 != null)
				{
					ParseStages(sequence2, infoAnimationPlayer);
				}
				sequence2 = item.GetSequence("Sounds");
				if (sequence2 != null)
				{
					ParseSounds(sequence2, infoAnimationPlayer);
				}
				ParseRepulsionRect(item, infoAnimationPlayer);
				infoAnimationPlayer.InitAnimationPlayer();
				infoAnimationPlayer.FileName = fileName;
				receiverAnimations.Add(infoAnimationPlayer.name, infoAnimationPlayer);
			}
		}
		private static void ParseSwitchFrames(Sequence sequnce, InfoAnimationPlayer AnimationPlayer)
		{
			foreach (Mapping item in sequnce.nodesInside)
			{
				string result;
				YamlUtils.TryGetString(out result, item, "Name");
				int result2;
				YamlUtils.TryGetInt(out result2, item, "Frame", -1);
				if (result != null && result2 >= 0)
				{
					switch (result)
					{
					case "Main":
						AnimationPlayer.AddWeaponSwitchFrame(false, result2);
						break;
					case "Mirror":
						AnimationPlayer.AddWeaponSwitchFrame(true, result2);
						break;
					default:
						GD.PrintErr("Wrong switch frame name!");
						break;
					}
				}
				else
				{
					GD.PrintErr("Failed to parse switch frames sequence!");
				}
			}
			AnimationPlayer.SortSwitchFrames();
		}
		private static void ParseSounds(Sequence sequnce, InfoAnimationPlayer AnimationPlayer)
		{
			List<InfoAnimationPlayer.SoundsForFrame> soundsFrameIn = InfoAnimationPlayer.SoundsForFrame.Parse(sequnce);
			AnimationPlayer.AddSounds(soundsFrameIn);
		}
		private static void ParseTriggers(string filename, YamlDocumentNekki yamlTriggers, Dictionary<string, InfoTriggerSet> receiverTriggers)
		{
			Sequence sequence = yamlTriggers.GetRoot().GetSequence("TriggerSets");
			foreach (Mapping item in sequence.nodesInside)
			{
				Scalar text = item.GetText("Name");
				if (text == null)
				{
					GD.PrintErr("Cant parse AnimationPlayer name!!!");
				}
				else
				{
					if (text.text.Equals("Tactics"))
					{
						continue;
					}
					Scalar text2 = item.GetText("Type");
					if (text2 != null && text2.text.Equals("Template"))
					{
						continue;
					}
					InfoTriggerSet infoTriggerSet = new InfoTriggerSet(text.text);
					infoTriggerSet.SetFileName(filename);
					Sequence sequence2 = item.GetSequence("Templates");
					if (sequence2 != null)
					{
						List<string> list = new List<string>();
						foreach (Scalar item2 in sequence2)
						{
							list.Add(item2.text);
						}
						infoTriggerSet.SetTemplates(list);
					}
					Sequence sequence3 = item.GetSequence("Locks");
					if (sequence3 != null)
					{
						ParseLocks(sequence3, infoTriggerSet);
					}
					Sequence sequence4 = item.GetSequence("Triggers");
					if (sequence4 != null)
					{
						foreach (Mapping item3 in sequence4)
						{
							Sequence sequence5 = item3.GetSequence("Actions");
							if (sequence5 == null)
							{
								continue;
							}
							Scalar text3 = item3.GetText("Name");
							InfoTrigger infoTrigger = new InfoTrigger(text.text, (text3 != null) ? text3.text : string.Empty);
							ParseActions(sequence5, infoTrigger);
							text3 = item3.GetText("Priority");
							if (text3 != null)
							{
								infoTrigger.SetPriority(int.Parse(text3.text));
							}
							text3 = item3.GetText("Unresumable");
							if (text3 != null)
							{
								int num = int.Parse(text3.text);
								infoTrigger.SetUnresumable(num != 0);
							}
							text3 = item3.GetText("AllowInFight");
							if (text3 != null)
							{
								int num2 = int.Parse(text3.text);
								infoTrigger.SetAllowInFight(num2 != 0);
							}
							HashSet<string> hashSet = new HashSet<string>();
							if (sequence2 != null)
							{
								foreach (Node item4 in sequence2.nodesInside)
								{
									hashSet.Add(item4.ToString());
								}
							}
							infoTrigger.SetTemplates(hashSet.ToList());
							sequence5 = item3.GetSequence("Events");
							if (sequence5 != null)
							{
								ParseEvents(sequence5, infoTrigger);
							}
							sequence5 = item3.GetSequence("Conditions");
							if (sequence5 != null)
							{
								ParseConditions(sequence5, infoTrigger);
							}
							Mapping mapping3 = item3.GetMapping("Tactics");
							if (mapping3 != null)
							{
								ParseTactics(mapping3, infoTrigger);
							}
							infoTriggerSet.AddTrigger(infoTrigger);
						}
					}
					receiverTriggers.Add(infoTriggerSet.name, infoTriggerSet);
				}
			}
		}
		private static void ParseAlign(Mapping nodeAlign, InfoAnimationPlayer animInfo)
		{
			MoveAlign moveAlign = new MoveAlign();
			Sequence sequence = nodeAlign.GetSequence("Axis");
			bool x;
			bool y;
			bool z;
			if (sequence == null)
			{
				x = (y = (z = true));
			}
			else
			{
				x = (y = (z = false));
				foreach (Scalar item in sequence.nodesInside)
				{
					if (item.text.Equals("X"))
					{
						x = true;
						continue;
					}
					if (item.text.Equals("Y"))
					{
						y = true;
						continue;
					}
					if (item.text.Equals("Z"))
					{
						z = true;
						continue;
					}
					GD.PrintErr(string.Concat("ERROR: alignParse - wrong axis ", item, " in ", item.text));
				}
			}
			moveAlign.SetAxis(x, y, z);
			Mapping mapping = nodeAlign.GetMapping("Shift");
			Scalar text;
			if (mapping != null)
			{
				text = mapping.GetText("X");
				if (text != null)
				{
					moveAlign.SetShift(new Vector3(float.Parse(text.text, CultureInfo.InvariantCulture), moveAlign.shift.Y, moveAlign.shift.Z));
				}
				text = mapping.GetText("Y");
				if (text != null)
				{
					moveAlign.SetShift(new Vector3(moveAlign.shift.X, float.Parse(text.text, CultureInfo.InvariantCulture), moveAlign.shift.Z));
				}
				text = mapping.GetText("Z");
				if (text != null)
				{
					moveAlign.SetShift(new Vector3(moveAlign.shift.X, moveAlign.shift.Y, float.Parse(text.text, CultureInfo.InvariantCulture)));
				}
			}
			Mapping mapping2 = nodeAlign.GetMapping("Position");
			PositionObject positionObject = new PositionObject();
			text = mapping2.GetText("Player");
			if (text != null)
			{
				positionObject.playerType = Model.GetPlayerTypeByName(text.text);
			}
			text = mapping2.GetText("Object");
			positionObject.pivotObject = Model.GetPivotObjectByName(text.text);
			text = mapping2.GetText("Part");
			if (text != null)
			{
				positionObject.partName = text.text;
			}
			moveAlign.SetAlignPosition(positionObject);
			Mapping mapping3 = nodeAlign.GetMapping("Pivot");
			PositionObject positionObject2 = new PositionObject();
			text = mapping3.GetText("Object");
			positionObject2.pivotObject = Model.GetPivotObjectByName(text.text);
			text = mapping3.GetText("Part");
			if (text != null)
			{
				positionObject2.partName = text.text;
			}
			moveAlign.SetAlignPivot(positionObject2);
			text = nodeAlign.GetText("FollowPositionObject");
			if (text != null)
			{
				moveAlign.SetFollowPositionObject(text.text.Equals("1") ? true : false);
			}
			Mapping mapping4 = nodeAlign.GetMapping("Rotation");
			if (mapping4 != null)
			{
				Vector3 zero = Vector3.Zero;
				text = mapping4.GetText("X");
				if (text != null)
				{
					zero.X = float.Parse(text.text, CultureInfo.InvariantCulture);
				}
				text = mapping4.GetText("Y");
				if (text != null)
				{
					zero.Y = float.Parse(text.text, CultureInfo.InvariantCulture);
				}
				text = mapping4.GetText("Z");
				if (text != null)
				{
					zero.Z = float.Parse(text.text, CultureInfo.InvariantCulture);
				}
				moveAlign.SetRotation(zero);
			}
			animInfo.SetAlign(moveAlign);
		}
		private static void ParseStages(Sequence nodes, InfoAnimationPlayer animInfo)
		{
			foreach (Scalar node in nodes)
			{
				FightController.EFightStage enumerator2 = EnumsCompliancer.GetEnumerator<FightController.EFightStage>(node.GetText());
				animInfo.stages.Add(enumerator2);
			}
		}
		private static void ParseTransitions(Sequence nodes, InfoAnimationPlayer animInfo)
		{
			foreach (Mapping node in nodes)
			{
				Scalar text = node.GetText("FrameShift");
				if (text == null)
				{
					continue;
				}
				AnimationTransition animationTransition = new AnimationTransition();
				animationTransition.SetFrameShift(int.Parse(text.text));
				text = node.GetText("AITransistable");
				if (text != null)
				{
					animInfo.aiTransistable = text.text.Equals("1") || text.text.ToUpper().Equals("TRUE");
				}
				Sequence sequence = node.GetSequence("Animations");
				foreach (Scalar item in sequence)
				{
					animationTransition.AddAnimationPlayer(item.text);
				}
				animInfo.AddTransition(animationTransition);
			}
		}
		private static void ParseGroups(Sequence nodes, InfoAnimationPlayer animInfo)
		{
			List<string> list = new List<string>();
			foreach (Scalar node in nodes)
			{
				list.Add(node.text);
			}
			animInfo.SetGroups(list);
		}
		private static void ParseIntervals(Sequence nodes, InfoAnimationPlayer animInfo)
		{
			List<IntervalAnimationPlayer> list = IntervalAnimationPlayer.Create(nodes);
			if (list != null)
			{
				animInfo.SetIntervals(list);
			}
			else
			{
				GD.PushWarning("AnimationPlayer intervals is null");
			}
		}
		private static void ParseTactics(Mapping nodeTactics, InfoTrigger infoTrigger)
		{
			Tactics tactics = Tactics.Parse(nodeTactics);
			if (tactics != null)
			{
				infoTrigger.SetTactics(tactics);
			}
			else
			{
				GD.PushWarning("Tactics is null");
			}
		}
		private static void ParseLocks(Sequence nodeLocks, InfoTriggerSet infoTriggerSet)
		{
			List<IConditionEqual> list = Condition.Parse(nodeLocks);
			if (list != null && list.Count > 0)
			{
				infoTriggerSet.SetLocks(list);
			}
			else
			{
				GD.PushWarning("Havnt any locks here :)");
			}
		}
		private static void ParseConditions(Sequence nodeConditions, InfoTrigger infoTrigger)
		{
			List<IConditionEqual> list = Condition.Parse(nodeConditions);
			if (list != null && list.Count > 0)
			{
				infoTrigger.SetConditions(list);
			}
			else
			{
				GD.PushWarning("Havnt any conditions here ^^");
			}
		}
		private static void ParseEvents(Sequence nodes, InfoTrigger infoTrigger)
		{
			List<TriggerEvent> list = TriggerEvent.Parse(nodes);
			if (list != null && list.Count > 0)
			{
				infoTrigger.SetEvents(list);
			}
			else
			{
				GD.PushWarning("Havnt any events here ^^");
			}
		}
		private static void ParseActions(Sequence nodes, InfoTrigger infoTrigger)
		{
			List<ITriggerAction> list = new List<ITriggerAction>();
			foreach (Node item in nodes.nodesInside)
			{
				ITriggerAction triggerAction = TriggerAction.Create(item);
				if (triggerAction != null)
				{
					list.Add(triggerAction);
				}
			}
			infoTrigger.SetActions(list);
		}
		private static void ParseRepulsionRect(Mapping sourceNode, InfoAnimationPlayer AnimationPlayer)
		{
			Mapping mapping = sourceNode.GetMapping("RepulsionScale");
			if (mapping != null)
			{
				Vector2 one = Vector2.One;
				Scalar scalar = null;
				scalar = mapping.GetText("X");
				if (scalar != null)
				{
					one.X = float.Parse(scalar.ToString(), CultureInfo.InvariantCulture);
				}
				scalar = mapping.GetText("Y");
				if (scalar != null)
				{
					one.Y = float.Parse(scalar.ToString(), CultureInfo.InvariantCulture);
				}
				AnimationPlayer.SetRepulsionScale(one);
			}
		}
	}
}

