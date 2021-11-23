/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace MainUI
{
    public partial class UI_BattleInfo : GLabel
    {
        public GGraph m_aaa;
        public const string URL = "ui://k4mja8t1kbte10";

        public static UI_BattleInfo CreateInstance()
        {
            return (UI_BattleInfo)UIPackage.CreateObject("MainUI", "BattleInfo");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_aaa = (GGraph)GetChildAt(0);
            Init();
        }
        partial void Init();
    }
}