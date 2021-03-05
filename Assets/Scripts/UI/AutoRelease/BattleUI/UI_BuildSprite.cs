/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace BattleUI
{
    public partial class UI_BuildSprite : GComponent
    {
        public Controller m_typeControl;
        public Controller m_cooldown;
        public Controller m_canUse;
        public UI_Head m_headIcon;
        public GTextField m_cost;
        public GProgressBar m_bar;
        public GTextField m_resetTime;
        public const string URL = "ui://vp312gabf1463l";

        public static UI_BuildSprite CreateInstance()
        {
            return (UI_BuildSprite)UIPackage.CreateObject("BattleUI", "BuildSprite");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_typeControl = GetControllerAt(0);
            m_cooldown = GetControllerAt(1);
            m_canUse = GetControllerAt(2);
            m_headIcon = (UI_Head)GetChildAt(0);
            m_cost = (GTextField)GetChildAt(3);
            m_bar = (GProgressBar)GetChildAt(4);
            m_resetTime = (GTextField)GetChildAt(6);
            Init();
        }
        partial void Init();
    }
}