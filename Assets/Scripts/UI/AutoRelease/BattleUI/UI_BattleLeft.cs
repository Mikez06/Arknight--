/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace BattleUI
{
    public partial class UI_BattleLeft : GComponent
    {
        public Controller m_left;
        public Controller m_time;
        public UI_Pic m_standPic;
        public GTextField m_name;
        public GTextField m_Lv;
        public GProgressBar m_Hp;
        public GTextField m_atk;
        public GTextField m_def;
        public GTextField m_magDef;
        public GTextField m_block;
        public GTextField m_subSkill;
        public GTextField m_SkillName;
        public GLoader m_skillIcon;
        public GTextField m_SkillDesc;
        public GComponent m_Pro;
        public GComponent m_Recover;
        public GComponent m_UseType;
        public GTextField m_lastTime;
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
            m_standPic = (UI_Pic)GetChildAt(1);
            m_name = (GTextField)GetChildAt(2);
            m_Lv = (GTextField)GetChildAt(4);
            m_Hp = (GProgressBar)GetChildAt(9);
            m_atk = (GTextField)GetChildAt(10);
            m_def = (GTextField)GetChildAt(11);
            m_magDef = (GTextField)GetChildAt(12);
            m_block = (GTextField)GetChildAt(13);
            m_subSkill = (GTextField)GetChildAt(17);
            m_SkillName = (GTextField)GetChildAt(19);
            m_skillIcon = (GLoader)GetChildAt(20);
            m_SkillDesc = (GTextField)GetChildAt(21);
            m_Pro = (GComponent)GetChildAt(22);
            m_Recover = (GComponent)GetChildAt(23);
            m_UseType = (GComponent)GetChildAt(24);
            m_lastTime = (GTextField)GetChildAt(26);
            Init();
        }
        partial void Init();
    }
}