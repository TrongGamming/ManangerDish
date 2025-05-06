namespace ManagerDish.Models.Enum
{
    public enum RoleEnum
    {
        Admin = 1,
        Manager = 2,
        Kitchen = 3,
        Staff = 4,
        Guest = 5
    }

    public enum PaymentMethod
    {
        Cash = 1,
        CreditCard = 2,
        MobilePayment = 3
    }

    public enum OrderStatus
    {
        Pending = 1,
        Processing = 2,
        Completed = 3,
        Cancelled = 4
    }
    
    public enum TableStatus
    {
        Available = 1,
        Occupied = 2,
        Reserved = 3
    }

    public enum DishStatus
    {
        Available = 1,
        Unavailable = 2,
        OutOfStock = 3
    }
}
