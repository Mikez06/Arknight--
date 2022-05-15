/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace MainUI
{
    public partial class UI_Loading : GComponent
    {
        public GTextField m_name;
        public const string URL = "ui://k4mja8t1it6gr51";

        public static UI_Loading CreateInstance()
        {
            return (UI_Loading)UIPackage.CreateObject("MainUI", "Loading");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_name = (GTextField)GetChildAt(1);
            Init();
        }
        partial void Init();
    }
}