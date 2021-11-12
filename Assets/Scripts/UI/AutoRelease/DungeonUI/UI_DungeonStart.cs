/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace DungeonUI
{
    public partial class UI_DungeonStart : GComponent
    {
        public Controller m_choose;
        public GButton m_Start;
        public UI_StartUnit m_StartUnit;
        public UI_InitUnit m_ChooseWindow;
        public const string URL = "ui://hgasjns6t5h90";

        public static UI_DungeonStart CreateInstance()
        {
            return (UI_DungeonStart)UIPackage.CreateObject("DungeonUI", "DungeonStart");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_choose = GetControllerAt(0);
            m_Start = (GButton)GetChildAt(1);
            m_StartUnit = (UI_StartUnit)GetChildAt(2);
            m_ChooseWindow = (UI_InitUnit)GetChildAt(4);
            Init();
        }
        partial void Init();
    }
}