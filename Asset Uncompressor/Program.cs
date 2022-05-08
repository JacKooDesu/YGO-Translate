using Ionic.Zlib;
using Ionic.Zip;

class Program
{
    static void Main(string[] args)
    {
        //byte[] data = File.ReadAllBytes(".\\files\\test");
        //byte[] ch = new byte[data.Length];
        //byte[] v1 = new byte[data.Length];
        //int iter = 0;
        //foreach(byte b in data)
        //{
        //    ch[iter] = (byte)(b ^ (0xB3 * ((iter + 0x2f0) % 0x800000FF)));
        //    v1[iter] = (byte)(ch[iter] ^ (iter % 0x7));
        //    iter++;
        //}


        //File.WriteAllBytes(".\\files\\output.txt", ZlibStream.UncompressBuffer(v1));

        byte[] data = File.ReadAllBytes(".\\files\\modify");
        Console.WriteLine(data.Length);
        var after = CompressBuffer(data);
        File.WriteAllBytes(".\\files\\compressed", after);
    }

    static byte[] CompressBuffer(byte[] b)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            using(ZlibStream zs = new ZlibStream(ms, CompressionMode.Compress, CompressionLevel.Level6, true))
            {
                zs.Write(b,0,(int)b.Length);
            }
            return ms.ToArray();
        }
    }
}