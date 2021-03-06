using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonUI
{
    partial class UI_DungeonStart : IGameUIView
    {
        Dungeon Dungeon => DungeonManager.Instance.Dungeon;

        partial void Init()
        {
            m_back.onClick.Add(() =>
            {
                DungeonManager.Instance.Finish();
                UIManager.Instance.ChangeView<MainUI.UI_Main>(MainUI.UI_Main.URL);
            });
            m_ChooseWindow.m_Units.RemoveChildrenToPool();
            foreach (var cardData in Database.Instance.GetAll<CardData>())
            {
                var item = m_ChooseWindow.m_Units.AddItemFromPool() as UI_StartUnit;
                item.icon = Database.Instance.Get<UnitData>(cardData.units[0]).HeadIcon.ToHeadIcon();
                item.data = cardData;
                item.onClick.Add(() =>
                {
                    Dungeon.StartCard.CardId = cardData.Id;
                    freshChoose();
                });
            }
            m_StartUnit.onClick.Add(() => { m_choose.selectedIndex = 1;freshChoose(); });
            m_ChooseWindow.m_back.onClick.Add(() => { m_choose.selectedIndex = 0; Refresh(); });
            m_Start.onClick.Add(async () =>
            {
                await DungeonManager.Instance.StartDungeon();
            });
        }

        public void Enter()
        {
            Refresh();
        }

        public void Refresh()
        {
            m_StartUnit.icon = Dungeon.StartCard.UnitData.HeadIcon.ToHeadIcon();
        }

        public void freshChoose()
        {
            foreach (UI_StartUnit uiStartCard in m_ChooseWindow.m_Units.GetChildren())
            {
                uiStartCard.m_selected.selectedIndex = uiStartCard.data as CardData == Dungeon.StartCard.CardData ? 1 : 0;
            }
        }
    }
}
