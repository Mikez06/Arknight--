/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace MainUI
{
    public partial class UI_Main : GComponent
    {
        public GButton m_team;
        public GButton m_member;
        public GLoader m_standPic;
        public GTextField m_Name;
        public GButton m_battle;
        public GButton m_rogue;
        public GButton m_contract;
        public const string URL = "ui://k4mja8t1kbte0";

        public static UI_Main CreateInstance()
        {
            return (UI_Main)UIPackage.CreateObject("MainUI", "Main");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_team = (GButton)GetChildAt(1);
            m_member = (GButton)GetChildAt(2);
            m_standPic = (GLoader)GetChildAt(3);
            m_Name = (GTextField)GetChildAt(5);
            m_battle = (GButton)GetChildAt(6);
            m_rogue = (GButton)GetChildAt(7);
            m_contract = (GButton)GetChildAt(8);
            Init();
        }
        partial void Init();
    }
}