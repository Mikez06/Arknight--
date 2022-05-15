/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace BattleUI
{
    public partial class UI_ElementBar : GProgressBar
    {
        public Controller m_Recover;
        public GImage m_back;
        public const string URL = "ui://vp312gabiexc56";

        public static UI_ElementBar CreateInstance()
        {
            return (UI_ElementBar)UIPackage.CreateObject("BattleUI", "ElementBar");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_Recover = GetControllerAt(0);
            m_back = (GImage)GetChildAt(0);
            Init();
        }
        partial void Init();
    }
}