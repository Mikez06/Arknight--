/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace DungeonUI
{
    public partial class UI_TeamSkill : GComponent
    {
        public Controller m_select;
        public GLoader m_icon;
        public const string URL = "ui://hgasjns6t5h97";

        public static UI_TeamSkill CreateInstance()
        {
            return (UI_TeamSkill)UIPackage.CreateObject("DungeonUI", "TeamSkill");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_select = GetControllerAt(0);
            m_icon = (GLoader)GetChildAt(0);
            Init();
        }
        partial void Init();
    }
}