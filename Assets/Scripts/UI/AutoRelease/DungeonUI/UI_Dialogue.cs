/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace DungeonUI
{
    public partial class UI_Dialogue : GComponent
    {
        public Controller m_chooseReward;
        public GComponent m_cArea;
        public GLoader m_background;
        public GTextField m_EventTitle;
        public GTextField m_DialogueText;
        public GList m_Chooices;
        public GList m_Rewards;
        public Transition m_chooseAni;
        public Transition m_reset;
        public const string URL = "ui://hgasjns6gbwm11";

        public static UI_Dialogue CreateInstance()
        {
            return (UI_Dialogue)UIPackage.CreateObject("DungeonUI", "Dialogue");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_chooseReward = GetControllerAt(0);
            m_cArea = (GComponent)GetChildAt(0);
            m_background = (GLoader)GetChildAt(1);
            m_EventTitle = (GTextField)GetChildAt(4);
            m_DialogueText = (GTextField)GetChildAt(5);
            m_Chooices = (GList)GetChildAt(6);
            m_Rewards = (GList)GetChildAt(8);
            m_chooseAni = GetTransitionAt(0);
            m_reset = GetTransitionAt(1);
            Init();
        }
        partial void Init();
    }
}