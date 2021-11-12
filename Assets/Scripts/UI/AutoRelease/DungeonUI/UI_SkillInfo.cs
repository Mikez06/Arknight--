/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace DungeonUI
{
    public partial class UI_SkillInfo : GComponent
    {
        public Controller m_seleted;
        public GLoader m_icon;
        public GTextField m_name;
        public GTextField m_desc;
        public const string URL = "ui://hgasjns6t5h9s";

        public static UI_SkillInfo CreateInstance()
        {
            return (UI_SkillInfo)UIPackage.CreateObject("DungeonUI", "SkillInfo");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_seleted = GetControllerAt(0);
            m_icon = (GLoader)GetChildAt(0);
            m_name = (GTextField)GetChildAt(1);
            m_desc = (GTextField)GetChildAt(3);
            Init();
        }
        partial void Init();
    }
}