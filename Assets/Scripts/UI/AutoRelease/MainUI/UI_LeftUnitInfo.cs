/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace MainUI
{
    public partial class UI_LeftUnitInfo : GComponent
    {
        public Controller m_empty;
        public GTextField m_name;
        public GTextField m_lv;
        public GComponent m_attackArea;
        public GList m_Skills;
        public GTextField m_reset;
        public GTextField m_cost;
        public GTextField m_stop;
        public GTextField m_agi;
        public GTextField m_hp;
        public GTextField m_attack;
        public GTextField m_def;
        public GTextField m_magdefence;
        public const string URL = "ui://k4mja8t1k6t713";

        public static UI_LeftUnitInfo CreateInstance()
        {
            return (UI_LeftUnitInfo)UIPackage.CreateObject("MainUI", "LeftUnitInfo");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_empty = GetControllerAt(0);
            m_name = (GTextField)GetChildAt(5);
            m_lv = (GTextField)GetChildAt(7);
            m_attackArea = (GComponent)GetChildAt(10);
            m_Skills = (GList)GetChildAt(11);
            m_reset = (GTextField)GetChildAt(20);
            m_cost = (GTextField)GetChildAt(21);
            m_stop = (GTextField)GetChildAt(22);
            m_agi = (GTextField)GetChildAt(23);
            m_hp = (GTextField)GetChildAt(24);
            m_attack = (GTextField)GetChildAt(25);
            m_def = (GTextField)GetChildAt(26);
            m_magdefence = (GTextField)GetChildAt(27);
            Init();
        }
        partial void Init();
    }
}