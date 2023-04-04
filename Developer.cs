using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
namespace HR
{
    class Developer: Employee
    {
        private static List<string> all_skills = new List<string>();
        private double bonus = 0;
        private List<string> skills = new List<string>();
        public static TextBox textBoxBonus;
        public static CheckedListBox checkedListBox;

        public static void Init()
        {
            string fileData = Employee.DIR_TO_SAVE + "\\skills.txt";
            string[] lines = File.ReadAllLines(fileData);
            foreach (string line in lines)
                all_skills.Add(line.Trim());
        }
        public static string[] GetAllSkills()
        {
            string[] result = new string[all_skills.Count];
            all_skills.CopyTo(result);
            return result;
        }
        public override void displayEmployees()
        {
            base.displayEmployees();
            textBoxBonus.Text = bonus.ToString();
            for (int i = 0; i < skills.Count; i++)
            {
                checkedListBox.SetItemCheckState(checkedListBox.Items.IndexOf(Developer.all_skills[all_skills.IndexOf(skills[i])]), CheckState.Checked);
            }
        }

        public static Developer CreateFromFile(string line)
        {
            string[] data = line.Split(',');
            Developer dev = new Developer(int.Parse(data[0]), data[1].Trim(), data[2].Trim(), int.Parse(data[3]),
                                             double.Parse(data[6]));
            dev.SetSalary(double.Parse(data[5]));
            for (int i = 7; i < data.Length; i++)
            {
                if (all_skills.Contains(data[i].Trim()))
                {
                    dev.AddSkill(data[i].Trim());
                }
                else
                {
                    dev.AddLanguage(data[i].Trim());
                }
            }
            return dev;
        }
        public static void clearSkills()
        {
            for (int i = 0; i < all_skills.Count; i++)
            {
                checkedListBox.SetItemCheckState(checkedListBox.Items.IndexOf(Developer.all_skills[i]), CheckState.Unchecked);
            }
        }
        public Developer(int IdParam, string firstNameParam, string lastNameParam, int yobParam, 
                          double bonusParam):
            base(IdParam, firstNameParam, lastNameParam, yobParam)
        {
            SetTitle("developer");
            bonus = bonusParam;
        }

        public override string GetPersonalStringToFile()
        {
            string skillsToAdd = "";
            foreach (var item in skills)
	        {
                skillsToAdd += item + ", ";
	        }
            return base.GetPersonalStringToFile() + ", " + bonus.ToString() + ", " + skillsToAdd;
        }

        
        public double GetBonus()
        {
            return bonus;
        }

        public void SetBonus(double newBonus)
        {
            bonus = newBonus;
        }

        public string AddSkill(string skill)
        {
            if (!all_skills.Contains(skill))
                return "Unknown skill.";
            else
            {
                skills.Add(skill);
                return "";
            }
        }
        public void setSkills()
        {
            skills.Clear();
             foreach (object skill in checkedListBox.CheckedItems)
             {
                   skills.Add(skill.ToString());
             }
        }
        public override double getYearlySalary()
        {
            return base.getYearlySalary() + bonus;
        }
        public void ClearSkills()
        {
            skills.Clear();
        }

        public List<string> GetSkills()
        {
            return new List<string>(skills);
        }

    }
}
