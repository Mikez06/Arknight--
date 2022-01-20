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
        public GComponent m_Setting;
        public GTextField m_number;
        public GTextField m_cost;
        public UI_CostBar m_costBar;
        public GGroup m_youxia;
        public UI_FastSpeed m_GameSpeed;
        public UI_Pause m_Pause;
        public GGroup m_normalGroup;
        public GComponent m_endClick;
        public GLoader m_endPic;
        public GGroup m_endGroup;
        public GGraph m_GiveUpBack;
        public GComponent m_CancelGiveUp;
        public GComponent m_GiveUp;
        public GGroup m_giveupGroup;
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
            m_Setting = (GComponent)GetChildAt(9);
            m_number = (GTextField)GetChildAt(11);
            m_cost = (GTextField)GetChildAt(12);
            m_costBar = (UI_CostBar)GetChildAt(13);
            m_youxia = (GGroup)GetChildAt(14);
            m_GameSpeed = (UI_FastSpeed)GetChildAt(15);
            m_Pause = (UI_Pause)GetChildAt(16);
            m_normalGroup = (GGroup)GetChildAt(17);
            m_endClick = (GComponent)GetChildAt(18);
            m_endPic = (GLoader)GetChildAt(19);
            m_endGroup = (GGroup)GetChildAt(27);
            m_GiveUpBack = (GGraph)GetChildAt(28);
            m_CancelGiveUp = (GComponent)GetChildAt(31);
            m_GiveUp = (GComponent)GetChildAt(32);
            m_giveupGroup = (GGroup)GetChildAt(33);
            m_win1 = GetTransitionAt(0);
            m_win2 = GetTransitionAt(1);
            m_win3 = GetTransitionAt(2);
            m_reset = GetTransitionAt(3);
            Init();
        }
        partial void Init();
    }
}