using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chess
{
    class Program
    {


        static void Main(string[] args)
        {
            gameOn gameOn = new gameOn();
            gameOn.newGmae();
        }
    }




    class gameOn
    {
        public static bool iswin = false;
        public static bool whiteTurn = false;
        public static int countInsignificantMoveWhite;
        public static int countInsignificantMoveBlack;
        public static string positionOfPlayerCheck;
        public static string typeOfPlayerCheck;
        public static string striptOfCeck;
        public static string[] positionsAllGame;


        public void newGmae()
        {
            Players[] allPlayers;
            string[,] board;
            (board, allPlayers) = startgame();
            char colorTurn;

            while (!gameOn.iswin)
            {
                // בודק תור מי
                colorTurn = whoTurn();
                Console.WriteLine((colorTurn == 'W' ? "White player turn" : "Black player turn"));

                // ביצוע מהלך תור
                board = playerTurn(board, colorTurn, allPlayers);

                // הדפסת המגרש לאחר התור
                print(board);

                // האם יש מספיק שחקנים
                if ((!gameOn.isEnoughPlayersToWin(allPlayers)))
                {
                    Console.WriteLine("Draw !!");
                    Console.WriteLine("Not enough soldiers");
                    gameOn.gameOver();
                }


                //בודק האם יש שח
                if (gameOn.ischeck(allPlayers, board, colorTurn == 'W' ? 'B' : 'W', "EndTurn"))
                {

                    // בודק האם יש שחמט
                    if (!(gameOn.isCheckmate(allPlayers, board, colorTurn == 'W' ? 'B' : 'W')))
                    {
                        bool checkOnBlack = Melech.checkOnBlack;
                        Console.WriteLine(checkOnBlack ? "Black king has been checked" : "White king has been checked");
                    }
                    else
                    {
                        Console.WriteLine("Checkmate!");
                        Console.WriteLine("The {0} king can't move to a safe place", colorTurn);
                        Console.WriteLine(colorTurn == 'W' ? "White player win!" : "Black player win!");
                        gameOn.gameOver();
                    }
                }
            }


            Console.ReadLine();

        }

        //פונקציה של התחלת משחק חדש
        public (string[,], Players[]) startgame()
        {
            Players[] players = new Players[32];
            string[,] board =
                     { {" ", "  A", "  B", "  C", "  D", "  E", "  F", "  G", "  H" },
                               {"1", " BR", " BK", " BB", " BQ", " BM", " BB", " BK", " BR" },
                               {"2", " BP", " BP", " BP", " BP", " BP", " BP", " BP", " BP" },
                               {"3", " EE", " EE", " EE", " EE", " EE", " EE", " EE", " EE" },
                               {"4", " EE", " EE", " EE", " EE", " EE", " EE", " EE", " EE" },
                               {"5", " EE", " EE", " EE", " EE", " EE", " EE", " EE", " EE" },
                               {"6", " EE", " EE", " EE", " EE", " EE", " EE", " EE", " EE" },
                               {"7", " WP", " WP", " WP", " WP", " WP", " WP", " WP", " WP" },
                               {"8", " WR", " WK", " WB", " WQ", " WM", " WB", " WK", " WR" }};
            Console.WriteLine("White start !");


            players[0] = new Pawn(" WP", "71");
            players[1] = new Pawn(" WP", "72");
            players[2] = new Pawn(" WP", "73");
            players[3] = new Pawn(" WP", "74");
            players[4] = new Pawn(" WP", "75");
            players[5] = new Pawn(" WP", "76");
            players[6] = new Pawn(" WP", "77");
            players[7] = new Pawn(" WP", "78");
            players[8] = new Pawn(" BP", "21");
            players[9] = new Pawn(" BP", "22");
            players[10] = new Pawn(" BP", "23");
            players[11] = new Pawn(" BP", "24");
            players[12] = new Pawn(" BP", "25");
            players[13] = new Pawn(" BP", "26");
            players[14] = new Pawn(" BP", "27");
            players[15] = new Pawn(" BP", "28");

            players[16] = new Rook(" WR", "81");
            players[17] = new Rook(" WR", "88");
            players[18] = new Rook(" BR", "11");
            players[19] = new Rook(" BR", "18");

            players[20] = new Knight(" WK", "82");
            players[21] = new Knight(" WK", "87");
            players[22] = new Knight(" BK", "12");
            players[23] = new Knight(" BK", "17");

            players[24] = new Bishop(" WB", "83");
            players[25] = new Bishop(" WB", "86");
            players[26] = new Bishop(" BB", "13");
            players[27] = new Bishop(" BB", "16");

            players[28] = new Queen(" WQ", "84");
            players[29] = new Queen(" BQ", "14");
            players[30] = new Melech(" WM", "85");
            players[31] = new Melech(" BM", "15");



            positionsAllGame = new string[] { "" };
            for (int i = 0; i < 32; i++)
                positionsAllGame[0] += players[i].currentPosition;



            print(board);

            return (board, players);
        }



        // הדפסת לוח המשחק
        public void print(string[,] board)
        {
            for (int r = 0; r < 9; r++)
            {
                for (int c = 0; c < 9; c++)
                {
                    Console.Write(board[r, c] + " ");
                }
                Console.WriteLine();
            }
        }


        // בודק תור מי
        public char whoTurn()
        {
            whiteTurn = !(whiteTurn);
            return whiteTurn ? 'W' : 'B';
        }

        // תור שחקן הכולל את כל התור
        public string[,] playerTurn(string[,] board, char colorTurn, Players[] allPlayers)
        {

            bool valid = false;
            string[] move = new string[2];
            while (!valid)
            {
                // בחירת השחקן שאותו אני רוצה להזיז
                Console.WriteLine("1) Please enter the position of the soldier you want to move");
                move[0] = Console.ReadLine().Trim();
                if (move[0].Length != 2)
                {
                    errorMassege();
                    continue;
                }

                move[0] = charToNum(move[0], board);
                if (move[0] == "ff")
                    continue;


                valid = basicValidation(move[0], board, 'a', colorTurn);

                if (!valid)
                {
                    errorMassege();
                    continue;
                }

                valid = false;

                // בחירת המיקום שאליו אני רוצה להעביר את השחקן
                Console.WriteLine("2) Please enter the position that you want to move to");
                move[1] = Console.ReadLine().Trim();
                if (move[1].Length != 2)
                {
                    errorMassege();
                    continue;
                }
                move[1] = charToNum(move[1], board);
                if (move[1] == "ff")
                    continue;


                valid = basicValidation(move[1], board, 'b', colorTurn);

                if (move[0] == move[1])
                    valid = false;
                if (!valid)
                {
                    errorMassege();
                    continue;
                }

                Players player = new Players();
                for (int i = 0; i < 32; i++)
                {

                    if (allPlayers[i].currentPosition == move[0])
                    {
                        player = allPlayers[i];
                        break;
                    }
                }

                // בדיקת סוג השחקן שנבחר וקריאה לקלאס שלו למען הזזתו
                switch (board[int.Parse(move[0][0] + ""), int.Parse(move[0][1] + "")])
                {
                    case " WR"://Rook
                        (board, valid) = Rook.move(move, board, colorTurn, player, allPlayers);
                        break;
                    case " BR":
                        (board, valid) = Rook.move(move, board, colorTurn, player, allPlayers);
                        break;
                    case " WK"://Knight
                        (board, valid) = Knight.move(move, board, colorTurn, player, allPlayers);
                        break;
                    case " BK":
                        (board, valid) = Knight.move(move, board, colorTurn, player, allPlayers);
                        break;
                    case " WB"://Bishop
                        (board, valid) = Bishop.move(move, board, colorTurn, player, allPlayers);
                        break;
                    case " BB":
                        (board, valid) = Bishop.move(move, board, colorTurn, player, allPlayers);
                        break;
                    case " WP"://Pwan
                        (board, valid) = Pawn.move(move, board, true, player, allPlayers);
                        break;
                    case " BP":
                        (board, valid) = Pawn.move(move, board, false, player, allPlayers);
                        break;
                    case " WQ"://Queen
                        (board, valid) = Queen.move(move, board, colorTurn, player, allPlayers);
                        break;
                    case " BQ":
                        (board, valid) = Queen.move(move, board, colorTurn, player, allPlayers);
                        break;
                    case " WM"://Melech
                        (board, valid) = Melech.move(move, board, colorTurn, player, allPlayers);
                        break;
                    case " BM":
                        (board, valid) = Melech.move(move, board, colorTurn, player, allPlayers);
                        break;
                    default:
                        valid = false;
                        break;
                }
                if (valid == false)
                    errorMassege();
            }
            return board;
        }


        // בדיקת וולידציה בסיסית של בחירת השחקן
        public bool basicValidation(string move, string[,] board, char fromto, char whosTurn)
        {
            // בודק שמספר השורה תקינה
            string validNums = "12345678 ";

            for (int i = 0; i <= validNums.Length; i++)
            {
                if (i == validNums.Length)
                    return false;
                if (move[0] == validNums[i])
                    break;
            }
            int row = int.Parse(move[0] + "");
            int col = int.Parse(move[1] + "");
            char rivelColor = board[row, col][1];


            // בודק שהשחקן משחק בתורו
            if (fromto == 'a' && whosTurn != rivelColor)
            {
                Console.WriteLine((whosTurn == 'W' ? "White player turn" : "Black player turn"));
                return false;
            }



            // בודק שמספר הטור תקין
            for (int i = 0; i <= validNums.Length; i++)
            {
                if (i == validNums.Length)
                    return false;
                if (col == int.Parse(validNums[i] + ""))
                    break;
            }

            // בודק שלא נבחרה משבצת ריקה
            if (fromto == 'a' && board[row, col] == " EE")
                return false;

            // כאשר הכל תקין מחזיר שתקין
            return true;
        }

        // הופך את האות של הטור למספר
        public string charToNum(string move, string[,] board)
        {
            switch (move[1])
            {
                case 'a':
                case 'A':
                    return move = move[0] + "1";
                case 'b':
                case 'B':
                    return move = move[0] + "2";
                case 'c':
                case 'C':
                    return move = move[0] + "3";
                case 'd':
                case 'D':
                    return move = move[0] + "4";
                case 'e':
                case 'E':
                    return move = move[0] + "5";
                case 'f':
                case 'F':
                    return move = move[0] + "6";
                case 'g':
                case 'G':
                    return move = move[0] + "7";
                case 'h':
                case 'H':
                    return move = move[0] + "8";
                default:
                    errorMassege();
                    return "ff";

            }
            return move;
        }



        //שגיאה שמופיעה כאשר מהלך שנבחר אינו חוקי
        public static void errorMassege()
        {
            Console.WriteLine("The move you made is invalid");
        }


        // ברגע שמישהו ניצח פונקציה זו מסיימת את המשחק
        public static void gameOver()
        {
            Console.WriteLine("Game over !!");
            iswin = true;
        }



        //בודק האם הצעד היה משמעותי או לא
        public static void insignificantMove(char colorTurn, int Trow, int Tcol, string[,] board)
        {
            if (board[Trow, Tcol] != " EE")
            {
                if (colorTurn == 'W')
                    gameOn.countInsignificantMoveWhite = 0;
                else
                    gameOn.countInsignificantMoveBlack = 0;
            }
            else
            {
                if (colorTurn == 'W')
                    gameOn.countInsignificantMoveWhite++;
                else
                    gameOn.countInsignificantMoveBlack++;
            }
        }


        //  בסוף תור בודק האם יש שח
        public static bool ischeck(Players[] allPlayers, string[,] board, char color)
        {
            return ischeck(allPlayers, board, color, "00");
        }


        // בודק אם יש שח בנקודה ספציפית
        public static bool ischeck(Players[] allPlayers, string[,] board, char colorTurn, string kingMove)
        {
            int col, row;
            string W_King = allPlayers[30].currentPosition;
            string B_King = allPlayers[31].currentPosition;
            char rivalcolor;
            rivalcolor = (colorTurn == 'W' ? 'B' : 'W');


            if (kingMove == "00" || kingMove == "EndTurn")
            {
                row = int.Parse(colorTurn == 'W' ? W_King[0] + "" : B_King[0] + "");
                col = int.Parse(colorTurn == 'W' ? W_King[1] + "" : B_King[1] + "");
            }
            else
            {
                row = int.Parse(kingMove[0] + "");
                col = int.Parse(kingMove[1] + "");
            }

            //for (int l = 0; l < loop; l++)
            //{


            int rowplus1 = row + 1 > 8 ? 0 : row + 1;
            int rowplus2 = row + 2 > 8 ? 0 : row + 2;
            int rowminus1 = row - 1 < 0 ? 0 : row - 1;
            int rowminus2 = row - 2 < 0 ? 0 : row - 2;
            int colplus1 = col + 1 > 8 ? 0 : col + 1;
            int colplus2 = col + 2 > 8 ? 0 : col + 2;
            int colminus1 = col - 1 < 0 ? 0 : col - 1;
            int colminus2 = col - 2 < 0 ? 0 : col - 2;




            string k = " " + rivalcolor + 'K';
            // האם יש חייל שיכול לאכול את המלך
            if (colorTurn == 'B')
            {
                if (board[rowplus1, colminus1] == " WP")
                {
                    Melech.checkOnBlack = true;
                    gameOn.positionOfPlayerCheck = "" + rowplus1 + colminus1;
                    gameOn.typeOfPlayerCheck = board[rowplus1, colminus1];
                    gameOn.striptOfCeck = "dl";
                    return true;
                }
                else if (board[rowplus1, colplus1] == " WP")
                {
                    Melech.checkOnBlack = true;
                    gameOn.positionOfPlayerCheck = "" + rowplus1 + colplus1;
                    gameOn.typeOfPlayerCheck = board[rowplus1, colplus1];
                    gameOn.striptOfCeck = "dr";
                    return true;
                }

            }

            else
            {
                if (board[rowminus1, colminus1] == " BP")
                {
                    Melech.checkOnWhite = true;
                    gameOn.positionOfPlayerCheck = "" + rowminus1 + colminus1;
                    gameOn.typeOfPlayerCheck = board[rowminus1, colminus1];
                    gameOn.striptOfCeck = "ul";
                    return true;
                }
                else if (board[rowminus1, colplus1] == " BP")
                {
                    Melech.checkOnBlack = true;
                    gameOn.positionOfPlayerCheck = "" + rowminus1 + colplus1;
                    gameOn.typeOfPlayerCheck = board[rowminus1, colplus1];
                    gameOn.striptOfCeck = "ur";
                    return true;
                }
            }


            // האם יש סוס שמאיים על המלך

            if (board[rowminus2, colplus1] == k)
            {

                if (colorTurn == 'W')
                    Melech.checkOnWhite = true;
                else
                    Melech.checkOnBlack = true;
                gameOn.positionOfPlayerCheck = "" + rowminus2 + colplus1;
                gameOn.typeOfPlayerCheck = board[rowminus2, colplus1];
                gameOn.striptOfCeck = "kn";
                return true;
            }
            else if (board[rowminus2, colminus1] == k)
            {

                if (colorTurn == 'W')
                    Melech.checkOnWhite = true;
                else
                    Melech.checkOnBlack = true;
                gameOn.positionOfPlayerCheck = "" + rowminus2 + colminus1;
                gameOn.typeOfPlayerCheck = board[rowminus2, colminus1];
                gameOn.striptOfCeck = "kn";
                return true;
            }
            else if (board[rowplus2, colminus1] == k)
            {

                if (colorTurn == 'W')
                    Melech.checkOnWhite = true;
                else
                    Melech.checkOnBlack = true;
                gameOn.positionOfPlayerCheck = "" + rowplus2 + colminus1;
                gameOn.typeOfPlayerCheck = board[rowplus2, colminus1];
                gameOn.striptOfCeck = "kn";
                return true;
            }
            else if (board[rowplus2, colplus1] == k)
            {

                if (colorTurn == 'W')
                    Melech.checkOnWhite = true;
                else
                    Melech.checkOnBlack = true;
                gameOn.positionOfPlayerCheck = "" + rowplus2 + colplus1;
                gameOn.typeOfPlayerCheck = board[rowplus2, colplus1];
                gameOn.striptOfCeck = "kn";
                return true;
            }
            else if (board[rowplus1, colminus2] == k)
            {

                if (colorTurn == 'W')
                    Melech.checkOnWhite = true;
                else
                    Melech.checkOnBlack = true;
                gameOn.positionOfPlayerCheck = "" + rowplus1 + colminus2;
                gameOn.typeOfPlayerCheck = board[rowplus1, colminus2];
                gameOn.striptOfCeck = "kn";
                return true;
            }
            else if (board[rowminus1, colminus2] == k)
            {

                if (colorTurn == 'W')
                    Melech.checkOnWhite = true;
                else
                    Melech.checkOnBlack = true;
                gameOn.positionOfPlayerCheck = "" + rowminus1 + colminus2;
                gameOn.typeOfPlayerCheck = board[rowminus1, colminus2];
                gameOn.striptOfCeck = "kn";
                return true;
            }
            else if (board[rowplus1, colplus2] == k)
            {

                if (colorTurn == 'W')
                    Melech.checkOnWhite = true;
                else
                    Melech.checkOnBlack = true;
                gameOn.positionOfPlayerCheck = "" + rowplus1 + colplus2;
                gameOn.typeOfPlayerCheck = board[rowplus1, colplus2];
                gameOn.striptOfCeck = "kn";
                return true;
            }
            else if (board[rowminus1, colplus2] == k)
            {

                if (colorTurn == 'W')
                    Melech.checkOnWhite = true;
                else
                    Melech.checkOnBlack = true;
                gameOn.positionOfPlayerCheck = "" + rowminus1 + colplus2;
                gameOn.typeOfPlayerCheck = board[rowminus1, colplus2];
                gameOn.striptOfCeck = "kn";
                return true;
            }



            // צד ימין תנועה אופקית

            for (int i = colplus1; i <= 8; i++)
                if (board[row, i] != " EE" && (board[row, i] == " " + rivalcolor + 'Q' || board[row, i] == " " + rivalcolor + 'R'))
                {
                    if (colorTurn == 'W')
                        Melech.checkOnWhite = true;
                    else
                        Melech.checkOnBlack = true;
                    gameOn.positionOfPlayerCheck = "" + row + i;
                    gameOn.typeOfPlayerCheck = board[row, i];
                    gameOn.striptOfCeck = "rr";
                    return true;
                }
                else if (board[row, i] != " EE")
                    break;




            // צד שמאל תנועה אופקית

            for (int i = colminus1; i > 0; i--)
                if (board[row, i] != " EE" && (board[row, i] == " " + rivalcolor + 'Q' || board[row, i] == " " + rivalcolor + 'R'))
                {
                    if (colorTurn == 'W')
                        Melech.checkOnWhite = true;
                    else
                        Melech.checkOnBlack = true;
                    gameOn.positionOfPlayerCheck = "" + row + i;
                    gameOn.typeOfPlayerCheck = board[row, i];
                    gameOn.striptOfCeck = "ll";
                    return true;
                }
                else if (board[row, i] != " EE")
                    break;




            // למעלה תנועה אנכית

            for (int i = rowminus1; i > 0; i--)
                if (board[i, col] != " EE" && (board[i, col] == " " + rivalcolor + 'Q' || board[i, col] == " " + rivalcolor + 'R'))
                {
                    if (colorTurn == 'W')
                        Melech.checkOnWhite = true;
                    else
                        Melech.checkOnBlack = true;
                    gameOn.positionOfPlayerCheck = "" + i + col;
                    gameOn.typeOfPlayerCheck = board[i, col];
                    gameOn.striptOfCeck = "uu";
                    return true;
                }
                else if (board[i, col] != " EE")
                    break;





            // למטה תנועה אנכית

            for (int i = rowplus1; i <= 8; i++)
                if (board[i, col] != " EE" && (board[i, col] == " " + rivalcolor + 'Q' || board[i, col] == " " + rivalcolor + 'R'))
                {
                    if (colorTurn == 'W')
                        Melech.checkOnWhite = true;
                    else
                        Melech.checkOnBlack = true;
                    gameOn.positionOfPlayerCheck = "" + i + col;
                    gameOn.typeOfPlayerCheck = board[i, col];
                    gameOn.striptOfCeck = "dd";

                    return true;
                }
                else if (board[i, col] != " EE")
                    break;





            //אלכסון שמאלה למעלה
            for (int r = rowminus1, c = colminus1; c > 0 && r > 0; c--, r--)
            {

                if (board[r, c] != " EE" && (board[r, c] == " " + rivalcolor + 'Q' || board[r, c] == " " + rivalcolor + 'B'))
                {
                    if (colorTurn == 'W')
                        Melech.checkOnWhite = true;
                    else
                        Melech.checkOnBlack = true;
                    gameOn.positionOfPlayerCheck = "" + r + c;
                    gameOn.typeOfPlayerCheck = board[r, c];
                    gameOn.striptOfCeck = "ul";

                    return true;
                }
                else if (board[r, c] != " EE")
                    break;
            }




            //אלכסון ימינה למעלה
            for (int r = rowminus1, c = colplus1; r > 0 && c <= 8; r--, c++)
                if (board[r, c] != " EE" && (board[r, c] == " " + rivalcolor + 'Q' || board[r, c] == " " + rivalcolor + 'B'))
                {
                    if (colorTurn == 'W')
                        Melech.checkOnWhite = true;
                    else
                        Melech.checkOnBlack = true;
                    gameOn.positionOfPlayerCheck = "" + r + c;
                    gameOn.typeOfPlayerCheck = board[r, c];
                    gameOn.striptOfCeck = "ur";
                    return true;
                }
                else if (board[r, c] != " EE")
                    break;



            //אלכסון ימינה למטה

            for (int r = rowplus1, c = colplus1; c <= 8 && r <= 8; c++, r++)
                if (board[r, c] != " EE" && (board[r, c] == " " + rivalcolor + 'Q' || board[r, c] == " " + rivalcolor + 'B'))
                {
                    if (colorTurn == 'W')
                        Melech.checkOnWhite = true;
                    else
                        Melech.checkOnBlack = true;
                    gameOn.positionOfPlayerCheck = "" + r + c;
                    gameOn.typeOfPlayerCheck = board[r, c];
                    gameOn.striptOfCeck = "dr";
                    return true;
                }
                else if (board[r, c] != " EE")
                    break;



            //אלכסון שמאלה למטה
            for (int r = rowplus1, c = colminus1; r <= 8 && c > 0; r++, c--)
                if (board[r, c] != " EE" && (board[r, c] == " " + rivalcolor + 'Q' || board[r, c] == " " + rivalcolor + 'B'))
                {
                    if (colorTurn == 'W')
                        Melech.checkOnWhite = true;
                    else
                        Melech.checkOnBlack = true;
                    gameOn.positionOfPlayerCheck = "" + r + c;
                    gameOn.typeOfPlayerCheck = board[r, c];
                    gameOn.striptOfCeck = "dl";
                    return true;
                }
                else if (board[r, c] != " EE")
                    break;

            if (kingMove == "EndTurn")
            {
                if (colorTurn == 'W')
                    Melech.checkOnWhite = false;
                else
                    Melech.checkOnBlack = false;
            }
            return false;
        }



        // פונקצית בדיקה של שחמט
        public static bool isCheckmate(Players[] allPlayers, string[,] board, char colorTurn)
        {

            char kingColor = colorTurn;
            string kingPosition;
            if (colorTurn == 'B')
                kingPosition = allPlayers[31].currentPosition;
            else
                kingPosition = allPlayers[30].currentPosition;

            int row = int.Parse(kingPosition[0] + "");
            int col = int.Parse(kingPosition[1] + "");


            int rowplus1 = row + 1 > 8 ? 0 : row + 1;
            int rowminus1 = row - 1 < 0 ? 0 : row - 1;
            int colplus1 = col + 1 > 8 ? 0 : col + 1;
            int colminus1 = col - 1 < 0 ? 0 : col - 1;

            // בודק באם המלך יכול לברוח
            if (rowplus1 != 0 && colplus1 != 0)
            {
                if (board[rowplus1, colplus1][1] != kingColor && (!gameOn.ischeck(allPlayers, board, kingColor, ("" + rowplus1 + colplus1))))
                {

                    return false;
                }
            }

            if (colplus1 != 0)
            {
                if (board[row, colplus1][1] != kingColor && (!gameOn.ischeck(allPlayers, board, kingColor, ("" + row + colplus1))))
                {

                    return false;
                }
            }

            if (rowminus1 != 0 && colplus1 != 0)
            {
                if (board[rowminus1, colplus1][1] != kingColor && !(gameOn.ischeck(allPlayers, board, kingColor, ("" + rowminus1 + colplus1))))
                {

                    return false;
                }
            }
            if (rowminus1 != 0)
            {
                if (board[rowminus1, col][1] != kingColor && (!gameOn.ischeck(allPlayers, board, kingColor, ("" + rowminus1 + col))))
                {

                    return false;
                }
            }

            if (rowminus1 != 0 && colminus1 != 0)
            {
                if (board[rowminus1, colminus1][1] != kingColor && (!gameOn.ischeck(allPlayers, board, kingColor, ("" + rowminus1 + colminus1))))
                {

                    return false;
                }
            }
            if (colminus1 != 0)
            {
                if (board[row, colminus1][1] != kingColor && (!gameOn.ischeck(allPlayers, board, kingColor, ("" + row + colminus1))))
                {

                    return false;
                }
            }
            if (rowplus1 != 0 && colminus1 != 0)
            {
                if (board[rowplus1, colminus1][1] != kingColor && (!(gameOn.ischeck(allPlayers, board, kingColor, ("" + rowplus1 + colminus1)))))
                {

                    return false;
                }
            }
            if (rowplus1 != 0)
            {
                if (board[rowplus1, col][1] != kingColor && (!gameOn.ischeck(allPlayers, board, kingColor, ("" + rowplus1 + col))))
                {

                    return false;
                }
            }

            // בודק האם שחקן אחר יכול למנוע את השח
            string[,] oldBoard = new string[9, 9];
            for (int r = 0; r < 9; r++)
                for (int c = 0; c < 9; c++)
                {
                    oldBoard[r, c] = board[r, c];
                }
            string[] move = new string[2];
            bool valid;
            for (int i = 0; i < 32; i++)
            {
                if (allPlayers[i].currentPosition == "00")
                    continue;

                if (allPlayers[i].symbol[1] == kingColor)
                {

                    switch (allPlayers[i].symbol[2])
                    {
                        case 'P':
                            switch (gameOn.striptOfCeck)
                            {
                                case "kn":
                                    move[0] = allPlayers[i].currentPosition;
                                    move[1] = gameOn.positionOfPlayerCheck;
                                    (board, valid) = Pawn.move(move, board, kingColor == 'W' ? true : false, allPlayers[i], allPlayers, true);
                                    if (valid)
                                    {
                                        board = oldBoard;
                                        return false;
                                    }
                                    break;
                                case "uu":
                                    for (int r = rowminus1; r >= int.Parse(gameOn.positionOfPlayerCheck[0] + ""); r--)
                                    {
                                        move[0] = allPlayers[i].currentPosition;
                                        move[1] = "" + r + col;
                                        (board, valid) = Pawn.move(move, board, kingColor == 'W' ? true : false, allPlayers[i], allPlayers, true);
                                        if (valid)
                                        {
                                            board = oldBoard;
                                            return false;
                                        }

                                    }
                                    break;
                                case "dd":
                                    for (int r = rowplus1; r <= int.Parse(gameOn.positionOfPlayerCheck[0] + ""); r++)
                                    {
                                        move[0] = allPlayers[i].currentPosition;
                                        move[1] = "" + r + col;
                                        (board, valid) = Pawn.move(move, board, kingColor == 'W' ? true : false, allPlayers[i], allPlayers, true);
                                        if (valid)
                                        {
                                            board = oldBoard;
                                            return false;
                                        }

                                    }
                                    break;

                                case "rr":
                                    for (int c = colplus1; c >= int.Parse(gameOn.positionOfPlayerCheck[1] + ""); c++)
                                    {
                                        move[0] = allPlayers[i].currentPosition;
                                        move[1] = "" + row + c;
                                        (board, valid) = Pawn.move(move, board, kingColor == 'W' ? true : false, allPlayers[i], allPlayers, true);
                                        if (valid)
                                        {
                                            board = oldBoard;
                                            return false;
                                        }

                                    }
                                    break;
                                case "ll":
                                    for (int c = colminus1; c >= int.Parse(gameOn.positionOfPlayerCheck[1] + ""); c--)
                                    {
                                        move[0] = allPlayers[i].currentPosition;
                                        move[1] = "" + row + c;
                                        (board, valid) = Pawn.move(move, board, kingColor == 'W' ? true : false, allPlayers[i], allPlayers, true);
                                        if (valid)
                                        {
                                            board = oldBoard;
                                            return false;
                                        }

                                    }
                                    break;

                                case "ur":
                                    for (int r = rowminus1, c = colplus1; r >= int.Parse(gameOn.positionOfPlayerCheck[0] + "") && c <= int.Parse(gameOn.positionOfPlayerCheck[1] + ""); r--, c++)
                                    {
                                        move[0] = allPlayers[i].currentPosition;
                                        move[1] = "" + r + c;
                                        (board, valid) = Pawn.move(move, board, kingColor == 'W' ? true : false, allPlayers[i], allPlayers, true);
                                        if (valid)
                                        {
                                            board = oldBoard;
                                            return false;
                                        }

                                    }
                                    break;
                                case "ul":
                                    for (int r = rowminus1, c = colminus1; r >= int.Parse(gameOn.positionOfPlayerCheck[0] + "") && c >= int.Parse(gameOn.positionOfPlayerCheck[1] + ""); r--, c--)
                                    {
                                        move[0] = allPlayers[i].currentPosition;
                                        move[1] = "" + r + c;
                                        (board, valid) = Pawn.move(move, board, kingColor == 'W' ? true : false, allPlayers[i], allPlayers, true);
                                        if (valid)
                                        {
                                            board = oldBoard;
                                            return false;
                                        }

                                    }
                                    break;
                                case "dl":
                                    for (int r = rowplus1, c = colminus1; r <= int.Parse(gameOn.positionOfPlayerCheck[0] + "") && c >= int.Parse(gameOn.positionOfPlayerCheck[1] + ""); r++, c--)
                                    {
                                        move[0] = allPlayers[i].currentPosition;
                                        move[1] = "" + r + c;
                                        (board, valid) = Pawn.move(move, board, kingColor == 'W' ? true : false, allPlayers[i], allPlayers, true);
                                        if (valid)
                                        {
                                            board = oldBoard;
                                            return false;
                                        }

                                    }
                                    break;
                                case "dr":
                                    for (int r = rowplus1, c = colplus1; r <= int.Parse(gameOn.positionOfPlayerCheck[0] + "") && c <= int.Parse(gameOn.positionOfPlayerCheck[1] + ""); r++, c++)
                                    {
                                        move[0] = allPlayers[i].currentPosition;
                                        move[1] = "" + r + c;
                                        (board, valid) = Pawn.move(move, board, kingColor == 'W' ? true : false, allPlayers[i], allPlayers, true);
                                        if (valid)
                                        {
                                            board = oldBoard;
                                            return false;
                                        }

                                    }
                                    break;
                            }
                            break;
                        case 'R':
                            switch (gameOn.striptOfCeck)
                            {
                                case "kn":
                                    move[0] = allPlayers[i].currentPosition;
                                    move[1] = gameOn.positionOfPlayerCheck;
                                    (board, valid) = Rook.move(move, board, kingColor, allPlayers[i], allPlayers, true);
                                    if (valid)
                                    {
                                        board = oldBoard;
                                        return false;
                                    }
                                    break;
                                case "uu":
                                    for (int r = rowminus1; r >= int.Parse(gameOn.positionOfPlayerCheck[0] + ""); r--)
                                    {
                                        move[0] = allPlayers[i].currentPosition;
                                        move[1] = "" + r + col;
                                        (board, valid) = Rook.move(move, board, kingColor, allPlayers[i], allPlayers, true);
                                        if (valid)
                                        {
                                            board = oldBoard;
                                            return false;
                                        }

                                    }
                                    break;
                                case "dd":
                                    for (int r = rowplus1; r <= int.Parse(gameOn.positionOfPlayerCheck[0] + ""); r++)
                                    {
                                        move[0] = allPlayers[i].currentPosition;
                                        move[1] = "" + r + col;
                                        (board, valid) = Rook.move(move, board, kingColor, allPlayers[i], allPlayers, true);
                                        if (valid)
                                        {
                                            board = oldBoard;
                                            return false;
                                        }

                                    }
                                    break;

                                case "rr":
                                    for (int c = colplus1; c >= int.Parse(gameOn.positionOfPlayerCheck[1] + ""); c++)
                                    {
                                        move[0] = allPlayers[i].currentPosition;
                                        move[1] = "" + row + c;
                                        (board, valid) = Rook.move(move, board, kingColor, allPlayers[i], allPlayers, true);
                                        if (valid)
                                        {
                                            board = oldBoard;
                                            return false;
                                        }

                                    }
                                    break;
                                case "ll":
                                    for (int c = colminus1; c >= int.Parse(gameOn.positionOfPlayerCheck[1] + ""); c--)
                                    {
                                        move[0] = allPlayers[i].currentPosition;
                                        move[1] = "" + row + c;
                                        (board, valid) = Rook.move(move, board, kingColor, allPlayers[i], allPlayers, true);
                                        if (valid)
                                        {
                                            board = oldBoard;
                                            return false;
                                        }

                                    }
                                    break;

                                case "ur":
                                    for (int r = rowminus1, c = colplus1; r >= int.Parse(gameOn.positionOfPlayerCheck[0] + "") && c <= int.Parse(gameOn.positionOfPlayerCheck[1] + ""); r--, c++)
                                    {
                                        move[0] = allPlayers[i].currentPosition;
                                        move[1] = "" + r + c;
                                        (board, valid) = Rook.move(move, board, kingColor, allPlayers[i], allPlayers, true);
                                        if (valid)
                                        {
                                            board = oldBoard;
                                            return false;
                                        }

                                    }
                                    break;
                                case "ul":
                                    for (int r = rowminus1, c = colminus1; r >= int.Parse(gameOn.positionOfPlayerCheck[0] + "") && c >= int.Parse(gameOn.positionOfPlayerCheck[1] + ""); r--, c--)
                                    {
                                        move[0] = allPlayers[i].currentPosition;
                                        move[1] = "" + r + c;
                                        (board, valid) = Rook.move(move, board, kingColor, allPlayers[i], allPlayers, true);
                                        if (valid)
                                        {
                                            board = oldBoard;
                                            return false;
                                        }

                                    }
                                    break;
                                case "dl":
                                    for (int r = rowplus1, c = colminus1; r <= int.Parse(gameOn.positionOfPlayerCheck[0] + "") && c >= int.Parse(gameOn.positionOfPlayerCheck[1] + ""); r++, c--)
                                    {
                                        move[0] = allPlayers[i].currentPosition;
                                        move[1] = "" + r + c;
                                        (board, valid) = Rook.move(move, board, kingColor, allPlayers[i], allPlayers, true);
                                        if (valid)
                                        {
                                            board = oldBoard;
                                            return false;
                                        }

                                    }
                                    break;
                                case "dr":
                                    for (int r = rowplus1, c = colplus1; r <= int.Parse(gameOn.positionOfPlayerCheck[0] + "") && c <= int.Parse(gameOn.positionOfPlayerCheck[1] + ""); r++, c++)
                                    {
                                        move[0] = allPlayers[i].currentPosition;
                                        move[1] = "" + r + c;
                                        (board, valid) = Rook.move(move, board, kingColor, allPlayers[i], allPlayers, true);
                                        if (valid)
                                        {
                                            board = oldBoard;
                                            return false;
                                        }

                                    }
                                    break;
                            }
                            break;
                        case 'B':
                            switch (gameOn.striptOfCeck)
                            {
                                case "kn":
                                    move[0] = allPlayers[i].currentPosition;
                                    move[1] = gameOn.positionOfPlayerCheck;
                                    (board, valid) = Bishop.move(move, board, kingColor, allPlayers[i], allPlayers, true);
                                    if (valid)
                                    {
                                        board = oldBoard;
                                        return false;
                                    }
                                    break;
                                case "uu":
                                    for (int r = rowminus1; r >= int.Parse(gameOn.positionOfPlayerCheck[0] + ""); r--)
                                    {
                                        move[0] = allPlayers[i].currentPosition;
                                        move[1] = "" + r + col;
                                        (board, valid) = Bishop.move(move, board, kingColor, allPlayers[i], allPlayers, true);
                                        if (valid)
                                        {
                                            board = oldBoard;
                                            return false;
                                        }

                                    }
                                    break;
                                case "dd":
                                    for (int r = rowplus1; r <= int.Parse(gameOn.positionOfPlayerCheck[0] + ""); r++)
                                    {
                                        move[0] = allPlayers[i].currentPosition;
                                        move[1] = "" + r + col;
                                        (board, valid) = Bishop.move(move, board, kingColor, allPlayers[i], allPlayers, true);
                                        if (valid)
                                        {
                                            board = oldBoard;
                                            return false;
                                        }

                                    }
                                    break;

                                case "rr":
                                    for (int c = colplus1; c >= int.Parse(gameOn.positionOfPlayerCheck[1] + ""); c++)
                                    {
                                        move[0] = allPlayers[i].currentPosition;
                                        move[1] = "" + row + c;
                                        (board, valid) = Bishop.move(move, board, kingColor, allPlayers[i], allPlayers, true);
                                        if (valid)
                                        {
                                            board = oldBoard;
                                            return false;
                                        }

                                    }
                                    break;
                                case "ll":
                                    for (int c = colminus1; c >= int.Parse(gameOn.positionOfPlayerCheck[1] + ""); c--)
                                    {
                                        move[0] = allPlayers[i].currentPosition;
                                        move[1] = "" + row + c;
                                        (board, valid) = Bishop.move(move, board, kingColor, allPlayers[i], allPlayers, true);
                                        if (valid)
                                        {
                                            board = oldBoard;
                                            return false;
                                        }

                                    }
                                    break;

                                case "ur":
                                    for (int r = rowminus1, c = colplus1; r >= int.Parse(gameOn.positionOfPlayerCheck[0] + "") && c <= int.Parse(gameOn.positionOfPlayerCheck[1] + ""); r--, c++)
                                    {
                                        move[0] = allPlayers[i].currentPosition;
                                        move[1] = "" + r + c;
                                        (board, valid) = Bishop.move(move, board, kingColor, allPlayers[i], allPlayers, true);
                                        if (valid)
                                        {
                                            board = oldBoard;
                                            return false;
                                        }

                                    }
                                    break;
                                case "ul":
                                    for (int r = rowminus1, c = colminus1; r >= int.Parse(gameOn.positionOfPlayerCheck[0] + "") && c >= int.Parse(gameOn.positionOfPlayerCheck[1] + ""); r--, c--)
                                    {
                                        move[0] = allPlayers[i].currentPosition;
                                        move[1] = "" + r + c;
                                        (board, valid) = Bishop.move(move, board, kingColor, allPlayers[i], allPlayers, true);
                                        if (valid)
                                        {
                                            board = oldBoard;
                                            return false;
                                        }

                                    }
                                    break;
                                case "dl":
                                    for (int r = rowplus1, c = colminus1; r <= int.Parse(gameOn.positionOfPlayerCheck[0] + "") && c >= int.Parse(gameOn.positionOfPlayerCheck[1] + ""); r++, c--)
                                    {
                                        move[0] = allPlayers[i].currentPosition;
                                        move[1] = "" + r + c;
                                        (board, valid) = Bishop.move(move, board, kingColor, allPlayers[i], allPlayers, true);
                                        if (valid)
                                        {
                                            board = oldBoard;
                                            return false;
                                        }

                                    }
                                    break;
                                case "dr":
                                    for (int r = rowplus1, c = colplus1; r <= int.Parse(gameOn.positionOfPlayerCheck[0] + "") && c <= int.Parse(gameOn.positionOfPlayerCheck[1] + ""); r++, c++)
                                    {
                                        move[0] = allPlayers[i].currentPosition;
                                        move[1] = "" + r + c;
                                        (board, valid) = Bishop.move(move, board, kingColor, allPlayers[i], allPlayers, true);
                                        if (valid)
                                        {
                                            board = oldBoard;
                                            return false;
                                        }

                                    }
                                    break;
                            }
                            break;
                        case 'Q':
                            switch (gameOn.striptOfCeck)
                            {
                                case "kn":
                                    move[0] = allPlayers[i].currentPosition;
                                    move[1] = gameOn.positionOfPlayerCheck;
                                    (board, valid) = Queen.move(move, board, kingColor, allPlayers[i], allPlayers, true);
                                    if (valid)
                                    {
                                        board = oldBoard;
                                        return false;
                                    }
                                    break;
                                case "uu":
                                    for (int r = rowminus1; r >= int.Parse(gameOn.positionOfPlayerCheck[0] + ""); r--)
                                    {
                                        move[0] = allPlayers[i].currentPosition;
                                        move[1] = "" + r + col;
                                        (board, valid) = Queen.move(move, board, kingColor, allPlayers[i], allPlayers, true);
                                        if (valid)
                                        {
                                            board = oldBoard;
                                            return false;
                                        }

                                    }
                                    break;
                                case "dd":
                                    for (int r = rowplus1; r <= int.Parse(gameOn.positionOfPlayerCheck[0] + ""); r++)
                                    {
                                        move[0] = allPlayers[i].currentPosition;
                                        move[1] = "" + r + col;
                                        (board, valid) = Queen.move(move, board, kingColor, allPlayers[i], allPlayers, true);
                                        if (valid)
                                        {
                                            board = oldBoard;
                                            return false;
                                        }

                                    }
                                    break;

                                case "rr":
                                    for (int c = colplus1; c >= int.Parse(gameOn.positionOfPlayerCheck[1] + ""); c++)
                                    {
                                        move[0] = allPlayers[i].currentPosition;
                                        move[1] = "" + row + c;
                                        (board, valid) = Queen.move(move, board, kingColor, allPlayers[i], allPlayers, true);
                                        if (valid)
                                        {
                                            board = oldBoard;
                                            return false;
                                        }

                                    }
                                    break;
                                case "ll":
                                    for (int c = colminus1; c >= int.Parse(gameOn.positionOfPlayerCheck[1] + ""); c--)
                                    {
                                        move[0] = allPlayers[i].currentPosition;
                                        move[1] = "" + row + c;
                                        (board, valid) = Queen.move(move, board, kingColor, allPlayers[i], allPlayers, true);
                                        if (valid)
                                        {
                                            board = oldBoard;
                                            return false;
                                        }

                                    }
                                    break;

                                case "ur":
                                    for (int r = rowminus1, c = colplus1; r >= int.Parse(gameOn.positionOfPlayerCheck[0] + "") && c <= int.Parse(gameOn.positionOfPlayerCheck[1] + ""); r--, c++)
                                    {
                                        move[0] = allPlayers[i].currentPosition;
                                        move[1] = "" + r + c;
                                        (board, valid) = Queen.move(move, board, kingColor, allPlayers[i], allPlayers, true);
                                        if (valid)
                                        {
                                            board = oldBoard;
                                            return false;
                                        }

                                    }
                                    break;
                                case "ul":
                                    for (int r = rowminus1, c = colminus1; r >= int.Parse(gameOn.positionOfPlayerCheck[0] + "") && c >= int.Parse(gameOn.positionOfPlayerCheck[1] + ""); r--, c--)
                                    {
                                        move[0] = allPlayers[i].currentPosition;
                                        move[1] = "" + r + c;
                                        (board, valid) = Queen.move(move, board, kingColor, allPlayers[i], allPlayers, true);
                                        if (valid)
                                        {
                                            board = oldBoard;
                                            return false;
                                        }

                                    }
                                    break;
                                case "dl":
                                    for (int r = rowplus1, c = colminus1; r <= int.Parse(gameOn.positionOfPlayerCheck[0] + "") && c >= int.Parse(gameOn.positionOfPlayerCheck[1] + ""); r++, c--)
                                    {
                                        move[0] = allPlayers[i].currentPosition;
                                        move[1] = "" + r + c;
                                        (board, valid) = Queen.move(move, board, kingColor, allPlayers[i], allPlayers, true);
                                        if (valid)
                                        {
                                            board = oldBoard;
                                            return false;
                                        }

                                    }
                                    break;
                                case "dr":
                                    for (int r = rowplus1, c = colplus1; r <= int.Parse(gameOn.positionOfPlayerCheck[0] + "") && c <= int.Parse(gameOn.positionOfPlayerCheck[1] + ""); r++, c++)
                                    {
                                        move[0] = allPlayers[i].currentPosition;
                                        move[1] = "" + r + c;
                                        (board, valid) = Queen.move(move, board, kingColor, allPlayers[i], allPlayers, true);
                                        if (valid)
                                        {
                                            board = oldBoard;
                                            return false;
                                        }

                                    }
                                    break;
                            }
                            break;

                        case 'K':
                            switch (gameOn.striptOfCeck)
                            {
                                case "kn":
                                    move[0] = allPlayers[i].currentPosition;
                                    move[1] = gameOn.positionOfPlayerCheck;
                                    (board, valid) = Knight.move(move, board, kingColor, allPlayers[i], allPlayers, true);
                                    if (valid)
                                    {
                                        board = oldBoard;
                                        return false;
                                    }
                                    break;
                                case "uu":
                                    for (int r = rowminus1; r >= int.Parse(gameOn.positionOfPlayerCheck[0] + ""); r--)
                                    {
                                        move[0] = allPlayers[i].currentPosition;
                                        move[1] = "" + r + col;
                                        (board, valid) = Knight.move(move, board, kingColor, allPlayers[i], allPlayers, true);
                                        if (valid)
                                        {
                                            board = oldBoard;
                                            return false;
                                        }

                                    }
                                    break;
                                case "dd":
                                    for (int r = rowplus1; r <= int.Parse(gameOn.positionOfPlayerCheck[0] + ""); r++)
                                    {
                                        move[0] = allPlayers[i].currentPosition;
                                        move[1] = "" + r + col;
                                        (board, valid) = Knight.move(move, board, kingColor, allPlayers[i], allPlayers, true);
                                        if (valid)
                                        {
                                            board = oldBoard;
                                            return false;
                                        }

                                    }
                                    break;

                                case "rr":
                                    for (int c = colplus1; c >= int.Parse(gameOn.positionOfPlayerCheck[1] + ""); c++)
                                    {
                                        move[0] = allPlayers[i].currentPosition;
                                        move[1] = "" + row + c;
                                        (board, valid) = Knight.move(move, board, kingColor, allPlayers[i], allPlayers, true);
                                        if (valid)
                                        {
                                            board = oldBoard;
                                            return false;
                                        }

                                    }
                                    break;
                                case "ll":
                                    for (int c = colminus1; c >= int.Parse(gameOn.positionOfPlayerCheck[1] + ""); c--)
                                    {
                                        move[0] = allPlayers[i].currentPosition;
                                        move[1] = "" + row + c;
                                        (board, valid) = Knight.move(move, board, kingColor, allPlayers[i], allPlayers, true);
                                        if (valid)
                                        {
                                            board = oldBoard;
                                            return false;
                                        }

                                    }
                                    break;

                                case "ur":
                                    for (int r = rowminus1, c = colplus1; r >= int.Parse(gameOn.positionOfPlayerCheck[0] + "") && c <= int.Parse(gameOn.positionOfPlayerCheck[1] + ""); r--, c++)
                                    {
                                        move[0] = allPlayers[i].currentPosition;
                                        move[1] = "" + r + c;
                                        (board, valid) = Knight.move(move, board, kingColor, allPlayers[i], allPlayers, true);
                                        if (valid)
                                        {
                                            board = oldBoard;
                                            return false;
                                        }

                                    }
                                    break;
                                case "ul":
                                    for (int r = rowminus1, c = colminus1; r >= int.Parse(gameOn.positionOfPlayerCheck[0] + "") && c >= int.Parse(gameOn.positionOfPlayerCheck[1] + ""); r--, c--)
                                    {
                                        move[0] = allPlayers[i].currentPosition;
                                        move[1] = "" + r + c;
                                        (board, valid) = Knight.move(move, board, kingColor, allPlayers[i], allPlayers, true);
                                        if (valid)
                                        {
                                            board = oldBoard;
                                            return false;
                                        }

                                    }
                                    break;
                                case "dl":
                                    for (int r = rowplus1, c = colminus1; r <= int.Parse(gameOn.positionOfPlayerCheck[0] + "") && c >= int.Parse(gameOn.positionOfPlayerCheck[1] + ""); r++, c--)
                                    {
                                        move[0] = allPlayers[i].currentPosition;
                                        move[1] = "" + r + c;
                                        (board, valid) = Knight.move(move, board, kingColor, allPlayers[i], allPlayers, true);
                                        if (valid)
                                        {
                                            board = oldBoard;
                                            return false;
                                        }

                                    }
                                    break;
                                case "dr":
                                    for (int r = rowplus1, c = colplus1; r <= int.Parse(gameOn.positionOfPlayerCheck[0] + "") && c <= int.Parse(gameOn.positionOfPlayerCheck[1] + ""); r++, c++)
                                    {
                                        move[0] = allPlayers[i].currentPosition;
                                        move[1] = "" + r + c;
                                        (board, valid) = Knight.move(move, board, kingColor, allPlayers[i], allPlayers, true);
                                        if (valid)
                                        {
                                            board = oldBoard;
                                            return false;
                                        }

                                    }
                                    break;
                            }
                            break;
                    }


                }
            }

            return true;
        }



        // פונקציה שבודקת האם נשארו שחקנים רלוונטים כדי לסיים משחק
        public static bool isEnoughPlayersToWin(Players[] allPlayers)
        {
            bool isEnoug;
            int WQ = 0;
            int WP = 0;
            int WR = 0;
            int WB = 0;
            int WK = 0;
            int BQ = 0;
            int BP = 0;
            int BR = 0;
            int BB = 0;
            int BK = 0;


            for (int i = 0; i < 32; i++)
            {
                if (allPlayers[i].currentPosition != "00")
                    switch (allPlayers[i].symbol)
                    {
                        case " BP":
                            BP++;
                            break;
                        case " BQ":
                            BQ++;
                            break;
                        case " BR":
                            BR++;
                            break;
                        case " BB":
                            BB++;
                            break;
                        case " BK":
                            BK++;
                            break;
                        case " WP":
                            WP++;
                            break;
                        case " WQ":
                            WQ++;
                            break;
                        case " WR":
                            WR++;
                            break;
                        case " WB":
                            WB++;
                            break;
                        case " WK":
                            WK++;
                            break;
                    }
            }


            if (WQ > 0 || WR > 0 || WP > 0 || WB > 1 || (WB > 0 && WK > 0))
                isEnoug = true;
            else
            {
                Console.WriteLine("White player is out of soldiers");
                return false;
            }

            if (BQ > 0 || BR > 0 || BP > 0 || BB > 1 || (BB > 0 && BK > 0))
                isEnoug = true;
            else
            {
                Console.WriteLine("Black player is out of soldiers");
                return false;
            }


            return isEnoug;
        }


        // בודק אם לאחר הזזת שחקן יש שח או מת או תיקו
        public static (string[,], bool) isCheckOnThisMove(Players[] allPlayers, Players player, string[,] board, char rivelColor, char colorTurn, int Frow, int Fcol, int Trow, int Tcol)
        {
            int index = 32;
            for (int i = 0; i < 32; i++)
                if (allPlayers[i].currentPosition == "" + Trow + Tcol && allPlayers[i].symbol != " EE")
                {
                    index = i;
                    allPlayers[i].currentPosition = "00";
                    break;
                }
            player.currentPosition = "" + Trow + Tcol;

            string oldPosition = board[Frow, Fcol];
            string newPosition = board[Trow, Tcol];
            bool ischess = colorTurn == 'W' ? Melech.checkOnWhite : Melech.checkOnBlack;

            board[Frow, Fcol] = " EE";
            board[Trow, Tcol] = player.symbol;
            if (rivelColor != colorTurn && gameOn.ischeck(allPlayers, board, colorTurn))
            {
                // בודק האם יש פט
                if (gameOn.isCheckmate(allPlayers, board, colorTurn) && (!ischess))
                {
                    Console.WriteLine("Draw !!");
                    gameOn.gameOver();
                }
                board[Trow, Tcol] = newPosition;
                board[Frow, Fcol] = oldPosition;
                player.currentPosition = "" + Frow + Fcol;
                if (index < 32)
                    allPlayers[index].currentPosition = "00";
                gameOn.errorMassege();
                Console.WriteLine("this move will risk your king");
                return (board, true);
            }
            board[Trow, Tcol] = newPosition;
            board[Frow, Fcol] = oldPosition;
            player.currentPosition = "" + Frow + Fcol;
            if (index < 32)
                allPlayers[index].currentPosition = "00";


            return (board, false);

        }

        //האם יש תיקו לאחר 3 פעמים שהלוח מסודר באותו צורה    

        public static void isSameBoard3Times(Players[] allPlayers)
        {
            string[] copy = new string[positionsAllGame.Length + 1];

            for (int i = 0; i < positionsAllGame.Length; i++)
            {
                copy[i] += positionsAllGame[i];
            }

            copy[positionsAllGame.Length] = "";
            for (int i = 0; i < 32; i++)
            {

                copy[positionsAllGame.Length] += allPlayers[i].currentPosition;
            }

            positionsAllGame = copy;

            for (int i = 0; i < positionsAllGame.Length; i++)
            {
                int returns = 0;
                for (int y = 0; y < positionsAllGame.Length; y++)
                {
                    if (i == y)
                        continue;

                    if (positionsAllGame[i] == positionsAllGame[y])
                        returns += 1;
                }
                //Console.WriteLine(returns);
                if (returns > 2)
                {
                    Console.WriteLine("Draw!");
                    Console.WriteLine("The board is been 3 time the same");
                    gameOver();
                }
            }


        }
    }

    /// <summary>
    /// ////////////////////////////////////////////////////////////////////////////////////// 
    /// </summary>
    class Players
    {
        public string currentPosition;
        string[] positions;
        int[] timesInTheSamePosition;
        public string symbol;
        public bool isMoved;
        public bool pawlJumpNext2;

        public Players() { }
        public Players(string symbol, string currentPosition)
        {
            gameOn.countInsignificantMoveWhite = 0;
            gameOn.countInsignificantMoveBlack = 0;
            this.pawlJumpNext2 = false;
            this.currentPosition = currentPosition;
            this.isMoved = false;
            this.symbol = symbol;
            this.positions = new string[64];
            this.timesInTheSamePosition = new int[64];
            int index = 0;
            for (int r = 1; r <= 8; r++)
                for (int c = 1; c <= 8; c++)
                {
                    timesInTheSamePosition[index] = 0;
                    positions[index] = "" + r + c;
                    index++;
                }
        }

        public void checkIfMove3TimeToTheSamePosition(int row, int col, Players[] allPlayers, Players player)
        {
            if (gameOn.countInsignificantMoveWhite == 50)
            {
                Console.WriteLine("White player has made 50 insignificant moves in a row");
                gameOn.gameOver();
            }
            else if (gameOn.countInsignificantMoveBlack == 50)
            {
                Console.WriteLine("Black player has made 50 insignificant moves in a row");
                gameOn.gameOver();
            }
            for (int i = 0; i < 32; i++)
            {
                if (allPlayers[i].currentPosition == "" + row + col)
                {


                    allPlayers[i].currentPosition = "00";
                    break;
                }
            }

            if (allPlayers[30].currentPosition == "00" || allPlayers[31].currentPosition == "00")
                gameOn.gameOver();
            this.currentPosition = "" + row + col;

            for (int i = 0; i < 64; i++)

                if (this.positions[i] == "" + row + col)
                {
                    this.timesInTheSamePosition[i]++;
                    if (player.timesInTheSamePosition[i] == 3)
                    {
                        Console.WriteLine(player.timesInTheSamePosition[i]);
                        Console.WriteLine("Draw!!");
                        Console.WriteLine("The soldier has been in the same position for 3 times in a row");

                        gameOn.gameOver();
                        break;
                    }
                    else
                        break;
                }

            gameOn.isSameBoard3Times(allPlayers);
        }



    }

    class Melech
        : Players
    {

        public static bool isWhiteMoved = false;
        public static bool isBlackMoved = false;
        public static bool checkOnWhite = false;
        public static bool checkOnBlack = false;
        public Melech(string name, string currentPosition) : base(name, currentPosition)
        {

        }


        // פונקציית תנועה
        public static (string[,], bool) move(string[] move, string[,] board, char colorTurn, Players player, Players[] allPlayers) // וולידציה והזזה של המלך לפי חוקיו
        {
            string moveFrom = move[0];
            int Frow = int.Parse(moveFrom[0] + "");
            int Fcol = int.Parse(moveFrom[1] + "");
            string moveTo = move[1];
            int Trow = int.Parse(moveTo[0] + "");
            int Tcol = int.Parse(moveTo[1] + "");
            char rivelColor = (board[Trow, Tcol])[1];
            bool valid = false;




            //בודק האם תזוזה ימינה שמאלה למעלה למטה תקנית
            if (((1 == Math.Abs(Trow - Frow) && Fcol == Tcol) || (1 == Math.Abs(Tcol - Fcol) && Frow == Trow)) && rivelColor != colorTurn)
            {
                (board, valid) = gameOn.isCheckOnThisMove(allPlayers, player, board, rivelColor, colorTurn, Frow, Fcol, Trow, Tcol);
                if (valid)
                    return (board, false);

                gameOn.insignificantMove(colorTurn, Trow, Tcol, board);
                board[Frow, Fcol] = " EE";
                board[Trow, Tcol] = player.symbol;

                player.checkIfMove3TimeToTheSamePosition(Trow, Tcol, allPlayers, player);
                player.isMoved = true;
                return (board, true);
            }


            // תזוזה באלכסון
            if ((1 == Math.Abs(Trow - Frow) && 1 == Math.Abs(Tcol - Fcol)) && rivelColor != colorTurn)
            {
                (board, valid) = gameOn.isCheckOnThisMove(allPlayers, player, board, rivelColor, colorTurn, Frow, Fcol, Trow, Tcol);
                if (valid)
                    return (board, false);

                gameOn.insignificantMove(colorTurn, Trow, Tcol, board);
                board[Frow, Fcol] = " EE";
                board[Trow, Tcol] = player.symbol;

                player.checkIfMove3TimeToTheSamePosition(Trow, Tcol, allPlayers, player);
                player.isMoved = true;
                return (board, true);
            }

            Players rook = new Players();
            for (int i = 0; i < 32; i++)
                if (allPlayers[i].currentPosition == moveTo)
                {
                    rook = allPlayers[i];
                    break;
                }

            //הצרחה
            if (player.isMoved == false && colorTurn == 'W' ? (!Melech.checkOnWhite) : (!Melech.checkOnBlack))
                if (rook.isMoved == false)
                {
                    switch ("" + Trow + Tcol)
                    {
                        case "11":
                            valid = (!gameOn.ischeck(allPlayers, board, colorTurn, "13"));
                            break;
                        case "18":
                            valid = (!gameOn.ischeck(allPlayers, board, colorTurn, "17"));
                            break;
                        case "81":
                            valid = (!gameOn.ischeck(allPlayers, board, colorTurn, "83"));
                            break;
                        case "88":
                            valid = (!gameOn.ischeck(allPlayers, board, colorTurn, "87"));
                            break;
                        default:
                            valid = false;
                            break;
                    }
                    if (valid)
                    {
                        (board, valid) = hatzraca(board, Trow, Tcol, Frow, Fcol, player.symbol, allPlayers, player);
                        if (valid)
                        {
                            if (colorTurn == 'W')
                                gameOn.countInsignificantMoveWhite++;
                            else
                                gameOn.countInsignificantMoveBlack++;
                            player.isMoved = true;
                            rook.isMoved = true;
                            return (board, true);
                        }
                        else
                            Console.WriteLine("The king cannot do 'Hatzraha'");
                        return (board, false);
                    }
                }

            // ברירת מחדל של פונקציית תנועה

            return (board, false);
        }



        // פונקציית הצרחה
        public static (string[,], bool) hatzraca(string[,] board, int Trow, int Tcol, int Frow, int Fcol, string playerSymbol, Players[] allPlayers, Players currentPlayer)
        {
            Players otherPlayer = new Players();
            for (int i = 15; i < 32; i++)
                if (allPlayers[i].currentPosition == "" + Trow + Tcol)
                {
                    otherPlayer = allPlayers[i];
                    break;
                }

            if (Trow == Frow && Tcol < Fcol)
            {


                int index = Tcol + 1;
                for (int i = index; i < Fcol; i++)
                    if (board[Frow, i] != " EE" || gameOn.ischeck(allPlayers, board, playerSymbol[1], "" + Frow + i))
                    {
                        Console.WriteLine("You cant do 'Hatzraha' because the king has been checked or the road unsafe");
                        return (board, false);
                    }

                board[Frow, Fcol] = " EE";
                board[Trow, Tcol] = " EE";
                if (playerSymbol[2] == 'M')
                {
                    board[Trow, 3] = playerSymbol;
                    board[Trow, 4] = " " + playerSymbol[1] + "R";
                    currentPlayer.checkIfMove3TimeToTheSamePosition(Trow, 3, allPlayers, currentPlayer);
                    otherPlayer.checkIfMove3TimeToTheSamePosition(Trow, 4, allPlayers, currentPlayer);


                }
                else
                {
                    board[Trow, 6] = playerSymbol;
                    board[Trow, 7] = " " + playerSymbol[1] + "M";
                    currentPlayer.checkIfMove3TimeToTheSamePosition(Trow, 6, allPlayers, currentPlayer);
                    otherPlayer.checkIfMove3TimeToTheSamePosition(Trow, 7, allPlayers, currentPlayer);
                }

                return (board, true);
            }




            if (Frow == Trow && Tcol > Fcol)
            {
                int index = Fcol + 1;
                for (int i = index; i < Tcol; i++)
                    if (board[Frow, i] != " EE" || gameOn.ischeck(allPlayers, board, playerSymbol[1], "" + Frow + i))
                    {
                        Console.WriteLine("You cant do 'Hatzraha' because the king has been checked or the road unsafe");
                        return (board, false);
                    }

                board[Frow, Fcol] = " EE";
                board[Trow, Tcol] = " EE";
                if (playerSymbol[2] == 'R')
                {
                    board[Trow, 4] = playerSymbol;
                    board[Trow, 3] = " " + playerSymbol[1] + "M";
                    currentPlayer.checkIfMove3TimeToTheSamePosition(Trow, 4, allPlayers, currentPlayer);
                    otherPlayer.checkIfMove3TimeToTheSamePosition(Trow, 3, allPlayers, currentPlayer);
                }
                else
                {
                    board[Trow, 7] = playerSymbol;
                    board[Trow, 6] = " " + playerSymbol[1] + "R";
                    currentPlayer.checkIfMove3TimeToTheSamePosition(Trow, 7, allPlayers, currentPlayer);
                    otherPlayer.checkIfMove3TimeToTheSamePosition(Trow, 6, allPlayers, currentPlayer);
                }
                return (board, true);
            }
            return (board, false);
        }



    }

    class Queen : Players
    {

        public Queen(string name, string currentPosition) : base(name, currentPosition)
        {

        }



        //פונקציית תנועה
        public static (string[,], bool) move(string[] move, string[,] board, char colorTurn, Players player, Players[] allPlayers)
        {
            return Queen.move(move, board, colorTurn, player, allPlayers, false);
        }
        public static (string[,], bool) move(string[] move, string[,] board, char colorTurn, Players player, Players[] allPlayers, bool iscallByChekFunction)
        {
            string moveFrom = move[0];
            int Frow = int.Parse(moveFrom[0] + "");
            int Fcol = int.Parse(moveFrom[1] + "");
            string moveTo = move[1];
            int Trow = int.Parse(moveTo[0] + "");
            int Tcol = int.Parse(moveTo[1] + "");
            char rivelColor = board[Trow, Tcol][1];
            bool valid;


            //בודק האם תזוזה בודדת ימינה שמאלה למעלה למטה תקנית ואלכסון
            if (((1 == Math.Abs(Trow - Frow) && Fcol == Tcol) || (1 == Math.Abs(Tcol - Fcol) && Frow == Trow) || (1 == Math.Abs(Trow - Frow) && 1 == Math.Abs(Tcol - Fcol))) && rivelColor != colorTurn)
            {

                (board, valid) = gameOn.isCheckOnThisMove(allPlayers, player, board, rivelColor, colorTurn, Frow, Fcol, Trow, Tcol);
                if (valid)
                    return (board, false);

                if (!iscallByChekFunction)
                    gameOn.insignificantMove(colorTurn, Trow, Tcol, board);
                board[Frow, Fcol] = iscallByChekFunction ? player.symbol : " EE";
                board[Trow, Tcol] = iscallByChekFunction ? " EE" : player.symbol;
                if (!iscallByChekFunction)
                    player.checkIfMove3TimeToTheSamePosition(Trow, Tcol, allPlayers, player);
                return (board, true);
            }

            // תנועה אופקית
            // ימינה
            if ((Tcol > Fcol && Trow == Frow) && rivelColor != colorTurn)
            {
                int index = Fcol + 1;
                for (int i = index; i < Tcol; i++)
                    if (board[Frow, i] != " EE")
                    {

                        return (board, false);

                    }
                (board, valid) = gameOn.isCheckOnThisMove(allPlayers, player, board, rivelColor, colorTurn, Frow, Fcol, Trow, Tcol);
                if (valid)
                    return (board, false);
                if (!iscallByChekFunction)
                    gameOn.insignificantMove(colorTurn, Trow, Tcol, board);
                board[Frow, Fcol] = iscallByChekFunction ? player.symbol : " EE";
                board[Trow, Tcol] = iscallByChekFunction ? " EE" : player.symbol;
                if (!iscallByChekFunction)
                    player.checkIfMove3TimeToTheSamePosition(Trow, Tcol, allPlayers, player);
                return (board, true);
            }
            //שמאלה
            if ((Fcol > Tcol && Trow == Frow) && rivelColor != colorTurn)
            {
                int index = Tcol + 1;
                for (int i = index; i < Fcol; i++)
                    if (board[Frow, i] != " EE")
                    {

                        return (board, false);
                    }
                (board, valid) = gameOn.isCheckOnThisMove(allPlayers, player, board, rivelColor, colorTurn, Frow, Fcol, Trow, Tcol);
                if (valid)
                    return (board, false);
                if (!iscallByChekFunction)
                    gameOn.insignificantMove(colorTurn, Trow, Tcol, board);
                board[Frow, Fcol] = iscallByChekFunction ? player.symbol : " EE";
                board[Trow, Tcol] = iscallByChekFunction ? " EE" : player.symbol;
                if (!iscallByChekFunction)
                    player.checkIfMove3TimeToTheSamePosition(Trow, Tcol, allPlayers, player);
                return (board, true);
            }

            // תנועה אנכית 
            //למטה
            if ((Trow > Frow && Tcol == Fcol) && rivelColor != colorTurn)
            {
                int index = Frow + 1;
                for (int i = index; i < Trow; i++)
                    if (board[i, Fcol] != " EE")
                    {

                        return (board, false);
                    }
                (board, valid) = gameOn.isCheckOnThisMove(allPlayers, player, board, rivelColor, colorTurn, Frow, Fcol, Trow, Tcol);
                if (valid)
                    return (board, false);
                if (!iscallByChekFunction)
                    gameOn.insignificantMove(colorTurn, Trow, Tcol, board);
                board[Frow, Fcol] = iscallByChekFunction ? player.symbol : " EE";
                board[Trow, Tcol] = iscallByChekFunction ? " EE" : player.symbol;
                if (!iscallByChekFunction)
                    player.checkIfMove3TimeToTheSamePosition(Trow, Tcol, allPlayers, player);
                return (board, true);
            }
            //למעלה
            if ((Frow > Trow && Tcol == Fcol) && rivelColor != colorTurn)
            {
                int index = Trow + 1;
                for (int i = index; i < Frow; i++)
                    if (board[i, Fcol] != " EE")
                    {

                        return (board, false);
                    }

                (board, valid) = gameOn.isCheckOnThisMove(allPlayers, player, board, rivelColor, colorTurn, Frow, Fcol, Trow, Tcol);
                if (valid)
                    return (board, false);
                if (!iscallByChekFunction)
                    gameOn.insignificantMove(colorTurn, Trow, Tcol, board);
                board[Frow, Fcol] = iscallByChekFunction ? player.symbol : " EE";
                board[Trow, Tcol] = iscallByChekFunction ? " EE" : player.symbol;
                if (!iscallByChekFunction)
                    player.checkIfMove3TimeToTheSamePosition(Trow, Tcol, allPlayers, player);
                return (board, true);
            }

            //אלכסון שמאלה למעלה
            if ((Fcol - Tcol) == (Frow - Trow))
            {
                for (int r = Frow - 1, c = Fcol - 1; c > Tcol && r > Trow; c--, r--)
                {
                    if (board[r, c] != " EE")
                    {

                        return (board, false);
                    }
                }
                if (rivelColor != colorTurn)
                {
                    (board, valid) = gameOn.isCheckOnThisMove(allPlayers, player, board, rivelColor, colorTurn, Frow, Fcol, Trow, Tcol);
                    if (valid)
                        return (board, false);
                    if (!iscallByChekFunction)
                        gameOn.insignificantMove(colorTurn, Trow, Tcol, board);
                    board[Frow, Fcol] = iscallByChekFunction ? player.symbol : " EE";
                    board[Trow, Tcol] = iscallByChekFunction ? " EE" : player.symbol;
                    if (!iscallByChekFunction)
                        player.checkIfMove3TimeToTheSamePosition(Trow, Tcol, allPlayers, player);
                    return (board, true);
                }
                else
                {
                    return (board, false);
                }
            }

            //אלכסון ימינה למעלה
            if ((Frow > Trow && Fcol < Tcol) && Frow + Fcol == Trow + Tcol)
            {

                for (int r = Frow - 1, c = Fcol + 1; r > Trow && c < Tcol; r--, c++)
                    if (board[r, c] != " EE")
                    {

                        return (board, false);
                    }

                if (rivelColor != colorTurn)
                {
                    (board, valid) = gameOn.isCheckOnThisMove(allPlayers, player, board, rivelColor, colorTurn, Frow, Fcol, Trow, Tcol);
                    if (valid)
                        return (board, false);
                    if (!iscallByChekFunction)
                        gameOn.insignificantMove(colorTurn, Trow, Tcol, board);
                    board[Frow, Fcol] = iscallByChekFunction ? player.symbol : " EE";
                    board[Trow, Tcol] = iscallByChekFunction ? " EE" : player.symbol;
                    if (!iscallByChekFunction)
                        player.checkIfMove3TimeToTheSamePosition(Trow, Tcol, allPlayers, player);
                    return (board, true);
                }
                else
                {
                    return (board, false);
                }

            }

            //אלכסון ימינה למטה
            if ((Tcol - Fcol) == (Trow - Frow))
            {
                for (int r = Frow + 1, c = Fcol + 1; c < Tcol && r < Trow; c++, r++)
                {
                    if (board[r, c] != " EE")
                    {

                        return (board, false);
                    }
                }
                if (rivelColor != colorTurn)
                {
                    (board, valid) = gameOn.isCheckOnThisMove(allPlayers, player, board, rivelColor, colorTurn, Frow, Fcol, Trow, Tcol);
                    if (valid)
                        return (board, false);
                    if (!iscallByChekFunction)
                        gameOn.insignificantMove(colorTurn, Trow, Tcol, board);
                    board[Frow, Fcol] = iscallByChekFunction ? player.symbol : " EE";
                    board[Trow, Tcol] = iscallByChekFunction ? " EE" : player.symbol;
                    if (!iscallByChekFunction)
                        player.checkIfMove3TimeToTheSamePosition(Trow, Tcol, allPlayers, player);
                    return (board, true);
                }
                else
                {
                    return (board, false);
                }
            }

            //אלכסון שמאלה למטה
            if ((Frow < Trow && Fcol > Tcol) && Frow + Fcol == Trow + Tcol)
            {

                for (int r = Frow + 1, c = Fcol - 1; r > Trow && c < Tcol; r++, c--)
                {
                    if (board[r, c] != " EE")
                    {

                        return (board, false);
                    }
                }
                if (rivelColor != colorTurn)
                {
                    (board, valid) = gameOn.isCheckOnThisMove(allPlayers, player, board, rivelColor, colorTurn, Frow, Fcol, Trow, Tcol);
                    if (valid)
                        return (board, false);
                    if (!iscallByChekFunction)
                        gameOn.insignificantMove(colorTurn, Trow, Tcol, board);
                    board[Frow, Fcol] = iscallByChekFunction ? player.symbol : " EE";
                    board[Trow, Tcol] = iscallByChekFunction ? " EE" : player.symbol;
                    if (!iscallByChekFunction)
                        player.checkIfMove3TimeToTheSamePosition(Trow, Tcol, allPlayers, player);
                    return (board, true);
                }
                else
                {
                    return (board, false);
                }

            }
            return (board, false);
        }
    }

    class Bishop : Players
    {

        public Bishop(string name, string currentPosition) : base(name, currentPosition)
        {

        }


        //פונקציית תנועה
        public static (string[,], bool) move(string[] move, string[,] board, char colorTurn, Players player, Players[] allPlayers)
        {
            return Bishop.move(move, board, colorTurn, player, allPlayers, false);
        }
        public static (string[,], bool) move(string[] move, string[,] board, char colorTurn, Players player, Players[] allPlayers, bool iscallByChekFunction)
        {
            string moveFrom = move[0];
            int Frow = int.Parse(moveFrom[0] + "");
            int Fcol = int.Parse(moveFrom[1] + "");
            string moveTo = move[1];
            int Trow = int.Parse(moveTo[0] + "");
            int Tcol = int.Parse(moveTo[1] + "");
            char rivelColor = board[Trow, Tcol][1];
            bool valid;


            //האם זה תנועה קצרה
            if (Math.Abs(Frow - Trow) == 1 && Math.Abs(Fcol - Tcol) == 1)
            {
                if (rivelColor != colorTurn)
                {
                    (board, valid) = gameOn.isCheckOnThisMove(allPlayers, player, board, rivelColor, colorTurn, Frow, Fcol, Trow, Tcol);
                    if (valid)
                        return (board, false);
                    if (!iscallByChekFunction)
                        gameOn.insignificantMove(colorTurn, Trow, Tcol, board);
                    board[Frow, Fcol] = iscallByChekFunction ? player.symbol : " EE";
                    board[Trow, Tcol] = iscallByChekFunction ? " EE" : player.symbol;
                    if (!iscallByChekFunction)
                        player.checkIfMove3TimeToTheSamePosition(Trow, Tcol, allPlayers, player);
                    return (board, true);
                }
                else
                {
                    return (board, false);
                }
            }

            //אלכסון שמאלה למעלה
            if ((Fcol - Tcol) == (Frow - Trow))
            {
                for (int r = Frow - 1, c = Fcol - 1; c > Tcol && r > Trow; c--, r--)
                {
                    if (board[r, c] != " EE")
                    {

                        return (board, false);
                    }
                }
                if (rivelColor != colorTurn)
                {
                    (board, valid) = gameOn.isCheckOnThisMove(allPlayers, player, board, rivelColor, colorTurn, Frow, Fcol, Trow, Tcol);
                    if (valid)
                        return (board, false);
                    if (!iscallByChekFunction)
                        gameOn.insignificantMove(colorTurn, Trow, Tcol, board);
                    board[Frow, Fcol] = iscallByChekFunction ? player.symbol : " EE";
                    board[Trow, Tcol] = iscallByChekFunction ? " EE" : player.symbol;
                    if (!iscallByChekFunction)
                        player.checkIfMove3TimeToTheSamePosition(Trow, Tcol, allPlayers, player);
                    return (board, true);
                }
                else
                {

                    return (board, false);
                }
            }

            //אלכסון ימינה למעלה
            if ((Frow > Trow && Fcol < Tcol) && Frow + Fcol == Trow + Tcol)
            {

                for (int r = Frow - 1, c = Fcol + 1; r > Trow && c < Tcol; r--, c++)
                    if (board[r, c] != " EE")
                    {

                        return (board, false);
                    }

                if (rivelColor != colorTurn)
                {
                    (board, valid) = gameOn.isCheckOnThisMove(allPlayers, player, board, rivelColor, colorTurn, Frow, Fcol, Trow, Tcol);
                    if (valid)
                        return (board, false);
                    if (!iscallByChekFunction)
                        gameOn.insignificantMove(colorTurn, Trow, Tcol, board);
                    board[Frow, Fcol] = iscallByChekFunction ? player.symbol : " EE";
                    board[Trow, Tcol] = iscallByChekFunction ? " EE" : player.symbol;
                    if (!iscallByChekFunction)
                        player.checkIfMove3TimeToTheSamePosition(Trow, Tcol, allPlayers, player);
                    return (board, true);
                }
                else
                {

                    return (board, false);
                }

            }

            //אלכסון ימינה למטה
            if ((Tcol - Fcol) == (Trow - Frow))
            {
                for (int r = Frow + 1, c = Fcol + 1; c < Tcol && r < Trow; c++, r++)
                {
                    if (board[r, c] != " EE")
                    {

                        return (board, false);
                    }
                }
                if (rivelColor != colorTurn)
                {
                    (board, valid) = gameOn.isCheckOnThisMove(allPlayers, player, board, rivelColor, colorTurn, Frow, Fcol, Trow, Tcol);
                    if (valid)
                        return (board, false);
                    if (!iscallByChekFunction)
                        gameOn.insignificantMove(colorTurn, Trow, Tcol, board);
                    board[Frow, Fcol] = iscallByChekFunction ? player.symbol : " EE";
                    board[Trow, Tcol] = iscallByChekFunction ? " EE" : player.symbol;
                    if (!iscallByChekFunction)
                        player.checkIfMove3TimeToTheSamePosition(Trow, Tcol, allPlayers, player);
                    return (board, true);
                }
                else
                {

                    return (board, false);
                }
            }

            //אלכסון שמאלה למטה
            if ((Frow < Trow && Fcol > Tcol) && Frow + Fcol == Trow + Tcol)
            {

                for (int r = (Frow + 1), c = (Fcol - 1); r > Trow && c < Tcol; r++, c--)
                {
                    if (board[r, c] != " EE")
                    {

                        return (board, false);
                    }
                }
                if (rivelColor != colorTurn)
                {
                    (board, valid) = gameOn.isCheckOnThisMove(allPlayers, player, board, rivelColor, colorTurn, Frow, Fcol, Trow, Tcol);
                    if (valid)
                        return (board, false);
                    if (!iscallByChekFunction)
                        gameOn.insignificantMove(colorTurn, Trow, Tcol, board);
                    board[Frow, Fcol] = iscallByChekFunction ? player.symbol : " EE";
                    board[Trow, Tcol] = iscallByChekFunction ? " EE" : player.symbol;
                    if (!iscallByChekFunction)
                        player.checkIfMove3TimeToTheSamePosition(Trow, Tcol, allPlayers, player);
                    return (board, true);
                }
                else
                {

                    return (board, false);
                }

            }
            return (board, false);

        }
    }

    class Knight : Players
    {

        public Knight(string name, string currentPosition) : base(name, currentPosition)
        {

        }

        //פונקציית תנועה
        public static (string[,], bool) move(string[] move, string[,] board, char colorTurn, Players player, Players[] allPlayers)
        {
            return Knight.move(move, board, colorTurn, player, allPlayers, false);
        }
        public static (string[,], bool) move(string[] move, string[,] board, char colorTurn, Players player, Players[] allPlayers, bool iscallByChekFunction)
        {
            string moveFrom = move[0];
            int Frow = int.Parse(moveFrom[0] + "");
            int Fcol = int.Parse(moveFrom[1] + "");
            string moveTo = move[1];
            int Trow = int.Parse(moveTo[0] + "");
            int Tcol = int.Parse(moveTo[1] + "");
            char rivelColor = board[Trow, Tcol][1];
            bool valid;




            //תנועה אפשרית
            if ((2 == Math.Abs(Trow - Frow) && 1 == Math.Abs(Fcol - Tcol)) || (2 == Math.Abs(Tcol - Fcol) && 1 == Math.Abs(Frow - Trow)))
                if (rivelColor != colorTurn)
                {
                    (board, valid) = gameOn.isCheckOnThisMove(allPlayers, player, board, rivelColor, colorTurn, Frow, Fcol, Trow, Tcol);
                    if (valid)
                        return (board, false);

                    if (!iscallByChekFunction)
                        gameOn.insignificantMove(colorTurn, Trow, Tcol, board);

                    board[Frow, Fcol] = iscallByChekFunction ? player.symbol : " EE";
                    board[Trow, Tcol] = iscallByChekFunction ? " EE" : player.symbol;
                    if (!iscallByChekFunction)
                        player.checkIfMove3TimeToTheSamePosition(Trow, Tcol, allPlayers, player);
                    return (board, true);
                }
            return (board, false);
        }


    }

    class Rook : Players
    {
        public Rook(string name, string currentPosition) : base(name, currentPosition)
        {

        }

        //פונקציית תנועה
        public static (string[,], bool) move(string[] move, string[,] board, char colorTurn, Players player, Players[] allPlayers)
        {
            return Rook.move(move, board, colorTurn, player, allPlayers, false);
        }
        public static (string[,], bool) move(string[] move, string[,] board, char colorTurn, Players player, Players[] allPlayers, bool iscallByChekFunction)
        {
            string moveFrom = move[0];
            int Frow = int.Parse(moveFrom[0] + "");
            int Fcol = int.Parse(moveFrom[1] + "");
            string moveTo = move[1];
            int Trow = int.Parse(moveTo[0] + "");
            int Tcol = int.Parse(moveTo[1] + "");
            char rivelColor = board[Trow, Tcol][1];
            bool valid = false;









            // תנועה אופקית
            if ((Tcol > Fcol && Trow == Frow) && rivelColor != colorTurn)
            {
                int index = Fcol + 1;
                for (int i = index; i < Tcol; i++)
                    if (board[Frow, i] != " EE")
                    {

                        return (board, false);

                    }
                (board, valid) = gameOn.isCheckOnThisMove(allPlayers, player, board, rivelColor, colorTurn, Frow, Fcol, Trow, Tcol);
                if (valid)
                    return (board, false);

                if (!iscallByChekFunction)
                    gameOn.insignificantMove(colorTurn, Trow, Tcol, board);
                board[Frow, Fcol] = iscallByChekFunction ? player.symbol : " EE";
                board[Trow, Tcol] = iscallByChekFunction ? " EE" : player.symbol;
                if (!iscallByChekFunction)
                    player.checkIfMove3TimeToTheSamePosition(Trow, Tcol, allPlayers, player);
                player.isMoved = true;
                return (board, true);
            }
            if ((Fcol > Tcol && Trow == Frow) && rivelColor != colorTurn)
            {
                int index = Tcol + 1;
                for (int i = index; i < Fcol; i++)
                    if (board[Frow, i] != " EE")
                    {

                        return (board, false);
                    }
                (board, valid) = gameOn.isCheckOnThisMove(allPlayers, player, board, rivelColor, colorTurn, Frow, Fcol, Trow, Tcol);
                if (valid)
                    return (board, false);

                if (!iscallByChekFunction)
                    gameOn.insignificantMove(colorTurn, Trow, Tcol, board);
                board[Frow, Fcol] = iscallByChekFunction ? player.symbol : " EE";
                board[Trow, Tcol] = iscallByChekFunction ? " EE" : player.symbol;
                if (!iscallByChekFunction)
                    player.checkIfMove3TimeToTheSamePosition(Trow, Tcol, allPlayers, player);
                player.isMoved = true;

                return (board, true);
            }



            // תנועה אנכית
            // למטה
            if ((Trow > Frow && Tcol == Fcol) && rivelColor != colorTurn)
            {
                int index = Frow;
                for (int i = index + 1; i < Trow; i++)
                    if (board[i, Fcol] != " EE")
                    {

                        return (board, false);
                    }
                (board, valid) = gameOn.isCheckOnThisMove(allPlayers, player, board, rivelColor, colorTurn, Frow, Fcol, Trow, Tcol);
                if (valid)
                    return (board, false);
                if (!iscallByChekFunction)
                    gameOn.insignificantMove(colorTurn, Trow, Tcol, board);
                board[Frow, Fcol] = iscallByChekFunction ? player.symbol : " EE";
                board[Trow, Tcol] = iscallByChekFunction ? " EE" : player.symbol;
                if (!iscallByChekFunction)
                    player.checkIfMove3TimeToTheSamePosition(Trow, Tcol, allPlayers, player);
                player.isMoved = true;
                return (board, true);
            }
            // למעלה
            if ((Frow > Trow && Tcol == Fcol) && rivelColor != colorTurn)
            {
                int index = Frow;
                for (int i = index - 1; i > Trow; i--)
                    if (board[i, Fcol] != " EE")
                    {

                        return (board, false);
                    }
                (board, valid) = gameOn.isCheckOnThisMove(allPlayers, player, board, rivelColor, colorTurn, Frow, Fcol, Trow, Tcol);
                if (valid)
                    return (board, false);

                if (!iscallByChekFunction)
                    gameOn.insignificantMove(colorTurn, Trow, Tcol, board);
                board[Frow, Fcol] = iscallByChekFunction ? player.symbol : " EE";
                board[Trow, Tcol] = iscallByChekFunction ? " EE" : player.symbol;
                if (!iscallByChekFunction)
                    player.checkIfMove3TimeToTheSamePosition(Trow, Tcol, allPlayers, player);
                player.isMoved = true;
                return (board, true);
            }



            Players king = new Players();
            for (int i = 30; i < 32; i++)
                if (allPlayers[i].currentPosition == moveTo && allPlayers[i].symbol[2] == 'M')
                {
                    king = allPlayers[i];

                    if (player.isMoved == false && king.isMoved == false)
                    {
                        switch ("" + Trow + Tcol)
                        {
                            case "15":
                                valid = (!Melech.checkOnBlack);
                                break;
                            case "85":
                                valid = (!Melech.checkOnWhite);
                                break;
                            default:
                                valid = false;
                                break;
                        }
                        if (valid)
                        {
                            (board, valid) = Melech.hatzraca(board, Trow, Tcol, Frow, Fcol, player.symbol, allPlayers, player);
                            if (valid)
                            {
                                player.isMoved = true;
                                king.isMoved = true;
                                if (colorTurn == 'W')
                                    gameOn.countInsignificantMoveWhite++;
                                else
                                    gameOn.countInsignificantMoveBlack++;
                                return (board, true);
                            }
                            else
                                return (board, false);
                        }
                    }

                    break;
                }
            return (board, false);
        }




    }


    class Pawn : Players
    {



        public Pawn(string name, string currentPosition) : base(name, currentPosition)
        {

        }


        //פונקציית תנועה
        public static (string[,], bool) move(string[] move, string[,] board, bool isWhite, Players player, Players[] allPlayers)
        {
            return Pawn.move(move, board, isWhite, player, allPlayers, false);
        }
        public static (string[,], bool) move(string[] move, string[,] board, bool isWhite, Players player, Players[] allPlayers, bool iscallByChekFunction)
        {
            char colorTurn = isWhite ? 'W' : 'B';
            char rivalColor = isWhite ? 'B' : 'W';
            string moveFrom = move[0];
            int Frow = int.Parse(moveFrom[0] + "");
            int Fcol = int.Parse(moveFrom[1] + "");
            string moveTo = move[1];
            int Trow = int.Parse(moveTo[0] + "");
            int Tcol = int.Parse(moveTo[1] + "");
            bool valid;


            //צעד אחד קדימה
            if ((1 == (isWhite ? Frow - Trow : Trow - Frow) && Fcol == Tcol) && board[Trow, Tcol] == " EE")
            {
                (board, valid) = gameOn.isCheckOnThisMove(allPlayers, player, board, rivalColor, colorTurn, Frow, Fcol, Trow, Tcol);
                if (valid)
                    return (board, false);
                player.pawlJumpNext2 = false;
                board[Frow, Fcol] = iscallByChekFunction ? player.symbol : " EE";
                board[Trow, Tcol] = iscallByChekFunction ? " EE" : player.symbol; ;
                if (colorTurn == 'W')
                    gameOn.countInsignificantMoveWhite = 0;
                else
                    gameOn.countInsignificantMoveBlack = 0;
                if (!iscallByChekFunction)
                    player.checkIfMove3TimeToTheSamePosition(Trow, Tcol, allPlayers, player);
                if ((Trow == 1 || Trow == 8) && !iscallByChekFunction)
                {
                    board[Trow, Tcol] = choosePlayerSymbol(colorTurn, player);
                }
                return (board, true);
            }


            //שני צעדים קדימה כאשר נמצא בשורה 2 או 7
            if ((2 == (isWhite ? Frow - Trow : Trow - Frow) && Fcol == Tcol) && (Frow == (isWhite ? 7 : 2) && board[(Frow + (isWhite ? -2 : 2)), Fcol] == " EE"))
            {
                (board, valid) = gameOn.isCheckOnThisMove(allPlayers, player, board, rivalColor, colorTurn, Frow, Fcol, Trow, Tcol);
                if (valid)
                    return (board, false);

                if ((Tcol + 1 > 8 ? false : board[Trow, Tcol + 1] == " " + rivalColor + 'P') || (Tcol - 1 < 1 ? false : board[Trow, Tcol - 1] == " " + rivalColor + 'P'))
                {
                    player.pawlJumpNext2 = true;
                }
                board[Frow, Fcol] = iscallByChekFunction ? player.symbol : " EE";
                board[Trow, Tcol] = iscallByChekFunction ? " EE" : player.symbol; ;

                if (colorTurn == 'W')
                    gameOn.countInsignificantMoveWhite = 0;
                else
                    gameOn.countInsignificantMoveBlack = 0;
                if (!iscallByChekFunction)
                    player.checkIfMove3TimeToTheSamePosition(Trow, Tcol, allPlayers, player);
                return (board, true);
            }


            // אכילת שחקן באלכסון
            if ((1 == Math.Abs(Trow - Frow) && 1 == Math.Abs(Fcol - Tcol)) && (board[Trow, Tcol] != " EE" && rivalColor != colorTurn))
            {
                (board, valid) = gameOn.isCheckOnThisMove(allPlayers, player, board, rivalColor, colorTurn, Frow, Fcol, Trow, Tcol);
                if (valid)
                    return (board, false);

                player.pawlJumpNext2 = false;
                board[Frow, Fcol] = iscallByChekFunction ? player.symbol : " EE";
                board[Trow, Tcol] = iscallByChekFunction ? " EE" : player.symbol;
                if (colorTurn == 'W')
                    gameOn.countInsignificantMoveWhite = 0;
                else
                    gameOn.countInsignificantMoveBlack = 0;

                if (!iscallByChekFunction)
                    player.checkIfMove3TimeToTheSamePosition(Trow, Tcol, allPlayers, player);
                if ((Trow == 1 || Trow == 8) && !iscallByChekFunction)
                {
                    board[Trow, Tcol] = choosePlayerSymbol(colorTurn, player);
                }
                return (board, true);
            }

            // אכילת שחקן באלכסון שנמצא לידי לאחר קפיצה של 2
            if ((1 == Math.Abs(Trow - Frow) && 1 == Math.Abs(Fcol - Tcol)) && (board[Trow, Tcol] == " EE" && rivalColor != colorTurn))
            {

                for (int i = 0; i < 16; i++)
                {
                    if ((allPlayers[i].currentPosition == "" + Frow + (Fcol + 1) && allPlayers[i].symbol[1] == rivalColor) && allPlayers[i].pawlJumpNext2)
                    {
                        (board, valid) = gameOn.isCheckOnThisMove(allPlayers, player, board, rivalColor, colorTurn, Frow, Fcol, Trow, Tcol);
                        if (valid)
                            return (board, false);

                        player.pawlJumpNext2 = false;
                        allPlayers[i].currentPosition = "00";
                        board[Frow, Fcol] = " EE";
                        board[Frow, Fcol + 1] = " EE";
                        board[Trow, Tcol] = player.symbol;
                        if (colorTurn == 'W')
                            gameOn.countInsignificantMoveWhite = 0;
                        else
                            gameOn.countInsignificantMoveBlack = 0;

                        if (!iscallByChekFunction)
                            player.checkIfMove3TimeToTheSamePosition(Trow, Tcol, allPlayers, player);
                        return (board, true);
                    }
                    else if ((allPlayers[i].currentPosition == "" + Frow + (Fcol - 1) && allPlayers[i].symbol[1] == rivalColor) && allPlayers[i].pawlJumpNext2)
                    {
                        player.pawlJumpNext2 = false;
                        allPlayers[i].currentPosition = "00";
                        if (iscallByChekFunction)
                            board[Frow, Fcol - 1] = board[Trow, Tcol];
                        else
                            board[Frow, Fcol - 1] = " EE";
                        board[Frow, Fcol] = iscallByChekFunction ? player.symbol : " EE";
                        board[Trow, Tcol] = iscallByChekFunction ? " EE" : player.symbol;
                        if (colorTurn == 'W')
                            gameOn.countInsignificantMoveWhite = 0;
                        else
                            gameOn.countInsignificantMoveBlack = 0;

                        if (!iscallByChekFunction)
                            player.checkIfMove3TimeToTheSamePosition(Trow, Tcol, allPlayers, player);
                        return (board, true);
                    }
                }

            }



            // ברירת מחדל
            return (board, false);
        }

        // פונקציה שמחליפה חייל שהגיעה לסוף
        static string choosePlayerSymbol(char color, Players player)
        {
            Console.WriteLine("Your Pawn arrived to the end!! You can choose a new soldier:  Rook | Knight | Bishop | Queen | Pawn");
            string playerSymbol = " " + color;
            string choise = Console.ReadLine().Trim();
            switch (choise.ToLower())
            {
                case "pawn":
                    playerSymbol = playerSymbol + 'P';
                    player.symbol = playerSymbol;
                    break;
                case "rook":
                    playerSymbol = playerSymbol + 'R';
                    player.symbol = playerSymbol;
                    break;
                case "queen":
                    playerSymbol = playerSymbol + 'Q';
                    player.symbol = playerSymbol;
                    break;
                case "knight":
                    playerSymbol = playerSymbol + 'K';
                    player.symbol = playerSymbol;
                    break;
                case "bishop":
                    playerSymbol = playerSymbol + 'B';
                    player.symbol = playerSymbol;
                    break;
                default:
                    choosePlayerSymbol(color, player);
                    break;

            }
            return playerSymbol;
        }


    }

}