/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace BattleUI
{
    public partial class UI_BattleUnit : GComponent
    {
        public Controller m_readyControl;
        public Controller m_skillCount;
        public Controller m_unitType;
        public GProgressBar m_hp;
        public UI_SK0 m_sk;
        public GTextField m_skillCount_2;
        public GProgressBar m_eHp;
        public const string URL = "ui://vp312gabh4sa41";

        public static UI_BattleUnit CreateInstance()
        {
            return (UI_BattleUnit)UIPackage.CreateObject("BattleUI", "BattleUnit");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_readyControl = GetControllerAt(0);
            m_skillCount = GetControllerAt(1);
            m_unitType = GetControllerAt(2);
            m_hp = (GProgressBar)GetChildAt(0);
            m_sk = (UI_SK0)GetChildAt(1);
            m_skillCount_2 = (GTextField)GetChildAt(5);
            m_eHp = (GProgressBar)GetChildAt(6);
            Init();
        }
        partial void Init();
    }
}