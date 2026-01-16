using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZombieSurvival
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.Title = "🧟 좀비 서바이벌"; // 콘솔 제목
            Console.ForegroundColor = ConsoleColor.Red;
            Console.CursorVisible = false;

            // GameManager 생성
            GameManager game = new GameManager();

            // 게임 시작
            game.Start();

            // 종료 대기 (엔딩 메시지 확인용)
            Console.WriteLine("\n아무 키나 누르면 종료됩니다...");
            Console.ReadKey();
        }

        
    }
}

