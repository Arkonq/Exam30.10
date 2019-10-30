using DbUp;
using Domain;
using Repository;
using System;
using System.Reflection;

namespace Exam
{
	class Program
	{
		private const string CONNECTION_STRING = "Server=A-305-04;Database=StoreExam;Trusted_Connection=true;";

		public static StoreRepository storeRepository = new StoreRepository(CONNECTION_STRING);
		public static StorekeeperRepository storekeeperRepository = new StorekeeperRepository(CONNECTION_STRING);
		public static AcceptedWaybillRepository acceptedWaybillRepository = new AcceptedWaybillRepository(CONNECTION_STRING);
		public static WaybillRepository waybillRepository = new WaybillRepository(CONNECTION_STRING);

		static string[] actions = { "Вывоз", "Ввоз" };

		static Store store = storeRepository.GetStore();
		static Storekeeper storekeeper;

		static void Main()
		{
			//DbUp();
			Menu();
		}

		static void Menu()
		{
			int entryAnswer = -1;
			while (entryAnswer != 0)
			{
				Console.Clear();
				Console.WriteLine("1. Вход");
				Console.WriteLine("0. Выход");
				Console.Write("Выберите действие: ");
				if (Int32.TryParse(Console.ReadLine(), out entryAnswer) == false || entryAnswer < 0 || entryAnswer > 1)
				{
					entryAnswer = -1;  
					WriteMessage("Введенное действие не является корректным. Выберите действие из списка!");
					continue;
				}
				switch (entryAnswer)
				{
					case 1:
						if (Entry())
						{							
							MenuActions();
						}
						break;
					case 0:
						entryAnswer = 0;
						break;
				}
			}
		}

		private static void MenuActions()
		{
			int menuAnswer = -1;
			while (menuAnswer != 0)
			{
				Console.Clear();
				Console.WriteLine("1. Принять накладную");
				Console.WriteLine("2. Выдать со склада по накладной");
				Console.WriteLine("3. Создать накладную (Вообще в реальной программе такого не будет)");
				Console.WriteLine("4. Вывести кол-во на складе");
				Console.WriteLine("0. Выход");
				Console.Write("Выберите действие: ");
				if (Int32.TryParse(Console.ReadLine(), out menuAnswer) == false || menuAnswer < 0)
				{
					menuAnswer = -1;  // При выпадении false в Int32.TryParse параметр out menuAnswer присваивается значние 0, и с новым циклом программа завершает работу
					WriteMessage("Введенное действие не является корректным. Выберите действие из списка!");
					continue;
				}
				Console.Clear();
				switch (menuAnswer)
				{
					case 1:
						AcceptWaybill();
						break;
					case 2:
						CreateWaybillFromStore();
						break;
					case 3:
						CreateWaybill();
						break;
					case 4:
						ShowStore();
						break;
					case 0:
						break;
				}
			}
		}

		private static void CreateWaybillFromStore()
		{
			int apples, bananas, pears;
			Console.Write("Введите кол-во яблок: ");
			Int32.TryParse(Console.ReadLine(), out apples);
			Console.Write("Введите кол-во бананов: ");
			Int32.TryParse(Console.ReadLine(), out bananas);
			Console.Write("Введите кол-во груш: ");
			Int32.TryParse(Console.ReadLine(), out pears);

			Waybill waybill = new Waybill
			{
				StoreId = store.Id,
				Apples = apples,
				Bananas = bananas,
				Pears = pears,
				Action = actions[1]
			};
			waybillRepository.Add(waybill);
			Console.WriteLine("Накладная сформирована");
			Console.ReadLine();
		}

		private static void ShowStore()
		{
			Console.WriteLine($"Яблок - {store.Apples}");
			Console.WriteLine($"Бабанов - {store.Bananas}");
			Console.WriteLine($"Груш - {store.Pears}");
			Console.ReadLine();
		}

		private static void AcceptWaybill()
		{
			Console.WriteLine("\tСписок накладных:");
			int num = 1;
			foreach(var waybill in waybillRepository.GetAll())
			{
				Console.WriteLine($"{num++}) Яблоки - {waybill.Apples}, Бананы - {waybill.Bananas}, Груши - {waybill.Pears}");
			}
			Console.Write("\tВведите номер принимаемой накладной: ");
			int acceptNum;
			if (Int32.TryParse(Console.ReadLine(), out acceptNum) == false || acceptNum < 1 || acceptNum > waybillRepository.GetAll().Count)
			{
				WriteMessage("Введенное действие не является корректным. Возвращение в главное меню!");
				return;
			}
			var wayBill = waybillRepository.GetByNum(acceptNum);

			store.Apples += wayBill.Apples;
			store.Bananas += wayBill.Bananas;
			store.Pears += wayBill.Pears;

			storeRepository.Update(store);
			waybillRepository.Delete(wayBill);
			acceptedWaybillRepository.Add(store,wayBill,storekeeper);
		}

		private static void CreateWaybill()
		{
			int apples, bananas, pears;
			Console.Write("Введите кол-во яблок: ");
			Int32.TryParse(Console.ReadLine(), out apples);
			Console.Write("Введите кол-во бананов: ");
			Int32.TryParse(Console.ReadLine(), out bananas);
			Console.Write("Введите кол-во груш: ");
			Int32.TryParse(Console.ReadLine(), out pears);

			Waybill waybill = new Waybill
			{
				StoreId = store.Id,
				Apples = apples,
				Bananas = bananas,
				Pears = pears,
				Action = actions[1]
			};
			waybillRepository.Add(waybill);
			Console.WriteLine("Накладная сформирована");
			Console.ReadLine();
		}

		private static bool Entry()
		{
			Console.Clear();
			Console.WriteLine("Введите Имя: ");
			string name = Console.ReadLine();
			Console.WriteLine("Введите пароль: ");
			string pass = Console.ReadLine();
			if (storekeeperRepository.Authorization(name, pass))
			{
				storekeeper = storekeeperRepository.GetStorekeeper(name);
				return true;
			}
			return false;
		}

		private static void WriteMessage(string message)
		{
			Console.Clear();
			Console.WriteLine(message);
			Console.ReadLine();
			Console.Clear();
		}

		private static void DbUp()
		{
			EnsureDatabase.For.SqlDatabase(CONNECTION_STRING);  // Создание БД
			var upgrader =
					DeployChanges.To                      // Накатывание всех скриптов
							.SqlDatabase(CONNECTION_STRING)
							.WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly()) // Получение всех файлов в сборке со словом Script и параметром Embedded
							.LogToConsole()
							.Build();
			upgrader.PerformUpgrade();
		}
	}
}
