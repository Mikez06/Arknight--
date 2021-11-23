/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace MainUI
{
    public partial class UI_TeamSelect : GComponent
    {
        public Controller m_quick;
        public UI_LeftUnitInfo m_leftUnit;
        public GButton m_back;
        public GButton m_home;
        public GList m_Cards;
        public GLabel m_cancel;
        public GComponent m_ok;
        public GLabel m_clear;
        public UI_sortBtn m_level;
        public UI_sortBtn m_rare;
        public UI_sortBtn m_name;
        public UI_sortBtn m_nn;
        public GGroup m_youshang;
        public const string URL = "ui://k4mja8t1r8hrn";

        public static UI_TeamSelect CreateInstance()
        {
            return (UI_TeamSelect)UIPackage.CreateObject("MainUI", "TeamSelect");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_quick = GetControllerAt(0);
            m_leftUnit = (UI_LeftUnitInfo)GetChildAt(1);
            m_back = (GButton)GetChildAt(2);
            m_home = (GButton)GetChildAt(3);
            m_Cards = (GList)GetChildAt(4);
            m_cancel = (GLabel)GetChildAt(5);
            m_ok = (GComponent)GetChildAt(6);
            m_clear = (GLabel)GetChildAt(7);
            m_level = (UI_sortBtn)GetChildAt(9);
            m_rare = (UI_sortBtn)GetChildAt(10);
            m_name = (UI_sortBtn)GetChildAt(11);
            m_nn = (UI_sortBtn)GetChildAt(12);
            m_youshang = (GGroup)GetChildAt(13);
            Init();
        }
        partial void Init();
    }
}