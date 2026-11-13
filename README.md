# Rabota_OOP

## Занятие 4. Свойства и перегрузка методов

## Учебная группа 251-337

Участники группы: Попова София Николаевна (лидер группы), Макаров Роман Сергеевич, Титова Мария Дмитриевна, Лебедев Александр Андреевич.

## Исходный текст задания

### Задание 3 (групповое)

#### Проект

Мы **продолжаем проект маркетплейса** (занятия 2 и 3). Каталог, доступ к данным и печать уже готовы; на этом занятии заменяем методы `Get...`/`Set...` на **свойства** и добавляем **перегрузки** `GetInfo`.

Правила проекта:
1. Один тип — один файл; пространства имён `Marketplace.Domain` и `Marketplace.Services`.
2. Имена свойств — существительные с большой буквы (`Price`, `IsBudget`), имена методов — глаголы (`Calculate`, `GetReport`).
3. Названия типов из занятий 2 и 3 не меняются: `Product`, `CustomerProfile`, `Cart`, `CartItem` (у `CartItem` меняется только вид: класс становится `readonly struct`).

#### Распределение задач по участникам

##### Участник 1 — класс `Product` (`Product.cs`) — главный по теме занятия

Класс должен содержать **все четыре вида свойств** из лабораторной и **две перегрузки** `GetInfo`:

```cs
namespace Marketplace.Domain;

public class Product
{
    // 1. string — сокращённая (автоматическая) запись, обязательна при создании
    public required string Title { get; init; }

    // 2. string — полная запись с проверкой на пустоту
    private string article = string.Empty;
    public string Article
    {
        get => article;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Артикул обязателен", nameof(value));
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

**Требуется дополнительно:**
1. Свойство `Status` с `private set` (статус меняется только внутри класса).
2. Метод `ChangeStatus(string newStatus)` с **приватной** проверкой допустимых значений: `"В наличии"`, `"Нет в наличии"`, `"Под заказ"` (список допустимых значений храните в приватном статическом поле).
3. Убедиться, что `product.Price = -10;` и `product.Status = "Продан";` не компилируются, а `product.ChangeStatus("Продано");` выбрасывает исключение.

##### Участник 2 — класс `CustomerProfile` (`CustomerProfile.cs`)

**Требуется:**
1. `FullName` — автоматическое свойство, `required` + `init`.
2. `Phone` — автоматическое свойство с инициализатором `string.Empty`.
3. `Email` — **полная** запись: сеттер проверяет, что строка содержит `@` (приватный метод `EnsureValidEmail`).
4. `Rating` — `int`, полная запись с проверкой диапазона `0..5`, резервное поле `rating`.
5. Вычисляемое свойство `IsTrusted => Rating >= 4`.
6. Перегруженные методы:
	- `string GetInfo()`;
	- `string GetInfo(bool includeRating)`;
	- `string GetContactInfo(bool includePhone)`.
7. **1-2 приватных** метода (`EnsureValidEmail`, `NormalizePhone`).

```cs
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

##### Участник 3 — скидки и расчёт (`Discount.cs`)

**Требуется:**
1. Класс `Discount` со свойствами:
	- `Name` — сокращённая запись, `required`;
	- `Percent` — `decimal`, полная запись с проверкой `0..100`;
	- `MinSum` — `int`, полная запись с проверкой `>= 0` (минимальная сумма заказа);
	- `ValidUntil` — `DateTime` с `init`;
	- `IsActive` — вычисляемое `bool` (`ValidUntil > DateTime.Now`).
2. Метод `int Calculate(int total)` — цена после скидки с учётом минимальной суммы; внутри **1-2 приватных** метода (`ApplyDiscount`, `EnsurePercent`).
3. **Перегрузка** `int Calculate(int total, decimal percent)`.
4. Показать, что передача недопустимых значений через свойства не проходит.

```cs
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

    //TODO: ⚠️ private static int ApplyDiscount(int total, decimal percent) ⚠️
    //TODO: ⚠️ private static void EnsurePercent(decimal value) ⚠️
}
```

##### Участник 4 — позиция корзины и сервис оформления заказа (`CartItem.cs`, `CheckoutService.cs`)

**Требуется:**
1. `readonly struct CartItem` — позиция корзины со свойствами:
	- `ProductId`, `Quantity`, `PricePerUnit` — только для чтения, задаются конструктором;
	- `Total` — вычисляемое свойство (`PricePerUnit * Quantity`);
	- конструктор проверяет, что количество положительное, а цена не отрицательная.

> ℹ️ На занятии 2 позиция корзины была классом. Теперь оформляем её как `readonly struct`: данные позиции после создания не меняются, а стоимость считается свойством.

2. Сервисный класс `CheckoutService` без состояния (только методы):
	- `int GetTotal(IEnumerable<CartItem> items)` — сумма `Total` по всем позициям;
	- `IReadOnlyList<CartItem> FindCheaperThan(IEnumerable<CartItem> items, int maxPrice)` - найти позиции дешевле заданной цены;
	- перегруженные `string GetReport(IEnumerable<CartItem> items)` (короткая сводка) и `string GetReport(IEnumerable<CartItem> items, bool includeLines)` (сводка со строками позиций).
3. **1-2 приватных** метода: `IsAvailable(CartItem item)` и `PrepareLine(CartItem item)` - "доступна ли позиция для покупки?" и "Подготовить красивую строку для отчёта или чека на основе товарной позиции в корзине".
4. Сервис не должен обращаться к закрытым полям чужих классов — только к открытым свойствам и методам.

```cs
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

```cs
using Marketplace.Domain;

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

#### Компоновка (задача Лидера)

Лидер рабочей группы:
1. собирает все классы в один проект: по одному типу на файл, пространства имён `Marketplace.Domain` и `Marketplace.Services`;
2. пишет точку входа (с явно прописанным методом **Main** и служебными методами для каждого этапа демонстрации), где:
	- создаёт **3 товара** и **2 покупателя** (обязательные свойства заданы в инициализаторах);
	- собирает `List<CartItem>` и печатает **отчёты** через `CheckoutService`;
	- показывает работу **вычисляемого свойства** `IsBudget` и обоих вариантов `GetInfo`;
	- применяет **скидку** через `Discount.Calculate` и показывает **обе перегрузки**;
	- в `try/catch` демонстрирует попытку записать **отрицательную цену**, **пустой артикул** и **недопустимый рейтинг** (в catch блоке делаем вывод в консоль сообщения с описанием ситуации);
	- демонстрирует, что после создания объекта **нельзя изменить** `Title` (`init`) и `Status` (`private set`) - текст предупреждения от редактора кода записываем в отчёт - в листинге оставляем комментарий с кодом попытки вызвать сеттер и дополнительным комментарием "**Иллюстрация попытки вызвать сеттер для неизменяемого свойства**";
3. проверяет единообразие названий и стиля;
4. оформляет **отчёт**.

#### Рекомендации по выполнению

1. **Один класс — один файл**, имена свойств — существительные с большой буквы (`Price`, `IsTrusted`), имена методов — глаголы (`Calculate`, `GetReport`).
2. **Проверка в сеттере или в конструкторе?** Выберите один подход на группу и придерживайтесь его: либо сеттер бросает исключение, либо отклоняет значение. В отчёте объясните выбор.
3. **`required` + `init`** — хорошее сочетание для «паспортных» данных (`Title`, `FullName`), которые не должны меняться после создания.
4. **Вычисляемое свойство** не должно иметь сеттера и не должно быть «дорогим»: только простая формула по другим свойствам.
5. **Перегрузки** не должны дублировать код: короткая версия всегда вызывает полную.
6. **Каждый участник** пишет либо небольшой класс, либо как минимум один серьёзный публичный метод сервиса с 1-2 приватными методами внутри.

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
| `Program.cs` | `internal` | `Marketplace.Presentation` | точка входа: сценарии работы с профилем, успешной и неуспешной оплаты, проверка владельца корзины, работа каталога, свойства `Product`/`CustomerProfile`/`Discount`, `CartItem` и `CheckoutService`, сохранение/загрузка товаров и отчёты |
| `User.cs` | `public` | `Marketplace.Domain` | класс `User` — пользователь (ФИО, логин, пароль, уникальный ID) |
| `Contact.cs` | `public` | `Marketplace.Domain` | класс `Contact` — контакт пользователя (значение + ID канала) |
| `Channel.cs` | `public` | `Marketplace.Domain` | класс `Channel` — канал связи (телефон, email, telegram) |
| `UserProfile.cs` | `public` | `Marketplace.Domain` | класс `UserProfile` — профиль пользователя: контакты, денежный и бонусный балансы, персональная скидка, история покупок |
| `ProfileAdd.cs` | `public` | `Marketplace.Domain` | сервис `ProfileAdd` — создание профиля, контакты, вывод сводки и пополнение баланса |
| `Product.cs` | `public` | `Marketplace.Domain` | класс `Product` — товар: обязательное `Title`, `Article`, `Price`, вычисляемое `IsBudget`, `Status`; проверки в `init` и сеттерах |
| `Catalog.cs` | `public` | `Marketplace.Domain` | класс `Catalog` — каталог товаров: добавление, поиск, удаление и защита внутренней коллекции от изменения снаружи |
| `CartItem.cs` | `public` | `Marketplace.Domain` | `public readonly struct` `CartItem` — позиция корзины: `ProductId`, `Quantity`, `PricePerUnit`, вычисляемое `Total`; проверки в конструкторе; данные после создания не меняются |
| `ProductItem.cs` | `public` | `Marketplace.Domain` | класс `ProductItem` — сохранённая покупка: `ProductId`, `Quantity`, `PricePerUnit`, `PurchaseDate` и вычисляемое `Total` |
| `Cart.cs` | `public` | `Marketplace.Domain` | класс `Cart` — корзина пользователя (ID, ID пользователя, список позиций `CartItem`) |
| `CartPrice.cs` | `public` | `Marketplace.Domain` | структура `CartPrice` — расчётная цена корзины (исходная цена, множитель скидки, размер скидки) |
| `BuyerBalance.cs` | `public` | `Marketplace.Domain` | класс `BuyerBalance` — баланс покупателя (бонусы, деньги, доля оплаты бонусами) |
| `BuyerBalanceState.cs` | `public` | `Marketplace.Domain` | структура `BuyerBalanceState` — снимок состояния баланса покупателя |
| `PaymentService.cs` | `public` | `Marketplace.Domain` | сервис `PaymentService` — расчёт оплаты, списание бонусов и денег, сообщения об оплате, проверка владельца корзины |
| `Services/CheckoutService.cs` | `public` | `Marketplace.Services` | сервис `CheckoutService` — оформление заказа: `GetTotal`, `FindCheaperThan`, две перегрузки `GetReport`, приватные `IsAvailable` и `PrepareLine` |
| `CustomerProfile.cs` | `public` | `Marketplace.Domain` | класс `CustomerProfile` — профиль покупателя: ФИО, телефон, email, рейтинг, вычисляемое свойство `IsTrusted` и перегрузки `GetInfo` |
| `Discount.cs` | `public` | `Marketplace.Domain` | класс `Discount` — скидка: процент, минимальная сумма, срок действия и расчёт суммы со скидкой |
| `DataAccess/IProductRepository.cs` | `public` | `Marketplace.DataAccess` | интерфейс `IProductRepository` — абстракция хранения товаров с методами `Save` и `Load` |
| `DataAccess/FileProductRepository.cs` | `internal` | `Marketplace.DataAccess` | `internal class FileProductRepository` — файловое хранилище товаров в формате `id;title;price` |
| `Presentation/CatalogPrinter.cs` | `public`; `file` | `Marketplace.Presentation` | класс `CatalogPrinter` — печать всех товаров и топа по цене; в этом же файле находится `file static class PriceFormatter` |
| `Presentation/CatalogReport.cs` | `public` | `Marketplace.Presentation` | класс `CatalogReport` — расчёты средней цены и сводки по каталогу без печати и работы с файлами |

Модификатор `public` делает тип доступным из других сборок, `internal` — только внутри текущей сборки, `file` — только в пределах одного файла.

### Слои приложения

- `Marketplace.Domain` — доменные сущности и бизнес-правила: `Product`, `Catalog`, `Cart`, `CartItem`, `UserProfile` и другие.
- `Marketplace.Services` — сервисы приложения: `CheckoutService` считает суммы и печатает отчёты по корзине, не храня состояния.
- `Marketplace.DataAccess` — хранение товаров через `IProductRepository`; конкретная реализация — `FileProductRepository`.
- `Marketplace.Presentation` — точка входа `Program`, печать `CatalogPrinter` и отчёты `CatalogReport`.

`CatalogPrinter` отвечает только за вывод, `CatalogReport` — только за расчёты, а `Catalog` — только за работу с коллекцией товаров. Это разделение демонстрирует принцип единственной ответственности (SRP). Работа с хранилищем в `Program` идёт через интерфейс `IProductRepository`, то есть по принципу инверсии зависимостей (DIP).

## Таблица свойств проекта

В таблицу включены свойства, реализованные в задании 4; свойства `Cart`, `UserProfile` и других классов занятия 2–3 не перечислены.

| Свойство | Тип | Вид записи | Кто реализовал |
|---|---|---|---|
| `Product.Title` | `string` | авто, `required` + `init` | Мария Титова |
| `Product.Id` | `int` | авто, `init` | Мария Титова, добавлено при сведении в `main` |
| `Product.Article` | `string` | полная, проверка на пустоту | Мария Титова |
| `Product.Price` | `int` | полная, проверка `< 0` | Мария Титова |
| `Product.IsBudget` | `bool` | вычисляемая | Мария Титова |
| `Product.Status` | `string` | авто с `private set` | Мария Титова |
| `CustomerProfile.FullName` | `string` | авто, `required` + `init` | Роман Макаров |
| `CustomerProfile.Phone` | `string` | авто с инициализатором `string.Empty` | Роман Макаров |
| `CustomerProfile.Email` | `string` | полная, проверка `@` | Роман Макаров |
| `CustomerProfile.Rating` | `int` | полная, диапазон `0..5` | Роман Макаров |
| `CustomerProfile.IsTrusted` | `bool` | вычисляемая | Роман Макаров |
| `Discount.Name` | `string` | авто, `required` + `init` | Александр Лебедев |
| `Discount.ValidUntil` | `DateTime` | авто, `init` | Александр Лебедев |
| `Discount.Percent` | `decimal` | полная, диапазон `0..100` | Александр Лебедев |
| `Discount.MinSum` | `int` | полная, проверка `>= 0` | Александр Лебедев |
| `Discount.IsActive` | `bool` | вычисляемая | Александр Лебедев |
| `CartItem.ProductId` | `int` | только для чтения, задаётся конструктором | София Попова |
| `CartItem.Quantity` | `int` | только для чтения, конструктор проверяет `> 0` | София Попова |
| `CartItem.PricePerUnit` | `int` | только для чтения, конструктор проверяет `>= 0` | София Попова |
| `CartItem.Total` | `int` | вычисляемая | София Попова |
| `ProductItem.ProductId` | `int` | только для чтения, задаётся конструктором | София Попова |
| `ProductItem.Quantity` | `int` | только для чтения, конструктор проверяет `> 0` | София Попова |
| `ProductItem.PricePerUnit` | `int` | только для чтения, конструктор проверяет `>= 0` | София Попова |
| `ProductItem.PurchaseDate` | `DateTime` | только для чтения, задаётся конструктором | София Попова |
| `ProductItem.Total` | `int` | вычисляемая | София Попова |

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
14. Свойства `Product`: обязательное `Title`, `Article` с проверкой, `Price` с проверкой неотрицательности, вычисляемое `IsBudget`, `Status` с `private set`, метод `ChangeStatus()` и две перегрузки `GetInfo()`.
15. Обе перегрузки `GetInfo` для товара: `GetInfo()` выводит цену и признак бюджетного товара, `GetInfo(false)` — только название и артикул; короткая версия вызывает полную.
16. Двух покупателей `CustomerProfile`: обязательное `FullName` (`required` + `init`), email с проверкой символа `@`, рейтинг в диапазоне `0..5`, вычисляемое `IsTrusted`, перегрузки `GetInfo` и `GetContactInfo(true)`; попытки задать рейтинг `7` и email без `@` отклоняются исключениями.
17. Скидку `Discount`: `IsActive`, `Calculate(900)` (заказ меньше `MinSum` — скидка не применяется), `Calculate(2000)` с процентом из свойства и `Calculate(2000, 25m)` с явно заданным процентом; `Percent = 120` и `MinSum = -1` отклоняются проверками сеттеров.
18. `readonly struct CartItem`: позиции с `ProductId`, `Quantity`, `PricePerUnit` и вычисляемым `Total`; конструктор отклоняет количество `0` и отрицательную цену; свойство `Quantity` нельзя изменить после создания (`CS0200`).
19. `CheckoutService`: сумму `GetTotal`, поиск позиций дешевле заданной цены `FindCheaperThan`, короткую сводку `GetReport(items)` и подробную `GetReport(items, true)` со строками позиций.
20. Единый публичный контракт проекта: у `Product` нет дублирующих `GetId()`/`GetTitle()`/`GetPrice()`/`SetPrice()`, данные меняются только через свойства с проверками, а после успешной оплаты история профиля пополняется записями `ProductItem`.

## Доказательство инкапсуляции

Инкапсуляция — это сокрытие внутреннего состояния объекта и доступ к нему только через публичные свойства и методы, которые проверяют допустимость значений. В проекте это доказывают три вещи:

1. Внутренние поля закрыты (`private`), поэтому снаружи их нельзя изменить напрямую.
2. Изменение состояния идёт через публичный контракт: свойства `Id`, `Title`, `Article`, `Price`, `Status`, `IsBudget` и методы каталога.
3. Некорректные значения не проходят: компилятор запрещает обходить проверки, а проверки выбрасывают исключения.

| Проверка | Код, который не должен компилироваться | Ошибка компилятора |
|---|---|---|
| Закрытое поле `Product.price` | `product.price = -100;` | `CS0122: "Product.price" недоступен из-за его уровня защиты.` |
| `Title` задаётся только при создании | `product.Title = "Другой товар";` | `CS8852: свойство "Product.Title" можно назначить только в инициализаторе объекта или в аксессоре init.` |
| `Id` задаётся только при создании | `product.Id = 2;` | `CS8852: свойство "Product.Id" можно назначить только в инициализаторе объекта или в аксессоре init.` |
| `Status` меняется только через `ChangeStatus()` | `product.Status = "Продан";` | `CS0272: аксессор set свойства "Product.Status" недоступен.` |
| `CartItem` — `readonly struct`, данные задаёт конструктор | `item.Quantity = 5;` | `CS0200: свойство "CartItem.Quantity" доступно только для чтения.` |
| Закрытое поле `CustomerProfile.email` | `profile.email = "bad";` | `CS0122: "CustomerProfile.email" недоступен из-за его уровня защиты.` |
| Закрытая коллекция `Catalog.products` | `catalog.products.Add(product);` | `CS0122: "Catalog.products" недоступен из-за его уровня защиты.` |
| Коллекция отдаётся только для чтения | `catalog.GetProducts().Clear();` | `CS1061: "IReadOnlyList<Product>" не содержит определения "Clear".` |

Ошибка `CS1061` для `GetProducts().Clear()` воспроизводится отдельной сборкой:

```bash
dotnet build Rabota_OOP/Rabota_OOP.csproj -p:DefineConstants=NEGATIVE_CHECKS
```

Проверка цены находится в сеттере свойства:

```csharp
private int price;

public int Price
{
    get => price;
    set
    {
        if (value < 0)
            throw new ArgumentOutOfRangeException(
                nameof(value),
                "Цена товара не может быть отрицательной.");

        price = value;
    }
}
```

Проверка во время выполнения:

```csharp
try
{
    Product product = new Product
    {
        Id = 1,
        Title = "Кружка",
        Price = 100
    };

    product.Price = -100;
}
catch (ArgumentOutOfRangeException ex)
{
    Console.WriteLine(ex.Message);
}
```

Вывод:

```text
Цена товара не может быть отрицательной. (Parameter 'value')
```

`Status` также защищён: снаружи свойство доступно только для чтения, а изменить его можно лишь через `ChangeStatus()`, который проверяет допустимые значения. Попытка `product.ChangeStatus("Продано")` выбрасывает `ArgumentException`.

`Catalog` не отдаёт внутренний список: поле `products` закрыто, а `GetProducts()` возвращает `IReadOnlyList<Product>`, созданный через `products.ToList()`. Поэтому снаружи нельзя добавить товар в обход `Add()` и нельзя изменить внутреннюю коллекцию через возвращённое значение.

### Вывод программы

Фрагмент вывода, который подтверждает обе перегрузки `GetInfo` и отклонение недопустимых значений:

```text
Название: Блокнот, Артикул: ART-7, Цена: 20, Бюджетный: да
Название: Блокнот, Артикул: ART-7
Price = -100 отклонён: Цена товара не может быть отрицательной. (Parameter 'value')
Пустой артикул отклонён: Артикул обязателен. (Parameter 'value')
Рейтинг 7 отклонён: Рейтинг должен находиться в диапазоне от 0 до 5. (Parameter 'value')
Percent = 120 отклонён: Percent должен быть 0..100. (Parameter 'value')
MinSum = -1 отклонён: MinSum должен быть >= 0. (Parameter 'value')
```

Попытки изменить `Title` и `Status` после создания объекта не компилируются; сообщения компилятора `CS8852` и `CS0272` приведены в таблице выше.

## Распределение работы

С 11 ноября участники подготовили свои классы отдельными коммитами, после чего лидер группы свёл их в `main` и привёл проект к требованиям задания.

### Часть Ромы Макарова (участник 2)

11 ноября Роман добавил класс `CustomerProfile` (`CustomerProfile.cs`, коммит `3fed92e`):

- `FullName` — автоматическое свойство с `required` + `init`: имя покупателя обязательно и не меняется после создания;
- `Phone` — автосвойство с инициализатором `string.Empty`;
- `Email` — полная запись: сеттер вызывает приватную проверку `EnsureValidEmail` (строка должна содержать `@`), поэтому некорректный адрес не попадает в объект;
- `Rating` — `int`, полная запись с проверкой диапазона `0..5` и резервным полем;
- вычисляемое свойство `IsTrusted => Rating >= 4` не хранит отдельного значения;
- перегрузки `GetInfo()` и `GetInfo(bool includeRating)`, метод `GetContactInfo(bool includePhone)` и приватный `NormalizePhone`.

### Часть Саши Лебедева (участник 3)

11 ноября Александр добавил класс `Discount` (`Discount.cs`, коммит `ac78f5e`):

- `Name` — сокращённая запись, `required`;
- `Percent` — `decimal`, полная запись с проверкой `0..100`;
- `MinSum` — `int`, полная запись с проверкой `>= 0` (минимальная сумма заказа);
- `ValidUntil` — `DateTime` с `init` и вычисляемое `IsActive => ValidUntil > DateTime.Now`;
- `Calculate(int total)` и перегрузка `Calculate(int total, decimal percent)`; расчёт вынесен в приватные `ApplyDiscount` и `EnsurePercent`.

При сведении работы в `main` файл `Discount` (без расширения) был переименован в `Discount.cs`: без расширения он не попадал в сборку проекта.

### Часть Маши Титовой (участник 1)

11 ноября Мария прислала класс `Product` в ветке `Task1` (коммит `9f2b89d`): четыре вида свойств, две перегрузки `GetInfo` и смена статуса с проверкой допустимых значений.

13 ноября её часть доведена до листинга задания и перенесена в `main`:

- `Title` — обязательное автоматическое свойство (`required`, `init`), название задаётся при создании и не меняется;
- `Article` — полная запись с проверкой на пустоту;
- `Price` — `int`, полная запись с проверкой, что цена не отрицательна;
- `IsBudget` — вычисляемое свойство (`Price < 25`) без сеттера;
- `Status` — свойство с `private set`; допустимые значения хранятся в приватном статическом поле, менять статус можно только через `ChangeStatus()`;
- `GetInfo()` вызывает `GetInfo(true)`, а `GetInfo(bool includeIntProperty)` выводит цену и признак бюджетного товара только при `true`.

Старые методы `GetId()`, `GetTitle()`, `GetPrice()` и `SetPrice()` удалены, чтобы не дублировать публичный API: доступ к данным идёт только через свойства. Свойство `Id` (`init`) сохранено дополнительно — оно нужно каталогу, корзине и файловому хранилищу, хотя в листинге не показано.

### Часть Сони Поповой (участник 4, лидер группы)

13 ноября я отвечала за сведение работы в `main`:

- влила `Task1` в `main` и разрешила конфликт `Product`: версия из ветки не собиралась, поэтому за основу взята рабочая версия, а из `Task1` перенесены `Article`, `IsBudget`, `Status`, `ChangeStatus()` и `GetInfo(bool)`;
- исправила замечания преподавателя: единый публичный API без дублирования, разбор файла товаров по первому и последнему `;`, сценарии в `Program`, переименование `Discount` в `Discount.cs`;
- дополнила README таблицей структуры проекта, доказательством инкапсуляции, описанием демонстрации и выводами.

В рамках задачи участника 4 реализованы `CartItem` и `CheckoutService`:

- `CartItem` стал `readonly struct`: данные задаются конструктором, есть `ProductId`, `Quantity`, `PricePerUnit` и вычисляемое `Total`; конструктор отклоняет неположительное количество и отрицательную цену;
- `ProductItem` переработан в самостоятельный класс сохранённой покупки (`ProductId`, `Quantity`, `PricePerUnit`, `PurchaseDate`, `Total`), потому что структура не может быть базовым типом;
- `CheckoutService` (пространство имён `Marketplace.Services`) содержит `GetTotal`, `FindCheaperThan`, две перегрузки `GetReport` и приватные `IsAvailable` и `PrepareLine`; сервис не хранит состояния и работает только с открытыми свойствами `CartItem`;
- в `Program` добавлен сценарий, который собирает `List<CartItem>`, печатает оба отчёта, ищет позиции дешевле заданной цены и показывает исключения конструктора.

## Выводы

**Проверка в сеттере или в конструкторе.** Во всей группе выбран единый подход: проверка **бросает исключение** (`ArgumentException` или `ArgumentOutOfRangeException`) и не даёт сохранить недопустимое значение. Этот подход выбран потому, что он сразу сообщает об ошибке и не оставляет объект в «полувалидном» состоянии; молчаливое отклонение значения скрыло бы ошибку в вызывающем коде. В сеттерах проверяются `Price`, `Article`, `Email`, `Rating`, `Percent` и `MinSum`, а в конструкторе — параметры `CartItem`, потому что у `readonly struct` данные задаются только при создании.

**Когда свойство удобнее открытого поля.** Свойство сохраняет привычный синтаксис поля, но проверяет значение в момент записи: `Product.Price`, `Product.Article`, `CustomerProfile.Rating` и `CustomerProfile.Email` не принимают некорректные данные, а открытое поле приняло бы их молча. Свойство скрывает способ хранения: за `Price`, `Article` и `Rating` стоят закрытые резервные поля, поэтому внутреннее представление можно изменить, не затрагивая код снаружи. Вычисляемые свойства (`IsBudget`, `IsTrusted`, `CartItem.Total`, `Discount.IsActive`, `CartPrice.FinalPrice`) не хранят производные значения, поэтому данные не могут рассогласоваться. Связка `required` + `init` (`Product.Title`, `CustomerProfile.FullName`, `Discount.Name`) фиксирует «паспортные» данные после создания объекта, а `private set` (`Product.Status`) оставляет изменение состояния самому классу. Открытое поле уместно только для простого значения без проверок и вычислений, например во внутренней структуре данных; в публичном контракте этого проекта таких полей нет.

**Когда метод удобнее свойства.** Метод нужен, когда выполняется действие или алгоритм с параметрами и возможным побочным эффектом: `ChangeStatus()` проверяет список допустимых статусов, `Discount.Calculate()` считает сумму с учётом `MinSum`, `CheckoutService.GetTotal()`, `FindCheaperThan()` и `GetReport()` работают с коллекцией, `PaymentService.PayForPurchase()` меняет балансы и очищает корзину, `User.SetPassword()` хеширует пароль. Свойство должно быть быстрым, не принимать параметров и не иметь заметных побочных эффектов; всё остальное оформляется методом. Приватные проверки `EnsureStatus`, `EnsurePercent`, `EnsureValidEmail`, `IsAvailable` и `PrepareLine` — тоже методы, потому что это операции над данными, а не сами данные.

**Где перегрузка упростила вызов.** `GetInfo()` и `GetInfo(bool)`: короткая версия вызывает полную, поэтому вызывающий код не должен помнить разные имена методов. `Discount.Calculate(total)` и `Calculate(total, percent)`: одно действие либо с процентом из свойства, либо с явно заданным процентом. `CheckoutService.GetReport(items)` и `GetReport(items, includeLines)`: одна и та же сводка, при необходимости — со строками позиций. Перегрузки не дублируют код: короткая версия всегда делегирует полной, поэтому формат отчёта меняется в одном месте. При этом перегрузка по имени параметра или по типу возврата невозможна: тип возврата не входит в сигнатуру, и пара `GetInfo()`/`string GetInfo()` даёт ошибку `CS0111`.

**Структура или класс.** `CartItem` — `readonly struct`: позиция корзины является небольшим значением, которое после создания не меняется, поэтому `set` в ней запрещён, данные задаёт конструктор, а `Total` вычисляется. `Product`, `CustomerProfile`, `Cart` и `UserProfile` — классы: у них есть идентичность и изменяемое состояние, и на них ссылаются другие объекты.

**Итог.** В проекте присутствуют все четыре вида свойств из лабораторной: автосвойство (`Title`), полная запись с проверкой (`Price`, `Article`), вычисляемое свойство (`IsBudget`, `IsTrusted`, `Total`, `IsActive`) и свойство только для инициализации (`Id`, `ValidUntil`) вместе с обязательными (`required`) свойствами. Обе перегрузки `GetInfo` работают, недопустимые значения не попадают в объект, публичный API не дублируется, каждый публичный тип находится в отдельном файле (`PriceFormatter` — локальный помощник внутри `CatalogPrinter.cs`), а расчёты, печать и хранение разделены по слоям (`Domain`, `Services`, `DataAccess`, `Presentation`).

## Запуск

```bash
dotnet run --project Rabota_OOP/Rabota_OOP.csproj
```

Требуется .NET SDK 10.0 или новее.
