/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace BattleUI
{
    public partial class UI_PingJiaXingXing : GComponent
    {
        public GImage m_ShineEffect;
        public GImage m_Star;
        public Transition m_ShineIn;
        public Transition m_ShineOut;
        public Transition m_Reset;
        public const string URL = "ui://vp312gabkbte4d";

        public static UI_PingJiaXingXing CreateInstance()
        {
            return (UI_PingJiaXingXing)UIPackage.CreateObject("BattleUI", "评价星星");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_ShineEffect = (GImage)GetChildAt(0);
            m_Star = (GImage)GetChildAt(1);
            m_ShineIn = GetTransitionAt(0);
            m_ShineOut = GetTransitionAt(1);
            m_Reset = GetTransitionAt(2);
            Init();
        }
        partial void Init();
    }
}