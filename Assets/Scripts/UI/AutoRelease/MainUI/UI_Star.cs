/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace MainUI
{
    public partial class UI_Star : GComponent
    {
        public Controller m_full;
        public Controller m_ex;
        public const string URL = "ui://k4mja8t1voep4p";

        public static UI_Star CreateInstance()
        {
            return (UI_Star)UIPackage.CreateObject("MainUI", "Star");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_full = GetControllerAt(0);
            m_ex = GetControllerAt(1);
            Init();
        }
        partial void Init();
    }
}