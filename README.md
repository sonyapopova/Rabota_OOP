# Rabota_OOP

## Занятие 5. Статические члены

## Учебная группа 251-337

Участники группы: Попова София Николаевна (лидер группы), Макаров Роман Сергеевич, Титова Мария Дмитриевна, Лебедев Александр Андреевич.

## Исходный текст задания

### Задание 3 (групповое)

#### Проект

Мы продолжаем проект маркетплейса (занятия 2–4). Каталог, свойства, перегрузки и позиции корзины уже готовы. На этом занятии добавляем общие данные, константы и неизменяемые значения.

**Правила проекта:**

1. Один тип — один файл; пространства имён `Marketplace.Domain` и `Marketplace.Services`.
2. Названия типов из занятий 2–4 не меняются: `Product`, `CustomerProfile`, `Cart`, `CartItem`, `CheckoutService`.
3. Новые типы: `PriceRules`, `Money`, `OrderNumberGenerator`, `ReceiptService`.
4. Имена статических членов — с большой буквы (`CreatedCount`, `MaxPrice`), закрытые поля — с маленькой (`nextId`).

#### Распределение задач по участникам

##### Участник 1 — класс `Product` (`Product.cs`) — главный по теме занятия

**Требуется:**

1. `MaxPrice` — `const int` со значением `1_000_000`.
2. Закрытое статическое поле `nextId` — общий счётчик товаров.
3. Статическое свойство `CreatedCount => nextId` только для чтения.
4. Свойство `Id` только для чтения, задаётся в конструкторе.
5. `readonly`-поле `createdAt` и свойство `CreatedAt`.
6. Статическое поле `CatalogStartedAt` (`static readonly DateTime`) и статический конструктор, который его заполняет.
7. Сохраните свойства занятия 4: `Title` (`required` + `init`), `Article`, `Price` с проверкой, `Status` с `private set`.

```csharp
namespace Marketplace.Domain;

public class Product
{
    public const int MaxPrice = 1_000_000;
    private static int nextId;
    public static int CreatedCount => nextId;
    public static readonly DateTime CatalogStartedAt;
    private readonly DateTime createdAt;
    public int Id { get; }
    public required string Title { get; init; }
    public string Article { get; set; } = string.Empty;
    private int price;
    public int Price
    {
        //TODO: ⚠️ get и set с проверкой value < 0 || value > MaxPrice ⚠️
    }
    public DateTime CreatedAt => createdAt;
    public string Status { get; private set; } = "В наличии";
    static Product()
    {
        //TODO: ⚠️ Задайте CatalogStartedAt (например, DateTime.UtcNow) ⚠️
    }
    public Product()
    {
        //TODO: ⚠️ Id = ++nextId; createdAt = DateTime.Now; ⚠️
    }
    //TODO: ⚠️ public void ChangeStatus(string newStatus) с приватной проверкой ⚠️
    //TODO: ⚠️ private static void EnsureStatus(string value) ⚠️
}
```

**Проверьте:**
1. Три товара получают `Id` 1, 2 и 3, `Product.CreatedCount` равно 3.
2. `Product.MaxPrice` доступно без объекта, а `product.MaxPrice` не компилируется.
3. `product.Id = 10;` и `product.CreatedAt = DateTime.Now;` не компилируются.
4. `Product.CatalogStartedAt` заполнено и не меняется.
5. `product.Price = -1;` и `product.Price = 2_000_000;` выбрасывают исключение.

##### Участник 2 — статический класс `PriceRules` (`PriceRules.cs`)

**Требуется:**

1. `const decimal VatRate = 0.2m`.
2. `const int MaxDiscountPercent = 70`.
3. `static readonly string DefaultCurrency = "RUB"`.
4. `static int WithVat(int price)` — цена с НДС.
5. `static int ApplyDiscount(int price, int percent)` — цена после скидки.
6. Приватные методы `EnsurePrice` и `EnsurePercent` с проверками.
7. Все методы статические; создать объект `PriceRules` нельзя.

```csharp
namespace Marketplace.Domain;

public static class PriceRules
{
    public const decimal VatRate = 0.2m;
    public const int MaxDiscountPercent = 70;
    public static readonly string DefaultCurrency = "RUB";

    public static int WithVat(int price)
    {
        //TODO: ⚠️ EnsurePrice(price); вернуть цену с НДС ⚠️
        return 0;
    }

    public static int ApplyDiscount(int price, int percent)
    {
        //TODO: ⚠️ Проверки и расчёт цены после скидки ⚠️
        return 0;
    }

    //TODO: ⚠️ private static void EnsurePrice(int price) ⚠️
    //TODO: ⚠️ private static void EnsurePercent(int percent) ⚠️
}
```

**Проверьте:**
1. `PriceRules.WithVat(1000)` возвращает `1200`.
2. `PriceRules.ApplyDiscount(1000, 10)` возвращает `900`.
3. `PriceRules.ApplyDiscount(1000, 80)` и `ApplyDiscount(-1, 10)` выбрасывают исключение.
4. `new PriceRules()` не компилируется.
5. `PriceRules.VatRate` и `PriceRules.DefaultCurrency` доступны через имя класса.

##### Участник 3 — `readonly struct Money` (`Money.cs`)

**Требуется:**

1. `readonly struct Money` со свойствами `Amount` (`decimal`) и `Currency` (`string`) только для чтения.
2. `const decimal MaxAmount = 10_000_000m`.
3. `static readonly Money Zero = new(0m, "RUB")`.
4. Конструктор с проверкой суммы и валюты.
5. Статический метод `FromRubles(decimal amount) => new(amount, "RUB")`.
6. Метод `Add(Money other)` с проверкой одинаковой валюты.
7. Приватный метод `EnsureAmount` и приватная проверка валюты.

```csharp
namespace Marketplace.Domain;

public readonly struct Money
{
    public const decimal MaxAmount = 10_000_000m;
    public static readonly Money Zero = new(0m, "RUB");
    public decimal Amount { get; }
    public string Currency { get; }

    public Money(decimal amount, string currency)
    {
        //TODO: ⚠️ Проверки и присваивание свойств ⚠️
    }

    public static Money FromRubles(decimal amount) => new(amount, "RUB");

    public Money Add(Money other)
    {
        //TODO: ⚠️ Проверить валюты и вернуть новую сумму ⚠️
        return Zero;
    }

    //TODO: ⚠️ private static void EnsureAmount(decimal amount) ⚠️
    //TODO: ⚠️ private static void EnsureCurrency(string currency) ⚠️
}
```

**Проверьте:**
1. `Money.Zero.Amount` равно `0`, а `Money.Zero.Currency` — `"RUB"`.
2. `Money.FromRubles(1000).Add(Money.FromRubles(2000))` даёт сумму `3000`.
3. `money.Amount = 500;` не компилируется.
4. `new Money(-1, "RUB")` и `new Money(1, "")` выбрасывают исключение.
5. Сложение рублей с долларами выбрасывает исключение.

##### Участник 4 — статические классы `OrderNumberGenerator` и `ReceiptService`

**Требуется:**

1. `static class OrderNumberGenerator`:
	- закрытое статическое поле `lastNumber`;
	- статический конструктор, который задаёт начальное значение `1000`;
	- статическое свойство `LastNumber` только для чтения;
	- статический метод `Next()`.
2. `static class ReceiptService`:
	- `static string BuildReceipt(IEnumerable<CartItem> items)` — номер заказа, строки позиций, итог с НДС;
	- приватные методы `PrepareLine(CartItem item)` и `FormatMoney(int amount)`;
	- использует `CheckoutService.GetTotal`, `PriceRules` и `OrderNumberGenerator`.
3. Оба класса не должны создаваться через `new`.

```csharp
namespace Marketplace.Services;

public static class OrderNumberGenerator
{
    private static int lastNumber;

    static OrderNumberGenerator()
    {
        //TODO: ⚠️ lastNumber = 1000; ⚠️
    }

    public static int LastNumber => lastNumber;

    public static int Next()
    {
        //TODO: ⚠️ Вернуть следующее значение счётчика ⚠️
        return 0;
    }
}
```

```csharp
using System.Text;
using Marketplace.Domain;

namespace Marketplace.Services;

public static class ReceiptService
{
    public static string BuildReceipt(IEnumerable<CartItem> items)
    {
        //TODO: ⚠️ Собрать чек: номер заказа, позиции, итог с НДС ⚠️
        return string.Empty;
    }

    //TODO: ⚠️ private static string PrepareLine(CartItem item) ⚠️
    //TODO: ⚠️ private static string FormatMoney(int amount) ⚠️
}
```

**Проверьте:**
1. Первый вызов `OrderNumberGenerator.Next()` возвращает `1001`, второй — `1002`.
2. `OrderNumberGenerator.LastNumber` не позволяет сбросить счётчик.
3. `new OrderNumberGenerator()` и `new ReceiptService()` не компилируются.
4. `ReceiptService.BuildReceipt` печатает номер заказа, позиции и итог с НДС.
5. В чеке используется `PriceRules.DefaultCurrency`.

#### Компоновка (задача Лидера)

**Лидер рабочей группы:**

1. собирает все классы в один проект: по одному типу на файл, пространства имён `Marketplace.Domain` и `Marketplace.Services`;
2. пишет точку входа, где:
	- создаёт 3 товара и показывает их уникальные `Id`, а также общее `Product.CreatedCount`;
	- печатает `Product.MaxPrice`, `Product.CatalogStartedAt`, `PriceRules.VatRate`, `PriceRules.DefaultCurrency`;
	- показывает, что `Money` — неизменяемая структура: создаёт `Money`, получает новую сумму через `Add`, а исходное значение не меняется;
	- демонстрирует работу `PriceRules.WithVat` и `PriceRules.ApplyDiscount`;
	- собирает `List<CartItem>` и печатает чек через `ReceiptService`;
	- в `try/catch` показывает попытку задать отрицательную цену, скидку больше `MaxDiscountPercent` и сложить разные валюты;
	- демонстрирует, что `Id`, `CreatedCount` и `Money.Amount` нельзя изменить;
3. проверяет единообразие названий и стиля;
4. оформляет отчёт.

#### Рекомендации по выполнению

1. Один тип — один файл, имена статических членов — с большой буквы (`CreatedCount`, `MaxPrice`), закрытые статические поля — с маленькой (`nextId`, `lastNumber`).
2. Счётчик держите закрытым. Наружу отдавайте только свойство только для чтения, иначе любой код сможет сбросить нумерацию.
3. `const` или `static readonly`? Если значение — простое число или строка, известная заранее, подойдёт `const`. Если значение вычисляется, зависит от времени или имеет сложный тип, используйте `static readonly`.
4. `readonly`-поле задавайте в конструкторе; не пытайтесь менять его позже.
5. `readonly struct` должна оставаться неизменяемой: все свойства `get`-only, изменение — только через создание новой копии.
6. Статический класс — это набор функций, а не объект: не добавляйте в него состояние, которое должно быть у каждого товара или покупателя.
7. Каждый участник пишет либо небольшой класс, либо как минимум один серьёзный публичный метод сервиса с 1-2 приватными методами внутри.

## Задачи были распределены между участниками следующим образом

Мария:
Задачи участника 1 — `Product` и статические члены

Роман:
Задачи участника 2 — статический класс `PriceRules`

Александр:
Задачи участника 3 — `readonly struct Money`

София:
Задачи участника 4 — `OrderNumberGenerator`, `ReceiptService` и компоновка проекта



## Структура проекта

| Файл | Модификатор | Пространство имён | Назначение |
|---|---|---|---|
| `Program.cs` | `internal` | `Marketplace.Presentation` | точка входа: статические члены `Product` и `PriceRules`, чек `ReceiptService`, сценарии профиля, оплаты, каталога, свойств, `CartItem` и `CheckoutService`, сохранение/загрузка товаров и отчёты |
| `User.cs` | `public` | `Marketplace.Domain` | класс `User` — пользователь (ФИО, логин, пароль, уникальный ID) |
| `Contact.cs` | `public` | `Marketplace.Domain` | класс `Contact` — контакт пользователя (значение + ID канала) |
| `Channel.cs` | `public` | `Marketplace.Domain` | класс `Channel` — канал связи (телефон, email, telegram) |
| `UserProfile.cs` | `public` | `Marketplace.Domain` | класс `UserProfile` — профиль пользователя: контакты, денежный и бонусный балансы, персональная скидка, история покупок |
| `ProfileAdd.cs` | `public` | `Marketplace.Domain` | сервис `ProfileAdd` — создание профиля, контакты, вывод сводки и пополнение баланса |
| `Product.cs` | `public` | `Marketplace.Domain` | класс `Product` — товар: `MaxPrice`, статический счётчик `nextId`, `CreatedCount`, `CatalogStartedAt`, `Id` и `CreatedAt` только для чтения; свойства `Title`, `Article`, `Price`, `IsBudget`, `Status` из занятия 4 |
| `Catalog.cs` | `public` | `Marketplace.Domain` | класс `Catalog` — каталог товаров: добавление, поиск, удаление и защита внутренней коллекции от изменения снаружи |
| `CartItem.cs` | `public` | `Marketplace.Domain` | `public readonly struct` `CartItem` — позиция корзины: `ProductId`, `Quantity`, `PricePerUnit`, вычисляемое `Total`; проверки в конструкторе; данные после создания не меняются |
| `ProductItem.cs` | `public` | `Marketplace.Domain` | класс `ProductItem` — сохранённая покупка: `ProductId`, `Quantity`, `PricePerUnit`, `PurchaseDate` и вычисляемое `Total` |
| `Cart.cs` | `public` | `Marketplace.Domain` | класс `Cart` — корзина пользователя (ID, ID пользователя, список позиций `CartItem`) |
| `CartPrice.cs` | `public` | `Marketplace.Domain` | структура `CartPrice` — расчётная цена корзины (исходная цена, множитель скидки, размер скидки) |
| `BuyerBalance.cs` | `public` | `Marketplace.Domain` | класс `BuyerBalance` — баланс покупателя (бонусы, деньги, доля оплаты бонусами) |
| `BuyerBalanceState.cs` | `public` | `Marketplace.Domain` | структура `BuyerBalanceState` — снимок состояния баланса покупателя |
| `PaymentService.cs` | `public` | `Marketplace.Domain` | сервис `PaymentService` — расчёт оплаты, списание бонусов и денег, сообщения об оплате, проверка владельца корзины |
| `PriceRules.cs` | `public static` | `Marketplace.Domain` | статический класс `PriceRules` — `VatRate`, `MaxDiscountPercent`, `DefaultCurrency`, расчёты `WithVat` и `ApplyDiscount` с приватными проверками |
| `Services/CheckoutService.cs` | `public` | `Marketplace.Services` | сервис `CheckoutService` — оформление заказа: `GetTotal`, `FindCheaperThan` по `Total`, две перегрузки `GetReport`, приватные `IsAvailable` и `PrepareLine`; отсекает `default(CartItem)` |
| `Services/OrderNumberGenerator.cs` | `public static` | `Marketplace.Services` | статический класс `OrderNumberGenerator` — закрытый счётчик `lastNumber`, статический конструктор, `LastNumber` и `Next()` |
| `Services/ReceiptService.cs` | `public static` | `Marketplace.Services` | статический класс `ReceiptService` — `BuildReceipt` собирает чек с номером заказа, позициями и итогом с НДС; приватные `PrepareLine` и `FormatMoney` |
| `CustomerProfile.cs` | `public` | `Marketplace.Domain` | класс `CustomerProfile` — профиль покупателя: ФИО, телефон, email, рейтинг, вычисляемое свойство `IsTrusted`, перегрузки `GetInfo` и нормализация телефона |
| `Discount.cs` | `public` | `Marketplace.Domain` | класс `Discount` — скидка: процент, минимальная сумма, срок действия; обе перегрузки `Calculate` учитывают `MinSum` и `IsActive` |
| `DataAccess/IProductRepository.cs` | `public` | `Marketplace.DataAccess` | интерфейс `IProductRepository` — абстракция хранения товаров с методами `Save` и `Load` |
| `DataAccess/FileProductRepository.cs` | `internal` | `Marketplace.DataAccess` | `internal class FileProductRepository` — файловое хранилище товаров в формате `id;title;price` |
| `Presentation/CatalogPrinter.cs` | `public`; `file` | `Marketplace.Presentation` | класс `CatalogPrinter` — печать всех товаров и топа по цене; в этом же файле находится `file static class PriceFormatter` |
| `Presentation/CatalogReport.cs` | `public` | `Marketplace.Presentation` | класс `CatalogReport` — расчёты средней цены и сводки по каталогу без печати и работы с файлами |

Модификатор `public` делает тип доступным из других сборок, `internal` — только внутри текущей сборки, `file` — только в пределах одного файла.

### Слои приложения

- `Marketplace.Domain` — доменные сущности и бизнес-правила: `Product`, `Catalog`, `Cart`, `CartItem`, `CustomerProfile`, `Discount`, `PriceRules` и другие.
- `Marketplace.Services` — сервисы приложения: `CheckoutService` считает суммы и печатает отчёты по корзине, `OrderNumberGenerator` выдаёт номера заказов, `ReceiptService` собирает чек; состояние хранится только в статическом счётчике номеров.
- `Marketplace.DataAccess` — хранение товаров через `IProductRepository`; конкретная реализация — `FileProductRepository`.
- `Marketplace.Presentation` — точка входа `Program`, печать `CatalogPrinter` и отчёты `CatalogReport`.

`CatalogPrinter` отвечает только за вывод, `CatalogReport` — только за расчёты, а `Catalog` — только за работу с коллекцией товаров. Это разделение демонстрирует принцип единственной ответственности (SRP). Работа с хранилищем в `Program` идёт через интерфейс `IProductRepository`, то есть по принципу инверсии зависимостей (DIP).

## Таблица свойств проекта

В таблицу включены свойства, реализованные в заданиях 4–5; свойства `Cart`, `UserProfile` и других классов занятия 2–3 не перечислены.

| Свойство | Тип | Вид записи | Кто реализовал |
|---|---|---|---|
| `Product.Title` | `string` | авто, `required` + `init` | Мария Титова |
| `Product.Id` | `int` | только для чтения, задаётся конструктором | Мария Титова |
| `Product.CreatedAt` | `DateTime` | только для чтения, `readonly`-поле | Мария Титова |
| `Product.Article` | `string` | полная, проверка на пустоту | Мария Титова |
| `Product.Price` | `int` | полная, проверка `0..MaxPrice` | Мария Титова |
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

## Доказательство инкапсуляции

В задании 5 инкапсуляция проверяется на общих данных и неизменяемых значениях:

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

### Вывод программы

Фрагмент вывода, который подтверждает статические члены, чек, исправленные перегрузки и проверки:

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
```

Попытки изменить `Product.Id`, `Product.CreatedAt`, `Product.CreatedCount`, `OrderNumberGenerator.LastNumber`, `const` `Product.MaxPrice` и `static readonly` `Product.CatalogStartedAt` не компилируются; коды ошибок `CS0200`, `CS0131`, `CS0198`, `CS0176`, `CS0122` и `CS0712` приведены в таблице выше.

## Распределение работы

Ниже — работа, выполненная в рамках задания 5. Я, как лидер группы, свела результаты в `main`, проверила сборку и оформила отчёт.

### Мария Титова (участник 1)

Мария подготовила в ветке `Task1` обновлённый класс `Product` со статическими членами:

- `MaxPrice` — `const int` с ограничением `1_000_000`;
- закрытый статический счётчик `nextId` и свойство `CreatedCount` только для чтения;
- `static readonly CatalogStartedAt`, который заполняется в статическом конструкторе;
- `readonly`-поле `createdAt` и свойство `CreatedAt`;
- `Id` только для чтения, задаётся в конструкторе;
- свойства занятия 4 сохранены: `Title` (`required` + `init`), `Article` с проверкой, `Price` с проверкой `0..MaxPrice`, `IsBudget`, `Status` с `private set` и `ChangeStatus()`.

Я влила `Task1` в `main`, сохранила рабочую версию свойств и добавила статические члены. Старые методы `GetId()`, `GetTitle()`, `GetPrice()` и `SetPrice()` удалены, чтобы не дублировать публичный API.

### Роман Макаров (участник 2)

Роман реализовал статический класс `PriceRules`:

- `const decimal VatRate = 0.2m`;
- `const int MaxDiscountPercent = 70`;
- `static readonly string DefaultCurrency = "RUB"`;
- статические методы `WithVat` и `ApplyDiscount`;
- приватные проверки `EnsurePrice` и `EnsurePercent`.

Класс полностью статический: создать его экземпляр через `new` нельзя, а все расчёты выполняются через имя класса.

### Александр Лебедев (участник 3)

Александр реализовал `readonly struct Money`:

- `MaxAmount` — `const decimal` с ограничением `10_000_000m`;
- `Zero` — `static readonly Money` со значением `0` в валюте `RUB`;
- `Amount` и `Currency` — свойства только для чтения, задаются в конструкторе;
- конструктор проверяет сумму и валюту через приватные `EnsureAmount` и `EnsureCurrency`;
- `FromRubles(decimal amount)` создаёт денежную сумму в рублях;
- `Add(Money other)` проверяет совпадение валют и возвращает новую сумму, не изменяя исходные значения.

Структура полностью неизменяемая: после создания `Money` нельзя изменить `Amount` или `Currency`, а операции возвращают новый экземпляр.

### София Попова (участник 4, лидер группы)

Я выполнила задачу участника 4 и свела работу группы:

- реализовала статические классы `OrderNumberGenerator` и `ReceiptService`: генератор выдаёт номера начиная с 1001, а чек содержит номер заказа, позиции и итог с НДС;
- добавила в `Program` сценарий статических членов, печать чека и проверки `PriceRules`;
- обновила `README`: исходный текст задания, структуру проекта, таблицы членов, доказательство инкапсуляции и выводы;
- исправила замечания преподавателя: `CustomerProfile.GetInfo()` вызывает полную перегрузку, `NormalizePhone` нормализует номер, обе перегрузки `Discount.Calculate` учитывают `IsActive` и `MinSum`, `FindCheaperThan` сравнивает `Total`, а `CheckoutService` отсекает `default(CartItem)`.

## Выводы

**Статический счётчик и уникальность.** Закрытое статическое поле `Product.nextId` и свойство `CreatedCount` позволили выдавать товарам уникальные `Id` без передачи счётчика извне. Три созданных подряд товара получили `Id` 1, 2 и 3, а `CreatedCount` показал общее количество созданных объектов. Если бы счётчик был открытым, любой код мог бы сбросить нумерацию и получить повторяющиеся идентификаторы, поэтому поле закрыто, а наружу отдано только значение для чтения.

**Константы и `static readonly`.** `Product.MaxPrice`, `PriceRules.VatRate` и `PriceRules.MaxDiscountPercent` удобно объявить через `const`: это простые значения, известные на этапе компиляции, которые не должны меняться. `Product.CatalogStartedAt` и `PriceRules.DefaultCurrency` зависят от времени или являются ссылочным типом, поэтому для них выбран `static readonly`: значение задаётся один раз и дальше доступно только для чтения. Попытки изменить такие члены не компилируются.

**Статический конструктор.** `Product.CatalogStartedAt` заполняется в статическом конструкторе. Это гарантирует, что общая дата старта каталога будет установлена до первого обращения к статическому члену, но не потребует создавать объект `Product`. Статический конструктор удобен именно для инициализации общих данных класса, а не для работы с конкретным товаром.

**Статические методы и статические классы.** `PriceRules` собрал общие правила расчёта НДС и скидки, а `OrderNumberGenerator` — выдачу номеров заказов. Оба класса не хранят данные конкретного объекта, поэтому их методы статические, а создавать экземпляры через `new` нельзя. `ReceiptService` использует эти правила, `CheckoutService` и `CartItem`, но сам остаётся stateless-сервисом: чек собирается из переданных позиций и не зависит от предыдущих вызовов.

**`readonly`-поля и неизменяемые значения.** Поле `Product.createdAt` объявлено как `readonly` и задаётся в конструкторе, а свойства `Id` и `CreatedAt` доступны только для чтения. Это не позволяет изменить «паспортные» данные товара после создания. Тот же принцип применён к `CartItem`: данные позиции задаются конструктором, а `Total` вычисляется. Для денежной суммы `readonly struct` был бы удобен по той же причине: две суммы с одинаковыми `Amount` и `Currency` логически равны, а операции должны возвращать новое значение, не меняя исходное.

**Опасность общего состояния.** Статические члены упрощают общие счётчики и правила, но изменяемое статическое состояние может стать опасным: два независимых сценария начнут влиять друг на друга, а тесты — зависеть от порядка запуска. Поэтому в проекте статические поля закрыты, счётчики доступны только для чтения, а изменяемое состояние конкретных объектов остаётся в экземплярах `Product`, `CustomerProfile` и `Cart`.

**Итог.** В проекте появились статическое поле, статическое свойство, статические методы, статический конструктор, статический класс, `const`, `static readonly` и `readonly`-поле. `Product` выдаёт уникальные `Id`, `PriceRules` централизует расчёты, `OrderNumberGenerator` и `ReceiptService` формируют чек, а закрытые поля и свойства только для чтения не позволяют обойти проверки. Сборка проходит без ошибок и предупреждений, программа демонстрирует уникальные идентификаторы, НДС, скидку и готовый чек.

## Запуск

```bash
dotnet run --project Rabota_OOP/Rabota_OOP.csproj
```

Требуется .NET SDK 10.0 или новее.
