/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace MainUI
{
    public partial class UI_HalfUnit : GComponent
    {
        public Controller m_typeControl;
        public GLoader m_halfPic;
        public GList m_stars;
        public GTextField m_lv;
        public GTextField m_name;
        public GLoader m_skillIcon;
        public const string URL = "ui://k4mja8t1kbte1";

        public static UI_HalfUnit CreateInstance()
        {
            return (UI_HalfUnit)UIPackage.CreateObject("MainUI", "HalfUnit");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_typeControl = GetControllerAt(0);
            m_halfPic = (GLoader)GetChildAt(0);
            m_stars = (GList)GetChildAt(5);
            m_lv = (GTextField)GetChildAt(8);
            m_name = (GTextField)GetChildAt(9);
            m_skillIcon = (GLoader)GetChildAt(10);
            Init();
        }
        partial void Init();
    }
}