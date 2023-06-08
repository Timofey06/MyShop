using System;
namespace MyShop_Utility
{
    public static class PathManager
    {
        public const string ImageProductPass = @"images/product/";

        public const string SessionCard = "SessionCart";
        public const string SessionQuery = "SessionQuery";
        public const string AdminRole = "Admin";
        public const string CustomerRole = "Customer";
        public const string Email = "tima.loktionov6@gmail.com";
        public const string NameCategory = "Category";
        public const string NameMyModel = "MyModel";
        public const string Success = "Success";
        public const string Error = "Error";


        public const string StatusPending = "Pending"; //ожидание
        public const string StatusAccepted = "Accepted"; //утверждён
        public const string StatusDenied = "Denied"; //отменён
        public const string StatusInProcessed = "InProcess";
        public const string StatusOrderDone = "OrderDone";
        public static IEnumerable<string> StatusList=new List<string>()
        {
            StatusAccepted,StatusDenied,StatusPending,StatusInProcessed,StatusOrderDone
        };

    }
}
