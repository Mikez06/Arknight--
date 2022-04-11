/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace MapBuilderUI
{
    public partial class UI_ContractIcon : GLabel
    {
        public Controller m_state;
        public const string URL = "ui://wof4wytzei155";

        public static UI_ContractIcon CreateInstance()
        {
            return (UI_ContractIcon)UIPackage.CreateObject("MapBuilderUI", "ContractIcon");
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