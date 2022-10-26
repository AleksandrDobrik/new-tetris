// See https://aka.ms/new-console-template for more information
bool test;
int[,] fieldGame = new int [21, 12];
for (int i = 0; i < fieldGame.GetLength(0); i++)
    for (int j = 0; j < fieldGame.GetLength(1); j++)
        if (j == 0 || i == fieldGame.GetLength(0) - 1 || j == fieldGame.GetLength(1) - 1 )  fieldGame[i, j] = 1;
void Print(int[,] field)
{
    Console.Clear();
    for (int i = 0; i < field.GetLength(0); i++)
    {
        for (int j = 0; j < field.GetLength(1); j++)
        {
            if (field[i, j] == 0)  Console.Write("  |");
            else Console.Write($"{field[i, j]} |");
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
void AddFigure(int[,] field, int[,] figure, int kordI, int kordJ)
{
    for (int i = 0; i < figure.GetLength(0); i++)
        for (int j = 0; j < figure.GetLength(1); j++)
            if (figure[i, j] != 0) field[kordI + i, kordJ + j] = figure[i, j];
}
void DeleteFigure(int[,] field, int[,] figure, int kordI, int kordJ)
{
    for (int i = 0; i < figure.GetLength(0); i++)
        for (int j = 0; j < figure.GetLength(1); j++)
            if (figure[i, j] != 0) field[kordI + i, kordJ + j] = 0;
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
void Game (int[,] field)
{
    while(true)
    {
        int i = 0, j = 5;
        int[,] newFigure =  NewFigure();
        test = CheckCollision(field, newFigure, i, j);
        if (test == false) { Console.WriteLine("Game over");return;}
        AddFigure(field, newFigure, i, j);
        Print(field);
        while(true)
        {
            DeleteFigure(field, newFigure, i, j);
            var key = Console.ReadKey(true).Key;
            if(key == ConsoleKey.LeftArrow && (test = CheckCollision(field, newFigure, i, j - 1)) == true) j-=1; 
            if(key == ConsoleKey.RightArrow && (test = CheckCollision(field, newFigure, i, j + 1)) == true) j+=1;
            if(key == ConsoleKey.Spacebar)
            {
                int[,] testFigure = Rotation(newFigure);
                if ((test = CheckCollision(field, testFigure, i, j)) == true)
                newFigure = testFigure;
            }
            if(key == ConsoleKey.DownArrow) 
            if (test = CheckCollision(field, newFigure, i + 1, j) == true) i+=1;
            else 
            {
                AddFigure(field, newFigure, i, j);
                int line;
                while((line = CheckLine (field)) != -1) 
                    DeleteLine(field, line);
                break;
            }           
            AddFigure(field, newFigure, i, j);            
            Print(field);
        }
    }
}
Game(fieldGame);