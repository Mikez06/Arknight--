using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class IconHelper
{
    public static string ToHeadIcon(this string icon)
    {
        return "ui://Res/" + icon;
    }
    public static string ToSkillIcon(this string icon)
    {
        return "ui://SkillIcon/" + icon;
    }
    public static string ToContractIcon(this string icon)
    {
        return "ui://Res/" + icon;
    }

    public static string ToRelicIcon(this string icon)
    {
        return "ui://Res/" + icon;
    }
}
