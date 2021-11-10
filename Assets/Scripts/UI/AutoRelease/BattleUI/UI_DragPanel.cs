/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace BattleUI
{
    public partial class UI_DragPanel : GComponent
    {
        public UI_HuiZhao m_DirectionBack;
        public GComponent m_DirectonCancal;
        public UI_SetPanel m_DirectionPanel;
        public const string URL = "ui://vp312gabt5h94o";

        public static UI_DragPanel CreateInstance()
        {
            return (UI_DragPanel)UIPackage.CreateObject("BattleUI", "DragPanel");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_DirectionBack = (UI_HuiZhao)GetChildAt(0);
            m_DirectonCancal = (GComponent)GetChildAt(1);
            m_DirectionPanel = (UI_SetPanel)GetChildAt(2);
            Init();
        }
        partial void Init();
    }
}