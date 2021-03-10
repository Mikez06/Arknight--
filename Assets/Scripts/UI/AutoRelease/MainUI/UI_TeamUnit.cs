/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace MainUI
{
    public partial class UI_TeamUnit : GComponent
    {
        public Controller m_empty;
        public UI_HalfUnit m_Unit;
        public const string URL = "ui://k4mja8t1r8hro";

        public static UI_TeamUnit CreateInstance()
        {
            return (UI_TeamUnit)UIPackage.CreateObject("MainUI", "TeamUnit");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_empty = GetControllerAt(0);
            m_Unit = (UI_HalfUnit)GetChildAt(0);
            Init();
        }
        partial void Init();
    }
}