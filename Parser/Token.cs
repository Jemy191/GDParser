namespace GDParser;

 class Token
 {
     public int Start { get; }
     public int End { get; }
     
     public Token(int start, int end)
     {
         this.Start = start;
         this.End = end;
     }
 }