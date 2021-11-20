using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class RewardDataEx
{
    public static DungeonReward GetReward(this RewardData rewardData,System.Random seed)
    {
        DungeonReward result = new DungeonReward();
        if (rewardData.RewardGold > 0)
        {
            result.Type = 2;
            result.Data = rewardData.RewardGold;
        }
        else if (rewardData.RewardPoint > 0)
        {
            result.Type = 3;
            result.Data = rewardData.RewardPoint;
        }
        else
        {
            int weightAll = rewardData.Weights.Sum();
            int r = seed.Next(0, weightAll);
            for (int i = 0; i < rewardData.Weights.Length; i++)
            {
                int w = rewardData.Weights[i];
                r -= w;
                if (r < 0)
                {
                    RelicData relicData = Database.Instance.Get<RelicData>(rewardData.RewardItem[i]);
                    result.Data = Database.Instance.GetIndex(relicData);
                    if (relicData.Profession != null)
                    {
                        result.Type = 1;
                    }
                    else
                    {
                        result.Type = 0;
                    }
                    break;
                }
            }
        }
        return result;
    }
}
