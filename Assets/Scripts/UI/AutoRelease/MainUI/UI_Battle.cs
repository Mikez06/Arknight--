/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace MainUI
{
    public partial class UI_Battle : GComponent
    {
        public Controller m_showLevelInfo;
        public Controller m_contractChoose;
        public UI_World1 m_world;
        public GButton m_back;
        public GButton m_home;
        public UI_LevelInfo m_levelInfo;
        public GGraph m_contractBack;
        public GList m_contracts;
        public const string URL = "ui://k4mja8t1kbtew";

        public static UI_Battle CreateInstance()
        {
            return (UI_Battle)UIPackage.CreateObject("MainUI", "Battle");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_showLevelInfo = GetControllerAt(0);
            m_contractChoose = GetControllerAt(1);
            m_world = (UI_World1)GetChildAt(1);
            m_back = (GButton)GetChildAt(2);
            m_home = (GButton)GetChildAt(3);
            m_levelInfo = (UI_LevelInfo)GetChildAt(4);
            m_contractBack = (GGraph)GetChildAt(5);
            m_contracts = (GList)GetChildAt(7);
            Init();
        }
        partial void Init();
    }
}