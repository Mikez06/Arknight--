/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace DungeonUI
{
    public partial class UI_RecruitUnit : GComponent
    {
        public Controller m_select;
        public Controller m_upgrade;
        public Controller m_ifUpgrade;
        public Controller m_temp;
        public GLoader m_Head;
        public GTextField m_Name;
        public GTextField m_level;
        public GTextField m_Cost;
        public const string URL = "ui://hgasjns6gbwm16";

        public static UI_RecruitUnit CreateInstance()
        {
            return (UI_RecruitUnit)UIPackage.CreateObject("DungeonUI", "RecruitUnit");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_select = GetControllerAt(0);
            m_upgrade = GetControllerAt(1);
            m_ifUpgrade = GetControllerAt(2);
            m_temp = GetControllerAt(3);
            m_Head = (GLoader)GetChildAt(1);
            m_Name = (GTextField)GetChildAt(2);
            m_level = (GTextField)GetChildAt(3);
            m_Cost = (GTextField)GetChildAt(6);
            Init();
        }
        partial void Init();
    }
}