/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace DungeonUI
{
    public partial class UI_MissionInfo : GComponent
    {
        public Controller m_type;
        public GTextField m_Name;
        public GButton m_go;
        public GButton m_useTower;
        public const string URL = "ui://hgasjns6gbwmv";

        public static UI_MissionInfo CreateInstance()
        {
            return (UI_MissionInfo)UIPackage.CreateObject("DungeonUI", "MissionInfo");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_type = GetControllerAt(0);
            m_Name = (GTextField)GetChildAt(1);
            m_go = (GButton)GetChildAt(2);
            m_useTower = (GButton)GetChildAt(6);
            Init();
        }
        partial void Init();
    }
}