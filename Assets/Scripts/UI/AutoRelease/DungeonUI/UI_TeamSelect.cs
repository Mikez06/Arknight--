/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace DungeonUI
{
    public partial class UI_TeamSelect : GComponent
    {
        public Controller m_quick;
        public UI_LeftUnitInfo m_leftUnit;
        public GList m_units;
        public GButton m_cancel;
        public GButton m_ok;
        public GButton m_back;
        public const string URL = "ui://hgasjns6t5h9b";

        public static UI_TeamSelect CreateInstance()
        {
            return (UI_TeamSelect)UIPackage.CreateObject("DungeonUI", "TeamSelect");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_quick = GetControllerAt(0);
            m_leftUnit = (UI_LeftUnitInfo)GetChildAt(0);
            m_units = (GList)GetChildAt(1);
            m_cancel = (GButton)GetChildAt(2);
            m_ok = (GButton)GetChildAt(3);
            m_back = (GButton)GetChildAt(4);
            Init();
        }
        partial void Init();
    }
}