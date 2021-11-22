/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace MainUI
{
    public partial class UI_UnitInfo : GComponent
    {
        public Controller m_star;
        public GLoader m_standPic;
        public UI_UnitInfoUpgrade m_upgrade;
        public GTextField m_hp;
        public GTextField m_atk;
        public GTextField m_def;
        public GTextField m_magDef;
        public GTextField m_rebuild;
        public GTextField m_cost;
        public GTextField m_stop;
        public GTextField m_agi;
        public GList m_abs;
        public GComponent m_attackArea;
        public UI_Pro m_Pro;
        public GTextField m_rank;
        public GTextField m_place;
        public GTextField m_tags;
        public GTextField m_LV;
        public GList m_Skills;
        public GButton m_back;
        public GButton m_home;
        public GTextField m_engName;
        public GTextField m_Name;
        public const string URL = "ui://k4mja8t1voep24";

        public static UI_UnitInfo CreateInstance()
        {
            return (UI_UnitInfo)UIPackage.CreateObject("MainUI", "UnitInfo");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_star = GetControllerAt(0);
            m_standPic = (GLoader)GetChildAt(1);
            m_upgrade = (UI_UnitInfoUpgrade)GetChildAt(7);
            m_hp = (GTextField)GetChildAt(10);
            m_atk = (GTextField)GetChildAt(11);
            m_def = (GTextField)GetChildAt(12);
            m_magDef = (GTextField)GetChildAt(13);
            m_rebuild = (GTextField)GetChildAt(14);
            m_cost = (GTextField)GetChildAt(15);
            m_stop = (GTextField)GetChildAt(16);
            m_agi = (GTextField)GetChildAt(17);
            m_abs = (GList)GetChildAt(18);
            m_attackArea = (GComponent)GetChildAt(20);
            m_Pro = (UI_Pro)GetChildAt(22);
            m_rank = (GTextField)GetChildAt(23);
            m_place = (GTextField)GetChildAt(31);
            m_tags = (GTextField)GetChildAt(32);
            m_LV = (GTextField)GetChildAt(34);
            m_Skills = (GList)GetChildAt(35);
            m_back = (GButton)GetChildAt(36);
            m_home = (GButton)GetChildAt(37);
            m_engName = (GTextField)GetChildAt(38);
            m_Name = (GTextField)GetChildAt(39);
            Init();
        }
        partial void Init();
    }
}