/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace BattleUI
{
    public partial class UI_Head : GComponent
    {
        public GLoader m_headIcon;
        public const string URL = "ui://vp312gabkbte3o";

        public static UI_Head CreateInstance()
        {
            return (UI_Head)UIPackage.CreateObject("BattleUI", "Head");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_headIcon = (GLoader)GetChildAt(0);
            Init();
        }
        partial void Init();
    }
}