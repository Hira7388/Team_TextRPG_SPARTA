using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace ConsoleTextRPG.Managers
{
    public class SkillManager
    {
        public List<Skill> AllSkills { get; private set; }

        private static SkillManager _instance;
        public static SkillManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new SkillManager();
                return _instance;
            }
        }

        private SkillManager()
        {
            AllSkills = new List<Skill>();
            LoadSkillDatabase();
        }

        private void LoadSkillDatabase()
        {
            try
            {
                string projectRootPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "..", ".."));
                string skillDbPath = Path.Combine(projectRootPath, "Json", "skills.json");

                if (File.Exists(skillDbPath))
                {
                    string json = File.ReadAllText(skillDbPath, Encoding.UTF8);
                    AllSkills = JsonConvert.DeserializeObject<List<Skill>>(json);
                }
                else
                {
                    Console.WriteLine("스킬 데이터베이스 파일을 찾을 수 없습니다!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"스킬 데이터베이스 로딩 중 오류 발생: {ex.Message}");
                AllSkills = new List<Skill>();
            }
        }

        public Skill GetSkillById(int id)
        {
            return AllSkills.FirstOrDefault(s => s.Id == id);
        }
    }
}