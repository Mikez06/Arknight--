/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace BattleUI
{
    public partial class UI_Battle : GComponent
    {
        public Controller m_state;
        public GTextField m_enemy;
        public GTextField m_hp;
        public GComponent m_Builds;
        public GTextField m_number;
        public GTextField m_cost;
        public UI_BattleLeft m_left;
        public GProgressBar m_costBar;
        public const string URL = "ui://vp312gabf1460";

        public static UI_Battle CreateInstance()
        {
            return (UI_Battle)UIPackage.CreateObject("BattleUI", "Battle");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_state = GetControllerAt(0);
            m_enemy = (GTextField)GetChildAt(2);
            m_hp = (GTextField)GetChildAt(4);
            m_Builds = (GComponent)GetChildAt(5);
            m_number = (GTextField)GetChildAt(8);
            m_cost = (GTextField)GetChildAt(10);
            m_left = (UI_BattleLeft)GetChildAt(11);
            m_costBar = (GProgressBar)GetChildAt(14);
            Init();
        }
        partial void Init();
    }
}