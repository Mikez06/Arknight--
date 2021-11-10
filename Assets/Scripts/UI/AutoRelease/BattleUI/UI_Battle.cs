/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace BattleUI
{
    public partial class UI_Battle : GComponent
    {
        public Controller m_state;
        public Controller m_win;
        public GTextField m_enemy;
        public GTextField m_hp;
        public GComponent m_Units;
        public GComponent m_Builds;
        public GComponent m_DamageInfo;
        public UI_BattleLeft m_left;
        public GComponent m_SkillUseBack;
        public UI_SkillUsePanel m_SkillUsePanel;
        public GTextField m_number;
        public GTextField m_cost;
        public UI_CostBar m_costBar;
        public GGroup m_youxia;
        public GComponent m_endClick;
        public GLoader m_endPic;
        public Transition m_win1;
        public Transition m_win2;
        public Transition m_win3;
        public Transition m_reset;
        public const string URL = "ui://vp312gabf1460";

        public static UI_Battle CreateInstance()
        {
            return (UI_Battle)UIPackage.CreateObject("BattleUI", "Battle");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_state = GetControllerAt(0);
            m_win = GetControllerAt(1);
            m_enemy = (GTextField)GetChildAt(1);
            m_hp = (GTextField)GetChildAt(2);
            m_Units = (GComponent)GetChildAt(3);
            m_Builds = (GComponent)GetChildAt(4);
            m_DamageInfo = (GComponent)GetChildAt(5);
            m_left = (UI_BattleLeft)GetChildAt(6);
            m_SkillUseBack = (GComponent)GetChildAt(7);
            m_SkillUsePanel = (UI_SkillUsePanel)GetChildAt(8);
            m_number = (GTextField)GetChildAt(10);
            m_cost = (GTextField)GetChildAt(11);
            m_costBar = (UI_CostBar)GetChildAt(12);
            m_youxia = (GGroup)GetChildAt(13);
            m_endClick = (GComponent)GetChildAt(15);
            m_endPic = (GLoader)GetChildAt(16);
            m_win1 = GetTransitionAt(0);
            m_win2 = GetTransitionAt(1);
            m_win3 = GetTransitionAt(2);
            m_reset = GetTransitionAt(3);
            Init();
        }
        partial void Init();
    }
}