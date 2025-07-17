using ConsoleTextRPG.Data;
using ConsoleTextRPG.Managers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTextRPG.Scenes
{
    internal class ConsoleHelper
    {
        private static int _width = 18;                   //이름 너비 제한
        private static int _statWidth = 12;                //스텟 너비 제한
        private static int _commentWidth = 62;              //설명 너비 제한
        private static int _priceWidth = 5;                  //가격 너비 제한

        //콘솔창의 들쑥날쑥한 모습을 깔끔하게 정리하기 위해 글자수로 쪼개서 TEXT로 만든다음 너비를 제한합니다.
        public static string PadRightKorean(string text, int totalLength)
        {
            int currentLength = 0;
            foreach (char c in text)
            {
                // 한글은 2칸, 영문/숫자는 1칸으로 가정 (폰트에 따라 다를 수 있음)
                if (c >= '\uAC00' && c <= '\uD7A3') // 한글 유니코드 범위
                {
                    currentLength += 2;
                }
                else
                {
                    currentLength += 1;
                }
            }

            if (currentLength >= totalLength)
            {
                // 길이가 이미 총 길이보다 길거나 같으면 잘라냄 (또는 그냥 반환)
                // 여기서는 간단히 잘라내지 않고 반환합니다. 필요에 따라 substring 적용
                return text;
            }
            else
            {
                return text + new string(' ', totalLength - currentLength);
            }
        }

        // 아이템_번호, 아이템_이름, 물약_카운트, 스텟타입, 스텟보너스, 설명, 장착여부, 아이템_타임(아이템_번호-1), 모드
        public static void DisplayHelper(int id, string name, int count, string statType, int statusBouns, string comment, string price, bool equipped, int type, int mode)
        {
            string equippedStatus = equipped ? "[E]" : "   "; //아이템 장착여부 표시

            string statAndBouns = $"{statType} + {statusBouns.ToString()}"; //스텟타입을 string으로 변환 후 합침
            string pad_StatType = PadRightKorean(statAndBouns, _statWidth); // 스텟타입 + 스텟보너스 둘을 합쳐서 변환

            string pad_Comment = PadRightKorean(comment, _commentWidth); //설명 변환

            string pad_Price = PadRightKorean(price, _priceWidth); // 가격 변환

            if (mode == 0) //인벤토리에서 보여줄 창
            {
                if (type >= 6)
                {
                    string idAndName = $"{id.ToString()}. {name} (x{count}"; //번호를 string으로 변환 후 합침
                    string pad_Id = PadRightKorean(idAndName, _width); //번호+이름을 변환

                    Console.WriteLine($"{pad_Id} | {pad_StatType} | {pad_Comment} |"); // 인벤토리에서 물약이 보여줄 창 
                }
                else
                {
                    string idAndName = $"{id.ToString()}. {name}"; //번호를 string으로 변환 후 합침
                    string pad_Id = PadRightKorean(idAndName, _width); //번호+이름을 변환

                    if (equipped)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan; //장착시 초록색으로 표현
                    }

                        Console.WriteLine($"{equippedStatus} {pad_Id} | {pad_StatType} | {pad_Comment} |"); // 인벤토리에서 보여줄 창 

                        Console.ResetColor(); //다음 콘솔에 영향을 주지 않도록 원래색으로 전환합니다.
                }
            }
            else if(mode == 1) //상점에서 보여줄 창
            {
                if (type >= 6)
                {
                    string idAndName = $"{name} (x{count})"; //번호를 string으로 변환 후 합침
                    string pad_Id = PadRightKorean(idAndName, _width); //번호+이름을 변환

                    Console.WriteLine($"- {pad_Id} | {pad_StatType} | {pad_Comment} | {pad_Price}"); // 상점에서 물약이 보여줄 창 
                }
                else
                {
                    string idAndName = $"{name}";
                    string pad_Id = PadRightKorean(idAndName, _width); //번호+이름을 변환

                    Console.WriteLine($"- {pad_Id} | {pad_StatType} | {pad_Comment} | {pad_Price}"); // 상점에서 보여줄 창

                }
            }
            else if(mode == 2) //판매할때 보여줄 창
            {
                if (type >= 6)
                {
                    string idAndName = $"{id.ToString()}. {name} (x{count}"; //번호를 string으로 변환 후 합침
                    string pad_Id = PadRightKorean(idAndName, _width); //번호+이름을 변환

                    Console.WriteLine($" {pad_Id} | {pad_StatType} | {pad_Comment} | {pad_Price}"); // 상점에서 물약을 판매시 보여줄 창 
                }
                else
                {
                    string idAndName = $"{id.ToString()}. {name}"; //번호를 string으로 변환 후 합침
                    string pad_Id = PadRightKorean(idAndName, _width); //번호+이름을 변환

                    if (equipped)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan; //장착시 초록색으로 표현
                    }

                        Console.WriteLine($"{equippedStatus} {pad_Id} | {pad_StatType} | {pad_Comment} | {pad_Price}"); // 상점에서 판매시 보여줄 창

                        Console.ResetColor(); //다음 콘솔에 영향을 주지 않도록 원래색으로 전환합니다.
                }
            }
            else //구매할때 보여줄 창
            {
                if (type >= 6)
                {
                    string idAndName = $"{id.ToString()}. {name} (x{count})"; //번호를 string으로 변환 후 합침
                    string pad_Id = PadRightKorean(idAndName, _width); //번호+이름을 변환

                    Console.WriteLine($"{pad_Id} | {pad_StatType} | {pad_Comment} | {pad_Price} "); // 상점에서 물약을 구매시 보여줄 창 
                }
                else
                {
                    string idAndName = $"{id.ToString()}. {name}"; //번호를 string으로 변환 후 합침
                    string pad_Id = PadRightKorean(idAndName, _width); //번호+이름을 변환

                    Console.WriteLine($"{pad_Id} | {pad_StatType} | {pad_Comment} | {pad_Price} "); // 상점에서 구매시 보여줄 창
                }
            }

        }

    }
}

