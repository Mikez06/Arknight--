/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace DungeonUI
{
    public partial class UI_Map : GComponent
    {
        public GButton m_Team;
        public const string URL = "ui://hgasjns6t5h94";

        public static UI_Map CreateInstance()
        {
            return (UI_Map)UIPackage.CreateObject("DungeonUI", "Map");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_Team = (GButton)GetChildAt(0);
            Init();
        }
        partial void Init();
    }
}