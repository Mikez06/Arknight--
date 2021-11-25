using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FairyGUI;
using UnityEngine;

namespace DungeonUI
{
    partial class UI_Dialogue
    {
        TaskCompletionSource<bool> clickTcs;
        TaskCompletionSource<int> chooseTcs;
        TaskCompletionSource<bool> rewardTcs;
        int nowChooseIndex = -1;

        partial void Init()
        {
            m_cArea.onClick.Add(() => { clickTcs?.TrySetResult(true); });
            m_Chooices.onClickItem.AddCapture(doChoose);
            m_Rewards.onClickItem.Add(doReward);
        }

        public async Task StartDialogue(string group)
        {
            nowEventIndex = Database.Instance.GetIndex(Database.Instance.GetAll<EventData>().FirstOrDefault(x => x.Group == group && x.EventId == "入口"));
            await showDialogue();
        }

        int nowEventIndex;
        EventData nowEvent => Database.Instance.Get<EventData>(nowEventIndex);

        async Task showDialogue()
        {
            if (nowEvent.Background != null)
            {
                m_background.texture = new NTexture(ResHelper.GetAsset<UnityEngine.Sprite>(PathHelper.SpritePath + nowEvent.Background));
            }
            if (nowEvent.Tile != null)
            {
                m_EventTitle.text = nowEvent.Tile;
            }
            m_DialogueText.text = nowEvent.Description;
            int next = nowEventIndex + 1;
            if (nowEvent.Choices != null)
            {
                int chooseResult=0;
                if (nowEvent.Branch != null)
                {
                    //按权重自动分支
                }
                else
                {
                    clickTcs = new TaskCompletionSource<bool>();
                    await clickTcs.Task;
                    //点击屏幕后进行选项
                    setChoice();
                    await m_chooseAni.PlayAsync();
                    chooseTcs = new TaskCompletionSource<int>();
                    chooseResult = await chooseTcs.Task;
                    if (nowEvent.Next == null || nowEvent.Next.Length <= chooseResult)
                    {
                        //不执行任何操作，把下一行当下一句
                    }
                    else
                    {
                        next = Database.Instance.GetIndex(Database.Instance.GetAll<EventData>().FirstOrDefault(x => x.Group == nowEvent.Group && x.EventId == nowEvent.Next[chooseResult]));
                        m_reset.Play();
                    }
                }

                if (nowEvent.Rewards != null && nowEvent.AutoReward)
                {
                    Rewards = DungeonManager.Instance.Dungeon.GetRewards(nowEvent.Rewards);
                    DungeonManager.Instance.Dungeon.GainReward(Rewards[chooseResult]);
                }
            }
            else
            {
                clickTcs = new TaskCompletionSource<bool>();
                await clickTcs.Task;
            }
            if (nowEvent.Rewards != null)
            {
                Rewards = DungeonManager.Instance.Dungeon.GetRewards(nowEvent.Rewards);
                if (!nowEvent.AutoReward)
                {
                    m_chooseReward.selectedIndex = 1;
                    freshReward();
                    rewardTcs = new TaskCompletionSource<bool>();
                    await rewardTcs.Task;
                    m_chooseReward.selectedIndex = 0;
                }
            }

            var nextEvent = Database.Instance.Get<EventData>(next);
            if (nextEvent == null || nextEvent.Group != nowEvent.Group) return;
            nowEventIndex = next;
            await showDialogue();
        }

        void doChoose(EventContext evt)
        {
            var c = evt.data as UI_Choice;
            var index = m_Chooices.GetChildIndex(c);
            if (nowChooseIndex == index)
                chooseTcs?.TrySetResult(index);
            else
                nowChooseIndex = index;
        }

        List<DungeonReward> Rewards;

        async void doReward(EventContext evt)
        {
            UI_Reward uiReward = evt.data as UI_Reward;
            if (uiReward.RewardData.Type == 1)//招募券的情况
            {
                var uiRecruit= UIManager.Instance.ChangeView<UI_Recruit>(UI_Recruit.URL);
                var result = await uiRecruit.Choose(DungeonManager.Instance.Dungeon.GetUnitList(uiReward.RewardData.Data));
                DungeonManager.Instance.Dungeon.GainUnit(Database.Instance.GetIndex(Database.Instance.GetAll<CardData>().First(x => x.units[0] == result)));
                UIManager.Instance.ChangeView<GComponent>(URL);
            }
            else
            {
                DungeonManager.Instance.Dungeon.GainReward(uiReward.RewardData);
            }
            Rewards.Remove(uiReward.RewardData);
            if (Rewards.Count == 0)
            {
                rewardTcs?.TrySetResult(true);
            }
            else
            {
                freshReward();
            }
        }

        void freshReward()
        {
            m_Rewards.RemoveChildrenToPool();
            foreach (var reward in Rewards)
            {
                var uiReward = m_Rewards.AddItemFromPool() as UI_Reward;
                uiReward.SetReward(reward);
            }
        }

        void setChoice()
        {
            nowChooseIndex = -1;
            m_Chooices.RemoveChildrenToPool();
            for (int i = 0; i < nowEvent.Choices.Length; i++)
            {
                var uiChoose = m_Chooices.AddItemFromPool() as UI_Choice;
                uiChoose.m_Description.text = nowEvent.Choices[i];
                uiChoose.title = nowEvent.ChoicesTitle[i];
            }
        }
    }
}
