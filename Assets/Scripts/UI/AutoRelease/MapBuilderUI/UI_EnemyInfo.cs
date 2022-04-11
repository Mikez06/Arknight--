/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace MapBuilderUI
{
    public partial class UI_EnemyInfo : GComponent
    {
        public GTextField m_name;
        public GLoader m_head;
        public GTextField m_atk;
        public GTextField m_def;
        public GTextField m_hp;
        public const string URL = "ui://wof4wytzq2ung";

        public static UI_EnemyInfo CreateInstance()
        {
            return (UI_EnemyInfo)UIPackage.CreateObject("MapBuilderUI", "EnemyInfo");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_name = (GTextField)GetChildAt(0);
            m_head = (GLoader)GetChildAt(1);
            m_atk = (GTextField)GetChildAt(3);
            m_def = (GTextField)GetChildAt(5);
            m_hp = (GTextField)GetChildAt(7);
            Init();
        }
        partial void Init();
    }
}