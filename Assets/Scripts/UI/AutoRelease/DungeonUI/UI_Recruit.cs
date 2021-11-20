/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace DungeonUI
{
    public partial class UI_Recruit : GComponent
    {
        public GList m_units;
        public GButton m_cancel;
        public GButton m_ok;
        public UI_LeftUnitInfo m_leftUnit;
        public const string URL = "ui://hgasjns6gbwm15";

        public static UI_Recruit CreateInstance()
        {
            return (UI_Recruit)UIPackage.CreateObject("DungeonUI", "Recruit");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_units = (GList)GetChildAt(1);
            m_cancel = (GButton)GetChildAt(2);
            m_ok = (GButton)GetChildAt(3);
            m_leftUnit = (UI_LeftUnitInfo)GetChildAt(4);
            Init();
        }
        partial void Init();
    }
}