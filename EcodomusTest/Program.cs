using System;

namespace EcodomusTest
{
	class Program
	{
		static void Main(string[] args)
		{
			Database db = new Database();

			////Example1
			//db.Set("a", 50);
			//PrintResult(db.Get("a"));
			//db.Delete("a");
			//PrintResult(db.Get("a"));



			////Example 2
			//db.Begin();
			//db.Set("a", 50);
			//PrintResult(db.Get("a"));

			//db.Begin();
			//db.Set("a", 60);
			//PrintResult(db.Get("a"));
			//db.Rollback();

			//PrintResult(db.Get("a"));
			//db.Rollback();
			//PrintResult(db.Get("a"));



			////Example3
			//db.Begin();
			//db.Set("a", 40);
			//db.Begin();
			//db.Set("a", 50);
			//db.Commit();
			//PrintResult(db.Get("a"));
			//db.Rollback();


			////Example4
			//db.Set("a", 50);
			//db.Begin();
			//PrintResult(db.Get("a"));
			//db.Set("a", 70);
			//db.Begin();
			//db.Set("b", 90);
			//PrintResult(db.Get("b"));
			//db.Delete("a");
			//PrintResult(db.Get("a"));
			//db.Rollback();
			//PrintResult(db.Get("a"));
			//db.Commit();
			//PrintResult(db.Get("a"));
			//PrintResult(db.Get("b"));



			////Example5
			//db.Set("a", 20);
			//db.Begin();
			//PrintResult(db.Count(20));
			//db.Begin();
			//db.Delete("a");
			//PrintResult(db.Count(20));
			//db.Rollback();
			//db.Set("b", 20);
			//PrintResult(db.Count(20));
			//db.Commit();
			//PrintResult(db.Count(20));


			/*
			//Example6
			db.Rollback();
			*/

			Console.ReadLine();
		}

		private static void PrintResult(int? result)
		{
			if (result.HasValue)
			{
				Console.WriteLine(result);
			}
			else
			{
				Console.WriteLine("NULL");
			}
		}
	}
}
