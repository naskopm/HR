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
	class Manager : Employee
	{
		public static ListBox listBox;
		public static TextBox Bonus;
		public static ComboBox comboManager;
		public static ComboBox comboTitle;
		public static List<string> ManagerTitles = new List<string>();
		public static List<Manager> managers = new List<Manager>();
		public static DataGridView ShowTeam;
		public static Label TeamBudget;

		double bonus;
		List<int> team = new List<int>();
		public Manager(int idParam, string firstNameParam, string lastNameParam, int yobParam,
			string titleParam, double bonusParam) : base(idParam, firstNameParam, lastNameParam, yobParam)
		{
			bonus = bonusParam;
			SetTitle(titleParam);
		}
		public static void ReadManagerTitles()
		{
			string fileData = Employee.DIR_TO_SAVE + "\\managerTitles.txt";
			string[] lines = File.ReadAllLines(fileData);
			foreach (string line in lines)
			{
				ManagerTitles.Add(line.Trim());
			}
		}

		public void AddToTeam(int id)
		{
			foreach (Employee item in employees)
			{
				try
				{
					Manager man = (Manager)item;
					while (man.GetTeam().IndexOf(id) != -1)
					{
						man.GetTeam().RemoveAt(man.GetTeam().IndexOf(id));

					}
				}
				catch (Exception)
				{


				}
			}
			team.Add(id);
		}
		public static void TakeEmpToTeam(Employee emp)
        {
			foreach (Employee item in Employee.GetAll())
			{
				if (comboManager.SelectedItem != null)
				{
					if (item == comboManager.SelectedItem)
					{
						Manager man = (Manager)item;
						man.AddToTeam(emp.GetID());
					}
				}
			}
		}
		public static Manager[] takeManagersToCombo()
        {
			Manager.ReadManagerTitles();
			List<Manager> inCombo = new List<Manager>();
            foreach (Employee emp in Employee.GetAllEmployees())
            {
				if(ManagerTitles.IndexOf(emp.GetTitle()) != -1)
                {
					inCombo.Add((Manager)emp);
                }
            }
			Manager [] toCopy = new Manager[inCombo.Count];
            for (int i = 0; i < inCombo.Count; i++)
            {
				toCopy[i] = inCombo[i];
            }
			return toCopy;
		}

		public static Manager CreateFromFile(string line)
		{
			string[] data = line.Split(',');
			for (int i = 0; i < data.Length; i++)
			{
				data[i] = data[i].Trim();
			}
			Manager man = new Manager(int.Parse(data[0]), data[1].Trim(), data[2].Trim(), int.Parse(data[3]), data[4].Trim(), double.Parse(data[6]));
			man.SetSalary(int.Parse(data[5]));
			for (int i = 7; i < data.Length; i++)
			{
				int outter;
				if (int.TryParse(data[i], out outter))
				{
					man.addToTeam(outter);
				}
				else
				{
					if (GetAllLanguages().Contains(data[i].Trim()))
					{
						man.AddLanguage(data[i].Trim());
					}
				}
			}
			return man;
		}
		private static Manager FindManager(int id)
		{
			foreach (Manager emp in Manager.findAllManagers())
			{
				if (emp.GetID() == id)
					return emp;
			}
			return null;
		}
		public static void delteSubordinant(int id)
        {
			int managerID = Manager.findManager(id);
			if(managerID != -1)
            {
				Manager man = Manager.FindManager(managerID);
				man.GetTeam().RemoveAt(man.GetTeam().IndexOf(id));
			}
			
        }

		public override string GetPersonalStringToFile()
		{
			string crewMembers = "";
			foreach (var item in team)
			{
				crewMembers += item + ", ";
			}
			return base.GetPersonalStringToFile() + ", " + bonus.ToString() + ", " + crewMembers;
		}
		public double GetBonus()
		{
			return bonus;
		}

		public override double getYearlySalary()
		{
			return base.getYearlySalary() + bonus + this.getYearlyBudget();
		}
		public double getYearlyBudget()
        {
			double budget = 0;
            foreach (var item in team)
            {
				budget += FindEmployee(item).getYearlySalary();
            }
			return budget;
        }
		public void SetBonus(double newBonus)
		{
			bonus = newBonus;
		}
		public List<int> GetTeam()
		{
			return team;
		}
		public override void displayEmployees()
		{
			base.displayEmployees();
			string subordinants = "";
			foreach (var item in team)
			{
				subordinants += item + ",";
			}
			Bonus.Text = bonus.ToString();
			
			Employee selectedEmplyoee = FindEmployee(Manager.findManager(this.GetID()));
			if (selectedEmplyoee != null)
			{
				if (Manager.ManagerTitles.IndexOf(selectedEmplyoee.GetTitle()) != -1)
				{
					Manager selectedManager = (Manager)selectedEmplyoee;
					comboManager.Text = selectedManager.ToString();
				}
			}
	        TeamBudget.Text = this.getYearlyBudget().ToString();
        }
		public static int findManager(int id)
        {
			foreach (Manager man in Manager.findAllManagers())
            {
                for (int i = 0; i < man.GetTeam().Count; i++)
                {
					if(id == man.GetTeam()[i])
                    {
						return man.GetID();
                    }
                }
            }
			return -1;
        }
		public static List<Manager> findAllManagers()
        {
			List<Manager> managers = new List<Manager>();
            foreach (Employee item in employees)
            {
                try
                {
					Manager man = (Manager)item;
					managers.Add(man);
                }
                catch (Exception)
                {

                }
            }
			return managers;
        }
		public void SetTeam(string teamParam)
		{
			team.Clear();
			string[] data = teamParam.Split(',');
			foreach (var item in data)
			{
				team.Add(int.Parse(item));
			}
		}
		public void addToTeam(int ID)
		{
			team.Add(ID);
		}
	}
}


//da go napravvq po analogiq s Developer