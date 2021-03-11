/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace MainUI
{
    public partial class UI_Battle : GComponent
    {
        public GComponent m_world;
        public GButton m_back;
        public const string URL = "ui://k4mja8t1kbtew";

        public static UI_Battle CreateInstance()
        {
            return (UI_Battle)UIPackage.CreateObject("MainUI", "Battle");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_world = (GComponent)GetChildAt(1);
            m_back = (GButton)GetChildAt(2);
            Init();
        }
        partial void Init();
    }
}