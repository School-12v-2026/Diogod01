# EasyShop2

EasyShop2 е учебен онлайн магазин, създаден с **ASP.NET Core MVC** и **Entity Framework Core**, с база данни SQL Server. Проектът позволява управление на продукти, добавяне в количка и финализиране на поръчки. Има различни права за **администратор** и обикновени потребители.

---

## 📦 Функционалности

### За всички потребители:
- Разглеждане на налични продукти
- Търсене на продукти по име
- Добавяне на продукти в количката

### За администратор:
- Създаване на нови продукти
- Редактиране на съществуващи продукти
- Изтриване на продукти
- Управление на изображения на продуктите

---

## 🗂 Структура на проекта

EasyShop2/
├─ Controllers/
│ ├─ AccountController.cs # Управление на регистрация и логин
│ └─ ProductsController.cs # Управление на продуктите
├─ Models/
│ ├─ Product.cs # Модел за продукт
│ └─ User.cs # Модел за потребител
├─ Views/
│ ├─ Products/
│ │ ├─ Index.cshtml # Списък с продукти
│ │ ├─ Create.cshtml # Създаване на продукт
│ │ ├─ Edit.cshtml # Редактиране на продукт
│ └─ Shared/
│ └─ _Layout.cshtml # Общ layout
├─ wwwroot/
│ └─ images/ # Снимки на продуктите
├─ Program.cs # Основна конфигурация на приложението
└─ appsettings.json # Конфигурация на базата данни


---

## ⚙️ Инсталация и стартиране

1. Уверете се, че имате инсталирани:
   - [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
   - SQL Server или SQL Server Express

2. Клонирайте проекта:
```bash
git clone https://github.com/yourusername/EasyShop2.git
Отворете проекта в Visual Studio 2022 или VS Code.

Настройте връзката към базата данни в appsettings.json:

"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=EasyShop2Db;Trusted_Connection=True;"
}
Стартирайте приложението:

dotnet run
Отворете браузъра на:

https://localhost:7007