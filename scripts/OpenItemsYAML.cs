using Godot;
using System.Collections.Generic;
using Nekki.Yaml;

[Tool]
public class OpenItemsYAML : Node
{
    [Export] public TextAsset items;
    [Export] public TextAsset itemTypes;
    private Dictionary<string, List<Node>> itemScalarData;

    private void Run()
    {
        string text = itemTypes.Text;
        YamlDocumentNekki yamlDocumentNekki = YamlDocumentNekki.FromYamlContent(text);
        Sequence sequence = yamlDocumentNekki.GetRoot().GetSequence("ItemTypes");
        itemScalarData = new Dictionary<string, List<Node>>();
        foreach (Mapping item in sequence)
        {
            foreach (Mapping item2 in item.nodesInside)
            {
                string text2 = FindSubTypeKey(item2);
                if (text2 != string.Empty)
                {
                    itemScalarData.Add(text2, new List<Node>());
                    FillScalarData(text2, item2);
                }
            }
        }
        foreach (string key in itemScalarData.Keys)
        {
            GD.Print(key + " ----- ");
            foreach (Node item3 in itemScalarData[key])
                GD.Print(string.Concat(item3.value, " ", item3.key));
        }
        text = items.Text;
        YamlDocumentNekki yamlDocumentNekki2 = YamlDocumentNekki.FromYamlContent(text);
        foreach (Mapping item4 in yamlDocumentNekki2.GetRoot().GetSequence("Items"))
        {
            foreach (Node item5 in item4.nodesInside)
            {
                if (!(item5.key == "ItemTypes")) continue;
                foreach (Sequence item6 in ((Mapping)item5).nodesInside)
                {
                    for (int i = 0; i < item6.nodesInside.Count; i++)
                    {
                        GD.Print("////");
                        AddNewScalars((Mapping)item6.nodesInside[i], item6.nodesInside[i].key);
                        GD.Print(item6.nodesInside[i].value);
                    }
                }
            }
        }
    }

    private Mapping AddNewScalars(Mapping item, string subType)
    {
        GD.Print(item.value);
        if (itemScalarData.ContainsKey(subType))
        {
            bool flag = false;
            Mapping mapping = new Mapping(item.key, new Node[0]);
            foreach (Node item2 in itemScalarData[subType])
            {
                flag = true;
                foreach (Node item3 in item.nodesInside)
                {
                    if (!(item2.key == item3.key)) continue;
                    if (item2.key == "Attributes")
                    {
                        foreach (Node item4 in ((Mapping)item2).nodesInside)
                        {
                            if (((Mapping)item3).GetNode(item4.key) == null)
                                ((Mapping)item3).Add(item4);
                        }
                    }
                    else
                        mapping.Add(item3);
                    flag = false;
                    break;
                }
                if (flag)
                {
                    mapping.Add(item2);
                    item.Add(item2);
                }
            }
        }
        return item;
    }

    private void FillScalarData(string key, Node node)
    {
        if (node is Mapping)
        {
            if (!(node.key == "Attributes"))
            {
                foreach (Node item in ((Mapping)node).nodesInside)
                    FillScalarData(key, item);
                return;
            }
            itemScalarData[key].Add(node);
        }
        else if (node is Scalar)
            itemScalarData[key].Add((Scalar)node);
    }

    private string FindSubTypeKey(Node node)
    {
        if (node is Mapping)
        {
            foreach (Node item in ((Mapping)node).nodesInside)
            {
                string text = FindSubTypeKey(item);
                if (text != string.Empty) return text;
            }
        }
        else if (node is Scalar && node.key == "SubType")
            return node.value.ToString();
        return string.Empty;
    }
}
