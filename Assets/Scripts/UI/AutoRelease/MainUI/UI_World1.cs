/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace MainUI
{
    public partial class UI_World1 : GComponent
    {
        public GComponent m_levelBack;
        public GComponent m_rrr;
        public const string URL = "ui://k4mja8t1kbte12";

        public static UI_World1 CreateInstance()
        {
            return (UI_World1)UIPackage.CreateObject("MainUI", "World1");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_levelBack = (GComponent)GetChildAt(0);
            m_rrr = (GComponent)GetChildAt(3);
            Init();
        }
        partial void Init();
    }
}