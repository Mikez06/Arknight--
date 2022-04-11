/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace MapBuilderUI
{
    public partial class UI_PathPage : GComponent
    {
        public Controller m_select;
        public GList m_Paths;
        public GButton m_AddPath;
        public GButton m_DeletePath;
        public GList m_PathPoints;
        public GButton m_AddPoint;
        public GButton m_DeletePoint;
        public GButton m_CopyPath;
        public GButton m_InsertPoint;
        public const string URL = "ui://wof4wytzei158";

        public static UI_PathPage CreateInstance()
        {
            return (UI_PathPage)UIPackage.CreateObject("MapBuilderUI", "PathPage");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_select = GetControllerAt(0);
            m_Paths = (GList)GetChildAt(0);
            m_AddPath = (GButton)GetChildAt(1);
            m_DeletePath = (GButton)GetChildAt(2);
            m_PathPoints = (GList)GetChildAt(3);
            m_AddPoint = (GButton)GetChildAt(4);
            m_DeletePoint = (GButton)GetChildAt(5);
            m_CopyPath = (GButton)GetChildAt(7);
            m_InsertPoint = (GButton)GetChildAt(8);
            Init();
        }
        partial void Init();
    }
}