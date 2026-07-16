using System;
using System.Collections.Generic;
using Nekki.UI;

public class LocaleImport
{
    [Serializable]
    public class LocaleString
    {
        public enum SectionTypes { Text = 0, Image = 1 }

        [Serializable]
        public class Section
        {
            [Serializable]
            public class PositionInfo
            {
                public int Index;
                public int Scale;
                public bool ScaleToRow;
                public string Content;

                public PositionInfo(int index, Section section)
                {
                    Content = section.Content;
                    ScaleToRow = section.ScaleToRow;
                    Scale = section.Scale;
                    Index = index;
                }
            }

            public int ID;
            public SectionTypes SectionType;
            public string Content;
            public int Scale;
            public bool ScaleToRow;
            public List<PositionInfo> Positions = new List<PositionInfo>();

            public Section(string content)
            {
                Positions = new List<PositionInfo>();
                Content = content;
                ID = -1;
                ScaleToRow = false;
                Scale = 100;
                SectionType = content.Contains("image") ? SectionTypes.Image : SectionTypes.Text;
                try
                {
                    if (content.Contains(":"))
                    {
                        string[] array = content.Trim('{', '}', ' ').Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                        if (array.Length == 2 && array[0].Contains("image"))
                        {
                            if (array[1].ToLower().Equals("row"))
                                ScaleToRow = true;
                            else
                            {
                                try { Scale = int.Parse(array[1]); } catch { Scale = 100; }
                            }
                            ID = int.Parse(array[0].Replace("image", string.Empty).Trim('{', '}'));
                        }
                    }
                    else
                    {
                        ID = int.Parse(content.Replace("image", string.Empty).Trim('{', '}'));
                    }
                }
                catch { ID = -1; }
            }

            public bool ContainsPosition(int index)
            {
                foreach (PositionInfo p in Positions)
                    if (p.Index == index) return true;
                return false;
            }

            public PositionInfo GetPositionAtIndex(int index)
            {
                foreach (PositionInfo p in Positions)
                    if (p.Index == index) return p;
                return null;
            }

            public void ReplacePositionAtIndex(int index, PositionInfo info)
            {
                for (int i = 0; i < Positions.Count; i++)
                    if (Positions[i].Index == index)
                        Positions[i] = info;
            }
        }

        public bool Simple;
        public bool ContainsImageRef;
        public string String;
        public List<Section> Sections = new List<Section>();
        public List<string> ImgSplit = new List<string>();

        public LocaleString(string source)
        {
            Simple = true;
            source = source.Trim();
            String = source;
            if (string.IsNullOrEmpty(String)) return;
            int num = -1;
            int num2 = (source[0] == '{') ? -1 : 0;
            for (int i = 0; i < source.Length; i++)
            {
                if (num == -1 && source[i].Equals('{'))
                {
                    num = i;
                    Simple = false;
                }
                if (!source[i].Equals('}')) continue;
                Simple = false;
                Section section = new Section(source.Substring(num, i - num + 1));
                if (section.ID > -1)
                {
                    if (!ContainsSection(section.ID))
                        Sections.Add(section);
                    if (section.SectionType == SectionTypes.Image)
                    {
                        Section sectionByID = GetSectionByID(section.ID);
                        if (sectionByID.ContainsPosition(num2))
                            sectionByID.ReplacePositionAtIndex(num2, new Section.PositionInfo(num2, section));
                        else
                            sectionByID.Positions.Add(new Section.PositionInfo(num2, section));
                        num2++;
                        if (!ImgSplit.Contains(section.Content))
                        {
                            ContainsImageRef = true;
                            ImgSplit.Add(section.Content);
                        }
                    }
                }
                num = -1;
            }
        }

        public bool ContainsSection(int id)
        {
            foreach (Section s in Sections)
                if (s.ID == id) return true;
            return false;
        }

        public Section GetSectionByID(int id)
        {
            foreach (Section s in Sections)
                if (s.ID == id) return s;
            return null;
        }

        public void ReplaceSectionWithID(int id, Section section)
        {
            for (int i = 0; i < Sections.Count; i++)
                if (Sections[i].ID == id)
                    Sections[i] = section;
        }

        public static implicit operator string(LocaleString i) { return i.String; }

        public string[] CompileWith(object[] replacement, NekkiUILabel label)
        {
            string text = String;
            for (int i = 0; i < replacement.Length; i++)
            {
                if (ContainsSection(i))
                {
                    Section sectionByID = GetSectionByID(i);
                    if (sectionByID.SectionType == SectionTypes.Text)
                        text = text.Replace(sectionByID.Content, replacement[i].ToString());
                }
            }
            if (!ContainsImageRef)
                return new string[] { text };
            return text.Split(ImgSplit.ToArray(), StringSplitOptions.RemoveEmptyEntries);
        }
    }

    public SystemLanguage Language { get; private set; }
    public Dictionary<string, LocaleString> Data { get; private set; }

    public LocaleImport(SystemLanguage language)
    {
        Language = language;
        Data = new Dictionary<string, LocaleString>();
        Load();
    }

    private void Load()
    {
        string localization = ConfigsSourceResolver.GetLocalization(Language);
        if (string.IsNullOrEmpty(localization)) return;
        var xmlDocument = new System.Xml.XmlDocument();
        xmlDocument.LoadXml(localization);
        foreach (object childNode in xmlDocument.ChildNodes)
        {
            if (!(childNode is System.Xml.XmlElement) || !((System.Xml.XmlElement)childNode).Name.Equals("Localization"))
                continue;
            System.Xml.XmlNode firstChild = ((System.Xml.XmlElement)childNode).FirstChild;
            foreach (object childNode2 in firstChild.ChildNodes)
            {
                var xmlElement = childNode2 as System.Xml.XmlElement;
                if (xmlElement != null)
                {
                    string attribute = xmlElement.GetAttribute("Title");
                    string innerText = xmlElement.InnerText;
                    if (!Data.ContainsKey(attribute))
                        Data.Add(attribute, new LocaleString(innerText));
                    else
                        GD.PrintErr(string.Format("key [{0}] is not unique", attribute));
                }
            }
        }
    }

    public static implicit operator SystemLanguage(LocaleImport i) { return i.Language; }
    public static implicit operator Dictionary<string, LocaleString>(LocaleImport i) { return i.Data; }
}
