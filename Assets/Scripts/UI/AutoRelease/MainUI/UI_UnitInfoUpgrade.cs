/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace MainUI
{
    public partial class UI_UnitInfoUpgrade : GButton
    {
        public Controller m_upgrade;
        public const string URL = "ui://k4mja8t1voep2y";

        public static UI_UnitInfoUpgrade CreateInstance()
        {
            return (UI_UnitInfoUpgrade)UIPackage.CreateObject("MainUI", "UnitInfoUpgrade");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_upgrade = GetControllerAt(0);
            Init();
        }
        partial void Init();
    }
}