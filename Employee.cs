using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HR
{
    class Employee
    {
        protected static List<Employee> employees = new List<Employee>();
        private static double SalaryIncreaseLimit = 0.5;
        private static double SalaryDecreaseLimit = 0.2;
        private static List<string> titles = new List<string>();
        private static List<string> all_languages = new List<string>();
        public const string DIR_TO_SAVE = "D:\\Наско\\HR";
        public const string FILE_TO_SAVE = "employees.txt";
        private static int maxID = 1;
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
        public static void Init()
        {
            string fileData = Employee.DIR_TO_SAVE + "\\titles.txt";
            string[] lines = File.ReadAllLines(fileData);
            foreach (string line in lines)
                titles.Add(line.Trim());

            fileData = Employee.DIR_TO_SAVE + "\\languages.txt";
            lines = File.ReadAllLines(fileData);
            foreach (string line in lines)
                all_languages.Add(line.Trim());
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
        private List<string> languages = new List<string>();
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
