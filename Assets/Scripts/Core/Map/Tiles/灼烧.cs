using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiles
{
    public class 灼烧 : Tile
    {
        int minTime, maxTime;
        float damage;
        CountDown burn = new CountDown();
        public override void Init(Map map, MapGrid mapGrid)
        {
            base.Init(map, mapGrid);
            minTime = TileData.Data.GetInt("BurnTimeMin");
            maxTime = TileData.Data.GetInt("BurnTimeMax");
            damage = TileData.Data.GetInt("BurnDamage");
        }

        public override void Update()
        {
            base.Update();
            if (burn.Finished())
            {
                burn.Set(Battle.Random.Next(minTime, maxTime + 1));
            }
            if (burn.Update(SystemConfig.DeltaTime))
            {
                var targets = Battle.FindAll(new UnityEngine.Vector2Int(X, Y), 0);
                var targets1 = Battle.FindAll(new UnityEngine.Vector2Int(X, Y), 1);
                targets.Union(targets1);
                foreach (var target in targets)
                {
                    target.Damage(new DamageInfo()
                    {
                        Attack = damage,
                        DamageType = DamageTypeEnum.Real,
                    });
                }
            }
        }
    }
}
