using System.Text.Json;

namespace Client
{
    internal class Program
    {
        static async Task Main(string[] args)
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
                return;
            }

            Console.WriteLine("Введите первое число:");
            int number1 = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Введите второе число:");
            int number2 = Convert.ToInt32(Console.ReadLine());

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
                    var result = JsonSerializer.Deserialize<int>(responseContent);

                    Console.WriteLine($"Результат операции: {result}");
                }
                else
                {
                    Console.WriteLine($"Ошибка при выполнении запроса: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
    }

    public class Numbers
    {
        public int A { get; set; }
        public int B { get; set; }
    }
}

