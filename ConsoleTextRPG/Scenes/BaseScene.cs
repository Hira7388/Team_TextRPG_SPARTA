using ConsoleTextRPG.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTextRPG.Scenes
{
    internal abstract class BaseScene
    {
        public abstract void Render();
        public abstract void Update();

        // 텍스트 출력 함수 오버로딩
        public void Print<T>(string text1, T no, string text2, ConsoleColor c)
        {
            Console.ResetColor();// 기본 색 복원
            Console.Write(text1);
            Console.ForegroundColor = c;   // 번호 색
            Console.Write($"{no}. ");
            Console.ResetColor();// 기본 색 복원
            Console.WriteLine(text2);
        }
        public void Print<T>(T no, string text, ConsoleColor c)
        {
            Console.ForegroundColor = c;   // 번호 색
            Console.Write($"{no}. ");
            Console.ResetColor();// 기본 색 복원
            Console.WriteLine(text);
        }
        public void Print<T>(T no, string text)
        {
            Console.Write($"{no}. ");
            Console.ResetColor();// 기본 색 복원
            Console.WriteLine(text);
        }
        public void Print<T>(T text, ConsoleColor c, T no)
        {
            Console.ForegroundColor = c;   // 번호 색
            Console.WriteLine(text);
            Console.ResetColor();// 기본 색 복원 
            Console.Write($"{no}. ");
        }
        public void Print<T>(string text, T no, ConsoleColor c)
        {
            Console.ResetColor();// 기본 색 복원 
            Console.Write(text);
            Console.ForegroundColor = c;   // 번호 색
            Console.Write($"{no}");
        }
        public void Print<T>(ConsoleColor c, T no)
        {
            Console.ForegroundColor = c;   // 번호 색
            Console.Write($"{no}. ");
            Console.ResetColor();// 기본 색 복원 
        }
        public void Print<T>(T text)
        {
            Console.WriteLine(text);
        }
        public void Print<T>(T text, ConsoleColor c)
        {
            Console.ForegroundColor = c;   // 번호 색
            Console.WriteLine(text);
            Console.ResetColor();// 기본 색 복원 
        }

        // 화면내 행동 정보 출력
        public void Info(string msg)
        {
            Console.WriteLine($"\n{msg}");
            Thread.Sleep(800);// 0.8초 대기
        }
    }
}
