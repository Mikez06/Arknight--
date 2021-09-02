using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiles
{
    public class 陷坑 : Tile
    {
        const float DropSpeed = 1;
        public override void Update()
        {
            base.Update();
            var targets = Battle.FindAll(new UnityEngine.Vector2Int(X, Y), 1, false);
            foreach (var target in targets)
            {
                if (target.Position.x < X-0.5f || target.Position.x > X + 0.5f || target.Position.z < Y-0.5f || target.Position.z > Y + 0.5f) continue;
                if (target.Alive())
                {
                    target.DoDie(this);
                }
                target.Position.y -= DropSpeed * SystemConfig.DeltaTime;
            }
        }
    }
}
