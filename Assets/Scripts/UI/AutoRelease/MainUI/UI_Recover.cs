/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace MainUI
{
    public partial class UI_Recover : GComponent
    {
        public Controller m_recover;
        public GLoader m_r;
        public const string URL = "ui://k4mja8t1voep4q";

        public static UI_Recover CreateInstance()
        {
            return (UI_Recover)UIPackage.CreateObject("MainUI", "Recover");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_recover = GetControllerAt(0);
            m_r = (GLoader)GetChildAt(0);
            Init();
        }
        partial void Init();
    }
}