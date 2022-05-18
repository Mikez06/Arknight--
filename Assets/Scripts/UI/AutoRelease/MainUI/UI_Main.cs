/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace MainUI
{
    public partial class UI_Main : GComponent
    {
        public Controller m_settingC;
        public GButton m_team;
        public GButton m_member;
        public GLoader m_standPic;
        public GTextInput m_Name;
        public GButton m_battle;
        public GButton m_rogue;
        public GButton m_Map;
        public GSlider m_bgm;
        public GButton m_close;
        public GButton m_Setting;
        public GTextField m_Version;
        public const string URL = "ui://k4mja8t1kbte0";

        public static UI_Main CreateInstance()
        {
            return (UI_Main)UIPackage.CreateObject("MainUI", "Main");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_settingC = GetControllerAt(0);
            m_team = (GButton)GetChildAt(1);
            m_member = (GButton)GetChildAt(2);
            m_standPic = (GLoader)GetChildAt(3);
            m_Name = (GTextInput)GetChildAt(5);
            m_battle = (GButton)GetChildAt(6);
            m_rogue = (GButton)GetChildAt(7);
            m_Map = (GButton)GetChildAt(8);
            m_bgm = (GSlider)GetChildAt(11);
            m_close = (GButton)GetChildAt(13);
            m_Setting = (GButton)GetChildAt(15);
            m_Version = (GTextField)GetChildAt(17);
            Init();
        }
        partial void Init();
    }
}