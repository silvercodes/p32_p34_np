//int a = 3;
//switch (a)
//{
//    case 0:

//        break;
//    case 1:

//        break;
//    case 2:
//    case 3:
//        break;
//    default:

//        break;
//}


// ======= Pattern matching ==========
// 1. Constant matching
// 2. Type pattern
// 3. Property pattern
// 3.1 Positional pattern
// 4. Relational pattern
// 5. Logical pattern
// 6. List pattern


// --- Type pattern + when
//object? obj = "Hello";
//switch (obj)
//{
//    case int i when i > 0:
//        Console.WriteLine($"Positive integer: {i}");
//        break;

//    case string s:
//        Console.WriteLine($"String: {s}");
//        break;
//    case null:
//        Console.WriteLine("Null object ref");
//        break;
//}


// --- Property pattern

//User user = new User("Vasia", 26);

//switch (user)
//{
//    case { Age: < 18 }:
//        Console.WriteLine("No adult");
//        break;
//    case { Name: "John" }:
//        Console.WriteLine("Hello John");
//        break;
//    case { Age: >= 18 and <= 65 }:
//        Console.WriteLine("Hello");
//        break;
//}

//record User(string Name, int Age);


// --- Positional pattern (кортежи и деконструируемые объекты)
//User user = new User("Vasia", 23);
//(string n, int a) = user;
//Console.WriteLine($"{n} {a}");

//void Method((string n, int a) user)
//{
//    Console.WriteLine($"{n} {a}");
//}

//class User
//{
//    private String name;
//    private int age;
//    public User(String name, int age)
//    {
//        this.name = name;
//        this.age = age;
//    }
//    public void Deconstruct(out string userName, out int userAge)
//    {
//        userName = name;
//        userAge = age;
//    }
//}


//var point = (4, 4);
//switch(point)
//{
//    case (0, 0):
//        Console.WriteLine("0, 0");
//        break;
//    case (0, var y):
//        Console.WriteLine($"Y: {y}");
//        break;
//    case (var x, var y) when x == y:
//        Console.WriteLine("x == y");
//        break;
//}


// --- List
//int[] nums = { 1, 2, 3, 4, 5 };
//switch (nums)
//{
//    case [1, 2]:
//        Console.WriteLine($"1, 2");
//        break;
//    case [1, .. var middle, 5]:
//        Console.WriteLine($"Middle: {string.Join(",", middle)}");
//        break;
//    case [_, _, 3, ..]:
//        Console.WriteLine("Third element: 3");
//        break;
//}



// --- Combination
//int val = 45;
//switch (val)
//{
//    case 0 or 1:
//        Console.WriteLine("0 OR 1");
//        break;
//    case > 100 and < 200:
//        Console.WriteLine("range");
//        break;
//    case not 0 and not 1:
//        Console.WriteLine("not 0 and not 1");
//        break;
//}


// --- Var + condition
//switch (new[] { 10, 20 })
//{
//    case var arr when arr.Length == 2:
//        Console.WriteLine("var arr when arr.Length == 2");
//        break;
//    case var arr when arr.Sum() > 10:
//        Console.WriteLine("var arr when arr.Sum() > 10");
//        break;
//}



// --- as statement

//User u = new User("Vasia", 20);

//string result = u switch
//{
//    { Age: < 18} => "Child",
//    { Age: >= 18} => "Adult",
//    _ => "Invalid Age"
//};

//string GetDescription(User user) => user switch
//{
//    { Age: < 18 } => "Child",
//    { Age: >= 18 } => "Adult",
//    _ => "Invalid Age"
//};

//GetDescription(u);


//record User(string Name, int Age);




// --- CLI example

//string command = "add -user Vasia";
//switch (command.Split())
//{
//    case ["add", "-user", var name]:
//        Console.WriteLine($"Adding user: {name}");
//        break;
//    case ["delete", "-user", var username] when username != "admin":
//        Console.WriteLine($"Adding user: {username}");
//        break;
//    case ["exit"]:
//        Environment.Exit(0);
//        break;
//}












