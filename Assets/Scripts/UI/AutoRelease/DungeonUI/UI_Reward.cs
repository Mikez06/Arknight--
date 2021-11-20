/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace DungeonUI
{
    public partial class UI_Reward : GComponent
    {
        public Controller m_type;
        public GTextField m_Name;
        public GLoader m_icon;
        public GGraph m_get;
        public GTextField m_count;
        public Transition m_get_2;
        public const string URL = "ui://hgasjns6gbwm14";

        public static UI_Reward CreateInstance()
        {
            return (UI_Reward)UIPackage.CreateObject("DungeonUI", "Reward");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_type = GetControllerAt(0);
            m_Name = (GTextField)GetChildAt(1);
            m_icon = (GLoader)GetChildAt(2);
            m_get = (GGraph)GetChildAt(3);
            m_count = (GTextField)GetChildAt(6);
            m_get_2 = GetTransitionAt(0);
            Init();
        }
        partial void Init();
    }
}