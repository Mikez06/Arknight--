/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace MapBuilderUI
{
    public partial class UI_WavePage : GComponent
    {
        public Controller m_selectEnemy;
        public Controller m_Hide;
        public GList m_wavwList;
        public GButton m_AddWave;
        public GButton m_DeleteWave;
        public GButton m_CopyWave;
        public GGraph m_selectBack;
        public GList m_filterList;
        public GTextInput m_filterName;
        public GButton m_Hide_2;
        public const string URL = "ui://wof4wytzq2unc";

        public static UI_WavePage CreateInstance()
        {
            return (UI_WavePage)UIPackage.CreateObject("MapBuilderUI", "WavePage");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_selectEnemy = GetControllerAt(0);
            m_Hide = GetControllerAt(1);
            m_wavwList = (GList)GetChildAt(1);
            m_AddWave = (GButton)GetChildAt(2);
            m_DeleteWave = (GButton)GetChildAt(3);
            m_CopyWave = (GButton)GetChildAt(4);
            m_selectBack = (GGraph)GetChildAt(5);
            m_filterList = (GList)GetChildAt(7);
            m_filterName = (GTextInput)GetChildAt(9);
            m_Hide_2 = (GButton)GetChildAt(12);
            Init();
        }
        partial void Init();
    }
}