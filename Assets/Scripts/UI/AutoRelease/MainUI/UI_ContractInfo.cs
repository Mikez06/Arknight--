/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace MainUI
{
    public partial class UI_ContractInfo : GComponent
    {
        public Controller m_level;
        public GTextField m_info;
        public const string URL = "ui://k4mja8t1usve4w";

        public static UI_ContractInfo CreateInstance()
        {
            return (UI_ContractInfo)UIPackage.CreateObject("MainUI", "ContractInfo");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_level = GetControllerAt(0);
            m_info = (GTextField)GetChildAt(0);
            Init();
        }
        partial void Init();
    }
}