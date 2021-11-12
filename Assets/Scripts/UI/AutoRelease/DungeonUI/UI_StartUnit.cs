/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace DungeonUI
{
    public partial class UI_StartUnit : GLabel
    {
        public Controller m_selected;
        public const string URL = "ui://hgasjns6t5h92";

        public static UI_StartUnit CreateInstance()
        {
            return (UI_StartUnit)UIPackage.CreateObject("DungeonUI", "StartUnit");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_selected = GetControllerAt(0);
            Init();
        }
        partial void Init();
    }
}