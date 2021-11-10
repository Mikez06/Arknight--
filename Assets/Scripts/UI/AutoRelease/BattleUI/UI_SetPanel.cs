/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace BattleUI
{
    public partial class UI_SetPanel : GComponent
    {
        public Controller m_coner;
        public GButton m_grip;
        public const string URL = "ui://vp312gabkbte3y";

        public static UI_SetPanel CreateInstance()
        {
            return (UI_SetPanel)UIPackage.CreateObject("BattleUI", "SetPanel");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_coner = GetControllerAt(0);
            m_grip = (GButton)GetChildAt(7);
            Init();
        }
        partial void Init();
    }
}