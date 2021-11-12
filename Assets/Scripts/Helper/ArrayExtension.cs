using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class ArrayExtension
{
    public static T Get<T>(this T[] self, int index)
    {
        if (index < 0 || index >= self.Length)
        {
            return default(T);
        }
        return self[index];
    }
    public static T Get<T>(this T[,] self, int index0,int index1)
    {
        if (index0 < 0 || index0 >= self.GetLength(0) || index1<0 || index1>=self.GetLength(1))
        {
            return default(T);
        }
        return self[index0, index1];
    }

    public static T Get<T>(this T[][] self,int index0,int index1)
    {
        if (index0 < 0 || index0 >= self.GetLength(0) || index1 < 0 || index1 >= self[index0].GetLength(0))
        {
            return default(T);
        }
        return self[index0][ index1];
    }
}