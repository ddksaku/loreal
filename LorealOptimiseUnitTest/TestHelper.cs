using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LorealOptimiseUnitTest
{
    public class TestHelper
    {
        public static bool GetRandomBool()
        {
            Random r = new Random(DateTime.Now.Millisecond * DateTime.Now.Second);

            return r.Next(10) < 5;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="max">Upper exclusive bound</param>
        /// <returns></returns>
        public static int GetRandomNumber(int max)
        {
            Random r = new Random(DateTime.Now.Millisecond * DateTime.Now.Second);

            return r.Next(max);
        }
    }
}
