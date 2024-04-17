using System.Text.Json;

namespace Client
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            bool runAgain = true;

            while (runAgain)
            {
                Console.WriteLine("Выберите операцию:");
                Console.WriteLine("1. Сложение");
                Console.WriteLine("2. Вычитание");
                Console.WriteLine("3. Умножение");
                Console.WriteLine("4. Деление");

                string operationChoice = Console.ReadLine();

                if (operationChoice != "1" && operationChoice != "2" && operationChoice != "3" && operationChoice != "4")
                {
                    Console.WriteLine("Неверный выбор операции.");
                    continue;
                }
                Console.WriteLine("Введите первое число:");
                string number1 = Convert.ToString(Console.ReadLine());
                
                Console.WriteLine("Введите второе число:");
                string number2 = Convert.ToString(Console.ReadLine());

                string url = "";
                switch (operationChoice)
                {
                    case "1":
                        url = "http://localhost:5085/Plus";
                        break;
                    case "2":
                        url = "http://localhost:5085/Minus";
                        break;
                    case "3":
                        url = "http://localhost:5085/Multiplication";
                        break;
                    case "4":
                        url = "http://localhost:5085/Divide";
                        break;
                }

                using var client = new HttpClient();

                var numbers = new Numbers { A = number1, B = number2 };
                var json = JsonSerializer.Serialize(numbers);

                try
                {
                    var response = await client.PostAsync(url, new StringContent(json, System.Text.Encoding.UTF8, "application/json"));

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var result = JsonSerializer.Deserialize<double>(responseContent);

                        Console.WriteLine($"Результат операции: {result}");
                    }
                    else
                    {
                        Console.WriteLine($"Ошибка при выполнении запроса: {response.StatusCode}");
                        if (response.StatusCode == System.Net.HttpStatusCode.ExpectationFailed)
                        {
                            runAgain = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }

                Console.WriteLine("Желаете выполнить еще одну операцию? (Да/Нет)");
                string answer = Console.ReadLine();

                if (answer.ToLower() != "да")
                    runAgain = false;
               
            }
        }

        public class Numbers
        {
            public string A { get; set; }
            public string B { get; set; }
        }
    }
}
