﻿using System;
using System.Security.Cryptography;

namespace Nels.SemanticKernel.InternalUtilities;

/* This code is taken from jhtodd/SequentialGuid https://github.com/jhtodd/SequentialGuid/blob/master/SequentialGuid/Classes/SequentialGuid.cs */
/// <summary>
/// Implements <see cref="IGuidGenerator"/> by creating sequential Guids.
/// Use <see cref="AbpSequentialGuidGeneratorOptions"/> to configure.
/// </summary>
public class SequentialGuidGenerator
{
    private static readonly RandomNumberGenerator RandomNumberGenerator = RandomNumberGenerator.Create();
    public static Guid Create()
    {
        // We start with 16 bytes of cryptographically strong random data.
        var randomBytes = new byte[10];
        RandomNumberGenerator.GetBytes(randomBytes);

        // An alternate method: use a normally-created GUID to get our initial
        // random data:
        // byte[] randomBytes = Guid.NewGuid().ToByteArray();
        // This is faster than using RNGCryptoServiceProvider, but I don't
        // recommend it because the .NET Framework makes no guarantee of the
        // randomness of GUID data, and future versions (or different
        // implementations like Mono) might use a different method.

        // Now we have the random basis for our GUID.  Next, we need to
        // create the six-byte block which will be our timestamp.

        // We start with the number of milliseconds that have elapsed since
        // DateTime.MinValue.  This will form the timestamp.  There's no use
        // being more specific than milliseconds, since DateTime.Now has
        // limited resolution.

        // Using millisecond resolution for our 48-bit timestamp gives us
        // about 5900 years before the timestamp overflows and cycles.
        // Hopefully this should be sufficient for most purposes. :)
        long timestamp = DateTime.UtcNow.Ticks / 10000L;

        // Then get the bytes
        byte[] timestampBytes = BitConverter.GetBytes(timestamp);

        // Since we're converting from an Int64, we have to reverse on
        // little-endian systems.
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(timestampBytes);
        }

        byte[] guidBytes = new byte[16];

        Buffer.BlockCopy(randomBytes, 0, guidBytes, 0, 10);
        Buffer.BlockCopy(timestampBytes, 2, guidBytes, 10, 6);

        return new Guid(guidBytes);
    }
}
