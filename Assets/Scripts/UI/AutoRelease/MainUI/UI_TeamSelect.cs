/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace MainUI
{
    public partial class UI_TeamSelect : GComponent
    {
        public Controller m_quick;
        public GButton m_back;
        public GList m_Cards;
        public UI_sortBtn m_level;
        public UI_sortBtn m_rare;
        public UI_sortBtn m_name;
        public UI_sortBtn m_nn;
        public GLabel m_cancel;
        public GComponent m_ok;
        public GLabel m_clear;
        public const string URL = "ui://k4mja8t1r8hrn";

        public static UI_TeamSelect CreateInstance()
        {
            return (UI_TeamSelect)UIPackage.CreateObject("MainUI", "TeamSelect");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_quick = GetControllerAt(0);
            m_back = (GButton)GetChildAt(2);
            m_Cards = (GList)GetChildAt(3);
            m_level = (UI_sortBtn)GetChildAt(6);
            m_rare = (UI_sortBtn)GetChildAt(7);
            m_name = (UI_sortBtn)GetChildAt(8);
            m_nn = (UI_sortBtn)GetChildAt(9);
            m_cancel = (GLabel)GetChildAt(10);
            m_ok = (GComponent)GetChildAt(11);
            m_clear = (GLabel)GetChildAt(12);
            Init();
        }
        partial void Init();
    }
}