using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapBuilderUI
{
    partial class UI_PathPoint
    {
        public PathPoint PathPoint;

        partial void Init()
        {
            m_Delay.onChanged.Add(() =>
            {
                PathPoint.Delay = float.Parse(m_Delay.text);
            });
            m_DirectMove.onClick.Add(() =>
            {
                PathPoint.DirectMove = !PathPoint.DirectMove;
            });
            m_HideMove.onClick.Add(() =>
            {
                PathPoint.HideMove = !PathPoint.HideMove;
            });
        }

        public void SetInfo(PathPoint pathPoint)
        {
            this.PathPoint = pathPoint;
            m_X.text = pathPoint.Pos.x.ToString();
            m_Y.text = pathPoint.Pos.z.ToString();
            m_Delay.text = pathPoint.Delay.ToString();
            m_DirectMove.selected = !pathPoint.DirectMove;
            m_HideMove.selected = !pathPoint.HideMove;
        }
    }
}
