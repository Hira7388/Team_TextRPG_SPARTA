using ConsoleTextRPG.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTextRPG.Scenes
{
    internal class ConsoleHelper
    {

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

        // 번호, 상품 이름, 스텟타입, 스텟보너스, 설명, 장착여부, 이름너비, 스텟너비, 설명너비
        public static void DisplayInventory(int id, string name, string statType, int statusBouns, string comment, bool equipped, int Width, int statWidth, int commentWidth)
        {
            string equippedStatus = equipped ? "[E]" : "   ";

            string idAndName = $"{id.ToString()}. {name}"; //번호를 string으로 변환 후 합침
            string pad_Id = PadRightKorean(idAndName, Width); //번호+이름을 변환

            string statAndBouns = $"{statType} + {statusBouns.ToString()}"; //스텟타입을 string으로 변환 후 합침
            string pad_StatType = PadRightKorean(statAndBouns, statWidth); // 스텟타입 + 스텟보너스 둘을 합쳐서 변환

            string pad_Comment = PadRightKorean(comment, commentWidth); //설명 변환

            if (equipped)
            {
                Console.ForegroundColor = ConsoleColor.Cyan; //장착시 초록색으로 표현
            }

            Console.WriteLine($" {equippedStatus} {pad_Id} | {pad_StatType} | {pad_Comment} |"); // 인벤토리에서 보여줄 창 

            Console.ResetColor(); //다음 콘솔에 영향을 주지 않도록 원래색으로 전환합니다.
        }


        // 상품 이름, 스텟타입, 스텟보너스, 설명, 가격, 이름너비, 스텟너비, 설명너비, 가격너비
        public static void DisplayShopItem( string name, string statType, int statusBouns, string comment, string price, int Width, int statWidth, int commentWidth, int priceWidth)
        {
            string pad_Id = PadRightKorean(name, Width); //이름을 변환

            string statAndBouns = $"{statType} + {statusBouns.ToString()}"; //스텟타입을 string으로 변환 후 합침
            string pad_StatType = PadRightKorean(statAndBouns, statWidth); // 둘을 합쳐서 변환

            string pad_Comment = PadRightKorean(comment, commentWidth); //설명 변환

            string pad_Price = PadRightKorean(price, priceWidth); // 가격 변환

            Console.WriteLine($"-   {pad_Id} | {pad_StatType} | {pad_Comment} | {pad_Price}"); // 상점에서 보여줄 창
        }

        // 번호, 상품 이름, 스텟타입, 스텟보너스, 설명, 가격, 이름너비, 스텟너비, 설명너비, 가격너비
        public static void DisplayShopItemBuy(int id, string name, string statType, int statusBouns, string comment, string price, int Width, int statWidth, int commentWidth, int priceWidth)
        {
            string idAndName = $"{id.ToString()}. {name}"; //번호를 string으로 변환 후 합침
            string pad_Id = PadRightKorean(idAndName, Width); //번호+이름을 변환

            string statAndBouns = $"{statType} + {statusBouns.ToString()}"; //스텟타입을 string으로 변환 후 합침
            string pad_StatType = PadRightKorean(statAndBouns, statWidth); // 스텟타입 + 스텟보너스 둘을 합쳐서 변환

            string pad_Comment = PadRightKorean(comment, commentWidth); //설명 변환

            string pad_Price = PadRightKorean(price, priceWidth); // 가격 변환

            Console.WriteLine($" {pad_Id} | {pad_StatType} | {pad_Comment} | {pad_Price}"); // 상점에서 구매시 보여줄 창 
        }

        // 번호, 상품 이름, 스텟타입, 스텟보너스, 설명, 판매가격, 이름너비, 스텟너비, 설명너비, 판매가격너비
        public static void DisplayShopItemSell(int id, string name, string statType, int statusBouns, string comment, int price, bool equipped, int Width, int statWidth, int commentWidth, int priceWidth)
        {

            string equippedStatus = equipped ? "[E]" : "   "; 

            string idAndName = $"{id.ToString()}. {name}"; //번호를 string으로 변환 후 합침
            string pad_Id = PadRightKorean(idAndName, Width); //번호+이름을 변환

            string statAndBouns = $"{statType} + {statusBouns.ToString()}"; //스텟타입을 string으로 변환 후 합침
            string pad_StatType = PadRightKorean(statAndBouns, statWidth); // 스텟타입 + 스텟보너스 둘을 합쳐서 변환

            string pad_Comment = PadRightKorean(comment, commentWidth); //설명 변환

            string pad_Price = price.ToString().PadLeft(priceWidth); // 판매가격 변환

            if (equipped)
            {
                Console.ForegroundColor = ConsoleColor.Cyan; //장착시 초록색으로 표현
            }

            Console.WriteLine($" {equippedStatus} {pad_Id} | {pad_StatType} | {pad_Comment} | {pad_Price} G"); // 상점에서 판매시 보여줄 창

            Console.ResetColor(); //다음 콘솔에 영향을 주지 않도록 원래색으로 전환합니다.
        }

    }
}

