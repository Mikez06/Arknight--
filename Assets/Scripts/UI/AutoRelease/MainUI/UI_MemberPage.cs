/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace MainUI
{
    public partial class UI_MemberPage : GComponent
    {
        public GList m_Cards;
        public GButton m_back;
        public UI_sortBtn m_level;
        public UI_sortBtn m_rare;
        public UI_sortBtn m_name;
        public const string URL = "ui://k4mja8t1kbteb";

        public static UI_MemberPage CreateInstance()
        {
            return (UI_MemberPage)UIPackage.CreateObject("MainUI", "MemberPage");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_Cards = (GList)GetChildAt(1);
            m_back = (GButton)GetChildAt(2);
            m_level = (UI_sortBtn)GetChildAt(5);
            m_rare = (UI_sortBtn)GetChildAt(6);
            m_name = (UI_sortBtn)GetChildAt(7);
            Init();
        }
        partial void Init();
    }
}