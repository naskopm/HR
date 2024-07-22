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
using Npgsql;
using System.Runtime.CompilerServices;
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
          NpgsqlConnection connection = new NpgsqlConnection(Employee.connectionString);
            connection.Open();
            NpgsqlCommand command = new NpgsqlCommand("select skill from developerskills", connection);
            NpgsqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                all_skills.Add(reader.GetString(0));
            }
            connection.Close();
        }
        public override void deleteEmployeeSQL()
        {
            NpgsqlConnection connection = new NpgsqlConnection(Employee.connectionString);
            connection.Open();
            NpgsqlCommand delete = new NpgsqlCommand("delete from developers\r\nwhere developers.dev_id = @id;\r\ndelete from bonuses\r\nwhere bonuses.emp_id = @id;", connection);
            delete.Parameters.AddWithValue("@id", this.GetID());
            delete.ExecuteNonQuery();
            connection.Close();
            base.deleteEmployeeSQL();
        }
        public static void loadFromSQL(NpgsqlDataReader reader, int id, string firstname, string lastname,int yob, double salary)
        { 
            Developer dev = new Developer(id, firstname, lastname, yob, reader.IsDBNull(9) ? 0 : reader.GetDouble(9));
            string title = reader.GetString(5);
            try
            {
                string[] skills = reader.GetString(7).Split(',');
                
                dev.SetSalary(salary);
                foreach (string skill in skills)
                {
                    dev.AddSkill(skill.Trim());
                }

            }
            catch (Exception)
            {

       
            }
            
            try
            {
              
                dev.SetSalary(salary);
                dev.SetTitle(title);
                if (reader.GetString(6).Split(',') != null)
                {
                    string[] languagess = reader.GetString(6).Split(',');
                    foreach (var language in languagess)
                    {
                        dev.languages.Add(language.Trim());
                    }
                }
            }
            catch (Exception)
            {


            }
        }
        public override void saveToSQL()
        {
            base.saveToSQL();
            NpgsqlConnection connection = new NpgsqlConnection(Employee.connectionString);
            connection.Open();
            NpgsqlCommand delete = new NpgsqlCommand($@"delete from developers where developers.dev_id = {this.GetID()};", connection);
            delete.ExecuteNonQuery();
            foreach (string skill in this.GetSkills())
            {
                string langCommand = $@"
do $$
declare 
    skill integer;
begin
    with joined as (
        select ds.id_skill, ds.skill, d.dev_id
        from developerskills ds
        left join developers d on d.dev_skill = ds.id_skill
    ),
    alsoJoined as (
        select j.id_skill
        from joined j
        where j.skill = '{skill.Replace("'", "''").Trim()}'
    )
    select alsoJoined.id_skill
    into skill
    from alsoJoined
    limit 1;
        insert into developers(dev_id, dev_skill)
        values({this.GetID()}, skill);
end $$;;";

                using (var command = new NpgsqlCommand(langCommand, connection))
                {
                    command.ExecuteNonQuery();
                }

            }
            NpgsqlCommand updateBonus = new NpgsqlCommand("delete from bonuses\r\nwhere bonuses.emp_id = @id;\r\ninsert into bonuses(emp_id,bonus)\r\nvalues(@id,@bonus)", connection);
            updateBonus.Parameters.AddWithValue("@id", this.GetID());
            updateBonus.Parameters.AddWithValue("@bonus", this.GetBonus());
            updateBonus.ExecuteNonQuery();
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
        public Developer(int IdParam, string firstNameParam, string lastNameParam, int yobParam) :
            base(IdParam, firstNameParam, lastNameParam, yobParam)
        {
            SetTitle("developer");
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
