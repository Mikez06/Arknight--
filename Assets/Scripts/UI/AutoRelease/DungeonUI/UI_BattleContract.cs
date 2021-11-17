/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace DungeonUI
{
    public partial class UI_BattleContract : GComponent
    {
        public Controller m_button;
        public GLoader m_icon;
        public GTextField m_TagName;
        public const string URL = "ui://hgasjns6gbwmy";

        public static UI_BattleContract CreateInstance()
        {
            return (UI_BattleContract)UIPackage.CreateObject("DungeonUI", "BattleContract");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_button = GetControllerAt(0);
            m_icon = (GLoader)GetChildAt(1);
            m_TagName = (GTextField)GetChildAt(2);
            Init();
        }
        partial void Init();
    }
}