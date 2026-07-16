using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Nekki.Yaml
{
    [Serializable]
    public class YamlDocumentNekki
    {
        private Mapping _rootMapping;
        private string _content;

        public YamlDocumentNekki() { }

        public static YamlDocumentNekki FromYamlFile(string fileName)
        {
            string text = FilesUtil.ReadFileText(fileName);
            if (string.IsNullOrEmpty(text))
            {
                GD.PrintErr("YAML file is not exists!!!");
                return null;
            }
            return FromYamlContent(text);
        }

        public static YamlDocumentNekki FromYamlContent(string yamlContent)
        {
            YamlDocumentNekki result = new YamlDocumentNekki();
            result._content = yamlContent;
            result._rootMapping = ParseYamlToMapping(yamlContent);
            return result;
        }

        private static Mapping ParseYamlToMapping(string yamlContent)
        {
            Mapping root = new Mapping("Root");
            string[] lines = yamlContent.Split('\n');
            int indent = 0;
            ParseBlock(lines, 0, ref indent, root);
            return root;
        }

        private static int ParseBlock(string[] lines, int startLine, ref int currentIndent, Node parentNode)
        {
            int i = startLine;
            while (i < lines.Length)
            {
                string line = lines[i];
                if (string.IsNullOrWhiteSpace(line)) { i++; continue; }

                int indent = 0;
                foreach (char c in line)
                {
                    if (c == ' ' || c == '\t') indent++;
                    else break;
                }

                if (i > startLine && indent <= currentIndent)
                {
                    currentIndent = indent;
                    return i;
                }

                string trimmed = line.Trim();
                if (trimmed.StartsWith("#")) { i++; continue; }

                if (trimmed.EndsWith(":"))
                {
                    string key = trimmed.Substring(0, trimmed.Length - 1).Trim();
                    Mapping mapping = new Mapping(key);
                    if (parentNode is Mapping parentMapping)
                        parentMapping.Add(mapping);
                    else if (parentNode is Sequence parentSeq)
                        parentSeq.AddNode(mapping);

                    int subIndent = indent + 2;
                    i = ParseBlock(lines, i + 1, ref subIndent, mapping);
                    currentIndent = subIndent;
                }
                else if (trimmed.StartsWith("- "))
                {
                    string item = trimmed.Substring(2);
                    if (!(parentNode is Sequence))
                    {
                        string seqKey = (parentNode is Mapping pm) ? pm.GetKeys().Count.ToString() : "items";
                        var seq = new Sequence(seqKey);
                        ((Mapping)parentNode).Add(seq);
                        parentNode = seq;
                    }

                    if (item.EndsWith(":"))
                    {
                        string key = item.Substring(0, item.Length - 1);
                        Mapping subMapping = new Mapping(key);
                        ((Sequence)parentNode).AddNode(subMapping);
                        int subIndent = indent + 2;
                        i = ParseBlock(lines, i + 1, ref subIndent, subMapping);
                        currentIndent = subIndent;
                    }
                    else
                    {
                        int colonIdx = item.IndexOf(':');
                        if (colonIdx > 0)
                        {
                            string k = item.Substring(0, colonIdx).Trim();
                            string v = item.Substring(colonIdx + 1).Trim();
                            ((Sequence)parentNode).AddNode(new Scalar(k, v));
                        }
                        else
                        {
                            ((Sequence)parentNode).AddNode(new Scalar("item", item));
                        }
                        i++;
                    }
                }
                else if (trimmed.Contains(": "))
                {
                    int colonIdx = trimmed.IndexOf(':');
                    string key = trimmed.Substring(0, colonIdx).Trim();
                    string value = trimmed.Substring(colonIdx + 1).Trim();
                    if (parentNode is Mapping parentMapping)
                        parentMapping.Add(new Scalar(key, value));
                    else if (parentNode is Sequence parentSeq)
                        parentSeq.AddNode(new Scalar(key, value));
                    i++;
                }
                else
                {
                    if (parentNode is Sequence parentSeq)
                        parentSeq.AddNode(new Scalar("item", trimmed));
                    else if (parentNode is Mapping parentMapping)
                    {
                        string key = parentMapping.GetKeys().Count.ToString();
                        parentMapping.Add(new Scalar(key, trimmed));
                    }
                    i++;
                }
            }
            return i;
        }

        public Node GetRoot(string name)
        {
            return _rootMapping.GetNode(name);
        }

        public Mapping GetRoot(int index = 0)
        {
            return _rootMapping;
        }

        public override string ToString()
        {
            return _content;
        }

        public string GetYamlContent()
        {
            return _content;
        }
    }
}
