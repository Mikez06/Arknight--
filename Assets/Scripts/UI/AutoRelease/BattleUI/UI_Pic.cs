/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace BattleUI
{
    public partial class UI_Pic : GComponent
    {
        public GLoader m_standPic;
        public const string URL = "ui://vp312gabvoep4z";

        public static UI_Pic CreateInstance()
        {
            return (UI_Pic)UIPackage.CreateObject("BattleUI", "Pic");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_standPic = (GLoader)GetChildAt(0);
            Init();
        }
        partial void Init();
    }
}