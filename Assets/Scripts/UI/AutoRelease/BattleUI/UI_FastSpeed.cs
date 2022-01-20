/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace BattleUI
{
    public partial class UI_FastSpeed : GComponent
    {
        public Controller m_Speed;
        public const string URL = "ui://vp312gabrkte51";

        public static UI_FastSpeed CreateInstance()
        {
            return (UI_FastSpeed)UIPackage.CreateObject("BattleUI", "FastSpeed");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_Speed = GetControllerAt(0);
            Init();
        }
        partial void Init();
    }
}