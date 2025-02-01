using System.Security.Cryptography;

namespace SharedKernel;

public static class GuidV7
{
    public static Guid NewGuid()
    {
        // Get the current timestamp in milliseconds since Unix epoch
        long unixTimeMillis = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        // Convert the timestamp to bytes
        byte[] timestampBytes = BitConverter.GetBytes(unixTimeMillis);
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(timestampBytes);
        }

        // Prepare the byte array for the GUID
        byte[] guidBytes = new byte[16];

        // Fill the first 6 bytes with the timestamp (48 bits)
        Array.Copy(timestampBytes, 2, guidBytes, 0, 6);

        // Generate 10 random bytes
        byte[] randomBytes = new byte[10];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomBytes);
        }

        // Fill the remaining bytes with randomness
        Array.Copy(randomBytes, 0, guidBytes, 6, 10);

        // Set the version (v7)
        guidBytes[6] &= 0x0F;
        guidBytes[6] |= 0x70;

        // Set the variant
        guidBytes[8] &= 0x3F;
        guidBytes[8] |= 0x80;

        return new Guid(guidBytes);
    }
}