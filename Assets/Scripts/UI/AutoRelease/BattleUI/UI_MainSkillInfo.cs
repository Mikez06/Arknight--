/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace BattleUI
{
    public partial class UI_MainSkillInfo : GComponent
    {
        public Controller m_using;
        public GImage m_bar;
        public const string URL = "ui://vp312gabkbte48";

        public static UI_MainSkillInfo CreateInstance()
        {
            return (UI_MainSkillInfo)UIPackage.CreateObject("BattleUI", "MainSkillInfo");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_using = GetControllerAt(0);
            m_bar = (GImage)GetChildAt(1);
            Init();
        }
        partial void Init();
    }
}