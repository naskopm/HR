using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using Npgsql;
using System.Diagnostics.Eventing.Reader;
using System.Diagnostics;

namespace HR
{
    class Employee
    {
        protected static List<Employee> employees = new List<Employee>();
        private static double SalaryIncreaseLimit = 0.5;
        private static double SalaryDecreaseLimit = 0.2;
        private static List<string> titles = new List<string>();
        private static List<string> all_languages = new List<string>();
        public const string DIR_TO_SAVE = "D:\\HR";
        public const string FILE_TO_SAVE = "employees.txt";
        private static int maxID = 0;
        public static TextBox textBoxID;
        public static TextBox textBoxFirstName;
        public static TextBox textBoxLastName;
        public static ComboBox comboTitle;
        public static TextBox textBoxYOB;
        public static TextBox textBoxFilter;
        public static TextBox textBoxSalary;
        public static CheckedListBox checkedListBox;
        public static DataGridView dataGridView;
        public static DataGridView dataGridView1;
        public static string connectionString = "Host=localhost;Username=postgres;Password=nasikrasi;Database=HR";
        public static string bigQuery = "\r\n\tSELECT \r\n    e.emp_id,\r\n    e.FirstName,\r\n    e.LastName,\r\n    e.YearOfBirth,\r\n\te.salary,\r\n\tti.title,\r\n    STRING_AGG(DISTINCT l.language, ', ') AS languages,\r\n    STRING_AGG(DISTINCT devs.skill, ', ') AS skills,\r\n    STRING_AGG(DISTINCT man.idSubordinate::text, ', ') AS subordinates,\r\n    bon.bonus\r\nFROM Employees e\r\nLEFT JOIN languageSpoken ls ON e.emp_id = ls.emp_id\r\nLEFT JOIN languages l ON ls.language_spoken = l.language_id\r\nLEFT JOIN employeeTitles et ON e.emp_id = et.emp_id\r\nLEFT JOIN titles ti ON et.emp_title = ti.title_id\r\nLEFT JOIN developers dev ON e.emp_id = dev.dev_id\r\nLEFT JOIN developerSkills devs ON dev.dev_skill = devs.id_skill\r\nLEFT JOIN manager man ON e.emp_id = man.idManager\r\nLEFT JOIN bonuses bon ON e.emp_id = bon.emp_id\r\nGROUP BY e.emp_id, e.FirstName, e.LastName, e.YearOfBirth, bon.bonus, ti.title;";
        public static void Init()
        {

            NpgsqlConnection titlesCon = new NpgsqlConnection(connectionString);
            titlesCon.Open();
            NpgsqlCommand commandt = new NpgsqlCommand("select ti.title from titles ti", titlesCon);
            NpgsqlDataReader readert = commandt.ExecuteReader();
            while (readert.Read())
            {
                titles.Add(readert.GetString(0).Trim());
            }
            titlesCon.Close();

            NpgsqlConnection languages = new NpgsqlConnection(connectionString);
            languages.Open();
            NpgsqlCommand command = new NpgsqlCommand("select languages.language from languages", languages);
            NpgsqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                all_languages.Add(reader.GetString(0));
            }
            languages.Close();
        }
        public virtual void saveToSQL()
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                bool is_new = true;
                NpgsqlCommand check = new NpgsqlCommand("select e.emp_id from employees e", connection);
                NpgsqlDataReader reader = check.ExecuteReader();
                while (reader.Read())
                {
                    if (this.ID == reader.GetInt32(0))
                    {
                        check.Cancel();
                        reader.Close();
                        NpgsqlCommand command1 = new NpgsqlCommand("update employees set emp_id = @id, FirstName = @firstName, LastName = @lastName, YearOfBirth = @yob, salary = @salary where emp_id = @id", connection);
                        command1.Parameters.AddWithValue("@id", this.GetID());
                        command1.Parameters.AddWithValue("@firstName", this.GetFirstName());
                        command1.Parameters.AddWithValue("@lastName", this.GetLastName());
                        command1.Parameters.AddWithValue("@yob", this.GetYOB());
                        command1.Parameters.AddWithValue("@salary", this.GetSalary());
                        command1.ExecuteNonQuery();
                        Manager.saveManager(this);
                        is_new = false;
                        break;
                    }
                }
                if (is_new)
                {
                    check.Cancel();
                    reader.Close();
                    NpgsqlCommand command = new NpgsqlCommand("insert into employees (emp_id,FirstName, LastName, YearOfBirth, salary) values (@id, @firstName, @lastName, @yob, @salary)", connection);
                    command.Parameters.AddWithValue("@firstName", this.GetFirstName());
                    command.Parameters.AddWithValue("@id", this.GetID());
                    command.Parameters.AddWithValue("@lastName", this.GetLastName());
                    command.Parameters.AddWithValue("@yob", this.GetYOB());
                    command.Parameters.AddWithValue("@salary", this.GetSalary());
                    command.ExecuteNonQuery();
                }
                NpgsqlCommand command2 = new NpgsqlCommand("delete from languageSpoken where emp_id = @id", connection);
                command2.Parameters.AddWithValue("@id", this.GetID());
                command2.ExecuteNonQuery();

                foreach (string lang in this.GetLanguages())
                {
                    string langCommand = $@"
    do $$
    declare 
        lang_id integer;
    begin
    with joined as(
        select l.language_id, l.language, ls.emp_id
        from languages l
        left join languagespoken ls on ls.language_spoken = l.language_id 
    ),
    alsoJoined as(
        select j.language_id
        from joined j
        where j.language = '{lang.Replace("'", "''")}')
    select alsoJoined.language_id from alsoJoined limit 1
    into lang_id;
    insert into languagespoken(emp_id, language_spoken)
    values({this.GetID()}, lang_id);
    end $$;";

                    using (var command = new NpgsqlCommand(langCommand, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
                string queryTitle = $@"delete from employeetitles et
where et.emp_id = {this.GetID()};
do $$
declare 
	title integer;
begin
with joined as(
	select t.title_id,t.title,ts.emp_id
	from titles t
	left join employeetitles ts on ts.emp_title = t.title_id 
),
alsoJoined as(
select j.title_id
from joined j
where j.title = '{this.GetTitle().Replace("'", "''")}')
select alsoJoined.title_id from alsoJoined  limit 1
into title;
insert into employeetitles(emp_id, emp_title)
values({this.GetID()},title);
end $$;";
                NpgsqlCommand chageTitle  = new NpgsqlCommand(queryTitle, connection);
                chageTitle.ExecuteNonQuery();
            }
        }       
        public static void clearLanguages()
        {
            for (int i = 0; i < all_languages.Count; i++)
            {
                checkedListBox.SetItemCheckState(checkedListBox.Items.IndexOf(Employee.all_languages[i]), CheckState.Unchecked);
            }
        }
        public void setLanguages()
        {
            languages.Clear();
            foreach (object lang in checkedListBox.CheckedItems)
            {
                languages.Add(lang.ToString());
            }
        }
        public static void loadFromSQL()
        {
            
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand(bigQuery, connection);
                NpgsqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {

                    int id = reader.GetInt32(0);
                    string firstName = reader.GetString(1);
                    string lastName = reader.GetString(2);
                    int yob = reader.GetInt32(3);
                    string title = reader.IsDBNull(5) ? "Analyst" : reader.GetString(5);

                    double salary = reader.GetDouble(4);
                    if (title.ToLower().IndexOf("developer") != -1)
                    {
                        Developer.loadFromSQL(reader,id,firstName,lastName,yob,salary);
                        continue;
                    }
                    else if (checkWetherManager(title, connection))
                    {
                        Manager.loadFromSQL(reader);
                        continue;
                    }
                    

                    Employee emp = new Employee(id, firstName, lastName, yob);
                    emp.SetSalary(salary);
                    emp.SetTitle(title);
                    try
                    {
                        if (reader.GetString(6).Split(',') != null)
                        {
                            string[] languages = reader.GetString(6).Split(',');
                            foreach (var language in languages)
                            {
                                emp.languages.Add(language.Trim());
                            }
                        }
                    }
                    catch (Exception)
                    {


                    }       
                }
                reader.Close();
            }

        }
        static bool checkWetherManager(string title, NpgsqlConnection con)
        {
            string query = "select\r\nti.title\r\nfrom titles ti\r\njoin managertitlles mant on mant.title_id = ti.title_id";
            NpgsqlConnection managerConnect = new NpgsqlConnection(connectionString);
            managerConnect.Open();
            NpgsqlCommand command = new NpgsqlCommand(query, managerConnect);
            NpgsqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                string manTitle = reader.GetString(0);
                if (manTitle == title)
                {
                    return true;
                }
            }
            return false;
        }
        public static string[] GetTitles()
        {
            string[] result = new string[titles.Count];
            titles.CopyTo(result);
            return result;
        }

        public static string[] GetAllLanguages()
        {
            string[] result = new string[all_languages.Count];
            all_languages.CopyTo(result);
            return result;
        }

        private int ID;
        private string firstName;
        private string lastName;
        private string title;
        private int yob;
        private double salary;
        public List<string> languages = new List<string>();
        public virtual void displayEmployees()
        {
            textBoxID.Text = ID.ToString();
            textBoxFirstName.Text = firstName;
            textBoxLastName.Text = lastName;
            comboTitle.Text = title;
            textBoxYOB.Text = yob.ToString();
            textBoxSalary.Text = salary.ToString();
            for (int i = 0; i < languages.Count; i++)
            {
                checkedListBox.SetItemCheckState(checkedListBox.Items.IndexOf(Employee.all_languages[all_languages.IndexOf(languages[i])]), CheckState.Checked);
            }
        }
        public static Employee CreateNewEmployee(string firstNameParam, string lastNameParam, int yobParam)
        {
            if (yobParam < 1955)
            {
                MessageBox.Show("The person is too old.");
                return null;
            }
            else if (yobParam > 2007)
            {
                MessageBox.Show("The person is too young.");
                return null;
            }

            for (int i = 0; i < firstNameParam.Length; i++)
            {
                if (!Char.IsLetter(firstNameParam[i]))
                {
                    MessageBox.Show("The first name is not a valid name.");
                    return null;
                }
            }

            for (int i = 0; i < lastNameParam.Length; i++)
            {
                if (!Char.IsLetter(lastNameParam[i]))
                {
                    MessageBox.Show("The last name is not a valid name.");
                    return null;
                }
            }
            Employee toAdd = new Employee(-1, firstNameParam, lastNameParam, yobParam);

            return toAdd;
        }

        public static Employee CreateFromFile(string line)
        {
           
                string[] data = line.Split(',');
            if (data[4].Trim() == "developer")
                return Developer.CreateFromFile(line);
            if (Manager.ManagerTitles.IndexOf(data[4].Trim()) != -1)
                return Manager.CreateFromFile(line);
            Employee emp = new Employee(int.Parse(data[0]), data[1].Trim(), data[2].Trim(), int.Parse(data[3]),
                data[4].Trim(), double.Parse(data[5]));
            for (int i = 6; i < data.Length; i++)
            {
                emp.AddLanguage(data[i].Trim());
            }

            return emp;
        }
        public static void SaveToFile()
        {
            string fileToSave = Employee.DIR_TO_SAVE + "\\" + FILE_TO_SAVE;
            StreamWriter sw = File.CreateText(fileToSave);
            foreach (Employee em in employees)
            {
                sw.WriteLine(em.GetStringToFile());
            }
            sw.Close();
        }
        public static List<Employee> GetAll()
        {
            return new List<Employee>(employees);
        }
        public static void Load(string line)
        {
            Employee emp = Employee.CreateFromFile(line);
        }
        public static void Remove(Employee toDelete)
        {
            employees.RemoveAt(employees.IndexOf(toDelete));
        }

        public Employee(int IdParam, string firstNameParam, string lastNameParam, int yobParam)
        {
            if (IdParam <= 0)
                ID = ++maxID;
            else
                ID = IdParam;
            if (ID > maxID)
                maxID = ID;
            firstName = firstNameParam.Trim();
            lastName = lastNameParam.Trim();
            yob = yobParam;
            employees.Add(this);
        }
        public Employee FindEmployee(int id)
        {
            foreach (Employee item in employees)
            {
                if (item.GetID() == id)
                    return item;
            }
            return null;
        }
        public Employee(int IdParam, string firstNameParam, string lastNameParam, int yobParam,
                        string titleParam, double salaryParam) :
               this(IdParam, firstNameParam, lastNameParam, yobParam)
        {
            title = titleParam.Trim();
            salary = salaryParam;
        }

        public int GetID()
        {
            return ID;
        }

        public string GetFirstName()
        {
            return firstName;
        }

        public string GetLastName()
        {
            return lastName;
        }
        public void fillSecondTable()
        {
            Employee.clearLanguages();
            Developer.clearSkills();
            int dataGridID = 0;
            bool isNotInTable = true;
    

            this.displayEmployees();
            for (int i = 0; i < dataGridView.Rows.Count - 1; i++)
            {
                dataGridID = int.Parse(dataGridView.Rows[i].Cells[0].Value.ToString());
                if (dataGridID == this.GetID())
                {
                    dataGridView.Rows[i].Selected = true;
                    Employee toDisplay = FindEmployee(dataGridID);
                    toDisplay.displayEmployees();
                    isNotInTable = false;
                    break;
                }
            }
            if (isNotInTable)
            {
                dataGridView.Rows.Clear();
                foreach (Employee employee in Employee.GetAllEmployees())
                {
                    dataGridView.Rows.Add(employee.GetID(), employee.GetFirstName(), employee.GetLastName(), employee.GetTitle());
                }
                for (int i = 0; i < dataGridView.Rows.Count - 1; i++)
                {
                    dataGridID = int.Parse(dataGridView.Rows[i].Cells[0].Value.ToString());
                    if (dataGridID == this.GetID())
                    {
                        textBoxFilter.Text = "";

                        dataGridView.Rows[i].Selected = true;
                        Employee toDisplay = FindEmployee(dataGridID);
                        toDisplay.displayEmployees();
                        isNotInTable = false;
                        break;
                    }
                }
            }
            foreach (Employee employee in Employee.GetAllEmployees())
            {
                if (employee.GetID() == this.GetID())
                {
                    try
                    {
                        Manager man = (Manager)employee;
                        if (employee != null)
                        {
                            employee.displayEmployees();
                            if (Manager.ManagerTitles.IndexOf(this.GetTitle()) != -1)
                            {
                                dataGridView1.Enabled = true;
                                dataGridView1.Rows.Clear();
                                foreach (var item in man.GetTeam())
                                {
                                    Employee emp1 = FindEmployee(item);
                                    dataGridView1.Rows.Add(emp1.GetID(), emp1.GetFirstName(), emp1.GetLastName(), emp1.GetTitle());
                                }
                            }
                            else
                            {
                                dataGridView1.Enabled = false;
                            }
                        }

                    }
                    catch (Exception)
                    {

                    }
                }
            }
        }

        public string GetFullName()
        {
            return firstName + " " + lastName;
        }

        public virtual string GetPersonalStringToFile()
        {
            return ID.ToString() + ", " + firstName + ", " + lastName + ", " + yob.ToString() + ", "
                            + title + ", " + salary.ToString();
        }
        public virtual string GetStringToFile()
        {
            string result = GetPersonalStringToFile();
            foreach (string lang in languages)
                result += ", " + lang;
            return result;
        }

        public string GetTitle()
        {
            return title;
        }
        public static List<Employee> GetAllEmployees()
        {
            return employees;
        }
        public string SetTitle(string newTitle)
        {
            if (!titles.Contains(newTitle))
                return "Invalid title.";
            title = newTitle;
            return null;
        }

        public double GetSalary()
        {
            return salary;
        }

        public string SetSalary(double newSalary)
        {
            if (salary != 0 && newSalary / salary > 1.0 + Employee.SalaryIncreaseLimit)
                throw new Exception("The salary increase is too big.");
            if (salary != 0 && (salary - newSalary) / salary > Employee.SalaryDecreaseLimit)
                throw new Exception("The salary decrease is too big.");
            salary = newSalary;
            return null;
        }
        public virtual double getYearlySalary()
        {
            return 12 * this.GetSalary();
        }
        public override string ToString()
        {
            return GetFullName() + ", " + title;
        }
        public virtual void deleteEmployeeSQL()
        {
            NpgsqlConnection connection = new NpgsqlConnection(connectionString);
            connection.Open();
            NpgsqlCommand command = new NpgsqlCommand("delete from languagespoken\r\nwhere languagespoken.emp_id = @id;\r\ndelete from employeetitles\r\nwhere employeetitles.emp_id = @id; delete from manager\r\nwhere idsubordinate = @id;", connection);
            command.Parameters.AddWithValue("@id", this.GetID());
            command.ExecuteNonQuery();
                NpgsqlCommand command1 = new NpgsqlCommand("delete from employees\r\nwhere employees.emp_id = @id", connection);
                command1.Parameters.AddWithValue("@id", this.GetID());
                command1.ExecuteNonQuery();

        }

        public int GetAge()
        {
            return DateTime.Now.Year - yob;
        }

        public int GetYOB()
        {
            return yob;
        }

        public string AddLanguage(string lang)
        {
            if (!all_languages.Contains(lang))
                return "Unknown language";
            languages.Add(lang);
            return null;
        }

        public void ClearLanguages()
        {
            languages.Clear();
        }

        public List<string> GetLanguages()
        {
            return new List<string>(languages);
        }

        public Boolean IsMatch(string filter)
        {
            filter = filter.ToLower();
            if (filter.Trim() == "")
                return true;
            return (firstName.ToLower() == filter) || (lastName.ToLower() == filter)
                || (title.ToLower() == filter);
        }


    }
}
