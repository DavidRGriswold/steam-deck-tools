using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.Security.RightsManagement;

namespace SteamController.ProfilesSettings
{
    [Category("Shortcuts")]
    internal abstract class BackPanelSettings : CommonHelpers.BaseSettings
    {
        private const String MappingsDescription = @"Only some of those keys do work. Allowed shortcuts are to be changed in future release.";

        private ModifierList l4mods;
        private ModifierList r4mods;
        private ModifierList l5mods;
        private ModifierList r5mods;
        public void updateAll()
        {
            Set("L4_KEY_MODIFIERS", l4mods);
        }
        public BackPanelSettings(String settingsKey) : base(settingsKey)
        {
            l4mods = new ModifierList(this);
        }

        [Description(MappingsDescription)]
        public VirtualKeyCode L4_KEY
        {
            get { return Get<VirtualKeyCode>("L4_KEY", VirtualKeyCode.None); }
            set { Set("L4_KEY", value); }
        }
        [Description("Which modifiers should be pressed with the L4 Key?")]
        public ModifierList L4_KEY_MODIFIERS
        {
            get
            {
                ModifierList ml = Get<ModifierList>("L4_KEY_MODIFIERS", l4mods);
                ml.boss = this; return ml;
            }
            set { 
                l4mods = value; 
                Set("L4_KEY_MODIFIERS", value); }
        }

        [Description(MappingsDescription)]
        public VirtualKeyCode L5_KEY
        {
            get { return Get<VirtualKeyCode>("L5_KEY", VirtualKeyCode.None); }
            set { Set("L5_KEY", value); }
        }

        [Description(MappingsDescription)]
        public VirtualKeyCode R4_KEY
        {
            get { return Get<VirtualKeyCode>("R4_KEY", VirtualKeyCode.None); }
            set { Set("R4_KEY", value); }
        }

        [Description(MappingsDescription)]
        public VirtualKeyCode R5_KEY
        {
            get { return Get<VirtualKeyCode>("R5_KEY", VirtualKeyCode.None); }
            set { Set("R5_KEY", value); }
        }


        [TypeConverter(typeof(ExpandableObjectConverter))]
        public class ModifierList
        {
            [Browsable(false)]
            public BackPanelSettings? boss { get; set; }

            public ModifierList()
            {

            }
            public ModifierList(BackPanelSettings b)
            {
                this.boss = b;
            }
            private bool _win;
            public bool Win
            {
                get { return _win; }
                set
                {
                    _win = value;
                    if (boss != null)
                        boss.updateAll();
                }
            }
            public bool Ctrl { get; set; }
            public bool Alt { get; set; }
            public bool Shift { get; set; }



            public override string ToString()
            {
                string output = "";
                if (Ctrl) output += "Ctrl + ";
                if (Shift) output += "Shift + ";
                if (Alt) output += "Alt + ";
                if (Win) output += "Win + ";
                if (output.Equals("")) return "None";
                return output.Substring(0, output.Length - 3);
            }
        }

        public class ModifierListConverter : ExpandableObjectConverter
        {
            public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
            {
                if (sourceType == typeof(string)) return true;
                return base.CanConvertFrom(context, sourceType);
            }

            public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
            {
                ModifierList output = new ModifierList();
                if (value is string)
                {
                    var s = value as string;
                    if (s.Contains("Alt")) output.Alt = true;
                    if (s.Contains("Win")) output.Win = true;
                    if (s.Contains("Ctrl")) output.Ctrl = true;
                    if (s.Contains("Shift")) output.Shift = true;

                }
                return base.ConvertFrom(context, culture, value);
            }

        }


    }

}
