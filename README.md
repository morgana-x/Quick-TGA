# Quick-TGA
A small utility for rapid editing of TGA file's colours, 

enables you to supply a Method that 
+ takes an array of 4 integers (R,G,B,A)
+ returns a new array of integers (R,G,B,A)

Example of inversing an image's colors (including alpha channel!)
```cs
      public static int[] example(int[] inp)
      {
          return inp.Reverse().ToArray();
      }
...
     Stream stream = new FileStream(path, FileMode.Open);
     QuickTGA.QuickTGA.editTGA(stream, example);
```
