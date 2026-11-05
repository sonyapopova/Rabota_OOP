# Rabota_OOP

## Занятие 3

## Учебная группа 251-337

Участники группы: Попова София Николаевна, Макаров Роман Сергеевич, Титова Мария Дмитриевна, Лебедев Александр Андреевич.

Исходный текст задания: 
Мы продолжаем проект маркетплейса с занятия 2. В нём уже есть Product , CartItem ,
Cart , PriceCalculation и OrderService (или аналог). На этом занятии добавляем
каталог товаров, инкапсуляцию и слой доступа к данным.
Т.е. делаем хранилище товаров и инструменты просмотра и изменения состава
товаров в этом хранилище.
Правила проекта:

Как распределены типы проекта:

Тип Пространство имён Модификатор Почему
Product Marketplace.Domain public сущность нужн
и приложению,
слою хранения
Catalog Marketplace.Domain public публичный API
каталога, но
коллекция
внутри закрыта
IProductRepository Marketplace.DataAccess public абстракция, от
которой завися
остальные (DIP
FileProductRepository Marketplace.DataAccess internal деталь
реализации,
снаружи сборки
не нужна
5. Из другого файла тип с модификатором file не виден, поэтому наследник не
скомпилируется: компилятор сообщит, что такой тип не найден.
6. DIP: зависимость идёт от конкретного класса, а не от интерфейса
IProductRepository .
7. SRP: у класса несколько причин для изменения — данные, хранение и вывод.

1. Пространства имён фиксированы: Marketplace.Domain (сущности),
Marketplace.DataAccess (хранение), Marketplace.Presentation (вывод).
2. Один тип — один файл.
3. Снаружи типов — только то, что действительно является контрактом; всё остальное
— private , internal или file .

Тип Пространство имён Модификатор Почему
CatalogPrinter Marketplace.Presentation public точка вывода
для приложени
PriceFormatter Marketplace.Presentation file вспомогательны
тип одного фай

Распределение задач по участникам
Участник 1 — сущность Product ( Product.cs )
Требуется: товар из занятия 2 приводим к «взрослому» виду — поля закрываем,
проверки собираем в одном месте.
1. Все поля — закрытые: идентификатор, название, цена за единицу.
2. Конструктор проверяет данные и бросает исключение при недопустимых значениях:
идентификатор больше нуля;
название не пустое;
цена не отрицательная.
3. Открытые методы: GetId() , GetTitle() , GetPrice() , SetPrice(int) , GetInfo() .
4. 1-2 приватных метода проверки, например EnsureTitle(string title) и
EnsurePrice(int price) .

namespace ;
public class Product
{
//TODO: ⚠️ Закрытые поля: id, title, price ⚠️
public Product(int id, string title, int price)
{
//TODO: ⚠️ Проверки и присвоение полей ⚠️
}
//TODO: ⚠️ public int GetId() ⚠️
//TODO: ⚠️ public string GetTitle() ⚠️
//TODO: ⚠️ public int GetPrice() ⚠️
//TODO: ⚠️ public void SetPrice(int newPrice) с проверкой ⚠️
//TODO: ⚠️ public string GetInfo() — одна строка о товаре ⚠️
//TODO: ⚠️ private static void EnsureTitle(string value) ⚠️
//TODO: ⚠️ private static void EnsurePrice(int value) ⚠️
}

Marketplace.Domain

Что показать в отчёте: product.price = -100; не компилируется, а
product.SetPrice(-100) выбрасывает исключение.

Участник 2 — агрегирующий класс Catalog ( Catalog.cs )
Требуется:

Что показать в отчёте: catalog.GetProducts().Clear(); не компилируется — метода
Clear у IReadOnlyList<T> нет.
1. Закрытая коллекция товаров: private readonly List<Product> products = new();
2. Открытые методы:
Add(Product product) — не принимать null и не дублировать товары
(сравнение по названию без учёта регистра и пробелов по краям);
Remove(string title) — возвращать bool ;
Find(string title) — возвращать Product? ;
GetProducts() — возвращать копию коллекции как IReadOnlyList<Product> ;
GetCount() — количество товаров.
3. 1-2 приватных метода: Normalize(string title) , SameTitle(string left, string
right) .

namespace ;
public class Catalog
{
private readonly List<Product> products = new();
public void Add(Product product)
{
//TODO: ⚠️ Проверить product на null и на дубликат по названию ⚠️
}
//TODO: ⚠️ public bool Remove(string title) ⚠️
//TODO: ⚠️ public Product? Find(string title) ⚠️
// Возвращаем копию: снаружи нельзя изменить внутренний список
public IReadOnlyList<Product> GetProducts() => products.ToList();
//TODO: ⚠️ public int GetCount() ⚠️
//TODO: ⚠️ private static string Normalize(string title) ⚠️
//TODO: ⚠️ private static bool SameTitle(string left, string right) ⚠️
}

Marketplace.Domain

Участник 3 — доступ к данным: пространство имён
Marketplace.DataAccess ( DataAccess/ )
Требуется:
1. Объявить открытый интерфейс (DIP):

using ;
namespace ;
public interface IProductRepository
{
void Save(IEnumerable<Product> products);
List<Product> Load();
}
Marketplace.Domain Marketplace.DataAccess

2. Реализовать его в внутреннем классе — деталь реализации не обязана быть видна
снаружи сборки (пример реализации репозитория на базе файла - см. в конце этих
материалов):

using ;
namespace ;
internal class FileProductRepository : IProductRepository
{
private readonly string filePath;
public FileProductRepository(string filePath) => this.filePath =
filePath;
public void Save(IEnumerable<Product> products)
{
//TODO: ⚠️ Сохранить каждый товар строкой в файл ⚠️
//TODO: ⚠️ Использовать ПРИВАТНЫЙ метод Serialize ⚠️
}
public List<Product> Load()
{
//TODO: ⚠️ Прочитать строки файла и собрать List<Product> ⚠️
//TODO: ⚠️ При отсутствии файла вернуть пустой список ⚠️
//TODO: ⚠️ Использовать ПРИВАТНЫЙ метод Deserialize ⚠️
return new List<Product>();
Marketplace.Domain Marketplace.DataAccess

Участник 4 — вывод и отчёты (SRP)
Требуется:
}
//TODO: ⚠️ private static string Serialize(Product product) ⚠️
//TODO: ⚠️ private static Product Deserialize(string line) ⚠️
}

3. Формат хранения выберите сами (например, 1;Кружка;450 — по строке на товар) и
опишите его в комментарии к классу.
4. Не забудьте using Marketplace.Domain; — типы слоя хранения берутся из домена.

1. Класс CatalogPrinter ( CatalogPrinter.cs ) — только печать:
PrintAll(IEnumerable<Product> products) , PrintTop(IEnumerable<Product>
products, int count) ; внутри 1-2 приватных метода подготовки строки.
2. В том же файле — file static class PriceFormatter с методом Format(int
price) (демонстрация файла как области видимости).
3. Отдельный класс CatalogReport ( CatalogReport.cs ) — только расчёты:
GetAveragePrice(IEnumerable<Product> products) и
GetSummary(IEnumerable<Product> products) ; ни печати, ни работы с файлами.
4. Убедиться, что ни Product , ни Catalog не занимаются печатью — это и есть SRP.

using ;
namespace ;
public class CatalogPrinter
{
public void PrintAll(IEnumerable<Product> products)
{
//TODO: ⚠️ Напечатать все товары через PrepareLine ⚠️
}
public void PrintTop(IEnumerable<Product> products, int count)
{
//TODO: ⚠️ Напечатать count самых дорогих товаров ⚠️
}
//TODO: ⚠️ private string PrepareLine(Product product) ⚠️
}
// Виден только в этом файле Marketplace.Domain Marketplace.Presentation

Вопрос для отчёта: почему PriceFormatter не виден в файле CatalogReport.cs и что
пришлось бы сделать, если бы он понадобился в двух файлах?
Ответ: модификатор file ограничивает тип одним файлом; для двух файлов нужен
internal (внутри сборки) или public (для всех).
Компоновка (задача Лидера)
Лидер рабочей группы:

Пример точки входа для компоновки:
file static class PriceFormatter
{
public static string Format(int price) => $"{price} у.е.";
}

1. собирает проект: по одному типу на файл, пространства имён Marketplace.Domain ,
Marketplace.DataAccess , Marketplace.Presentation ;
2. пишет точку входа, где:
создаёт 3-4 товара, в том числе с недопустимыми данными (в try/catch ) —
чтобы показать защиту целостности;
добавляет их в Catalog , ищет, удаляет, выводит;
сохраняет и загружает товары через IProductRepository через интерфейс (DIP);
печатает отчёт через CatalogPrinter и CatalogReport ;
3. проверяет, что изменить коллекцию каталога снаружи нельзя;
4. оформляет отчёт.

## Задачи были распределены между участниками следующим образом

Задача 3 (Мария):
Участник 3 — доступ к данным: пространство имён
Marketplace.DataAccess ( DataAccess/ )
Требуется:
1. Объявить открытый интерфейс (DIP):

using ;
namespace ;
public interface IProductRepository
{
void Save(IEnumerable<Product> products);
List<Product> Load();
}
Marketplace.Domain Marketplace.DataAccess

2. Реализовать его в внутреннем классе — деталь реализации не обязана быть видна
снаружи сборки (пример реализации репозитория на базе файла - см. в конце этих
материалов):

using ;
namespace ;
internal class FileProductRepository : IProductRepository
{
private readonly string filePath;
public FileProductRepository(string filePath) => this.filePath =
filePath;
public void Save(IEnumerable<Product> products)
{
//TODO: ⚠️ Сохранить каждый товар строкой в файл ⚠️
//TODO: ⚠️ Использовать ПРИВАТНЫЙ метод Serialize ⚠️
}
public List<Product> Load()
{
//TODO: ⚠️ Прочитать строки файла и собрать List<Product> ⚠️
//TODO: ⚠️ При отсутствии файла вернуть пустой список ⚠️
//TODO: ⚠️ Использовать ПРИВАТНЫЙ метод Deserialize ⚠️
return new List<Product>();
Marketplace.Domain Marketplace.DataAccess

3. Формат хранения выберите сами (например, 1;Кружка;450 — по строке на товар) и
опишите его в комментарии к классу.
4. Не забудьте using Marketplace.Domain; — типы слоя хранения берутся из домена.

Задача 1(Роман):
Участник 1 — сущность Product ( Product.cs )
Требуется: товар из занятия 2 приводим к «взрослому» виду — поля закрываем,
проверки собираем в одном месте.
1. Все поля — закрытые: идентификатор, название, цена за единицу.
2. Конструктор проверяет данные и бросает исключение при недопустимых значениях:
идентификатор больше нуля;
название не пустое;
цена не отрицательная.
3. Открытые методы: GetId() , GetTitle() , GetPrice() , SetPrice(int) , GetInfo() .
4. 1-2 приватных метода проверки, например EnsureTitle(string title) и
EnsurePrice(int price) .

namespace ;
public class Product
{
//TODO: ⚠️ Закрытые поля: id, title, price ⚠️
public Product(int id, string title, int price)
{
//TODO: ⚠️ Проверки и присвоение полей ⚠️
}
//TODO: ⚠️ public int GetId() ⚠️
//TODO: ⚠️ public string GetTitle() ⚠️
//TODO: ⚠️ public int GetPrice() ⚠️
//TODO: ⚠️ public void SetPrice(int newPrice) с проверкой ⚠️
//TODO: ⚠️ public string GetInfo() — одна строка о товаре ⚠️
//TODO: ⚠️ private static void EnsureTitle(string value) ⚠️
//TODO: ⚠️ private static void EnsurePrice(int value) ⚠️
}

Marketplace.Domain

Что показать в отчёте: product.price = -100; не компилируется, а
product.SetPrice(-100) выбрасывает исключение.

Задача 2 (Александр):
Участник 2 — агрегирующий класс Catalog ( Catalog.cs )
Требуется:

Что показать в отчёте: catalog.GetProducts().Clear(); не компилируется — метода
Clear у IReadOnlyList<T> нет.
1. Закрытая коллекция товаров: private readonly List<Product> products = new();
2. Открытые методы:
Add(Product product) — не принимать null и не дублировать товары
(сравнение по названию без учёта регистра и пробелов по краям);
Remove(string title) — возвращать bool ;
Find(string title) — возвращать Product? ;
GetProducts() — возвращать копию коллекции как IReadOnlyList<Product> ;
GetCount() — количество товаров.
3. 1-2 приватных метода: Normalize(string title) , SameTitle(string left, string
right) .

namespace ;
public class Catalog
{
private readonly List<Product> products = new();
public void Add(Product product)
{
//TODO: ⚠️ Проверить product на null и на дубликат по названию ⚠️
}
//TODO: ⚠️ public bool Remove(string title) ⚠️
//TODO: ⚠️ public Product? Find(string title) ⚠️
// Возвращаем копию: снаружи нельзя изменить внутренний список
public IReadOnlyList<Product> GetProducts() => products.ToList();
//TODO: ⚠️ public int GetCount() ⚠️
//TODO: ⚠️ private static string Normalize(string title) ⚠️
//TODO: ⚠️ private static bool SameTitle(string left, string right) ⚠️
}

Marketplace.Domain

Задача 4 (София):
Участник 4 — вывод и отчёты (SRP)
Требуется:
}
//TODO: ⚠️ private static string Serialize(Product product) ⚠️
//TODO: ⚠️ private static Product Deserialize(string line) ⚠️
}

3. Формат хранения выберите сами (например, 1;Кружка;450 — по строке на товар) и
опишите его в комментарии к классу.
4. Не забудьте using Marketplace.Domain; — типы слоя хранения берутся из домена.

1. Класс CatalogPrinter ( CatalogPrinter.cs ) — только печать:
PrintAll(IEnumerable<Product> products) , PrintTop(IEnumerable<Product>
products, int count) ; внутри 1-2 приватных метода подготовки строки.
2. В том же файле — file static class PriceFormatter с методом Format(int
price) (демонстрация файла как области видимости).
3. Отдельный класс CatalogReport ( CatalogReport.cs ) — только расчёты:
GetAveragePrice(IEnumerable<Product> products) и
GetSummary(IEnumerable<Product> products) ; ни печати, ни работы с файлами.
4. Убедиться, что ни Product , ни Catalog не занимаются печатью — это и есть SRP.

using ;
namespace ;
public class CatalogPrinter
{
public void PrintAll(IEnumerable<Product> products)
{
//TODO: ⚠️ Напечатать все товары через PrepareLine ⚠️
}
public void PrintTop(IEnumerable<Product> products, int count)
{
//TODO: ⚠️ Напечатать count самых дорогих товаров ⚠️
}
//TODO: ⚠️ private string PrepareLine(Product product) ⚠️
}
// Виден только в этом файле Marketplace.Domain Marketplace.Presentation

Вопрос для отчёта: почему PriceFormatter не виден в файле CatalogReport.cs и что
пришлось бы сделать, если бы он понадобился в двух файлах?
Ответ: модификатор file ограничивает тип одним файлом; для двух файлов нужен
internal (внутри сборки) или public (для всех).
Компоновка (задача Лидера)
Лидер рабочей группы:

Пример точки входа для компоновки:
file static class PriceFormatter
{
public static string Format(int price) => $"{price} у.е.";
}

1. собирает проект: по одному типу на файл, пространства имён Marketplace.Domain ,
Marketplace.DataAccess , Marketplace.Presentation ;
2. пишет точку входа, где:
создаёт 3-4 товара, в том числе с недопустимыми данными (в try/catch ) —
чтобы показать защиту целостности;
добавляет их в Catalog , ищет, удаляет, выводит;
сохраняет и загружает товары через IProductRepository через интерфейс (DIP);
печатает отчёт через CatalogPrinter и CatalogReport ;
3. проверяет, что изменить коллекцию каталога снаружи нельзя;
4. оформляет отчёт.


## Структура проекта

| Файл | Пространство имён | Назначение |
|---|---|---|
| `Program.cs` | `Marketplace.Presentation` | точка входа: сценарии работы с профилем, успешной и неуспешной оплаты, проверка владельца корзины, работа каталога, сохранение/загрузка товаров и отчёты |
| `User.cs` | `Marketplace.Domain` | класс `User` — пользователь (ФИО, логин, пароль, уникальный ID) |
| `Contact.cs` | `Marketplace.Domain` | класс `Contact` — контакт пользователя (значение + ID канала) |
| `Channel.cs` | `Marketplace.Domain` | класс `Channel` — канал связи (телефон, email, telegram) |
| `UserProfile.cs` | `Marketplace.Domain` | класс `UserProfile` — профиль пользователя: контакты, денежный и бонусный балансы, персональная скидка, история покупок |
| `ProfileAdd.cs` | `Marketplace.Domain` | сервис `ProfileAdd` — создание профиля, контакты, вывод сводки и пополнение баланса |
| `Product.cs` | `Marketplace.Domain` | класс `Product` — товар (ID, название, цена за единицу) с проверкой входных данных |
| `Catalog.cs` | `Marketplace.Domain` | класс `Catalog` — каталог товаров: добавление, поиск, удаление и защита внутренней коллекции от изменения снаружи |
| `CartItem.cs` | `Marketplace.Domain` | класс `CartItem` — позиция корзины (ID, ID товара, количество, стоимость) |
| `ProductItem.cs` | `Marketplace.Domain` | класс `ProductItem` — сохранённая покупка (позиция и дата приобретения) |
| `Cart.cs` | `Marketplace.Domain` | класс `Cart` — корзина пользователя (ID, ID пользователя, список позиций `CartItem`) |
| `CartPrice.cs` | `Marketplace.Domain` | структура `CartPrice` — расчётная цена корзины (исходная цена, множитель скидки, размер скидки) |
| `BuyerBalance.cs` | `Marketplace.Domain` | класс `BuyerBalance` — баланс покупателя (бонусы, деньги, доля оплаты бонусами) |
| `BuyerBalanceState.cs` | `Marketplace.Domain` | структура `BuyerBalanceState` — снимок состояния баланса покупателя |
| `PaymentService.cs` | `Marketplace.Domain` | сервис `PaymentService` — расчёт оплаты, списание бонусов и денег, сообщения об оплате, проверка владельца корзины |
| `DataAccess/IProductRepository.cs` | `Marketplace.DataAccess` | интерфейс `IProductRepository` — абстракция хранения товаров с методами `Save` и `Load` |
| `DataAccess/FileProductRepository.cs` | `Marketplace.DataAccess` | `internal class FileProductRepository` — файловое хранилище товаров в формате `id;title;price` |
| `Presentation/CatalogPrinter.cs` | `Marketplace.Presentation` | класс `CatalogPrinter` — печать всех товаров и топа по цене; в этом же файле находится `file static class PriceFormatter` |
| `Presentation/CatalogReport.cs` | `Marketplace.Presentation` | класс `CatalogReport` — расчёты средней цены и сводки по каталогу без печати и работы с файлами |

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

### Вклад участников

Программа объединяет результаты работы всех участников:

- **Роман** — сущность `Product` с проверкой данных, а также бонусный баланс, персональная скидка, история покупок и пополнение баланса в профиле.
- **Александр** — корзина, товарные позиции, баланс покупателя и сервис оплаты, а также класс `Catalog` из отдельного вордовского файла.
- **Мария** — идентификаторы товара, позиции и корзины, структуры `CartPrice` и `BuyerBalanceState`, интерфейс `IProductRepository` и файловое хранилище `FileProductRepository`.
- **София** — успешный и неуспешный сценарии оплаты, общая компоновка проекта, `CatalogPrinter`, `CatalogReport`, сценарий каталога в `Program`, пространства имён и отчёт в README.

## Распределение работы

### Часть Ромы Макарова

Роман выполнил сущность `Product` и бонусно-платёжную подсистему профиля: добавил в `UserProfile` бонусный баланс, персональную скидку, лимит оплаты бонусами и историю покупок (`ProductItem`), а в `ProfileAdd` — пополнение баланса с начислением бонусов. В `Product` он использовал `private`-поля и `private`-методы проверки, а доступ к данным организовал через `public`-геттеры и методы, чтобы нельзя было записать некорректные значения в обход конструктора. В `UserProfile` закрытые поля соединены с `public`-свойствами, которые проверяют значения при записи, а коллекции контактов и покупок доступны только для чтения. В итоговой версии `Product` сохранены `public`-геттеры и проверки в конструкторе, а прямого доступа к полям нет.

### Часть Саши Лебедева

Александр подготовил модуль корзины и оплаты (`Cart`, `CartItem`, `BuyerBalance`, `PaymentService`), а также класс `Catalog` из отдельного вордовского файла. В `Cart` и `Catalog` внутренние коллекции объявлены как `private readonly`, доступ к ним идёт через `public`-методы, а `GetProducts()` возвращает `IReadOnlyList<Product>` — копию списка, поэтому снаружи нельзя обойти проверки `Add`/`Remove`. В `PaymentService` публичными оставлены только сценарии оплаты и расчёта, а вспомогательные методы сделаны `private static`. В `Catalog` также есть `private static`-методы нормализации и сравнения названий, чтобы поиск и защита от дублей оставались внутренней деталью реализации.

### Часть Маши Титовой

Мария добавила идентификаторы товару, товарной позиции и корзине, привязала корзину к пользователю через `Cart.UserId` и оформила структуры `CartPrice` и `BuyerBalanceState`. В этих структурах использованы `public`-свойства только для чтения и `private static`-методы валидации, поэтому объект нельзя привести к некорректному состоянию. Она также создала `public interface IProductRepository` и `internal class FileProductRepository`: интерфейс задаёт контракт хранения, а конкретная файловая реализация скрыта внутри сборки. В `FileProductRepository` путь хранится в `private readonly`-поле, а сериализация и десериализация вынесены в `private static`-методы.

### Часть Сони Поповой (лидер группы)

Я собрала общую версию проекта, реализовала успешный и неуспешный сценарии оплаты, перенесла расчёты в `PaymentService` и оформила отчёт в README. На этапе задания 3 она добавила `CatalogPrinter`, `CatalogReport`, сценарий каталога в `Program`, свела доменные типы в `Marketplace.Domain`, а работу с файлами — в `Marketplace.DataAccess`; точка входа `Program` сделана `internal`, так как не является частью публичного API. В `CatalogPrinter.cs` вспомогательный `PriceFormatter` объявлен как `file static class`, чтобы форматирование цены было доступно только в этом файле, а `CatalogReport` отвечает только за расчёты. Публичными оставлены только классы и методы, которые действительно нужны другим слоям, а детали реализации скрыты.

## Выводы

`internal` пригодился для `FileProductRepository`: снаружи сборки клиенты видят только `IProductRepository`, поэтому формат хранения и класс реализации можно заменить без изменения `Program` и других потребителей. Тот же модификатор выбран для `Program`, чтобы точка входа не входила в публичный API. Если сделать файловую реализацию `public`, она станет частью API, и переход, например, на базу данных потребует правок у клиентов.

`file` пригодился для `PriceFormatter`: это небольшая деталь форматирования, нужная только `CatalogPrinter`, поэтому `file static class` не засоряет пространство имён и не позволяет использовать себя из других файлов. Без такого ограничения пришлось бы либо дублировать форматирование, либо делать его публичным и раскрывать лишнюю деталь реализации.

Без инкапсуляции внешний код мог бы напрямую менять списки товаров, корзины и контактов, обходя проверки: удалять товары не через `Remove`, добавлять дубли, записывать отрицательные балансы или цены. Тогда нарушились бы инварианты классов, а `PaymentService` мог бы считать стоимость по уже испорченным данным. `private`-поля, `readonly`, `IReadOnlyList` и проверки в конструкторах и сеттерах сохраняют корректное состояние объектов, а `internal` и `file` не дают деталям реализации распространяться за пределы нужного файла или сборки.


## Запуск

```bash
dotnet run --project Rabota_OOP/Rabota_OOP.csproj
```

Требуется .NET SDK 10.0 или новее.
