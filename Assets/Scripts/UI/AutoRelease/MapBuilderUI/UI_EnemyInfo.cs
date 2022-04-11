/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace MapBuilderUI
{
    public partial class UI_EnemyInfo : GComponent
    {
        public GTextField m_name;
        public GLoader m_head;
        public GTextInput m_atk;
        public GTextInput m_def;
        public GTextInput m_hp;
        public GTextInput m_magDef;
        public const string URL = "ui://wof4wytzq2ung";

        public static UI_EnemyInfo CreateInstance()
        {
            return (UI_EnemyInfo)UIPackage.CreateObject("MapBuilderUI", "EnemyInfo");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_name = (GTextField)GetChildAt(4);
            m_head = (GLoader)GetChildAt(5);
            m_atk = (GTextInput)GetChildAt(7);
            m_def = (GTextInput)GetChildAt(9);
            m_hp = (GTextInput)GetChildAt(11);
            m_magDef = (GTextInput)GetChildAt(13);
            Init();
        }
        partial void Init();
    }
}