# Rabota_OOP

## Занятие 6. Null, псевдонимы типов и наследование

## Учебная группа 251-337

Участники группы: Попова София Николаевна (лидер группы), Макаров Роман Сергеевич, Титова Мария Дмитриевна, Лебедев Александр Андреевич.

## Исходный текст задания

### Задание 3 (групповое)

#### Проект

Мы продолжаем проект маркетплейса (занятия 2–5). Каталог, свойства, статические члены и денежные суммы уже готовы. На этом занятии добавляем nullable-данные, наследование и удобные имена типов.

**Правила проекта:**

1. Один тип — один файл; пространства имён `Marketplace.Domain` и `Marketplace.Services`.
2. Названия типов из занятий 2–5 не меняются: `Product`, `CustomerProfile`, `Cart`, `CartItem`, `CheckoutService`, `PriceRules`, `Money`.
3. Новые типы: `DigitalProduct`, `PhysicalProduct`, `PremiumCustomer`, `ContactRules`, `OrderService`.
4. Nullable-свойства помечайте `?`; обязательные — оставляйте без `?` и проверяйте null guard.
5. В проекте включите nullable-контекст: `<Nullable>enable</Nullable>`.

#### Распределение задач по участникам

##### Участник 1 — базовый `Product` и наследник `DigitalProduct`

**Требуется:**

1. В `Product` добавьте:
   - псевдоним `using ProductId = System.Int32;`;
   - свойство `Id` типа `ProductId`;
   - nullable-свойство `Description`;
   - метод `GetDescription()`, возвращающий описание или `"описание не указано"`.
2. Создайте `DigitalProduct : Product`:
   - свойства `DownloadUrl` и `FileSizeMb`;
   - конструктор с null guard для `DownloadUrl` и проверкой размера файла;
   - метод `GetFullInfo()`, который вызывает `base.GetDescription()`.
3. Сохраните статический счётчик `Id` и `required Title` из занятия 5.

```csharp
using ProductId = System.Int32;

namespace Marketplace.Domain;

public class Product
{
    private static int nextId;
    public ProductId Id { get; }
    public required string Title { get; init; }
    public string? Description { get; set; }

    public Product()
    {
        //TODO: ⚠️ Id = ++nextId; ⚠️
    }

    public string GetDescription()
    {
        //TODO: ⚠️ Description ?? "описание не указано" ⚠️
        return string.Empty;
    }
}
```

```csharp
namespace Marketplace.Domain;

public class DigitalProduct : Product
{
    public string DownloadUrl { get; }
    public int FileSizeMb { get; }

    public DigitalProduct(string downloadUrl, int fileSizeMb)
    {
        //TODO: ⚠️ ArgumentNullException.ThrowIfNull(downloadUrl); ⚠️
        //TODO: ⚠️ Проверка fileSizeMb > 0 ⚠️
        DownloadUrl = downloadUrl;
        FileSizeMb = fileSizeMb;
    }

    public string GetFullInfo()
    {
        //TODO: ⚠️ base.GetDescription() + размер файла ⚠️
        return string.Empty;
    }
}
```

**Проверьте:**

1. `new DigitalProduct("url", 12) { Title = "Книга", Description = null }` компилируется.
2. `new DigitalProduct(null!, 12) { Title = "Книга" }` проходит компиляцию, но выбрасывает `ArgumentNullException`.
3. `GetDescription()` возвращает `"описание не указано"` при `Description == null`.
4. `GetFullInfo()` содержит описание базового класса.
5. `Id` унаследованного товара увеличивает общий `Product.CreatedCount`.

##### Участник 2 — наследник `PhysicalProduct`

**Требуется:**

1. `PhysicalProduct : Product` со свойствами:
   - `WeightKg` (`double`);
   - `WarehouseCode` (`string?`).
2. Конструктор с явным вызовом `: base()`:
   - null guard не нужен, но вес проверяется приватным методом;
   - `WarehouseCode` может быть `null`.
3. Вычисляемое свойство `DeliveryInfo => WarehouseCode ?? "склад не назначен"`.
4. Метод `GetFullInfo()` с `base.GetDescription()`.
5. Приватный метод `EnsureWeight`.

```csharp
namespace Marketplace.Domain;

public class PhysicalProduct : Product
{
    public double WeightKg { get; }
    public string? WarehouseCode { get; set; }

    public PhysicalProduct(double weightKg, string? warehouseCode) : base()
    {
        //TODO: ⚠️ EnsureWeight(weightKg); ⚠️
        WeightKg = weightKg;
        WarehouseCode = warehouseCode;
    }

    public string DeliveryInfo => WarehouseCode ?? "склад не назначен";

    public string GetFullInfo()
    {
        //TODO: ⚠️ Описание, вес и DeliveryInfo ⚠️
        return string.Empty;
    }

    //TODO: ⚠️ private static void EnsureWeight(double weightKg) ⚠️
}
```

**Проверьте:**

1. `new PhysicalProduct(1.5, null) { Title = "Стол" }` компилируется.
2. `DeliveryInfo` возвращает `"склад не назначен"` при `WarehouseCode == null`.
3. `new PhysicalProduct(-1, "A-1")` выбрасывает исключение.
4. `base.GetDescription()` используется в `GetFullInfo`.
5. Создание физического товара увеличивает общий счётчик `Product.CreatedCount`.

##### Участник 3 — nullable-профиль покупателя

**Требуется:**

1. `CustomerProfile`:
   - `FullName` — обязательное свойство;
   - `MiddleName`, `Email`, `Phone` — nullable;
   - `Rating` — `int` с проверкой `0..5`;
   - псевдоним `using CustomerId = System.Int32;` и свойство `Id`.
2. Статический класс `ContactRules`:
   - константа `UnknownEmail`;
   - метод `NormalizeEmail(string? value)`;
   - метод `LooksLikeEmail(string? value)`.
3. В `CustomerProfile` подключите `using static Marketplace.Domain.ContactRules;`.
4. Методы:
   - `GetDisplayName()` — `MiddleName ?? string.Empty`;
   - `GetEmail()` — `Email ?? UnknownEmail`;
   - `EnsureEmail()` — `Email ??= UnknownEmail`;
   - `TryNormalizeEmail()` — использует `NormalizeEmail`.
5. Приватный метод проверки рейтинга.

```csharp
namespace Marketplace.Domain;

public static class ContactRules
{
    public const string UnknownEmail = "почта не указана";

    public static string NormalizeEmail(string? value)
    {
        //TODO: ⚠️ Trim + нижний регистр; для null — UnknownEmail ⚠️
        return UnknownEmail;
    }

    public static bool LooksLikeEmail(string? value)
    {
        //TODO: ⚠️ value?.Contains('@') == true ⚠️
        return false;
    }
}
```

```csharp
using CustomerId = System.Int32;
using static Marketplace.Domain.ContactRules;

namespace Marketplace.Domain;

public class CustomerProfile
{
    public CustomerId Id { get; init; }
    public required string FullName { get; init; }
    public string? MiddleName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    private int rating;

    public int Rating
    {
        //TODO: ⚠️ Полная запись с проверкой 0..5 ⚠️
    }

    public string GetDisplayName()
    {
        //TODO: ⚠️ FullName + MiddleName ?? string.Empty ⚠️
        return string.Empty;
    }

    public string GetEmail()
    {
        //TODO: ⚠️ Email ?? UnknownEmail ⚠️
        return string.Empty;
    }

    public void EnsureEmail()
    {
        //TODO: ⚠️ Email ??= UnknownEmail; ⚠️
    }

    public string TryNormalizeEmail() => NormalizeEmail(Email);

    //TODO: ⚠️ private static void EnsureRating(int value) ⚠️
}
```

**Проверьте:**

1. `new CustomerProfile { FullName = "Анна", Email = null }` компилируется.
2. `GetEmail()` возвращает `"почта не указана"` при `Email == null`.
3. `EnsureEmail()` заполняет только пустую почту.
4. `TryNormalizeEmail()` приводит строку к нижнему регистру и обрезает пробелы.
5. `Rating = 7` выбрасывает исключение.
6. Псевдоним `CustomerId` и статический импорт `ContactRules` используются в коде.

##### Участник 4 — наследник `PremiumCustomer` и сервис `OrderService`

**Требуется:**

1. `PremiumCustomer : CustomerProfile`:
   - свойство `DiscountPercent` (`int`);
   - конструктор `: base()` с проверкой `0..50`;
   - метод `GetPremiumInfo()`, использующий `base.GetDisplayName()`.
2. `OrderService`:
   - `string BuildGreeting(CustomerProfile? customer)` — `customer.FullName` или `"гость"`;
   - `int GetDiscount(CustomerProfile? customer)` — `PremiumCustomer` через `is`;
   - `string GetEmail(CustomerProfile? customer)` — `customer?.Email ?? "почта не указана"`;
   - `int GetTotal(IEnumerable<CartItem>? items)` — null guard и `CheckoutService.GetTotal`;
   - `string FormatOrderNumber(OrderId id)` с псевдонимом `OrderId = System.Int32`;
   - приватный метод `EnsureItems`.
3. В сервисе используйте null guard и `?.`.

```csharp
namespace Marketplace.Domain;

public class PremiumCustomer : CustomerProfile
{
    public int DiscountPercent { get; }

    public PremiumCustomer(int discountPercent) : base()
    {
        //TODO: ⚠️ Проверка 0..50 ⚠️
        DiscountPercent = discountPercent;
    }

    public string GetPremiumInfo()
    {
        //TODO: ⚠️ base.GetDisplayName() + скидка ⚠️
        return string.Empty;
    }
}
```

```csharp
using OrderId = System.Int32;
using System.Collections.Generic;
using Marketplace.Domain;

namespace Marketplace.Services;

public class OrderService
{
    public string BuildGreeting(CustomerProfile? customer)
    {
        //TODO: ⚠️ customer?.FullName ?? "гость" ⚠️
        return string.Empty;
    }

    public int GetDiscount(CustomerProfile? customer)
    {
        //TODO: ⚠️ customer is PremiumCustomer premium ? premium.DiscountPercent : 0 ⚠️
        return 0;
    }

    public string GetEmail(CustomerProfile? customer)
    {
        //TODO: ⚠️ customer?.Email ?? "почта не указана" ⚠️
        return string.Empty;
    }

    public int GetTotal(IEnumerable<CartItem>? items)
    {
        //TODO: ⚠️ EnsureItems(items); вернуть сумму через CheckoutService ⚠️
        return 0;
    }

    public string FormatOrderNumber(OrderId id) => $"Заказ №{id}";

    //TODO: ⚠️ private static void EnsureItems(IEnumerable<CartItem>? items) ⚠️
}
```

**Проверьте:**

1. `new PremiumCustomer(10) { FullName = "Анна" }` компилируется.
2. `GetDiscount(vip)` возвращает `10`, `GetDiscount(null)` — `0`.
3. `GetEmail(null)` возвращает `"почта не указана"`.
4. `GetTotal(null)` выбрасывает `ArgumentNullException`.
5. `GetPremiumInfo()` использует метод базового класса.
6. Псевдоним `OrderId` используется в `FormatOrderNumber`.

#### Компоновка (задача Лидера)

**Лидер рабочей группы:**

1. собирает все классы в один проект: по одному типу на файл, пространства имён `Marketplace.Domain` и `Marketplace.Services`;
2. пишет точку входа, где:
   - создаёт базовый `Product`, `DigitalProduct` и `PhysicalProduct`, показывает общий `Product.CreatedCount` и уникальные `Id`;
   - демонстрирует `Description ?? "описание не указано"` и `WarehouseCode ?? "склад не назначен"`;
   - создаёт обычного и премиального покупателя, показывает `GetDisplayName`, `GetEmail`, `EnsureEmail` и `GetDiscount`;
   - передаёт `null` в `OrderService` и показывает безопасные результаты `?.` и `??`;
   - вызывает `GetTotal(null)` в `try/catch` и показывает `ArgumentNullException`;
   - демонстрирует порядок вызова конструкторов: временно добавляет `Console.WriteLine` в конструкторы `Product` и `DigitalProduct`;
   - показывает использование псевдонимов `ProductId`, `CustomerId`, `OrderId` и статического импорта `ContactRules`;
3. проверяет единообразие названий и стиля;
4. оформляет отчёт.

#### Рекомендации по выполнению

1. Один тип — один файл, nullable-свойства помечайте `?`, обязательные — нет.
2. Null guard — в начале метода. Для параметров используйте `ArgumentNullException.ThrowIfNull`.
3. Не подавляйте `!` без необходимости. Если значение может быть `null`, лучше проверить его или использовать `??`.
4. Наследник не должен ломать базовый класс. `DigitalProduct` и `PhysicalProduct` добавляют свои данные, но сохраняют общий `Id` и `Title`.
5. `required`-свойства базового класса задавайте в инициализаторе: `new DigitalProduct(...) { Title = "..." }`.
6. Псевдонимы используйте для смысла (`ProductId`), а не для сокрытия типов.
7. Каждый участник пишет либо небольшой класс, либо как минимум один серьёзный публичный метод сервиса с 1-2 приватными методами внутри.

## Задачи были распределены между участниками следующим образом

Мария:
Задачи участника 1 — `Product` и `DigitalProduct`

Роман:
Задачи участника 2 — `PhysicalProduct`

Александр:
Задачи участника 3 — nullable-профиль покупателя и `ContactRules`

София:
Задачи участника 4 — `PremiumCustomer`, `OrderService` и компоновка проекта

## Структура проекта

| Файл | Модификатор | Пространство имён | Назначение |
|---|---|---|---|
| `Program.cs` | `internal` | `Marketplace.Presentation` | точка входа: статические члены, чек `ReceiptService`, сценарии профиля и оплаты, каталога, свойств, `CartItem`/`CheckoutService`, сохранение товаров, отчёты и сценарий задания 6 с nullable-значениями и наследованием |
| `User.cs` | `public` | `Marketplace.Domain` | класс `User` — пользователь (ФИО, логин, пароль, уникальный ID) |
| `Contact.cs` | `public` | `Marketplace.Domain` | класс `Contact` — контакт пользователя (значение + ID канала) |
| `Channel.cs` | `public` | `Marketplace.Domain` | класс `Channel` — канал связи (телефон, email, telegram) |
| `UserProfile.cs` | `public` | `Marketplace.Domain` | класс `UserProfile` — профиль пользователя: контакты, денежный и бонусный балансы, персональная скидка, история покупок |
| `ProfileAdd.cs` | `public` | `Marketplace.Domain` | сервис `ProfileAdd` — создание профиля, контакты, вывод сводки и пополнение баланса |
| `Product.cs` | `public` | `Marketplace.Domain` | базовый класс `Product` — товар: псевдоним `ProductId`, nullable-свойство `Description`, `GetDescription()`, статический счётчик `nextId`, `CreatedCount`, `CatalogStartedAt`, `Id` и `CreatedAt` только для чтения, свойства занятия 4 |
| `DigitalProduct.cs` | `public` | `Marketplace.Domain` | наследник `DigitalProduct : Product` — `DownloadUrl`, `FileSizeMb`, null guard в конструкторе и `GetFullInfo()` с `base.GetDescription()` |
| `PhysicalProduct.cs` | `public` | `Marketplace.Domain` | наследник `PhysicalProduct : Product` — `WeightKg`, nullable `WarehouseCode`, `DeliveryInfo` через `??` и `GetFullInfo()` с `base.GetDescription()` |
| `Catalog.cs` | `public` | `Marketplace.Domain` | класс `Catalog` — каталог товаров: добавление, поиск, удаление и защита внутренней коллекции от изменения снаружи |
| `CartItem.cs` | `public` | `Marketplace.Domain` | `public readonly struct` `CartItem` — позиция корзины: `ProductId`, `Quantity`, `PricePerUnit`, вычисляемое `Total`; проверки в конструкторе; данные после создания не меняются |
| `ProductItem.cs` | `public` | `Marketplace.Domain` | класс `ProductItem` — сохранённая покупка: `ProductId`, `Quantity`, `PricePerUnit`, `PurchaseDate` и вычисляемое `Total` |
| `Cart.cs` | `public` | `Marketplace.Domain` | класс `Cart` — корзина пользователя (ID, ID пользователя, список позиций `CartItem`) |
| `CartPrice.cs` | `public` | `Marketplace.Domain` | структура `CartPrice` — расчётная цена корзины (исходная цена, множитель скидки, размер скидки) |
| `BuyerBalance.cs` | `public` | `Marketplace.Domain` | класс `BuyerBalance` — баланс покупателя (бонусы, деньги, доля оплаты бонусами) |
| `BuyerBalanceState.cs` | `public` | `Marketplace.Domain` | структура `BuyerBalanceState` — снимок состояния баланса покупателя |
| `PaymentService.cs` | `public` | `Marketplace.Domain` | сервис `PaymentService` — расчёт оплаты, списание бонусов и денег, сообщения об оплате, проверка владельца корзины |
| `PriceRules.cs` | `public static` | `Marketplace.Domain` | статический класс `PriceRules` — `VatRate`, `MaxDiscountPercent`, `DefaultCurrency`, расчёты `WithVat` и `ApplyDiscount` с приватными проверками |
| `Money.cs` | `public readonly struct` | `Marketplace.Domain` | неизменяемая денежная сумма: `MaxAmount`, `Zero`, `Amount`, `Currency`, `FromRubles()` и `Add()` с проверкой валюты |
| `Services/CheckoutService.cs` | `public` | `Marketplace.Services` | сервис `CheckoutService` — оформление заказа: `GetTotal`, `FindCheaperThan` по `Total`, две перегрузки `GetReport`, приватные `IsAvailable` и `PrepareLine`; отсекает `default(CartItem)` |
| `Services/OrderNumberGenerator.cs` | `public static` | `Marketplace.Services` | статический класс `OrderNumberGenerator` — закрытый счётчик `lastNumber`, статический конструктор, `LastNumber` и `Next()` |
| `Services/ReceiptService.cs` | `public static` | `Marketplace.Services` | статический класс `ReceiptService` — `BuildReceipt` собирает чек с номером заказа, позициями и итогом с НДС; приватные `PrepareLine` и `FormatMoney` |
| `Services/OrderService.cs` | `public` | `Marketplace.Services` | сервис `OrderService` — null-safe `BuildGreeting`, `GetDiscount`, `GetEmail`, `GetTotal` с `EnsureItems` и `FormatOrderNumber(OrderId)` |
| `CustomerProfile.cs` | `public` | `Marketplace.Domain` | класс `CustomerProfile` — профиль покупателя: nullable-поля `MiddleName`, `Email`, `Phone`, псевдоним `CustomerId`, рейтинг, `GetDisplayName()`, `GetEmail()`, `EnsureEmail()` и `TryNormalizeEmail()` |
| `ContactRules.cs` | `public static` | `Marketplace.Domain` | статические правила контактов: `UnknownEmail`, `NormalizeEmail()` и `LooksLikeEmail()` |
| `PremiumCustomer.cs` | `public` | `Marketplace.Domain` | наследник `PremiumCustomer : CustomerProfile` — `DiscountPercent` с проверкой `0..50` и `GetPremiumInfo()` через `base.GetDisplayName()` |
| `Discount.cs` | `public` | `Marketplace.Domain` | класс `Discount` — скидка: процент, минимальная сумма, срок действия; обе перегрузки `Calculate` учитывают `MinSum` и `IsActive` |
| `DataAccess/IProductRepository.cs` | `public` | `Marketplace.DataAccess` | интерфейс `IProductRepository` — абстракция хранения товаров с методами `Save` и `Load` |
| `DataAccess/FileProductRepository.cs` | `internal` | `Marketplace.DataAccess` | `internal class FileProductRepository` — файловое хранилище товаров в формате `id;title;price` |
| `Presentation/CatalogPrinter.cs` | `public`; `file` | `Marketplace.Presentation` | класс `CatalogPrinter` — печать всех товаров и топа по цене; в этом же файле находится `file static class PriceFormatter` |
| `Presentation/CatalogReport.cs` | `public` | `Marketplace.Presentation` | класс `CatalogReport` — расчёты средней цены и сводки по каталогу без печати и работы с файлами |

Модификатор `public` делает тип доступным из других сборок, `internal` — только внутри текущей сборки, `file` — только в пределах одного файла.

### Слои приложения

- `Marketplace.Domain` — доменные сущности и бизнес-правила: `Product`, `DigitalProduct`, `PhysicalProduct`, `Catalog`, `Cart`, `CartItem`, `CustomerProfile`, `ContactRules`, `PremiumCustomer`, `Discount`, `Money`, `PriceRules` и другие.
- `Marketplace.Services` — сервисы приложения: `CheckoutService` считает суммы, `OrderNumberGenerator` выдаёт номера, `ReceiptService` собирает чек, `OrderService` безопасно работает с nullable-покупателем и корзиной.
- `Marketplace.DataAccess` — хранение товаров через `IProductRepository`; конкретная реализация — `FileProductRepository`.
- `Marketplace.Presentation` — точка входа `Program`, печать `CatalogPrinter` и отчёты `CatalogReport`.

`CatalogPrinter` отвечает только за вывод, `CatalogReport` — только за расчёты, а `Catalog` — только за работу с коллекцией товаров. Это разделение демонстрирует принцип единственной ответственности (SRP). Работа с хранилищем в `Program` идёт через интерфейс `IProductRepository`, то есть по принципу инверсии зависимостей (DIP).

## Таблица свойств проекта

В таблицу включены свойства, реализованные в заданиях 4–6; свойства `Cart`, `UserProfile` и других классов занятия 2–3 не перечислены.

| Свойство | Тип | Вид записи | Кто реализовал |
|---|---|---|---|
| `Product.Title` | `string` | авто, `required` + `init` | Мария Титова |
| `Product.Id` | `ProductId` (`int`) | только для чтения, задаётся конструктором | Мария Титова |
| `Product.Description` | `string?` | авто, nullable | Мария Титова |
| `Product.CreatedAt` | `DateTime` | только для чтения, `readonly`-поле | Мария Титова |
| `Product.Article` | `string` | полная, проверка на пустоту | Мария Титова |
| `Product.Price` | `int` | полная, проверка `0..MaxPrice` | Мария Титова |
| `Product.IsBudget` | `bool` | вычисляемая | Мария Титова |
| `Product.Status` | `string` | авто с `private set` | Мария Титова |
| `DigitalProduct.DownloadUrl` | `string` | только для чтения, задаётся конструктором | Мария Титова |
| `DigitalProduct.FileSizeMb` | `int` | только для чтения, проверка в конструкторе | Мария Титова |
| `PhysicalProduct.WeightKg` | `double` | только для чтения, проверка `> 0` | Роман Макаров |
| `PhysicalProduct.WarehouseCode` | `string?` | авто, nullable | Роман Макаров |
| `PhysicalProduct.DeliveryInfo` | `string` | вычисляемая через `??` | Роман Макаров |
| `CustomerProfile.FullName` | `string` | авто, `required` + `init` | Роман Макаров |
| `CustomerProfile.Id` | `CustomerId` (`int`) | авто, `init` | Александр Лебедев |
| `CustomerProfile.MiddleName` | `string?` | авто, nullable | Александр Лебедев |
| `CustomerProfile.Phone` | `string?` | авто, nullable | Александр Лебедев |
| `CustomerProfile.Email` | `string?` | полная, допускает `null` и `UnknownEmail`, иначе проверка `@` | Александр Лебедев |
| `CustomerProfile.Rating` | `int` | полная, диапазон `0..5` | Роман Макаров |
| `CustomerProfile.IsTrusted` | `bool` | вычисляемая | Роман Макаров |
| `PremiumCustomer.DiscountPercent` | `int` | только для чтения, конструктор проверяет `0..50` | София Попова |
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

### Статические члены, константы и `readonly`-поля

| Член | Тип | Вид | Назначение | Кто реализовал |
|---|---|---|---|---|
| `Product.MaxPrice` | `int` | `const` | максимально допустимая цена | Мария Титова |
| `Product.nextId` | `int` | `private static` | общий счётчик созданных товаров | Мария Титова |
| `Product.CreatedCount` | `int` | `static`, только для чтения | количество созданных товаров | Мария Титова |
| `Product.CatalogStartedAt` | `DateTime` | `static readonly` | момент старта каталога | Мария Титова |
| `Product.createdAt` | `DateTime` | `private readonly` | время создания товара | Мария Титова |
| `Product.CreatedAt` | `DateTime` | `get`-only | доступ к времени создания | Мария Титова |
| `PriceRules.VatRate` | `decimal` | `const` | ставка НДС `0.2m` | Роман Макаров |
| `PriceRules.MaxDiscountPercent` | `int` | `const` | максимальная скидка `70` | Роман Макаров |
| `PriceRules.DefaultCurrency` | `string` | `static readonly` | валюта по умолчанию `"RUB"` | Роман Макаров |
| `ContactRules.UnknownEmail` | `string` | `const` | запасное значение email | Александр Лебедев |
| `ContactRules.NormalizeEmail()` | `string` | `static`-метод | нормализация email | Александр Лебедев |
| `ContactRules.LooksLikeEmail()` | `bool` | `static`-метод | проверка наличия `@` | Александр Лебедев |
| `OrderNumberGenerator.lastNumber` | `int` | `private static` | последний выданный номер заказа | София Попова |
| `OrderNumberGenerator.LastNumber` | `int` | `static`, только для чтения | чтение последнего номера | София Попова |
| `OrderNumberGenerator.Next()` | `int` | `static`-метод | выдать следующий номер | София Попова |
| `ReceiptService.BuildReceipt()` | `string` | `static`-метод | собрать чек с номером, позициями и НДС | София Попова |
| `PriceRules.WithVat()` | `int` | `static`-метод | цена с НДС | Роман Макаров |
| `PriceRules.ApplyDiscount()` | `int` | `static`-метод | цена после скидки | Роман Макаров |

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
14. Свойства `Product`: обязательное `Title`, `Article` с проверкой, `Price` с проверкой диапазона `0..MaxPrice`, вычисляемое `IsBudget`, `Status` с `private set`, метод `ChangeStatus()` и две перегрузки `GetInfo()`.
15. Обе перегрузки `GetInfo` для товара: `GetInfo()` выводит цену и признак бюджетного товара, `GetInfo(false)` — только название и артикул; короткая версия вызывает полную.
16. Двух покупателей `CustomerProfile`: обязательное `FullName` (`required` + `init`), email с проверкой символа `@`, рейтинг в диапазоне `0..5`, вычисляемое `IsTrusted`, перегрузки `GetInfo` и метод `GetContactInfo(true)`; `GetInfo()` выводит полную информацию с рейтингом, `GetInfo(false)` скрывает рейтинг, а телефон нормализуется до цифр с ведущим `+`.
17. Скидку `Discount`: `IsActive`, `Calculate(900)` (заказ меньше `MinSum` — скидка не применяется), `Calculate(2000)` с процентом из свойства и `Calculate(2000, 25m)` с явно заданным процентом; обе перегрузки учитывают `MinSum` и `IsActive`, поэтому у истёкшей скидки обе возвращают исходную сумму; `Percent = 120` и `MinSum = -1` отклоняются проверками сеттеров.
18. `readonly struct CartItem`: позиции с `ProductId`, `Quantity`, `PricePerUnit` и вычисляемым `Total`; конструктор отклоняет количество `0` и отрицательную цену; свойство `Quantity` нельзя изменить после создания (`CS0200`).
19. `CheckoutService`: сумму `GetTotal`, поиск позиций дешевле заданной цены `FindCheaperThan` (сравнивается стоимость позиции `Total`, а не цена единицы), короткую сводку `GetReport(items)` и подробную `GetReport(items, true)` со строками позиций; `default(CartItem)` отсекается проверкой `IsAvailable`.
20. Единый публичный контракт проекта: у `Product` нет дублирующих `GetId()`/`GetTitle()`/`GetPrice()`/`SetPrice()`, данные меняются только через свойства с проверками, а после успешной оплаты история профиля пополняется записями `ProductItem`.
21. Статические члены `Product`: `MaxPrice`, закрытый счётчик `nextId`, `CreatedCount`, `CatalogStartedAt`, `readonly`-поле `createdAt`, свойства `Id` и `CreatedAt` только для чтения; три товара получают `Id` 1, 2, 3, а `CreatedCount` равно 3.
22. Статический класс `PriceRules`: `WithVat(1000)` возвращает 1200, `ApplyDiscount(1000, 10)` — 900; отрицательная цена и скидка больше 70% отклоняются.
23. Статические классы `OrderNumberGenerator` и `ReceiptService`: первый выдаёт номера начиная с 1001, второй собирает чек с номером заказа, позициями, итогом и НДС, используя `CheckoutService`, `PriceRules` и `DefaultCurrency`.
24. Запрет на создание экземпляров статических классов и изменение `const`/`static readonly`/`readonly`-членов: соответствующие попытки не компилируются, а нарушения проверяются в таблице ниже.
25. Исправленные сценарии: `GetInfo()` выводит полную информацию, `GetInfo(false)` скрывает рейтинг, телефон нормализуется до цифр с ведущим `+`, обе перегрузки `Discount.Calculate` учитывают `MinSum` и `IsActive`, `FindCheaperThan` сравнивает `Total`, а `default(CartItem)` не попадает в сумму и отчёт.
26. Наследование `Product` → `PhysicalProduct`: `Description` остаётся `null`, `GetDescription()` возвращает запасной текст, `DeliveryInfo` использует `WarehouseCode ?? "Склад не назначен"`, а `GetFullInfo()` вызывает `base.GetDescription()`.
27. Наследник `PremiumCustomer : CustomerProfile`: конструктор с явным `base()` проверяет скидку в диапазоне `0..50`, а `GetPremiumInfo()` использует `base.GetDisplayName()`.
28. Null-safe сервис `OrderService`: `BuildGreeting(null)` возвращает «гость», `GetEmail(null)` — «почта не указана», `GetDiscount(premium)` читает `DiscountPercent` через сопоставление с образцом, а `FormatOrderNumber(42)` использует псевдоним `OrderId`.
29. Null guard в `OrderService.GetTotal`: вызов с `null` выбрасывает `ArgumentNullException`, а корректный список `CartItem` считается через `CheckoutService`.
30. Проверка `DigitalProduct`: null guard отклоняет `null` в `DownloadUrl`, а `!` в демонстрационном вызове показывает, что подавление nullable-предупреждения не отменяет проверку во время выполнения.
31. Псевдонимы типов и обращение к базовому классу: `ProductId` используется для `Product.Id`, `OrderId` — в `OrderService`, а `base.GetDescription()` и `base.GetDisplayName()` позволяют наследникам переиспользовать логику базовых классов.

## Доказательство инкапсуляции

В заданиях 5–6 инкапсуляция проверяется на общих данных, неизменяемых значениях, nullable-полях и наследовании:

1. Закрытые статические поля не доступны снаружи: `Product.nextId` хранит общий счётчик товаров, `OrderNumberGenerator.lastNumber` — последний номер заказа. Оба поля `private static`, поэтому изменить их можно только методами самих классов.
2. `const`-значения нельзя изменить после компиляции: `Product.MaxPrice`, `PriceRules.VatRate` и `PriceRules.MaxDiscountPercent` доступны только для чтения.
3. `static readonly`-поля задаются один раз: `Product.CatalogStartedAt` заполняется в статическом конструкторе, `PriceRules.DefaultCurrency` — в инициализаторе. Снаружи присвоить им новое значение нельзя.
4. Статические свойства отдают только чтение: `Product.CreatedCount` и `OrderNumberGenerator.LastNumber` не имеют сеттера, поэтому счётчики нельзя сбросить из вызывающего кода.
5. Свойства экземпляра, которые задаются один раз, защищены: `Product.Id` и `Product.CreatedAt` доступны только для чтения и устанавливаются конструктором.
6. Статические классы `PriceRules`, `OrderNumberGenerator` и `ReceiptService` нельзя создать через `new`: они существуют только как набор статических членов.
7. `Product.createdAt` — `readonly`-поле: оно задаётся в конструкторе и не меняется после создания товара.
8. Принцип `readonly struct` применён к `CartItem`: данные задаются конструктором, свойства только для чтения, а `Total` вычисляется.

| Проверка | Код, который не должен компилироваться | Ошибка компилятора |
|---|---|---|
| Закрытое статическое поле `Product.nextId` | `Product.nextId = 0;` | `CS0122: "Product.nextId" недоступен из-за его уровня защиты.` |
| Закрытое статическое поле `OrderNumberGenerator.lastNumber` | `OrderNumberGenerator.lastNumber = 0;` | `CS0122: "OrderNumberGenerator.lastNumber" недоступен из-за его уровня защиты.` |
| `const` нельзя изменить | `Product.MaxPrice = 2_000_000;` | `CS0131: левая часть выражения присваивания должна быть переменной, свойством или индексатором.` |
| `static readonly` нельзя изменить | `Product.CatalogStartedAt = DateTime.Now;` | `CS0198: присваивание значений доступному только для чтения статическому полю допускается только в статическом конструкторе и в инициализаторе переменных.` |
| `static readonly`-строку нельзя изменить | `PriceRules.DefaultCurrency = "USD";` | `CS0198: присваивание значений доступному только для чтения статическому полю допускается только в статическом конструкторе и в инициализаторе переменных.` |
| Статический счётчик только для чтения | `Product.CreatedCount = 0;` | `CS0200: свойство "Product.CreatedCount" доступно только для чтения.` |
| Номер заказа только для чтения | `OrderNumberGenerator.LastNumber = 0;` | `CS0200: свойство "OrderNumberGenerator.LastNumber" доступно только для чтения.` |
| `Id` задаётся только конструктором | `product.Id = 2;` | `CS0200: свойство "Product.Id" доступно только для чтения.` |
| `CreatedAt` задаётся только конструктором | `product.CreatedAt = DateTime.Now;` | `CS0200: свойство "Product.CreatedAt" доступно только для чтения.` |
| К статическому члену нельзя обращаться через объект | `int limit = product.MaxPrice;` | `CS0176: доступ к члену "Product.MaxPrice" через ссылку на экземпляр невозможен; укажите имя типа.` |
| Статический класс нельзя создать | `new PriceRules();` | `CS0712: нельзя создать экземпляр статического класса "PriceRules".` |
| Статический класс нельзя создать | `new OrderNumberGenerator();` | `CS0712: нельзя создать экземпляр статического класса "OrderNumberGenerator".` |
| Статический класс нельзя создать | `new ReceiptService();` | `CS0712: нельзя создать экземпляр статического класса "ReceiptService".` |
| `CartItem` — `readonly struct` | `item.Quantity = 5;` | `CS0200: свойство "CartItem.Quantity" доступно только для чтения.` |

Проверки в сеттерах и статических методах дополняют защиту:

- `Product.Price` принимает только значения `0..MaxPrice`;
- `Product.Article` не допускает пустую строку;
- `PriceRules.EnsurePrice` и `PriceRules.EnsurePercent` отклоняют отрицательную цену и скидку больше `MaxDiscountPercent`;
- `Product.ChangeStatus` проверяет статус по закрытому списку допустимых значений.

```csharp
private int price;

public int Price
{
    get => price;
    set
    {
        if (value < 0 || value > MaxPrice)
            throw new ArgumentOutOfRangeException(
                nameof(value),
                $"Цена должна быть от 0 до {MaxPrice}.");

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
Цена должна быть от 0 до 1000000. (Parameter 'value')
```

### Nullable, наследование и псевдонимы

В задании 6 к проверкам добавились nullable-состояния и повторное использование базового класса:

1. `Product.Description` объявлено как `string?`, поэтому отсутствие описания — допустимое состояние, а не ошибка. Метод `GetDescription()` возвращает `Description ?? "Описание не указано"`.
2. `PhysicalProduct.WarehouseCode` также nullable; `DeliveryInfo` использует `WarehouseCode ?? "Склад не назначен"` и не падает при отсутствии склада.
3. `DigitalProduct` защищает обязательный `DownloadUrl` через `ArgumentNullException.ThrowIfNull`, а размер файла проверяется в конструкторе.
4. `OrderService` принимает nullable-покупателя и nullable-список позиций: `customer?.FullName ?? "гость"`, `customer?.Email ?? "почта не указана"`, а `EnsureItems` не пропускает `null` в расчёт.
5. `PremiumCustomer` вызывает `base()` и приватную проверку скидки, а `GetPremiumInfo()` переиспользует `base.GetDisplayName()`. Это пример наследования без копирования логики базового класса.
6. `Product.Id` объявлен через псевдоним `ProductId`, номер заказа — через `OrderId`; псевдонимы делают сигнатуры понятнее, не создавая новый тип.

| Проверка | Код | Результат |
|---|---|---|
| Nullable-описание допустимо | `product.Description = null;` | компилируется, `GetDescription()` возвращает запасной текст |
| Null guard для URL | `new DigitalProduct(null!, 12) { Title = "Книга" }` | `ArgumentNullException` с параметром `downloadUrl` |
| Проверка веса | `new PhysicalProduct(0, "A-1") { Title = "Стол" }` | `ArgumentOutOfRangeException` с параметром `weightKg` |
| Проверка скидки | `new PremiumCustomer(60) { FullName = "Тест" }` | `ArgumentOutOfRangeException` с параметром `value` |
| Null guard для корзины | `orderService.GetTotal(null)` | `ArgumentNullException` с параметром `items` |
| Безопасный доступ | `orderService.GetEmail(null)` | `"почта не указана"` |
| Наследование | `premium.GetPremiumInfo()` | вызывает `base.GetDisplayName()` |
| Псевдонимы | `ProductId id = product.Id; OrderId orderId = 42;` | компилируется и читается как смысловые имена |

```csharp
public string BuildGreeting(CustomerProfile? customer) =>
    customer?.FullName ?? "гость";

public int GetTotal(IEnumerable<CartItem>? items)
{
    EnsureItems(items);
    return new CheckoutService().GetTotal(items!);
}

private static void EnsureItems(IEnumerable<CartItem>? items)
{
    ArgumentNullException.ThrowIfNull(items);
}
```

`!` в `GetTotal` стоит только после явной проверки `EnsureItems`: он сообщает компилятору, что `null` уже отсечён. Если убрать проверку, `!` лишь скроет предупреждение, но не защитит от `NullReferenceException`; поэтому null guard остаётся обязательным.

### Вывод программы

Фрагмент вывода, который подтверждает статические члены, наследование, nullable-сценарии и проверки:

```text
=== Задание 5: статические члены ===
Id товаров: 1, 2, 3
Product.CreatedCount: 3
Product.MaxPrice: 1000000
PriceRules.VatRate: 0,2
PriceRules.DefaultCurrency: RUB
WithVat(1000): 1200
ApplyDiscount(1000, 10): 900
Чек №1001
  Товар №1: 2 × 100 RUB = 200 RUB
  Товар №2: 1 × 200 RUB = 200 RUB
Итого с НДС: 480 RUB
OrderNumberGenerator.LastNumber: 1001
Отрицательная цена отклонена: Цена не может быть отрицательной. (Parameter 'price')
Скидка 80% отклонена: Скидка должна быть от 0 до 70%. (Parameter 'percent')
...
Название: Блокнот, Артикул: ART-7, Цена: 20, Бюджетный: да
Название: Блокнот, Артикул: ART-7
Price = -100 отклонён: Цена должна быть от 0 до 1000000. (Parameter 'value')
Пустой артикул отклонён: Артикул обязателен. (Parameter 'value')
Анна Петрова (anna@example.com) — Рейтинг: 5/5
Анна Петрова (anna@example.com)
Email: anna@example.com, Телефон: +79000000000
...
Неактивная скидка: Calculate(2000) = 2000, Calculate(2000, 25) = 2000
...
Позиций с общей стоимостью дешевле 500 котокоинов: 2
  Товар №2: 100 котокоинов за позицию
  Товар №3: 270 котокоинов за позицию
GetTotal с default(CartItem): 1000 котокоинов
...

=== Задание 6: null, наследование и псевдонимы ===
Product.CreatedCount: 19
Id: 18, 19
Описание: Описание не указано
Склад: Склад не назначен
Описание не указано.
 Вес: 12,5 кг.
 Склад: Склад не назначен.
Профиль: Анна
Приветствие: гость
Email: почта не указана
Скидка: 10
Premium: Иван — скидка 10%
Заказ №42
Сумма: 200
GetTotal(null): items
Скидка 60% отклонена: Скидка премиального покупателя должна быть от 0 до 50%. (Parameter 'value')
DigitalProduct(null): downloadUrl
```

Попытки изменить `Product.Id`, `Product.CreatedAt`, `Product.CreatedCount`, `OrderNumberGenerator.LastNumber`, `const` `Product.MaxPrice` и `static readonly` `Product.CatalogStartedAt` не компилируются; коды ошибок `CS0200`, `CS0131`, `CS0198`, `CS0176`, `CS0122` и `CS0712` приведены в таблице выше. Блок «Задание 6» подтверждает null guard, nullable-значения, наследование и работу псевдонимов.

## Распределение работы

Ниже — работа, выполненная в рамках задания 6. Я, как лидер группы, свела результаты в `main`, проверила сборку и оформила отчёт.

### Мария Титова (участник 1)

Мария обновила базовый `Product` и подготовила наследника `DigitalProduct`:

- добавила псевдоним `ProductId` для идентификатора товара;
- добавила nullable-свойство `Description` и метод `GetDescription()`;
- создала `DigitalProduct : Product` с `DownloadUrl`, `FileSizeMb`, null guard в конструкторе и `GetFullInfo()`, использующим `base.GetDescription()`.

Я влила изменения Марии из ветки `Member-1` в `main` и сохранила общий контракт `Product`.

### Роман Макаров (участник 2)

Роман реализовал наследника `PhysicalProduct : Product`:

- добавил свойства `WeightKg` и nullable `WarehouseCode`;
- реализовал `DeliveryInfo` через `??`;
- добавил приватную проверку веса `EnsureWeight`;
- реализовал `GetFullInfo()` с использованием `base.GetDescription()`.

### Александр Лебедев (участник 3)

Александр реализовал nullable-профиль покупателя и правила контактов:

- создал статический класс `ContactRules` с константой `UnknownEmail`, методами `NormalizeEmail` и `LooksLikeEmail`;
- перевёл `CustomerProfile` на nullable-поля `MiddleName`, `Email` и `Phone`;
- добавил псевдоним `CustomerId` и свойство `Id`;
- подключил `#nullable enable` и статический импорт `using static Marketplace.Domain.ContactRules`;
- реализовал `GetDisplayName()` с учётом `MiddleName`, `GetEmail()`, `EnsureEmail()` и `TryNormalizeEmail()`;
- выделил проверки рейтинга и email в приватные методы.

### София Попова (участник 4, лидер группы)

Я выполнила задачу участника 4 и свела работу группы:

- реализовала `PremiumCustomer : CustomerProfile` с проверкой скидки `0..50` и `GetPremiumInfo()`, использующим `base.GetDisplayName()`;
- добавила в `CustomerProfile` метод `GetDisplayName()` для интеграции наследника;
- реализовала `OrderService` с nullable-покупателем и nullable-списком позиций: `BuildGreeting`, `GetDiscount`, `GetEmail`, `GetTotal` с null guard и `FormatOrderNumber(OrderId)`;
- добавила в `Program` сценарий задания 6: nullable-описание, `PhysicalProduct`, `PremiumCustomer`, `OrderService`, проверки исключений и null guard;
- влила изменения Марии из `Member-1` в `main`;
- перенесла `ContactRules.cs` в папку проекта, чтобы файл участвовал в сборке;
- обновила `README`: исходный текст задания, структуру проекта, таблицу свойств, доказательство инкапсуляции, вывод программы, выводы и распределение работы.

## Выводы

**Nullable как часть контракта.** `Product.Description` и `PhysicalProduct.WarehouseCode` объявлены nullable, потому что отсутствие описания или склада — нормальное состояние предметной области. Если бы такие поля были обычными `string`, пришлось бы подставлять пустые строки и терять разницу между «значение не задано» и «значение пустое». Тип `string?` делает это различие явным.

**Null guard и операторы `?.`, `??`, `!`.** В `OrderService` nullable-покупатель и nullable-корзина сначала проверяются, а уже потом используются. `customer?.FullName ?? "гость"` и `customer?.Email ?? "почта не указана"` дают безопасный результат без ветвлений. `GetTotal(null)` выбрасывает `ArgumentNullException`, а `!` применяется только после `EnsureItems` — он не заменяет проверку, а сообщает компилятору, что `null` уже отсечён.

**Наследование и `base`.** `DigitalProduct` и `PhysicalProduct` добавляют свои данные, но сохраняют общий `Id`, `Title` и `GetDescription()` базового `Product`. Вызовы `base.GetDescription()` и `base.GetDisplayName()` позволяют наследникам переиспользовать логику, а не копировать её. `PremiumCustomer` аналогично расширяет `CustomerProfile`, добавляя скидку.

**Порядок конструкторов.** В производном классе сначала выполняется конструктор базового класса, затем — конструктор наследника. Поэтому `Product` успевает назначить `Id` и `CreatedAt`, а `PhysicalProduct` — проверить вес и сохранить `WarehouseCode`. Явный `: base()` в `PhysicalProduct` и `PremiumCustomer` фиксирует это намерение в коде.

**Псевдонимы типов.** `ProductId` и `OrderId` не создают новых типов, а дают смысловые имена для `int`. Это улучшает читаемость сигнатур, но не ослабляет проверки: `Id` по-прежнему доступен только для чтения, а `FormatOrderNumber` принимает именно номер заказа.

**Проверки и границы.** Null guard защищает от `NullReferenceException`, а конструкторы `DigitalProduct`, `PhysicalProduct` и `PremiumCustomer` проверяют допустимые значения. При этом nullable-поля не означают вседозволенность: `GetDescription()` и `DeliveryInfo` задают понятные запасные значения, а `OrderService` не пропускает `null` в расчёт суммы.

**Итог.** В проекте появились nullable-свойства, null guard, операторы `?.` и `??`, псевдонимы типов, наследование с `base` и явным вызовом конструктора базового класса. Сборка проходит без ошибок и предупреждений, а программа демонстрирует безопасную работу с `null`, наследование и единый контракт `Product`/`CustomerProfile`.

## Запуск

```bash
dotnet run --project Rabota_OOP/Rabota_OOP.csproj
```

Требуется .NET SDK 10.0 или новее.
