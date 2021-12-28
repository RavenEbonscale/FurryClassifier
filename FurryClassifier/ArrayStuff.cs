using System;


namespace ArryStuff
{
    static public class ArryStuff
    {
        static public int ArgsMax(this float[] Arry)
        {
            float? maxVal = null; //nullable so this works even if you have all super-low negatives
            int index = -1;
            for (int i = 0; i < Arry.Length; i++)
            {
                float thisNum = Arry[i];
                if (!maxVal.HasValue || thisNum > maxVal.Value)
                {
                    maxVal = thisNum;
                    index = i;
                }

            }


            return index;

        }



    }
}
