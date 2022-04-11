/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace MapBuilderUI
{
    public partial class UI_MidInfo : GButton
    {
        public GGraph m_headback;
        public GTextField m_name;
        public GTextInput m_delay;
        public GLoader m_head;
        public GTextInput m_x;
        public GTextInput m_y;
        public GTextInput m_tag;
        public GComboBox m_direction;
        public const string URL = "ui://wof4wytzdy0mi";

        public static UI_MidInfo CreateInstance()
        {
            return (UI_MidInfo)UIPackage.CreateObject("MapBuilderUI", "MidInfo");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_headback = (GGraph)GetChildAt(4);
            m_name = (GTextField)GetChildAt(5);
            m_delay = (GTextInput)GetChildAt(7);
            m_head = (GLoader)GetChildAt(9);
            m_x = (GTextInput)GetChildAt(12);
            m_y = (GTextInput)GetChildAt(13);
            m_tag = (GTextInput)GetChildAt(16);
            m_direction = (GComboBox)GetChildAt(17);
            Init();
        }
        partial void Init();
    }
}