/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace MapBuilderUI
{
    public partial class UI_MidPage : GComponent
    {
        public Controller m_selectEnemy;
        public Controller m_Hide;
        public GList m_unitList;
        public GButton m_AddWave;
        public GButton m_DeleteWave;
        public GGraph m_selectBack;
        public GList m_filterList;
        public GTextInput m_filterName;
        public GButton m_Hide_2;
        public const string URL = "ui://wof4wytzdy0mh";

        public static UI_MidPage CreateInstance()
        {
            return (UI_MidPage)UIPackage.CreateObject("MapBuilderUI", "MidPage");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_selectEnemy = GetControllerAt(0);
            m_Hide = GetControllerAt(1);
            m_unitList = (GList)GetChildAt(1);
            m_AddWave = (GButton)GetChildAt(2);
            m_DeleteWave = (GButton)GetChildAt(3);
            m_selectBack = (GGraph)GetChildAt(4);
            m_filterList = (GList)GetChildAt(6);
            m_filterName = (GTextInput)GetChildAt(8);
            m_Hide_2 = (GButton)GetChildAt(11);
            Init();
        }
        partial void Init();
    }
}