/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace MainUI
{
    public partial class UI_Team : GComponent
    {
        public Controller m_goBattle;
        public UI_TeamUnit m_u0;
        public UI_TeamUnit m_u1;
        public UI_TeamUnit m_u2;
        public UI_TeamUnit m_u3;
        public UI_TeamUnit m_u4;
        public UI_TeamUnit m_u5;
        public UI_TeamUnit m_u6;
        public UI_TeamUnit m_u7;
        public UI_TeamUnit m_u8;
        public UI_TeamUnit m_u9;
        public UI_TeamUnit m_u10;
        public UI_TeamUnit m_u11;
        public GButton m_back;
        public GButton m_team0;
        public GButton m_team1;
        public GButton m_team2;
        public GButton m_team3;
        public GComponent m_quickTeam;
        public GComponent m_delete;
        public GButton m_right;
        public GButton m_left;
        public GComponent m_support;
        public UI_GoBattle m_battle;
        public const string URL = "ui://k4mja8t1r8hrm";

        public static UI_Team CreateInstance()
        {
            return (UI_Team)UIPackage.CreateObject("MainUI", "Team");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_goBattle = GetControllerAt(0);
            m_u0 = (UI_TeamUnit)GetChildAt(1);
            m_u1 = (UI_TeamUnit)GetChildAt(2);
            m_u2 = (UI_TeamUnit)GetChildAt(3);
            m_u3 = (UI_TeamUnit)GetChildAt(4);
            m_u4 = (UI_TeamUnit)GetChildAt(5);
            m_u5 = (UI_TeamUnit)GetChildAt(6);
            m_u6 = (UI_TeamUnit)GetChildAt(7);
            m_u7 = (UI_TeamUnit)GetChildAt(8);
            m_u8 = (UI_TeamUnit)GetChildAt(9);
            m_u9 = (UI_TeamUnit)GetChildAt(10);
            m_u10 = (UI_TeamUnit)GetChildAt(11);
            m_u11 = (UI_TeamUnit)GetChildAt(12);
            m_back = (GButton)GetChildAt(13);
            m_team0 = (GButton)GetChildAt(14);
            m_team1 = (GButton)GetChildAt(15);
            m_team2 = (GButton)GetChildAt(16);
            m_team3 = (GButton)GetChildAt(17);
            m_quickTeam = (GComponent)GetChildAt(18);
            m_delete = (GComponent)GetChildAt(19);
            m_right = (GButton)GetChildAt(20);
            m_left = (GButton)GetChildAt(21);
            m_support = (GComponent)GetChildAt(22);
            m_battle = (UI_GoBattle)GetChildAt(23);
            Init();
        }
        partial void Init();
    }
}