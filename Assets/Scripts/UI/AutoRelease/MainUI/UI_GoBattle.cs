/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace MainUI
{
    public partial class UI_GoBattle : GComponent
    {
        public Controller m_ex;
        public const string URL = "ui://k4mja8t1kbtey";

        public static UI_GoBattle CreateInstance()
        {
            return (UI_GoBattle)UIPackage.CreateObject("MainUI", "GoBattle");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_ex = GetControllerAt(0);
            Init();
        }
        partial void Init();
    }
}