/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace MapBuilderUI
{
    public partial class UI_MidUnitInfo : GComponent
    {
        public GTextField m_name;
        public GLoader m_head;
        public GTextField m_desc;
        public const string URL = "ui://wof4wytzdy0mj";

        public static UI_MidUnitInfo CreateInstance()
        {
            return (UI_MidUnitInfo)UIPackage.CreateObject("MapBuilderUI", "MidUnitInfo");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_name = (GTextField)GetChildAt(0);
            m_head = (GLoader)GetChildAt(1);
            m_desc = (GTextField)GetChildAt(2);
            Init();
        }
        partial void Init();
    }
}