/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace BattleUI
{
    public partial class UI_SK0 : GProgressBar
    {
        public Controller m_useControl;
        public const string URL = "ui://vp312gabh4sa43";

        public static UI_SK0 CreateInstance()
        {
            return (UI_SK0)UIPackage.CreateObject("BattleUI", "SK0");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_useControl = GetControllerAt(0);
            Init();
        }
        partial void Init();
    }
}