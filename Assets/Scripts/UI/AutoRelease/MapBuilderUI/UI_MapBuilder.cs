/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace MapBuilderUI
{
    public partial class UI_MapBuilder : GComponent
    {
        public Controller m_state;
        public Controller m_sure2;
        public UI_MapInfo m_StartPage;
        public GButton m_DoPath;
        public GButton m_DoUnit;
        public GButton m_DoMidUnit;
        public UI_MidPage m_MidPage;
        public UI_WavePage m_WavePage;
        public GButton m_upgrid;
        public GButton m_downgrid;
        public GButton m_moveable;
        public GButton m_unmoveable;
        public UI_PathPage m_PathPage;
        public GButton m_exit;
        public GButton m_back;
        public GButton m_save;
        public GTextInput m_X;
        public GTextInput m_Y;
        public GTextInput m_Z;
        public GTextField m_mapName2;
        public GButton m_yes;
        public GButton m_no;
        public Transition m_saveSuccess;
        public const string URL = "ui://wof4wytzei150";

        public static UI_MapBuilder CreateInstance()
        {
            return (UI_MapBuilder)UIPackage.CreateObject("MapBuilderUI", "MapBuilder");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_state = GetControllerAt(0);
            m_sure2 = GetControllerAt(1);
            m_StartPage = (UI_MapInfo)GetChildAt(1);
            m_DoPath = (GButton)GetChildAt(2);
            m_DoUnit = (GButton)GetChildAt(3);
            m_DoMidUnit = (GButton)GetChildAt(4);
            m_MidPage = (UI_MidPage)GetChildAt(5);
            m_WavePage = (UI_WavePage)GetChildAt(6);
            m_upgrid = (GButton)GetChildAt(7);
            m_downgrid = (GButton)GetChildAt(8);
            m_moveable = (GButton)GetChildAt(9);
            m_unmoveable = (GButton)GetChildAt(10);
            m_PathPage = (UI_PathPage)GetChildAt(11);
            m_exit = (GButton)GetChildAt(12);
            m_back = (GButton)GetChildAt(13);
            m_save = (GButton)GetChildAt(14);
            m_X = (GTextInput)GetChildAt(17);
            m_Y = (GTextInput)GetChildAt(20);
            m_Z = (GTextInput)GetChildAt(23);
            m_mapName2 = (GTextField)GetChildAt(31);
            m_yes = (GButton)GetChildAt(33);
            m_no = (GButton)GetChildAt(34);
            m_saveSuccess = GetTransitionAt(0);
            Init();
        }
        partial void Init();
    }
}