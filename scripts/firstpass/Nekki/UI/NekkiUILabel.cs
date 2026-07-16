using Godot;
using System;
using System.Collections.Generic;

namespace Nekki.UI
{
    public class NekkiUILabel : Control, IHasAlias
    {
        public enum Types { Localized = 0, Simple = 1 }

        public class SubUnit
        {
            public Label Label;
            public NekkiUISprite Sprite;
            protected ImageData data;

            public Node Obj
            {
                get
                {
                    if (Label != null) return Label;
                    if (Sprite != null) return Sprite;
                    return null;
                }
            }

            public Vector2 Pivot { get { return Vector2.Zero; } }
            public float Width { get { return UnscaledWidth * (Obj != null ? Obj.Scale.X : 1f); } }
            public float UnscaledWidth
            {
                get
                {
                    if (Label != null) return Label.Size.X;
                    if (Sprite != null) return Sprite.Size.X;
                    return 0f;
                }
            }
            public float Height { get { return UnscaledHeight * (Obj != null ? Obj.Scale.Y : 1f); } }
            public float UnscaledHeight
            {
                get
                {
                    if (Label != null) return Label.Size.Y;
                    if (Sprite != null) return Sprite.Size.Y;
                    return 0f;
                }
            }
            public ImageData Data { get { return data; } }
            public Node Unit { get { if (Label != null) return Label; return Sprite; } }

            public SubUnit(Label label) { Label = label; }
            public SubUnit(NekkiUISprite sprite, ImageData data) { Sprite = sprite; this.data = data; }
        }

        private class Line
        {
            public NekkiUILabel Parent;
            private Vector2 _verticalBounds = Vector2.Zero;
            protected LineData data;
            public List<SubUnit> SubUnits = new List<SubUnit>();

            public LineData Data { get { return data; } }

            public Vector2 VerticalBounds
            {
                get
                {
                    if (_verticalBounds != Vector2.Zero || SubUnits.Count == 0)
                        return _verticalBounds;
                    _verticalBounds = new Vector2(float.MaxValue, float.MinValue);
                    for (int i = 0; i < SubUnits.Count; i++)
                    {
                        float num = (0f - SubUnits[i].UnscaledHeight) / 2f + GetSubUnitOffsetY(i);
                        if (num < _verticalBounds.X) _verticalBounds.X = num;
                        num = SubUnits[i].UnscaledHeight / 2f + GetSubUnitOffsetY(i);
                        if (num > _verticalBounds.Y) _verticalBounds.Y = num;
                    }
                    _verticalBounds.Y += data.PaddingTop;
                    return _verticalBounds;
                }
            }

            public Vector3 Position
            {
                get
                {
                    if (SubUnits.Count > 0)
                        return new Vector3(SubUnits[0].Obj.Position.X, SubUnits[0].Obj.Position.Y + (VerticalBounds.Y + VerticalBounds.X) * SubUnits[0].Obj.Scale.Y / 2f);
                    return Vector3.Zero;
                }
            }

            public float Width { get { return UnscaledWidth * SubUnits[0].Obj.Scale.X; } }
            public float Height { get { return UnscaledHeight * SubUnits[0].Obj.Scale.Y; } }
            public float UnscaledWidth
            {
                get
                {
                    float num = 0f;
                    for (int i = 0; i < SubUnits.Count; i++)
                        num += SubUnits[i].UnscaledWidth;
                    return num + Parent.Margin.X * (float)(SubUnits.Count - 1);
                }
            }
            public float UnscaledHeight { get { return VerticalBounds.Y - VerticalBounds.X; } }
            public float Scale { get { return SubUnits[0].Obj.Scale.Y; } }

            public Line(NekkiUILabel parent, LineData lineData) { Parent = parent; data = lineData; }

            protected float GetSubUnitOffsetY(int index)
            {
                return (SubUnits[index].Data != null) ? ((!SubUnits[index].Data.UseDefaultImageOffset) ? SubUnits[index].Data.CustomOffsetY : Parent.DefaultImageOffsetY) : 0;
            }

            public void Add(SubUnit unit)
            {
                for (int i = 0; i < SubUnits.Count; i++)
                {
                    if (SubUnits[i].Obj == unit.Obj)
                    {
                        SubUnits.RemoveAt(i);
                        break;
                    }
                }
                SubUnits.Add(unit);
            }

            public bool Remove(Label subLabel)
            {
                for (int i = 0; i < SubUnits.Count; i++)
                {
                    if (SubUnits[i].Unit == subLabel)
                    {
                        SubUnits.Remove(SubUnits[i]);
                        return true;
                    }
                }
                return false;
            }

            public void MoveHorizontal(float delta)
            {
                for (int i = 0; i < SubUnits.Count; i++)
                    SubUnits[i].Obj.Position += Vector3.Left * delta;
            }

            public void MoveVertical(float delta)
            {
                for (int i = 0; i < SubUnits.Count; i++)
                    SubUnits[i].Obj.Position += Vector3.Down * delta;
            }

            public void Clear(bool editorUsePool)
            {
                SubUnits.Clear();
            }

            public void LineUp()
            {
                for (int i = 0; i < SubUnits.Count; i++)
                {
                    float y = GetSubUnitOffsetY(i) * SubUnits[i].Obj.Scale.X;
                    if (i == 0)
                        SubUnits[i].Obj.Position = new Vector3(GetXOffset() * SubUnits[i].Obj.Scale.X, y);
                    else if (SubUnits[i - 1].Label != null && string.IsNullOrEmpty(SubUnits[i - 1].Label.Text))
                        SubUnits[i].Obj.Position = new Vector3(SubUnits[i - 1].Obj.Position.X + Parent.Margin.X * SubUnits[i].Obj.Scale.X, y);
                    else
                        SubUnits[i].Obj.Position = new Vector3(SubUnits[i - 1].Width + SubUnits[i - 1].Obj.Position.X + Parent.Margin.X * SubUnits[i].Obj.Scale.X, y);
                }
            }

            public void Shrink(float scale)
            {
                if (SubUnits.Count > 0 && SubUnits[0].Obj.Scale.X != scale)
                {
                    for (int i = 0; i < SubUnits.Count; i++)
                        SubUnits[i].Obj.Scale = new Vector3(scale, scale, scale);
                }
            }

            private float GetXOffset()
            {
                float result = Parent.OffsetX;
                return result;
            }
        }

        private class Lines
        {
            public NekkiUILabel Parent;
            public List<Line> Units = new List<Line>();

            public float Height { get { return UnscaledHeight * Units[0].SubUnits[0].Obj.Scale.Y; } }
            public float Width { get { return UnscaledWidth * Units[0].SubUnits[0].Obj.Scale.X; } }
            public float UnscaledWidth
            {
                get
                {
                    float num = 0f;
                    foreach (Line line in Units)
                    {
                        float w = line.UnscaledWidth;
                        if (w > num) num = w;
                    }
                    return num;
                }
            }
            public float UnscaledHeight
            {
                get
                {
                    float num = 0f;
                    foreach (Line line in Units)
                        num += line.UnscaledHeight;
                    return num + (float)(Units.Count - 1) * Parent.Margin.Y;
                }
            }

            public Lines(NekkiUILabel label) { Parent = label; }
            public void NewLine(LineData lineData) { Units.Add(new Line(Parent, lineData)); }
            public void Clear(bool editorUsePool = false) { Units.Clear(); }

            public void LineUp()
            {
                LineUpHorizontalLeft();
            }

            private void LineUpRows()
            {
                float num = 1f;
                for (int i = 0; i < Units.Count; i++)
                {
                    Units[i].Parent = Parent;
                    Units[i].Shrink(num);
                    Units[i].LineUp();
                    if (i > 0)
                        Units[i].MoveVertical(Units[i].Position.Y - Units[i - 1].Position.Y + Units[i - 1].Height / 2f + Units[i].Height / 2f + Parent.Margin.Y * num);
                }
                float num2 = Units[0].Position.Y + Units[0].Height / 2f;
                foreach (Line line in Units)
                    line.MoveVertical(num2 - Height / 2f);
            }

            private void LineUpHorizontalLeft() { LineUpRows(); }
            private void LineUpHorizontalRight() { LineUpRows(); }
            private void LineUpHorizontalCenter() { LineUpRows(); }

            public void Remove(Label subLabel)
            {
                foreach (Line line in Units)
                {
                    if (line.Remove(subLabel)) break;
                }
            }

            public Label InsertLabel()
            {
                Label label = new Label();
                AddChild(label);
                label.Position = Vector3.Zero;
                label.Scale = Vector3.One;
                AddUnit(label);
                return label;
            }

            public NekkiUISprite InsertImage(string sprite, float scale, bool rowSize, ImageData data)
            {
                NekkiUISprite spriteNode = new NekkiUISprite();
                AddChild(spriteNode);
                spriteNode.Position = Vector3.Zero;
                spriteNode.Scale = Vector3.One;
                spriteNode.SpriteName = sprite;
                AddUnit(spriteNode, data);
                return spriteNode;
            }

            private void AddUnit(Label label) { Units[Units.Count - 1].Add(new SubUnit(label)); }
            private void AddUnit(NekkiUISprite sprite, ImageData data) { Units[Units.Count - 1].Add(new SubUnit(sprite, data)); }
        }

        [Serializable]
        public class ImageData
        {
            private int _customOffsetY;
            public NekkiUISprite Sprite;
            public int sectionID;
            public bool UseDefaultImageOffset = true;

            public int CustomOffsetY
            {
                get { return _customOffsetY; }
                set { _customOffsetY = value; UseDefaultImageOffset = false; }
            }
        }

        [Serializable]
        public class LineData
        {
            private int _paddingTop;
            public int PaddingTop { get { return _paddingTop; } set { _paddingTop = value; } }
        }

        private Lines _lines;
        private Vector2 _margin = Vector2.Zero;
        private Types _type = Types.Simple;
        private string _alias;
        private bool _initDone;
        public LocaleImport.LocaleString LocaleString;
        public string[] LastReplacement;
        public List<ImageData> ImagesInfo;
        public int DefaultImageOffsetY;
        public int OffsetX;

        public int MaxSymbols { get; set; }

        public string Alias
        {
            get { return _alias; }
            set
            {
                if (_alias == value) return;
                if (Type == Types.Simple) { GD.PrintErr("cant set alias for a plain text!"); return; }
                _alias = value;
                Clear();
                if (string.IsNullOrEmpty(_alias))
                    Text = string.Empty;
                else if (Localization.Contains(_alias, true))
                {
                    LocaleString = (MaxSymbols != 0) ? Localization.Get(_alias, MaxSymbols, LastReplacement) : Localization.Get(_alias);
                    if (LocaleString.Simple)
                    {
                        Text = LocaleString;
                        if (Text.Contains("\\n"))
                            Text = Text.Replace("\\n", "\n");
                        return;
                    }
                    ImagesInfo = new List<ImageData>();
                    foreach (var section in LocaleString.Sections)
                    {
                        if (section.SectionType == LocaleImport.LocaleString.SectionTypes.Image)
                        {
                            ImageData imageData = new ImageData();
                            imageData.sectionID = section.ID;
                            ImagesInfo.Add(imageData);
                        }
                    }
                    ImagesInfo.Sort((a, b) => a.sectionID.CompareTo(b.sectionID));
                    Text = string.Empty;
                }
                else
                    Text = "Error_" + _alias;
            }
        }

        public Types Type
        {
            get { return _type; }
            set
            {
                if (_type != value)
                {
                    _type = value;
                    switch (_type)
                    {
                        case Types.Localized: Text = string.Empty; break;
                        case Types.Simple: _alias = string.Empty; Clear(); break;
                    }
                }
            }
        }

        public Vector2 Margin
        {
            get { return _margin; }
            set
            {
                if (Vector2.Distance(_margin, value) < 0.01f) return;
                _margin = value;
            }
        }

        public string Text
        {
            get
            {
                var label = GetNode<Label>(".");
                return label != null ? label.Text : string.Empty;
            }
            set
            {
                var label = GetNode<Label>(".");
                if (label != null) label.Text = value;
            }
        }

        public event Action<string, string> OnTextChangeEvent;

        public virtual void Format(params object[] replacement)
        {
            Init(false);
            if (!Localization.Contains(_alias, true))
                Text = "ERROR_" + _alias;
            _lines = new Lines(this);
            _lines.Parent = this;
            _lines.NewLine(new LineData());
            LastReplacement = new string[replacement.Length];
            for (int i = 0; i < LastReplacement.Length; i++)
            {
                if (replacement[i] == null) { GD.PrintErr("Image is not set"); return; }
                LastReplacement[i] = replacement[i].ToString();
            }
            LocaleString = (MaxSymbols != 0) ? Localization.Get(_alias, MaxSymbols, LastReplacement) : Localization.Get(_alias);
            string[] array = LocaleString.CompileWith(replacement, this);
            if (array.Length == 0) return;
            if (LocaleString.Simple || (array.Length == 1 && !LocaleString.ContainsImageRef))
            {
                Text = array[0];
                return;
            }
            Text = string.Empty;
            foreach (var section in LocaleString.Sections)
            {
                if (section.ContainsPosition(-1))
                {
                    if (replacement.Length <= section.ID) { GD.PrintErr("Image name is not set"); return; }
                    ImageData imageDataByID = GetImageDataByID(section.ID);
                    imageDataByID.Sprite = _lines.InsertImage(replacement[section.ID].ToString(), section.GetPositionAtIndex(-1).Scale, section.GetPositionAtIndex(-1).ScaleToRow, imageDataByID);
                    break;
                }
            }
            for (int j = 0; j < array.Length; j++)
            {
                bool hasNewline = array[j].Contains("\n");
                if (hasNewline)
                {
                    string[] sub = array[j].Split('\n');
                    for (int k = 0; k < sub.Length; k++)
                    {
                        Label label = _lines.InsertLabel();
                        label.Text = sub[k];
                        if (k < sub.Length - 1)
                            _lines.NewLine(new LineData());
                    }
                }
                else
                {
                    Label label = _lines.InsertLabel();
                    label.Text = array[j];
                }
                foreach (var section2 in LocaleString.Sections)
                {
                    if (section2.Positions.Count > 0 && section2.ContainsPosition(j))
                    {
                        if (replacement.Length <= section2.ID) { GD.PrintErr("Image name is not set"); return; }
                        ImageData imageDataByID2 = GetImageDataByID(section2.ID);
                        imageDataByID2.Sprite = _lines.InsertImage(replacement[section2.ID].ToString(), section2.GetPositionAtIndex(j).Scale, section2.GetPositionAtIndex(j).ScaleToRow, imageDataByID2);
                    }
                }
            }
            LineUp();
        }

        public void LineUp()
        {
            _lines?.LineUp();
        }

        public int GetLinesWidth()
        {
            return Mathf.CeilToInt(_lines.UnscaledWidth);
        }

        public int GetLinesHeight()
        {
            return Mathf.CeilToInt(_lines.UnscaledHeight);
        }

        protected virtual void Init(bool format) { }
        protected virtual void Clear() { _lines?.Clear(); LocaleString = null; ImagesInfo = null; }

        public ImageData GetImageDataByID(int sectionID)
        {
            if (ImagesInfo == null) return null;
            foreach (ImageData item in ImagesInfo)
            {
                if (item.sectionID == sectionID) return item;
            }
            ImageData imageData = new ImageData();
            imageData.sectionID = sectionID;
            ImagesInfo.Add(imageData);
            return imageData;
        }

        public string AliasProp { get { return _alias; } set { Alias = value; } }
    }

    public interface IHasAlias
    {
        string Alias { get; set; }
    }
}
