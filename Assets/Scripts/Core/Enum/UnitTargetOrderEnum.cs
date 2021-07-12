using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum AttackTargetOrderEnum
{
    终点距离,
    血量升序,
    血量降序, 
    血量比例升序,
    血量比例降序,

    放置降序,
    区域顺序,
    防御降序,
    防御升序,
    自身距离升序,

    自身距离降序,
    随机,
    攻击力升序,
    攻击力降序,
    最大血量降序,

    最大血量升序,
    血量未满随机,
    重量降序,
    重量升序,

    未阻挡优先,
    隐身优先,
    未眩晕优先,
    飞行优先,
    沉睡优先,
    无抵抗优先,
}

public enum AttackTargetOrder2Enum
{
    飞行,
    远程,
    Tag,
    Buff,
}