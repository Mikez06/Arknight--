/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace MainUI
{
    public partial class UI_ContractIcon : GLabel
    {
        public Controller m_state;
        public const string URL = "ui://k4mja8t1usve4u";

        public static UI_ContractIcon CreateInstance()
        {
            return (UI_ContractIcon)UIPackage.CreateObject("MainUI", "ContractIcon");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_state = GetControllerAt(0);
            Init();
        }
        partial void Init();
    }
}