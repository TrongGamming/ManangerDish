# Hệ Thống Quản Lý Thực Đơn & Gọi Món Thời Gian Thực (ManangerDish)

![.NET](https://img.shields.io/badge/.NET-6.0-blue?style=for-the-badge&logo=dotnet)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-MVC-purple?style=for-the-badge&logo=dotnet)
![SQL Server](https://img.shields.io/badge/SQL%20Server-Database-red?style=for-the-badge&logo=microsoft-sql-server)
![SignalR](https://img.shields.io/badge/SignalR-Real--time-orange?style=for-the-badge)
![JWT](https://img.shields.io/badge/JWT-Authentication-black?style=for-the-badge)

ManangerDish là một giải pháp quản lý thực đơn và đặt món ăn tại bàn theo thời gian thực dành cho nhà hàng. Hệ thống giúp tối ưu hóa quy trình làm việc từ lúc khách hàng quét mã QR gọi món tại bàn, nhân viên bếp tiếp nhận chế biến cho đến khi nhân viên phục vụ giao món ăn.

---

<details>
<summary>🇬🇧 Click to view English Version</summary>

# Real-time Restaurant Order & Dish Management System (ManangerDish)

ManangerDish is a comprehensive real-time restaurant menu and order management solution. The system optimizes the workflow from the moment customers scan QR codes to order at the table, through kitchen preparation, to server delivery.

</details>

---

## 🚀 Các Tính Năng Nổi Bật (Key Features)

*   **Cập nhật thời gian thực (Real-time Hub)**: Sử dụng ASP.NET Core SignalR để đồng bộ trạng thái đơn hàng tức thời giữa khách hàng, nhà bếp và nhân viên phục vụ mà không cần tải lại trang.
*   **Bảo mật JWT & Tự động Làm mới (JWT & Auto-Refresh)**: Cơ chế xác thực JWT kết hợp Middleware `AutomaticRefreshToken` tự động cấp lại Token mới khi Token cũ hết hạn, mang lại trải nghiệm mượt mà và an toàn.
*   **Mô hình Repository & Unit of Work**: Đảm bảo cấu trúc code mạch lạc, quản lý Database Transaction tập trung và dễ dàng viết Unit Test.
*   **Lazy Loading Proxies**: Tối ưu hóa việc tải dữ liệu từ cơ sở dữ liệu SQL Server, chỉ tải các thực thể liên quan khi thực sự cần thiết.
*   **Mã QR động**: Tích hợp thư viện `QRCoder` giúp hệ thống tự động tạo mã QR độc bản cho từng bàn ăn để khách hàng tự quét chọn món.

---

<details>
<summary>🇬🇧 Click to view English Version</summary>

## 🚀 Key Features

*   **Real-time Synchronization**: Powered by ASP.NET Core SignalR to sync order states immediately between clients, kitchen, and servers without page reloads.
*   **JWT & Auto-Refresh Token**: Secure authentication leveraging JWT Bearer combined with a custom `AutomaticRefreshToken` Middleware to transparently renew expired tokens.
*   **Repository & Unit of Work**: Decoupled data access logic, clean transaction boundaries, and high testability.
*   **Lazy Loading Proxies**: Database querying optimization via Entity Framework Core proxies, loading navigation properties on demand.
*   **Dynamic QR Code**: Integrated with `QRCoder` to auto-generate unique QR codes for tables, allowing instant self-ordering.

</details>

---

## 📁 Cấu Trúc Thư Mục (Folder Structure)

```text
ManagerDish/
├── Areas/
│   ├── Guest/           # Giao diện và logic dành cho khách hàng tại bàn
│   └── Staff/           # Giao diện và logic quản lý dành cho nhân viên/admin
├── Data/                # EF Core DBContext và cấu hình database
├── Hubs/                # SignalR Hub (OrderHub) xử lý real-time
├── Middleware/          # Middleware xử lý tự động Refresh Token
├── Models/              # Các đối tượng thực thể (Dish, Table, Order...)
├── Repository/          # Triển khai Repository Pattern & Unit of Work
├── Services/            # Các dịch vụ xử lý logic (AuthService, v.v.)
└── Views/               # Giao diện Razor Views
```

---

<details>
<summary>🇬🇧 Click to view English Version</summary>

## 📁 Folder Structure

```text
ManagerDish/
├── Areas/
│   ├── Guest/           # UI & logic for customers at the table
│   └── Staff/           # UI & logic for staff/admin management
├── Data/                # EF Core DBContext and database configurations
├── Hubs/                # SignalR Hub (OrderHub) handling real-time events
├── Middleware/          # Middleware handling auto token refresh
├── Models/              # Database entities (Dish, Table, Order...)
├── Repository/          # Repository Pattern & Unit of Work implementation
├── Services/            # Business logic services (AuthService, etc.)
└── Views/               # Razor Views templates
```

</details>

---

## 🛠️ Công Nghệ Sử Dụng (Tech Stack)

*   **Backend**: .NET 6.0, ASP.NET Core Web MVC
*   **Database**: SQL Server, Entity Framework Core 6.0
*   **Bảo mật**: JWT (JSON Web Tokens), BCrypt.Net-Next
*   **Real-time**: ASP.NET Core SignalR
*   **Tiện ích**: QRCoder (Tạo mã QR)

---

<details>
<summary>🇬🇧 Click to view English Version</summary>

## 🛠️ Tech Stack

*   **Backend**: .NET 6.0, ASP.NET Core Web MVC
*   **Database**: SQL Server, Entity Framework Core 6.0
*   **Security**: JWT (JSON Web Tokens), BCrypt.Net-Next
*   **Real-time**: ASP.NET Core SignalR
*   **Utility**: QRCoder (QR Code generator)

</details>

---

## 💻 Hướng Dẫn Cài Đặt (Installation)

1.  **Cài đặt các công cụ cần thiết**:
    *   Cài đặt .NET SDK 6.0 trở lên.
    *   Cài đặt SQL Server Local hoặc Server từ xa.
2.  **Cấu hình chuỗi kết nối**:
    Mở file `appsettings.json` trong dự án `ManagerDish` và cập nhật cấu hình database:
    ```json
    "ConnectionStrings": {
      "DatabaseSettings": "Server=YOUR_SERVER;Database=ManagerDishDB;Trusted_Connection=True;MultipleActiveResultSets=true"
    }
    ```
3.  **Khởi tạo Database (Migrations)**:
    Mở terminal tại thư mục dự án và chạy lệnh:
    ```bash
    dotnet ef database update
    ```
4.  **Chạy dự án**:
    ```bash
    dotnet run
    ```
    Ứng dụng sẽ khả dụng tại: `https://localhost:7142` hoặc `http://localhost:5142`.

---

<details>
<summary>🇬🇧 Click to view English Version</summary>

## 💻 Installation

1.  **Prerequisites**:
    *   Install .NET SDK 6.0 or higher.
    *   Install SQL Server database engine.
2.  **Configuration**:
    Update the database connection string in `appsettings.json`:
    ```json
    "ConnectionStrings": {
      "DatabaseSettings": "Server=YOUR_SERVER;Database=ManagerDishDB;Trusted_Connection=True;MultipleActiveResultSets=true"
    }
    ```
3.  **Database Migration**:
    Run migrations to set up database schema:
    ```bash
    dotnet ef database update
    ```
4.  **Run Application**:
    ```bash
    dotnet run
    ```
    Access the app at: `https://localhost:7142` or `http://localhost:5142`.

</details>
