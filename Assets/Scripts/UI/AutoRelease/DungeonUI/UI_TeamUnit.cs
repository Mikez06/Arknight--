/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace DungeonUI
{
    public partial class UI_TeamUnit : GComponent
    {
        public Controller m_State;
        public GLoader m_Head;
        public GTextField m_Name;
        public GList m_Skills;
        public GTextField m_level;
        public GTextField m_exp;
        public const string URL = "ui://hgasjns6t5h98";

        public static UI_TeamUnit CreateInstance()
        {
            return (UI_TeamUnit)UIPackage.CreateObject("DungeonUI", "TeamUnit");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_State = GetControllerAt(0);
            m_Head = (GLoader)GetChildAt(1);
            m_Name = (GTextField)GetChildAt(2);
            m_Skills = (GList)GetChildAt(3);
            m_level = (GTextField)GetChildAt(4);
            m_exp = (GTextField)GetChildAt(5);
            Init();
        }
        partial void Init();
    }
}