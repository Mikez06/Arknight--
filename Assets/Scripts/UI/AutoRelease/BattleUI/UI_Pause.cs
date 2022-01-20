/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace BattleUI
{
    public partial class UI_Pause : GComponent
    {
        public Controller m_Speed;
        public const string URL = "ui://vp312gabrkte52";

        public static UI_Pause CreateInstance()
        {
            return (UI_Pause)UIPackage.CreateObject("BattleUI", "Pause");
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