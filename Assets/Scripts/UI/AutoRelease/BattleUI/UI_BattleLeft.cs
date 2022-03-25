/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace BattleUI
{
    public partial class UI_BattleLeft : GComponent
    {
        public Controller m_left;
        public Controller m_time;
        public Controller m_palyerUnit;
        public UI_Pic m_standPic;
        public GProgressBar m_Hp;
        public GTextField m_atk;
        public GTextField m_def;
        public GTextField m_magDef;
        public GTextField m_block;
        public GComponent m_attackArea;
        public GTextField m_subSkill;
        public GTextField m_agi;
        public GTextField m_name;
        public GTextField m_Lv;
        public GTextField m_SkillName;
        public GLoader m_skillIcon;
        public GTextField m_SkillDesc;
        public GComponent m_Recover;
        public GComponent m_Pro;
        public GComponent m_UseType;
        public GTextField m_lastTime;
        public GGroup m_skillInfo;
        public GTextField m_midUnitDesc;
        public const string URL = "ui://vp312gabkbte3s";

        public static UI_BattleLeft CreateInstance()
        {
            return (UI_BattleLeft)UIPackage.CreateObject("BattleUI", "BattleLeft");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_left = GetControllerAt(0);
            m_time = GetControllerAt(1);
            m_palyerUnit = GetControllerAt(2);
            m_standPic = (UI_Pic)GetChildAt(1);
            m_Hp = (GProgressBar)GetChildAt(6);
            m_atk = (GTextField)GetChildAt(7);
            m_def = (GTextField)GetChildAt(8);
            m_magDef = (GTextField)GetChildAt(9);
            m_block = (GTextField)GetChildAt(10);
            m_attackArea = (GComponent)GetChildAt(13);
            m_subSkill = (GTextField)GetChildAt(15);
            m_agi = (GTextField)GetChildAt(17);
            m_name = (GTextField)GetChildAt(18);
            m_Lv = (GTextField)GetChildAt(21);
            m_SkillName = (GTextField)GetChildAt(23);
            m_skillIcon = (GLoader)GetChildAt(24);
            m_SkillDesc = (GTextField)GetChildAt(25);
            m_Recover = (GComponent)GetChildAt(26);
            m_Pro = (GComponent)GetChildAt(27);
            m_UseType = (GComponent)GetChildAt(28);
            m_lastTime = (GTextField)GetChildAt(30);
            m_skillInfo = (GGroup)GetChildAt(31);
            m_midUnitDesc = (GTextField)GetChildAt(32);
            Init();
        }
        partial void Init();
    }
}