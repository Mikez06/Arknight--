/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace MainUI
{
    public partial class UI_HalfUnit : GComponent
    {
        public Controller m_typeControl;
        public Controller m_seletd;
        public Controller m_star;
        public Controller m_ugrade;
        public GLoader m_halfPic;
        public GTextField m_lv;
        public GTextField m_name;
        public GLoader m_skillIcon;
        public GTextField m_index;
        public const string URL = "ui://k4mja8t1kbte1";

        public static UI_HalfUnit CreateInstance()
        {
            return (UI_HalfUnit)UIPackage.CreateObject("MainUI", "HalfUnit");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_typeControl = GetControllerAt(0);
            m_seletd = GetControllerAt(1);
            m_star = GetControllerAt(2);
            m_ugrade = GetControllerAt(3);
            m_halfPic = (GLoader)GetChildAt(1);
            m_lv = (GTextField)GetChildAt(8);
            m_name = (GTextField)GetChildAt(9);
            m_skillIcon = (GLoader)GetChildAt(10);
            m_index = (GTextField)GetChildAt(12);
            Init();
        }
        partial void Init();
    }
}