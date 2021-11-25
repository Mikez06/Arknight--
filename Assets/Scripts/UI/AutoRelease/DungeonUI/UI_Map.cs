/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace DungeonUI
{
    public partial class UI_Map : GComponent
    {
        public Controller m_showInfo;
        public GButton m_Team;
        public GComponent m_unselect;
        public UI_MissionInfo m_MissionInfo;
        public GTextField m_gold;
        public GTextField m_hope;
        public const string URL = "ui://hgasjns6t5h94";

        public static UI_Map CreateInstance()
        {
            return (UI_Map)UIPackage.CreateObject("DungeonUI", "Map");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_showInfo = GetControllerAt(0);
            m_Team = (GButton)GetChildAt(0);
            m_unselect = (GComponent)GetChildAt(1);
            m_MissionInfo = (UI_MissionInfo)GetChildAt(2);
            m_gold = (GTextField)GetChildAt(3);
            m_hope = (GTextField)GetChildAt(4);
            Init();
        }
        partial void Init();
    }
}