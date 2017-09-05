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
                //check branch and pull request with error
            }
            return 4; //it's error
        }
        internal int forTestData() => 3;
    }
}
