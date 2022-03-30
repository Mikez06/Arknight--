/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace MainUI
{
    public partial class UI_Contract : GComponent
    {
        public GButton m_back;
        public GButton m_home;
        public GTextField m_HisMax;
        public GList m_supportList;
        public GList m_conList;
        public GTextField m_mapName;
        public GList m_choose;
        public GButton m_start;
        public GGraph m_clear;
        public GTextField m_nowLevel;
        public const string URL = "ui://k4mja8t1usve4s";

        public static UI_Contract CreateInstance()
        {
            return (UI_Contract)UIPackage.CreateObject("MainUI", "Contract");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_back = (GButton)GetChildAt(3);
            m_home = (GButton)GetChildAt(4);
            m_HisMax = (GTextField)GetChildAt(7);
            m_supportList = (GList)GetChildAt(10);
            m_conList = (GList)GetChildAt(11);
            m_mapName = (GTextField)GetChildAt(12);
            m_choose = (GList)GetChildAt(13);
            m_start = (GButton)GetChildAt(15);
            m_clear = (GGraph)GetChildAt(16);
            m_nowLevel = (GTextField)GetChildAt(19);
            Init();
        }
        partial void Init();
    }
}