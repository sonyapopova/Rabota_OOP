# Rabota_OOP

## Занятие 4

## Учебная группа 251-337

Участники группы: Попова София Николаевна, Макаров Роман Сергеевич, Титова Мария Дмитриевна, Лебедев Александр Андреевич.

Исходный текст задания: 
Мы продолжаем проект маркетплейса (занятия 2 и 3). Каталог, доступ к данным и
печать уже готовы; на этом занятии заменяем методы Get... / Set... на свойства и
добавляем перегрузки GetInfo .
Правила проекта:

Распределение задач по участникам
Участник 1 — класс Product ( Product.cs ) — главный по теме занятия
Класс должен содержать все четыре вида свойств из лабораторной и две перегрузки
GetInfo :
5. Как объявить обязательное свойство FullName , без которого объект не создать?
6. Почему нельзя объявить два метода GetInfo() с разными типами возврата?
7. Почему у свойства в readonly struct не может быть аксессора set ?

1. public string Group { get; set; } = "не распределён";
2. public int Age { get; private set; }
3. public bool IsJunior => Age < 20;
4. public string CardNumber { get; init; }
5. public required string FullName { get; init; }
6. Тип возврата не входит в сигнатуру метода, поэтому обе версии имеют одну и ту же
сигнатуру — ошибка CS0111.
7. readonly struct запрещает изменять свои поля после создания, поэтому set не
компилируется: допустимы init (значение задаётся при создании) или свойство
только для чтения.

1. Один тип — один файл; пространства имён Marketplace.Domain и
Marketplace.Services .
2. Имена свойств — существительные с большой буквы ( Price , IsBudget ), имена
методов — глаголы ( Calculate , GetReport ).
3. Названия типов из занятий 2 и 3 не меняются: Product , CustomerProfile , Cart ,
CartItem (у CartItem меняется только вид: класс становится readonly struct ).

Требуется дополнительно:

```csharp
namespace Marketplace.Domain;

public class Product
{
// 1. string — сокращённая (автоматическая) запись, обязательна при
создании
public required string Title { get; init; }
// 2. string — полная запись с проверкой на пустоту
private string article = string.Empty;
public string Article
{
get => article;
set
{
if (string.IsNullOrWhiteSpace(value))
throw new ArgumentException("Артикул обязателен",
nameof(value));
article = value;
}
}
// 3. int — полная запись, не допускает отрицательных значений
private int price;
public int Price
{
//TODO: ⚠️ get и set с проверкой value < 0 ⚠️
}
// 4. bool — вычисляемое свойство: true, если Price < 25
//TODO: ⚠️ public bool IsBudget => ... ⚠️
// 5. Перегрузки
//TODO: ⚠️ public string GetInfo() => GetInfo(true); ⚠️
//TODO: ⚠️ public string GetInfo(bool includeIntProperty) { ... } ⚠️
// 6. Статус меняется только внутри класса
public string Status { get; private set; } = "В наличии";
//TODO: ⚠️ public void ChangeStatus(string newStatus) ⚠️
//TODO: ⚠️ private static void EnsureStatus(string value) ⚠️
}
```

Участник 2 — класс CustomerProfile ( CustomerProfile.cs )
Требуется:

Участник 3 — скидки и расчёт ( Discount.cs )
1. Свойство Status с private set (статус меняется только внутри класса).
2. Метод ChangeStatus(string newStatus) с приватной проверкой допустимых
значений: "В наличии" , "Нет в наличии" , "Под заказ" (список допустимых
значений храните в приватном статическом поле).
3. Убедиться, что product.Price = -10; и product.Status = "Продан"; не
компилируются, а product.ChangeStatus("Продано"); выбрасывает исключение.

1. FullName — автоматическое свойство, required + init .
2. Phone — автоматическое свойство с инициализатором string.Empty .
3. Email — полная запись: сеттер проверяет, что строка содержит @ (приватный метод
EnsureValidEmail ).
4. Rating — int , полная запись с проверкой диапазона 0..5 , резервное поле
rating .
5. Вычисляемое свойство IsTrusted => Rating >= 4 .
6. Перегруженные методы:
string GetInfo() ;
string GetInfo(bool includeRating) ;
string GetContactInfo(bool includePhone) .
7. 1-2 приватных метода ( EnsureValidEmail , NormalizePhone ).

```csharp
namespace Marketplace.Domain;

public class CustomerProfile
{
public required string FullName { get; init; }
public string Phone { get; set; } = string.Empty;
//TODO: ⚠️ Email — полная запись с EnsureValidEmail ⚠️
//TODO: ⚠️ Rating — полная запись с проверкой 0..5 ⚠️
//TODO: ⚠️ public bool IsTrusted => Rating >= 4; ⚠️
//TODO: ⚠️ public string GetInfo() и GetInfo(bool includeRating) ⚠️
//TODO: ⚠️ public string GetContactInfo(bool includePhone) ⚠️
//TODO: ⚠️ private static void EnsureValidEmail(string value) ⚠️
//TODO: ⚠️ private static string NormalizePhone(string value) ⚠️
}
```

Требуется:

Участник 4 — позиция корзины и сервис оформления заказа
( CartItem.cs , CheckoutService.cs )
Требуется:
1. Класс Discount со свойствами:
Name — сокращённая запись, required ;
Percent — decimal , полная запись с проверкой 0..100 ;
MinSum — int , полная запись с проверкой >= 0 (минимальная сумма заказа);
ValidUntil — DateTime с init ;
IsActive — вычисляемое bool ( ValidUntil > DateTime.Now ).
2. Метод int Calculate(int total) — цена после скидки с учётом минимальной
суммы; внутри 1-2 приватных метода ( ApplyDiscount , EnsurePercent ).
3. Перегрузка int Calculate(int total, decimal percent) .
4. Показать, что передача недопустимых значений через свойства не проходит.

```csharp
namespace Marketplace.Domain;

public class Discount
{
public required string Name { get; init; }
public DateTime ValidUntil { get; init; }
//TODO: ⚠️ Percent — decimal, полная запись, 0..100 ⚠️
//TODO: ⚠️ MinSum — int, полная запись, >= 0 ⚠️
//TODO: ⚠️ public bool IsActive => ValidUntil > DateTime.Now; ⚠️
//TODO: ⚠️ public int Calculate(int total) ⚠️
//TODO: ⚠️ public int Calculate(int total, decimal percent) ⚠️
//TODO: ⚠️ private static int ApplyDiscount(int total, decimal percent)
⚠️
//TODO: ⚠️ private static void EnsurePercent(decimal value) ⚠️
}
```

1. readonly struct CartItem — позиция корзины со свойствами:
ProductId , Quantity , PricePerUnit — только для чтения, задаются
конструктором;
Total — вычисляемое свойство ( PricePerUnit * Quantity );
конструктор проверяет, что количество положительное, а цена не отрицательная.

i️ На занятии 2 позиция корзины была классом. Теперь оформляем её как readonly
struct : данные позиции после создания не меняются, а стоимость считается
свойством.
2. Сервисный класс CheckoutService без состояния (только методы):
int GetTotal(IEnumerable<CartItem> items) — сумма Total по всем позициям;
IReadOnlyList<CartItem> FindCheaperThan(IEnumerable<CartItem> items, int
maxPrice) - найти позиции дешевле заданной цены;
перегруженные string GetReport(IEnumerable<CartItem> items) (короткая
сводка) и string GetReport(IEnumerable<CartItem> items, bool includeLines)
(сводка со строками позиций).
3. 1-2 приватных метода: IsAvailable(CartItem item) и PrepareLine(CartItem
item) - "доступна ли позиция для покупки?" и "Подготовить красивую строку для
отчёта или чека на основе товарной позиции в корзине".
4. Сервис не должен обращаться к закрытым полям чужих классов — только к открытым
свойствам и методам.

```csharp
namespace Marketplace.Domain;

public readonly struct CartItem
{
public int ProductId { get; }
public int Quantity { get; }
public int PricePerUnit { get; }
//TODO: ⚠️ Конструктор с проверками ⚠️
//TODO: ⚠️ public int Total => PricePerUnit * Quantity; ⚠️
}
```

```csharp
using System;
using System.Collections.Generic;

namespace Marketplace.Services;

public class CheckoutService
{
    public int GetTotal(IEnumerable<CartItem> items)
    {
        //TODO: ⚠️ Сумма Total по всем позициям ⚠️
        return 0;
    }

    //TODO: ⚠️ public IReadOnlyList<CartItem> FindCheaperThan(...) ⚠️
    //TODO: ⚠️ public string GetReport(IEnumerable<CartItem> items) ⚠️
    //TODO: ⚠️ public string GetReport(IEnumerable<CartItem> items, bool includeLines) ⚠️
    //TODO: ⚠️ private bool IsAvailable(CartItem item) ⚠️
    //TODO: ⚠️ private string PrepareLine(CartItem item) ⚠️
}
```

Компоновка (задача Лидера)
Лидер рабочей группы:

Рекомендации по выполнению

## Задачи были распределены между участниками следующим образом

Мария:
Задачи участника 1
Роман:
Задачи участника 2
Александр:
Задачи участника 3
София:
Задачи участника 4 — вывод и отчёты (SRP)



## Структура проекта

| Файл | Модификатор | Пространство имён | Назначение |
|---|---|---|---|
| `Program.cs` | `internal` | `Marketplace.Presentation` | точка входа: сценарии работы с профилем, успешной и неуспешной оплаты, проверка владельца корзины, работа каталога, сохранение/загрузка товаров и отчёты |
| `User.cs` | `public` | `Marketplace.Domain` | класс `User` — пользователь (ФИО, логин, пароль, уникальный ID) |
| `Contact.cs` | `public` | `Marketplace.Domain` | класс `Contact` — контакт пользователя (значение + ID канала) |
| `Channel.cs` | `public` | `Marketplace.Domain` | класс `Channel` — канал связи (телефон, email, telegram) |
| `UserProfile.cs` | `public` | `Marketplace.Domain` | класс `UserProfile` — профиль пользователя: контакты, денежный и бонусный балансы, персональная скидка, история покупок |
| `ProfileAdd.cs` | `public` | `Marketplace.Domain` | сервис `ProfileAdd` — создание профиля, контакты, вывод сводки и пополнение баланса |
| `Product.cs` | `public` | `Marketplace.Domain` | класс `Product` — товар (ID, название, цена за единицу) с проверкой входных данных |
| `Catalog.cs` | `public` | `Marketplace.Domain` | класс `Catalog` — каталог товаров: добавление, поиск, удаление и защита внутренней коллекции от изменения снаружи |
| `CartItem.cs` | `public` | `Marketplace.Domain` | класс `CartItem` — позиция корзины (ID, ID товара, количество, стоимость) |
| `ProductItem.cs` | `public` | `Marketplace.Domain` | класс `ProductItem` — сохранённая покупка (позиция и дата приобретения) |
| `Cart.cs` | `public` | `Marketplace.Domain` | класс `Cart` — корзина пользователя (ID, ID пользователя, список позиций `CartItem`) |
| `CartPrice.cs` | `public` | `Marketplace.Domain` | структура `CartPrice` — расчётная цена корзины (исходная цена, множитель скидки, размер скидки) |
| `BuyerBalance.cs` | `public` | `Marketplace.Domain` | класс `BuyerBalance` — баланс покупателя (бонусы, деньги, доля оплаты бонусами) |
| `BuyerBalanceState.cs` | `public` | `Marketplace.Domain` | структура `BuyerBalanceState` — снимок состояния баланса покупателя |
| `PaymentService.cs` | `public` | `Marketplace.Domain` | сервис `PaymentService` — расчёт оплаты, списание бонусов и денег, сообщения об оплате, проверка владельца корзины |
| `CustomerProfile.cs` | `public` | `Marketplace.Domain` | класс `CustomerProfile` — профиль покупателя: ФИО, телефон, email, рейтинг, вычисляемое свойство `IsTrusted` и перегрузки `GetInfo` |
| `Discount` | `public` | `Marketplace.Domain` | класс `Discount` — скидка: процент, минимальная сумма, срок действия и расчёт суммы со скидкой |
| `DataAccess/IProductRepository.cs` | `public` | `Marketplace.DataAccess` | интерфейс `IProductRepository` — абстракция хранения товаров с методами `Save` и `Load` |
| `DataAccess/FileProductRepository.cs` | `internal` | `Marketplace.DataAccess` | `internal class FileProductRepository` — файловое хранилище товаров в формате `id;title;price` |
| `Presentation/CatalogPrinter.cs` | `public`; `file` | `Marketplace.Presentation` | класс `CatalogPrinter` — печать всех товаров и топа по цене; в этом же файле находится `file static class PriceFormatter` |
| `Presentation/CatalogReport.cs` | `public` | `Marketplace.Presentation` | класс `CatalogReport` — расчёты средней цены и сводки по каталогу без печати и работы с файлами |

Модификатор `public` делает тип доступным из других сборок, `internal` — только внутри текущей сборки, `file` — только в пределах одного файла.

### Слои приложения

- `Marketplace.Domain` — доменные сущности и бизнес-правила: `Product`, `Catalog`, `Cart`, `UserProfile` и другие.
- `Marketplace.DataAccess` — хранение товаров через `IProductRepository`; конкретная реализация — `FileProductRepository`.
- `Marketplace.Presentation` — точка входа `Program`, печать `CatalogPrinter` и отчёты `CatalogReport`.

`CatalogPrinter` отвечает только за вывод, `CatalogReport` — только за расчёты, а `Catalog` — только за работу с коллекцией товаров. Это разделение демонстрирует принцип единственной ответственности (SRP). Работа с хранилищем в `Program` идёт через интерфейс `IProductRepository`, то есть по принципу инверсии зависимостей (DIP).

## Что демонстрирует программа

1. Создание списка каналов связи.
2. Создание пользователя и его профиля.
3. Добавление контактов (телефон, email, Telegram).
4. Вывод сводки по профилю.
5. Удаление контакта по его ID и повторный вывод сводки.
6. Сценарий успешной оплаты корзины: расчёт цены со скидкой, списание бонусов и денег, очистка корзины.
7. Проверка того, что корзину другого пользователя оплатить нельзя.
8. Сценарий неуспешной оплаты: сохранение корзины и балансов, сообщение о нехватке средств.
9. Пополнение баланса на недостающую сумму и повторная успешная оплата.
10. Работу каталога: отклонение некорректного товара, добавление, поиск, удаление, вывод товаров, сохранение и загрузка через `IProductRepository`, расчёт средней цены и сводки через `CatalogReport`.
11. Отклонение дубликата товара: повторное добавление «Кружки» в другом регистре выбрасывает `InvalidOperationException`.
12. Сохранение и загрузку товара с символом `;` в названии — формат `id;title;price` разбирается от первого и последнего разделителя.
13. Защиту коллекции: `catalog.GetProducts().Clear()` не компилируется (`CS1061`), потому что `GetProducts()` возвращает `IReadOnlyList<Product>`.

## Доказательство инкапсуляции

Инкапсуляция — это сокрытие внутреннего состояния объекта и доступ к нему только через публичные свойства и методы, которые проверяют допустимость значений. В проекте это доказывают три вещи:

1. Внутренние поля закрыты (`private`), поэтому снаружи их нельзя изменить напрямую.
2. Изменение состояния идёт через публичный контракт: `GetId()`, `GetTitle()`, `GetPrice()`, `SetPrice()` и методы каталога.
3. Некорректные значения не проходят: компилятор запрещает обходить проверки, а проверки выбрасывают исключения.

| Проверка | Код, который не должен компилироваться | Ошибка компилятора |
|---|---|---|
| Закрытое поле `Product.price` | `product.price = -100;` | `CS0122: "Product.price" недоступен из-за его уровня защиты.` |
| В `Product` нет дублирующего свойства `Price` | `product.Price = -100;` | `CS1061: "Product" не содержит определения "Price".` |
| В `Product` нет дублирующего свойства `Id` | `product.Id = 2;` | `CS1061: "Product" не содержит определения "Id".` |
| Закрытое поле `CustomerProfile.email` | `profile.email = "bad";` | `CS0122: "CustomerProfile.email" недоступен из-за его уровня защиты.` |
| Закрытая коллекция `Catalog.products` | `catalog.products.Add(product);` | `CS0122: "Catalog.products" недоступен из-за его уровня защиты.` |
| Коллекция отдаётся только для чтения | `catalog.GetProducts().Clear();` | `CS1061: "IReadOnlyList<Product>" не содержит определения "Clear".` |

Ошибка `CS1061` для `GetProducts().Clear()` воспроизводится отдельной сборкой:

```bash
dotnet build Rabota_OOP/Rabota_OOP.csproj -p:DefineConstants=NEGATIVE_CHECKS
```

Проверки внутри класса:

```csharp
private decimal price;

public decimal GetPrice() => price;

public void SetPrice(int newPrice)
{
    EnsurePrice(newPrice);
    price = newPrice;
}

private static void EnsurePrice(decimal price)
{
    if (price < 0m)
        throw new ArgumentOutOfRangeException(
            nameof(price),
            "Цена товара не может быть отрицательной.");
}
```

Проверка во время выполнения:

```csharp
try
{
    Product product = new Product(1, "Кружка", 100m);
    product.SetPrice(-100);
}
catch (ArgumentOutOfRangeException ex)
{
    Console.WriteLine(ex.Message);
}
```

Вывод:

```text
Цена товара не может быть отрицательной. (Parameter 'price')
```

`Catalog` также не отдаёт внутренний список: поле `products` закрыто, а `GetProducts()` возвращает `IReadOnlyList<Product>`, созданный через `products.ToList()`. Поэтому снаружи нельзя добавить товар в обход `Add()` и нельзя изменить внутреннюю коллекцию через возвращённое значение.

## Распределение работы

### Часть Ромы Макарова


### Часть Саши Лебедева



### Часть Маши Титовой



### Часть Сони Поповой (лидер группы)

В коммите Романа от 4 ноября `SetPrice()` и `GetInfo()` действительно были. При слиянии его ветки с основной версией `Product` я (лидер группы) сохранила другую реализацию: вариант Романа использовал `int price` и был несовместим с текущими `CartItem`, `Catalog`, `FileProductRepository` и `CatalogReport`. Из-за этого при разрешении конфликта методы не были перенесены в итоговый файл — это ошибка интеграции с моей стороны.

Сейчас я вернула в итоговый `Product.cs` методы `SetPrice(int)` и `GetInfo()`, а также добавила перегрузку `SetPrice(decimal)`, чтобы сохранить используемый в проекте `decimal`. Проверка названия и цены вынесена в приватные методы `EnsureTitle` и `EnsurePrice`.

Требование «`SetPrice(-100)` бросает исключение» теперь можно показать в `Program`:

```csharp
try
{
    Product priceCheck = new Product(5, "Проверка цены", 100m);
    priceCheck.SetPrice(-100);
}
catch (ArgumentOutOfRangeException ex)
{
    Console.WriteLine($"SetPrice(-100) отклонён: {ex.Message}");
}
```

При запуске выводится:

```text
SetPrice(-100) отклонён: Цена товара не может быть отрицательной.
```

Прямое присваивание `product.Price = -100` не компилируется: в `Product` оставлен один способ доступа к цене — `GetPrice()` для чтения и `SetPrice()` для проверяемого изменения. Свойств `Id`, `Name` и `Price` больше нет, поэтому публичный API не дублируется.



## Выводы


## Запуск

```bash
dotnet run --project Rabota_OOP/Rabota_OOP.csproj
```

Требуется .NET SDK 10.0 или новее.
