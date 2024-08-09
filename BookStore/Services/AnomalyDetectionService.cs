using BookStore.Model;

namespace BookStore.Services
{
    public class AnomalyDetectionService
    {
        public static bool IsFraudulentOrder(Order order)
        {
            // 使用简单的规则检测异常,金额大于10000认为是异常订单
            return order.Amount > 10000; 
        }
    }
}
