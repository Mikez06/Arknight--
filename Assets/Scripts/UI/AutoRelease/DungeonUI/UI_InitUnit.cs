/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace DungeonUI
{
    public partial class UI_InitUnit : GComponent
    {
        public GGraph m_back;
        public GList m_Units;
        public const string URL = "ui://hgasjns6t5h93";

        public static UI_InitUnit CreateInstance()
        {
            return (UI_InitUnit)UIPackage.CreateObject("DungeonUI", "InitUnit");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_back = (GGraph)GetChildAt(0);
            m_Units = (GList)GetChildAt(2);
            Init();
        }
        partial void Init();
    }
}