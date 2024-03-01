namespace QuickTGA
{
    internal class QuickTGA
    {
        // Quick color manipulation for TGA files

     
        public static void editColor(Stream newStream, Func<int[],int[]> colorReplaceMethod)
        {
            int old_b = newStream.ReadByte();
            int old_g = newStream.ReadByte();
            int old_r = newStream.ReadByte();
            int old_a = newStream.ReadByte();
            int[] newData = colorReplaceMethod.Invoke(new int[4] { old_r, old_g, old_b, old_a });
            newStream.Position -= 4;
            newStream.WriteByte((byte)newData[2]); // b
            newStream.WriteByte((byte)newData[1]); // g
            newStream.WriteByte((byte)newData[0]); // r
            newStream.WriteByte((byte)newData[3]); // a
        }
        public static void editColorMap1(Stream newStream, Func<int[], int[]> colorReplaceMethod)
        {
            newStream.Position = 5;

            byte[] buf2 = new byte[2];
            newStream.Read(buf2);
            short ColorMapLength = BitConverter.ToInt16(buf2);

            newStream.Position = 18;

            for (int i = 0; i < ColorMapLength; i++)
            {
                editColor(newStream, colorReplaceMethod);
            }
        }
        public static void editColorMap0(Stream newStream, Func<int[], int[]> colorReplaceMethod)
        {
            newStream.Position = 12;
            byte[] buf2 = new byte[2];

            newStream.Read(buf2);
            short width = BitConverter.ToInt16(buf2);

            newStream.Read(buf2);
            short height = BitConverter.ToInt16(buf2);

            newStream.Position = 18;

            for (int i =0; i < width; i++)
            {
                for (int y=0; y < height; y++)
                {
                    editColor(newStream, colorReplaceMethod);
                }
            }
        }
        public static Stream changeColours(Stream tgaStream, Func<int[], int[]> colorReplaceMethod)
        {
            Stream newStream = new MemoryStream();
            tgaStream.Position = 0;
            newStream.Position = 0;
            tgaStream.CopyTo(newStream);

            newStream.Position = 1;
            int colormaptype = newStream.ReadByte();
            if (colormaptype != 0)
            {
                editColorMap1(newStream, colorReplaceMethod);
            }
            else
            {
                editColorMap0(newStream, colorReplaceMethod);
            }
            newStream.Position = 0;

            return newStream;
        }
    }
}
