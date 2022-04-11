using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapBuilderUI
{
    partial class UI_PathInfo
    {
        public PathInfo PathInfo;
        partial void Init()
        {
            m_PathName.onChanged.Add(() =>
            {
                PathInfo.Name = m_PathName.text;
            });
        }
        public void SetInfo(PathInfo pathInfo)
        {
            PathInfo = pathInfo;
            m_PathName.text = PathInfo.Name;
        }
    }
}
