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
        public GComponent m_Units;
        public GComponent m_Builds;
        public GTextField m_number;
        public GTextField m_cost;
        public UI_BattleLeft m_left;
        public UI_HuiZhao m_DirectionBack;
        public GComponent m_DirectonCancal;
        public UI_SetPanel m_DirectionPanel;
        public GProgressBar m_costBar;
        public GComponent m_SkillUseBack;
        public UI_SkillUsePanel m_SkillUsePanel;
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
            m_Units = (GComponent)GetChildAt(5);
            m_Builds = (GComponent)GetChildAt(6);
            m_number = (GTextField)GetChildAt(9);
            m_cost = (GTextField)GetChildAt(11);
            m_left = (UI_BattleLeft)GetChildAt(12);
            m_DirectionBack = (UI_HuiZhao)GetChildAt(13);
            m_DirectonCancal = (GComponent)GetChildAt(14);
            m_DirectionPanel = (UI_SetPanel)GetChildAt(15);
            m_costBar = (GProgressBar)GetChildAt(16);
            m_SkillUseBack = (GComponent)GetChildAt(17);
            m_SkillUsePanel = (UI_SkillUsePanel)GetChildAt(18);
            Init();
        }
        partial void Init();
    }
}