/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace BattleUI
{
    public partial class UI_DamageInfo : GComponent
    {
        public Controller m_type;
        public GTextField m_number;
        public Transition m_show;
        public const string URL = "ui://vp312gabn3en4g";

        public static UI_DamageInfo CreateInstance()
        {
            return (UI_DamageInfo)UIPackage.CreateObject("BattleUI", "DamageInfo");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_type = GetControllerAt(0);
            m_number = (GTextField)GetChildAt(0);
            m_show = GetTransitionAt(0);
            Init();
        }
        partial void Init();
    }
}