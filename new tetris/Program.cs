// See https://aka.ms/new-console-template for more information
void Print(int[,] field)
{
    Console.Clear();
    for (int i = 0; i < field.GetLength(0); i++)
    {
        for (int j = 0; j < field.GetLength(1); j++)
        {
            if (field[i, j] == 0)  Console.Write("  |");
            else Console.Write("X |");
        }
        Console.WriteLine();
    }
    Console.WriteLine();
}
int[,] NewFigure ()
{
    int[,] line = new int[1, 4] {{1, 1, 1, 1}};
    int[,] tank = new int[2, 3] {{0, 1, 0}, {1, 1, 1}};
    int[,] zLeft = new int[2, 3] {{1, 1, 0},{0, 1, 1}};
    int[,] zRight = new int[2, 3] {{0, 1, 1},{1, 1, 0}};
    int[,] horseLeft = new int[2, 3] {{1, 1, 1},{0, 0, 1}};
    int[,] horseRight = new int[2, 3] {{1, 1, 1},{1, 0, 0}};
    int[,] square = new int[2, 2] {{1, 1},{1, 1}};
    int NewFigure = new Random().Next (0, 7);
    if (NewFigure == 0 ) return line;
    if (NewFigure == 1 ) return tank;
    if (NewFigure == 2 ) return zLeft;
    if (NewFigure == 3 ) return zRight;
    if (NewFigure == 4 ) return horseLeft;
    if (NewFigure == 5 ) return horseRight;
    return square;
}
void ReplaceFigure(int[,] field, int[,] figure, int kordI, int kordJ, bool add)
{
    for (int i = 0; i < figure.GetLength(0); i++)
        for (int j = 0; j < figure.GetLength(1); j++)
            if (figure[i, j] != 0) if(add) field[kordI + i, kordJ + j] = figure[i, j];
                                    else field[kordI + i, kordJ + j] = 0;
}
bool CheckCollision(int[,] field, int[,] figure, int kordI, int kordJ)
{
    for (int i = 0; i < figure.GetLength(0); i++)
        for (int j = 0; j < figure.GetLength(1) ; j++)
            if (figure[i, j] == 1) 
                if (field[kordI + i , kordJ + j ] == 1) return false;
    return true;
}
int[,] Rotation(int[,] figure)
{
    int[,] rotationFigure = new int[figure.GetLength(1), figure.GetLength(0)];
    for (int i = 0; i < figure.GetLength(1); i++)
        for (int j = 0; j < figure.GetLength(0)  ; j++)
            rotationFigure[ i, j ] = figure [ figure.GetLength(0) - (j + 1),  i  ];
    return rotationFigure;
}
int CheckLine (int[,] field)
{
    for( int i = field.GetLength(0) - 2; i > 0; i--)
        for( int j = 1; j < field.GetLength(1) - 1; j++)    
        {
            if (field[i, j] == 0) break;
            if (j == field.GetLength(1) - 2 && field[i, j] == 1)  return i;
        }
    return -1;
}
void DeleteLine(int[,] field, int kordLine)
{
    for( int i = kordLine; i > 0; i --)
        for(int j = 1; j < field.GetLength(1) - 1; j++)
            field[i, j] = field[i - 1, j];
    for( int j = 1; j < field.GetLength(1) - 1; j++)
        field[0, j] = 0;
}
int[,] fieldGame = new int [21, 12];
for (int l = 0; l < fieldGame.GetLength(0); l++)
    for (int k = 0; k < fieldGame.GetLength(1); k++)
        if (k == 0 || l == fieldGame.GetLength(0) - 1 
                || k == fieldGame.GetLength(1) - 1 )  
                fieldGame[l, k] = 1;
int i = 0, j = 5;
bool test, end = false, testNewFigure = true;
int[,] newFigure = new int[2,4];
new Thread(() =>
{
    while(true)
    {
        if (testNewFigure == true)
        {
        i = 0;
        j = 5;
        newFigure =  NewFigure();
        testNewFigure = false;
        test = CheckCollision(fieldGame, newFigure, i, j);
        if (test == false) {Console.WriteLine("Game Over"); break;}
        }
        ReplaceFigure(fieldGame, newFigure, i , j, true);
        Print(fieldGame);
        ReplaceFigure(fieldGame, newFigure, i, j, false); 
        i++;
        test = CheckCollision(fieldGame, newFigure, i + 1, j);
        if (test == false)
        {
                testNewFigure = true;
                ReplaceFigure(fieldGame, newFigure, i , j, true);
                int line;
                while((line = CheckLine (fieldGame)) != -1) 
                    DeleteLine(fieldGame, line);
        }
        Thread.Sleep(500);
    }
}).Start();
while(true)
{
    if (end) break;
    var key = Console.ReadKey(true).Key;
    if(key == ConsoleKey.LeftArrow 
        && (test = CheckCollision(fieldGame, newFigure, i, j - 1)) == true) j-=1; 
    if(key == ConsoleKey.RightArrow 
        && (test = CheckCollision(fieldGame, newFigure, i, j + 1)) == true) j+=1;
    if(key == ConsoleKey.Spacebar)
    {
        int[,] testFigure = Rotation(newFigure);
        if ((test = CheckCollision(fieldGame, testFigure, i, j)) == true)
        newFigure = testFigure;
    }
    if(key == ConsoleKey.DownArrow) 
    {
        while (CheckCollision(fieldGame, newFigure, i, j)) i++;
        ReplaceFigure(fieldGame, newFigure, i - 1, j, true);
        int line;
        while((line = CheckLine (fieldGame)) != -1) 
            DeleteLine(fieldGame, line);
        testNewFigure = true;
    }
}