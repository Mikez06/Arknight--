/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace BattleUI
{
    public partial class UI_CostBar : GProgressBar
    {
        public GGraph m_back;
        public const string URL = "ui://vp312gabkbte40";

        public static UI_CostBar CreateInstance()
        {
            return (UI_CostBar)UIPackage.CreateObject("BattleUI", "CostBar");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_back = (GGraph)GetChildAt(0);
            Init();
        }
        partial void Init();
    }
}