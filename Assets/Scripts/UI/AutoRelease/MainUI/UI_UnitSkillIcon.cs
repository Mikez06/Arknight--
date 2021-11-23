/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace MainUI
{
    public partial class UI_UnitSkillIcon : GComponent
    {
        public Controller m_level;
        public Controller m_default;
        public Controller m_start;
        public Controller m_style;
        public GLoader m_icon;
        public GTextField m_cost;
        public GTextField m_startTime;
        public GTextField m_skillName;
        public const string URL = "ui://k4mja8t1voep3b";

        public static UI_UnitSkillIcon CreateInstance()
        {
            return (UI_UnitSkillIcon)UIPackage.CreateObject("MainUI", "UnitSkillIcon");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_level = GetControllerAt(0);
            m_default = GetControllerAt(1);
            m_start = GetControllerAt(2);
            m_style = GetControllerAt(3);
            m_icon = (GLoader)GetChildAt(1);
            m_cost = (GTextField)GetChildAt(4);
            m_startTime = (GTextField)GetChildAt(6);
            m_skillName = (GTextField)GetChildAt(7);
            Init();
        }
        partial void Init();
    }
}