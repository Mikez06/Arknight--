/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace MapBuilderUI
{
    public partial class UI_PathPoint : GButton
    {
        public GTextField m_X;
        public GTextField m_Y;
        public GTextInput m_Delay;
        public GButton m_DirectMove;
        public GButton m_HideMove;
        public const string URL = "ui://wof4wytzjh399";

        public static UI_PathPoint CreateInstance()
        {
            return (UI_PathPoint)UIPackage.CreateObject("MapBuilderUI", "PathPoint");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_X = (GTextField)GetChildAt(2);
            m_Y = (GTextField)GetChildAt(5);
            m_Delay = (GTextInput)GetChildAt(8);
            m_DirectMove = (GButton)GetChildAt(10);
            m_HideMove = (GButton)GetChildAt(12);
            Init();
        }
        partial void Init();
    }
}