using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CountDown
{
    public float value;

    public CountDown(float value = 0)
    {
        this.value = value;
    }

    public bool Update(float time)
    {
        if (value > 0)
        {
            value -= time;
            if (value <= 0)
            {
                Finish();
                return true;
            }
        }
        return false;
    }
    public void Set(float value)
    {
        this.value = value;
    }
    public void Finish()
    {
        value = 0;
    }

    public bool Finished()
    {
        return value <= 0;
    }

    public bool Infinity()
    {
        return float.IsInfinity(value);
    }
}
