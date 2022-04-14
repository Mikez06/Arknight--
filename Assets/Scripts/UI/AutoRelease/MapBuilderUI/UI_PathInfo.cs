/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace MapBuilderUI
{
    public partial class UI_PathInfo : GButton
    {
        public GTextInput m_PathName;
        public GButton m_Fly;
        public const string URL = "ui://wof4wytzei157";

        public static UI_PathInfo CreateInstance()
        {
            return (UI_PathInfo)UIPackage.CreateObject("MapBuilderUI", "PathInfo");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_PathName = (GTextInput)GetChildAt(2);
            m_Fly = (GButton)GetChildAt(5);
            Init();
        }
        partial void Init();
    }
}