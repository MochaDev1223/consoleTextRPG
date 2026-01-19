using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ZombieSurvival
{
    class GameManager
    {
        Player player;
        EventManager eventManager;
        bool isRunning = true;
        const int MaxActionsPerDay = 5;
        const int HudHeight = 6;


        // ===== 게임 시작 =====
        public void Start()
        {
            ShowLoadingScreen();
            ShowIntro();

            player = new Player();
            eventManager = new EventManager();
            Console.WriteLine("🧟 좀비 생존 시뮬레이터 시작!");
            Console.WriteLine("당신은 이 세계에서 얼마나 버틸 수 있을까요?\n");
            //player.OnDamaged += ScreenShake;
            GameLoop();
        }

        void ShowLoadingScreen()
        {
            Console.Clear();
            Console.CursorVisible = false;

            string loading = "Loading...";
            int width = Math.Max(1, Console.WindowWidth);
            int height = Math.Max(1, Console.WindowHeight);

            int startX = Math.Max(0, (width - loading.Length) / 2);
            int centerY = Math.Max(0, height / 2);

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.SetCursorPosition(startX, centerY);
            Console.Write(loading);

            // 키 버퍼 비우기
            while (Console.KeyAvailable)
                Console.ReadKey(true);

            // Spacebar 대기
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey(true);
            } while (key.Key != ConsoleKey.Spacebar);

            Console.Clear();
            Console.ResetColor();
        }


        void ShowIntro()
        {
            Console.Clear();
            Console.CursorVisible = false;

            string intro = "ZOMBIE SURVIVAL...";
            int width = Math.Max(1, Console.WindowWidth);
            int height = Math.Max(1, Console.WindowHeight);
            int startX = Math.Max(0, (width - intro.Length) / 2);
            int centerY = Math.Max(0, height / 2);

            // 한 글자씩 출력
            Console.ForegroundColor = ConsoleColor.Red;
            for (int i = 0; i < intro.Length; i++)
            {
                int x = startX + i;
                if (x >= 0 && x < Console.BufferWidth && centerY >= 0 && centerY < Console.BufferHeight)
                    Console.SetCursorPosition(x, centerY);
                Console.Write(intro[i]);
                Thread.Sleep(100); // 글자 간 딜레이 (100ms)
            }

            // 모두 출력된 후 3초 대기
            Thread.Sleep(1500);

            // 두 줄 아래에 "Press Spacebar" 표시
            string prompt = "Press Spacebar";
            int promptStartX = Math.Max(0, (width - prompt.Length) / 2);
            int promptY = Math.Min(Console.BufferHeight - 1, centerY + 2);
            if (promptStartX >= 0 && promptStartX < Console.BufferWidth && promptY >= 0 && promptY < Console.BufferHeight)
                Console.SetCursorPosition(promptStartX, promptY);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(prompt);

            // 기존 키 입력 버퍼 비움
            while (Console.KeyAvailable)
                Console.ReadKey(true);

            // 스페이스바 입력 대기
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey(true);
            } while (key.Key != ConsoleKey.Spacebar);

            // 진행 전 화면 정리
            Console.Clear();
            Console.ResetColor();
        }

        void ShowSleepSequence()
        {
            Console.Clear();
            Console.CursorVisible = false;

            string[] lines = new[]
            {
                    "🌙 당신은 잠자리에 듭니다...",
                    "하루가 지나갔습니다.",
                    "개운하게 잠이 들고 체력을 회복했습니다."
            };

            int width = Math.Max(1, Console.WindowWidth);
            int height = Math.Max(1, Console.WindowHeight);
            int startY = Math.Max(0, (height - lines.Length) / 2);

            Console.ForegroundColor = ConsoleColor.Yellow;

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                int startX = Math.Max(0, (width - line.Length) / 2);
                int y = startY + i;

                if (startX >= 0 && startX < Console.BufferWidth && y >= 0 && y < Console.BufferHeight)
                    Console.SetCursorPosition(startX, y);
                Console.Write(line);

                // 각 줄 출력 후 잠깐 대기 (한 줄씩 엔터 효과)
                Thread.Sleep(1000);
            }

            // 마지막 줄 출력된 후 2초 대기
            Thread.Sleep(2000);

            Console.Clear();
            Console.ResetColor();
        }

        // ===== 메인 게임 루프 =====
        void GameLoop()
        {
            while (isRunning && player.IsAlive())
            {
                Console.Clear();
                DrawHUD();

                // 👇 HUD 아래에서부터 출력 시작
                Console.SetCursorPosition(0, HudHeight);

                HandleInput();
                CheckEnding();

                Console.WriteLine("\n(계속하려면 아무 키나 누르세요)");
                Console.ReadKey(true);
            }

            Console.WriteLine("\n게임이 종료되었습니다.");
        }


        void DrawHUD()
        {
            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = ConsoleColor.White;

            DrawBar("Day", player.Day, 7, 20, ConsoleColor.Magenta);
            DrawBar("HP", player.Hp, player.MaxHp, 20, ConsoleColor.Red);
            DrawBar("Food", player.Food, 10, 20, ConsoleColor.Green);
            DrawBar("Ammo", player.Ammo, 50, 20, ConsoleColor.Yellow);
            DrawBar("Day Progress", player.ActionCount, MaxActionsPerDay, 20, ConsoleColor.Cyan);

            Console.WriteLine(new string('-', Console.WindowWidth));
            Console.ResetColor();
        }


        void DrawBar(string label, int value, int max, int barWidth, ConsoleColor color)
        {
            if (max <= 0) max = 1;
            if (value < 0) value = 0;
            if (value > max) value = max;

            float ratio = (float)value / max;
            if (ratio < 0f) ratio = 0f;
            if (ratio > 1f) ratio = 1f;

            int filled = (int)(ratio * barWidth);
            if (filled < 0) filled = 0;
            if (filled > barWidth) filled = barWidth;

            Console.Write($"{label,-12}[");
            Console.ForegroundColor = color;
            Console.Write(new string('█', filled));
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write(new string('░', barWidth - filled));
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"] {value}/{max}");
        }



        //public void ScreenShake()
        //{
        //    int shakeCount = 6;
        //    int offset = 2;

        //    for (int i = 0; i < shakeCount; i++)
        //    {
        //        Console.Clear();
        //        Console.SetCursorPosition(i % 2 == 0 ? offset : 0, 0);
        //        DrawHUD();
        //        Thread.Sleep(40);
        //    }

        //    Console.Clear();
        //}



        // ===== 상태 출력 =====
        //void PrintStatus()
        //{
        //    Console.WriteLine("================================");
        //    Console.WriteLine($"ㄴDay {player.Day}");
        //    Console.WriteLine($"HP   : {player.Hp} / {player.MaxHp}");
        //    Console.WriteLine($"Food : {player.Food}");
        //    Console.WriteLine($"Ammo : {player.Ammo}");
        //    Console.WriteLine($"Action : {player.ActionCount}");
        //    Console.WriteLine("================================");
        //}

        // ===== 입력 및 선택 처리 =====
        void HandleInput()
        {
            Console.WriteLine("\n오늘 무엇을 하시겠습니까?");
            Console.WriteLine("1. 탐색한다");
            Console.WriteLine("2. 쉰다");
            Console.WriteLine("3. 거래한다");
            Console.WriteLine("4. 잠든다");
            Console.Write("\n선택: ");

            string input = Console.ReadLine();
            Console.WriteLine();

            switch (input)
            {
                case "1":
                    Explore();
                    break;
                case "2":
                    Rest();
                    break;
                case "3":
                    Trade();
                    break;
                case "4":
                    Sleep();
                    break;
                default:
                    Console.WriteLine("❌ 잘못된 입력입니다. 아무 일도 일어나지 않았습니다.");
                    break;
            }
        }

        // ===== 행동 1: 탐색 =====
        void Explore()
        {
            //Console.Clear();
            Console.WriteLine("🔎 주변을 탐색합니다...");
            Random rand = new Random();
            int roll = rand.Next(0, 100);


            if (player.Ammo < 5)
            {
                Console.WriteLine("❌ 탐색에 필요한 탄약이 부족합니다.");
            }
            else
            {
                if (roll < 40)
                {
                    Console.WriteLine("🎁 식량을 발견했습니다! (+2)");
                    //player.Food++;
                    player.Food += 2;
                }
                else if (roll < 70)
                {
                    Console.WriteLine("☠️ 좀비와 마주쳤습니다!");
                    player.TakeDamage(15);
                    Console.WriteLine("HP -15");
                }
                else
                {
                    Console.WriteLine("… 아무 일도 일어나지 않았습니다.");
                }
                eventManager.TriggerRandomEvent(player);
                player.ActionCount++;
                player.Ammo -= 5;

            }

        }

        // ===== 행동 2: 휴식 =====
        void Rest()
        {
            //Console.Clear();

            Console.WriteLine("🛌 휴식을 취합니다...");
            if (player.Food > 0)
            {
                player.ConsumeFood();
                player.Heal(10);
                Console.WriteLine("HP +10, Food -1");
                eventManager.TriggerRandomEvent(player);
            }
            else
            {
                Console.WriteLine("🍞 식량이 없어 회복하지 못했습니다.");
            }

            player.ActionCount++;
        }

        // ===== 행동 3: 거래 =====
        void Trade()
        {
            //Console.Clear();
            Console.WriteLine("🔁 생존자와 거래를 시도합니다...");
            if (player.Food >= 2)
            {
                player.Food -= 2;
                player.Ammo += 5;
                Console.WriteLine("Food -2 → Ammo +5");
                eventManager.TriggerRandomEvent(player);
            }
            else
            {
                Console.WriteLine("🍞 식량이 부족해 거래에 실패했습니다.");
            }

            player.ActionCount++;
        }

        // ===== 행동 4: 잠들기 =====
        void Sleep()
        {
            //Console.Clear();
            if (player.ActionCount < 5)
            {
                Console.WriteLine("😴 아직 너무 이릅니다...");
                Console.WriteLine($"(현재 행동 횟수: {player.ActionCount} / 5)");
                Console.WriteLine("하루를 마치려면 최소 5번의 행동이 필요합니다.");
                return;
            }

            ShowSleepSequence();
            //Console.WriteLine("🌙 당신은 잠자리에 듭니다...");
            //Console.WriteLine("하루가 지나갔습니다.");
            //Console.WriteLine("개운하게 잠이 들고 체력을 회복했습니다.");

            // 하루 종료 처리
            player.Day++;
            player.Heal(20);
            player.ActionCount = 0;   // 🔄 다음 날을 위해 초기화
        }


        // ===== 엔딩 체크 =====
        void CheckEnding()
        {
            if (!player.IsAlive())
            {
                Console.WriteLine("\n💀 당신은 사망했습니다...");
                isRunning = false;
                return;
            }

            if (player.Food == 0 && player.Ammo < 5)
            {
                Console.WriteLine("\n💀 당신은 고립되었습니다...");
                isRunning = false;
                return;
            }

            if (player.Day >= 7)
            {
                Console.WriteLine("\n🏆 7일을 생존했습니다! 당신은 살아남았습니다!");
                isRunning = false;
            }
        }
    }
}