using System;
using System.Threading.Tasks;

namespace MassTransit.Order.Contracts.Order
{
    public interface IInsertOrder
    {
        /// <summary>
        /// 订单id
        /// </summary>
        Guid OrderId { get; }

        /// <summary>
        /// 总价
        /// </summary>
        double TotalPrice { get; }

        /// <summary>
        /// 创建时间
        /// </summary>
        DateTime CreateTime { get; }
    }
}