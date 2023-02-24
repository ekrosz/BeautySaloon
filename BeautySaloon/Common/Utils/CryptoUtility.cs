using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace BeautySaloon.Common.Utils;
public static class CryptoUtility
{
    private const string Format = "X2";

    private const int IvLength = 16;

    public static string GetEncryptedPassword(string password)
    {
        using var hash = SHA512.Create();

        return string.Concat(hash.ComputeHash(Encoding.UTF8.GetBytes(password)).Select(x => x.ToString(Format)));
    }

    public static byte[] EncryptData<T>(T data, byte[] secretKey) where T : struct
    {
        var encriptingData = ToByteArray(data);

        using var aes = Aes.Create();
        aes.Key = secretKey;
        aes.GenerateIV();

        return aes.IV.Concat(aes.EncryptCbc(encriptingData, aes.IV)).ToArray();
    }

    public static T DecryptData<T>(byte[] data, byte[] secretKey) where T : struct
    {
        var iv = data[..IvLength];
        var decriptingData = data[IvLength..];

        using var aes = Aes.Create();
        aes.Key = secretKey;

        var decriptedData = aes.DecryptCbc(decriptingData, iv);

        return ToStruct<T>(decriptedData);
    }


    private static byte[] ToByteArray<T>(T value) where T : struct
    {
        var size = Marshal.SizeOf(value);
        var byteArray = new byte[size];
        var ptr = Marshal.AllocHGlobal(size);

        Marshal.StructureToPtr(value, ptr, true);
        Marshal.Copy(ptr, byteArray, 0, size);
        Marshal.FreeHGlobal(ptr);

        return byteArray;
    }

    private static T ToStruct<T>(byte[] byteArrayValue) where T : struct
    {
        var size = Marshal.SizeOf<T>();
        var ptr2 = Marshal.AllocHGlobal(size);

        try
        {
            Marshal.Copy(byteArrayValue, 0, ptr2, size);

            return (T)Marshal.PtrToStructure(ptr2, typeof(T))!;
        }
        finally
        {
            Marshal.FreeHGlobal(ptr2);
        }
    }
}
