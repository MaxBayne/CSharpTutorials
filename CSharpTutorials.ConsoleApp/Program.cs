

using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Numerics;

namespace CSharpTutorials.ConsoleApp
{
    public class Program
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

        #region Threading

        /*
            //Approch 1: Create Thread and Start it
            //--------------------------

            var threadA = new Thread(ProcessA);
            var threadB = new Thread(ProcessB);

            threadA.Start();
            threadB.Start();

            //Approch 2: Use ThreadPool to Queue the Threads and Start it
            //------------------------------------------------

            ThreadPool.QueueUserWorkItem((state)=>ProcessA());
            ThreadPool.QueueUserWorkItem((state) => ProcessB());


            //Approch 3: Use Task.Run As ThreadPool to Queue the Tasks and Start it
            //------------------------------------------------
            Task.Run(()=>DoJobA());

            //Task A Will Run after it finish Task B will Run , compiler will wait TaskA and will wait TaskB 

            var taskA  = ProcessA_Async();

            var taskB = await taskA.ContinueWith((taskA)=> ProcessB_Async());

            await taskB;



        



         */

        public static object _lock = new object();

        public static void ProcessA()
        {
            Console.WriteLine($"Process-A Thread Id = {Environment.CurrentManagedThreadId}");

            for (int i = 0; i < 1000; i++)
            {
                lock (_lock)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Green - {i}");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                
            }
        }

        public static void ProcessB()
        {
            Console.WriteLine($"Process-B Thread Id = {Environment.CurrentManagedThreadId}");

            for (int i = 1001; i < 2000; i++)
            {
                lock (_lock)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Red - {i}");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                
            }
        }

        public static async Task ProcessA_Async()
        {
            await Task.Delay(1);

            Console.WriteLine($"Process-A Async Thread Id = {Environment.CurrentManagedThreadId}");

            for (int i = 0; i < 1000; i++)
            {
                lock (_lock)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Green - {i}");
                    Console.ForegroundColor = ConsoleColor.White;
                }

            }
        }

        public static async Task ProcessB_Async()
        {
            await Task.Delay(1);

            Console.WriteLine($"Process-B Async Thread Id = {Environment.CurrentManagedThreadId}");

            for (int i = 1001; i < 2000; i++)
            {
                lock (_lock)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Red - {i}");
                    Console.ForegroundColor = ConsoleColor.White;
                }

            }
        }

        #endregion

        #region Collections::Enumerables

        /*
            //Create Invoice
            var invoice = new Invoice(1000,"Sales Invoice");

            //Create Items For Invoice
            invoice.AddItem("1", "Product A", 1.0, 150.10);
            invoice.AddItem("2", "Product B", 2.0, 120.00);
            invoice.AddItem("3", "Product C", 3.0, 170.50);
            invoice.AddItem("4", "Product D", 4.0, 180.10);
            invoice.AddItem("5", "Product E", 5.0, 140.20);

            
            Console.WriteLine("ForEach Loop For Items of Invoice using List");
            Console.WriteLine("====================================");
            foreach (var item in invoice.Items)
            {
                Console.WriteLine($"Code={item.Code},Name={item.Name},Quantity={item.Quantity},UnitPrice={item.UnitPrice},Total={item.Total}");
                Console.WriteLine("-------------------------------------------------------------");
            }

            Console.WriteLine();
            Console.WriteLine("ForEach Loop For Items of Invoice using IEnumerator");
            Console.WriteLine("===========================================");
           
            foreach (var item in invoice)
            {
                Console.WriteLine($"Code={item.Code},Name={item.Name},Quantity={item.Quantity},UnitPrice={item.UnitPrice},Total={item.Total}");
                Console.WriteLine("-------------------------------------------------------------");
            }

            Console.WriteLine();
            Console.WriteLine("For Loop For Items of Invoice using IEnumerator");
            Console.WriteLine("===========================================");

            for (int i = 0; i < invoice.Count(); i++)
            {
                Console.WriteLine($"Code={invoice[i].Code},Name={invoice[i].Name},Quantity={invoice[i].Quantity},UnitPrice={invoice[i].UnitPrice},Total={invoice[i].Total}");
                Console.WriteLine("-------------------------------------------------------------");
            }
            
            */

        public class InvoiceItem
        {
            public InvoiceItem(string code,string name,double quantity,double unitPrice)
            {
                Code = code;
                Name = name;
                Quantity = quantity;
                UnitPrice = unitPrice;
            }

            public string Code { get; private set; }
            public string Name { get; private set; }
            public double Quantity { get; private set; }
            public double UnitPrice { get; private set; }
            public double Total => Quantity * UnitPrice;
        }

        public class Invoice: IEnumerable<InvoiceItem> 
        {
            #region Properties


            public int InvoiceId { get; private set; }
            public DateTime InvoiceDate { get; private set; }
            public string InvoiceNotes { get; private set; }

            private readonly List<InvoiceItem> _items;
            public IReadOnlyList<InvoiceItem> Items => _items.AsReadOnly();

            #endregion

            #region Constructors

            public Invoice(int id,string notes)
            {
                InvoiceId = id;
                InvoiceDate = DateTime.Now;
                InvoiceNotes = notes;
                _items = new();
            }

            #endregion
            

            #region Items Methods

            public void AddItem(string code, string name, double quantity, double unitPrice)
            {
                _items.Add(new InvoiceItem(code,name,quantity,unitPrice));
            }
            public void RemoveItem(string code)
            {
                var itemToRemove = _items.Find((c) => c.Code == code);
                if(itemToRemove!=null)
                    _items.Remove(itemToRemove);
            }
            public void RemoveItem(int index)
            {
                _items.RemoveAt(index);
            }



            #endregion

            #region IEnumerable Methods

            public IEnumerator<InvoiceItem> GetEnumerator()
            {
                //return _items.GetEnumerator();

                //or

                foreach (var item in _items)
                {
                    yield return item;
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            #endregion

            #region Support Index Like Array or List
            
            public InvoiceItem this[int index] => _items[index];

            #endregion
        }

        #endregion

        #region Generic

        /*
            var stringRepo = new Repository<string>();

            stringRepo.Add("Ahmed");
            stringRepo.Add("Ali");
            stringRepo.Add("Khalid");
            stringRepo.Add("Mostafa");

            var result = Add<decimal>(100,500);

         */

       

        //Generic Class with Constraints
        public class Repository<TEntity,TDbContext> where TEntity:class,new()
                                                    where TDbContext:class,new()
        {
            private readonly TDbContext _dbContext;
            private readonly List<TEntity> _entities;

            public Repository()
            {
                _dbContext = new();
                _entities = new List<TEntity>();
            }

            public void Add(TEntity entity)
            {
                _entities.Add(entity);
            }

            public void Remove(TEntity entity)
            {
                _entities.Remove(entity);
            }

            public TEntity Find(Predicate<TEntity> predicate)
            {
                return _entities.Find(predicate)!;
            }
        }

        //Generic Interface
        public interface IRepository<TEntity> where TEntity:class
        {
            void Add(TEntity entity);

            void Remove(TEntity entity);

            TEntity Find(Predicate<TEntity> predicate);
        }

        //Generic Methods
        public void AddToRepository<TRepository>(TRepository entity)
        {
            Repository<TRepository> repository=new();

            repository.Add(entity);
        }
        
        public static T Add<T>(T number1, T number2) where T : INumber<T>
        {
            return number1 + number2;
        }
        
        #endregion


        public static async Task Main(string[] args)
        {

            



            Console.WriteLine("Hello World");
            Console.ReadKey();

            
        }
    }

}

