/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace MainUI
{
    public partial class UI_UseType : GComponent
    {
        public Controller m_useType;
        public const string URL = "ui://k4mja8t1voep4r";

        public static UI_UseType CreateInstance()
        {
            return (UI_UseType)UIPackage.CreateObject("MainUI", "UseType");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_useType = GetControllerAt(0);
            Init();
        }
        partial void Init();
    }
}