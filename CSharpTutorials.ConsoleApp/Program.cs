

namespace CSharpTutorials.ConsoleApp
{
    public static class Program
    {
        #region Delegates

        /*
            CalculateByDelegate(10, 5, Add);
            CalculateByDelegate(10, 5, Multiply);
            CalculateByDelegate(10, 5,delegate (int number1, int number2) { return number1 / number2; });
            CalculateByDelegate(10, 5, (int number1, int number2) =>number1 + number2 + 5);
            CalculateByDelegate(10, 5, (int number1, int number2)=>
            {
                return number1 * number2; 
            });

         */

        public delegate int CalculateDelegate(int number1,int number2);
        
        public static int CalculateByDelegate(int number1,int number2,CalculateDelegate dlg)
        {
            var result = dlg(number1, number2);

            Console.WriteLine(result);

            return result;
        }

        public static int Add(int number1,int number2)
        {
            Console.WriteLine($"Add {number1}+{number2}");
            return number1 + number2;
        }
        public static int Subtract(int number1, int number2)
        {
            Console.WriteLine($"Subtract {number1}-{number2}");
            return number1 - number2;
        }
        public static int Multiply(int number1, int number2)
        {
            Console.WriteLine($"Multiply {number1}*{number2}");
            return number1 * number2;
        }

        #endregion

        #region Events

        /*
            Employee emp = new Employee(100, "Ahmed Ali");

            //Register Event
            emp.EmployeeSalaryCalculated += (emp, netSalary) => Console.WriteLine($"Employee = {emp.Name} has Net Salary = {netSalary}");

            //Make Event Fire
            emp.SetSalary(1000,500,120);
         */

        public class Employee
        {
            #region Events

            //Define the Delegate For Event Method (Event Handlers)
            public delegate void EmployeeNameChangedEventHandler(string oldName, string newName);
            public delegate void EmployeeSalaryCalculatedEventHandler(Employee employee,decimal netSalary);
            public delegate void EmployeeHaveDeductionEventHandler(Employee employee, decimal deductedValue);

            //Define Events that will be Fired 
            public event EmployeeNameChangedEventHandler EmployeeNameChanged;
            public event EmployeeSalaryCalculatedEventHandler EmployeeSalaryCalculated;
            public event EmployeeHaveDeductionEventHandler EmployeeHaveDeduction;

            #endregion

            #region Properties


            /// <summary>
            /// كود الموظف
            /// </summary>
            public int Id { get; private set; }

            /// <summary>
            /// اسم الموظف
            /// </summary>
            public string Name { get; private set; }

            /// <summary>
            /// المرتب الاساسي
            /// </summary>
            public decimal BasicSalary { get; private set; }

            /// <summary>
            /// الاضافات
            /// </summary>
            public decimal Bonus { get; private set; }

            /// <summary>
            /// الخصومات
            /// </summary>
            public decimal Deduction { get; private set; }

            /// <summary>
            /// صافي الراتب
            /// </summary>
            public decimal NetSalary { get; private set; }

            #endregion

            #region Constructor

            public Employee(int id, string name)
            {
                Id = id;
                Name = name;
            }
            public Employee(int id, string name, decimal basicSalary)
            {
                Id = id;
                Name = name;
                BasicSalary = basicSalary;

                CalculateNetSalary();
            }
            public Employee(int id,string name,decimal basicSalary,decimal bonus,decimal deduction)
            {
                Id = id;
                Name = name;
                BasicSalary = basicSalary;
                Bonus = bonus;
                Deduction = deduction;

                CalculateNetSalary();
            }

            #endregion

            #region Actions

            public void SetName(string name)
            {
                EmployeeNameChanged?.Invoke(Name,name);

                Name = name;
            }

            public void SetSalary(decimal basic,decimal bonus,decimal deduction)
            {
                BasicSalary = basic;
                Bonus = bonus;
                Deduction = deduction;

                CalculateNetSalary();
            }

            public void SetBasicSalary(decimal basicSalary)
            {
                BasicSalary = basicSalary;

                CalculateNetSalary();
            }

            public void SetBonus(decimal bonus)
            {
                Bonus = bonus;

                CalculateNetSalary();
            }

            public void SetDeduction(decimal deduction)
            {
                Deduction = deduction;

                EmployeeHaveDeduction?.Invoke(this, deduction);

                CalculateNetSalary();
            }

            public void PrintInfo()
            {
                Console.WriteLine($"Id={Id},Name={Name}");
            }

            public void PrintSalary()
            {
                Console.WriteLine($"BasicSalary={BasicSalary},Bonus={Bonus},Deduction={Deduction},NetSalary={NetSalary}");
            }

            private void CalculateNetSalary()
            {
                NetSalary = BasicSalary + Bonus - Deduction;

                EmployeeSalaryCalculated?.Invoke(this,NetSalary);
            }

            #endregion


        }

        #endregion

        public static void Main(string[] args)
        {
            


            Console.ReadKey();
        }
    }

}

