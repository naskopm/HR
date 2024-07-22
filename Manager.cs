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
using System.Security.Cryptography;
using System.ComponentModel.Design;

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
        public Manager(int idParam, string firstNameParam, string lastNameParam, int yobParam,
            string titleParam) : base(idParam, firstNameParam, lastNameParam, yobParam)
		{ 
            SetTitle(titleParam);
        }
        public static void ReadManagerTitles()
		{
			NpgsqlConnection conn = new NpgsqlConnection(connectionString);
			conn.Open();
			string command = "select ti.title from managertitlles \r\njoin titles ti on managertitlles.title_id = ti.title_id";
            NpgsqlCommand cmd = new NpgsqlCommand(command, conn);
			NpgsqlDataReader reader = cmd.ExecuteReader(0);
			while (reader.Read())
			{
                ManagerTitles.Add(reader.GetString(0).Trim());
            }
			reader.Close();
			conn.Close();
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
			comboManager.Items.Clear();
			Manager.ReadManagerTitles();
			List<Manager> inCombo = new List<Manager>();
            foreach (Employee emp in Employee.GetAllEmployees())
            {
				if(ManagerTitles.IndexOf(emp.GetTitle()) != -1)
                {
					comboManager.Items.Add(emp);
                }
            }
			Manager [] toCopy = new Manager[inCombo.Count];
            for (int i = 0; i < inCombo.Count; i++)
            {
				toCopy[i] = inCombo[i];
            }
			return toCopy;
		}
        public static void loadFromSQL(NpgsqlDataReader reader)
        {
			int id = 0;
			string firstname = "";
			string lastname = "";
            int yob = 0;
            string title = "";
            double bonus = 0;
			double salary = 0;
			string[] subordinants = null;
			try
			{
				id = reader.GetInt32(0);
				firstname = reader.GetString(1);
				lastname = reader.GetString(2);
				yob = reader.GetInt32(3);
				title = reader.GetString(5);
                bonus = reader.IsDBNull(9) ? 0 : reader.GetDouble(9);
                salary = reader.GetDouble(4);
				subordinants = reader.GetString(8).Split(',');
			}
			catch (Exception)
			{
			}
                Manager man = new Manager(id, firstname, lastname, yob, title, bonus);
                man.SetSalary(salary);
				if (subordinants != null)
                foreach (string subordinant in subordinants)
                {
                    man.addToTeam(int.Parse(subordinant));
                }
                try
                {
                    if (reader.GetString(6).Split(',') != null)
                    {
                        string[] languagess = reader.GetString(6).Split(',');
                        foreach (var language in languagess)
                        {
                            man.languages.Add(language.Trim());
                        }
                    }
                }
                catch (Exception)
                {


                }
            
			
      
	


    }
		public static void saveManager(Employee emp)
		{ 
            NpgsqlConnection connection = new NpgsqlConnection(Employee.connectionString);
            connection.Open();
			int manID = findManager(emp.GetID());
			Manager man = FindManager(manID);
			NpgsqlCommand delete = new NpgsqlCommand(@"delete from manager where manager.idmanager = @id",connection);
			delete.Parameters.AddWithValue("@id",man.GetID());
			delete.ExecuteNonQuery();
            foreach (int id in man.team)
            {
                NpgsqlCommand command = new NpgsqlCommand("insert into manager(idmanager,idsubordinate)\r\nvalues(@id,@idsub)", connection);

                command.Parameters.AddWithValue("@id", man.GetID());
                command.Parameters.AddWithValue("@idsub", id);
                command.ExecuteNonQuery();
            }
			connection.Close();
        }
        public override void saveToSQL()
        {
            NpgsqlTransaction transaction = null;
			try { 
			
                NpgsqlConnection connection = new NpgsqlConnection(Employee.connectionString);
                connection.Open();
                transaction = connection.BeginTransaction();
            base.saveToSQL();

                NpgsqlCommand bonus = new NpgsqlCommand("delete from bonuses where bonuses.emp_id = @id; insert into bonuses(emp_id,bonus)\r\nvalues(@id,@bonus)", connection);
                bonus.Parameters.AddWithValue("@id", this.GetID());
                bonus.Parameters.AddWithValue("@bonus", this.GetBonus());
                bonus.ExecuteNonQuery();
				transaction.Commit();
            }
			catch (Exception)
			{
				transaction.Rollback();
			}
			
        }
		public static Manager FindTheManager(int id)
		{
			Manager managerID = null;
            foreach (Employee employee in employees)
			{
				foreach (Manager item in Manager.findAllManagers())
				{
					foreach (int sub_id in item.team)
					{
						if (id == sub_id)
						{
							managerID = item;
						}
					}
				}
			}
			return managerID;
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
        public override void deleteEmployeeSQL()
        {
			Exception exception = new Exception("The manager has subordinates that can't be deleted");
			if (this.team.Count != 0)
			{
				throw exception;
			}
			else
			{
				NpgsqlConnection connection = new NpgsqlConnection(Employee.connectionString);
				connection.Open();
				NpgsqlCommand delete = new NpgsqlCommand("delete from bonuses\r\nwhere bonuses.emp_id = @id;", connection);
				delete.Parameters.AddWithValue("@id", this.GetID());
				delete.ExecuteNonQuery();
				connection.Close();
				base.deleteEmployeeSQL();
			}
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
		public double GetBonus()
		{
			return bonus;
        }

		public override double getYearlySalary()
		{ 
				return base.getYearlySalary() + bonus;
		}

		public double getYearlyBudget()
        {
			double budget = this.getYearlySalary();
            foreach (var item in this.GetTeam())
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
	        TeamBudget.Text = (this.getYearlyBudget()).ToString();
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