using System;

namespace iskkonekb.kuvera.model
{
    public class SampleClass
    {

        public int publicData() {
            if (DateTime.Now.Second < -1)
            {
                //shows not covered line 
                // force trigger
                return 1;
            }
            return 2;
        }
        internal int forTestData() => 3;
    }
}
