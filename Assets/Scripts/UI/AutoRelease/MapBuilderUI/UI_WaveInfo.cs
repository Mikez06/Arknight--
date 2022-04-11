/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace MapBuilderUI
{
    public partial class UI_WaveInfo : GButton
    {
        public GGraph m_headback;
        public GTextField m_name;
        public GTextInput m_delay;
        public GTextInput m_count;
        public GTextInput m_gap;
        public GLoader m_head;
        public GTextInput m_x;
        public GTextInput m_y;
        public GComboBox m_path;
        public const string URL = "ui://wof4wytzq2unb";

        public static UI_WaveInfo CreateInstance()
        {
            return (UI_WaveInfo)UIPackage.CreateObject("MapBuilderUI", "WaveInfo");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_headback = (GGraph)GetChildAt(5);
            m_name = (GTextField)GetChildAt(6);
            m_delay = (GTextInput)GetChildAt(8);
            m_count = (GTextInput)GetChildAt(10);
            m_gap = (GTextInput)GetChildAt(12);
            m_head = (GLoader)GetChildAt(14);
            m_x = (GTextInput)GetChildAt(17);
            m_y = (GTextInput)GetChildAt(18);
            m_path = (GComboBox)GetChildAt(20);
            Init();
        }
        partial void Init();
    }
}