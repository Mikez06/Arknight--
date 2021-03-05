/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace BattleUI
{
    public partial class UI_BattleLeft : GComponent
    {
        public Controller m_left;
        public GLoader m_standPic;
        public GTextField m_name;
        public GTextField m_Lv;
        public GTextField m_atk;
        public GTextField m_def;
        public GTextField m_magDef;
        public GTextField m_block;
        public GTextField m_subSkill;
        public const string URL = "ui://vp312gabkbte3s";

        public static UI_BattleLeft CreateInstance()
        {
            return (UI_BattleLeft)UIPackage.CreateObject("BattleUI", "BattleLeft");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_left = GetControllerAt(0);
            m_standPic = (GLoader)GetChildAt(1);
            m_name = (GTextField)GetChildAt(3);
            m_Lv = (GTextField)GetChildAt(5);
            m_atk = (GTextField)GetChildAt(11);
            m_def = (GTextField)GetChildAt(12);
            m_magDef = (GTextField)GetChildAt(13);
            m_block = (GTextField)GetChildAt(14);
            m_subSkill = (GTextField)GetChildAt(19);
            Init();
        }
        partial void Init();
    }
}