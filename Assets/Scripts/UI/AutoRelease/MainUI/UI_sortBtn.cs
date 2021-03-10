/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace MainUI
{
    public partial class UI_sortBtn : GLabel
    {
        public Controller m_up;
        public const string URL = "ui://k4mja8t1kbtei";

        public static UI_sortBtn CreateInstance()
        {
            return (UI_sortBtn)UIPackage.CreateObject("MainUI", "sortBtn");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_up = GetControllerAt(0);
            Init();
        }
        partial void Init();
    }
}