using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleTextRPG.Data;

namespace ConsoleTextRPG.Scenes
{
    internal class Store
    {
        List<Item> inventory = new List<Item>();

        public static void Itemdisplay()
        {
            Console.Clear();
            Console.WriteLine("상점");
            Console.WriteLine("[보유 골드]");
            Console.WriteLine($" G");
            Console.WriteLine();
            Console.WriteLine("[아이템 목록]");
            Console.WriteLine();
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine(i);
            }
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("1. 아이템 구매");
            Console.WriteLine("0. 나가기");
            Console.WriteLine();
            Console.Write("원하시는 행동을 입력해주세요.\n>>");
            string input = Console.ReadLine();
            if (input == "0")
            {
                Console.Clear();
                return;
            }
            else if (input == "1")
            {
                Console.Clear();
                Itembuy();
            }
            else
            {
                Console.Clear();
                Console.WriteLine("잘못된 입력입니다.");
                Console.WriteLine();
            }
        }
        public static void Itembuy()
        {
            Console.Clear();
            Console.WriteLine("상점 - 구매중");
            Console.WriteLine("[보유 골드]");
            Console.WriteLine($" G");
            Console.WriteLine();
            Console.WriteLine("[아이템 목록]");
            Console.WriteLine("원하는 번호로 아이템 구매");

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("0. 나가기");
            Console.WriteLine();
            Console.Write("원하시는 행동을 입력해주세요.\n>>");
            string input = Console.ReadLine();
            if (input == "0")
            {
                Console.Clear();
                return;
            }
            else if (input == "1")
            {
                Console.Clear();

            }
            else
            {
                Console.Clear();
                Console.WriteLine("잘못된 입력입니다.");
                Console.WriteLine();
            }
        }
    }

}
