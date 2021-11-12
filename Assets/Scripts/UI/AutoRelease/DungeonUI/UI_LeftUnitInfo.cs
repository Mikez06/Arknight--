/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace DungeonUI
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
        public const string URL = "ui://hgasjns6t5h9q";

        public static UI_LeftUnitInfo CreateInstance()
        {
            return (UI_LeftUnitInfo)UIPackage.CreateObject("DungeonUI", "LeftUnitInfo");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_empty = GetControllerAt(0);
            m_name = (GTextField)GetChildAt(1);
            m_lv = (GTextField)GetChildAt(3);
            m_attackArea = (GComponent)GetChildAt(9);
            m_Skills = (GList)GetChildAt(10);
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