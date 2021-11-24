/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace MainUI
{
    public partial class UI_LevelInfo : GComponent
    {
        public GButton m_start;
        public GButton m_Train;
        public GTextField m_Desc;
        public GTextField m_level;
        public GTextField m_id;
        public GTextField m_name;
        public UI_Star m_star0;
        public UI_Star m_star1;
        public UI_Star m_star2;
        public UI_Star m_star3;
        public const string URL = "ui://k4mja8t1voep49";

        public static UI_LevelInfo CreateInstance()
        {
            return (UI_LevelInfo)UIPackage.CreateObject("MainUI", "LevelInfo");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_start = (GButton)GetChildAt(5);
            m_Train = (GButton)GetChildAt(6);
            m_Desc = (GTextField)GetChildAt(8);
            m_level = (GTextField)GetChildAt(9);
            m_id = (GTextField)GetChildAt(10);
            m_name = (GTextField)GetChildAt(11);
            m_star0 = (UI_Star)GetChildAt(14);
            m_star1 = (UI_Star)GetChildAt(15);
            m_star2 = (UI_Star)GetChildAt(16);
            m_star3 = (UI_Star)GetChildAt(17);
            Init();
        }
        partial void Init();
    }
}