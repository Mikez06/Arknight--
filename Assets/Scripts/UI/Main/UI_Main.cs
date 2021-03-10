﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FairyGUI;
using UnityEngine;

namespace MainUI
{
    partial class UI_Main : IGameUIView
    {
        GameData gameData => GameData.Instance;
        partial void Init()
        {
            m_member.onClick.Add(() =>
            {
                UIManager.Instance.ChangeView<UI_MemberPage>(UI_MemberPage.URL);
            });
        }

        public void Enter()
        {
            Flush();
        }

        public void Flush()
        {
            string picName = Database.Instance.Get<UnitConfig>(gameData.MainPageUnitId).StandPic;
            ResourcesManager.Instance.LoadBundle(PathHelper.SpritePath + picName);
            m_standPic.texture = new NTexture(ResourcesManager.Instance.GetAsset<Texture>(PathHelper.SpritePath + picName, picName));
        }
    }
}