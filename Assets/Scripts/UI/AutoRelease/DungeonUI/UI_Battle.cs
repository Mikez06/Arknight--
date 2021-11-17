/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace DungeonUI
{
    public partial class UI_Battle : GComponent
    {
        public GTextField m_MapName;
        public GList m_Contracts;
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
        public GComponent m_DoBattle;
        public GList m_Relics;
        public const string URL = "ui://hgasjns6gbwmx";

        public static UI_Battle CreateInstance()
        {
            return (UI_Battle)UIPackage.CreateObject("DungeonUI", "Battle");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_MapName = (GTextField)GetChildAt(2);
            m_Contracts = (GList)GetChildAt(3);
            m_u0 = (UI_TeamUnit)GetChildAt(6);
            m_u1 = (UI_TeamUnit)GetChildAt(7);
            m_u2 = (UI_TeamUnit)GetChildAt(8);
            m_u3 = (UI_TeamUnit)GetChildAt(9);
            m_u4 = (UI_TeamUnit)GetChildAt(10);
            m_u5 = (UI_TeamUnit)GetChildAt(11);
            m_u6 = (UI_TeamUnit)GetChildAt(12);
            m_u7 = (UI_TeamUnit)GetChildAt(13);
            m_u8 = (UI_TeamUnit)GetChildAt(14);
            m_u9 = (UI_TeamUnit)GetChildAt(15);
            m_u10 = (UI_TeamUnit)GetChildAt(16);
            m_u11 = (UI_TeamUnit)GetChildAt(17);
            m_DoBattle = (GComponent)GetChildAt(18);
            m_Relics = (GList)GetChildAt(19);
            Init();
        }
        partial void Init();
    }
}