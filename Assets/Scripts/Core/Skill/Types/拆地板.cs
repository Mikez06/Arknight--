using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skills
{
    public class 拆地板 : 非指向技能
    {
        const int range = 2;
        public override bool Ready()
        {
            var l = GetTiles();
            if (l.Count == 0) return false;
            return base.Ready();
        }

        public override void Effect(Unit target)
        {
            var l = GetTiles();
            List<Tile> targets = new List<Tile>();
            for (int i = 0; i < SkillData.DamageCount; i++)
            {
                if (l.Count == 0) break;
                var t = l[Battle.Random.Next(0, l.Count)];
                l.Remove(t);
                targets.Add(t);
            }
            foreach (var t in targets)
            {
                t.CanBuildUnit = false;
                if (t.Unit != null)
                {
                    t.Unit.DoDie(new DamageInfo()
                    {
                        Source = this,
                        Target = t.Unit,
                    });
                    t.Unit = null;
                }
                if (SkillData.HitEffect != null)
                {
                    var ps = EffectManager.Instance.GetEffect(SkillData.HitEffect.Value);
                    ps.transform.position = t.Pos;
                    ps.Play();
                }
            }
            base.Effect(target);
        }

        List<Tile> GetTiles()
        {
            List<Tile> result = new List<Tile>();
            for (int i = -range; i <= range; i++)
            {
                int yc = range - Math.Abs(i);
                for (int j = -yc; j <= yc; j++)
                {
                    int x = Unit.GridPos.x + i;
                    int y = Unit.GridPos.y + j;

                    if (x >= 0 && y >= 0 && x < Battle.Map.Tiles.GetLength(0) && y < Battle.Map.Tiles.GetLength(1) && Battle.Map.Tiles[x, y].CanBuildUnit)
                    {
                        result.Add(Battle.Map.Tiles[x, y]);
                    }
                }
            }
            return result;
        }
    }
}
