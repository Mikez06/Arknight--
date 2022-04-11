/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace MapBuilderUI
{
    public partial class UI_MapInfo : GComponent
    {
        public GTextInput m_MapName;
        public GTextInput m_MapDesc;
        public GTextInput m_SceneName;
        public GTextInput m_InitHp;
        public GTextInput m_InitCost;
        public GTextInput m_BuildCount;
        public GTextInput m_MaxCost;
        public GTextInput m_width;
        public GTextInput m_height;
        public GList m_Contract;
        public GGraph m_next;
        public GButton m_load;
        public GTextInput m_FileName;
        public const string URL = "ui://wof4wytzei151";

        public static UI_MapInfo CreateInstance()
        {
            return (UI_MapInfo)UIPackage.CreateObject("MapBuilderUI", "MapInfo");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_MapName = (GTextInput)GetChildAt(2);
            m_MapDesc = (GTextInput)GetChildAt(5);
            m_SceneName = (GTextInput)GetChildAt(8);
            m_InitHp = (GTextInput)GetChildAt(11);
            m_InitCost = (GTextInput)GetChildAt(14);
            m_BuildCount = (GTextInput)GetChildAt(17);
            m_MaxCost = (GTextInput)GetChildAt(20);
            m_width = (GTextInput)GetChildAt(25);
            m_height = (GTextInput)GetChildAt(28);
            m_Contract = (GList)GetChildAt(31);
            m_next = (GGraph)GetChildAt(32);
            m_load = (GButton)GetChildAt(35);
            m_FileName = (GTextInput)GetChildAt(38);
            Init();
        }
        partial void Init();
    }
}