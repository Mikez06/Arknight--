using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainUI
{
    partial class UI_MemberPage : IGameUIView
    {

        partial void Init()
        {
            m_back.onClick.Add(() =>
            {
                UIManager.Instance.ChangeView<UI_Main>(UI_Main.URL);
            });
        }

        public void Enter()
        {
            Flush();
        }

        public void Flush()
        {
            m_Cards.RemoveChildrenToPool();
            foreach (var card in GameData.Instance.Cards)
            {
                var uiCard = m_Cards.AddItemFromPool() as UI_HalfUnit;
                uiCard.SetCard(card);
            }
        }
    }
}

