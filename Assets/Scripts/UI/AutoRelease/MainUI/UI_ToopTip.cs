/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace MainUI
{
    public partial class UI_ToopTip : GComponent
    {
        public GTextField m_title;
        public const string URL = "ui://k4mja8t1kbtez";

        public static UI_ToopTip CreateInstance()
        {
            return (UI_ToopTip)UIPackage.CreateObject("MainUI", "ToopTip");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_title = (GTextField)GetChildAt(1);
            Init();
        }
        partial void Init();
    }
}