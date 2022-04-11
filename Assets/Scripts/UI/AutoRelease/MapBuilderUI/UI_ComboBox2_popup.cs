/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace MapBuilderUI
{
    public partial class UI_ComboBox2_popup : GComponent
    {
        public GList m_list;
        public const string URL = "ui://wof4wytzq2une";

        public static UI_ComboBox2_popup CreateInstance()
        {
            return (UI_ComboBox2_popup)UIPackage.CreateObject("MapBuilderUI", "ComboBox2_popup");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_list = (GList)GetChildAt(1);
            Init();
        }
        partial void Init();
    }
}