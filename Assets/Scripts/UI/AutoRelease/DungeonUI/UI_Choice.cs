/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace DungeonUI
{
    public partial class UI_Choice : GButton
    {
        public GTextField m_Description;
        public Transition m_t0;
        public Transition m_t1;
        public const string URL = "ui://hgasjns6gbwm13";

        public static UI_Choice CreateInstance()
        {
            return (UI_Choice)UIPackage.CreateObject("DungeonUI", "Choice");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_Description = (GTextField)GetChildAt(2);
            m_t0 = GetTransitionAt(0);
            m_t1 = GetTransitionAt(1);
            Init();
        }
        partial void Init();
    }
}