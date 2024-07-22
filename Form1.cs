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
using System.Security.Cryptography;

namespace HR
{
    public partial class Form1 : Form
    {
        List<Employee> filtered = new List<Employee>();
        Employee emp = null;

        public Form1()
        {
            InitializeComponent();
            Employee.Init();
            Developer.Init();
            comboBoxTitle.Items.AddRange(Employee.GetTitles());
            checkedListBoxLanguages.Items.AddRange(Employee.GetAllLanguages());
            checkedListBoxSkills.Items.AddRange(Developer.GetAllSkills());
            Manager.ReadManagerTitles();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            teamBudget.Text = "";
            textBoxID.Text = "";
            textBoxFirstName.Text = "";
            textBoxLastName.Text = "";
            textBoxYOB.Text = "";
            textBoxSalary.Text = "";
            comboBoxTitle.SelectedIndex = -1;
            Employee.clearLanguages();
            Developer.clearSkills();
            emp = null;
            textBoxFirstName.Enabled = true;
            textBoxLastName.Enabled = true;
            textBoxYOB.Enabled = true;
            dataGridView1.Enabled = false;
            textBoxBonus.Text = "";
            comboManagers.Text = "";
            comboManagers.Items.Clear();
            comboManagers.Items.AddRange(Manager.takeManagersToCombo());
            textBoxFirstName.Focus();
            dataGridView1.Rows.Clear();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Employee.textBoxID = textBoxID;
            Employee.textBoxFirstName = textBoxFirstName;
            Employee.textBoxLastName = textBoxLastName;
            Employee.comboTitle = comboBoxTitle;
            Manager.comboTitle = comboBoxTitle;
            Employee.textBoxYOB = textBoxYOB;
            Employee.textBoxSalary = textBoxSalary;
            Employee.checkedListBox = checkedListBoxLanguages;
            Developer.checkedListBox = checkedListBoxSkills;
            Developer.textBoxBonus = textBoxBonus;
            Manager.comboManager = comboManagers;
            Manager.Bonus = textBoxBonus;
            Manager.ShowTeam = dataGridView1;
            Employee.dataGridView1 = dataGridView1;
            Employee.dataGridView = dataGridView;
            Employee.textBoxFilter = textBoxFilter;
            Manager.TeamBudget = teamBudget;
            string fileData = Employee.DIR_TO_SAVE + "\\employees.txt";
            Employee.loadFromSQL();
            foreach (Employee emp in Employee.GetAllEmployees())
            {
                filtered.Add(emp);
            }
            foreach (Employee emp in Employee.GetAllEmployees())
            {
                dataGridView.Rows.Add(emp.GetID().ToString(), emp.GetFirstName(), emp.GetLastName(), emp.GetTitle());

            }
            comboManagers.Items.Clear();
            comboManagers.Items.AddRange(Manager.takeManagersToCombo());
        }
    
        private void dataGridView_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            comboManagers.Items.Clear();
            dataGridView1.Rows.Clear();
            teamBudget.Text = null;
           
                if (dataGridView.Rows[e.RowIndex].Cells[0].Value == null)
            {
                return;
            }
            int empId = int.Parse(dataGridView.Rows[e.RowIndex].Cells[0].Value.ToString());

            if (empId > 0 &&  empId != null && Employee.GetAllEmployees().Count != 0) 
                emp = FindEmployee(empId);
            Employee.clearLanguages();
            Developer.clearSkills();
            comboManagers.SelectedIndex = -1;
            dataGridView1.Rows.Clear();
          
            if (emp != null)
            {
                emp.displayEmployees();
                int managerID = Manager.findManager(emp.GetID());
                Manager manager = Manager.FindTheManager(emp.GetID());


                if (Manager.ManagerTitles.IndexOf(emp.GetTitle()) != -1)
                {
                    foreach (Manager managerCheck in Manager.takeManagersToCombo())
                    {
                        if (emp.GetID() != managerCheck.GetID())
                        {
                            comboManagers.Items.Add(managerCheck);
                        }
                    }

                    dataGridView1.Enabled = true;
                    dataGridView1.Rows.Clear();
                    Manager man = (Manager)emp;
                    foreach (var item in man.GetTeam())
                    {
                        Employee emp1 = FindEmployee(item);
                        //moje bi e null treysvay go
                        dataGridView1.Rows.Add(emp1.GetID(), emp1.GetFirstName(), emp1.GetLastName(), emp1.GetTitle());
                    }
                }
                else
                {
                    comboManagers.Items.AddRange(Manager.takeManagersToCombo());
                    dataGridView1.Enabled = false;
                   
                }

                if (manager != null)
                {
                    comboManagers.Text = manager.ToString();
                }
                else
                    comboManagers.SelectedIndex = -1;

            }
            textBoxFirstName.Enabled = false;
            textBoxLastName.Enabled = false;
            textBoxYOB.Enabled = false;
        }



        private Employee FindEmployee(int id)
        {
            if (filtered.Count == 0)
            {
                return null;
            }

            foreach (Employee emp in Employee.GetAllEmployees())
            {
                if (emp.GetID() == id)
                    return emp;
            }
            return null;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            textBoxFirstName.Enabled = false;
            textBoxLastName.Enabled = false;
            textBoxYOB.Enabled = false;
            Boolean bIsNew = false;
            if (emp == null)
            {
                if (comboBoxTitle.SelectedItem.ToString().ToLower().IndexOf("developer") != -1)
                {
                    emp = new Developer(-1, textBoxFirstName.Text, textBoxLastName.Text, int.Parse(textBoxYOB.Text));
                    Developer dev = (Developer)emp;
                    dev.setSkills();
                    if (textBoxBonus.Text != "")
                        dev.SetBonus(double.Parse(textBoxBonus.Text));
                }
                if (Manager.ManagerTitles.IndexOf(comboBoxTitle.SelectedItem.ToString().Trim()) != -1)
                {
                    emp = new Manager(-1, textBoxFirstName.Text, textBoxLastName.Text, int.Parse(textBoxYOB.Text), comboBoxTitle.Text);
                    Manager man = (Manager)emp;
                    if(textBoxBonus.Text != "")
                    man.SetBonus(double.Parse(textBoxBonus.Text));
                }
                if (Manager.ManagerTitles.IndexOf(comboBoxTitle.SelectedItem.ToString().Trim()) == -1 && comboBoxTitle.SelectedItem.ToString().ToLower().IndexOf("developer") == -1)
                    emp = new Employee(-1, textBoxFirstName.Text, textBoxLastName.Text, int.Parse(textBoxYOB.Text));
                bIsNew = true;
            }
            if (bIsNew)
            {

                textBoxID.Text = emp.GetID().ToString();
                if (emp.IsMatch(textBoxFilter.Text))
                    filtered.Add(emp);
                else
                    MessageBox.Show("The new employee " + emp.GetFullName() + " is not displayed in the grid because of the filer.");

                emp.SetSalary(double.Parse(textBoxSalary.Text));
                emp.SetTitle(comboBoxTitle.Text);
                emp.ClearLanguages();
                emp.setLanguages();
                Manager.TakeEmpToTeam(emp);
                dataGridView.Rows.Add(emp.GetID(), emp.GetFirstName(), emp.GetLastName(), emp.GetTitle());
                dataGridView1.Rows.Clear();
                dataGridView.Rows[dataGridView.Rows.Count - 2].Selected = true;
            }

            else
            {


                for (int i = 0; i < dataGridView.Rows.Count - 1; i++)
                {
                    if (dataGridView.Rows[i].Selected == true)
                    {
                        int id = int.Parse(dataGridView.Rows[i].Cells[0].Value.ToString());
                        emp = FindEmployee(id);
                    }
                }
                
                if (Manager.ManagerTitles.IndexOf(comboBoxTitle.SelectedItem.ToString().Trim()) != -1)
                {
                    try
                    {
                        emp.deleteEmployeeSQL();
                        Manager prevent = (Manager)emp;
                        if (prevent.GetTeam().Count == 0)
                        {

                            Employee.GetAllEmployees().Remove(emp);
                            emp = new Manager(emp.GetID(), textBoxFirstName.Text, textBoxLastName.Text, int.Parse(textBoxYOB.Text), comboBoxTitle.Text);
                            Manager man = (Manager)emp;
                            if (textBoxBonus.Text != "")
                                man.SetBonus(double.Parse(textBoxBonus.Text));
                        }
                    }
                    catch (Exception)
                    {
                        emp.deleteEmployeeSQL();
                        Employee.GetAllEmployees().Remove(emp);
                        emp = new Manager(emp.GetID(), textBoxFirstName.Text, textBoxLastName.Text, int.Parse(textBoxYOB.Text), comboBoxTitle.Text);
                        Manager man = (Manager)emp;
                            if (textBoxBonus.Text != "")
                                man.SetBonus(double.Parse(textBoxBonus.Text));
                    }
                    if (emp.GetTitle().ToLower().IndexOf("developer") != -1)
                    {
                        Developer dev = (Developer)emp;
                        dev.SetBonus(int.Parse(textBoxBonus.Text));
                        dev.setSkills();
                        if (textBoxBonus.Text != "")
                            dev.SetBonus(double.Parse(textBoxBonus.Text));
                    }
                    else if (Manager.ManagerTitles.IndexOf(emp.GetTitle()) != -1)
                    {
                        Manager man = (Manager)emp;
                        if (textBoxBonus.Text != "")
                            man.SetBonus(double.Parse(textBoxBonus.Text));
                    }

                }

                if (comboBoxTitle.SelectedItem.ToString().ToLower().IndexOf("developer") != -1)
                {
                    emp.deleteEmployeeSQL();
                    Employee.GetAllEmployees().Remove(emp);
                    emp = new Developer(emp.GetID(), textBoxFirstName.Text, textBoxLastName.Text, int.Parse(textBoxYOB.Text));
                    Developer dev = (Developer)emp;
                    if (textBoxBonus.Text != "")
                        dev.SetBonus(double.Parse(textBoxBonus.Text));
                    dev.setSkills();
                }


            }
            emp.SetSalary(double.Parse(textBoxSalary.Text));
            emp.SetTitle(comboBoxTitle.Text);
            emp.ClearLanguages();
            emp.setLanguages();
            Manager.TakeEmpToTeam(emp);

           
            string saveSelectedManager = comboManagers.Text;
            comboManagers.Items.Clear();
            comboManagers.Items.AddRange(Manager.takeManagersToCombo());
            emp.saveToSQL();
            for (int i = 0; i < checkedListBoxSkills.Items.Count; i++)
            {
                checkedListBoxSkills.SetItemChecked(i, false);
            }
            double bonusText = 0;
            bool didExceot = false;
            try
            {
                didExceot = true;
                bonusText = double.Parse(textBoxBonus.Text);
            }
            catch (Exception)
            {
                
            }
            textBoxBonus.Text = "";
            if (didExceot)
                textBoxBonus.Text = bonusText.ToString();
            if (bonusText == 0)
            {
                textBoxBonus.Text = "";
            }
            comboManagers.Items.Clear();
            comboManagers.Items.AddRange(Manager.takeManagersToCombo());
            comboManagers.Text = saveSelectedManager;
            comboManagers.Items.Clear();
            foreach (Manager managerCheck in Manager.takeManagersToCombo())
            {
                if (emp.GetID() != managerCheck.GetID())
                {
                    comboManagers.Items.Add(managerCheck);
                }
            }
            comboManagers.Text = saveSelectedManager;
            //filtered[filtered.IndexOf(emp)] = emp;
            emp.saveToSQL();
            emp.displayEmployees();
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            dataGridView.Rows.Clear();
            filtered.Clear();
            foreach (Employee emp in Employee.GetAllEmployees())
            {
                if (emp.IsMatch(textBoxFilter.Text))
                    filtered.Add(emp);
            }

            foreach (Employee emp in filtered)
            {
                dataGridView.Rows.Add(emp.GetID(),emp.GetFirstName(), emp.GetLastName(), emp.GetTitle());
            }
            dataGridView1.Rows.Clear();
            teamBudget.Text = "";
        }

        private void btnClearFilter_Click(object sender, EventArgs e)
        {
            textBoxFilter.Text = "";
            btnFilter_Click(sender, null);
        }

        private void textBoxFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (ch == 13)
                btnFilter_Click(sender, null);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int empId = int.Parse(dataGridView.SelectedRows[0].Cells[0].Value.ToString());
            emp = FindEmployee(empId);
            emp.deleteEmployeeSQL();
            comboManagers.Items.Clear();
            comboManagers.Items.AddRange(Manager.takeManagersToCombo());
            Manager.delteSubordinant(empId);
            Employee.GetAllEmployees().Remove(emp);
            filtered.Remove(emp);
            emp = null;
            dataGridView.Rows.RemoveAt(this.dataGridView.SelectedRows[0].Index);
            dataGridView.Rows[0].Selected = true;

        }

        private void comboBoxTitle_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBoxTitle.SelectedItem == null)
            {
                textBoxBonus.Enabled = false;
                checkedListBoxSkills.Enabled = false;
            }
            else
            {
                textBoxBonus.Enabled = (comboBoxTitle.SelectedItem.ToString().ToLower().IndexOf("developer") != -1 || Manager.ManagerTitles.IndexOf(comboBoxTitle.SelectedItem.ToString().Trim()) != -1);
                checkedListBoxSkills.Enabled = (comboBoxTitle.SelectedItem.ToString().ToLower().IndexOf("developer") != -1);
            }

            if (!textBoxBonus.Enabled)
                textBoxBonus.Text = "";
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {


            if (dataGridView1.Rows.Count == 0)
            {
                return;
            }
            Employee.clearLanguages();
            Developer.clearSkills();
            int empID = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
            Employee emp = FindEmployee(empID);
            emp.fillSecondTable();
            int managerID = Manager.findManager(emp.GetID());
            Employee manager = FindEmployee(managerID);
            comboManagers.Text = manager.ToString();
            dataGridView1.Rows.Clear();
            dataGridView1.Enabled = false;
        }

    }
}

