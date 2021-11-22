/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace MainUI
{
    public partial class UI_Pro : GComponent
    {
        public Controller m_p;
        public GLoader m_pro;
        public const string URL = "ui://k4mja8t1voep3f";

        public static UI_Pro CreateInstance()
        {
            return (UI_Pro)UIPackage.CreateObject("MainUI", "Pro");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_p = GetControllerAt(0);
            m_pro = (GLoader)GetChildAt(1);
            Init();
        }
        partial void Init();
    }
}