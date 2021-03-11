/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace MainUI
{
    public partial class UI_AttackArea : GComponent
    {
        public Controller m_type;
        public const string URL = "ui://k4mja8t1k6t714";

        public static UI_AttackArea CreateInstance()
        {
            return (UI_AttackArea)UIPackage.CreateObject("MainUI", "AttackArea");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_type = GetControllerAt(0);
            Init();
        }
        partial void Init();
    }
}