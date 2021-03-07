/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace BattleUI
{
    public partial class UI_HuiZhao : GButton
    {
        public GImage m_hole;
        public const string URL = "ui://vp312gabkbte3x";

        public static UI_HuiZhao CreateInstance()
        {
            return (UI_HuiZhao)UIPackage.CreateObject("BattleUI", "灰罩");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_hole = (GImage)GetChildAt(1);
            Init();
        }
        partial void Init();
    }
}