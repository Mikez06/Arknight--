/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace MainUI
{
    public partial class UI_SkillInfo : GComponent
    {
        public Controller m_time;
        public Controller m_selected;
        public UI_UnitSkillIcon m_skillIcon;
        public UI_Recover m_r;
        public UI_UseType m_UseType;
        public GTextField m_lastTime;
        public GTextField m_desc;
        public const string URL = "ui://k4mja8t1voep3q";

        public static UI_SkillInfo CreateInstance()
        {
            return (UI_SkillInfo)UIPackage.CreateObject("MainUI", "SkillInfo");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_time = GetControllerAt(0);
            m_selected = GetControllerAt(1);
            m_skillIcon = (UI_UnitSkillIcon)GetChildAt(0);
            m_r = (UI_Recover)GetChildAt(1);
            m_UseType = (UI_UseType)GetChildAt(2);
            m_lastTime = (GTextField)GetChildAt(4);
            m_desc = (GTextField)GetChildAt(5);
            Init();
        }
        partial void Init();
    }
}