/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace DungeonUI
{
    public partial class UI_DungeonTeam : GComponent
    {
        public UI_TeamUnit m_U0;
        public UI_TeamUnit m_U1;
        public UI_TeamUnit m_U2;
        public UI_TeamUnit m_U3;
        public UI_TeamUnit m_U4;
        public UI_TeamUnit m_U5;
        public UI_TeamUnit m_U6;
        public UI_TeamUnit m_U7;
        public UI_TeamUnit m_U8;
        public UI_TeamUnit m_U9;
        public UI_TeamUnit m_U10;
        public UI_TeamUnit m_U11;
        public GButton m_back;
        public const string URL = "ui://hgasjns6t5h96";

        public static UI_DungeonTeam CreateInstance()
        {
            return (UI_DungeonTeam)UIPackage.CreateObject("DungeonUI", "DungeonTeam");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_U0 = (UI_TeamUnit)GetChildAt(1);
            m_U1 = (UI_TeamUnit)GetChildAt(2);
            m_U2 = (UI_TeamUnit)GetChildAt(3);
            m_U3 = (UI_TeamUnit)GetChildAt(4);
            m_U4 = (UI_TeamUnit)GetChildAt(5);
            m_U5 = (UI_TeamUnit)GetChildAt(6);
            m_U6 = (UI_TeamUnit)GetChildAt(7);
            m_U7 = (UI_TeamUnit)GetChildAt(8);
            m_U8 = (UI_TeamUnit)GetChildAt(9);
            m_U9 = (UI_TeamUnit)GetChildAt(10);
            m_U10 = (UI_TeamUnit)GetChildAt(11);
            m_U11 = (UI_TeamUnit)GetChildAt(12);
            m_back = (GButton)GetChildAt(13);
            Init();
        }
        partial void Init();
    }
}