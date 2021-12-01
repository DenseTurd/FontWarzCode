using System;
using UnityEngine;

public static class Rand
{
    /*
    This is  Squirrel Eiserloh's A* noise function SquirrelNoise (v3).
    Kind of like looking up a value in an infinitely large (non-existant) table of previously generated random numbers.

    The bit noise constants were crafted to have distinctive and interesting bits.
    */
    public static uint SquirrelNoise(int positionX, uint seed)
    {
        const int BIT_NOISE1 = 0x68E31DA4; //0b0110 1000 1110 0011 0001 1101 1010 0100
        const uint BIT_NOISE2 = 0xB5297A4D; //0b1011 0101 0010 1001 0111 1010 0100 1101
        const uint BIT_NOISE3 = 0x1B56C4E9; //0b0001 1011 0101 0110 1100 0100 1110 1001

        uint mangledBits = (uint) positionX;
        mangledBits *= BIT_NOISE1;
        mangledBits += seed;
        mangledBits ^= (mangledBits >> 8);
        mangledBits += BIT_NOISE2;
        mangledBits ^= (mangledBits << 8);
        mangledBits *= BIT_NOISE3;
        mangledBits ^= (mangledBits >> 8);
        return mangledBits;
    }
    /*
     * max value for an 8 digit hexadecimal no. (0xffffffff) is: 4294967295.
     * 
    Prefixing with 0x makes it hexadecimal
    (mangledBits >> 8) This is the bit shift operator it literally moves the bits of the binary number left or right.

    Foud it! the ^= is the bitwise XOR assignment operator, comparing both sides and assigning the new value to the left side.
    Thats cleva!

    Lets use it with some scaling :D
    */

    public static uint defaultSeed = 0;

    public static float Om() // can do 60 million random values, can expand almost arbitrarily with some more time calculations
    {
        if (defaultSeed == 0)
        {
            defaultSeed = (uint)(DateTime.Now.Millisecond);
            Debug.Log("Getting random-ish seed");
        }
        else
        {
            defaultSeed++;
        }

        double t = Time.timeAsDouble;
        //int secs = (int)t;
        int micros = (int)((t * 1000000));

        uint uInty = SquirrelNoise(micros, defaultSeed);
        float f = (float)uInty / (float)0xffffffff; // it says the casts are redundant but it don't work without them.

        return f;
    }

    public static float Om(int positionX, int seed) // if you can provide values that are faster than calling Time.timeAsDouble then feel free :)
    {
        uint uInty = SquirrelNoise(positionX, (uint)seed);
        float f = (float)uInty / (float)0xffffffff; // it says the casts are redundant but it don't work without them.

        return f;
    }

    public static float Range(float min, float max)
    {
        float span = max - min;
        float scaled = Om() * span;
        float shifted = scaled + min;
        return shifted;
    }

    public static float Range(float min, float max, int positionX, int seed)
    {
        float span = max - min;
        float scaled = Om(positionX, seed) * span;
        float shifted = scaled + min;
        return shifted;
    }

    /// <summary>
    /// Min inclusive, Max exclusive
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static int Range(int min, int max) // Super hacky, especially considering squirrelNoise takes ints in the first place
    {
        float span = (max - float.Epsilon) - min;
        float scaled = Om() * span;
        float shifted = scaled + min;
        float rounded = shifted - (shifted % 1);
        //Debug.Log($"Min: {min}, Max: {max}, Val: {rounded}");
        return (int)rounded;
    }
}
